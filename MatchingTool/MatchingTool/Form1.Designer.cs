namespace MatchingTool
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.picTemplate = new System.Windows.Forms.PictureBox();
            this.trackLevelPyramid = new System.Windows.Forms.TrackBar();
            this.chkInv = new System.Windows.Forms.CheckBox();
            this.lbPyramid = new System.Windows.Forms.Label();
            this.btnTrain = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.lbNumFind = new System.Windows.Forms.Label();
            this.lbTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txbNumSearch = new System.Windows.Forms.TextBox();
            this.txbMinScore = new System.Windows.Forms.TextBox();
            this.lbNumSearch = new System.Windows.Forms.Label();
            this.lbMinScore = new System.Windows.Forms.Label();
            this.btnMatching = new System.Windows.Forms.Button();
            this.btnFile = new System.Windows.Forms.Button();
            this.btnDrawROI = new System.Windows.Forms.Button();
            this.btnGetROI1 = new System.Windows.Forms.Button();
            this.btnGetROI2 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txbPyramid = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPTrain = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkFindAll = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackLevelPyramid)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPTrain.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.picTemplate);
            this.panel1.Location = new System.Drawing.Point(5, 58);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 553);
            this.panel1.TabIndex = 0;
            // 
            // picTemplate
            // 
            this.picTemplate.Location = new System.Drawing.Point(0, 299);
            this.picTemplate.Margin = new System.Windows.Forms.Padding(2);
            this.picTemplate.Name = "picTemplate";
            this.picTemplate.Size = new System.Drawing.Size(280, 252);
            this.picTemplate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picTemplate.TabIndex = 0;
            this.picTemplate.TabStop = false;
            // 
            // trackLevelPyramid
            // 
            this.trackLevelPyramid.LargeChange = 1;
            this.trackLevelPyramid.Location = new System.Drawing.Point(80, 32);
            this.trackLevelPyramid.Margin = new System.Windows.Forms.Padding(2);
            this.trackLevelPyramid.Maximum = 8;
            this.trackLevelPyramid.Name = "trackLevelPyramid";
            this.trackLevelPyramid.Size = new System.Drawing.Size(158, 45);
            this.trackLevelPyramid.TabIndex = 6;
            this.trackLevelPyramid.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackLevelPyramid.Value = 2;
            this.trackLevelPyramid.Scroll += new System.EventHandler(this.trackLevelPyramid_Scroll);
            // 
            // chkInv
            // 
            this.chkInv.AutoSize = true;
            this.chkInv.Location = new System.Drawing.Point(80, 93);
            this.chkInv.Margin = new System.Windows.Forms.Padding(2);
            this.chkInv.Name = "chkInv";
            this.chkInv.Size = new System.Drawing.Size(72, 17);
            this.chkInv.TabIndex = 5;
            this.chkInv.Text = "Nền sáng";
            this.chkInv.UseVisualStyleBackColor = true;
            // 
            // lbPyramid
            // 
            this.lbPyramid.AutoSize = true;
            this.lbPyramid.Location = new System.Drawing.Point(7, 44);
            this.lbPyramid.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbPyramid.Name = "lbPyramid";
            this.lbPyramid.Size = new System.Drawing.Size(72, 13);
            this.lbPyramid.TabIndex = 3;
            this.lbPyramid.Text = "Level pyramid";
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(80, 136);
            this.btnTrain.Margin = new System.Windows.Forms.Padding(2);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(56, 37);
            this.btnTrain.TabIndex = 1;
            this.btnTrain.Text = "Train";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.picImage);
            this.panel2.Location = new System.Drawing.Point(338, 59);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(548, 543);
            this.panel2.TabIndex = 1;
            // 
            // picImage
            // 
            this.picImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picImage.Location = new System.Drawing.Point(0, 0);
            this.picImage.Margin = new System.Windows.Forms.Padding(2);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(548, 543);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picImage.TabIndex = 0;
            this.picImage.TabStop = false;
            this.picImage.Paint += new System.Windows.Forms.PaintEventHandler(this.picImage_Paint);
            this.picImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picImage_MouseDown);
            this.picImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picImage_MouseMove);
            this.picImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picImage_MouseUp);
            // 
            // lbNumFind
            // 
            this.lbNumFind.AutoSize = true;
            this.lbNumFind.Location = new System.Drawing.Point(193, 216);
            this.lbNumFind.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbNumFind.Name = "lbNumFind";
            this.lbNumFind.Size = new System.Drawing.Size(13, 13);
            this.lbNumFind.TabIndex = 7;
            this.lbNumFind.Text = "0";
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.Location = new System.Drawing.Point(193, 238);
            this.lbTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(13, 13);
            this.lbTime.TabIndex = 6;
            this.lbTime.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 238);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Time";
            // 
            // txbNumSearch
            // 
            this.txbNumSearch.Location = new System.Drawing.Point(184, 53);
            this.txbNumSearch.Margin = new System.Windows.Forms.Padding(2);
            this.txbNumSearch.Name = "txbNumSearch";
            this.txbNumSearch.Size = new System.Drawing.Size(47, 20);
            this.txbNumSearch.TabIndex = 4;
            this.txbNumSearch.Text = "1";
            this.txbNumSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txbNumSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txbNumSearch_KeyPress);
            // 
            // txbMinScore
            // 
            this.txbMinScore.Location = new System.Drawing.Point(184, 20);
            this.txbMinScore.Margin = new System.Windows.Forms.Padding(2);
            this.txbMinScore.Name = "txbMinScore";
            this.txbMinScore.Size = new System.Drawing.Size(47, 20);
            this.txbMinScore.TabIndex = 3;
            this.txbMinScore.Text = "0.85";
            this.txbMinScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txbMinScore.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txbMinScore_KeyPress);
            // 
            // lbNumSearch
            // 
            this.lbNumSearch.AutoSize = true;
            this.lbNumSearch.Location = new System.Drawing.Point(29, 53);
            this.lbNumSearch.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbNumSearch.Name = "lbNumSearch";
            this.lbNumSearch.Size = new System.Drawing.Size(120, 13);
            this.lbNumSearch.TabIndex = 2;
            this.lbNumSearch.Text = "Max Number To Search";
            // 
            // lbMinScore
            // 
            this.lbMinScore.AutoSize = true;
            this.lbMinScore.Location = new System.Drawing.Point(29, 20);
            this.lbMinScore.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbMinScore.Name = "lbMinScore";
            this.lbMinScore.Size = new System.Drawing.Size(116, 13);
            this.lbMinScore.TabIndex = 1;
            this.lbMinScore.Text = "Minimum Search Score";
            // 
            // btnMatching
            // 
            this.btnMatching.Location = new System.Drawing.Point(92, 161);
            this.btnMatching.Margin = new System.Windows.Forms.Padding(2);
            this.btnMatching.Name = "btnMatching";
            this.btnMatching.Size = new System.Drawing.Size(68, 46);
            this.btnMatching.TabIndex = 0;
            this.btnMatching.Text = "Matching";
            this.btnMatching.UseVisualStyleBackColor = true;
            this.btnMatching.Click += new System.EventHandler(this.btnMatching_Click);
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(17, 18);
            this.btnFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(56, 19);
            this.btnFile.TabIndex = 3;
            this.btnFile.Text = "File";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // btnDrawROI
            // 
            this.btnDrawROI.Location = new System.Drawing.Point(479, 24);
            this.btnDrawROI.Margin = new System.Windows.Forms.Padding(2);
            this.btnDrawROI.Name = "btnDrawROI";
            this.btnDrawROI.Size = new System.Drawing.Size(65, 26);
            this.btnDrawROI.TabIndex = 4;
            this.btnDrawROI.Text = "Draw ROI";
            this.btnDrawROI.UseVisualStyleBackColor = true;
            this.btnDrawROI.Click += new System.EventHandler(this.btnDrawROI_Click);
            // 
            // btnGetROI1
            // 
            this.btnGetROI1.Location = new System.Drawing.Point(577, 24);
            this.btnGetROI1.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetROI1.Name = "btnGetROI1";
            this.btnGetROI1.Size = new System.Drawing.Size(68, 26);
            this.btnGetROI1.TabIndex = 5;
            this.btnGetROI1.Text = "Get ROI 1";
            this.btnGetROI1.UseVisualStyleBackColor = true;
            this.btnGetROI1.Click += new System.EventHandler(this.btnGetROI1_Click);
            // 
            // btnGetROI2
            // 
            this.btnGetROI2.Location = new System.Drawing.Point(676, 24);
            this.btnGetROI2.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetROI2.Name = "btnGetROI2";
            this.btnGetROI2.Size = new System.Drawing.Size(66, 26);
            this.btnGetROI2.TabIndex = 6;
            this.btnGetROI2.Text = "Get ROI 2";
            this.btnGetROI2.UseVisualStyleBackColor = true;
            this.btnGetROI2.Click += new System.EventHandler(this.btnGetROI2_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txbPyramid
            // 
            this.txbPyramid.Location = new System.Drawing.Point(243, 41);
            this.txbPyramid.Margin = new System.Windows.Forms.Padding(2);
            this.txbPyramid.Name = "txbPyramid";
            this.txbPyramid.ReadOnly = true;
            this.txbPyramid.Size = new System.Drawing.Size(24, 20);
            this.txbPyramid.TabIndex = 7;
            this.txbPyramid.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPTrain);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(5, 58);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(278, 299);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPTrain
            // 
            this.tabPTrain.Controls.Add(this.lbPyramid);
            this.tabPTrain.Controls.Add(this.txbPyramid);
            this.tabPTrain.Controls.Add(this.trackLevelPyramid);
            this.tabPTrain.Controls.Add(this.chkInv);
            this.tabPTrain.Controls.Add(this.btnTrain);
            this.tabPTrain.Location = new System.Drawing.Point(4, 22);
            this.tabPTrain.Margin = new System.Windows.Forms.Padding(2);
            this.tabPTrain.Name = "tabPTrain";
            this.tabPTrain.Padding = new System.Windows.Forms.Padding(2);
            this.tabPTrain.Size = new System.Drawing.Size(270, 273);
            this.tabPTrain.TabIndex = 0;
            this.tabPTrain.Text = "Train";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chkFindAll);
            this.tabPage2.Controls.Add(this.btnMatching);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.lbTime);
            this.tabPage2.Controls.Add(this.lbNumFind);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.lbMinScore);
            this.tabPage2.Controls.Add(this.txbMinScore);
            this.tabPage2.Controls.Add(this.lbNumSearch);
            this.tabPage2.Controls.Add(this.txbNumSearch);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(270, 273);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Patern";
            // 
            // chkFindAll
            // 
            this.chkFindAll.AutoSize = true;
            this.chkFindAll.Location = new System.Drawing.Point(184, 83);
            this.chkFindAll.Margin = new System.Windows.Forms.Padding(2);
            this.chkFindAll.Name = "chkFindAll";
            this.chkFindAll.Size = new System.Drawing.Size(60, 17);
            this.chkFindAll.TabIndex = 9;
            this.chkFindAll.Text = "Find All";
            this.chkFindAll.UseVisualStyleBackColor = true;
            this.chkFindAll.CheckedChanged += new System.EventHandler(this.chkFindAll_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 216);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Number found";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(792, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 622);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnGetROI2);
            this.Controls.Add(this.btnGetROI1);
            this.Controls.Add(this.btnDrawROI);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackLevelPyramid)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPTrain.ResumeLayout(false);
            this.tabPTrain.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox picTemplate;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Button btnDrawROI;
        private System.Windows.Forms.Button btnGetROI1;
        private System.Windows.Forms.Button btnGetROI2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.Button btnMatching;
        private System.Windows.Forms.Label lbPyramid;
        private System.Windows.Forms.TextBox txbNumSearch;
        private System.Windows.Forms.TextBox txbMinScore;
        private System.Windows.Forms.Label lbNumSearch;
        private System.Windows.Forms.Label lbMinScore;
        private System.Windows.Forms.CheckBox chkInv;
        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbNumFind;
        private System.Windows.Forms.TrackBar trackLevelPyramid;
        private System.Windows.Forms.TextBox txbPyramid;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPTrain;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkFindAll;
        private System.Windows.Forms.Button button1;
    }
}

