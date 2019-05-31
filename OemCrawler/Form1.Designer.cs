namespace OemCrawler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnGo = new System.Windows.Forms.Button();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.txtToken = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLastStoppedMichlolName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRemoveSpaces = new System.Windows.Forms.Button();
            this.btnFixDiagramId = new System.Windows.Forms.Button();
            this.btnTestToken = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtModelYears = new System.Windows.Forms.TextBox();
            this.txtModelName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtColorList = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.v = new System.Windows.Forms.Button();
            this.txtImagePath = new System.Windows.Forms.TextBox();
            this.txtDiagramTextUrl = new System.Windows.Forms.TextBox();
            this.btnWatermarkRemoveTest = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtColorListToReplace = new System.Windows.Forms.TextBox();
            this.txtMakeGray = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnToGrayScale = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(559, 120);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 31);
            this.btnGo.TabIndex = 0;
            this.btnGo.Text = "Scrap";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(15, 39);
            this.txtUrl.Multiline = true;
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtUrl.Size = new System.Drawing.Size(667, 49);
            this.txtUrl.TabIndex = 1;
            // 
            // txtToken
            // 
            this.txtToken.Location = new System.Drawing.Point(12, 209);
            this.txtToken.Name = "txtToken";
            this.txtToken.Size = new System.Drawing.Size(667, 20);
            this.txtToken.TabIndex = 3;
            this.txtToken.Text = resources.GetString("txtToken.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 193);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Token:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "URL:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(12, 131);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(208, 21);
            this.cmbCategory.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Category:";
            // 
            // txtLastStoppedMichlolName
            // 
            this.txtLastStoppedMichlolName.Location = new System.Drawing.Point(12, 300);
            this.txtLastStoppedMichlolName.Name = "txtLastStoppedMichlolName";
            this.txtLastStoppedMichlolName.Size = new System.Drawing.Size(667, 20);
            this.txtLastStoppedMichlolName.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 284);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(285, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Continue from \'michlol\' name (add the last one you inserted)";
            // 
            // btnRemoveSpaces
            // 
            this.btnRemoveSpaces.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.btnRemoveSpaces.Location = new System.Drawing.Point(12, 335);
            this.btnRemoveSpaces.Name = "btnRemoveSpaces";
            this.btnRemoveSpaces.Size = new System.Drawing.Size(103, 31);
            this.btnRemoveSpaces.TabIndex = 10;
            this.btnRemoveSpaces.Text = "Remove Spaces";
            this.btnRemoveSpaces.UseVisualStyleBackColor = true;
            this.btnRemoveSpaces.Click += new System.EventHandler(this.btnRemoveSpaces_Click);
            // 
            // btnFixDiagramId
            // 
            this.btnFixDiagramId.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.btnFixDiagramId.Location = new System.Drawing.Point(121, 335);
            this.btnFixDiagramId.Name = "btnFixDiagramId";
            this.btnFixDiagramId.Size = new System.Drawing.Size(154, 31);
            this.btnFixDiagramId.TabIndex = 11;
            this.btnFixDiagramId.Text = "Get Diagram Id from Name";
            this.btnFixDiagramId.UseVisualStyleBackColor = true;
            this.btnFixDiagramId.Click += new System.EventHandler(this.btnFixDiagramId_Click);
            // 
            // btnTestToken
            // 
            this.btnTestToken.Location = new System.Drawing.Point(15, 236);
            this.btnTestToken.Name = "btnTestToken";
            this.btnTestToken.Size = new System.Drawing.Size(75, 23);
            this.btnTestToken.TabIndex = 12;
            this.btnTestToken.Text = "Test Token";
            this.btnTestToken.UseVisualStyleBackColor = true;
            this.btnTestToken.Click += new System.EventHandler(this.btnTestToken_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(260, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Model Years:";
            // 
            // txtModelYears
            // 
            this.txtModelYears.Location = new System.Drawing.Point(263, 132);
            this.txtModelYears.Name = "txtModelYears";
            this.txtModelYears.Size = new System.Drawing.Size(94, 20);
            this.txtModelYears.TabIndex = 15;
            // 
            // txtModelName
            // 
            this.txtModelName.Location = new System.Drawing.Point(398, 131);
            this.txtModelName.Name = "txtModelName";
            this.txtModelName.Size = new System.Drawing.Size(132, 20);
            this.txtModelName.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(395, 114);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Model Name:";
            // 
            // txtColorList
            // 
            this.txtColorList.Location = new System.Drawing.Point(12, 458);
            this.txtColorList.Multiline = true;
            this.txtColorList.Name = "txtColorList";
            this.txtColorList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtColorList.Size = new System.Drawing.Size(148, 66);
            this.txtColorList.TabIndex = 18;
            this.txtColorList.Text = "you will get here the results";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 405);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Get Colors from Image.";
            // 
            // v
            // 
            this.v.Location = new System.Drawing.Point(245, 458);
            this.v.Name = "v";
            this.v.Size = new System.Drawing.Size(75, 31);
            this.v.TabIndex = 20;
            this.v.Text = "GO";
            this.v.UseVisualStyleBackColor = true;
            this.v.Click += new System.EventHandler(this.BtnGetColors_Click);
            // 
            // txtImagePath
            // 
            this.txtImagePath.Location = new System.Drawing.Point(12, 421);
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.Size = new System.Drawing.Size(308, 20);
            this.txtImagePath.TabIndex = 21;
            this.txtImagePath.Text = "Paste here the file path (file system)";
            // 
            // txtDiagramTextUrl
            // 
            this.txtDiagramTextUrl.Location = new System.Drawing.Point(337, 421);
            this.txtDiagramTextUrl.Name = "txtDiagramTextUrl";
            this.txtDiagramTextUrl.Size = new System.Drawing.Size(327, 20);
            this.txtDiagramTextUrl.TabIndex = 22;
            this.txtDiagramTextUrl.Text = "Image url";
            // 
            // btnWatermarkRemoveTest
            // 
            this.btnWatermarkRemoveTest.Location = new System.Drawing.Point(589, 458);
            this.btnWatermarkRemoveTest.Name = "btnWatermarkRemoveTest";
            this.btnWatermarkRemoveTest.Size = new System.Drawing.Size(75, 31);
            this.btnWatermarkRemoveTest.TabIndex = 23;
            this.btnWatermarkRemoveTest.Text = "GO";
            this.btnWatermarkRemoveTest.UseVisualStyleBackColor = true;
            this.btnWatermarkRemoveTest.Click += new System.EventHandler(this.BtnWatermarkRemoveTest_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(337, 405);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(126, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Test watermark removing";
            // 
            // txtColorListToReplace
            // 
            this.txtColorListToReplace.Location = new System.Drawing.Point(340, 458);
            this.txtColorListToReplace.Multiline = true;
            this.txtColorListToReplace.Name = "txtColorListToReplace";
            this.txtColorListToReplace.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtColorListToReplace.Size = new System.Drawing.Size(145, 66);
            this.txtColorListToReplace.TabIndex = 25;
            this.txtColorListToReplace.Text = "Color list";
            // 
            // txtMakeGray
            // 
            this.txtMakeGray.Location = new System.Drawing.Point(15, 591);
            this.txtMakeGray.Name = "txtMakeGray";
            this.txtMakeGray.Size = new System.Drawing.Size(308, 20);
            this.txtMakeGray.TabIndex = 27;
            this.txtMakeGray.Text = "Paste here the file path (file system)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 575);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(171, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Make  Grayscale copy of an image";
            // 
            // btnToGrayScale
            // 
            this.btnToGrayScale.Location = new System.Drawing.Point(245, 617);
            this.btnToGrayScale.Name = "btnToGrayScale";
            this.btnToGrayScale.Size = new System.Drawing.Size(75, 31);
            this.btnToGrayScale.TabIndex = 28;
            this.btnToGrayScale.Text = "GO";
            this.btnToGrayScale.UseVisualStyleBackColor = true;
            this.btnToGrayScale.Click += new System.EventHandler(this.BtnToGrayScale_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 658);
            this.Controls.Add(this.btnToGrayScale);
            this.Controls.Add(this.txtMakeGray);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtColorListToReplace);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnWatermarkRemoveTest);
            this.Controls.Add(this.txtDiagramTextUrl);
            this.Controls.Add(this.txtImagePath);
            this.Controls.Add(this.v);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtColorList);
            this.Controls.Add(this.txtModelName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtModelYears);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnTestToken);
            this.Controls.Add(this.btnFixDiagramId);
            this.Controls.Add(this.btnRemoveSpaces);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtLastStoppedMichlolName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtToken);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.btnGo);
            this.Name = "Form1";
            this.Text = "OEM Crawler";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.TextBox txtToken;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLastStoppedMichlolName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnRemoveSpaces;
        private System.Windows.Forms.Button btnFixDiagramId;
        private System.Windows.Forms.Button btnTestToken;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtModelYears;
        private System.Windows.Forms.TextBox txtModelName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtColorList;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button v;
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.TextBox txtDiagramTextUrl;
        private System.Windows.Forms.Button btnWatermarkRemoveTest;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtColorListToReplace;
        private System.Windows.Forms.TextBox txtMakeGray;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnToGrayScale;
    }
}

