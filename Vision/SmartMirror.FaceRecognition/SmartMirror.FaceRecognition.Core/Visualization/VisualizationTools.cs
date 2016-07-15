using Emgu.CV;
using Emgu.CV.Structure;
using SmartMirror.FaceRecognition.Core.DataModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.FaceRecognition.Core.Visualization
{
    public static class VisualizationTools
    {


        public static Image<Bgr, byte> DrawFacesAsOverlay(Image<Bgr, byte> frameImage, Dictionary<string, FaceDetectionResult> labeledFaces, bool drawFaceRecognitionResizedVersion = false)
        {
            var frameImageCopy = frameImage.Copy();

            int nFaceCounter = 0;
            foreach (var labaledFace in labeledFaces.OrderBy(f => f.Value.Face.X))
            {
                var faceResult = labaledFace.Value;
                var label = labaledFace.Key;

                frameImageCopy.Draw(faceResult.Face, new Bgr(Color.BurlyWood), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them

                Rectangle faceROI = new Rectangle(new Point(0, 0), faceResult.FaceImage.Size);
                faceROI.X += (int)(faceROI.Height * 0.15);
                faceROI.Y += (int)(faceROI.Width * 0.22);
                faceROI.Height -= (int)(faceROI.Height * 0.3);
                faceROI.Width -= (int)(faceROI.Width * 0.3);


                using (var resized = faceResult.FaceImage.Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic))
                {
                    frameImageCopy.ROI = new Rectangle(new Point(100 * nFaceCounter, 0), resized.Size); // a rectangle
                    resized.CopyTo(frameImageCopy);
                    frameImageCopy.ROI = Rectangle.Empty;
                }

                if (drawFaceRecognitionResizedVersion)
                {
                    var croppedFace = faceResult.FaceImage.Copy(faceROI).Convert<Gray, byte>().Convert<Bgr, byte>();
                    croppedFace._EqualizeHist();

                    using (var resized = croppedFace.Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic))
                    {
                        frameImageCopy.ROI = new Rectangle(new Point(100 * nFaceCounter, 100), resized.Size); // a rectangle
                        resized.CopyTo(frameImageCopy);
                        frameImageCopy.ROI = Rectangle.Empty;
                    }
                }


                frameImageCopy.Draw(label, new Point(100 * nFaceCounter, 110), Emgu.CV.CvEnum.FontFace.HersheyComplex, 0.60, new Bgr(Color.Red));


                if (faceResult.Mouth.HasValue)
                {
                    frameImageCopy.Draw(faceResult.Mouth.Value, new Bgr(Color.Green), 2);
                }

                if (faceResult.RightEye.HasValue)
                {
                    frameImageCopy.Draw(faceResult.RightEye.Value, new Bgr(Color.Red), 2);
                }

                if (faceResult.LeftEye.HasValue)
                {
                    frameImageCopy.Draw(faceResult.LeftEye.Value, new Bgr(Color.Green), 2);
                }

                if (faceResult.Nose.HasValue)
                {
                    frameImageCopy.Draw(faceResult.Nose.Value, new Bgr(Color.Green), 2);
                }

                nFaceCounter++;
            }

            return frameImageCopy;
        }
    }
}
