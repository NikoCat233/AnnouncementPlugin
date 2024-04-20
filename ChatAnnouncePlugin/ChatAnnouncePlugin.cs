using Impostor.Api.Events.Managers;
using Impostor.Api.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ChatAnnouncePlugin
{
    [ImpostorPlugin("vex.ChatAnnouncePlugin")]
    public class ChatAnnouncePlugin : IPlugin
    {
        private readonly ILogger<ChatAnnouncePlugin> _logger;
        private readonly IEventManager _eventManager;
        private IDisposable _unregister;

        private string ConfigDirectoryPath = Path.Combine(Environment.CurrentDirectory, "config");
        private const string ConfigPath = "announce.json";


        public ChatAnnouncePlugin(ILogger<ChatAnnouncePlugin> logger, IEventManager eventManager)
        {
            _logger = logger;
            _eventManager = eventManager;
        }

        public Config LoadConfig() {
            string config_path = Path.Combine(ConfigDirectoryPath, ConfigPath);
            Config config;

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            if (File.Exists(config_path)) {
                config = JsonSerializer.Deserialize<Config>(File.ReadAllText(config_path));
            }
            else {
                config = new Config();
                File.WriteAllText(config_path, JsonSerializer.Serialize(config, options));
            }
            return config;

        }

        public void initialize()
        {
            bool directoryExists = Directory.Exists(ConfigDirectoryPath);

            if (!directoryExists)
            {
                Directory.CreateDirectory(ConfigDirectoryPath);
            }
        }


        public ValueTask EnableAsync()
        {
            initialize();
            var config = LoadConfig();

            _logger.LogInformation("ChatAnnouncePlugin is enabled.");
            _unregister = _eventManager.RegisterListener(new ChatAnnounceEventListener(_logger, config));
            return default;
        }

        public ValueTask DisableAsync()
        {
            _logger.LogInformation("ChatAnnouncePlugin is disabled.");
            _unregister.Dispose();
            return default;
        }

        public ValueTask ReloadAsync()
        {
            throw new NotImplementedException();
        }
    }
}