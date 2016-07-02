using Emgu.CV;
using Emgu.CV.Structure;
using SmartMirror.FaceRecognition.Core.DataModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartMirror.FaceRecognition.Core
{
    public class FaceDetector
    {
        private Lazy<CascadeClassifier> _faceClassifier;
        private Lazy<CascadeClassifier> _smileClassifier;
        private Lazy<CascadeClassifier> _noseClassifier;
        private Lazy<CascadeClassifier> _eyesClassifier;


        public FaceDetector()
        {
            _faceClassifier = new Lazy<CascadeClassifier>(() => BuildCascadeClassifierFromFile("haarcascade_frontalface_default.xml"));
            _noseClassifier = new Lazy<CascadeClassifier>(() => BuildCascadeClassifierFromFile("haarcascade_mcs_nose.xml"));
            _eyesClassifier = new Lazy<CascadeClassifier>(() => BuildCascadeClassifierFromFile("haarcascade_eye.xml"));
            _smileClassifier = new Lazy<CascadeClassifier>(() => BuildCascadeClassifierFromFile("haarcascade_smile.xml"));
        }


        public List<FaceDetectionResult> FindFacesInImage(Image<Bgr, Byte> myImage, bool detectFacialFeatures = false)
        {
            using (var grayframe = myImage.Convert<Gray, byte>())
            {
                var faces = _faceClassifier.Value.DetectMultiScale(grayframe, 1.1, 10, Size.Empty, myImage.Size); //the actual face detection happens here

                var detectedFaces = faces.Select(f => new FaceDetectionResult(f)).ToList();

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
                    var mouths = _smileClassifier.Value.DetectMultiScale(grayFace, 1.1, 3, Size.Empty, new Size(face.Width, face.Height / 2));

                    var eyesFilteres = eyes.Where(e => e.Y < nose.Y).OrderBy(e => e.Y);
                    var mouthsFilteres = mouths.Where(e => e.Y > (nose.Y + (nose.Height / 2)));

                    if (eyesFilteres.Any())
                    {
                        var twoEyes = eyesFilteres.Take(2).OrderBy(e => e.X).ToList();

                        leftEye = twoEyes[0];
                        rightEye = twoEyes[1];

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

            faceResult.Nose = nose != Rectangle.Empty ? (Rectangle?)nose : null ;
            faceResult.RightEye = rightEye != Rectangle.Empty ? (Rectangle?)rightEye : null;
            faceResult.LeftEye = leftEye != Rectangle.Empty ? (Rectangle?)leftEye : null;
            faceResult.Mouth = mouth != Rectangle.Empty ? (Rectangle?)mouth : null;

        }

        private CascadeClassifier BuildCascadeClassifierFromFile(string haarCascadeXmlFile)
        {
            //TODO Application.StartupPath should be replaced
            return new CascadeClassifier(Application.StartupPath + @"\Data\haarcascades\" + haarCascadeXmlFile);
        }

    }
}
