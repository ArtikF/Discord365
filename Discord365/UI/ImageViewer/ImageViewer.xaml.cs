using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Discord365.UI.ImageViewer
{
    /// <summary>
    /// Interaction logic for ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : Window
    {
        public string DownloadedFolder = null;

        public ImageViewer()
        {
            InitializeComponent();
        }

        public ImageInfo CurrentImage
        {
            get => Album[ImageIndex];
        }

        public List<ImageInfo> Album = new List<ImageInfo>();
        private int imageIndex = 0;
        public int ImageIndex
        {
            get => imageIndex;
            set => Select(value);
        }

        public void AddImage(ImageInfo i)
        {
            Album.Add(i);

            if (Album.Count == 1)
                Select(GetImageIndex(i));

            UpdateCounter();
        }

        public void UpdateCounter()
        {
            tbImageCounter.Text = $"{ImageIndex + 1}/{Album.Count}";
        }

        public int GetImageIndex(ImageInfo info)
        {
            for (int i = 0; i < Album.Count; i++)
            {
                if (Album[i] == info)
                    return i;
            }

            return 0;
        }

        public void Select(int Index)
        {
            if (Album.Count == 0)
                return;

            if (Index >= Album.Count)
            {
                Index = 0;
            }

            imageIndex = Index;

            ImageCanvas.Children.Clear();
            ImageCanvas.Children.Add(new Image() { Source = Album[Index].ToImageSource() });

            UpdateCounter();
        }

        private void ClickToCloseGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LeftBtn_Click(object sender, RoutedEventArgs e)
        {
            Select(ImageIndex - 1);
        }

        private void RightBtn_Click(object sender, RoutedEventArgs e)
        {
            Select(ImageIndex + 1);
        }

        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            if (DownloadedFolder != null)
            {
                Process.Start(DownloadedFolder);
                return;
            }

            new Thread(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    btnDownload.IsEnabled = false;
                    btnDownload.Content = "Downloading...";
                });

                try
                {
                    WebClient client = new WebClient();
                    var fldr = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                    var file = System.IO.Path.Combine(fldr, CurrentImage.filename);

                    client.DownloadFile(CurrentImage.source, file);
                    DownloadedFolder = fldr;

                    Dispatcher.Invoke(() =>
                    {
                        btnDownload.IsEnabled = true;
                        btnDownload.Content = "Open folder";
                    });
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(ex.Message, "Can't download image - Discord 365");
                        btnDownload.IsEnabled = true;
                        btnDownload.Content = "Try again";
                    });
                }

            }).Start();
        }

        private void TbShowOriginal_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start(CurrentImage.source);
        }
    }
}
