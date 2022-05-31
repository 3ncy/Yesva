using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Yesva.Modules;

internal class Moderation
{
    internal static async Task<string> Kick(SocketGuildUser user, string? reason = null)
    {
        await user.KickAsync(reason);        
        return $"User {user.DisplayName}#{user.Discriminator} has been kicked" + ((reason != "")?$" with the reason `{reason}`!":"!");
    }

    internal static async Task<string> Ban(SocketGuildUser user, string? reason = null, int pruneDays = 0)
    {
        await user.Guild.AddBanAsync(user, pruneDays, reason);
        return $"User {user.DisplayName}#{user.Discriminator} has been banned" + ((reason != "") ? $" with the reason `{reason}`!" : "!");
    }

    internal static async Task<string> Timeout(SocketGuildUser user, long duration)
    {
        TimeSpan time = TimeSpan.FromMilliseconds(duration);
        await user.SetTimeOutAsync(time);
        return $"User {user.DisplayName}#{user.Discriminator} has been timed out untill " + (DateTime.Now +  time);
    }
}
