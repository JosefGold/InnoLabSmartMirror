using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using SmartMirror.FaceRecognition.Core.DataModel;

namespace SmartMirror.FaceRecognition.Core
{
    public class FaceRecognizer : IDisposable
    {
        public const double EigenFaceRecognizerThreshold = 10;

        private object _syncRoot = new object();
        private bool _isTrained = false;
        private FaceDetector _faceDetector;
        private Emgu.CV.Face.FaceRecognizer _faceRecognizer;


        public FaceRecognizer()
        {
            _faceDetector = new FaceDetector();
            _faceRecognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);
        }

        public FaceRecognizer(string trainedModelFilePath)
            : this()
        {
            Load(trainedModelFilePath);
        }


        public void Save(string filePath)
        {
            lock (_syncRoot)
            {
                _faceRecognizer.Save(filePath);
            }
        }

        public void Load(string trainedModelFilePath)
        {
            lock (_syncRoot)
            {
                _faceRecognizer.Load(trainedModelFilePath);
                _isTrained = true;
            }
        }


        public void Train(List<FaceRecognitionRecord> trainData)
        {
            var faceImages = trainData.Select(face => face.FaceImage).ToArray();
            var faceIdentities = trainData.Select(face => face.FaceId).ToArray();
            
            lock (_syncRoot)
            {
                _faceRecognizer.Train(faceImages, faceIdentities);
                _isTrained = true;
            }
        }

        public int? ResolveMostDistinctFaceInImage(Image<Bgr, Byte> image)
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

        public int ResolveFaceImage(Image<Bgr, Byte> faceImage)
        {

           var recognitionResults = _faceRecognizer.Predict(faceImage);

           if (recognitionResults.Distance < EigenFaceRecognizerThreshold)
            {
                
            }
        }

        public void Dispose()
        {
            if (_faceRecognizer != null)
            {
                _faceRecognizer.Dispose();
            }
        }
    }
}
