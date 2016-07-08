using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.FaceRecognition.Core.DataModel
{
    public class FaceDetectionResult  : IDisposable
    {

        private FaceDetectionResult() // For clone
        {

        }

        public FaceDetectionResult(Rectangle face, Image<Bgr, Byte> faceImage)
        {
            Face = face;
            FaceImage = faceImage;
        }


        public Rectangle Face { get; set; }
        public Image<Bgr, Byte> FaceImage { get; set; }
        public Rectangle? LeftEye { get; set; }
        public Rectangle? RightEye { get; set; }
        public Rectangle? Nose { get; set; }
        public Rectangle? Mouth { get; set; }


        public FaceDetectionResult Clone()
        {
            return new FaceDetectionResult()
            {
                Face = Face,
                FaceImage = FaceImage.Clone(),
                LeftEye = LeftEye,
                Mouth = Mouth,
                Nose = Nose,
                RightEye = RightEye
            };
        }


        public void Dispose()
        {
            if(FaceImage != null)
            {
                FaceImage.Dispose();
            }
        }
    }
}
