using System.Globalization;
using Discord.Commands;

namespace SaintSeedCs;

public class PingModule : ModuleBase<SocketCommandContext>
{
    // ~say hello world -> hello world

    [Command("ping")]
    [Summary("Replies with pong")]
    public Task SayAsync()
    {
        return ReplyAsync(localization.Pong);
    }


    // ReplyAsync is a method on ModuleBase 
}