using Discord.WebSocket;
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

namespace Discord365.UI.User.DirectMsgsContents
{
    /// <summary>
    /// Interaction logic for DMUserEntry.xaml
    /// </summary>
    public partial class DMUserEntry : UserControl
    {
        public SocketChannel Channel = null;

        public DMUserEntry()
        {
            InitializeComponent();
        }

        public void UpdateMe()
        {
            if (Channel == null)
                return;

            if (Channel is SocketDMChannel)
            {
                var dm = Channel as SocketDMChannel;
                RelatedUser = dm.Recipient;
            }
            else if (Channel is SocketGroupChannel)
            {
                var dm = Channel as SocketGroupChannel;
                User.tbUser.Text = dm.Name;
                User.ShowAdditional = true;
                User.tbAdditional.Text = dm.Recipients.Count + " members";
            }
        }

        public SocketUser RelatedUser
        {
            set
            {
                Avatar.RelatedUser = value;
                User.RelatedUser = value;
            }
        }

        public string GetChannelName()
        {
            if (Channel is SocketDMChannel)
            {
                return (Channel as SocketDMChannel).Recipient.Username;
            }
            else if (Channel is SocketGroupChannel)
            {
                return (Channel as SocketGroupChannel).Name;
            }

            return "Unknown Channel";
        }
    }
}
