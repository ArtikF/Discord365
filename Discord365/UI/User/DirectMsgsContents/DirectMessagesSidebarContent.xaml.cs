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

        public void Select(DMUserEntry entry)
        {
            new Thread(() => {
                if (entry.Channel is SocketDMChannel)
                {
                    SocketDMChannel c = (SocketDMChannel)entry.Channel;

                    var msgs = c.GetCachedMessages();

                    SelectEx(entry, msgs.ToArray());
                }
                else if (entry.Channel is SocketGroupChannel)
                {
                    SocketGroupChannel c = (SocketGroupChannel)entry.Channel;

                    var msgs = c.GetCachedMessages();

                    SelectEx(entry, msgs.ToArray());
                }
            }).Start();
        }

        private void SelectEx(DMUserEntry entry, SocketMessage[] msgs)
        {
            msgs.Reverse();

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => SelectEx(entry, msgs));
                return;
            }

            MessagesPageHeader h = new MessagesPageHeader();
            h.tbChannelName.Text = entry.GetChannelName();

            MessagesPageBody b = new MessagesPageBody(entry.Channel);

            App.MainWnd.ContentBasic.Set(h, b);

            foreach (var msg in msgs.ToArray())
            {
                Message a = new Message();
                a.AddMessage(msg);

                b.MessagesPanel.Children.Add(a);
            }
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

            ResortMessages();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetMessages();
        }

        private void GetMessages()
        {
            new Thread(() =>
            {
                List<DMUserEntry> list = new List<DMUserEntry>();

                var client = App.MainWnd.client;


                foreach (var c in client.DMChannels)
                {
                    Dispatcher.Invoke(() =>
                    {
                        DMUserEntry e = new DMUserEntry();
                        e.RelatedUser = c.Recipient;
                        e.Channel = c;

                        list.Add(e);
                    });
                }

                foreach (var c in client.GroupChannels)
                {
                    Dispatcher.Invoke(() =>
                    {
                        DMUserEntry e = new DMUserEntry();
                        e.User.tbUser.Text = c.Name;
                        e.User.ShowAdditional = true;
                        e.User.tbAdditional.Text = c.Recipients.Count + " members";
                        e.Channel = c;

                        list.Add(e);
                    });
                }
                
                //foreach (var c in client.PrivateChannels)
                //{
                //    if (c.Recipients.Count > 1)
                //        continue;

                //    Dispatcher.Invoke(() =>
                //    {
                //        var avatar = new UserAvatar();
                //        avatar.RelatedUser = c.Recipients.First();

                //        var username = new UserNameUpdateable();
                //        username.RelatedUser = c.Recipients.First();

                //        DMListEntry e = new DMListEntry(avatar, username);
                //        e.PrivateChannel = c;
                //        list.Add(e);
                //    });
                //}

                foreach (var l in list)
                {
                    Add(l);
                }

            }).Start();
        }

        public void ResortMessages()
        {
            return; // idk

            //if (!Dispatcher.CheckAccess())
            //{
            //    Dispatcher.Invoke(() => ResortMessages());
            //    return;
            //}

            //List<DMListEntry> SortedMessages = new List<DMListEntry>();
            //Dictionary<DateTime, DMListEntry> UnsortedMessages = new Dictionary<DateTime, DMListEntry>();

            //foreach(var element in DMList.Items)
            //{
            //    if(element is Grid)
            //    {
            //        if (((Grid)element).Tag == null)
            //            continue;

            //        DMListEntry e = (DMListEntry)((Grid)element).Tag;

            //        if (e.DMChannel != null)
            //        {
            //            var d = e.DMChannel.CreatedAt.Date;
            //            UnsortedMessages.Add(d, e);
            //        }
            //        else if (e.GroupChannel != null)
            //        {
            //            var d = e.GroupChannel.CreatedAt.Date;
            //            UnsortedMessages.Add(d, e);
            //        }
            //    }
            //}

            //DateTime[] Dates = UnsortedMessages.Keys.ToArray();
            //Array.Sort(Dates);

            //foreach(var time in Dates)
            //{
            //    SortedMessages.Add(UnsortedMessages[time]);
            //}

            //SortedMessages.Reverse();

            
            //for (int i = 1; i < DMList.Items.Count; i++)
            //{
            //    DMList.Items.Remove(DMList.Items[i]);
            //}

            //foreach(var e in SortedMessages)
            //{
            //    Add(e);
            //}
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
}
