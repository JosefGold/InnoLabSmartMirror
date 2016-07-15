using Emgu.CV;
using Emgu.CV.Structure;
using SmartMirror.FaceRecognition.Core.DataModel;
using SmartMirror.FaceRecognition.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartMirror.FaceRecognition.Core
{
    //http://ahmedopeyemi.com/main/face-detection-and-recognition-in-c-using-emgucv-3-0-opencv-wrapper-part-1/
    //http://www.codeproject.com/Articles/239849/Multiple-face-detection-and-recognition-in-real
    //http://www.codeproject.com/Articles/261550/EMGU-Multiple-Face-Recognition-using-PCA-and-Paral

    public class FaceDetector : IDisposable
    {
        private LazyDisposable<CascadeClassifier> _faceClassifier;
        private LazyDisposable<CascadeClassifier> _mouthClassifier;
        private LazyDisposable<CascadeClassifier> _noseClassifier;
        private LazyDisposable<CascadeClassifier> _eyesClassifier;


        public FaceDetector()
        {
            _faceClassifier = new LazyDisposable<CascadeClassifier>(() => BuildCascadeClassifierFromFile("haarcascade_frontalface_default.xml"));
            _noseClassifier = new LazyDisposable<CascadeClassifier>(() => BuildCascadeClassifierFromFile("haarcascade_mcs_nose.xml"));
            _eyesClassifier = new LazyDisposable<CascadeClassifier>(() => BuildCascadeClassifierFromFile("haarcascade_eye.xml"));
            _mouthClassifier = new LazyDisposable<CascadeClassifier>(() => BuildCascadeClassifierFromFile("haarcascade_smile.xml"));
        }


        public FaceDetectionResult FindMostDistinctFaceInImage(Image<Bgr, Byte> myImage, bool detectFacialFeatures = false)
        {
            return FindFacesInImage(myImage, FaceDetectionDiscriminators.GetMostDistinctiveFaceInFrame, detectFacialFeatures).FirstOrDefault();
        }

        public List<FaceDetectionResult> FindFacesInImage(Image<Bgr, Byte> myImage, bool detectFacialFeatures = false)
        {
            return FindFacesInImage(myImage, (faces, image) => faces, detectFacialFeatures);
        }

        public List<FaceDetectionResult> FindFacesInImage(Image<Bgr, Byte> myImage, Func<IEnumerable<FaceDetectionResult>, Image<Bgr, Byte>, IEnumerable<FaceDetectionResult>> filter, bool detectFacialFeatures = false)
        {
            using (var grayframe = myImage.Convert<Gray, byte>())
            {
                var faces = _faceClassifier.Value.DetectMultiScale(grayframe, 1.1, 10, Size.Empty, myImage.Size); //the actual face detection happens here

                var detectedFaces = filter(faces.Select(f => new FaceDetectionResult(f, myImage.Copy(f))), myImage)
                                          .ToList();

                if (detectFacialFeatures)
                {
                    foreach (var detectedFace in detectedFaces)
                    {
                        DetectAndAddFacialFeatures(grayframe, detectedFace);
                    }
                }

                return detectedFaces;
            }
        }

        private void DetectAndAddFacialFeatures(Image<Gray, byte> myImage, FaceDetectionResult faceResult)
        {

            Rectangle face = faceResult.Face;


            Rectangle leftEye = Rectangle.Empty;
            Rectangle rightEye = Rectangle.Empty;
            Rectangle nose = Rectangle.Empty;
            Rectangle mouth = Rectangle.Empty;

            using (var grayFace = myImage.Copy(face))
            {
                // Detect Nose
                var noses = _noseClassifier.Value.DetectMultiScale(grayFace, 1.1, 3, Size.Empty, new Size(face.Width / 2, face.Height / 2));

                // Nose is the base for all other features recognition
                if (noses.Any())
                {
                    nose = noses.OrderByDescending(n => n.Y).First();
                    nose.Offset(face.X, face.Y);

                    // Detect Eyes
                    var eyes = _eyesClassifier.Value.DetectMultiScale(grayFace, 1.1, 3, Size.Empty, new Size(face.Width / 3, face.Height / 3));

                    // Detect Mouths
                    var mouths = _mouthClassifier.Value.DetectMultiScale(grayFace, 1.1, 3, Size.Empty, new Size(face.Width, face.Height / 2));

                    var eyesFilteres = eyes.Where(e => e.Y < nose.Y).OrderBy(e => e.Y);
                    var mouthsFilteres = mouths.Where(e => e.Y > (nose.Y + (nose.Height / 2)));

                    if (eyesFilteres.Any())
                    {
                        var twoEyes = eyesFilteres.Take(2).OrderBy(e => e.X).ToList();


                        leftEye = twoEyes[0];
                        rightEye = twoEyes.Count > 1 ? twoEyes[1] : Rectangle.Empty;

                        leftEye.Offset(face.X, face.Y);
                        rightEye.Offset(face.X, face.Y);
                    }

                    if (mouthsFilteres.Any())
                    {
                        mouth = mouthsFilteres.First();
                        mouth.Offset(face.X, face.Y);
                    }
                }
            }

            faceResult.Nose = nose != Rectangle.Empty ? (Rectangle?)nose : null;
            faceResult.RightEye = rightEye != Rectangle.Empty ? (Rectangle?)rightEye : null;
            faceResult.LeftEye = leftEye != Rectangle.Empty ? (Rectangle?)leftEye : null;
            faceResult.Mouth = mouth != Rectangle.Empty ? (Rectangle?)mouth : null;

        }

        private CascadeClassifier BuildCascadeClassifierFromFile(string haarCascadeXmlFile)
        {
            //TODO Application.StartupPath should be replaced
            return new CascadeClassifier(Application.StartupPath + @"\Data\haarcascades\" + haarCascadeXmlFile);
        }


        public void Dispose()
        {
            if(_faceClassifier != null )
            {
                _faceClassifier.Dispose();
            }

            if (_eyesClassifier != null)
            {
                _eyesClassifier.Dispose();
            }

            if (_noseClassifier != null)
            {
                _noseClassifier.Dispose();
            }

            if (_mouthClassifier != null)
            {
                _mouthClassifier.Dispose();
            }
        }
    }
}
