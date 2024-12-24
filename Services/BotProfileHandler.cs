
using DSharpPlus;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using LLM.Rori.Discord.Data;
using LLM.Rori.Discord.Extension;
using LLM.Rori.Discord.Base.Interfaces;

namespace LLM.Rori.Discord.Services
{
    internal class BotProfileHandler : IRoriService
    {
        private StartupConfig botStartupConfig;
        private DiscordActivity currentStatus;

        public const string StartupConfig = "startup_config.json";

        public StartupConfig GetBotConfig() => botStartupConfig;

        public async Task<DiscordConfiguration> SetConfig()
        {
            var rawConfig = await DataGrabber.GrabFromConfigs(StartupConfig);

            rawConfig.LogStatus(StartupConfig);

            botStartupConfig = JsonConvert.DeserializeObject<StartupConfig>(rawConfig);

            return CreateDiscordConfig(botStartupConfig);
        }

        public DiscordConfiguration SetConfig(StartupConfig customConfig) => CreateDiscordConfig(customConfig);

        public DiscordConfiguration CreateDiscordConfig(StartupConfig config)
        {
            return new DiscordConfiguration()
            {
                Token = config.Token,
                TokenType = config.TokenType,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            };
        }

        public async Task<DiscordActivity> SetStatus(ActivityType activity, string name, string streamUrl = "https://www.youtube.com/watch?v=jfKfPfyJRdk")
        {
            currentStatus = new DiscordActivity()
            {
                ActivityType = activity,
                Name = name,
                StreamUrl = streamUrl
            };

            return currentStatus;
        }

    }
}
