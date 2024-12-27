using DSharpPlus.Entities;
using Microsoft.Data.Sqlite;
using LLM.Rori.Discord.Data;
using LLM.Rori.Discord.Data.Bot;
using LLM.Rori.Discord.Builders;
using LLM.Rori.Discord.Extension;
using LLM.Rori.Discord.Data.Database;
using LLM.Rori.Discord.Data.Starchild;
using LLM.Rori.Discord.Base.Interfaces;
using LLM.Rori.Discord.Databases.Connectors;
using LLM.Rori.Discord.Databases.QueryHandlers;

namespace LLM.Rori.Discord.Databases
{
    public class SqliteDataHandler : IRoriService, IDatabaseHandler<User, User>
    {
        private SqliteDatabaseConfig cachedDbConfig = null;

        public Config Config { get; protected set; }

        public SqliteDataHandler(SqliteDatabaseConfig cachedDbConfig, Config mainConfig)
        {
            this.cachedDbConfig = cachedDbConfig;
            Config = mainConfig;
            CreateTableIfNeeded(new SqliteDbConnector(cachedDbConfig), false, true).ConfigureAwait(false);
        }

        public async Task SendData(User data, string customQuery = "")
        {
            var connector = new SqliteDbConnector(cachedDbConfig); //open connection
            await connector.ConnectAsync();

            //select user from db with matching id
            var user = SqliteQueryHandler.GetUserFromQuery(data.Id, connector);

            string query = string.Empty;
            if (!user.IsValid()) query = SqliteQueryHandler.GetUserInsertQuery(user);
            else query = customQuery != string.Empty ? customQuery : new SqliteUserUpdateQueryBuilder(user, QueryElement.All).Build();

            Console.WriteLine($"[LOAD] Sending user data to a db: {user.DiscordData.Username}");
            var updateCommand = new SqliteCommand(query, connector.Connection);
            await updateCommand.ExecuteScalarAsync();

            await updateCommand.DisposeAsync();

            Console.WriteLine($"[SUCCESS] Data updated for user: {user.DiscordData.Username}");
            //close connection
            await connector.DisconnectAndDisposeAsync();
        }

        public async Task<User> GetData(DiscordUser discordUser, string customQuery = "")
        {
            var connector = new SqliteDbConnector(cachedDbConfig); //open connection
            await connector.ConnectAsync();

            //select user from db with matching id
            var user = SqliteQueryHandler.GetUserFromQuery(discordUser.Id.ToString(), connector);


            if (!user.IsValid())
            {
                user = new User();
                user.DiscordData = new DiscordData();
                user.DiscordData.Id = discordUser.Id.ToString();
                user.DiscordData.Username = discordUser.Username;
                user.Id = discordUser.Id.ToString();

                await SendDataCustomQuery(SqliteQueryHandler.GetUserInsertQuery(user), connector);
            }

            await connector.DisconnectAndDisposeAsync(); //close connection

            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var connector = new SqliteDbConnector(cachedDbConfig); //open connection
            await connector.ConnectAsync();

            var users = SqliteQueryHandler.GetSelectAllUsersQuery(connector);

            await connector.DisconnectAndDisposeAsync(); //close connection
            return users;
        }

        private async Task SendDataCustomQuery(string customQuery, SqliteDbConnector connector)
        {
            var updateCommand = new SqliteCommand(customQuery, connector.Connection);
            await updateCommand.ExecuteScalarAsync();
        }

        private async Task CreateTableIfNeeded(SqliteDbConnector connector, bool isConnectedAlready = true, bool isDisposeAfter = false)
        {
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), cachedDbConfig.DbPath))) return;

            Console.WriteLine("Creating SQLite table..");
            //File.Create(Path.Combine(Directory.GetCurrentDirectory(), cachedDbConfig.DbPath)); //sqlite can handle it
            if(!isConnectedAlready) await connector.ConnectAsync();

            var createCommand = new SqliteCommand("CREATE TABLE Users(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, id TEXT NOT NULL, discord_data TEXT NOT NULL, minty_bar_data TEXT)", connector.Connection);
            await createCommand.ExecuteScalarAsync();

            Console.WriteLine($"{cachedDbConfig.DbPath} table created.");
            await createCommand.DisposeAsync();
            if (!isDisposeAfter) return;

            await connector.DisconnectAndDisposeAsync();
        }
    }
}
