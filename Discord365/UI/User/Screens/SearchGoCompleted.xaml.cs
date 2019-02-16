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

namespace Discord365.UI.User.Screens
{
    /// <summary>
    /// Interaction logic for SearchGoCompleted.xaml
    /// </summary>
    public partial class SearchGoCompleted : UserControl
    {
        public SearchGoCompleted()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            App.MainWnd.Sidebar.Set(null, null);
            App.MainWnd.ContentBasic.Set(new ScreenSearchHeader(), new ScreenSearch());
        }
    }
}
