using Impostor.Api.Events.Player;
using Impostor.Api.Events;
using Microsoft.Extensions.Logging;
using Impostor.Api.Net.Inner.Objects;
using Impostor.Api.Innersloth;

namespace ChatAnnouncePlugin
{
    public class ChatAnnounceEventListener : IEventListener
    {
        private readonly ILogger<ChatAnnouncePlugin> _logger;
        private Config _config;
        private List<IInnerPlayerControl> list_announced = new List<IInnerPlayerControl>();

        public ChatAnnounceEventListener(ILogger<ChatAnnouncePlugin> logger, Config config)
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
                bool messageSend()
                {
                    if (list_announced.Contains(playerControl))
                    {
                        return true;
                    }
                    return false;
                }
                while (clientPlayer != null && !messageSend())
                {
                    if (playerControl.PlayerInfo == null || playerControl.PlayerInfo.CurrentOutfit.IsIncomplete)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(200));
                        _logger.LogDebug("Continue.");
                        continue;
                    }

                    _logger.LogDebug(clientPlayer.Game.Code + " - Chat Announce Message sent to [" + clientPlayer.Client.Id + "] " + clientPlayer.Client.Name);
                    string message;
                    try
                    {
                        if (e.ClientPlayer.Client.Language == Language.SChinese || e.ClientPlayer.Client.Language == Language.TChinese)
                        {
                            message = string.Format(_config.ChineseMessage, e.ClientPlayer.Client.Name, (!e.ClientPlayer.Client.IsSub ? "Region 1" : "Region 2"), e.ClientPlayer.Client.GameVersion.ToString());
                        }
                        else
                        {
                            message = string.Format(_config.AnnouncementMessage, e.ClientPlayer.Client.Name, (!e.ClientPlayer.Client.IsSub ? "Region 1" : "Region 2"), e.ClientPlayer.Client.GameVersion.ToString());
                        }
                    }
                    catch(Exception e)
                    {
                        _logger.LogError(e.Message);
                        message = "Error while running ChatAnnounceMent.";
                    }

                    _logger.LogDebug("sending " + message);
                    await playerControl.SendChatToPlayerAsync(message);
                    list_announced.Append(playerControl);
                    break;
                }
            });
        }

        [EventListener]
        public void onDisconnect(IPlayerDestroyedEvent e)
        {
            list_announced.Remove(e.PlayerControl);
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
                string message;
                if (e.ClientPlayer.Client.Language is Language.SChinese or Language.TChinese)
                {
                    message = string.Format(_config.ChineseMessage, e.ClientPlayer.Client.Name, !e.ClientPlayer.Client.IsSub ? "Region 1" : "Region 2", e.ClientPlayer.Client.GameVersion.ToString());
                }
                else
                {
                    message = string.Format(_config.helpMessage, e.ClientPlayer.Client.Name, !e.ClientPlayer.Client.IsSub ? "Region 1" : "Region 2", e.ClientPlayer.Client.GameVersion.ToString());
                }
                await e.PlayerControl.SendChatToPlayerAsync(message);
            }
            else
            {
                e.IsCancelled = false;
            }
        }
    }
}