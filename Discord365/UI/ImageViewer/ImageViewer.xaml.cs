using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public ImageViewer()
        {
            InitializeComponent();
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
            if (Index >= Album.Count)
                return;

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
    }
}
