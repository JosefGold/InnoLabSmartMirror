using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartMirror.FaceRecognition.Core
{
    public delegate void FrameCapturedEventHandler(Image<Bgr, Byte> frameImage);

    public class CameraFeed : IDisposable
    {
        public event FrameCapturedEventHandler FrameCaptured;

        private Timer _frameCaptureTimer; // this is a "forms" timer on purpous, in case processing on event in slower than framerate 
        private Capture _capture;

        public CameraFeed(int requestedFramesPerSecond = 10)
        {
            if (requestedFramesPerSecond > 24 || requestedFramesPerSecond <= 0)
            {
                throw new ArgumentOutOfRangeException("requestedFramesPerSecond");
            }

            _frameCaptureTimer = new Timer();
            _frameCaptureTimer.Interval = 1000 / requestedFramesPerSecond;
            _frameCaptureTimer.Tick += _frameCaptureTimer_Tick;
            _capture = new Capture();

           if(_capture == null ||  _capture.QueryFrame() == null)
           {
               throw new ApplicationException("Camera feed not found");
           }


        }

        public void Start()
        {
            _frameCaptureTimer.Start();
        }

        public void Stop()
        {
            _frameCaptureTimer.Start();
        }


        void _frameCaptureTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (FrameCaptured != null)
                {
                    using (var frame = _capture.QueryFrame())
                    {
                        using (var frameImg = frame.ToImage<Bgr, Byte>())
                        {
                            FrameCaptured(frameImg);
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        public void Dispose()
        {
            if(_capture != null)
            {
                _capture.Dispose();
            }

            if(_frameCaptureTimer != null)
            {
                _frameCaptureTimer.Dispose();
            }
        }
    }
}
