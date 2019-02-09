using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Discord.WebSocket;

namespace Discord365.UI.User.DirectMsgsContents
{
    public class DMListEntry
    {
        public SocketDMChannel DMChannel = null;
        public ISocketPrivateChannel PrivateChannel = null;
        public SocketGroupChannel GroupChannel = null;

        public UserAvatar Avatar { get; set; }
        public UserNameUpdateable UserName { get; set; }

        public DMListEntry(UserAvatar a, UserNameUpdateable u)
        {
            Avatar = a;
            UserName = u;
        }

        public Grid Convert()
        {
            Grid g = new Grid();
            
            g.Children.Add(Avatar);

            Avatar.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            Avatar.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Avatar.Margin = new System.Windows.Thickness(16, 0, 0, 0);
            Avatar.Width = 32;
            Avatar.Height = 32;

            UserName.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            UserName.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            UserName.Margin = new System.Windows.Thickness(64, 0, 0, 0);

            g.Children.Add(UserName);

           // g.Width = 224;
            //g.Height = 40;

            g.Margin = new System.Windows.Thickness(16);

            g.Tag = this;

            return g;
        }
    }
}
