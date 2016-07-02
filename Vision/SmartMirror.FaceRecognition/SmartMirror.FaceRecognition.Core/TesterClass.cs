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
        private static FaceDetector faceeDetector;

         static TesterClass()
        {
            faceeDetector = new FaceDetector();
        }

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
            var faces = faceeDetector.FindFacesInImage(myImage, true);

            foreach (var faceResult in faces)
            {
                myImage.Draw(faceResult.Face, new Bgr(Color.BurlyWood), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them

                if (faceResult.Mouth.HasValue)
                {
                    myImage.Draw(faceResult.Mouth.Value, new Bgr(Color.Green), 2);
                }

                if (faceResult.RightEye.HasValue)
                {
                    myImage.Draw(faceResult.RightEye.Value, new Bgr(Color.Red), 2);
                }

                if (faceResult.LeftEye.HasValue)
                {
                    myImage.Draw(faceResult.LeftEye.Value, new Bgr(Color.Green), 2);
                }

                if (faceResult.Nose.HasValue)
                {
                    myImage.Draw(faceResult.Nose.Value, new Bgr(Color.Green), 2);
                }

            }

            return myImage;
        }


     

    }
}
