using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.FaceRecognition.Core.DataModel
{
    public class FaceRecognitionResult
    {
        public PersonInfo RecognizedPerson { get; set; }
        public double ConfidenceLevel { get; set; }

    }
}
