using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Yesva.Modules;

public class GeneralInteractions : InteractionModuleBase<SocketInteractionContext>
{
    //[SlashCommand("ping", "Pings the bot")]
    //public async Task PingCommand()
    //{
    //    await RespondAsync("pong");
    //}


    private static DateTime lastMemeCall; //musi byt static, protoze lifetime teto tridy je jenom po dobu volaneho commandu
    [SlashCommand("meme", "Send a meme")]
    //mozna pridat required permission na "can send images"
    public async Task Meme()
    {
        
        if (DateTime.Now.Subtract(lastMemeCall) < TimeSpan.FromSeconds(10))
        {
            await RespondAsync("you must wait atleast 10 seconds after using this command!", ephemeral: true);
            return;
        }

        lastMemeCall = DateTime.Now;

        DeferAsync();

        Random random = new Random();

        string[] subreddits = new string[] { "memes", "dankmemes", "funny" };

        var response = await TestCommands.httpClient.GetStringAsync($"https://www.reddit.com/r/{subreddits[random.Next(subreddits.Length)]}.json?limit=100");
        //var response = await TestCommands.httpClient.GetStringAsync($"https://www.reddit.com/r/ssps.json?limit=100");


        dynamic responseJson = JObject.Parse(response);
        dynamic posts = responseJson.data.children;



        dynamic randomPost = posts[random.Next(posts.Count)];
        //dynamic randomPost = posts[3];

        var embed = new EmbedBuilder()
            //.WithTitle($"[{posts[0].data.title.ToString()}]({link})");
            .WithDescription($"[{randomPost.data.title.ToString()}](https://reddit.com{randomPost.data.permalink.ToString()})")
            .WithColor(new Color(20, 247, 58));

        if (randomPost.data.is_video.ToString() == "True")
        {
            embed = embed.WithImageUrl(randomPost.data.thumbnail.ToString());
            embed.AddField("\u200B",$"[To watch this video, click here]({randomPost.data.secure_media.reddit_video.fallback_url.ToString()})", true);
        }
        else
        {
            embed = embed.WithImageUrl(randomPost.data.url.ToString());
        }

        await ModifyOriginalResponseAsync(m => m.Embed = embed.Build());
    }
}
