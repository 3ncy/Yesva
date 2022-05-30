using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Yesva.Services;

namespace Yesva;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureDiscordHost((context, config) =>
            {
                config.SocketConfig = new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Verbose,
                    AlwaysDownloadUsers = true,
                    MessageCacheSize = 200,
                    //GatewayIntents = Discord.GatewayIntents.All
                    GatewayIntents = Discord.GatewayIntents.AllUnprivileged
                                     | Discord.GatewayIntents.GuildPresences
                                     | Discord.GatewayIntents.GuildMembers
                                     | Discord.GatewayIntents.GuildMessages
                };

                config.Token = context.Configuration["token"];
            })
            .UseCommandService((context, config) =>
             {
                 config.CaseSensitiveCommands = false;
                 config.LogLevel = Discord.LogSeverity.Debug;
                 config.DefaultRunMode = Discord.Commands.RunMode.Async;  //mozna bude needed, ale nevim jak to bude zvladat moje RPi
             })
            .UseInteractionService((context, config) =>
             {
                 config.LogLevel = LogSeverity.Info;
                 config.UseCompiledLambda = true;
             })
            .ConfigureServices((context, services) =>
             {
                 services.AddHostedService<Services.CommandHandler>();
                 services.AddHostedService<Services.InteractionHandler>();
             }).Build();

        

        Modules.TestCommands.httpClient.DefaultRequestHeaders.Add("Authorization", "Bot " 
            + new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false, true).Build()["token"]); //trosku dirty, mozna by to chtelo predelat, ale nevim jestli to jde elegantneji

        await host.RunAsync();
    }
}