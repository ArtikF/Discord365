using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Discord365.CrashHandler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string IssuesPage = "https://github.com/Discord365/Discord365/issues";
        public const string IssuesPageNew = "https://github.com/Discord365/Discord365/issues/new";

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string DecodedText = "Something went wrong with Base64 decoder, so let's think that this program crashed too.\r\n\r\n\r\nStupid script kiddies. >:|";
        public static string[] Args = { };

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Args = e.Args;

            if (Args.Length <= 0)
            {
                Process.Start(IssuesPage);
                Environment.Exit(0);
            }

            try
            {
                DecodedText = Base64Decode(Args[0]);
            }
            catch { }

            var wnd = new MainWindow();
            wnd.BugTb.Text = DecodedText;
            wnd.ShowDialog();
            Environment.Exit(0);
        }
    }
}
