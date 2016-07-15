using Emgu.CV;
using Emgu.CV.Structure;
using SmartMirror.FaceRecognition.Core;
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


        public TrainCapture()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            _faceTrackers = new PersonOfInterestTracker();
            _faceTrackers.OnSceneUpdated += _faceTrackers_OnSceneUpdated;
            _faceTrackers.Start();
        }

        void _faceTrackers_OnSceneUpdated(SmartMirror.FaceRecognition.Core.DataModel.SceneInfo sceneInfo)
        {
            imageBox1.Image = sceneInfo.SceneFrame;


        }


        public virtual void Dispose()
        {
            if(_faceTrackers != null)
            {
                _faceTrackers.Dispose();
            }

            base.Dispose();
        }
    }
}
