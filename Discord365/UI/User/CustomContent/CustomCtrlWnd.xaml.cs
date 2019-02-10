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

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            App.MainWnd.DiscordWindowContent = MainClientWnd.DiscordWndConent.Content;
        }
    }
}
