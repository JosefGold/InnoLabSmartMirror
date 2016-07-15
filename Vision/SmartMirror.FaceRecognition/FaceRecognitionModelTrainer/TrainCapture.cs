using Emgu.CV;
using Emgu.CV.Structure;
using SmartMirror.FaceRecognition.Core;
using SmartMirror.FaceRecognition.Core.Visualization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceRecognitionModelTrainer
{
    public partial class TrainCapture : Form, IDisposable
    {
        public List<Image<Bgr, byte>> FaceImages { get; set; }

        private PersonOfInterestTracker _faceTrackers;
        private POITrackerVisualizer _visualizer; 

        public TrainCapture()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            FaceImages = new List<Image<Bgr, byte>>();
            _visualizer = new POITrackerVisualizer();
            _visualizer.FrameCaptured += _visualizer_FrameCaptured;
            _visualizer.Start();
            _faceTrackers = new PersonOfInterestTracker(fps:20, doFecialRecognition:false);
            _faceTrackers.OnSceneUpdated += _faceTrackers_OnSceneUpdated;
            _faceTrackers.Start();
        }

        void _visualizer_FrameCaptured(Image<Bgr, byte> frameImage)
        {
            imageBox1.Image = frameImage;
        }


        DateTime lastSample = DateTime.MinValue;

        void _faceTrackers_OnSceneUpdated(SmartMirror.FaceRecognition.Core.DataModel.SceneInfo sceneInfo)
        {
            if (sceneInfo.HasFace && (sceneInfo.SceneTime - lastSample).TotalSeconds > 2)
            {
                FaceImages.Add(sceneInfo.PersonOfInterest.FaceImage);

                lblCount.Text = FaceImages.Count.ToString();
            }

            if(FaceImages.Count == 20)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }


        public virtual void Dispose()
        {
            if(_faceTrackers != null)
            {
                _faceTrackers.OnSceneUpdated -= _faceTrackers_OnSceneUpdated;
                _faceTrackers.Dispose();
            }

            if (_visualizer != null)
            {
                _visualizer.FrameCaptured -= _visualizer_FrameCaptured;
                _visualizer.Dispose();
            }

            base.Dispose();
        }
    }
}
