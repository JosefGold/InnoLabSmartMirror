using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.FaceRecognition.Core.DataModel
{
    public class FaceRecognitionPersonTrainData
    {
        public PersonInfo PersonInfo { get; set; }
        public List<Image<Bgr, Byte>> FacialImages { get; set; }
    }
}
