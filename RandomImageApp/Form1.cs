using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace RandomImageApp
{
    public partial class Form1 : Form
    {
        private Image currentImage = null;
        private HttpClient httpClient = new HttpClient();

        public Form1()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            this.Text = "Случайные картинки - Лабораторная 1";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterScreen;

            PictureBox pictureBoxImage = new PictureBox();
            pictureBoxImage.Name = "pictureBoxImage";
            pictureBoxImage.Location = new Point(20, 20);
            pictureBoxImage.Size = new Size(440, 250);
            pictureBoxImage.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxImage.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(pictureBoxImage);

            Label labelStatus = new Label();
            labelStatus.Name = "labelStatus";
            labelStatus.Text = "Нажмите кнопку для загрузки картинки";
            labelStatus.Location = new Point(20, 280);
            labelStatus.Size = new Size(440, 23);
            this.Controls.Add(labelStatus);

            Label labelAuthor = new Label();
            labelAuthor.Name = "labelAuthor";
            labelAuthor.Text = "Автор: Царенков Артем Николаевич";
            labelAuthor.Location = new Point(20, 310);
            labelAuthor.Size = new Size(440, 23);
            labelAuthor.Font = new Font(labelAuthor.Font, FontStyle.Bold);
            this.Controls.Add(labelAuthor);

            Button btnShow = new Button();
            btnShow.Name = "btnShow";
            btnShow.Text = "Показать картинку";
            btnShow.Location = new Point(20, 350);
            btnShow.Size = new Size(200, 35);
            btnShow.Click += BtnShow_Click;
            this.Controls.Add(btnShow);

           // Button btnDownload = new Button();
           // btnDownload.Name = "btnDownload";
           // btnDownload.Text = "Скачать картинку";
           // btnDownload.Location = new Point(260, 350);
           // btnDownload.Size = new Size(200, 35);
           // btnDownload.Click += BtnDownload_Click;
           // btnDownload.Enabled = false;
           // this.Controls.Add(btnDownload);
        }

        private async void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {
                Label labelStatus = (Label)this.Controls["labelStatus"];
                PictureBox pictureBoxImage = (PictureBox)this.Controls["pictureBoxImage"];

                labelStatus.Text = "Загрузка...";
                string randomUrl = $"https://picsum.photos/400/250?random={DateTime.Now.Ticks}";

                byte[] imageBytes = await httpClient.GetByteArrayAsync(randomUrl);

                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    if (currentImage != null) currentImage.Dispose();
                    currentImage = Image.FromStream(ms);
                    pictureBoxImage.Image = currentImage;
                }

                labelStatus.Text = "Картинка загружена!";
                Button btnDownload = (Button)this.Controls["btnDownload"];
               // btnDownload.Enabled = true;
            }
            catch (Exception ex)
            {
                Label labelStatus = (Label)this.Controls["labelStatus"];
                labelStatus.Text = "Ошибка: " + ex.Message;
            }
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            if (currentImage != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Image Files|*.jpg;*.png;*.bmp";
                sfd.Title = "Сохранить картинку";
                sfd.FileName = "random_image_" + DateTime.Now.Ticks;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    currentImage.Save(sfd.FileName);
                    Label labelStatus = (Label)this.Controls["labelStatus"];
                    labelStatus.Text = "Картинка сохранена!";
                }
            }
        }
    }
}