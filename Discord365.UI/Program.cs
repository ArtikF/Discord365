using System;
using Avalonia;
using Avalonia.Logging.Serilog;
using Discord365.UI.ViewModels;
using Discord365.UI.Views;

namespace Discord365.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}
