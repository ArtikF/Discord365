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

namespace Discord365.UI.User
{
    /// <summary>
    /// Interaction logic for UserNameUpdateable.xaml
    /// </summary>
    public partial class UserNameUpdateable : UserControl
    {
        public UserNameUpdateable()
        {
            InitializeComponent();

            IsSelected = false;
            ShowAdditional = false;
        }

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

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;

                if (!isSelected)
                    tbUser.Foreground = tbAdditional.Foreground;
                else
                    tbUser.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        public bool ShowAdditional
        {
            get
            {
                if (tbAdditional.Visibility == Visibility.Hidden)
                    return false;
                else
                    return true;
            }
            set
            {
                if (value)
                {
                    tbAdditional.Visibility = Visibility.Visible;
                    panel.Children.Add(tbAdditional);
                }
                else
                {
                    tbAdditional.Visibility = Visibility.Hidden;
                    panel.Children.Remove(tbAdditional);
                }
            }
        }

        private bool isSelected = false;

        public void UpdateRelatedUser()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => UpdateRelatedUser());
                return;
            }

            if (RelatedUser == null)
                return;

            tbUser.Text = RelatedUser.Username;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.MainWnd.UpdateUserNames.Contains(this))
                App.MainWnd.UpdateUserNames.Add(this);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (App.MainWnd.UpdateUserNames.Contains(this))
                App.MainWnd.UpdateUserNames.Remove(this);
        }
    }
}
