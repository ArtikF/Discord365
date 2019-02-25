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

namespace Discord365.UI.User.CustomContent.Settings
{
    /// <summary>
    /// Interaction logic for Debug.xaml
    /// </summary>
    public partial class Debug : UserControl
    {
        public Debug()
        {
            InitializeComponent();
        }

        private void BtnAttachDebug_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debugger.Launch();
        }

        private void BtnCrashApp_Click(object sender, RoutedEventArgs e)
        {
            throw new Exception("Manually crashed application.");
        }
    }
}
