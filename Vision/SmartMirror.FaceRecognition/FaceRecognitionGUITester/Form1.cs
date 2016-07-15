using Emgu.CV;
using SmartMirror.FaceRecognition.Core;
using SmartMirror.FaceRecognition.Core.Visualization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceRecognitionGUITester
{
    public partial class Form1 : Form
    {
        IFeedVisualizer _visualizer;

        public Form1()
        {
            InitializeComponent();
        }



        private void btnFreeDetect_Click(object sender, EventArgs e)
        {
            ResetVisualizer();

            _visualizer = new MultipleFacesDetectionVisualizer(true);
            _visualizer.FrameCaptured += _visualizer_FrameCaptured;
            _visualizer.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ResetVisualizer();

            _visualizer = new POITrackerVisualizer();
            _visualizer.FrameCaptured += _visualizer_FrameCaptured;
            _visualizer.Start();
        }

        void _visualizer_FrameCaptured(Image<Emgu.CV.Structure.Bgr, byte> frameImage)
        {
            imageBox1.Image = frameImage;
        }

        private void ResetVisualizer()
        {
            if(_visualizer != null)
            {
                _visualizer.FrameCaptured -= _visualizer_FrameCaptured;
                _visualizer.Dispose();
                _visualizer = null;
            }
        }



    }
}
