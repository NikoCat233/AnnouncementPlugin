using Impostor.Api.Events.Player;
using Impostor.Api.Events;
using Microsoft.Extensions.Logging;
using Impostor.Api.Net.Inner.Objects;
using System.Text;
using System.Text.Json;
using Impostor.Api.Innersloth;
using Impostor.Hazel.Abstractions;
using Impostor.Api.Innersloth.Customization;
using System;

namespace ChatAnnouncePlugin
{
    public class ChatAnnounceEventListener : IEventListener
    {
        private readonly ILogger<ChatAnnouncePlugin> _logger;
        private Config _config;
        private List<IInnerPlayerControl> list_announced = new List<IInnerPlayerControl>();

        public ChatAnnounceEventListener(ILogger<ChatAnnouncePlugin> logger,Config config)
        {
            _logger = logger;
            _config = config;
        }

        [EventListener]
        public void OnPlayerSpawned(IPlayerSpawnedEvent e)
        {
            _logger.LogInformation("Player {player} > spawned", e.PlayerControl.PlayerInfo.PlayerName);

            var clientPlayer = e.ClientPlayer;
            var playerControl = e.PlayerControl;

            Task.Run(async () =>
            {
                // Give the player time to load.
                await Task.Delay(TimeSpan.FromSeconds(3));

                bool messageSend = false;
                while (clientPlayer.Client.Connection != null && clientPlayer.Client.Connection.IsConnected && !messageSend)
                {
                    if (!list_announced.Contains(playerControl))
                    {
                        await playerControl.SendChatToPlayerAsync(_config.AnnouncementMessage);
                    }
                    await Task.Delay(TimeSpan.FromMilliseconds(3000));
                    messageSend = true;
                    list_announced.Append(playerControl);
                }
            });
        }

        [EventListener]
        public void onDisconnect(IPlayerDestroyedEvent e) {
            if (list_announced.Contains(e.PlayerControl)) {
                list_announced.Remove(e.PlayerControl);
            }
        }

        [EventListener]
        public async void OnPlayerChat(IPlayerChatEvent e)
        {
            string input = e.Message;
            bool startsWithPrefix = e.Message.StartsWith("/") || e.Message.StartsWith("?");
            if (!startsWithPrefix) return;
            if (e.Message.Length < 2) return;
            e.IsCancelled = true;
            List<string> slashcommands = new List<string>{ "/timer", "/help", "/rules","/cancel","/end"};
            List<string> altCommands = new List<string> { "?timer", "?help", "?rules", "?end", "?cancel" };
            List<string> impchatCommands = new List<string> { "//", "??", "/w", "?w", "/say", "?say" };
            string command = input.Split(" ")[0].Trim().ToLower();
            string doublePrefix = input.Substring(0, 2);
            string message = "";
            bool isCommand = false;

            if (slashcommands.Contains(command) || altCommands.Contains(command) || impchatCommands.Contains(command)) {
                isCommand = true;
            }
            if(doublePrefix.Equals("//") || doublePrefix.Equals("??"))
            {
                isCommand = true;
            }
            if (!isCommand) 
            {
                message = _config.wrongCommandMessage;
                await e.PlayerControl.SendChatToPlayerAsync(message);
                return;
            }

            if (impchatCommands.Contains(command) || impchatCommands.Contains(doublePrefix))
            {
                e.IsCancelled = false;
                return;
            }

            if (input.Split(" ").Length > 1 && (command.Equals("/help") || command.Equals("?help")))
            {
                
                switch (e.Message.Split(" ")[1].Trim().ToLower())
                {
                    case "timer":
                        message = _config.timerMessage;
                        break;
                    case "help":
                        message = _config.helpMessage;
                        break;
                    case "rules":
                        message = _config.rulesMessage;
                        break;
                    case "cancel":
                        message = _config.cancelMessage;
                        break;
                    case "say":
                        message = _config.impChatMessage;
                        break;
                    case "w":
                        message = _config.impChatMessage;
                        break;
                    case "//":
                        message = _config.impChatMessage;
                        break;
                    case "??":
                        message = _config.impChatMessage;
                        break;
                    default:
                        message = _config.wrongCommandMessage;
                        break;
                } 
            } else
            {
                switch (command)
                {
                    case "/help":
                        message = _config.helpMessage;
                        break;
                    case "?help":
                        message = _config.helpMessage;
                        break;
                    case "/rules":
                        message = _config.rulesMessage;
                        break;
                    case "?rules":
                        message = _config.rulesMessage;
                        break;
                    default:
                        message = "";
                        break;
                }
            }
            if (message == "") {
                e.IsCancelled = false;
                return;
            }
            await e.PlayerControl.SendChatToPlayerAsync(message);
        }

        private void sendMessage(IInnerPlayerControl player, string message = "") 
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(message);
            player.SendChatToPlayerAsync(builder.ToString());
        }

    }
}