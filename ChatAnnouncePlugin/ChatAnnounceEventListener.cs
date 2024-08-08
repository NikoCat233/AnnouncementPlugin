using Impostor.Api.Events.Player;
using Impostor.Api.Events;
using Microsoft.Extensions.Logging;
using Impostor.Api.Net.Inner.Objects;
using System.Text;

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
            var clientPlayer = e.ClientPlayer;
            var playerControl = e.PlayerControl;

            Task.Run(async () =>
            {
                // Give the player time to load.
                await Task.Delay(TimeSpan.FromSeconds(3));

                bool messageSend = false;
                while (clientPlayer != null && clientPlayer.Client.Connection != null && clientPlayer.Client.Connection.IsConnected && playerControl.PlayerInfo != null &&!messageSend)
                {
                    _logger.LogDebug(clientPlayer.Game.Code + " - Chat Announce Message sent to [" + clientPlayer.Client.Id + "] " + clientPlayer.Client.Name);
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
            string input = e.Message.TrimStart('\n', '\r');
            bool startsWithPrefix = input.StartsWith('?');
            if (!startsWithPrefix) return;
            if (input.Length < 2) return;
            e.IsCancelled = true;
            string command = input.Split(" ")[0].Trim().ToLower();

            if (command.Equals("?help"))
            {
                string message = _config.helpMessage;
                await e.PlayerControl.SendChatToPlayerAsync(message);
            }
            else
            {
                e.IsCancelled = false;
            }
        }

        private void sendMessage(IInnerPlayerControl player, string message = "") 
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(message);
            player.SendChatToPlayerAsync(builder.ToString());
        }

    }
}