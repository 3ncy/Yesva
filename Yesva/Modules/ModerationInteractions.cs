using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yesva.Modules;

//mozna bych mohl implementovat groupu commandu a pridat i dalsi veci
[EnabledInDm(false)]
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

    [SlashCommand("mute", "Mutes user from chat")]
    [RequireUserPermission(GuildPermission.MuteMembers)]
    public async Task Mute(SocketGuildUser user, [Choice("1min", 60000)][Choice("5min", 300000)][Choice("10min", 600000)][Choice("1hour", 3600000)][Choice("1day", 86400000)][Choice("1 week", 604800000)] long duration)
    {
        string result = "";
        try
        {
            //DeferAsync();
            result = await Moderation.Timeout(user, duration);
        }
        catch (Discord.Net.HttpException ex)
        {
            if (ex.Reason == "Missing Permissions")
                result = "I don't have the permission to timeout others";
            else
                result = "Unexpected error happened: " + ex.Reason;
        }

        await RespondAsync(result);
    }
}
