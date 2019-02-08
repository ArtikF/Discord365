using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Logging.Serilog;


namespace Discord365
{
    class Program
    {
        static void Main(string[] args)
        {
            UI.Program.Main(args);
        }


        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<UI.App>()
                .UsePlatformDetect()
                .LogToDebug();
    }
}
