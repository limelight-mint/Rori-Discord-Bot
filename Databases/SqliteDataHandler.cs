using DSharpPlus.Entities;
using Microsoft.Data.Sqlite;
using LLM.Rori.Discord.Data;
using LLM.Rori.Discord.Builders;
using LLM.Rori.Discord.Extension;
using LLM.Rori.Discord.Data.Database;
using LLM.Rori.Discord.Data.Starchild;
using LLM.Rori.Discord.Base.Interfaces;
using LLM.Rori.Discord.Databases.Connectors;
using LLM.Rori.Discord.Databases.QueryHandlers;

namespace LLM.Rori.Discord.Databases
{
    public class SqliteDataHandler : IRoriService, IDatabaseHandler<UserData, UserData>
    {
        private SqliteDatabaseConfig cachedDbConfig = null;

        public Config Config { get; protected set; }

        public SqliteDataHandler(SqliteDatabaseConfig cachedDbConfig, Config mainConfig)
        {
            this.cachedDbConfig = cachedDbConfig;
            Config = mainConfig;
            //CreateTableIfNeeded(new SqliteDbConnector(cachedDbConfig), false, true).ConfigureAwait(false);
        }

        public async Task SendData(UserData data, string customQuery = "")
        {
            var connector = new SqliteDbConnector(cachedDbConfig); //open connection
            await connector.ConnectAsync();

            //select user from db with matching id
            var user = SqliteQueryHandler.GetUserFromQuery(data.Id, connector);

            string query = string.Empty;
            if (!user.IsValid()) query = SqliteQueryHandler.GetUserInsertQuery(user);
            else query = customQuery != string.Empty ? customQuery : new SqlUserUpdateQueryBuilder(user, QueryElement.All).Build();

            Console.WriteLine($"[LOAD] Sending user data to a db: {user.Username}");
            var updateCommand = new SqliteCommand(query, connector.Connection);
            await updateCommand.ExecuteScalarAsync();

            await updateCommand.DisposeAsync();

            Console.WriteLine($"[SUCCESS] Data updated for user: {user.Username}");
            //close connection
            await connector.DisconnectAndDisposeAsync();
        }

        public async Task<UserData> GetData(DiscordUser discordUser, string customQuery = "")
        {
            var connector = new SqliteDbConnector(cachedDbConfig); //open connection
            await connector.ConnectAsync();

            //select user from db with matching id
            var user = SqliteQueryHandler.GetUserFromQuery(discordUser.Id.ToString(), connector);


            if (!user.IsValid())
            {
                user = new UserData();
                user.Username = discordUser.Username;
                user.Id = discordUser.Id.ToString();

                await SendDataCustomQuery(SqliteQueryHandler.GetUserInsertQuery(user), connector);
            }

            await connector.DisconnectAndDisposeAsync(); //close connection

            return user;
        }

        public async Task<List<UserData>> GetAllUsers()
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

        public async Task CreateTableIfNeeded(SqliteDbConnector connector, bool isConnectedAlready = true, bool isDisposeAfter = false)
        {
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), cachedDbConfig.DbPath))) return;

            Console.WriteLine("Creating SQLite table..");
            //File.Create(Path.Combine(Directory.GetCurrentDirectory(), cachedDbConfig.DbPath)); //sqlite can handle it
            if(!isConnectedAlready) await connector.ConnectAsync();

            var createCommand = new SqliteCommand("CREATE TABLE Users(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, DiscordUid TEXT NOT NULL, DiscordData TEXT NOT NULL, MintyBarUid TEXT, MintyBarData TEXT, IsBlocked INTEGER NOT NULL)", connector.Connection);
            await createCommand.ExecuteScalarAsync();

            Console.WriteLine($"{cachedDbConfig.DbPath} table created.");
            await createCommand.DisposeAsync();
            if (!isDisposeAfter) return;

            await connector.DisconnectAndDisposeAsync();
        }
    }
}
