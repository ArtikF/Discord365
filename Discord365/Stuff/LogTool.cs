using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord365
{
    public class Log
    {
        private static string fileName = "";
        private static StreamWriter writer = null;

        public static string FileName => fileName;

        public static void Dispose()
        {
            try
            {
                writer.Dispose();
                writer = null;
            }
            catch { }
        }

        public static void Write(string text)
        {
            if (Debugger.IsAttached)
                Debugger.Log(0, Debugger.DefaultCategory, text);

            if (writer != null)
                writer.Write(text);
        }

        public static void WriteLine(string text)
        {
            Write(text + Environment.NewLine);
        }

        public static void Initialize()
        {
            try
            {
                // fileName = new FileInfo("discord365-" + DateTime.Now.ToString().Replace(':', '.').Replace(';', '.') + ".log").FullName;
                fileName = new FileInfo("discord365.log").FullName;
                writer = new StreamWriter(FileName)
                {
                    AutoFlush = true
                };
            }
            catch { }
        }
    }
}
