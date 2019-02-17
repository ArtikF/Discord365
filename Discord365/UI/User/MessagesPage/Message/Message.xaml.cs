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

namespace Discord365.UI.User.MessagesPage.Message
{
    /// <summary>
    /// Interaction logic for Message.xaml
    /// </summary>
    public partial class Message : UserControl
    {
        private Discord.IUser relatedUser = null;
        public DateTime TimeStamp
        {
            get => MessageHeader.TimeStamp;
            set => MessageHeader.TimeStamp = value;
        }

        public Message()
        {
            InitializeComponent();
        }

        public Discord.IUser RelatedUser
        {
            get => relatedUser;
            set
            {
                relatedUser = value;

                MessageHeader.RelatedUser = value;
            }
        }

        public void AppendMessage(Discord.IMessage msg)
        {
            SingleMessage m = new SingleMessage();
            m.Message = msg;
            MessagesPanel.Children.Add(m);

            if (msg.Content.Length >= 1)
            {
                var c = new TextMessageContent();
                c.Margin = DefaultPadding;
                c.MessageText = msg.Content;
                m.Panel.Children.Add(c);
            }

            if(msg.Attachments.Count >= 1)
            {
                foreach(var attachment in msg.Attachments)
                {
                    var aobj = new AttachmentContent();
                    aobj.HorizontalAlignment = HorizontalAlignment.Stretch;
                    aobj.Attach = attachment;
                    aobj.Margin = DefaultPadding;
                    m.Panel.Children.Add(aobj);
                }
            }
        }

        public Thickness DefaultPadding => new Thickness(0, 2, 0, 2);

        public void AddImageMessage(Discord.IMessage msg)
        {

        }

        public void RemoveMessage(SocketMessage msg)
        {
            foreach (var m in MessagesPanel.Children)
            {
                SingleMessage s = m as SingleMessage;

                if (s.Message == msg)
                    MessagesPanel.Children.Remove(s);
            }
        }
    }
}
