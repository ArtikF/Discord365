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
    /// Interaction logic for SidebarHeaderSerach.xaml
    /// </summary>
    public partial class SidebarHeaderSerach : UserControl
    {
        public SidebarHeaderSerach()
        {
            InitializeComponent();
        }

        private void TbSearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var s = new Screens.ScreenSearch();
            App.MainWnd.ContentBasic.Set(new TextBlock() { Text = "Search Placeholder, plesase remove it, ok?!", Margin = new Thickness(16, 0, 0, 0), Foreground = new SolidColorBrush(Colors.White), FontSize = 14, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center }, s);
            s.tbSearchBox.Text = tbSearchText.Text;
        }
    }
}
