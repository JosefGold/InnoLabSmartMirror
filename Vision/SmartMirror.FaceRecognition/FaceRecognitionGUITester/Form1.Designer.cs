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
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBox1
            // 
            this.imageBox1.Location = new System.Drawing.Point(128, 71);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(976, 451);
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // btnPOITracker
            // 
            this.btnPOITracker.Location = new System.Drawing.Point(536, 551);
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
            this.btnFreeDetect.Location = new System.Drawing.Point(653, 551);
            this.btnFreeDetect.Name = "btnFreeDetect";
            this.btnFreeDetect.Size = new System.Drawing.Size(75, 23);
            this.btnFreeDetect.TabIndex = 5;
            this.btnFreeDetect.Text = "Free Detect";
            this.btnFreeDetect.UseVisualStyleBackColor = true;
            this.btnFreeDetect.Click += new System.EventHandler(this.btnFreeDetect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1148, 603);
            this.Controls.Add(this.btnFreeDetect);
            this.Controls.Add(this.btnPOITracker);
            this.Controls.Add(this.imageBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox imageBox1;
        private System.Windows.Forms.Button btnPOITracker;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnFreeDetect;
    }
}

