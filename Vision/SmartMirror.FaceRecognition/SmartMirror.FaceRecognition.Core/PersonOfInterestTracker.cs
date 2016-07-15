﻿using Emgu.CV;
using Emgu.CV.Structure;
using SmartMirror.FaceRecognition.Core.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.FaceRecognition.Core
{
    public delegate void SceneUpdated(SceneInfo sceneInfo);


    public class PersonOfInterestTracker : IDisposable
    {
        bool _updateOnDetectionOnly;

        CameraFeed _camFeed;
        FaceDetector _faceDetector;

        Object _stateSyncObj = new object(); // This shouldn't be neccesery, but just in case 
        SceneTrackingState _state;

        public event SceneUpdated OnSceneUpdated;

        public PersonOfInterestTracker(bool detectFaceFeatures = false, bool doFecialRecognition = false, int fps = 10, bool updateOnDetectionOnly = true)
        {
            _camFeed = new CameraFeed(fps);
            _camFeed.FrameCaptured += _camFeed_FrameCaptured;
            _faceDetector = new FaceDetector();
            _updateOnDetectionOnly = updateOnDetectionOnly;

            _state = new SceneTrackingState()
            {
                StateUpdateTime = DateTime.MinValue,
                FaceLastSeen = DateTime.MinValue,
                FaceFirstSeen = DateTime.MinValue,
                FaceId = Guid.Empty
            };
        }


        public void Start()
        {
            _camFeed.Start();
        }

        public void Stop()
        {
            _camFeed.Stop();
        }

        void _camFeed_FrameCaptured(Image<Bgr, byte> frameImage)
        {
            lock (_stateSyncObj)
            {
                bool sceneUpdated = false;

                var foundFace = _faceDetector.FindMostDistinctFaceInImage(frameImage);

                DateTime frameTime = DateTime.Now;

                // We found a face!
                if (foundFace != null)
                {
                    sceneUpdated = HandleFaceFoundInFrame(foundFace, frameTime);
                }
                // If no face is found we need to know if this is due to face disengagment or frame detection error  
                else if (foundFace == null && _state.FoundFace != null)
                {
                    sceneUpdated = HandleExistingFaceDisappeared(frameTime);
                }
                // No new face and no old face... boring..
                else
                {
                    sceneUpdated = false;
                }

                _state.StateUpdateTime = frameTime;

                if (OnSceneUpdated != null && (sceneUpdated || !_updateOnDetectionOnly))
                {
                    bool faceFound = _state.FoundFace != null;

                    OnSceneUpdated(new SceneInfo()
                        {
                            SceneFrame = frameImage,
                            HasFace = faceFound,
                            PersonOfInterest = faceFound ? _state.FoundFace.Clone() : null,
                            PersonId = faceFound ? _state.FaceId : Guid.Empty,
                            SceneTime = frameTime
                        });
                }
            }

        }

        private bool HandleExistingFaceDisappeared(DateTime frameTime)
        {
            // Check if face is gone for over 2 seconds
            if ((frameTime - _state.FaceLastSeen).Seconds > 2)
            {
                var oldFace = _state.FoundFace;
                _state.FoundFace = null;
                oldFace.Dispose();

                _state.FaceFirstSeen = DateTime.MinValue;
                _state.FaceLastSeen = DateTime.MinValue;
                _state.FaceId = Guid.Empty;

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool HandleFaceFoundInFrame(FaceDetectionResult foundFace, DateTime frameTime)
        {
            // It's new!
            if (_state.FoundFace == null)
            {
                _state.FoundFace = foundFace;
                _state.FaceFirstSeen = frameTime;
                _state.FaceLastSeen = frameTime;
                _state.FaceId = Guid.NewGuid();

                return true;
            }
            // Same Old face
            else if (EstimateSameFaceAsState(foundFace, frameTime))
            {
                var oldFace = _state.FoundFace;
                _state.FoundFace = foundFace;
                oldFace.Dispose();

                _state.FaceLastSeen = frameTime;

                return true;
            }
            else
            {
                // This shouldn't really happen, unless it's Quicksilver and The Flash playing... 
                // So this is probably a hickup in face detection

                return false;
            }
        }

        private bool EstimateSameFaceAsState(FaceDetectionResult foundFace, DateTime frameTime)
        {

            // TODO Another approach to guesstimate
            // http://synaptitude.me/blog/smooth-face-tracking-using-opencv/
            /*

              if(abs(center.x - priorCenter.x) < frame.size().width / 6 &&
                  abs(center.y - priorCenter.y) < frame.size().height / 6) {

                    // Check to see if the user moved enough to update position                           
                    if(abs(center.x - priorCenter.x) < 7 &&
                       abs(center.y - priorCenter.y) < 7){
                        center = priorCenter;
                    }
                }
             
             * */
            // Add face recognition here


            var newFace = foundFace.Face;
            var oldFace = _state.FoundFace.Face;

            return oldFace.IntersectsWith(newFace);
        }


        protected class SceneTrackingState
        {
            public DateTime StateUpdateTime { get; set; }
            public FaceDetectionResult FoundFace { get; set; }
            public Guid FaceId { get; set; }
            public DateTime FaceFirstSeen { get; set; }
            public DateTime FaceLastSeen { get; set; }
        }



        public void Dispose()
        {
            if (_camFeed != null)
            {
                _camFeed.Dispose();
            }


            if (_faceDetector != null)
            {
                _faceDetector.Dispose();
            }
        }
    }
}
