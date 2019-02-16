using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Discord.Net;
using Discord.WebSocket;
using static Discord365.UI.Extensions;

namespace Discord365.UI
{
    /// <summary>
    /// Interaction logic for MainClientWnd.xaml
    /// </summary>
    public partial class MainClientWnd : UserControl
    {
        public const int ColumnServersWidth = 69;
        public const int ColumnLeftSidebarWidth = 240;

        public List<User.UserAvatar> UpdateAvatars = new List<User.UserAvatar>();
        public List<User.UserNameUpdateable> UpdateUserNames = new List<User.UserNameUpdateable>();
        
        public DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig() { LogLevel = Discord.LogSeverity.Info, MessageCacheSize = 50 });

        public enum DiscordWndConent
        {
            Status,
            Content,
            Custom
        };

        public DiscordWndConent PreviousContent = DiscordWndConent.Content;
        private DiscordWndConent discordWindowContent = DiscordWndConent.Content;
        public DiscordWndConent DiscordWindowContent
        {
            get => discordWindowContent;
            set
            {
                if (discordWindowContent == value)
                    return;

                PreviousContent = discordWindowContent;
                discordWindowContent = value;

                if (PreviousContent == DiscordWndConent.Status)
                {
                    DiscordStatus.FadeOut(FadeAnimationsDefaultTime);
                    new Thread(() =>
                    {
                        Thread.Sleep(FadeAnimationsDefaultTime);
                        Dispatcher.Invoke(() => DiscordStatus.Visibility = Visibility.Hidden);
                    }).Start();
                }
                else if (PreviousContent == DiscordWndConent.Custom)
                {
                    DiscordCustom.FadeOut(FadeAnimationsDefaultTime);
                    new Thread(() =>
                    {
                        Thread.Sleep(FadeAnimationsDefaultTime);
                        Dispatcher.Invoke(() => DiscordCustom.Visibility = Visibility.Hidden);
                    }).Start();
                }

                if (value == DiscordWndConent.Content)
                {
                    DiscordContent.Visibility = Visibility.Visible;
                    ContentBlur.Radius = 0;
                }
                else if (value == DiscordWndConent.Status)
                {
                    DiscordStatus.Visibility = Visibility.Visible;
                    // ContentBlur.Radius = 7;

                    DiscordStatus.FadeInSize(FadeAnimationsDefaultTime);
                }
                else if (value == DiscordWndConent.Custom)
                {
                    CustomCtrl.Set(null);
                    CustomCtrl.MenuBox.Items.Clear();

                    DiscordCustom.Visibility = Visibility.Visible;
                    ContentBlur.Radius = 7;

                    DiscordCustom.FadeInSize(FadeAnimationsDefaultTime);
                }
            }
        }

        public MainClientWnd()
        {
            App.MainWnd = this;

            InitializeComponent();

            ContentBasic.Set(null, null);
            Sidebar.Set(null, null);
            
            discordWindowContent = DiscordWndConent.Status;
            DiscordContent.Visibility = Visibility.Visible;
            DiscordStatus.Visibility = Visibility.Visible;
            DiscordCustom.Visibility = Visibility.Hidden;

            client.LoggedIn += Client_LoggedIn;
            client.Ready += Client_Ready;
            client.Disconnected += Client_Disconnected;
            client.Connected += Client_Connected;
            client.Log += Client_Log;
            client.LoggedOut += Client_LoggedOut;
            client.MessageReceived += Client_MessageReceived;
            client.GuildAvailable += Client_GuildAvailable;
            client.GuildUnavailable += Client_GuildUnavailable;
            client.GuildUpdated += Client_GuildUpdated;

            client.CurrentUserUpdated += Client_CurrentUserUpdated;
            client.UserUpdated += Client_UserUpdated;
        }

        private Task Client_UserUpdated(SocketUser arg1, SocketUser arg2)
        {
            new Thread(() =>
            {
                foreach (var u in UpdateAvatars)
                {
                    if (u.RelatedUser != null && u.RelatedUser.Id == arg1.Id)
                    {
                        u.RelatedUser = arg1;
                    }
                }
            }).Start();

            return Task.CompletedTask;
        }

        private Task Client_CurrentUserUpdated(SocketSelfUser arg1, SocketSelfUser arg2)
        {
            new Thread(() => 
            {
                string avatarUrl = arg1.GetAvatarUrl(Discord.ImageFormat.Png, 32);

                Dispatcher.Invoke(() =>
                {
                    Sidebar.CurrentUserInfo.RelatedUser = arg1;
                    Sidebar.CurrentUserInfo.e.User.IsSelected = true;
                    Sidebar.CurrentUserInfo.e.User.ShowAdditional = true;
                });
            }).Start();

            return Task.CompletedTask;
        }

        private Task Client_Connected()
        {
            SetStatus("Connected");
            return Task.CompletedTask;
        }

