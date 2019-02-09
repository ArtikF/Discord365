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
using static Discord365.UI.Animations;

namespace Discord365.UI
{
    /// <summary>
    /// Interaction logic for MainClientWnd.xaml
    /// </summary>
    public partial class MainClientWnd : Window
    {
        public List<User.UserAvatar> UpdateAvatars = new List<User.UserAvatar>();

        public LoginWindow LoginWnd = null;

        public DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig() { LogLevel = Discord.LogSeverity.Info, MessageCacheSize = 5 });

        public enum DiscordWndConent
        {
            Status,
            Content
        };

        private DiscordWndConent discordWindowContent = DiscordWndConent.Content;
        public DiscordWndConent DiscordWindowContent
        {
            get => discordWindowContent;
            set
            {
                discordWindowContent = value;

                if (value == DiscordWndConent.Content)
                {
                    DiscordContent.Visibility = Visibility.Visible;
                    ContentBlur.Radius = 0;

                    DiscordStatus.FadeOut(700);
                    new Thread(() =>
                    {
                        Thread.Sleep(700);
                        Dispatcher.Invoke(() => DiscordStatus.Visibility = Visibility.Hidden);
                    }).Start();
                }
                else if (value == DiscordWndConent.Status)
                {
                    DiscordStatus.Visibility = Visibility.Visible;
                    DiscordContent.Visibility = Visibility.Visible;
                    ContentBlur.Radius = 1;

                    DiscordStatus.FadeIn(700);
                }
            }
        }

        public MainClientWnd(LoginWindow l)
        {
            LoginWnd = l;
            App.MainWnd = this;

            InitializeComponent();

            Sidebar.CurrentUserInfo.client = client;

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
            foreach(var u in UpdateAvatars)
            {
                if(u.RelatedUser != null && u.RelatedUser.Id == arg1.Id)
                {
                    u.RelatedUser = arg1;
                }
            }

            return Task.CompletedTask;
        }

        private Task Client_CurrentUserUpdated(SocketSelfUser arg1, SocketSelfUser arg2)
        {
            string avatarUrl = arg1.GetAvatarUrl(Discord.ImageFormat.Auto, 32);

            Dispatcher.Invoke(() =>
            {
                Sidebar.CurrentUserInfo.tbUserName.Text = arg1.Username;
                Sidebar.CurrentUserInfo.CurrentUserAvatar.RelatedUser = arg1;
            });

            return Task.CompletedTask;
        }

        private Task Client_Connected()
        {
            SetStatus("Connected");
            return Task.CompletedTask;
        }

        private Task Client_GuildUpdated(SocketGuild arg1, SocketGuild arg2)
        {
            Dispatcher.Invoke(() =>
            {
                foreach(var s in DMPanel.ServerPanel.Children)
                {
                    User.DMPanelButton btn = (User.DMPanelButton)s;

                    if (btn.RelatedServer.Id == arg1.Id)
                        btn.RelatedServer = arg1;
                }
            });

            return Task.CompletedTask;
        }

        private Task Client_GuildUnavailable(SocketGuild arg)
        {
            return Task.CompletedTask;
        }

        private Task Client_GuildAvailable(SocketGuild arg)
        {
            Dispatcher.Invoke(() => 
            {
                DMPanel.AddServer(arg);
            });

            return Task.CompletedTask;
        }

        private Task Client_MessageReceived(SocketMessage arg)
        {
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
            SetStatus("Ready");

            new Thread(() =>
            {
                Thread.Sleep(600);

                Dispatcher.Invoke(() =>
                {
                    DiscordWindowContent = DiscordWndConent.Content;
                });
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

                await client.LoginAsync(type, token, false);
                await client.StartAsync();

            }).Start();
        }

        private bool CanExitNow = false;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
    }
}
