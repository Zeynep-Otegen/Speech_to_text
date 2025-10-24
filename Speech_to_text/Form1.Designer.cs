using System;
using System.Drawing;
using System.Windows.Forms;

namespace MultiLanguageSpeechToText
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnStart;
        private Button btnStop;
        private RichTextBox txtResult;
        private Label lblStatus;
        private ComboBox cmbLanguages;
        private ComboBox cmbRecognitionMode;
        private CheckBox chkAutoStart;
        private Button btnClear;
        private ProgressBar pbAudioLevel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnStart = new Button();
            this.btnStop = new Button();
            this.txtResult = new RichTextBox();
            this.lblStatus = new Label();
            this.cmbLanguages = new ComboBox();
            this.cmbRecognitionMode = new ComboBox();
            this.chkAutoStart = new CheckBox();
            this.btnClear = new Button();
            this.pbAudioLevel = new ProgressBar();
            this.SuspendLayout();

            // btnStart
            this.btnStart.Location = new Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new Size(120, 30);
            this.btnStart.Text = "Konuşmayı Başlat";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new EventHandler(this.btnStart_Click);

            // btnStop
            this.btnStop.Location = new Point(138, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new Size(120, 30);
            this.btnStop.Text = "Durdur";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new EventHandler(this.btnStop_Click);
            this.btnStop.Enabled = false;

            // cmbLanguages
            this.cmbLanguages.Location = new Point(264, 15);
            this.cmbLanguages.Name = "cmbLanguages";
            this.cmbLanguages.Size = new Size(150, 21);
            this.cmbLanguages.DropDownStyle = ComboBoxStyle.DropDownList;

            // cmbRecognitionMode
            this.cmbRecognitionMode.Location = new Point(420, 15);
            this.cmbRecognitionMode.Name = "cmbRecognitionMode";
            this.cmbRecognitionMode.Size = new Size(150, 21);
            this.cmbRecognitionMode.DropDownStyle = ComboBoxStyle.DropDownList;

            // chkAutoStart
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.Location = new Point(12, 48);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new Size(156, 17);
            this.chkAutoStart.Text = "Uygulama açıldığında başla";
            this.chkAutoStart.UseVisualStyleBackColor = true;

            // btnClear
            this.btnClear.Location = new Point(174, 45);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(84, 23);
            this.btnClear.Text = "Temizle";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new EventHandler(this.btnClear_Click);

            // txtResult
            this.txtResult.Location = new Point(12, 80);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new Size(660, 300);
            this.txtResult.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.txtResult.Font = new Font("Segoe UI", 10F);

            // pbAudioLevel
            this.pbAudioLevel.Location = new Point(12, 390);
            this.pbAudioLevel.Name = "pbAudioLevel";
            this.pbAudioLevel.Size = new Size(200, 20);
            this.pbAudioLevel.Style = ProgressBarStyle.Continuous;

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new Point(218, 393);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(62, 13);
            this.lblStatus.Text = "Hazır";

            // Form1
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(684, 430);
            this.Controls.Add(this.pbAudioLevel);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.chkAutoStart);
            this.Controls.Add(this.cmbRecognitionMode);
            this.Controls.Add(this.cmbLanguages);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "Çok Dilli Speech-to-Text Uygulaması";
            this.Load += new EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtResult.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde çalışacak kodlar buraya eklenebilir.
            // Örneğin, dilleri başlatmak için:
            InitializeLanguages();
        }
    }
}