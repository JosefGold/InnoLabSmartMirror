using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMirror.FaceRecognition.Core.DataModel
{
    public class SceneInfo : IDisposable
    {
        public Image<Bgr, Byte> SceneFrame { get; set; }
        public DateTime SceneTime { get; set; }
        public FaceDetectionResult PersonOfInterest { get; set; }
        public Guid PersonId { get; set; }
        public bool HasFace { get; set; }
        public bool IsFaceRecognized { get; set; }


        public void Dispose()
        {
            if(SceneFrame != null)
            {
                SceneFrame.Dispose();
            }

            if (PersonOfInterest != null)
            {
                PersonOfInterest.Dispose();
            }
        }
    }
}
