using Discord.WebSocket;
using Discord365.UI.User.MessagesPage;
using Discord365.UI.User.MessagesPage.Message;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Discord365.UI.User.DirectMsgsContents
{
    /// <summary>
    /// Interaction logic for DirectMessagesSidebarContent.xaml
    /// </summary>
    public partial class DirectMessagesSidebarContent : UserControl
    {
        public DMUserEntry[] UserEntries
        {
            get
            {
                List<DMUserEntry> result = new List<DMUserEntry>();

                foreach(var o in DMList.Items)
                {
                    if(o is DMUserEntry)
                    {
                        result.Add(o as DMUserEntry);
                    }
                }

                return result.ToArray();
            }
        }

        private string authorFilter = "";

        public string AuthorFilter
        {
            get => authorFilter;
            set
            {
                authorFilter = value;
            }
        }

        private DMUserEntry selectedChannel = null;
        public DMUserEntry SelectedChannel
        {
            get => selectedChannel;
            set => Select(value);
        }

        public void Select(DMUserEntry entry)
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

            //if (entry.Channel is SocketDMChannel)
            //{
            //    b.DmBotChannel = entry.User.RelatedUser.GetOrCreateDMChannelAsync().GetAwaiter().GetResult();
            //    b.User = entry.User.RelatedUser;
            //}

            App.MainWnd.ContentBasic.Set(h, b);

            b.UpdateMessages();
        }

        public DirectMessagesSidebarContent()
        {
            InitializeComponent();

            DMUserEntry friends = new DMUserEntry();
            friends.Avatar.OnlineMark = UserAvatar.UserOnlineMarks.None;
            friends.User.tbUser.Text = "Friends";

            Add(friends);
        }

        public void Add(DMUserEntry e)
        {
            if(!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => Add(e));
                return;
            }

            DMList.Items.Add(e);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateMessages();
        }

        private void UpdateMessages()
        {
            new Thread(() =>
            {
                List<SocketChannel> channels = new List<SocketChannel>();
                var client = App.MainWnd.client;

                for(int i = 0; i < client.DMChannels.Count; i++)
                    channels.Add(client.DMChannels.ElementAt(i));

                for (int i = 0; i < client.GroupChannels.Count; i++)
                    channels.Add(client.GroupChannels.ElementAt(i));

                // var sorted = ResortMessages(channels); // TAKES AROUND 1 MINUTE TO DOWNLOAD ALL CONVERSATION, LET'S NOT USE SORTING, OK :/

                FinishUpdate(channels);
            }).Start();
        }
        
        private List<SocketChannel> ResortMessages(List<SocketChannel> original)
        {
            List<SocketChannel> result = new List<SocketChannel>();

            var Dict = new Dictionary<DateTime, ChannelDate>();
            var UnsortedTime = new List<DateTime>();

            foreach(var c in original)
            {
                if (c is SocketDMChannel)
                {
                    var dm = c as SocketDMChannel;

                    var msgs = dm.GetMessagesAsync(1).ToList(); // get latest message from channel
                    var reslt = msgs.GetAwaiter().GetResult()[1];

                    if (reslt.Count >= 1)
                    {
                        Dict.Add(reslt.ToList()[0].Timestamp.DateTime, new ChannelDate(dm, reslt.ToList()[0]));
                        UnsortedTime.Add(reslt.ToList()[0].Timestamp.DateTime);
                    }
                    else
                    {
                        Dict.Add(dm.CreatedAt.DateTime, new ChannelDate(dm, null));
                        UnsortedTime.Add(dm.CreatedAt.DateTime);
                    }
                }
                else if (c is SocketGroupChannel)
                {
                    var dm = c as SocketGroupChannel;

                    var msgs = dm.GetMessagesAsync(1).ToList(); // get latest message from channel
                    var reslt = msgs.GetAwaiter().GetResult()[1];

                    if (reslt.Count >= 1)
                    {
                        Dict.Add(reslt.ToList()[0].Timestamp.DateTime, new ChannelDate(dm, reslt.ToList()[0]));
                        UnsortedTime.Add(reslt.ToList()[0].Timestamp.DateTime);
                    }
                    else
                    {
                        Dict.Add(dm.CreatedAt.DateTime, new ChannelDate(dm, null));
                        UnsortedTime.Add(dm.CreatedAt.DateTime);
                    }
                }
            }


            DateTime[] SortedTime = UnsortedTime.ToArray();

            Array.Sort(SortedTime);
            
            foreach (var time in SortedTime)
                result.Add(Dict[time].channel);

            result.Reverse();

            return result;
        }

        private void FinishUpdate(List<SocketChannel> sorted)
        {
            List<DMUserEntry> list = new List<DMUserEntry>();

            var client = App.MainWnd.client;

            for(int i = 0; i < sorted.Count; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    DMUserEntry e = new DMUserEntry();
                    e.Channel = sorted[i];
                    e.UpdateMe();

                    Add(e);
                });
            }
        }


        private void DMList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object o = DMList.SelectedItem;
            if (o == null)
                return;

            if (o is DMUserEntry)
            {
                Select(o as DMUserEntry);
            }

        }
    }

    public class ChannelDate
    {
        public SocketChannel channel;
        public Discord.IMessage message;

        public ChannelDate(SocketChannel c, Discord.IMessage m)
        {
            channel = c;
            message = m;
        }
    }
}
