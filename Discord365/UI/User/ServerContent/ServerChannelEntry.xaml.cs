using Discord.WebSocket;
using Discord365.UI.User.MessagesPage;
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

namespace Discord365.UI.User.ServerContent
{
    /// <summary>
    /// Interaction logic for ServerChannelEntry.xaml
    /// </summary>
    public partial class ServerChannelEntry : UserControl
    {
        public ServerChannelEntry()
        {
            InitializeComponent();
            tbChannel.Text = "";
        }

        public void SetIcon(UIElement icon)
        {
            IconGrid.Children.Clear();

            if(icon != null)
                IconGrid.Children.Add(icon);
        }

        private SocketGuildChannel channel = null;
        public SocketGuildChannel Channel
        {
            get => channel;
            set
            {
                channel = value;

                UpdateRelated();
            }
        }

        public void UpdateRelated()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => UpdateRelated());
                return;
            }

            tbChannel.Text = GetChannelName();

            if (Channel is SocketTextChannel)
                SetIcon(new Resources.TextChannelImage());
            else
                SetIcon(null);
        }

        public string GetChannelName()
        {
            if (Channel is SocketTextChannel)
            {
                return (Channel as SocketTextChannel).Name;
            }
            else if (Channel is SocketVoiceChannel)
            {
                return (Channel as SocketVoiceChannel).Name;
            }

            return "unknown_channel";
        }
    }
}
