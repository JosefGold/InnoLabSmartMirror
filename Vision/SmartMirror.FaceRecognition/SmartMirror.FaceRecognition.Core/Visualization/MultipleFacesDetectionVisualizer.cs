using Emgu.CV;
using Emgu.CV.Structure;
using SmartMirror.FaceRecognition.Core.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.FaceRecognition.Core.Visualization
{
    public class MultipleFacesDetectionVisualizer : IFeedVisualizer
    {

        private FaceDetector _faceDetector;
        private CameraFeed _feed;

        private bool _detectFecialFeatures;

        public event FrameCapturedEventHandler FrameCaptured;


        public MultipleFacesDetectionVisualizer(bool detectFecialFeatures)
        {
            _detectFecialFeatures = detectFecialFeatures;
            _feed = new CameraFeed(20);
            _faceDetector = new FaceDetector();
        }

        public void Start()
        {
            _feed.FrameCaptured += _feed_FrameCaptured;
            _feed.Start();
        }

        public void Stop()
        {
            _feed.Stop();
            _feed.FrameCaptured -= _feed_FrameCaptured;
        }


        void _feed_FrameCaptured(Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte> frameImage)
        {
            Image<Bgr, byte> frame;

            var faces = _faceDetector.FindFacesInImage(frameImage, _detectFecialFeatures);

            if (faces.Any())
            {
                frame = VisualizationTools.DrawFacesAsOverlay(frameImage, faces.ToDictionary(f => "UNKOWN", f => f), false);
            }
            else
            {
                frame = frameImage;
            }

            if (FrameCaptured != null)
            {
                FrameCaptured(frame);
            }

        }




        public void Dispose()
        {
            if (_feed != null)
            {
                _feed.Dispose();
            }
        }
    }
}
