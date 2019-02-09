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
    /// Interaction logic for DMPanelButton.xaml
    /// </summary>
    public partial class DMPanelButton : UserControl
    {
        public enum MarkStyles
        {
            None,
            SelectedServer,
            NewMessages
        }

        private bool serverHasNewMessages = false;
        public bool ServerHasNewMessages
        {
            get => serverHasNewMessages;
            set
            {
                serverHasNewMessages = value;

                MarkStyle = MarkStyle; // update
            }
        }

        private SocketGuild relatedServer = null;

        private MarkStyles markStyle = MarkStyles.None;
        public MarkStyles MarkStyle
        {
            get => markStyle;

            set
            {
                markStyle = value;

                if (value == MarkStyles.None)
                {
                    MarkSelected.Visibility = Visibility.Hidden;

                    if(!ServerHasNewMessages)
                        MarkNewMsg.Visibility = Visibility.Hidden;
                    else
                        MarkNewMsg.Visibility = Visibility.Visible;

                }
                else if (value == MarkStyles.SelectedServer)
                {
                    MarkSelected.Visibility = Visibility.Visible;
                    MarkNewMsg.Visibility = Visibility.Hidden;
                }
                else if (value == MarkStyles.NewMessages)
                {
                    MarkSelected.Visibility = Visibility.Hidden;
                    MarkNewMsg.Visibility = Visibility.Visible;
                }
            }
        }

        public SocketGuild RelatedServer
        {
            get => relatedServer;
            set
            {
                relatedServer = value;

                UpdateRelated();
            }
        }
        public void UpdateRelated()
        {
            if(!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => UpdateRelated());
                return;
            }

            Avatar.ServerName = RelatedServer.Name;
            Avatar.DownloadAndSetAvatar(RelatedServer.IconUrl);
        }

        public DMPanelButton()
        {
            InitializeComponent();

            MarkStyle = MarkStyles.None;
        }
    }
}
