using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.FaceRecognition.Core.DataModel
{
    public class FaceDetectionResult
    {
        public FaceDetectionResult(Rectangle face)
        {
            Face = face;
        }

        public Rectangle Face { get; set; }
        public Rectangle? LeftEye { get; set; }
        public Rectangle? RightEye { get; set; }
        public Rectangle? Nose { get; set; }
        public Rectangle? Mouth { get; set; }
    }
}
