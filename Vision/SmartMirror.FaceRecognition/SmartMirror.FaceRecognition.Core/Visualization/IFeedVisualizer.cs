using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.FaceRecognition.Core.Visualization
{
    public interface IFeedVisualizer : IDisposable
    {
        event FrameCapturedEventHandler FrameCaptured;

        void Start();

        void Stop();
    }
}
