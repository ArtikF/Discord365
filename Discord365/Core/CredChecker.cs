using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;
using Discord.WebSocket;

namespace Discord365.Core
{
    public class TokenChecker
    {
        DiscordSocketClient client = new DiscordSocketClient();
        string Token;

        public TokenChecker(string token)
        {
            Token = token;
        }

        public async void Check()
        {
            await client.LoginAsync(Discord.TokenType.Bot, Token);
        }
    }
}
