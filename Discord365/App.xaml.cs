using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Discord365
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UI.LoginWindow LoginWnd;
        public static UI.MainClientWnd MainWnd;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            LoginWnd = new UI.LoginWindow();
            LoginWnd.ShowDialog();
        }
    }
}
