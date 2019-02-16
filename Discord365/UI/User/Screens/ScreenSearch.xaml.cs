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
    }
}
