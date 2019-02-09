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

namespace Discord365.UI.User.DirectMsgsContents
{
    /// <summary>
    /// Interaction logic for DirectMessagesSidebarContent.xaml
    /// </summary>
    public partial class DirectMessagesSidebarContent : UserControl
    {
        public DirectMessagesSidebarContent()
        {
            InitializeComponent();

            var avatar = new UserAvatar();
            avatar.OnlineMark = UserAvatar.UserOnlineMarks.Offline;
            var username = new UserNameUpdateable();
            username.tbUser.Text = "Friends";

            var friends = new DMListEntry(avatar, username);

            Add(friends);
        }

        public void Add(DMListEntry e)
        {
            DMList.Items.Add(e.Convert());
        }
    }
}
