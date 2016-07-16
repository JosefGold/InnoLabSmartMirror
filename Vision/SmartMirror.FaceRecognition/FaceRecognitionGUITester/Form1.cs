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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceRecognitionGUITester
{
    public partial class Form1 : Form
    {
        IFeedVisualizer _visualizer;
        Capture _cap;
        FaceRecognizer _recognizer;

        public Form1()
        {
            InitializeComponent();
            _cap = new Capture();
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
            if (_visualizer != null)
            {
                _visualizer.FrameCaptured -= _visualizer_FrameCaptured;
                _visualizer.Dispose();
                _visualizer = null;
            }
        }

        private void btnRecognize_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            string file = openFileDialog1.FileName;

            var image = new Image<Bgr, byte>(file);
            imgRec.Image = image;

            var result = _recognizer.ResolveMostDistinctFaceInImage(image);
            if (result != null)
            {
                MessageBox.Show(result.RecognizedPerson.Name);
            }
            else
            {
                MessageBox.Show("No Face");
            }
        }

        private void btnLoadRecognizer_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();

            string dir = folderBrowserDialog1.SelectedPath;

            _recognizer = new FaceRecognizer(dir);

            btnRecognize.Enabled = true;
            btnRCCamera.Enabled = true;
        }

        private void btnRCCamera_Click(object sender, EventArgs e)
        {
            using (var frame = _cap.QueryFrame())
            {

                var result = _recognizer.ResolveMostDistinctFaceInImage(frame.ToImage<Bgr, byte>());
                if (result != null)
                {
                    MessageBox.Show(result.RecognizedPerson.Name + " - " + result.ConfidenceLevel + "%");
                }
                else
                {
                    MessageBox.Show("No Face");
                }
            }
        }



    }
}
