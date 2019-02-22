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

namespace Discord365.UI.User.CustomContent.UserSettings
{
    /// <summary>
    /// Interaction logic for Appearance.xaml
    /// </summary>
    public partial class Appearance : UserControl
    {
        public Appearance()
        {
            InitializeComponent();

            DoNotUseMarkdown.IsChecked = Properties.Settings.Default.PlainTextInsteadOfMarkdown;
        }

        private void DoNotUseMarkdown_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.PlainTextInsteadOfMarkdown = (bool)DoNotUseMarkdown.IsChecked;
            Properties.Settings.Default.Save();
        }
    }
}
