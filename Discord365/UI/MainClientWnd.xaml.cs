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

namespace Discord365.UI
{
    /// <summary>
    /// Interaction logic for MainClientWnd.xaml
    /// </summary>
    public partial class MainClientWnd : Window
    {
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
                    DiscordStatus.Visibility = Visibility.Hidden;
                    DiscordContent.Visibility = Visibility.Visible;
                }
                else if (value == DiscordWndConent.Status)
                {
                    DiscordStatus.Visibility = Visibility.Visible;
                    DiscordContent.Visibility = Visibility.Hidden;
                }
            }
        }

        public MainClientWnd(LoginWindow l)
        {
            LoginWnd = l;

            InitializeComponent();

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
        }

        private Task Client_CurrentUserUpdated(SocketSelfUser arg1, SocketSelfUser arg2)
        {
            string avatarUrl = arg1.GetAvatarUrl(Discord.ImageFormat.Auto, 32);

            Dispatcher.Invoke(() =>
            {
                Sidebar.CurrentUserInfo.tbUserName.Text = arg1.Username;
                Sidebar.CurrentUserInfo.CurrentUserAvatar.DownloadAndSetAvatar(avatarUrl);
            });

            return Task.CompletedTask;
        }

        private Task Client_Connected()
        {
            SetStatus("Connected to Discord!");
            return Task.CompletedTask;
        }

        private Task Client_GuildUpdated(SocketGuild arg1, SocketGuild arg2)
        {
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
            SetStatus("Logged out...");

            return Task.CompletedTask;
        }

        private Task Client_Log(Discord.LogMessage arg)
        {
            Debugger.Log(0, Debugger.DefaultCategory, arg.ToString() + Environment.NewLine);
            return Task.CompletedTask;
        }

        private Task Client_Disconnected(Exception arg)
        {
            MessageBox.Show(arg.ToString(), "Disconnected");

            this.Close();

            return Task.CompletedTask;
        }

        private Task Client_Ready()
        {
            SetStatus("Ready!");

            new Thread(() => 
            {
                Thread.Sleep(1000);

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
            SetStatus("Logged in...");

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

        public void StartConnection(string token)
        {
            new Thread(async () =>
            {
                SetStatus("Welcome to Discord 365!");

                await client.LoginAsync(Discord.TokenType.Bot, token);
                await client.StartAsync();

            }).Start();
        }
    }
}
