namespace RawFindDesktop
{
    partial class RawFind
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RawFind));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_process = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.btnSelect1 = new System.Windows.Forms.Button();
            this.btnSelect2 = new System.Windows.Forms.Button();
            this.btnSelect3 = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnResize = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelect4 = new System.Windows.Forms.Button();
            this.lbl_resize_path = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSelect5 = new System.Windows.Forms.Button();
            this.lbl_output_path = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(107, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(118, 21);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "C:\\p_jpg";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "精选JPG";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(107, 62);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(118, 21);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "C:\\p_raw";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "照片集合";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(107, 101);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(118, 21);
            this.textBox3.TabIndex = 1;
            this.textBox3.Text = "C:\\re";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "精选Raw生成";
            // 
            // btn_process
            // 
            this.btn_process.BackgroundImage = global::RawFindDesktop.Properties.Resources.btn;
            this.btn_process.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_process.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btn_process.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btn_process.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_process.Location = new System.Drawing.Point(385, 20);
            this.btn_process.Name = "btn_process";
            this.btn_process.Size = new System.Drawing.Size(101, 102);
            this.btn_process.TabIndex = 3;
            this.btn_process.UseVisualStyleBackColor = true;
            this.btn_process.Click += new System.EventHandler(this.btn_process_Click);
            this.btn_process.MouseLeave += new System.EventHandler(this.btn_process_MouseLeave);
            this.btn_process.MouseHover += new System.EventHandler(this.btn_process_MouseHover);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "Raw扩展名";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "NEF",
            "RAW",
            "DNG"});
            this.comboBox1.Location = new System.Drawing.Point(107, 139);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(118, 20);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.Text = "NEF";
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.SystemColors.WindowText;
            this.rtbLog.Location = new System.Drawing.Point(18, 29);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(470, 92);
            this.rtbLog.TabIndex = 5;
            this.rtbLog.Text = "";
            this.rtbLog.TextChanged += new System.EventHandler(this.rtbLog_TextChanged);
            // 
            // btnSelect1
            // 
            this.btnSelect1.Location = new System.Drawing.Point(276, 21);
            this.btnSelect1.Name = "btnSelect1";
            this.btnSelect1.Size = new System.Drawing.Size(75, 23);
            this.btnSelect1.TabIndex = 6;
            this.btnSelect1.Text = "选择路径";
            this.btnSelect1.UseVisualStyleBackColor = true;
            this.btnSelect1.Click += new System.EventHandler(this.btnSelect1_Click);
            // 
            // btnSelect2
            // 
            this.btnSelect2.Location = new System.Drawing.Point(276, 60);
            this.btnSelect2.Name = "btnSelect2";
            this.btnSelect2.Size = new System.Drawing.Size(75, 23);
            this.btnSelect2.TabIndex = 7;
            this.btnSelect2.Text = "选择路径";
            this.btnSelect2.UseVisualStyleBackColor = true;
            this.btnSelect2.Click += new System.EventHandler(this.btnSelect2_Click);
            // 
            // btnSelect3
            // 
            this.btnSelect3.Location = new System.Drawing.Point(276, 99);
            this.btnSelect3.Name = "btnSelect3";
            this.btnSelect3.Size = new System.Drawing.Size(75, 23);
            this.btnSelect3.TabIndex = 8;
            this.btnSelect3.Text = "选择路径";
            this.btnSelect3.UseVisualStyleBackColor = true;
            this.btnSelect3.Click += new System.EventHandler(this.btnSelect3_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(18, 127);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(470, 23);
            this.progressBar.TabIndex = 9;
            // 
            // btnResize
            // 
            this.btnResize.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnResize.BackgroundImage")));
            this.btnResize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnResize.FlatAppearance.BorderColor = System.Drawing.Color.Green;
            this.btnResize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnResize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResize.Location = new System.Drawing.Point(399, 20);
            this.btnResize.Name = "btnResize";
            this.btnResize.Size = new System.Drawing.Size(77, 77);
            this.btnResize.TabIndex = 14;
            this.btnResize.UseVisualStyleBackColor = true;
            this.btnResize.Click += new System.EventHandler(this.btnResize_Click);
            this.btnResize.MouseLeave += new System.EventHandler(this.btnResize_MouseLeave);
            this.btnResize.MouseHover += new System.EventHandler(this.btnResize_MouseHover);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.btn_process);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnSelect3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.btnSelect1);
            this.groupBox1.Controls.Add(this.btnSelect2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(503, 181);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "照片筛选";
            // 
            // btnSelect4
            // 
            this.btnSelect4.Location = new System.Drawing.Point(107, 32);
            this.btnSelect4.Name = "btnSelect4";
            this.btnSelect4.Size = new System.Drawing.Size(110, 23);
            this.btnSelect4.TabIndex = 9;
            this.btnSelect4.Text = "输入路径选择";
            this.btnSelect4.UseVisualStyleBackColor = true;
            this.btnSelect4.Click += new System.EventHandler(this.btnSelect4_Click);
            // 
            // lbl_resize_path
            // 
            this.lbl_resize_path.AutoSize = true;
            this.lbl_resize_path.Location = new System.Drawing.Point(16, 65);
            this.lbl_resize_path.Name = "lbl_resize_path";
            this.lbl_resize_path.Size = new System.Drawing.Size(89, 12);
            this.lbl_resize_path.TabIndex = 10;
            this.lbl_resize_path.Text = "请选择输入路径";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "800px",
            "1000px",
            "1500px",
            "2000px"});
            this.comboBox2.Location = new System.Drawing.Point(18, 34);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(72, 20);
            this.comboBox2.TabIndex = 9;
            this.comboBox2.Text = "800px";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSelect5);
            this.groupBox2.Controls.Add(this.lbl_output_path);
            this.groupBox2.Controls.Add(this.comboBox2);
            this.groupBox2.Controls.Add(this.btnSelect4);
            this.groupBox2.Controls.Add(this.lbl_resize_path);
            this.groupBox2.Controls.Add(this.btnResize);
            this.groupBox2.Location = new System.Drawing.Point(12, 208);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(503, 105);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "照片缩放";
            // 
            // btnSelect5
            // 
            this.btnSelect5.Location = new System.Drawing.Point(241, 32);
            this.btnSelect5.Name = "btnSelect5";
            this.btnSelect5.Size = new System.Drawing.Size(110, 23);
            this.btnSelect5.TabIndex = 18;
            this.btnSelect5.Text = "输出路径选择";
            this.btnSelect5.UseVisualStyleBackColor = true;
            this.btnSelect5.Click += new System.EventHandler(this.btnSelect5_Click);
            // 
            // lbl_output_path
            // 
            this.lbl_output_path.AutoSize = true;
            this.lbl_output_path.Location = new System.Drawing.Point(16, 85);
            this.lbl_output_path.Name = "lbl_output_path";
            this.lbl_output_path.Size = new System.Drawing.Size(89, 12);
            this.lbl_output_path.TabIndex = 16;
            this.lbl_output_path.Text = "请选择输出路径";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.progressBar);
            this.groupBox3.Controls.Add(this.rtbLog);
            this.groupBox3.Location = new System.Drawing.Point(12, 330);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(503, 168);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "操作日志";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 506);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(527, 22);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // RawFind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 528);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RawFind";
            this.Text = "RawFind";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_process;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Button btnSelect1;
        private System.Windows.Forms.Button btnSelect2;
        private System.Windows.Forms.Button btnSelect3;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnResize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSelect4;
        private System.Windows.Forms.Label lbl_resize_path;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lbl_output_path;
        private System.Windows.Forms.Button btnSelect5;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    }
}

