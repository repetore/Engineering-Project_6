namespace Koshkin_IP
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
            this.openGLControl1 = new SharpGL.OpenGLControl();
            this.button1 = new System.Windows.Forms.Button();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.trackBarX = new System.Windows.Forms.TrackBar();
            this.trackBarY = new System.Windows.Forms.TrackBar();
            this.trackBarZ = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.recordingLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZ)).BeginInit();
            this.SuspendLayout();
            // 
            // openGLControl1
            // 
            this.openGLControl1.DrawFPS = true;
            this.openGLControl1.FrameRate = 60;
            this.openGLControl1.Location = new System.Drawing.Point(12, 163);
            this.openGLControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.openGLControl1.Name = "openGLControl1";
            this.openGLControl1.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL4_4;
            this.openGLControl1.RenderContextType = SharpGL.RenderContextType.NativeWindow;
            this.openGLControl1.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControl1.Size = new System.Drawing.Size(1108, 622);
            this.openGLControl1.TabIndex = 2;
            this.openGLControl1.OpenGLInitialized += new System.EventHandler(this.openGLControl1_OpenGLInitialized);
            this.openGLControl1.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl1_OpenGLDraw);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(173, 49);
            this.button1.TabIndex = 3;
            this.button1.Text = "Старт";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Checked = true;
            this.radioButton3.Location = new System.Drawing.Point(12, 119);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(161, 20);
            this.radioButton3.TabIndex = 6;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Стандартный режим";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(1127, 163);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(355, 618);
            this.dataGridView1.TabIndex = 9;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // trackBarX
            // 
            this.trackBarX.Location = new System.Drawing.Point(435, 46);
            this.trackBarX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBarX.Minimum = -10;
            this.trackBarX.Name = "trackBarX";
            this.trackBarX.Size = new System.Drawing.Size(225, 56);
            this.trackBarX.TabIndex = 10;
            // 
            // trackBarY
            // 
            this.trackBarY.Location = new System.Drawing.Point(774, 46);
            this.trackBarY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBarY.Minimum = -10;
            this.trackBarY.Name = "trackBarY";
            this.trackBarY.Size = new System.Drawing.Size(225, 56);
            this.trackBarY.TabIndex = 11;
            // 
            // trackBarZ
            // 
            this.trackBarZ.Location = new System.Drawing.Point(1127, 46);
            this.trackBarZ.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBarZ.Minimum = -10;
            this.trackBarZ.Name = "trackBarZ";
            this.trackBarZ.Size = new System.Drawing.Size(225, 56);
            this.trackBarZ.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(431, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 16);
            this.label1.TabIndex = 13;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(771, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 16);
            this.label2.TabIndex = 14;
            this.label2.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1136, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "Z";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(444, 93);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(123, 64);
            this.button2.TabIndex = 16;
            this.button2.Text = "Выбрать проецируемое видео";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(609, 94);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(118, 62);
            this.button3.TabIndex = 17;
            this.button3.Text = "Выбрать видео";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(12, 94);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(129, 20);
            this.radioButton1.TabIndex = 18;
            this.radioButton1.Text = "Работа с видео";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(202, 95);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(171, 20);
            this.checkBox1.TabIndex = 19;
            this.checkBox1.Text = "Не учитывать камеру";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(850, 95);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(155, 44);
            this.button4.TabIndex = 20;
            this.button4.Text = "Начать запись";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1149, 95);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(144, 45);
            this.button5.TabIndex = 21;
            this.button5.Text = "Закончить запись";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // recordingLabel
            // 
            this.recordingLabel.AutoSize = true;
            this.recordingLabel.Location = new System.Drawing.Point(1056, 99);
            this.recordingLabel.Name = "recordingLabel";
            this.recordingLabel.Size = new System.Drawing.Size(44, 16);
            this.recordingLabel.TabIndex = 22;
            this.recordingLabel.Text = "label4";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1626, 793);
            this.Controls.Add(this.recordingLabel);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBarZ);
            this.Controls.Add(this.trackBarY);
            this.Controls.Add(this.trackBarX);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.openGLControl1);
            this.Name = "Form1";
            this.Text = "Koshkin IP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SharpGL.OpenGLControl openGLControl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TrackBar trackBarX;
        private System.Windows.Forms.TrackBar trackBarY;
        private System.Windows.Forms.TrackBar trackBarZ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label recordingLabel;
    }
}

