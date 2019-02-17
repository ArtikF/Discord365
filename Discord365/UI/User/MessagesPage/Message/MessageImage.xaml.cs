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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Discord365.UI.User.MessagesPage.Message
{
    /// <summary>
    /// Interaction logic for MessagePhoto.xaml
    /// </summary>
    public partial class MessageImage : UserControl
    {
        public string filename = "";

        public MessageImage(string file = "")
        {
            filename = file;
            InitializeComponent();
        }

        public void SetImage(string url)
        {
            MessageImageS.Source = BitmapFrame.Create(new Uri(url + "?width=400&height=225"));
            MessageImageS.Tag = url;
        }

        private void MessagePhoto_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MessageImageS.Tag == null || MessageImageS.Tag.GetType() != typeof(string))
                return;

            var viewer = new UI.ImageViewer.ImageViewer();
            viewer.AddImage(new ImageViewer.ImageInfo(filename, MessageImageS.Tag as string));
            viewer.ShowDialog();
        }
    }
}
