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

        public void AddSingleMessage(Discord.IMessage msg)
        {
            SingleMessage m = new SingleMessage();
            m.Margin = new Thickness(0, 0, 0, 4);

            m.MessageText = msg.Content;
            m.Message = msg;

            MessagesPanel.Children.Add(m);
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
