using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Yesva.Services;

internal class InteractionHandler : DiscordClientService
{
    private readonly IServiceProvider _provider;
    private readonly InteractionService _interactionService;
    private readonly IHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public InteractionHandler(DiscordSocketClient client, ILogger<DiscordClientService> logger, IServiceProvider provider, InteractionService interactionService, IHostEnvironment environment, IConfiguration configuration) : base(client, logger)
    {
        _provider = provider;
        _interactionService = interactionService;
        _environment = environment;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Client.InteractionCreated += HandleInteraction;

        //_interactionService.SlashCommandExecuted += SlashCommandExecuted;
        //_interactionService.ContextCommandExecuted += ContextCommandExected;
        //_interactionService.ComponentCommandExecuted += ComponentCommandExecuted;

        await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        await Client.WaitForReadyAsync(stoppingToken);


        //await _interactionService.RegisterCommandsToGuildAsync(548938544790372375);


        if (_environment.IsDevelopment())
        {
            Logger.LogInformation("The bot has started in DEV configuration");
            await _interactionService.RegisterCommandsToGuildAsync(_configuration.GetValue<ulong>("devGuild"));
        }
        else
        {
            Logger.LogInformation("The bot has started in PRODUCTION configuration");
            await _interactionService.RegisterCommandsGloballyAsync();
        }
    }



    private async Task HandleInteraction(SocketInteraction arg)
    {
        try
        {
            var context = new SocketInteractionContext(Client, arg);
            await _interactionService.ExecuteCommandAsync(context, _provider);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "exceptio with interaction");

            if (arg.Type == InteractionType.ApplicationCommand)
            {
                var msg = await arg.GetOriginalResponseAsync();
                await msg.DeleteAsync();
            }
        }
    }
}
