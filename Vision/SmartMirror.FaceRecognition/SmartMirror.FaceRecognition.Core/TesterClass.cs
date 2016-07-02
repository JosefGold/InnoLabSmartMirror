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


        public static Image<Bgr, Byte> TrainSnap(string who)
        {
            if (_capture == null)
            {
                _capture = new Capture();
            }

            using (var frame = _capture.QueryFrame())
            {
                using (var frameImage = frame.ToImage<Bgr, Byte>())
                {
                    return Train(frameImage, who);
                }
            }
        }


        private static Dictionary<string, List<Image<Bgr, Byte>>> trainingFaces = new Dictionary<string, List<Image<Bgr, byte>>>();

        public static Image<Bgr, Byte> Train(Image<Bgr, Byte> myImage, string who)
        {
            var faces = faceeDetector.FindFacesInImage(myImage);

            if (faces.Any())
            {
                var trainFace = faces.First();

                if (!trainingFaces.ContainsKey(who))
                {
                    trainingFaces.Add(who, new List<Image<Bgr, byte>>());
                }

                trainingFaces[who].Add(trainFace.FaceImage);

                return trainFace.FaceImage;
            }
            else
            {
                return new Image<Bgr, byte>(@"C:\Users\Yossi\Pictures\WebComix\New Thumbs\brevity_thumb.png");
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

        private static Image<Bgr, Byte> FindFacesInImageAndMark(Image<Bgr, Byte> myImage, bool faceFeatures = false)
        {
            var faces = faceeDetector.FindFacesInImage(myImage, faceFeatures);

            int nFaceCounter = 0;
            foreach (var faceResult in faces.OrderBy(f => f.Face.X))
            {
                myImage.Draw(faceResult.Face, new Bgr(Color.BurlyWood), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them

                using (var resized = faceResult.FaceImage.Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic))
                {
                    myImage.ROI = new Rectangle(new Point(100 * nFaceCounter, 0), resized.Size); // a rectangle
                    resized.CopyTo(myImage);
                    myImage.ROI = Rectangle.Empty;
                }

                myImage.Draw("Unkown", new Point(100 * nFaceCounter, 110), Emgu.CV.CvEnum.FontFace.HersheyComplex, 0.60, new Bgr(Color.Red));


                if (faceFeatures)
                {
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


                nFaceCounter++;
            }

            return myImage;
        }




    }
}
