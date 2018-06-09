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
            this.label4 = new System.Windows.Forms.Label();
            this.LbModelName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.LbMichlolName = new System.Windows.Forms.Label();
            this.LbPartName = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.LbModelYear = new System.Windows.Forms.Label();
            this.LbPartCounter = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.LbTotalPartCounter = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(662, 137);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 31);
            this.btnGo.TabIndex = 0;
            this.btnGo.Text = "GO";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(70, 20);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(667, 20);
            this.txtUrl.TabIndex = 1;
            this.txtUrl.Text = "https://www.motosport.com/motorcycle/oem-parts/suzuki/2017/gsxr1000?mmy=suzuki;dl" +
    "650abs;2018&mmy_source=oem";
            // 
            // txtToken
            // 
            this.txtToken.Location = new System.Drawing.Point(70, 95);
            this.txtToken.Name = "txtToken";
            this.txtToken.Size = new System.Drawing.Size(667, 20);
            this.txtToken.TabIndex = 3;
            this.txtToken.Text = resources.GetString("txtToken.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 98);
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
            this.cmbCategory.Location = new System.Drawing.Point(70, 56);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(208, 21);
            this.cmbCategory.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Category:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Model:";
            // 
            // LbModelName
            // 
            this.LbModelName.AutoSize = true;
            this.LbModelName.Location = new System.Drawing.Point(69, 181);
            this.LbModelName.Name = "LbModelName";
            this.LbModelName.Size = new System.Drawing.Size(64, 13);
            this.LbModelName.TabIndex = 9;
            this.LbModelName.Text = "ModelName";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 207);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Michlol:";
            // 
            // LbMichlolName
            // 
            this.LbMichlolName.AutoSize = true;
            this.LbMichlolName.Location = new System.Drawing.Point(69, 207);
            this.LbMichlolName.Name = "LbMichlolName";
            this.LbMichlolName.Size = new System.Drawing.Size(68, 13);
            this.LbMichlolName.TabIndex = 11;
            this.LbMichlolName.Text = "MichlolName";
            // 
            // LbPartName
            // 
            this.LbPartName.AutoSize = true;
            this.LbPartName.Location = new System.Drawing.Point(70, 234);
            this.LbPartName.Name = "LbPartName";
            this.LbPartName.Size = new System.Drawing.Size(54, 13);
            this.LbPartName.TabIndex = 13;
            this.LbPartName.Text = "PartName";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 234);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Part:";
            // 
            // LbModelYear
            // 
            this.LbModelYear.AutoSize = true;
            this.LbModelYear.Location = new System.Drawing.Point(139, 181);
            this.LbModelYear.Name = "LbModelYear";
            this.LbModelYear.Size = new System.Drawing.Size(58, 13);
            this.LbModelYear.TabIndex = 14;
            this.LbModelYear.Text = "ModelYear";
            // 
            // LbPartCounter
            // 
            this.LbPartCounter.AutoSize = true;
            this.LbPartCounter.Location = new System.Drawing.Point(70, 255);
            this.LbPartCounter.Name = "LbPartCounter";
            this.LbPartCounter.Size = new System.Drawing.Size(13, 13);
            this.LbPartCounter.TabIndex = 15;
            this.LbPartCounter.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 255);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Part:";
            // 
            // LbTotalPartCounter
            // 
            this.LbTotalPartCounter.AutoSize = true;
            this.LbTotalPartCounter.Location = new System.Drawing.Point(111, 255);
            this.LbTotalPartCounter.Name = "LbTotalPartCounter";
            this.LbTotalPartCounter.Size = new System.Drawing.Size(13, 13);
            this.LbTotalPartCounter.TabIndex = 17;
            this.LbTotalPartCounter.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(89, 255);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(12, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "/";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 281);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.LbTotalPartCounter);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.LbPartCounter);
            this.Controls.Add(this.LbModelYear);
            this.Controls.Add(this.LbPartName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.LbMichlolName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.LbModelName);
            this.Controls.Add(this.label4);
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label LbModelName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label LbMichlolName;
        private System.Windows.Forms.Label LbPartName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label LbModelYear;
        private System.Windows.Forms.Label LbPartCounter;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label LbTotalPartCounter;
        private System.Windows.Forms.Label label9;
    }
}

