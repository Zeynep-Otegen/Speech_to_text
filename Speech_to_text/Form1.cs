using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using NAudio.Wave;
using Vosk;
using System.Globalization;
using System.Collections.Generic;

namespace MultiLanguageSpeechToText
{
    public partial class Form1 : Form
    {
        private Model model;
        private VoskRecognizer recognizer;
        private WaveInEvent waveIn;
        private bool isRecognizing = false;
        private Dictionary<string, string> modelPaths;

        public Form1()
        {
            InitializeComponent();
            InitializeLanguages();
        }

        private void InitializeLanguages()
        {
            cmbLanguages.Items.Clear();
            cmbLanguages.Items.Add("Türkçe (tr-TR)");
            cmbLanguages.Items.Add("İngilizce (en-US)");
            cmbLanguages.SelectedIndex = 0;

            // Model yollarını eşleştir
            modelPaths = new Dictionary<string, string>
            {
                { "tr-TR", "vosk-model-small-tr-0.3" },
                { "en-US", "vosk-model-small-en-us-0.15" }
            };
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (isRecognizing)
                {
                    MessageBox.Show("Zaten dinleme başlatılmış.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string selectedLang = cmbLanguages.SelectedItem.ToString().Contains("Türkçe") ? "tr-TR" : "en-US";
                string modelDir = Path.Combine(Application.StartupPath, modelPaths[selectedLang]);

                if (!Directory.Exists(modelDir))
                {
                    MessageBox.Show($"Model klasörü bulunamadı:\n{modelDir}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                model = new Model(modelDir);
                recognizer = new VoskRecognizer(model, 16000.0f);
                waveIn = new WaveInEvent
                {
                    DeviceNumber = 0,
                    WaveFormat = new WaveFormat(16000, 1)
                };

                waveIn.DataAvailable += OnDataAvailable;
                waveIn.StartRecording();

                isRecognizing = true;
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                UpdateStatus($"Dinleniyor... ({selectedLang})");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Başlatma hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Hata oluştu");
            }
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                if (recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
                {
                    var result = recognizer.Result();
                    var text = JsonDocument.Parse(result).RootElement.GetProperty("text").GetString();

                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            string timestamp = DateTime.Now.ToString("HH:mm:ss");
                            txtResult.AppendText($"[{timestamp}] {text}\r\n");
                            txtResult.ScrollToCaret();
                            UpdateStatus($"Tanındı: {text}");
                        });
                    }
                }
                else
                {
                    var partial = recognizer.PartialResult();
                    var hypo = JsonDocument.Parse(partial).RootElement.GetProperty("partial").GetString();

                    if (!string.IsNullOrWhiteSpace(hypo))
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            UpdateStatus($"Tahmin: {hypo}...");
                        });
                    }
                }

                // Ses seviyesi göstergesi
                int level = CalculateAudioLevel(e.Buffer, e.BytesRecorded);
                Invoke((MethodInvoker)delegate
                {
                    pbAudioLevel.Value = Math.Min(pbAudioLevel.Maximum, level);
                });
            }
            catch { }
        }

        private int CalculateAudioLevel(byte[] buffer, int bytes)
        {
            int max = 0;
            for (int i = 0; i < bytes; i += 2)
            {
                short sample = BitConverter.ToInt16(buffer, i);
                max = Math.Max(max, Math.Abs(sample));
            }
            return (int)(max / 327.68);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isRecognizing) return;

                waveIn?.StopRecording();
                waveIn?.Dispose();
                recognizer?.Dispose();
                model?.Dispose();

                isRecognizing = false;
                btnStart.Enabled = true;
                btnStop.Enabled = false;

                UpdateStatus("Durduruldu");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Durdurulurken hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatus(string message)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { lblStatus.Text = "Durum: " + message; });
            }
            else
            {
                lblStatus.Text = "Durum: " + message;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                waveIn?.StopRecording();
                waveIn?.Dispose();
                recognizer?.Dispose();
                model?.Dispose();
            }
            catch { }
            base.OnFormClosing(e);
        }
    }
}
