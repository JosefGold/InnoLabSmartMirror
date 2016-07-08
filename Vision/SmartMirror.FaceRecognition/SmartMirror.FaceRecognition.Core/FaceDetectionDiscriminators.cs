using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using SmartMirror.FaceRecognition.Core.DataModel;

namespace SmartMirror.FaceRecognition.Core
{
    public class FaceDetectionDiscriminators
    {

        public static IEnumerable<FaceDetectionResult> GetMostDistinctiveFaceInFrame(IEnumerable<FaceDetectionResult> faces, Image<Bgr, byte> frame)
        {
            if (faces.Any())
            {
                var selectedFace = faces.OrderByDescending(face => CalculateFaceImportanceWeight(face, frame))
                                         .First();

                return new List<FaceDetectionResult>() { selectedFace };
            }
            else
            {
                return faces;
            }
        }

        private static double CalculateFaceImportanceWeight(FaceDetectionResult face, Image<Bgr, byte> frame)
        {
            int faceSpace = face.Face.Height * face.Face.Width;
            int faceCenterX = face.Face.X + (face.Face.Width / 2);
            int faceCenterY = face.Face.Y + (face.Face.Height / 2);

            int frameCenterX = frame.Width / 2;
            int frameCenterY = frame.Height / 2;

            double faceDistanceFromCenter = Math.Sqrt((frameCenterX - faceCenterX) ^ 2 + (frameCenterY - faceCenterY) ^ 2);
            double maxDistanceFromCenter = Math.Sqrt(frameCenterX ^ 2 + frameCenterY ^ 2);

            double centerDistanceWeight = Math.Abs(faceDistanceFromCenter - maxDistanceFromCenter) / frame.Width;

            return faceSpace * centerDistanceWeight;
        }
    }
}
