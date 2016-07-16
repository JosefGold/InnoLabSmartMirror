using Emgu.CV;
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
        bool _doFecialRecognition;

        CameraFeed _camFeed;
        FaceDetector _faceDetector;
        FaceRecognizer _faceRecognizer;

        Object _stateSyncObj = new object(); // This shouldn't be neccesery, but just in case 
        SceneTrackingState _state;

        public event SceneUpdated OnSceneUpdated;

        public PersonOfInterestTracker(int fps = 10, bool updateOnDetectionOnly = true, bool detectFaceFeatures = false, bool doFecialRecognition = false, string faceRecognizerModelDir = null)
        {
            _camFeed = new CameraFeed(fps);
            _camFeed.FrameCaptured += _camFeed_FrameCaptured;
            _faceDetector = new FaceDetector();
            _updateOnDetectionOnly = updateOnDetectionOnly;

            if(doFecialRecognition)
            {
                if (string.IsNullOrEmpty(faceRecognizerModelDir))
                {
                    throw new ArgumentNullException("faceRecognizerModelDir", "Must provide face recognizer model directory if wishing to do face recognition");
                }

                _doFecialRecognition = doFecialRecognition;
                _faceRecognizer = new FaceRecognizer(faceRecognizerModelDir);
            }

            _state = new SceneTrackingState()
            {
                StateUpdateTime = DateTime.MinValue,
                FaceLastSeen = DateTime.MinValue,
                FaceFirstSeen = DateTime.MinValue,
                FaceFirstRecognized = DateTime.MinValue,
                FaceLastRecognized = DateTime.MinValue,
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
                    bool faceRecognized = _state.FaceRecognitionResult != null;

                    OnSceneUpdated(new SceneInfo()
                        {
                            SceneFrame = frameImage,
                            HasFace = faceFound,
                            PersonOfInterest = faceFound ? _state.FoundFace.Clone() : null,
                            PersonId = faceFound ? _state.FaceId : Guid.Empty,
                            IsFaceRecognized = faceRecognized,
                            PersonOfInterestIdentity = faceRecognized ? _state.FaceRecognitionResult.Clone() : null,
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
                _state.FaceFirstRecognized = DateTime.MinValue;
                _state.FaceLastRecognized = DateTime.MinValue;
                _state.FaceRecognitionResult = null;
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

                FaceRecognitionResult result = RecognizeFace(foundFace);

                if(result != null && result.RecognizedPerson.Id > 0)
                {
                    _state.FaceFirstRecognized = frameTime;
                    _state.FaceLastRecognized = frameTime;
                    _state.FaceRecognitionResult = result;
                }

                return true;
            }
            // Same Old face
            else if (EstimateSameFaceAsState(foundFace, frameTime))
            {
                var oldFace = _state.FoundFace;
                _state.FoundFace = foundFace;
                oldFace.Dispose();

                if(_state.FaceRecognitionResult == null &&
                    (frameTime - _state.FaceLastSeen).TotalSeconds > 0) // Second rule makes sure we only attempt to recognize once every second
                {
                    FaceRecognitionResult result = RecognizeFace(foundFace);

                    if (result != null && result.RecognizedPerson.Id > 0)
                    {
                        _state.FaceFirstRecognized = frameTime;
                        _state.FaceLastRecognized = frameTime;
                        _state.FaceRecognitionResult = result;
                    }
                }
                else  if (_state.FaceRecognitionResult != null &&
                    (frameTime - _state.FaceLastRecognized).TotalSeconds > 3)
                {
                    // TODO: Handle recognision changing
                    FaceRecognitionResult result = RecognizeFace(foundFace);

                    // Slowly raise confidence level 
                    if (result.ConfidenceLevel > _state.FaceRecognitionResult.ConfidenceLevel)
                    {
                        result.ConfidenceLevel = (result.ConfidenceLevel + _state.FaceRecognitionResult.ConfidenceLevel) / 2; // AverageOut confidance
                    }
                    else
                    {
                        result.ConfidenceLevel = _state.FaceRecognitionResult.ConfidenceLevel;
                    }

                    if (result != null && result.RecognizedPerson.Id > 0)
                    {
                        _state.FaceLastRecognized = frameTime;
                        _state.FaceRecognitionResult = result;
                    }
                }

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

        private FaceRecognitionResult RecognizeFace(FaceDetectionResult foundFace)
        {
           return _faceRecognizer.ResolveFaceImage(foundFace.FaceImage);
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
            public FaceRecognitionResult FaceRecognitionResult { get; set; }
            public Guid FaceId { get; set; }
            public DateTime FaceFirstSeen { get; set; }
            public DateTime FaceLastSeen { get; set; }

            public DateTime FaceFirstRecognized { get; set; }
            public DateTime FaceLastRecognized { get; set; }
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
