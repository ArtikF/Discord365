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

namespace Discord365.UI.User.MessagesPage.Message
{
    /// <summary>
    /// Interaction logic for MessageHeader.xaml
    /// </summary>
    public partial class MessageHeader : UserControl
    {
        public DateTime TimeStamp
        {
            get
            {
                if (tbTimeStamp.Tag != null && tbTimeStamp.Tag is DateTime)
                    return (DateTime)tbTimeStamp.Tag;
                else
                    return DateTime.MinValue;
            }
            set { tbTimeStamp.Text = value.ToString(); tbTimeStamp.Tag = value; }
        }

        public MessageHeader()
        {
            InitializeComponent();
            User.IsSelected = true;
        }

        private Discord.IUser relatedUser = null;
        public Discord.IUser RelatedUser
        {
            set
            {
                relatedUser = value;

                UpdateRelated();
            }
        }

        public void UpdateRelated()
        {
            //    new Thread(() =>
            //    {
            //        var user = App.MainWnd.client.GetUser(relatedUser.Id);
            //        relatedUser = user;

            Avatar.RelatedUser = relatedUser;
            User.RelatedUser = relatedUser;
            User.IsSelected = true;
            //}).Start();
        }
    }
}
