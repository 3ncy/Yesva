using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Yesva.Modules;

public class TestCommands : ModuleBase<SocketCommandContext>
{
    internal static readonly HttpClient httpClient = new HttpClient(); //todo: tohle asi smazat :D

    [Command("t")]
    [RequireOwner]
    public async Task Test()
    {

    }

    [Command("t2")]
    [RequireOwner]
    public async Task Test2()
    {

    }

    [Command("list-interactions")]
    [RequireOwner]
    public async Task ListInteractions()
    {
        var response = await httpClient.GetStringAsync("https://discord.com/api/v10/applications/950776393355120680/commands");
        await ReplyAsync("```json\n" + response + "```");
    }

    [Command("list-guild-interactions")]
    [RequireOwner]
    public async Task ListGuildInteractions(ulong guildId = 0)
    {
        if (guildId == 0) guildId = Context.Guild.Id;
        var response = await httpClient.GetStringAsync("https://discord.com/api/v10/applications/950776393355120680/guilds/" + guildId + "/commands");
        await ReplyAsync("```json\n" + response + "```");

    }


    [Command("delete-interaction")]
    [RequireOwner]
    public async Task DeleteInteraction(ulong interactionId, ulong guildId = 0)
    {
        Uri uri;
        if (guildId == 0) //neni nastavene guild id, takze mazu globalni command
        {
            uri = new Uri("https://discord.com/api/v10/applications/950776393355120680/commands/" + interactionId);
        }
        else
        {
            uri = new Uri("https://discord.com/api/v10/applications/950776393355120680/guilds/" + guildId + "/commands/" + interactionId);
        }

        HttpRequestMessage request = new HttpRequestMessage()
        {
            Method = HttpMethod.Delete,
            RequestUri = uri,
            Content = new StringContent("")
        };
        var response = await httpClient.SendAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            await ReplyAsync("Deletion successful");
        }
        else
        {
            await ReplyAsync(await response.Content.ReadAsStringAsync());
        }
    }
}
