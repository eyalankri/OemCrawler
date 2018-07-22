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
            this.SuspendLayout();
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(604, 328);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 31);
            this.btnGo.TabIndex = 0;
            this.btnGo.Text = "GO";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(15, 39);
            this.txtUrl.Multiline = true;
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(667, 112);
            this.txtUrl.TabIndex = 1;
            // 
            // txtToken
            // 
            this.txtToken.Location = new System.Drawing.Point(12, 237);
            this.txtToken.Name = "txtToken";
            this.txtToken.Size = new System.Drawing.Size(667, 20);
            this.txtToken.TabIndex = 3;
            this.txtToken.Text = resources.GetString("txtToken.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 221);
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
            this.cmbCategory.Location = new System.Drawing.Point(12, 185);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(208, 21);
            this.cmbCategory.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Category:";
            // 
            // txtLastStoppedMichlolName
            // 
            this.txtLastStoppedMichlolName.Location = new System.Drawing.Point(12, 293);
            this.txtLastStoppedMichlolName.Name = "txtLastStoppedMichlolName";
            this.txtLastStoppedMichlolName.Size = new System.Drawing.Size(667, 20);
            this.txtLastStoppedMichlolName.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 277);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Continue from \'michlol\' name";
            // 
            // btnRemoveSpaces
            // 
            this.btnRemoveSpaces.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.btnRemoveSpaces.Location = new System.Drawing.Point(12, 328);
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
            this.btnFixDiagramId.Location = new System.Drawing.Point(121, 328);
            this.btnFixDiagramId.Name = "btnFixDiagramId";
            this.btnFixDiagramId.Size = new System.Drawing.Size(154, 31);
            this.btnFixDiagramId.TabIndex = 11;
            this.btnFixDiagramId.Text = "Get Diagram Id from Name";
            this.btnFixDiagramId.UseVisualStyleBackColor = true;
            this.btnFixDiagramId.Click += new System.EventHandler(this.btnFixDiagramId_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 369);
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
    }
}

