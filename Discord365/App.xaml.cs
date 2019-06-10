using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
        public const string CrashHandlerExe = "Discord365.CrashHandler.exe";

        public static UI.LoginWindow LoginWnd;
        public static UI.MainClientWnd MainWnd;
        public static UI.AppWindow AppWnd;
        public static Discord.WebSocket.DiscordSocketClient Client => MainWnd.client; 

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
            Log.Initialize();

            AppWnd = new UI.AppWindow();
            AppWnd.ShowDialog();
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
                return;

            Log.Dispose();

            StringBuilder b = new StringBuilder();
            b.AppendLine("Unhandled Exception Happened at " + DateTime.Now.ToString());
            b.AppendLine("Send this file there: https://github.com/feel-the-dz3n/Discord365/issues/new");
            b.AppendLine();
            b.AppendLine("Author: " + Environment.UserName);
            b.AppendLine("Receiver: https://github.com/feel-the-dz3n/Discord365/issues");
            b.AppendLine("Topic: " + ((Exception)e.ExceptionObject).Message);
            b.AppendLine();
            b.AppendLine(((Exception)e.ExceptionObject).ToString());

            try
            {
                using(StreamWriter w = new StreamWriter(ErrorLogFile))
                {
                    w.Write(b.ToString());
                }
            }
            catch { }

            try
            {
                if (File.Exists(CrashHandlerExe))
                {
                    string args = "\"base64:" + Base64Encode(b.ToString()) + "\"";

                    if (File.Exists(Log.FileName))
                        args += " \"txt:" + Log.FileName + "\"";

                    Process.Start(CrashHandlerExe, args);
                }
            }
            catch { }

            Environment.Exit(1);
        }
    }
}