        private Task Client_GuildUpdated(SocketGuild arg1, SocketGuild arg2)
        {
            new Thread(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    foreach (var s in DMPanel.ServerPanel.Children)
                    {
                        User.DMPanelButton btn = (User.DMPanelButton)s;

                        if (btn.RelatedServer.Id == arg1.Id)
                            btn.RelatedServer = arg1;
                    }
                });
            }).Start();

            return Task.CompletedTask;
        }

        private Task Client_GuildUnavailable(SocketGuild arg)
        {
            new Thread(() =>
            {
                Dispatcher.Invoke(() => 
                {
                    DMPanel.RemoveServer(arg);
                });
            }).Start();

            return Task.CompletedTask;
        }

        private Task Client_GuildAvailable(SocketGuild arg)
        {
            new Thread(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    DMPanel.AddServer(arg);
                });
            }).Start();

            return Task.CompletedTask;
        }

        private Task Client_MessageReceived(SocketMessage arg)
        {
            new Thread(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    foreach (var c in ContentBasic.GridContent.Children)
                    {
                        if (c is User.MessagesPage.MessagesPageBody)
                        {
                            var b = c as User.MessagesPage.MessagesPageBody;

                            if ((b.Channel != null && b.Channel.Id == arg.Channel.Id) || (b.DmBotChannel != null && b.DmBotChannel.Id == arg.Channel.Id))
                            {
                                var m = new User.MessagesPage.Message.Message();
                                m.RelatedUser = arg.Author;
                                m.AppendMessage(arg);

                                b.AddMessage(m);
                            }
                        }
                    }
                });
            }).Start();

            return Task.CompletedTask;
        }

        private Task Client_LoggedOut()
        {
            DiscordWindowContent = DiscordWndConent.Status;
            SetStatus("Logged out");

            return Task.CompletedTask;
        }

        private Task Client_Log(Discord.LogMessage arg)
        {
            Debugger.Log(0, Debugger.DefaultCategory, arg.ToString() + Environment.NewLine);
            return Task.CompletedTask;
        }

        private Task Client_Disconnected(Exception arg)
        {
            CanExitNow = true;

            Dispatcher.Invoke(() =>
            {
                DiscordWindowContent = DiscordWndConent.Status;
                SetStatus("Disconnected: " + arg.ToString());
            });

            return Task.CompletedTask;
        }

        private Task Client_Ready()
        {

            new Thread(() =>
            {
                SetStatus("Ready");

                Dispatcher.Invoke(() =>
                {
                    DiscordWindowContent = DiscordWndConent.Content;
                });

                Thread.Sleep(250);

                Dispatcher.Invoke(() => ContentBasic.Set(null, new User.Screens.ScreenWelcomeDiscord365()));
            }).Start();

            Client_CurrentUserUpdated(client.CurrentUser, null);

            return Task.CompletedTask;
        }

        private Task Client_LoggedIn()
        {
            SetStatus("Logged in");

            return Task.CompletedTask;
        }

        public void SetStatus(string text)
        {
            if(!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => SetStatus(text));
                return;
            }

            //DiscordWindowContent = DiscordWndConent.Status;

            tbDiscordStatus.Text = text;
        }

        public void StartConnection(string token, Discord.TokenType type)
        {
            new Thread(async () =>
            {
                SetStatus("Logging in...");

                try
                {
                    await client.LoginAsync(type, token, false);
                    await client.StartAsync();
                }
                catch (Exception ex)
                {
                    CanExitNow = true;
                    Dispatcher.Invoke(() => App.AppWnd.CurrentScreen = AppWindow._Screens.Login);
                    App.LoginWnd.Dispatcher.Invoke(() => App.LoginWnd.SetError(ex.Message));
                }

            }).Start();
        }

        public bool CanExitNow = false;

        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CanExitNow)
                e.Cancel = true;
            else
                return;

            if (client.ConnectionState == Discord.ConnectionState.Disconnecting || client.LoginState == Discord.LoginState.LoggingOut
                || client.ConnectionState == Discord.ConnectionState.Connecting || client.LoginState == Discord.LoginState.LoggingIn)
            {
                SetStatus("Client is busy. Please wait...");
                return;
            }
            
            if(!CanExitNow)
                CloseWndAfterConnections();

            if (!CanExitNow)
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        private void CloseWndAfterConnections()
        {
            DiscordWindowContent = DiscordWndConent.Status;

            SetStatus("Please wait...");

            if (client.ConnectionState == Discord.ConnectionState.Connected)
            {
                if (client.LoginState == Discord.LoginState.LoggedIn)
                {
                    SetStatus("Logging out");
                    //LogOut().GetAwaiter().GetResult();
                }

                SetStatus("Disconneting");
                //StopAsync().GetAwaiter().GetResult();
            }

            SetStatus("Finalizing...");
            //client.Dispose();

            CanExitNow = true;
        }

        public async Task LogOut() => await client.LogoutAsync();
        public async Task StopAsync() => await client.StopAsync();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            App.AppWnd.WindowTitle = "";
        }

        private void ColumnServers_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void ColumnSidebarLeft_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void TbQuestions_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // <user:GitHubIssuesCtrl Height="240" Margin="0,8,0,0"></user:GitHubIssuesCtrl>

            DiscordStatusPanel.Children.Remove(tbQuestions);

            var issues = new User.GitHubIssuesCtrl();
            issues.Height = 240;
            issues.Margin = new Thickness(0, 8, 0, 0);
            DiscordStatusPanel.Children.Add(issues);
        }
    }
}
