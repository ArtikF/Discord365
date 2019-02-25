using Discord.WebSocket;
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

namespace Discord365.UI.User.MessagesPage
{
    /// <summary>
    /// Interaction logic for MessageSendTextBox.xaml
    /// </summary>
    public partial class MessageSendTextBox : UserControl
    {
        public Discord.IDMChannel DmBotChannel = null;
        public SocketChannel Channel = null;
        public const int AnimationLen = 350;
        public bool AnimationDone = true;

        public MessageSendTextBox()
        {
            InitializeComponent();
            tbMessage.Text = "";
        }

        public async void SendMessage(string text)
        {
            // DoGhostAnimation(text); // temporary disabled

            if (Channel != null)
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
            else if (DmBotChannel != null)
            {
                await DmBotChannel.SendMessageAsync(text);
            }
        }

        public void DoGhostAnimation(string text)
        {
            tbMessageGhost.Text = text;

            if (AnimationDone)
            {
                AnimationDone = false;
                tbMessageGhost.Visibility = Visibility.Visible;
                tbMessageGhost.FadeOut(AnimationLen);
                new Thread(() => { Thread.Sleep(AnimationLen); AnimationDone = true; }).Start();
            }

            //new Thread(() => 
            //{
            //    double opacity = tbMessageGhost.Dispatcher.Invoke(() => tbMessageGhost.Opacity);
            //    Thickness margin = tbMessageGhost.Dispatcher.Invoke(() => tbMessageGhost.Margin);

            //    while (opacity > 0)
            //    {

            //    }
            //});
        }

        private void TbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (!Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
                {
                    SendMessage(tbMessage.Text);
                    tbMessage.Text = "";
                }
                else
                {
                    tbMessage.AppendText("\r\n");
                    try
                    {
                        tbMessage.Select(tbMessage.SelectionStart + 1, 0);
                    }
                    catch { }
                }
            }
        }
    }
}
