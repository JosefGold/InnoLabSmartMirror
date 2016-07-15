namespace FaceRecognitionModelTrainer
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAddPerson = new System.Windows.Forms.Button();
            this.chkCropFaces = new System.Windows.Forms.CheckBox();
            this.btnSelectTrainPicsFolder = new System.Windows.Forms.Button();
            this.txtPicturesFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkIsUnkownClass = new System.Windows.Forms.CheckBox();
            this.txtNewPersonName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.treeViewTrainData = new System.Windows.Forms.TreeView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSelectTrainSaveFolder = new System.Windows.Forms.Button();
            this.txtModelSaveFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnTrainAndSave = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnCaptureFromCam = new System.Windows.Forms.Button();
            this.btnAddUnkowns = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCaptureFromCam);
            this.groupBox1.Controls.Add(this.btnAddPerson);
            this.groupBox1.Controls.Add(this.chkCropFaces);
            this.groupBox1.Controls.Add(this.btnSelectTrainPicsFolder);
            this.groupBox1.Controls.Add(this.txtPicturesFolder);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.chkIsUnkownClass);
            this.groupBox1.Controls.Add(this.txtNewPersonName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(378, 377);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1. Add New Person Class:";
            // 
            // btnAddPerson
            // 
            this.btnAddPerson.Enabled = false;
            this.btnAddPerson.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnAddPerson.Location = new System.Drawing.Point(102, 246);
            this.btnAddPerson.Name = "btnAddPerson";
            this.btnAddPerson.Size = new System.Drawing.Size(138, 29);
            this.btnAddPerson.TabIndex = 7;
            this.btnAddPerson.Text = "Add";
            this.btnAddPerson.UseVisualStyleBackColor = true;
            this.btnAddPerson.Click += new System.EventHandler(this.btnAddPerson_Click);
            // 
            // chkCropFaces
            // 
            this.chkCropFaces.AutoSize = true;
            this.chkCropFaces.Checked = true;
            this.chkCropFaces.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCropFaces.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.chkCropFaces.Location = new System.Drawing.Point(20, 200);
            this.chkCropFaces.Name = "chkCropFaces";
            this.chkCropFaces.Size = new System.Drawing.Size(110, 24);
            this.chkCropFaces.TabIndex = 6;
            this.chkCropFaces.Text = "Crop Faces";
            this.chkCropFaces.UseVisualStyleBackColor = true;
            // 
            // btnSelectTrainPicsFolder
            // 
            this.btnSelectTrainPicsFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnSelectTrainPicsFolder.Location = new System.Drawing.Point(246, 155);
            this.btnSelectTrainPicsFolder.Name = "btnSelectTrainPicsFolder";
            this.btnSelectTrainPicsFolder.Size = new System.Drawing.Size(75, 29);
            this.btnSelectTrainPicsFolder.TabIndex = 5;
            this.btnSelectTrainPicsFolder.Text = "Browse..";
            this.btnSelectTrainPicsFolder.UseVisualStyleBackColor = true;
            this.btnSelectTrainPicsFolder.Click += new System.EventHandler(this.btnSelectTrainPicsFolder_Click);
            // 
            // txtPicturesFolder
            // 
            this.txtPicturesFolder.Location = new System.Drawing.Point(20, 155);
            this.txtPicturesFolder.Name = "txtPicturesFolder";
            this.txtPicturesFolder.Size = new System.Drawing.Size(220, 29);
            this.txtPicturesFolder.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.Location = new System.Drawing.Point(16, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(213, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Train Pictures Source Folder:";
            // 
            // chkIsUnkownClass
            // 
            this.chkIsUnkownClass.AutoSize = true;
            this.chkIsUnkownClass.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.chkIsUnkownClass.Location = new System.Drawing.Point(20, 74);
            this.chkIsUnkownClass.Name = "chkIsUnkownClass";
            this.chkIsUnkownClass.Size = new System.Drawing.Size(129, 24);
            this.chkIsUnkownClass.TabIndex = 2;
            this.chkIsUnkownClass.Text = "Unkown Class";
            this.chkIsUnkownClass.UseVisualStyleBackColor = true;
            // 
            // txtNewPersonName
            // 
            this.txtNewPersonName.Location = new System.Drawing.Point(77, 39);
            this.txtNewPersonName.Name = "txtNewPersonName";
            this.txtNewPersonName.Size = new System.Drawing.Size(244, 29);
            this.txtNewPersonName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(16, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // treeViewTrainData
            // 
            this.treeViewTrainData.Location = new System.Drawing.Point(19, 39);
            this.treeViewTrainData.Name = "treeViewTrainData";
            this.treeViewTrainData.Size = new System.Drawing.Size(265, 254);
            this.treeViewTrainData.TabIndex = 2;
            this.treeViewTrainData.DoubleClick += new System.EventHandler(this.treeViewTrainData_DoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAddUnkowns);
            this.groupBox2.Controls.Add(this.treeViewTrainData);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.groupBox2.Location = new System.Drawing.Point(428, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(328, 377);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2. Review Train Data Set";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSelectTrainSaveFolder);
            this.groupBox3.Controls.Add(this.txtModelSaveFolder);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.btnTrainAndSave);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.groupBox3.Location = new System.Drawing.Point(12, 406);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(744, 149);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "3. Train And Save Model";
            // 
            // btnSelectTrainSaveFolder
            // 
            this.btnSelectTrainSaveFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnSelectTrainSaveFolder.Location = new System.Drawing.Point(257, 76);
            this.btnSelectTrainSaveFolder.Name = "btnSelectTrainSaveFolder";
            this.btnSelectTrainSaveFolder.Size = new System.Drawing.Size(75, 29);
            this.btnSelectTrainSaveFolder.TabIndex = 11;
            this.btnSelectTrainSaveFolder.Text = "Browse..";
            this.btnSelectTrainSaveFolder.UseVisualStyleBackColor = true;
            this.btnSelectTrainSaveFolder.Click += new System.EventHandler(this.btnSelectTrainSaveFolder_Click);
            // 
            // txtModelSaveFolder
            // 
            this.txtModelSaveFolder.Location = new System.Drawing.Point(31, 76);
            this.txtModelSaveFolder.Name = "txtModelSaveFolder";
            this.txtModelSaveFolder.Size = new System.Drawing.Size(220, 29);
            this.txtModelSaveFolder.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label3.Location = new System.Drawing.Point(27, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(198, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Trained Model Save Folder";
            // 
            // btnTrainAndSave
            // 
            this.btnTrainAndSave.Enabled = false;
            this.btnTrainAndSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnTrainAndSave.Location = new System.Drawing.Point(402, 76);
            this.btnTrainAndSave.Name = "btnTrainAndSave";
            this.btnTrainAndSave.Size = new System.Drawing.Size(138, 29);
            this.btnTrainAndSave.TabIndex = 8;
            this.btnTrainAndSave.Text = "Train And Save";
            this.btnTrainAndSave.UseVisualStyleBackColor = true;
            this.btnTrainAndSave.Click += new System.EventHandler(this.btnTrainAndSave_Click);
            // 
            // btnCaptureFromCam
            // 
            this.btnCaptureFromCam.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnCaptureFromCam.Location = new System.Drawing.Point(77, 308);
            this.btnCaptureFromCam.Name = "btnCaptureFromCam";
            this.btnCaptureFromCam.Size = new System.Drawing.Size(208, 29);
            this.btnCaptureFromCam.TabIndex = 8;
            this.btnCaptureFromCam.Text = "Capture from Camera";
            this.btnCaptureFromCam.UseVisualStyleBackColor = true;
            this.btnCaptureFromCam.Click += new System.EventHandler(this.btnCaptureFromCam_Click);
            // 
            // btnAddUnkowns
            // 
            this.btnAddUnkowns.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnAddUnkowns.Location = new System.Drawing.Point(29, 308);
            this.btnAddUnkowns.Name = "btnAddUnkowns";
            this.btnAddUnkowns.Size = new System.Drawing.Size(137, 29);
            this.btnAddUnkowns.TabIndex = 9;
            this.btnAddUnkowns.Text = "Add Unkowns";
            this.btnAddUnkowns.UseVisualStyleBackColor = true;
            this.btnAddUnkowns.Click += new System.EventHandler(this.btnAddUnkowns_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 582);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Face Recognition Model Trainer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkIsUnkownClass;
        private System.Windows.Forms.TextBox txtNewPersonName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectTrainPicsFolder;
        private System.Windows.Forms.TextBox txtPicturesFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddPerson;
        private System.Windows.Forms.CheckBox chkCropFaces;
        private System.Windows.Forms.TreeView treeViewTrainData;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSelectTrainSaveFolder;
        private System.Windows.Forms.TextBox txtModelSaveFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnTrainAndSave;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnCaptureFromCam;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnAddUnkowns;
    }
}

