using _34Downloader.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*

if (lang == "ru-RU")
{
    label1.Text = smth;
}
else
{
    label1.Text = smthButEng;
}

this is a example of translation to languages (for form)

before use it, add this:
CultureInfo systemCulture = CultureInfo.InstalledUICulture;
string lang = systemCulture.Name;

and pls, don't delete link to my github :'(
*/

namespace _34Downloader
{
    public partial class Form1 : Form
    {
        private bool _isDownloading = false;
        private Downloader _downloader;

        public Form1()
        {
            InitializeComponent();
            SetupUI();
            Localization();
        }

        private void Localization()
        {
            CultureInfo systemCulture = CultureInfo.InstalledUICulture;
            string lang = systemCulture.Name;
            if (lang == "ru-RU")
            {
                postCount.Text = "Кол-во постов";
                tagsLabel.Text = "Теги";
                logsLabel.Text = "Логи";
                startInstallBtn.Text = "Начать скачивание";
            }
            else
            {
                postCount.Text = "Posts count";
                tagsLabel.Text = "Tags";
                logsLabel.Text = "Logs";
                startInstallBtn.Text = "Start download";
            }
        }
        private void SetupUI()
        {
            richTextBox1.Font = new Font("Consolas", 9);
            richTextBox1.BackColor = Color.FromArgb(30, 30, 30);
            richTextBox1.ForeColor = Color.Lime;
            postsCountDomainUD.Items.Clear();
            postsCountDomainUD.Items.AddRange(new object[] { "5", "10", "20", "50", "100", "200", "500", "1000" });
            postsCountDomainUD.SelectedIndex = 3;
        }

        private async void startInstallBtn_Click(object sender, EventArgs e)
        {
            if (_isDownloading)
            {
                MessageBox.Show("Идет скачивание!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(tagsTextBox.Text))
            {
                MessageBox.Show("Введите теги!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await StartDownload();
        }

        private async Task StartDownload()
        {
            _isDownloading = true;

            try
            {
                startInstallBtn.Enabled = false;
                tagsTextBox.Enabled = false;
                postsCountDomainUD.Enabled = false;
                richTextBox1.Clear();

                int limit = GetLimitFromDomainUpDown();

                _downloader = new Downloader(tagsTextBox.Text, richTextBox1, limit);

                await _downloader.RunAsync();

                MessageBox.Show("Скачивание завершено!", "Готово",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText($"\n[ОШИБКА] {ex.Message}\n");
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                startInstallBtn.Enabled = true;
                tagsTextBox.Enabled = true;
                postsCountDomainUD.Enabled = true;
                _isDownloading = false;
            }
        }

        private int GetLimitFromDomainUpDown()
        {
            string selectedValue = postsCountDomainUD.SelectedItem?.ToString();

            if (int.TryParse(selectedValue, out int limit))
            {
                return limit;
            }

            return 50;
        }

        private void clearLogBtn_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void openFolderBtn_Click(object sender, EventArgs e)
        {
            string tags = tagsTextBox.Text.Trim();
            if (string.IsNullOrEmpty(tags))
            {
                MessageBox.Show("Сначала введите теги!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string folderPath = Path.Combine("downloads", "images", tags);
            if (Directory.Exists(folderPath))
            {
                System.Diagnostics.Process.Start("explorer.exe", folderPath);
            }
            else
            {
                MessageBox.Show("Папка не найдена!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tagsTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                startInstallBtn_Click(sender, e);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tagsTextBox.Focus();
        }

        private void copyLogBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(richTextBox1.Text))
            {
                Clipboard.SetText(richTextBox1.Text);
                MessageBox.Show("Лог скопирован!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            if (_isDownloading)
            {
                MessageBox.Show("Для отмены загрузки закройте приложение",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.ScrollToCaret();
        }

        private void postsCountDomainUD_SelectedItemChanged(object sender, EventArgs e)
        {
            string selectedValue = postsCountDomainUD.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedValue))
            {
                toolTip1.SetToolTip(postsCountDomainUD, $"Будет загружено: {selectedValue} постов");
            }
        }

        private void btnTestApi_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tagsTextBox.Text))
            {
                int limit = GetLimitFromDomainUpDown();
                string testUrl = $"https://api.imgsrc.com?page=dapi&s=post&q=index&tags={Uri.EscapeDataString(tagsTextBox.Text)}&limit={limit}&json=1";
                richTextBox1.AppendText($"Тестовый URL: {testUrl}\n");
                Clipboard.SetText(testUrl);
                MessageBox.Show("URL скопирован в буфер обмена", "Информация");
            }
        }

        private void btnOpenCache_Click(object sender, EventArgs e)
        {
            string tags = tagsTextBox.Text.Trim();
            if (string.IsNullOrEmpty(tags))
            {
                tags = "default";
            }

            string cachePath = Path.Combine("downloads", "cache", tags);
            if (Directory.Exists(cachePath))
            {
                System.Diagnostics.Process.Start("explorer.exe", cachePath);
            }
            else
            {
                Directory.CreateDirectory(cachePath);
                System.Diagnostics.Process.Start("explorer.exe", cachePath);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/AlgorithmIntensity");
        }
    }
}