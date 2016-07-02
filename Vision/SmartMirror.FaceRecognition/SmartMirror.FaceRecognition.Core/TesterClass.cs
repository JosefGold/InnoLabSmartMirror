using Emgu.CV;
using Emgu.CV.Structure;
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
    public class TesterClass
    {
        private static Capture _capture;

        private static CascadeClassifier _faceClassifier;
        private static CascadeClassifier _smileClassifier;
        private static CascadeClassifier _noseClassifier;
        private static CascadeClassifier _eyesClassifier;


        public static Bitmap ReadImage(string fileapath)
        {
            Image<Bgr, Byte> myImage = new Image<Bgr, byte>(fileapath);


            myImage = FindFacesInImageAndMark(myImage);


            return myImage.ToBitmap();
        }


        public static Image<Bgr, Byte> Snap()
        {
            if (_capture == null)
            {
                _capture = new Capture();
            }

            using (var frame = _capture.QueryFrame())
            {
                return FindFacesInImageAndMark(frame);
            }
        }


        public static Image<Bgr, Byte> FindFacesInImageAndMark(Mat frame)
        {
            var imageFrame = frame.ToImage<Bgr, Byte>();

            if (imageFrame != null)
            {
                imageFrame = FindFacesInImageAndMark(imageFrame);
            }

            return imageFrame;

        }

        private static Image<Bgr, Byte> FindFacesInImageAndMark(Image<Bgr, Byte> myImage)
        {
            var faces = FindFacesInImage(myImage);

            foreach (var face in faces)
            {
                myImage.Draw(face, new Bgr(Color.BurlyWood), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them

                var features = FindFacialFeatures(myImage, face);

                foreach (var feature in features)
                {
                    myImage.Draw(feature, new Bgr(Color.Green), 2);
                }
            }

            return myImage;
        }

        private static Rectangle[] FindFacesInImage(Image<Bgr, Byte> myImage)
        {
            if (_faceClassifier == null)
            {
                _faceClassifier = new CascadeClassifier(Application.StartupPath + @"\Data\haarcascades\haarcascade_frontalface_default.xml");
            }


            using (var grayframe = myImage.Convert<Gray, byte>())
            {

                var faces = _faceClassifier.DetectMultiScale(grayframe, 1.1, 10, Size.Empty, myImage.Size); //the actual face detection happens here

                return faces;
            }
        }


        private static Rectangle[] FindFacialFeatures(Image<Bgr, Byte> myImage, Rectangle face)
        {
            List<Rectangle> features = new List<Rectangle>();
            if (_noseClassifier == null)
            {
                _noseClassifier = new CascadeClassifier(Application.StartupPath + @"\Data\haarcascades\haarcascade_mcs_nose.xml");
                _eyesClassifier = new CascadeClassifier(Application.StartupPath + @"\Data\haarcascades\haarcascade_eye.xml");
                _smileClassifier = new CascadeClassifier(Application.StartupPath + @"\Data\haarcascades\haarcascade_smile.xml");
            }

            using (var grayFace = myImage.Copy(face).Convert<Gray, byte>())
            {
                var noses = _noseClassifier.DetectMultiScale(grayFace, 1.1, 3, Size.Empty, new Size(face.Width / 2, face.Height / 2)); //the actual face detection happens here

                if (noses.Any())
                {
                    var nose = noses.OrderByDescending(n => n.Y).First();
                    nose.Offset(face.X, face.Y);

                    features.Add(nose);

                    var eyes = _eyesClassifier.DetectMultiScale(grayFace, 1.1, 3, Size.Empty, new Size(face.Width / 3, face.Height / 3)); //the actual face detection happens here
                    var mouths = _smileClassifier.DetectMultiScale(grayFace, 1.1, 3, Size.Empty, new Size(face.Width, face.Height / 2)); //the actual face detection happens here

                    var eyesFilteres = eyes.Where(e => e.Y < nose.Y).OrderBy(e => e.Y);
                    var mouthsFilteres = mouths.Where(e => e.Y > (nose.Y + (nose.Height / 2)));

                    if (eyesFilteres.Any())
                    {

                        foreach (var eye in eyesFilteres.Take(2))
                        {
                            eye.Offset(face.X, face.Y);
                            features.Add(eye);
                        }
                    }

                    if (mouthsFilteres.Any())
                    {
                        var mouth = mouthsFilteres.First();
                        mouth.Offset(face.X, face.Y);
                        features.Add(mouth);
                    }



                }
            }

            return features.ToArray();
        }

    }
}
