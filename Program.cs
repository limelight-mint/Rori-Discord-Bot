using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using LLM.Rori.Discord.Data;
using LLM.Rori.Discord.Services;
using LLM.Rori.Discord.Databases;
using LLM.Rori.Discord.Extension;
using LLM.Rori.Discord.Data.Database;
using LLM.Rori.Discord.Commands.Slash;
using LLM.Rori.Discord.Data.Starchild;
using LLM.Rori.Discord.Base.Interfaces;
using LLM.Rori.Discord.Databases.Connectors;

namespace LLM.Rori.Discord
{
    internal class Program
    {
        private class DiscordInfo
        {
            public DiscordClient Discord;
            public DiscordActivity Activity;
            public UserStatus Status;

            public DiscordInfo(DiscordClient discord, DiscordActivity activity, UserStatus status = UserStatus.Idle) { Discord = discord; Activity = activity; Status = status; }
        }

        private class DbInfo<TDbConfig>
        {
            public Config Config;
            public TDbConfig DbConfig;

            public DbInfo(TDbConfig config) { DbConfig = config; }
        }

        public const string ConfigFile = "rori_config.json";

        static async Task Main(string[] args)
        {
            //Init and cache localization packets
            await new Services.Localization().InitPacketAsync();

            //Init database config and data handlers
            var profileService = new BotProfileHandler();

            //Implemented Example of Database Swap:
            //var dbInfo = await GetDatabaseConfig<MySqlDatabaseConfig>("mysql_dbconfig.json");
            //IDatabaseHandler<UserData, UserData> dataHandler = new MySqlDataHandler(dbInfo.DbConfig, dbInfo.Config);

            var dbInfo = await GetDatabaseConfig<SqliteDatabaseConfig>("sqlite_dbconfig.json");
            IDatabaseHandler<UserData, UserData> dataHandler = new SqliteDataHandler(dbInfo.DbConfig, dbInfo.Config);

            //Init discord client itself and setup activity
            var discordInfo = await InitializeDiscord(profileService);

            RegisterCommands(discordInfo.Discord, BindServices(profileService, dataHandler, dbInfo.Config));

            await discordInfo.Discord.ConnectAsync(discordInfo.Activity, discordInfo.Status);
            await Task.Delay(-1);
        }



        private static async Task<DbInfo<TDbConfig>> GetDatabaseConfig<TDbConfig>(string dbConfigFilename)
        {
            var rawConfig = await DataGrabber.GrabFromConfigs(dbConfigFilename);
            rawConfig.LogStatus(dbConfigFilename);

            var dbInfo = new DbInfo<TDbConfig>(JsonConvert.DeserializeObject<TDbConfig>(rawConfig));

            var rawMainConfigData = await DataGrabber.GrabFromConfigs(ConfigFile);
            dbInfo.Config = JsonConvert.DeserializeObject<Config>(rawMainConfigData);
            return dbInfo;
        }


        private static async Task<DiscordInfo> InitializeDiscord(BotProfileHandler profileService)
        {
            var client = new DiscordClient(await profileService.SetConfig());
            DiscordActivity activity = await profileService.SetStatus(ActivityType.Streaming, "Minty Girl");
            return new DiscordInfo(client, activity, UserStatus.Idle);
        }


        private static ServiceProvider BindServices(
            BotProfileHandler profileService, IDatabaseHandler<UserData, UserData> dataHandler,
            Config mainConfig)
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton(profileService);
            services.AddSingleton(dataHandler);
            services.AddSingleton(mainConfig);

            return services.BuildServiceProvider();
        }


        private static void RegisterCommands(DiscordClient discord, IServiceProvider services)
        {
            //Attach slash commands service collection so the services will be injected in slash commands
            var slashCommands = new SlashCommandsConfiguration() { Services = services };
            var slash = discord.UseSlashCommands(slashCommands);

            //Register one-by-one is preffered instead of .RegisterCommands(typeof(Program).Assembly);
            slash.RegisterCommands<AboutCommand>();
        }
    }
}
        