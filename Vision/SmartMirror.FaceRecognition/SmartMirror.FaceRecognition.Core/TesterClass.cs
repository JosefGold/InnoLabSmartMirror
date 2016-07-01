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
    public class TesterClass
    {
        private static Capture _capture;

        private static CascadeClassifier _faceClassifier;
        private static CascadeClassifier _smileClassifier;


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


      

    }
}
