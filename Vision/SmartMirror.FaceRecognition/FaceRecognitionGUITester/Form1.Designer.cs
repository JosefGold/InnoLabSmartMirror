namespace FaceRecognitionGUITester
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.btnPOITracker = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnFreeDetect = new System.Windows.Forms.Button();
            this.btnRecognize = new System.Windows.Forms.Button();
            this.imgRec = new Emgu.CV.UI.ImageBox();
            this.btnLoadRecognizer = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnRCCamera = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgRec)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBox1
            // 
            this.imageBox1.Location = new System.Drawing.Point(427, 71);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(677, 445);
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // btnPOITracker
            // 
            this.btnPOITracker.Location = new System.Drawing.Point(645, 537);
            this.btnPOITracker.Name = "btnPOITracker";
            this.btnPOITracker.Size = new System.Drawing.Size(75, 23);
            this.btnPOITracker.TabIndex = 3;
            this.btnPOITracker.Text = "POI Tracker";
            this.btnPOITracker.UseVisualStyleBackColor = true;
            this.btnPOITracker.Click += new System.EventHandler(this.button2_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnFreeDetect
            // 
            this.btnFreeDetect.Location = new System.Drawing.Point(842, 537);
            this.btnFreeDetect.Name = "btnFreeDetect";
            this.btnFreeDetect.Size = new System.Drawing.Size(75, 23);
            this.btnFreeDetect.TabIndex = 5;
            this.btnFreeDetect.Text = "Free Detect";
            this.btnFreeDetect.UseVisualStyleBackColor = true;
            this.btnFreeDetect.Click += new System.EventHandler(this.btnFreeDetect_Click);
            // 
            // btnRecognize
            // 
            this.btnRecognize.Enabled = false;
            this.btnRecognize.Location = new System.Drawing.Point(151, 464);
            this.btnRecognize.Name = "btnRecognize";
            this.btnRecognize.Size = new System.Drawing.Size(75, 23);
            this.btnRecognize.TabIndex = 6;
            this.btnRecognize.Text = "Recognize";
            this.btnRecognize.UseVisualStyleBackColor = true;
            this.btnRecognize.Click += new System.EventHandler(this.btnRecognize_Click);
            // 
            // imgRec
            // 
            this.imgRec.Location = new System.Drawing.Point(25, 134);
            this.imgRec.Name = "imgRec";
            this.imgRec.Size = new System.Drawing.Size(332, 261);
            this.imgRec.TabIndex = 7;
            this.imgRec.TabStop = false;
            // 
            // btnLoadRecognizer
            // 
            this.btnLoadRecognizer.Location = new System.Drawing.Point(40, 464);
            this.btnLoadRecognizer.Name = "btnLoadRecognizer";
            this.btnLoadRecognizer.Size = new System.Drawing.Size(75, 23);
            this.btnLoadRecognizer.TabIndex = 8;
            this.btnLoadRecognizer.Text = "Load Train Data";
            this.btnLoadRecognizer.UseVisualStyleBackColor = true;
            this.btnLoadRecognizer.Click += new System.EventHandler(this.btnLoadRecognizer_Click);
            // 
            // btnRCCamera
            // 
            this.btnRCCamera.Enabled = false;
            this.btnRCCamera.Location = new System.Drawing.Point(256, 464);
            this.btnRCCamera.Name = "btnRCCamera";
            this.btnRCCamera.Size = new System.Drawing.Size(121, 23);
            this.btnRCCamera.TabIndex = 9;
            this.btnRCCamera.Text = "Recognize From Cam";
            this.btnRCCamera.UseVisualStyleBackColor = true;
            this.btnRCCamera.Click += new System.EventHandler(this.btnRCCamera_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1148, 603);
            this.Controls.Add(this.btnRCCamera);
            this.Controls.Add(this.btnLoadRecognizer);
            this.Controls.Add(this.imgRec);
            this.Controls.Add(this.btnRecognize);
            this.Controls.Add(this.btnFreeDetect);
            this.Controls.Add(this.btnPOITracker);
            this.Controls.Add(this.imageBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgRec)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox imageBox1;
        private System.Windows.Forms.Button btnPOITracker;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnFreeDetect;
        private System.Windows.Forms.Button btnRecognize;
        private Emgu.CV.UI.ImageBox imgRec;
        private System.Windows.Forms.Button btnLoadRecognizer;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnRCCamera;
    }
}

