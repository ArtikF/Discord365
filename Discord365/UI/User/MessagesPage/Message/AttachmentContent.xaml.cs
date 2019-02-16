using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for AttachmentContent.xaml
    /// </summary>
    public partial class AttachmentContent : UserControl
    {
        private Discord.IAttachment attach = null;
        public Discord.IAttachment Attach
        {
            get => attach;
            set
            {
                attach = value;

                tbFileName.Text = value.Filename;
                tbFileSize.Text = value.Size.ToString() + " bytes";
            }
        }

        public AttachmentContent()
        {
            InitializeComponent();
            tbFileName.Text = "";
            tbFileSize.Text = "";
        }

        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            if(Attach != null)
            {
                Process.Start(Attach.Url);
            }
        }
    }
}
