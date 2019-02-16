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

namespace Discord365.UI.User.MessagesPage
{
    /// <summary>
    /// Interaction logic for MessageSendTextBox.xaml
    /// </summary>
    public partial class MessageSendTextBox : UserControl
    {
        public SocketChannel Channel = null;

        public MessageSendTextBox()
        {
            InitializeComponent();
            tbMessage.Text = "";
        }

        public async void SendMessage(string text)
        {
            if (Channel is SocketDMChannel)
            {
                var c = Channel as SocketDMChannel;
                await c.SendMessageAsync(text);
            }
            else if (Channel is SocketGroupChannel)
            {
                var c = Channel as SocketGroupChannel;
                await c.SendMessageAsync(text);
            }
            else if (Channel is SocketTextChannel)
            {
                var c = Channel as SocketTextChannel;
                await c.SendMessageAsync(text);
            }
        }

        private void TbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                SendMessage(tbMessage.Text);
                tbMessage.Text = "";
            }
        }
    }
}
