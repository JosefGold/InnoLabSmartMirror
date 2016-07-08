using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMirror.FaceRecognition.Core.DataModel
{
    [Serializable]
    public class PersonInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
