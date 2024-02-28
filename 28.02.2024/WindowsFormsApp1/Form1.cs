using System;
using System.IO;
using System.IO.Compression;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string selectedFolderPath;

        public Form1()
        {
            InitializeComponent();
        }
        private void OpenFolder(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    selectedFolderPath = folderDialog.SelectedPath;
                    textEdit1.Text = selectedFolderPath;
                }
            }
        }
        private string GenerateZipFileName(string sourcePath)
        {
            string folderName = new DirectoryInfo(sourcePath).Name;
            System.DateTime now = System.DateTime.Now;
            string formattedDate = now.ToString("yyyy_MM_dd__HH_mm_ss");
            return $"{folderName}_{formattedDate}.zip";
        }
        private void FileUpload(object sender, EventArgs e)
        {
            
            string sourcePath = selectedFolderPath;

            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                MessageBox.Show("Lütfen bir klasör seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string zipFileName = GenerateZipFileName(sourcePath);
            string targetZipFile = Path.Combine("D:\\TestPlace", zipFileName); //FARKLI BİR BİLGİSAYARDA TEST EDİYORSAN KODU DEĞİŞTİR

            try
            {
                if (Directory.Exists(sourcePath))
                {
                    ZipFile.CreateFromDirectory(sourcePath, targetZipFile);
                    labelControl1.Text = "Son kayıt: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Tarihinde alındı.";
                    //Buraya bir fonksiyon gerekli. Fonksiyonun içeriği timeEdit1'deki veriyi alıp labelControl3.Text'deki yazıyı güncellemeli
                    MessageBox.Show("Sıkıştırma başarıyla tamamlandı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NextSaveTime(object sender, EventArgs e)
        {
            TimeSpan selectedDelayTime = timeEdit1.Time.TimeOfDay;
            DateTime selectedDateTime = DateTime.Now.Add(selectedDelayTime);

            string hourMinute = selectedDelayTime.ToString();
            string[] hourAndMinute = hourMinute.Split(':');
            int hour = int.Parse(hourAndMinute[0]);
            int minute = int.Parse(hourAndMinute[1]);

            TimeSpan interval = new TimeSpan(hour, minute, 0);

            if ((int)interval.TotalMilliseconds != 0)
            {
                timer1.Interval = (int)interval.TotalMilliseconds;
                timer1.Start();
                labelControl3.Text = "Sonraki Kayıt Zamanı: " + selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                timer1.Stop();
                labelControl3.Text = "Tekrar sıklığı 0 saniye olamaz.";
            }
            
        }
    }
}