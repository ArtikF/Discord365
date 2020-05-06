# Why Discord365 is bad?
1. Violates Discord rules
2. Targets outdated framework and required Windows 10
3. Bad code 

# Discord 365 [![Build status](https://ci.appveyor.com/api/projects/status/ri96nriwgo5ehpj7?svg=true)](https://ci.appveyor.com/project/feel-the-dz3n/discord365)
Discord 365 - is a Discord API client for Windows. .NET Framework 4.7.1 is required. Powered by [Discord.Net](https://github.com/discord-net/Discord.Net).

### Modified Discord.Net
Discord365 solution contains Discord.Net projects. They are [original](https://github.com/discord-net/Discord.Net), but ours has enabled self-botting feature. See section below for details.

*UPDATE:* Discord.Net has been retargeted to .NET 4.7.1 here.

# Self-botting
This program allows you to sign in using your account token, but it's **forbidden** by Discord ToS. So if you are violating the rules, your account can be **banned**.

# Purpose
The purpose of Discord 365 is to show how native application can be faster than Electron/Chromium crafts, even with better UI experience in some areas.

# Known Issues
They are listed [here](https://github.com/discord365/Discord365/issues).

# Compiling
## Discord.Net
See [Discord.Net](https://github.com/discord-net/Discord.Net)'s compiling section in README.md.

## Discord 365
 - Visual Studio 2017 with .NET Framework 4.7.1 and .NET Core stuff is required.
 - Before compiling Discord 365 for the first time, you have to rebuild Discord.Net
 
### Rebuilding Discord.Net
 - Go to Configuration Manager
 - Allow to build all Discord.Net projects for the desired configuration
 - Hit "Rebuild Solution"
 - After rebuilding it once, if you don't need to rebuild it again every time when you building Discord 365, you can disable all Discord.Net projects in Configuration Manager again.

# Credits
 - [Dz3n](https://github.com/feel-the-dz3n)

Discord 365 is powered by [Discord.Net](https://github.com/discord-net/Discord.Net) and [Octokit](https://github.com/octokit/octokit.net).

*it will not work on [ReactOS](https://github/reactos/reactos) unless they have .NET 4.7.1 and WPF*
