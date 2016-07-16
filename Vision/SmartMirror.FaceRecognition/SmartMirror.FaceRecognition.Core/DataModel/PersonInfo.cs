using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SmartMirror.FaceRecognition.Core.DataModel
{
    [Serializable]
    [DataContract]
    public class PersonInfo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }

        internal PersonInfo Clone()
        {
            return new PersonInfo()
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
