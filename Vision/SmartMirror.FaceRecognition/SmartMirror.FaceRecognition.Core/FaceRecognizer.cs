using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using SmartMirror.FaceRecognition.Core.DataModel;
using System.IO;
using System.Xml.Serialization;

namespace SmartMirror.FaceRecognition.Core
{
    //http://stackoverflow.com/questions/9457827/how-to-get-confidence-value-in-face-recognition-using-emgu-cv/9466619#9466619
    public class FaceRecognizer : IDisposable
    {
        public const double EigenFaceRecognizerThreshold = 2000;

        private object _syncRoot = new object();
        private bool _isTrained = false;
        private FaceDetector _faceDetector;
        private Emgu.CV.Face.FaceRecognizer _eigenFaceRecognizer;
        private Emgu.CV.Face.FaceRecognizer _fisherFaceRecognizer;
        private Emgu.CV.Face.FaceRecognizer _lbphFaceRecognizer;

        private Dictionary<int, PersonInfo> _personDB;

        private readonly PersonInfo DEFAULT_UNKOWN = new PersonInfo() { Id = -999, Name = "UNKOWN" };

        public FaceRecognizer()
        {
            _faceDetector = new FaceDetector();
            _eigenFaceRecognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);
            _fisherFaceRecognizer = new FisherFaceRecognizer(0, 3500);//4000
            _lbphFaceRecognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 100);//50
        }

        public FaceRecognizer(string trainedModelFilePath)
            : this()
        {
            Load(trainedModelFilePath);
        }


        public void Save(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            lock (_syncRoot)
            {
                SavePersonDB(folderPath);
                _eigenFaceRecognizer.Save(Path.Combine(folderPath, "EigenFaceRecTrainedModel.mdl"));
                _fisherFaceRecognizer.Save(Path.Combine(folderPath, "FisherFaceRecTrainedModel.mdl"));
                _lbphFaceRecognizer.Save(Path.Combine(folderPath, "LBPHFaceRecTrainedModel.mdl"));
            }
        }

        public void Load(string trainedModelsFolderPath)
        {
            lock (_syncRoot)
            {
                LoadPersonDB(trainedModelsFolderPath);
                _eigenFaceRecognizer.Load(Path.Combine(trainedModelsFolderPath, "EigenFaceRecTrainedModel.mdl"));
                _fisherFaceRecognizer.Load(Path.Combine(trainedModelsFolderPath, "FisherFaceRecTrainedModel.mdl"));
                _lbphFaceRecognizer.Load(Path.Combine(trainedModelsFolderPath, "LBPHFaceRecTrainedModel.mdl"));

                _isTrained = true;
            }
        }

        public void Train(List<FaceRecognitionPersonTrainData> trainData)
        {
            List<int> faceIdentities = new List<int>();
            List<Image<Gray, byte>> faceImages = new List<Image<Gray, byte>>();

            foreach (var personTrain in trainData)
            {
                foreach (var personImage in personTrain.FacialImages)
                {
                    faceIdentities.Add(personTrain.PersonInfo.Id);
                    faceImages.Add(PreProcessImageForRecognition(personImage));
                }
            }

            lock (_syncRoot)
            {
                _personDB = trainData.Select(t => t.PersonInfo).ToDictionary(p => p.Id, p => p);
                _eigenFaceRecognizer.Train(faceImages.ToArray(), faceIdentities.ToArray());
                _fisherFaceRecognizer.Train(faceImages.ToArray(), faceIdentities.ToArray());
                _lbphFaceRecognizer.Train(faceImages.ToArray(), faceIdentities.ToArray());
                _isTrained = true;
            }
        }

        public FaceRecognitionResult ResolveMostDistinctFaceInImage(Image<Bgr, Byte> image)
        {
            var foundFace = _faceDetector.FindMostDistinctFaceInImage(image);

            if (foundFace != null)
            {
                return ResolveFaceImage(foundFace.FaceImage);
            }
            else
            {
                return null;
            }

        }

        public FaceRecognitionResult ResolveFaceImage(Image<Bgr, Byte> faceImage)
        {
            if (!_isTrained)
            {
                throw new ApplicationException("Face Recognizer was not trained or loaded");
            }


            Image<Gray, Byte> proccessedFaceImage = PreProcessImageForRecognition(faceImage);


            FaceRecognitionResult result;

            result = RecognizeWithEigenFace(proccessedFaceImage);

            return result;
        }

        private FaceRecognitionResult RecognizeWithEigenFace(Image<Gray, Byte> faceImage)
        {
            FaceRecognitionResult result;
            var recognitionResults = _eigenFaceRecognizer.Predict(faceImage);

            if (recognitionResults.Distance < EigenFaceRecognizerThreshold)
            {
                result = new FaceRecognitionResult()
                {
                    RecognizedPerson = _personDB[recognitionResults.Label],
                    ConfidenceLevel = ((EigenFaceRecognizerThreshold - recognitionResults.Distance) / EigenFaceRecognizerThreshold) * 100
                };
            }
            else
            {
                result = new FaceRecognitionResult()
                {
                    RecognizedPerson = DEFAULT_UNKOWN,
                    ConfidenceLevel = 100
                };
            }
            return result;
        }


        private Image<Gray, byte> PreProcessImageForRecognition(Image<Bgr, byte> faceImage)
        {
            // TODO Add resize and histogram fixes


            // Trick 1: Crop Face
            /*                   
            facesDetected[i].X += (int)(facesDetected[i].Height * 0.15);
            facesDetected[i].Y += (int)(facesDetected[i].Width * 0.22);
            facesDetected[i].Height -= (int)(facesDetected[i].Height * 0.3);
            facesDetected[i].Width -= (int)(facesDetected[i].Width * 0.35);
            */

            // Trick 2: Resize 100x100
            /*
             result = currentFrame.Copy(facesDetected[i]).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            */

            // Trick 2: Equalize Histogram 100x100
            /*
                result._EqualizeHist();
            */



            return faceImage.Convert<Gray, byte>();
        }

        private void SavePersonDB(string folderPath)
        {
            string saveFilePath = Path.Combine(folderPath, "FaceRecPersonDB.xml");

            using (FileStream fs = new FileStream(saveFilePath, FileMode.Create))
            {
                XmlSerializer xSer = new XmlSerializer(typeof(Dictionary<int, PersonInfo>));

                xSer.Serialize(fs, _personDB);
            }
        }

        private void LoadPersonDB(string trainedModelsFolderPath)
        {
            string saveFilePath = Path.Combine(trainedModelsFolderPath, "FaceRecPersonDB.xml");

            using (FileStream fs = new FileStream(saveFilePath, FileMode.Open)) //double check that...
            {
                XmlSerializer _xSer = new XmlSerializer(typeof(Dictionary<int, PersonInfo>));

                var loadedDB = _xSer.Deserialize(fs) as Dictionary<int, PersonInfo>;

                if (loadedDB != null)
                {
                    _personDB = loadedDB;
                }
                else
                {
                    throw new ApplicationException("Person DB in train model invalid");
                }
            }
        }


        public void Dispose()
        {
            if (_eigenFaceRecognizer != null)
            {
                _eigenFaceRecognizer.Dispose();
            }
        }
    }
}
