using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Discord365.UI.User
{
    /// <summary>
    /// Interaction logic for ServerAvatar.xaml
    /// </summary>
    public partial class ServerAvatar : UserControl
    {
        private string serverName = "";

        public ServerAvatar()
        {
            InitializeComponent();

            ServerName = "Server";
        }

        public ImageSource AvatarImageSource { get => AvatarImage.ImageSource; set => AvatarImage.ImageSource = value; }

        public string ServerName
        {
            get => serverName;
            set
            {
                serverName = value;

                AvatarText.Content = UI.ShortNameTool.GetShortName(serverName);
            }
        }

        public void DownloadAndSetAvatar(string url)
        {
            var bitmap = BitmapFrame.Create(new Uri(url, UriKind.Absolute));

            AvatarImageSource = bitmap;
        }
    }
}
