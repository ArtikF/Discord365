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
using static Discord365.UI.Animations;

namespace Discord365.UI.User
{
    /// <summary>
    /// Interaction logic for UserAvatar.xaml
    /// </summary>
    public partial class UserAvatar : UserControl
    {
        public static Color ColorOnline = Color.FromArgb(255, 67, 181, 129);
        public static Color ColorOffline = Color.FromArgb(255, 116, 127, 121);
        public static Color ColorAFK = Color.FromArgb(255, 250, 166, 26);
        public static Color ColorDoNotDisturb = Color.FromArgb(255, 240, 71, 71);

        public UserAvatar()
        {
            InitializeComponent();
            
            App.MainWnd.UpdateAvatars.Add(this);

            UserStatusBorder.Visibility = Visibility.Hidden;
            UserStatusRect.Visibility = Visibility.Hidden;
            UserStatusRect.Fill = new SolidColorBrush(ColorOffline);
        }

        public enum UserOnlineMarks
        {
            None,
            Offline,
            Online,
            AFK,
            DoNotDisturb
        }
        
        private UserOnlineMarks onlineMark = UserOnlineMarks.None;
        private SocketUser relatedUser = null;

        public SocketUser RelatedUser
        {
            get => relatedUser;
            set
            {
                relatedUser = value;

                UpdateRelatedUser();
            }
        }

        public UserOnlineMarks OnlineMark
        {
            get => onlineMark;
            set
            {
                var previous = onlineMark;
                onlineMark = value;

                if (onlineMark == UserOnlineMarks.None)
                {
                    UserStatusBorder.FadeOut(450);
                    UserStatusRect.FadeOut(450);
                }
                else
                {
                    if(previous == UserOnlineMarks.None)
                    {
                        UserStatusBorder.FadeIn(450);
                        UserStatusRect.FadeIn(450);
                    }

                    if (onlineMark == UserOnlineMarks.Offline)
                        UserStatusRect.Fill = new SolidColorBrush(ColorOffline); //UserStatusRect.ColorTransition(((SolidColorBrush)UserStatusRect.Fill).Color, ColorOffline, 500, "Fill.Color");
                    else if (onlineMark == UserOnlineMarks.Online)
                        UserStatusRect.Fill = new SolidColorBrush(ColorOnline); //UserStatusRect.ColorTransition(((SolidColorBrush)UserStatusRect.Fill).Color, ColorOnline, 500, "Fill.Color");
                    else if (onlineMark == UserOnlineMarks.AFK)
                        UserStatusRect.Fill = new SolidColorBrush(ColorAFK); //UserStatusRect.ColorTransition(((SolidColorBrush)UserStatusRect.Fill).Color, ColorAFK, 500, "Fill.Color");
                    else if (onlineMark == UserOnlineMarks.DoNotDisturb)
                        UserStatusRect.Fill = new SolidColorBrush(ColorDoNotDisturb); //UserStatusRect.ColorTransition(((SolidColorBrush)UserStatusRect.Fill).Color, ColorDoNotDisturb, 500, "Fill.Color");
                }

            }
        }

        public void UpdateRelatedUser()
        {
            if(!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => UpdateRelatedUser());
                return;
            }

            if (RelatedUser == null)
                return;

            DownloadAndSetAvatar(RelatedUser.GetAvatarUrl(Discord.ImageFormat.Png, 32));

            if (RelatedUser.Status == Discord.UserStatus.Offline || RelatedUser.Status == Discord.UserStatus.Invisible)
                OnlineMark = UserOnlineMarks.Offline;
            else if (RelatedUser.Status == Discord.UserStatus.Online)
                OnlineMark = UserOnlineMarks.Online;
            else if (RelatedUser.Status == Discord.UserStatus.AFK || RelatedUser.Status == Discord.UserStatus.Idle)
                OnlineMark = UserOnlineMarks.AFK;
            else if (RelatedUser.Status == Discord.UserStatus.DoNotDisturb)
                OnlineMark = UserOnlineMarks.DoNotDisturb;
        }

        public void DownloadAndSetAvatar(string url)
        {
            var bitmap = BitmapFrame.Create(new Uri(url, UriKind.Absolute));

            AvatarImage.ImageSource = bitmap;

            this.FadeIn(500, 1000);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.MainWnd.UpdateAvatars.Contains(this))
                App.MainWnd.UpdateAvatars.Add(this);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (App.MainWnd.UpdateAvatars.Contains(this))
                App.MainWnd.UpdateAvatars.Remove(this);
        }
    }
}
