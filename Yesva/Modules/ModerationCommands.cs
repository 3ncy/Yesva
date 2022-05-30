using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Yesva.Modules;

public class ModerationCommands : ModuleBase<SocketCommandContext>
{
    [Command("kick")]
    [RequireUserPermission(GuildPermission.KickMembers, ErrorMessage = "You don't have permission to Kick other members!")]
    [RequireBotPermission(GuildPermission.KickMembers, ErrorMessage = "I don't have the permission to kick others")]
    public async Task Kick(SocketGuildUser user, string? reason = null)
    {
        string result = await Moderation.Kick(user, reason);
        await ReplyAsync(result);
    }
}
