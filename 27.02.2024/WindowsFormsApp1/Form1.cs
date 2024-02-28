using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    textBox1.Text = folderDialog.SelectedPath;
                }
            }
        }

        private string GenerateZipFileName(string sourcePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(sourcePath);
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("yyyy_MM_dd_HH_mm_ss");
            return $"{fileName}_{formattedDate}.zip";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sourcePath = textBox1.Text;

            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                MessageBox.Show("Lütfen bir klasör veya dosya seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string zipFileName = GenerateZipFileName(sourcePath);
            string targetZipFile = Path.Combine(@"D:\TestPlace", zipFileName);

            try
            {
                if (Directory.Exists(sourcePath))
                {
                    // Eğer seçilen bir klasörse
                    ZipFile.CreateFromDirectory(sourcePath, targetZipFile);
                }
                else if (File.Exists(sourcePath))
                {
                    // Eğer seçilen bir dosyaysa
                    using (FileStream fileStream = new FileStream(targetZipFile, FileMode.Create))
                    {
                        using (ZipArchive archive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                        {
                            string fileNameInArchive = Path.GetFileName(sourcePath);
                            archive.CreateEntryFromFile(sourcePath, fileNameInArchive);
                        }
                    }
                }
                MessageBox.Show("Sıkıştırma başarıyla tamamlandı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            { MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}



