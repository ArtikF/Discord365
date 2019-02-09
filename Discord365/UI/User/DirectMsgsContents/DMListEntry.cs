using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Discord365.UI.User.DirectMsgsContents
{
    public class DMListEntry
    {
        public UserAvatar Avatar { get; set; }
        public UserNameUpdateable UserName { get; set; }

        public DMListEntry(UserAvatar a, UserNameUpdateable u)
        {
            Avatar = a;
            UserName = u;
        }

        public WrapPanel Convert()
        {
            WrapPanel g = new WrapPanel() { Orientation = Orientation.Horizontal };

            Avatar.Margin = new System.Windows.Thickness(4);

            g.Children.Add(Avatar);
            g.Children.Add(UserName);

           // g.Width = 224;
            g.Height = 40;

            g.Margin = new System.Windows.Thickness(16);

            g.Tag = this;

            return g;
        }
    }
}
