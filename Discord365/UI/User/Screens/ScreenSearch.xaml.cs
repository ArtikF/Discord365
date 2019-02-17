using Discord.WebSocket;
using Discord365.UI.User.DirectMsgsContents;
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

namespace Discord365.UI.User.Screens
{
    /// <summary>
    /// Interaction logic for ScreenSearch.xaml
    /// </summary>
    public partial class ScreenSearch : UserControl
    {
        public ScreenSearch()
        {
            InitializeComponent();
        }

        public string SearchText => tbSearchBox.Text;

        private void ItsTextChannel_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                ulong textid = ulong.Parse(SearchText);

                var channel = App.MainWnd.client.GetChannel(textid);

                if (channel == null && sender != ItsDMChannel)
                    throw new Exception("Channel is not found");

                if (channel != null)
                {
                    if (channel is SocketGroupChannel)
                    {
                        App.MainWnd.Sidebar.Set(new ScreenSearchHeader(), new SearchGoCompleted());
                        var header = new MessagesPage.MessagesPageHeader();
                        header.Title = ((SocketGroupChannel)channel).Name;
                        var body = new MessagesPage.MessagesPageBody((SocketGroupChannel)channel);
                        App.MainWnd.ContentBasic.Set(header, body);
                        body.UpdateMessages();
                    }
                    else if (channel is SocketTextChannel)
                    {
                        App.MainWnd.Sidebar.Set(new ScreenSearchHeader(), new SearchGoCompleted());
                        var header = new MessagesPage.MessagesPageHeader();
                        header.Title = ((SocketTextChannel)channel).Name;
                        var body = new MessagesPage.MessagesPageBody((SocketTextChannel)channel);
                        App.MainWnd.ContentBasic.Set(header, body);
                        body.UpdateMessages();
                    }
                    else if (channel is SocketDMChannel)
                    {
                        App.MainWnd.Sidebar.Set(new ScreenSearchHeader(), new SearchGoCompleted());
                        var header = new MessagesPage.MessagesPageHeader();
                        header.Title = ((SocketDMChannel)channel).Recipient.Username;
                        var body = new MessagesPage.MessagesPageBody((SocketDMChannel)channel);
                        App.MainWnd.ContentBasic.Set(header, body);
                        body.UpdateMessages();
                    }
                }
                else if (sender == ItsDMChannel)
                {
                    App.MainWnd.Sidebar.Set(new ScreenSearchHeader(), new SearchGoCompleted());
                    var header = new MessagesPage.MessagesPageHeader();
                    var body = new MessagesPage.MessagesPageBody(null);
                    body.User = App.MainWnd.client.GetUser(textid);
                    header.Title = body.User.Username + " (direct messaging)";
                    var real = body.User.GetOrCreateDMChannelAsync().GetAwaiter().GetResult();
                    body.DmBotChannel = real;
                    body.Sender.DmBotChannel = real;
                    App.MainWnd.ContentBasic.Set(header, body);
                    body.UpdateMessages();
                }

            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        public void DisplayError(string text)
        {
            MessageBox.Show(text, "Error");
        }
        

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tbSearchBox.Focus();
        }

        public bool KillThread = false;

        public void AddToList<T>(List<T> list, T value)
        {
            if (list.Contains(value))
                return;

            list.Add(value);
        }

