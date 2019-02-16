using Discord.WebSocket;
using Discord365.UI.User.MessagesPage;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Discord365.UI.User.ServerContent
{
    /// <summary>
    /// Interaction logic for ServerLeftSidebar.xaml
    /// </summary>
    public partial class ServerLeftSidebar : UserControl
    {
        public SocketGuild RelatedServer = null;

        public ServerLeftSidebar()
        {
            InitializeComponent();
        }

        private ServerChannelEntry selectedChannel = null;
        public ServerChannelEntry SelectedChannel
        {
            get => selectedChannel;
            set => Select(value);
        }

        public void Select(ServerChannelEntry entry)
        {
            selectedChannel = entry;
            
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => Select(entry));
                return;
            }

            MessagesPageHeader h = new MessagesPageHeader();
            h.tbChannelName.Text = entry.GetChannelName();

            MessagesPageBody b = new MessagesPageBody(entry.Channel);
            App.MainWnd.ContentBasic.Set(h, b);

            b.UpdateMessages();
        }

        public void Add(ServerChannelEntry e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => Add(e));
                return;
            }

            ChannelBox.Items.Add(e);
        }

        private void ChannelList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object o = ChannelBox.SelectedItem;

            if (o == null)
                return;

            if (o is ServerChannelEntry)
            {
                Select(o as ServerChannelEntry);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetChannels();
        }


        private void GetChannels()
        {
            new Thread(() =>
            {
                List<ServerChannelEntry> list = new List<ServerChannelEntry>();

                var client = App.MainWnd.client;


                foreach (var c in RelatedServer.TextChannels)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ServerChannelEntry e = new ServerChannelEntry();
                        e.Channel = c;

                        list.Add(e);
                    });
                }
                
                /*
                foreach (var c in RelatedServer.VoiceChannels)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ServerChannelEntry e = new ServerChannelEntry();
                        e.Channel = c;

                        list.Add(e);
                    });
                }*/ // TODO

                foreach (var l in list)
                {
                    Add(l);
                }

            }).Start();
        }
    }
}
