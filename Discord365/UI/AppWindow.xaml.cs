using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Discord365.UI
{
    /// <summary>
    /// Interaction logic for AppWindow.xaml
    /// </summary>
    public partial class AppWindow : Window
    {
        public MainClientWnd MainWnd = null;

        public enum _Screens
        {
            Login,
            Main
        }

        private _Screens currentScreen = _Screens.Login;
        public _Screens CurrentScreen
        {
            get => currentScreen;
            set
            {
                var previous = currentScreen;
                currentScreen = value;

                if (value == _Screens.Login)
                {
                    // change screen to login

                    if(previous != _Screens.Login)
                        MainGrid.FadeOut(450);

                    new Thread(() =>
                    {
                        if (previous != _Screens.Login)
                            Thread.Sleep(450);

                        Dispatcher.Invoke(() =>
                        {
                            MainGrid.Visibility = Visibility.Hidden;
                            MainGrid.Children.Clear();

                            LoginGrid.Visibility = Visibility.Visible;
                            LoginGrid.FadeIn(450);
                        });
                    }).Start();
                }
                else if (value == _Screens.Main)
                {
                    // change screen to main

                    if (previous != _Screens.Main)
                        LoginGrid.FadeOut(450);

                    new Thread(() =>
                    {
                        if (previous != _Screens.Main)
                            Thread.Sleep(450);

                        Dispatcher.Invoke(() =>
                        {
                            LoginGrid.Visibility = Visibility.Hidden;
                            MainGrid.Visibility = Visibility.Visible;

                            MainGrid.Children.Add(App.MainWnd);
                            App.MainWnd.FadeIn(450);
                        });
                    }).Start();
                }
            }
        }

        public string WindowTitle
        {
            get => this.Title;
            set
            {
                if (value.Length >= 1)
                    this.Title = value + " - Discord 365";
                else
                    this.Title = "Discord 365";
            }
        }

        public AppWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
