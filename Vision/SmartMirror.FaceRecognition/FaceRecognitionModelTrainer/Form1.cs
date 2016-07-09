using Emgu.CV;
using Emgu.CV.Structure;
using SmartMirror.FaceRecognition.Core;
using SmartMirror.FaceRecognition.Core.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceRecognitionModelTrainer
{
    public partial class Form1 : Form
    {
        int nClassCounter = 0;
        int nUnkownClassCounter = 0;
        FaceDetector _faceDetector;
        FaceRecognizer _faceRecognizer;

        List<FaceRecognitionPersonTrainData> collectedTrainData = new List<FaceRecognitionPersonTrainData>();

        public Form1()
        {
            InitializeComponent();

            _faceDetector = new FaceDetector();
            _faceRecognizer = new FaceRecognizer();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSelectTrainPicsFolder_Click(object sender, EventArgs e)
        {
            var diaglogResult = folderBrowserDialog1.ShowDialog();

            if (diaglogResult == System.Windows.Forms.DialogResult.OK)
            {
                txtPicturesFolder.Text = folderBrowserDialog1.SelectedPath;

                btnAddPerson.Enabled = true;
            }
        }

        private void btnSelectTrainSaveFolder_Click(object sender, EventArgs e)
        {
            var diaglogResult = folderBrowserDialog1.ShowDialog();

            if (diaglogResult == System.Windows.Forms.DialogResult.OK)
            {
                txtModelSaveFolder.Text = folderBrowserDialog1.SelectedPath;
                btnTrainAndSave.Enabled = true;
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPersonName.Text))
            {
                MessageBox.Show("Missing Person Name");
                return;
            }

            if (string.IsNullOrEmpty(txtPicturesFolder.Text))
            {
                MessageBox.Show("No Pictures folder was selected");
                return;
            }

            List<Image<Bgr, byte>> faceImages = GetFaceImagesFromFolderPicture(txtPicturesFolder.Text, chkCropFaces.Checked);

            if (faceImages.Count == 0)
            {
                MessageBox.Show("No Face pictures found in fiven folder");
                return;
            }
            else
            {
                if (faceImages.Count < 5)
                {

                    MessageBox.Show("Warning: Less than 5 face images given");
                }


                int classId;

                if (chkIsUnkownClass.Checked)
                {
                    nUnkownClassCounter--;
                    classId = nUnkownClassCounter;
                }
                else
                {
                    nClassCounter++;
                    classId = nClassCounter;
                }


                FaceRecognitionPersonTrainData trainData = new FaceRecognitionPersonTrainData()
                {
                    PersonInfo = new PersonInfo()
                    {
                        Id = classId,
                        Name = chkIsUnkownClass.Checked ? "UNKOWN" : txtNewPersonName.Text
                    },
                    FacialImages = faceImages
                };

                AddNewPersonTrainData(trainData);

                ClearPersonAddingArae();
            }

        }

        private void ClearPersonAddingArae()
        {
            txtPicturesFolder.Clear();
            txtNewPersonName.Clear();
            chkCropFaces.Checked = true;
            chkIsUnkownClass.Checked = false;
        }


        private void btnTrainAndSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtModelSaveFolder.Text))
            {
                MessageBox.Show("No Model Save folder was selected");
                return;
            }

            if (collectedTrainData.Count == 0)
            {
                MessageBox.Show("No Train data was selected");
                return;
            }

            this.Enabled = false;

            try
            {
                _faceRecognizer.Train(collectedTrainData);
                _faceRecognizer.Save(txtModelSaveFolder.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to train and save model: " + ex.ToString());
            }

            this.Enabled = true;
        }



        private List<Image<Bgr, byte>> GetFaceImagesFromFolderPicture(string imagesPath, bool cropFaces)
        {
            List<Image<Bgr, byte>> faceImages = new List<Image<Bgr, byte>>();

            try
            {
                if (Directory.Exists(imagesPath))
                {
                    string[] pictureFiles = Directory.GetFiles(imagesPath, "*.jpg", SearchOption.TopDirectoryOnly);

                    if (pictureFiles.Length > 0)
                    {
                        var readImages = pictureFiles.Select(file => new Image<Bgr, byte>(file));

                        if (cropFaces)
                        {
                            readImages = readImages.Select(img => _faceDetector.FindMostDistinctFaceInImage(img))
                                         .Where(face => face != null)
                                         .Select(face => face.FaceImage);

                        }

                        faceImages = readImages.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading pictures in folder: " + ex.ToString());
            }


            return faceImages;
        }

        private void AddNewPersonTrainData(FaceRecognitionPersonTrainData trainData)
        {
            collectedTrainData.Add(trainData);

            TreeNode node = new TreeNode(trainData.PersonInfo.Name + " [" + trainData.PersonInfo.Id + "]");

            for (int i = 0; i < trainData.FacialImages.Count; i++)
            {

                node.Nodes.Add("Image " + (i+1) + " [" + trainData.FacialImages[i].GetHashCode() + "]");
            }

            treeViewTrainData.Nodes.Add(node);
        }

    }
}
