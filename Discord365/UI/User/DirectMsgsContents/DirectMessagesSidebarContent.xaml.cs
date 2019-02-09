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

namespace Discord365.UI.User.DirectMsgsContents
{
    /// <summary>
    /// Interaction logic for DirectMessagesSidebarContent.xaml
    /// </summary>
    public partial class DirectMessagesSidebarContent : UserControl
    {
        public DirectMessagesSidebarContent()
        {
            InitializeComponent();

            var avatar = new UserAvatar();
            avatar.OnlineMark = UserAvatar.UserOnlineMarks.None;
            var username = new UserNameUpdateable();
            username.tbUser.Text = "Friends";

            var friends = new DMListEntry(avatar, username);

            Add(friends);
        }

        public void Add(DMListEntry e)
        {
            if(!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => Add(e));
                return;
            }

            DMList.Items.Add(e.Convert());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetMessages();
        }

        private void GetMessages()
        {
            new Thread(() =>
            {
                List<DMListEntry> list = new List<DMListEntry>();

                var client = App.MainWnd.client;


                foreach (var c in client.DMChannels)
                {
                    Dispatcher.Invoke(() =>
                    {
                        var avatar = new UserAvatar();
                        avatar.RelatedUser = c.Recipient;

                        var username = new UserNameUpdateable();
                        username.RelatedUser = c.Recipient;

                        DMListEntry e = new DMListEntry(avatar, username);
                        e.DMChannel = c;
                        list.Add(e);
                    });
                }

                foreach (var c in client.GroupChannels)
                {
                    Dispatcher.Invoke(() =>
                    {
                        var avatar = new UserAvatar();

                        var username = new UserNameUpdateable();
                        username.tbUser.Text = c.Name;
                        username.ShowAdditional = true;
                        username.tbAdditional.Text = c.Recipients.Count + " members";

                        DMListEntry e = new DMListEntry(avatar, username);
                        e.GroupChannel = c;
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
    }
}
