using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yesva.Modules;

public class ModerationInteractions : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("kick", "Kicks user from the guild.")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    //[RequireBotPermission(GuildPermission.KickMembers)]
    public async Task Kick(SocketGuildUser user, string? reason = null)
    {
        string result = "";
        try
        {
            result = await Moderation.Kick(user, reason);
        }
        catch (Discord.Net.HttpException ex)
        {
            if (ex.Reason == "Missing Permissions")
                result = "I don't have the permission to kick others";
        }

        await RespondAsync(result);
    }

    [SlashCommand("ban", "Bans user form the guild.")]
    [RequireUserPermission(GuildPermission.BanMembers)]
    public async Task Ban(SocketGuildUser user, string? reason = null, int pruneDays = 0)
    {
        pruneDays = Math.Clamp(pruneDays, 0, 7);

        string result = "";
        try
        {
            result = await Moderation.Ban(user, reason, pruneDays);
        }
        catch (Discord.Net.HttpException ex)
        {
            if (ex.Reason == "Missing Permissions")
                result = "I don't have the permission to ban others";
        }

        await RespondAsync(result);
    }
}
