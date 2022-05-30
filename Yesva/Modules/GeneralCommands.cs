using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yesva.Modules;

public class GeneralCommands : ModuleBase<SocketCommandContext>
{
    [Command("ping")]
    public async Task Ping()
    {
        await ReplyAsync($"Pong! Bot ping: {Context.Client.Latency}ms");

    }


    [Command("goldcoin")]
    [Alias("g")]
    public async Task RemoveGoldcoin(string code)
    {
        var embed = new EmbedBuilder()
            .WithTitle(code.Replace(":goldcoin:", "").Replace("goldcoin", "")).Build();
        await ReplyAsync(embed: embed);
    }

    [Command("info")]
    public async Task UserInfo(SocketGuildUser? socketGuildUser = null)
    {
        if (socketGuildUser is null) socketGuildUser = Context.User as SocketGuildUser;

        var embed = new EmbedBuilder()
            .WithTitle(socketGuildUser.Username + "#" + socketGuildUser.Discriminator);

        if (socketGuildUser.Nickname is not null)
            embed = embed.AddField("Nick", socketGuildUser.Nickname ?? socketGuildUser.Username);

        embed = embed.AddField("ID", socketGuildUser.Id)
                     .AddField("Joined at", socketGuildUser.JoinedAt)
                     .WithThumbnailUrl(socketGuildUser.GetAvatarUrl() ?? socketGuildUser.GetDefaultAvatarUrl())
                     .WithColor(new Color(20, 247, 58));


        await ReplyAsync(embed: embed.Build());
    }
}
