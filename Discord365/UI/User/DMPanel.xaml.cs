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
using Discord.Net.WebSockets;
using Discord.WebSocket;

namespace Discord365.UI.User
{
    /// <summary>
    /// Interaction logic for DMPanel.xaml
    /// </summary>
    public partial class DMPanel : UserControl
    {
        public DMPanelButton btnDirect;
        public DMPanelButton btnSearch;

        public DMPanelButton[] AvailableButtons
        {
            get
            {
                List<DMPanelButton> result = new List<DMPanelButton>();


                foreach (var dm in PanelDM.Children)
                {
                    DMPanelButton b = dm as DMPanelButton;
                    result.Add(b);
                }

                foreach (var dm in ServerPanel.Children)
                {
                    DMPanelButton b = dm as DMPanelButton;
                    result.Add(b);
                }

                return result.ToArray();
            }
        }

        private DMPanelButton selected = null;
        public DMPanelButton Selected
        {
            get => selected;
            set
            {
                SetSelected(value);
            }
        }
        public void SetSelected(DMPanelButton btn)
        {
            selected = btn;

            foreach (var dm in AvailableButtons)
            {
                DMPanelButton b = dm as DMPanelButton;

                if (b.MarkStyle != DMPanelButton.MarkStyles.NewMessages)
                    b.MarkStyle = DMPanelButton.MarkStyles.None;

                if (btn != null && b == btn)
                    b.MarkStyle = DMPanelButton.MarkStyles.SelectedServer;
            }
        }

        public void AddServer(SocketGuild server)
        {
            RemoveServer(server);

            DMPanelButton b = new DMPanelButton();

            b.Width = 69;
            b.Height = 50;
            b.Margin = new Thickness(0, 4, 0, 4);

            b.RelatedServer = server;

            ServerPanel.Children.Add(b);

            b.MouseLeftButtonUp += B_MouseLeftButtonUp;
        }

        private void B_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DMPanelButton b = (DMPanelButton)sender;
            SetSelected(b);
            
            App.MainWnd.Sidebar.Set(null, new ServerContent.ServerLeftSidebar() { RelatedServer =  b.RelatedServer });
            App.MainWnd.ContentBasic.Set(null, new Screens.ScreenWelcomeDM());
        }

        public void RemoveServer(SocketGuild server)
        {
            for(int i = 0; i < ServerPanel.Children.Count; i++)
            {
                var s = ServerPanel.Children[i];

                if (((DMPanelButton)s).RelatedServer.Id == server.Id)
                    ServerPanel.Children.Remove((UIElement)s);
            }
        }

        public DMPanel()
        {
            InitializeComponent();

            btnDirect = new DMPanelButton();
            btnDirect.Avatar.ServerName = "Direct Messages";
            btnDirect.Margin = new Thickness(0, 4, 0, 4);
            btnDirect.Width = 69;
            btnDirect.Height = 50;
            btnDirect.MarkStyle = DMPanelButton.MarkStyles.None;
            btnDirect.MouseLeftButtonUp += BtnDirect_MouseLeftButtonUp;
            PanelDM.Children.Add(btnDirect);

            btnSearch = new DMPanelButton();
            btnSearch.Avatar.ServerName = "Search";
            btnSearch.Margin = new Thickness(0, 4, 0, 4);
            btnSearch.Width = 69;
            btnSearch.Height = 50;
            btnSearch.MarkStyle = DMPanelButton.MarkStyles.None;
            btnSearch.MouseLeftButtonUp += BtnSearch_MouseLeftButtonUp;
            PanelDM.Children.Add(btnSearch);
        }

        private void BtnSearch_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetSelected(null);

            App.MainWnd.Sidebar.Set(null, null);
            App.MainWnd.ContentBasic.Set(new Screens.ScreenSearchHeader(), new Screens.ScreenSearch());
        }

        private void BtnDirect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetSelected(btnDirect);

            App.MainWnd.Sidebar.Set(new DirectMsgsContents.SidebarHeaderSerach(), new DirectMsgsContents.DirectMessagesSidebarContent());
            App.MainWnd.ContentBasic.Set(null, new Screens.ScreenWelcomeDM());
        }

        private void WrapPanel_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        private void ScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }
    }
}
