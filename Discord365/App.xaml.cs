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
        public static UI.AppWindow AppWnd;

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppWnd = new UI.AppWindow();
            AppWnd.ShowDialog();
        }
    }
}
