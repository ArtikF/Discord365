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
using Discord.Net;
using Discord.WebSocket;

namespace Discord365.UI.User
{
    /// <summary>
    /// Interaction logic for SidebarUserInfo.xaml
    /// </summary>
    public partial class SidebarUserInfo : UserControl
    {
        public DiscordSocketClient client
        {
            get => App.MainWnd.client;
        }

        public SidebarUserInfo()
        {
            InitializeComponent();
        }

        public SocketUser RelatedUser
        {
            set
            {
                e.RelatedUser = value;
                CurrentUserAvatar.RelatedUser = value;
            }
        }

        private void CurrentUserAvatar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void MenuStatusOnline_Click(object sender, RoutedEventArgs e)
        {
            SetStatus(Discord.UserStatus.Online);
        }

        public async void SetStatus(Discord.UserStatus newstatus)
        {
            await client.SetStatusAsync(newstatus);
        }

        private void MenuStatusAFK_Click(object sender, RoutedEventArgs e)
        {
            SetStatus(Discord.UserStatus.AFK);
        }

        private void MenuStatusDoNotDist_Click(object sender, RoutedEventArgs e)
        {
            SetStatus(Discord.UserStatus.DoNotDisturb);
        }

        private void MenuStatusInvis_Click(object sender, RoutedEventArgs e)
        {
            SetStatus(Discord.UserStatus.Invisible);
        }

        private void BtnWrench_Click(object sender, RoutedEventArgs e)
        {
            App.MainWnd.DiscordWindowContent = MainClientWnd.DiscordWndConent.Custom;
        }
    }
}
