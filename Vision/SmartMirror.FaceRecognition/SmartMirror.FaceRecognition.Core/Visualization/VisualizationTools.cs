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


                DrawFacialFeatures(frameImageCopy, faceResult);

                nFaceCounter++;
            }

            return frameImageCopy;
        }

        private static void DrawFacialFeatures(Image<Bgr, byte> frameImageCopy, FaceDetectionResult faceResult)
        {
            Cross2DF? oneEdgeRightEye = null;
            Cross2DF? secondEdgeRightEye = null;
            Cross2DF? upEdgeRightEye = null;
            Cross2DF? downEdgeRightEye = null;
            Cross2DF? oneEdgeLeftEye = null;
            Cross2DF? secondEdgeLeftEye = null;
            Cross2DF? upEdgeLeftEye = null;
            Cross2DF? downEdgeLeftEye = null;
            Cross2DF? noseCenter = null;
            Cross2DF? oneEdgeMouth = null;
            Cross2DF? secondEdgeMouth=null;

            if (faceResult.RightEye.HasValue)
            {
                 oneEdgeRightEye = new Cross2DF(new PointF(faceResult.RightEye.Value.X, faceResult.RightEye.Value.Y + faceResult.RightEye.Value.Height / 2), 3, 3);
                 secondEdgeRightEye = new Cross2DF(new PointF(faceResult.RightEye.Value.X + faceResult.RightEye.Value.Width, faceResult.RightEye.Value.Y + faceResult.RightEye.Value.Height / 2), 3, 3);

                 upEdgeRightEye = new Cross2DF(new PointF(faceResult.RightEye.Value.X + faceResult.RightEye.Value.Width / 2, faceResult.RightEye.Value.Y + faceResult.RightEye.Value.Height/4), 3, 3);
                 downEdgeRightEye = new Cross2DF(new PointF(faceResult.RightEye.Value.X + faceResult.RightEye.Value.Width / 2, faceResult.RightEye.Value.Y + faceResult.RightEye.Value.Height - faceResult.RightEye.Value.Height / 3), 3, 3);


                frameImageCopy.Draw(oneEdgeRightEye.Value, new Bgr(Color.Red), 2);
                frameImageCopy.Draw(secondEdgeRightEye.Value, new Bgr(Color.Red), 2);
                frameImageCopy.Draw(upEdgeRightEye.Value, new Bgr(Color.Red), 2);
                frameImageCopy.Draw(downEdgeRightEye.Value, new Bgr(Color.Red), 2);

               // frameImageCopy.Draw(faceResult.RightEye.Value, new Bgr(Color.Red), 2);
            }

            if (faceResult.LeftEye.HasValue)
            {
                 oneEdgeLeftEye = new Cross2DF(new PointF(faceResult.LeftEye.Value.X, faceResult.LeftEye.Value.Y + faceResult.LeftEye.Value.Height / 2), 3,3);
                 secondEdgeLeftEye = new Cross2DF(new PointF(faceResult.LeftEye.Value.X + faceResult.LeftEye.Value.Width, faceResult.LeftEye.Value.Y + faceResult.LeftEye.Value.Height / 2), 3,3);

                 upEdgeRightEye = new Cross2DF(new PointF(faceResult.LeftEye.Value.X + faceResult.LeftEye.Value.Width / 2, faceResult.LeftEye.Value.Y + faceResult.LeftEye.Value.Height / 4), 3, 3);
                 downEdgeRightEye = new Cross2DF(new PointF(faceResult.LeftEye.Value.X + faceResult.LeftEye.Value.Width / 2, faceResult.LeftEye.Value.Y + faceResult.LeftEye.Value.Height - faceResult.LeftEye.Value.Height / 3), 3, 3);


                frameImageCopy.Draw(oneEdgeLeftEye.Value, new Bgr(Color.Red), 2);
                frameImageCopy.Draw(secondEdgeLeftEye.Value, new Bgr(Color.Red), 2);
                frameImageCopy.Draw(upEdgeRightEye.Value, new Bgr(Color.Red), 2);
                frameImageCopy.Draw(downEdgeRightEye.Value, new Bgr(Color.Red), 2);

               // frameImageCopy.Draw(faceResult.LeftEye.Value, new Bgr(Color.Green), 2);
            }

            if (faceResult.Nose.HasValue)
            {
                noseCenter = new Cross2DF(new PointF(faceResult.Nose.Value.X + faceResult.Nose.Value.Width / 2  + 10, faceResult.Nose.Value.Y + faceResult.Nose.Value.Height / 2 - 10), 5, 20);

                frameImageCopy.Draw(noseCenter.Value, new Bgr(Color.Red), 2);

              //  frameImageCopy.Draw(faceResult.Nose.Value, new Bgr(Color.Green), 2);
            }            
            
            if (faceResult.Mouth.HasValue)
            {
                oneEdgeMouth = new Cross2DF(new PointF(faceResult.Mouth.Value.X + 10, faceResult.Mouth.Value.Y + faceResult.Mouth.Value.Height / 2), 3, 3);
                secondEdgeMouth = new Cross2DF(new PointF(faceResult.Mouth.Value.X + faceResult.Mouth.Value.Width - 10, faceResult.Mouth.Value.Y + faceResult.Mouth.Value.Height / 2), 3, 3);

                frameImageCopy.Draw(oneEdgeMouth.Value, new Bgr(Color.Red), 2);
                frameImageCopy.Draw(secondEdgeMouth.Value, new Bgr(Color.Red), 2);

              //  frameImageCopy.Draw(faceResult.Mouth.Value, new Bgr(Color.Green), 2);
            }

/*
            if(oneEdgeRightEye.HasValue && secondEdgeLeftEye.HasValue)
            {
                LineSegment2DF eyeLine = new LineSegment2DF(oneEdgeRightEye.Value.Center, secondEdgeLeftEye.Value.Center);
                frameImageCopy.Draw(eyeLine, new Bgr(Color.Red), 1);
            }

            if (oneEdgeRightEye.HasValue && secondEdgeRightEye.HasValue & noseCenter.HasValue)
            {

                LineSegment2DF eyeNoseLine1 = new LineSegment2DF(oneEdgeRightEye.Value.Center, noseCenter.Value.Center);
                LineSegment2DF eyeNoseLine2 = new LineSegment2DF(secondEdgeRightEye.Value.Center, noseCenter.Value.Center);

                frameImageCopy.Draw(eyeNoseLine1, new Bgr(Color.Red), 1);
                frameImageCopy.Draw(eyeNoseLine2, new Bgr(Color.Red), 1);
            }

            if (oneEdgeLeftEye.HasValue && secondEdgeLeftEye.HasValue & noseCenter.HasValue)
            {

                LineSegment2DF eyeNoseLine1 = new LineSegment2DF(oneEdgeLeftEye.Value.Center, noseCenter.Value.Center);
                LineSegment2DF eyeNoseLine2 = new LineSegment2DF(secondEdgeLeftEye.Value.Center, noseCenter.Value.Center);

                frameImageCopy.Draw(eyeNoseLine1, new Bgr(Color.Red), 1);
                frameImageCopy.Draw(eyeNoseLine2, new Bgr(Color.Red), 1);
            }

            if(oneEdgeMouth.HasValue && secondEdgeMouth.HasValue && noseCenter.HasValue)
            {
                LineSegment2DF mouthNoseLine1 = new LineSegment2DF(oneEdgeMouth.Value.Center, noseCenter.Value.Center);
                LineSegment2DF mouthNoseLine2 = new LineSegment2DF(secondEdgeMouth.Value.Center, noseCenter.Value.Center);

                frameImageCopy.Draw(mouthNoseLine1, new Bgr(Color.Red), 1);
                frameImageCopy.Draw(mouthNoseLine2, new Bgr(Color.Red), 1);
            }*/

        }
    }
}
