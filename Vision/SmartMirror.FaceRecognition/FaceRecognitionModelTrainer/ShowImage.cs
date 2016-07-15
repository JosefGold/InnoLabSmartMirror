using Emgu.CV;
using Emgu.CV.Structure;
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
    public partial class ShowImage : Form
    {
        public ShowImage(Image<Bgr,byte> image)
        {
            InitializeComponent();

            imageBox1.Image = image;
        }

    }
}
