using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace Discord365.CrashHandler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Run_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start(App.IssuesPage);
        }
        

        private void CSave_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog s = new Microsoft.Win32.SaveFileDialog();
            s.Filter = "Text Files (*.txt)|*.txt|Log Files (*.log)|*.log|All Files (*.*)|*.*";

            if ((bool)s.ShowDialog())
            {
                try
                {
                    using (var w = new StreamWriter(s.FileName))
                    {
                        w.Write(BugTb.Text);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Can't save file", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void CSelectAll_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            BugTb.SelectAll();
        }

        private void CCopy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CSelectAll_MouseLeftButtonUp(sender, e);
            BugTb.Copy();
        }
    }
}
