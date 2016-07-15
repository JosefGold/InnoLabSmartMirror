using Emgu.CV;
using Emgu.CV.Structure;
using SmartMirror.FaceRecognition.Core.DataModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.FaceRecognition.Core.Visualization
{


    public class POITrackerVisualizer : IFeedVisualizer
    {
        PersonOfInterestTracker _tracker;

        public event FrameCapturedEventHandler FrameCaptured;

        public POITrackerVisualizer()
        {
            _tracker = new PersonOfInterestTracker(fps: 20, updateOnDetectionOnly: false);
        }

        public void Start()
        {
            _tracker.OnSceneUpdated += _tracker_OnSceneUpdated;
            _tracker.Start();
        }

        public void Stop()
        {
            _tracker.Stop();
            _tracker.OnSceneUpdated -= _tracker_OnSceneUpdated;
        }


        void _tracker_OnSceneUpdated(SceneInfo sceneInfo)
        {
            Image<Bgr, byte> frame;

            if (sceneInfo.HasFace)
            {
                frame = VisualizationTools.DrawFacesAsOverlay(sceneInfo.SceneFrame, new Dictionary<string, FaceDetectionResult>() { { sceneInfo.PersonId.ToString().Split('-')[0], sceneInfo.PersonOfInterest } }, true);
            }
            else
            {
                frame = sceneInfo.SceneFrame;
            }

            if (FrameCaptured != null)
            {
                FrameCaptured(frame);
            }
        }



        public void Dispose()
        {
            if (_tracker != null)
            {
                _tracker.Dispose();
            }
        }
    }
}