        public void AddUserDM(SocketDMChannel u)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => AddUserDM(u));
                return;
            }

            Grid g = new Grid();
            g.Margin = new Thickness(4);

            TextBlock i = new TextBlock();
            i.Text = u.Recipient.Username;
            g.Children.Add(i);
            i.HorizontalAlignment = HorizontalAlignment.Left;
            i.VerticalAlignment = VerticalAlignment.Center;
            i.Margin = new Thickness(4);

            var UserDMBtn = GetContainer(g);
            UserDMBtn.Click += UserDMBtn_Click;
            UserDMBtn.Tag = u;
            Add(UserDMBtn);
        }

        private void UserDMBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.Tag == null)
                return;

            if (btn.Tag is SocketDMChannel)
            {
                SocketDMChannel c = btn.Tag as SocketDMChannel;

                DMUserEntry etry = new DMUserEntry();
                etry.Channel = c;
                etry.UpdateMe();

                var body = new DirectMsgsContents.DirectMessagesSidebarContent();
                App.MainWnd.Sidebar.Set(new DirectMsgsContents.SidebarHeaderSerach(), body);
                body.Select(etry);
            }

            if (btn.Tag is SocketGroupChannel)
            {
                SocketGroupChannel c = btn.Tag as SocketGroupChannel;

                DMUserEntry etry = new DMUserEntry();
                etry.Channel = c;
                etry.UpdateMe();

                var body = new DirectMsgsContents.DirectMessagesSidebarContent();
                App.MainWnd.Sidebar.Set(new DirectMsgsContents.SidebarHeaderSerach(), body);
                body.Select(etry);
            }
        }

        public Button GetContainer(UIElement i)
        {
            Button btn = new Button();
            btn.Content = i;
            btn.Margin = new Thickness(4);
            return btn;
        }

        public void AddGroupDM(SocketGroupChannel u)
        {
            if(!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => AddGroupDM(u));
                return;
            }

            Grid g = new Grid();
            g.Margin = new Thickness(4);

            TextBlock i = new TextBlock();
            i.Text = "Group " + u.Name;
            g.Children.Add(i);
            i.HorizontalAlignment = HorizontalAlignment.Left;
            i.VerticalAlignment = VerticalAlignment.Center;
            i.Margin = new Thickness(4);

            var UserDMBtn = GetContainer(g);
            UserDMBtn.Click += UserDMBtn_Click;
            UserDMBtn.Tag = u;
            Add(UserDMBtn);
        }

        private void TbSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetTextBlock("Searching...");

            string text = tbSearchBox.Text.ToLower();
            ulong id = 0;

            try
            {
                id = ulong.Parse(tbSearchBox.Text);
            }
            catch { }
            
            if (text.Length == 0)
            {
                KillThread = true;
                WrapResults.Children.Clear();
                return;
            }

            new Thread(() =>
            {
                if (!KillThread)
                {
                    // if previous search unfinished wait until it finish
                    KillThread = true;
                    Thread.Sleep(2000);
                }

                KillThread = false;
                bool IsFound = false;

                foreach (var dm in App.MainWnd.client.DMChannels)
                {
                    if ((dm.Recipient != null && dm.Recipient.Username.ToLower().Contains(text)) || dm.Id == id)
                    {
                        IsFound = true;
                        AddUserDM(dm);
                    }
                        
                    if (KillThread)
                        return;
                }

                foreach (var dm in App.MainWnd.client.GroupChannels)
                {
                    if ((dm.Name != null &&dm.Name.ToLower().Contains(text)) || dm.Id == id)
                    {
                        IsFound = true;
                        AddGroupDM(dm);
                    }

                    if (KillThread)
                        return;
                }

                if (KillThread)
                    return;

                Dispatcher.Invoke(() => 
                {
                    WrapResults.Children.Remove(WrapResults.Children[0]); // remove 1st text box

                    if (IsFound)
                        AppendTextBlock("Search completed");
                    else
                        AppendTextBlock("Not found");
                });
            }).Start();
        }

        public void Add(UIElement e)
        {
            WrapResults.Children.Add(e);
            e.FadeIn(450);
        }

        public void SetTextBlock(string text)
        {
            WrapResults.Children.Clear();
            AppendTextBlock(text);
        }

        public void AppendTextBlock(string text)
        {
            var tb = new TextBlock() { Text = text, TextAlignment = TextAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(4) };
            WrapResults.Children.Add(tb);
        }
        
    }
}
