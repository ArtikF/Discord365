using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
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
        public const string ErrorLogFile = "discord365_crash.log";

        public static UI.LoginWindow LoginWnd;
        public static UI.MainClientWnd MainWnd;
        public static UI.AppWindow AppWnd;
        public static Discord.WebSocket.DiscordSocketClient client => MainWnd.client; 

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            AppWnd = new UI.AppWindow();
            AppWnd.ShowDialog();
        }

        private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
                return;

            try
            {
                using(StreamWriter w = new StreamWriter(ErrorLogFile))
                {
                    w.WriteLine("Unhandled Exception Happened at " + DateTime.Now.ToString());
                    w.WriteLine("Send this file there: https://github.com/discord365/Discord365/issues/new");
                    w.WriteLine();
                    w.WriteLine("Author: " + Environment.UserName);
                    w.WriteLine("Receiver: https://github.com/discord365/Discord365/issues");
                    w.WriteLine("Topic: " + ((Exception)e.ExceptionObject).Message);
                    w.WriteLine();
                    w.WriteLine(((Exception)e.ExceptionObject).ToString());
                }

                var dialog = MessageBox.Show($"Discord 365 just made a big WHOOPSIE.\r\n\r\nCrash information and instructions are saved in {ErrorLogFile}.\r\n\r\nPress 'Yes' to close the application and open this file.\r\nPress 'No' if you want to start the default debugger.", "Discord 365 Crashed", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (dialog == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start(ErrorLogFile);
                    Environment.Exit(1);
                }
            }
            catch { }
        }
    }
}
