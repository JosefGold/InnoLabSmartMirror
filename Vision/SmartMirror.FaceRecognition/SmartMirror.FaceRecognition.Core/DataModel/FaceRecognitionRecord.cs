using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace SmartMirror.FaceRecognition.Core.DataModel
{
    public class FaceRecognitionRecord
    {
        public int FaceId { get; set; }
        public Image<Bgr, Byte> FaceImage { get; set; }
    }
}
