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

    [Command("kick")]
    [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "You don't have permission to Ban other members!")]
    [RequireBotPermission(GuildPermission.BanMembers, ErrorMessage = "I don't have the permission to ban others")]
    public async void Ban(SocketGuildUser user, string? reason = null, int pruneDays = 0)
    {
        string result = await Moderation.Ban(user, reason, pruneDays);
        await ReplyAsync(result);
    }

    //slash commandy jsou lepsi na nektere veci 
    //[Command("mute")]
    //[RequireUserPermission(GuildPermission.MuteMembers, ErrorMessage = "You don't have permission to mute other members!")]
    //[RequireBotPermission(GuildPermission.MuteMembers, ErrorMessage = "I don't have the permission to timeout others")]
    //public async void Mute(SocketGuildUser user, string until)
    //{
    //    Console.WriteLine("wtf bro");
    //    //string result = await Moderation.Timeout(user, until);
    //    //await ReplyAsync(result);
    //}
}
