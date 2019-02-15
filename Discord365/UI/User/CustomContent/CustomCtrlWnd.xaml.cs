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

namespace Discord365.UI.User.CustomContent
{
    /// <summary>
    /// Interaction logic for CustomCtrlWnd.xaml
    /// </summary>
    public partial class CustomCtrlWnd : UserControl
    {
        public CustomCtrlWnd()
        {
            InitializeComponent();
        }

        public void Set(UIElement content)
        {
            ContentGrid.Children.Clear();

            if (content != null)
                ContentGrid.Children.Add(content);
        }

        public void AddMenuElement(params UIElement[] elements)
        {
            foreach (var element in elements)
            {
                Grid grid = new Grid();
                grid.Children.Add(element);
                grid.Height = 32;
                grid.Width = 192;
                grid.Margin = new Thickness(16, 2, 16, 2);

                if (element is Grid)
                    grid.Tag = ((Grid)element).Tag;


                MenuBox.Items.Add(grid);
            }
        }

        public void RemoveMenuElement(UIElement element)
        {
            foreach(var e in MenuBox.Items)
            {
                Grid grid = e as Grid;

                if (grid.Children[0] == element)
                    MenuBox.Items.Remove(grid);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            App.MainWnd.DiscordWindowContent = MainClientWnd.DiscordWndConent.Content;
        }

        public void SetUserSettingsContent()
        {
            MenuBox.Items.Clear();
            Set(null);

            AddMenuElement(GetLinkGrid("My Account", new Settings.User()));
            AddMenuElement(GetLinkGrid("Debug", new Settings.Debug()));
            AddMenuElement(GetLinkGrid("Log Out", new UserSettings.LogOut()));
            AddMenuElement(GetLinkGrid("About Discord 365", new AboutDiscord365()));
        }

        public Grid GetLinkGrid(string Text, UIElement link)
        {
            Grid grid = new Grid();
            grid.Background = new SolidColorBrush(Colors.Transparent);
            grid.Tag = new LinkTag(link);

            TextBlock b = new TextBlock();
            b.Text = Text;
            b.VerticalAlignment = VerticalAlignment.Center;
            b.HorizontalAlignment = HorizontalAlignment.Left;
            b.Margin = new Thickness(8, 0, 0, 0);
            b.FontSize = 14;
            b.Foreground = new SolidColorBrush(Color.FromArgb(85, 255, 255, 255));

            grid.Children.Add(b);

            return grid;
        }

        private void MenuBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object o = MenuBox.SelectedItem;

            if (o == null)
                return;

            if (o is Grid)
            {
                var g = o as Grid;

                if(g.Tag != null && g.Tag is LinkTag)
                {
                    var tag = g.Tag as LinkTag;

                    Set(tag.link);
                }
            }

        }
    }

    public class LinkTag
    {
        public UIElement link = null;

        public LinkTag(UIElement l)
        {
            link = l;
        }
    }
}
