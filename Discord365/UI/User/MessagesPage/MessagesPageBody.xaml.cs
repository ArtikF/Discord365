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
using static Discord365.UI.Extensions;

namespace Discord365.UI.User.MessagesPage
{
    /// <summary>
    /// Interaction logic for MessagesPageBody.xaml
    /// </summary>
    public partial class MessagesPageBody : UserControl
    {
        public SocketChannel Channel = null;
        public Discord.IDMChannel DmBotChannel = null;
        public Discord.IUser User = null;

        public MessagesPageBody(SocketChannel c = null)
        {
            Channel = c;

            InitializeComponent();

            Sender.Channel = Channel;
        }

        public static Border GetSeparator()
        {
            Border b = new Border();
            b.Margin = new Thickness(0, 8, 0, 8);
            b.HorizontalAlignment = HorizontalAlignment.Stretch;
            b.Height = 1;
            b.Background = new SolidColorBrush(Color.FromArgb(0x9, 0xFF, 0xFF, 0xFF));
            return b;
        }

        public void AddMessage(Message.Message e)
        {
            var related = GetRelatedMessage(e);

            if (related == null || related.TimeStamp.AddMinutes(3) < e.TimeStamp)
            {
                MessagesPanel.Children.Add(GetSeparator());
                e.Margin = new Thickness(0, 8, 0, 8);
                MessagesPanel.Children.Add(e);
            }
            else
            {
                foreach(var msg in e.MessagesPanel.Children)
                {
                    var m = msg as Message.SingleMessage;
                    related.AppendMessage(m.Message);
                }
            }

            MessagesScroll.ScrollToEnd(); // fix me https://github.com/discord365/Discord365/issues/27
        }

        private Message.Message GetRelatedMessage(Message.Message m)
        {
            if (MessagesPanel.Children.Count < 1)
                return null;

            var last = MessagesPanel.Children[MessagesPanel.Children.Count - 1] as Message.Message;

            if (last.RelatedUser != null && last.RelatedUser.Id == m.RelatedUser.Id)
                return last;

            return null;
        }

        public void UpdateMessages()
        {
            new Thread(() => 
            {
                const int HowManyMessagesDownload = 25;

                if (Channel != null)
                {
                    if (Channel is SocketDMChannel)
                    {
                        SocketDMChannel c = (SocketDMChannel)Channel;

                        var msgs = c.GetMessagesAsync(HowManyMessagesDownload).ToList();
                        var reslt = msgs.GetAwaiter().GetResult()[1];

                        UpdateMessagesEx(reslt.ToArray());
                    }
                    else if (Channel is SocketGroupChannel)
                    {
                        SocketGroupChannel c = (SocketGroupChannel)Channel;

                        var msgs = c.GetMessagesAsync(HowManyMessagesDownload).ToList();
                        var reslt = msgs.GetAwaiter().GetResult()[1];

                        UpdateMessagesEx(reslt.ToArray());
                    }
                    else if (Channel is SocketTextChannel)
                    {
                        SocketTextChannel c = (SocketTextChannel)Channel;

                        var msgs = c.GetMessagesAsync(HowManyMessagesDownload).ToList();
                        var reslt = msgs.GetAwaiter().GetResult()[1];

                        UpdateMessagesEx(reslt.ToArray());
                    }
                    else if (Channel is SocketChannel)
                    {
                        SocketTextChannel c = (SocketTextChannel)Channel;

                        var msgs = c.GetMessagesAsync(HowManyMessagesDownload).ToList();
                        var reslt = msgs.GetAwaiter().GetResult()[1];

                        UpdateMessagesEx(reslt.ToArray());
                    }
                }

                if (DmBotChannel != null)
                {
                    var msgs = DmBotChannel.GetMessagesAsync(HowManyMessagesDownload).ToList();
                    var reslt1 = msgs.GetAwaiter().GetResult();
                    var reslt = reslt1[0];

                    UpdateMessagesEx(reslt.ToArray());
                }
            }).Start();
        }

        private void UpdateMessagesEx(Discord.IMessage[] msgss)
        {
            var msgs = SortMessagesByDate(msgss);
            UpdateMessagesEx2(msgs);
        }

        private void UpdateMessagesEx2(Discord.IMessage[] msgs)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => UpdateMessagesEx2(msgs));
                return;
            }

            MessagesPanel.Children.Clear();

            // https://softwareengineering.stackexchange.com/questions/287369/beginners-c-question-about-array-reverse
            // I used foreach here previously

            for (int i = 0; i < msgs.Length; i++)
            {
                var msg = msgs[i];

                Message.Message a = new Message.Message();
                a.TimeStamp = msg.Timestamp.DateTime;
                a.RelatedUser = msg.Author;
                a.AppendMessage(msg);

                this.AddMessage(a);
            }

            MessagesScroll.ScrollToEnd(); // fix me
        }
    }
}
