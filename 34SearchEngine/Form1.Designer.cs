namespace _34Downloader
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.startInstallBtn = new System.Windows.Forms.Button();
            this.postCount = new System.Windows.Forms.Label();
            this.tagsTextBox = new System.Windows.Forms.TextBox();
            this.tagsLabel = new System.Windows.Forms.Label();
            this.logsLabel = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.postsCountDomainUD = new System.Windows.Forms.DomainUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(381, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(155, 152);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "34Downloader";
            // 
            // startInstallBtn
            // 
            this.startInstallBtn.Location = new System.Drawing.Point(19, 116);
            this.startInstallBtn.Name = "startInstallBtn";
            this.startInstallBtn.Size = new System.Drawing.Size(117, 23);
            this.startInstallBtn.TabIndex = 2;
            this.startInstallBtn.Text = "Начать скачивание";
            this.startInstallBtn.UseVisualStyleBackColor = true;
            this.startInstallBtn.Click += new System.EventHandler(this.startInstallBtn_Click);
            // 
            // postCount
            // 
            this.postCount.AutoSize = true;
            this.postCount.Location = new System.Drawing.Point(16, 36);
            this.postCount.Name = "postCount";
            this.postCount.Size = new System.Drawing.Size(68, 13);
            this.postCount.TabIndex = 3;
            this.postCount.Text = "posts counttt";
            // 
            // tagsTextBox
            // 
            this.tagsTextBox.Location = new System.Drawing.Point(19, 90);
            this.tagsTextBox.Name = "tagsTextBox";
            this.tagsTextBox.Size = new System.Drawing.Size(100, 20);
            this.tagsTextBox.TabIndex = 6;
            // 
            // tagsLabel
            // 
            this.tagsLabel.AutoSize = true;
            this.tagsLabel.Location = new System.Drawing.Point(16, 74);
            this.tagsLabel.Name = "tagsLabel";
            this.tagsLabel.Size = new System.Drawing.Size(30, 13);
            this.tagsLabel.TabIndex = 5;
            this.tagsLabel.Text = "tags)";
            // 
            // logsLabel
            // 
            this.logsLabel.AutoSize = true;
            this.logsLabel.Location = new System.Drawing.Point(13, 154);
            this.logsLabel.Name = "logsLabel";
            this.logsLabel.Size = new System.Drawing.Size(32, 13);
            this.logsLabel.TabIndex = 7;
            this.logsLabel.Text = "Логи";
            this.logsLabel.Click += new System.EventHandler(this.label4_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.Black;
            this.richTextBox1.ForeColor = System.Drawing.Color.Lime;
            this.richTextBox1.Location = new System.Drawing.Point(12, 170);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(524, 359);
            this.richTextBox1.TabIndex = 8;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // postsCountDomainUD
            // 
            this.postsCountDomainUD.Location = new System.Drawing.Point(19, 51);
            this.postsCountDomainUD.Name = "postsCountDomainUD";
            this.postsCountDomainUD.Size = new System.Drawing.Size(100, 20);
            this.postsCountDomainUD.TabIndex = 9;
            this.postsCountDomainUD.Text = "5";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(141, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(165, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "by @argdus | @AlgorithmIntensity";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGreen;
            this.ClientSize = new System.Drawing.Size(548, 541);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.postsCountDomainUD);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.logsLabel);
            this.Controls.Add(this.tagsTextBox);
            this.Controls.Add(this.tagsLabel);
            this.Controls.Add(this.postCount);
            this.Controls.Add(this.startInstallBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "34Downloader";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button startInstallBtn;
        private System.Windows.Forms.Label postCount;
        private System.Windows.Forms.TextBox tagsTextBox;
        private System.Windows.Forms.Label tagsLabel;
        private System.Windows.Forms.Label logsLabel;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.DomainUpDown postsCountDomainUD;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label5;
    }
}

