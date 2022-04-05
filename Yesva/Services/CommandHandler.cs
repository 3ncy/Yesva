using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Yesva.Services
{
    public class CommandHandler : DiscordClientService
    {
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _provider;
        private readonly CommandService _commandService;
        private readonly IConfiguration _configuration;

        public CommandHandler(DiscordSocketClient client, Microsoft.Extensions.Logging.ILogger<CommandHandler> logger, IServiceProvider provider, CommandService commandService, IConfiguration configuration) : base(client, logger)
        {
            _client = client;
            _provider = provider;
            _commandService = commandService;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Client.MessageReceived += OnMessageRecieved;
            _commandService.CommandExecuted += CommandExecutedAsync;

            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

        private async Task CommandExecutedAsync(Optional<CommandInfo> commandInfo, ICommandContext commandContext, IResult result)
        {
            if (result.IsSuccess || result.Error == CommandError.UnknownCommand) return;
            await commandContext.Channel.SendMessageAsync(result.ErrorReason);
        }

        private async Task OnMessageRecieved(SocketMessage socketMessage)
        {
            if (socketMessage is not SocketUserMessage message) return;
            if (message.Source != MessageSource.User) return;

            var argPos = 0;
            if (!message.HasStringPrefix(_configuration["prefix"] + " ", ref argPos)
                && !message.HasStringPrefix(_configuration["prefix"], ref argPos)
                && !message.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;

            var context = new SocketCommandContext(_client, message);
            await _commandService.ExecuteAsync(context, argPos, _provider);
        }
    }
}