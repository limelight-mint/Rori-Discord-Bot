using DSharpPlus.Entities;
using MySql.Data.MySqlClient;
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
    public class MySqlDataHandler : IRoriService, IDatabaseHandler<MintyBarData, MintyBarData>
    {
        private MySqlDatabaseConfig cachedDbConfig = null;

        public Config Config { get; protected set; }

        public MySqlDataHandler(MySqlDatabaseConfig cachedDbConfig, Config mainConfig)
        {
            this.cachedDbConfig = cachedDbConfig;
            Config = mainConfig;
        }

        public async Task SendData(MintyBarData data, string customQuery = "")
        {
            var connector = new MySqlDbConnector(cachedDbConfig); //open connection
            await connector.ConnectAsync();

            //select user from db with matching id
            var user = MySqlQueryHandler.GetUserFromQuery(data.Id, connector);

            string query = string.Empty;
            if (!user.IsValid()) query = MySqlQueryHandler.GetUserInsertQuery(user);
            else query = customQuery != string.Empty ? customQuery : new SqlUserUpdateQueryBuilder(user, QueryElement.All).Build();

            Console.WriteLine($"[LOAD] Sending user data to a db: {user.Username}");
            var updateCommand = new MySqlCommand(query, connector.Connection);
            await updateCommand.ExecuteScalarAsync();

            await updateCommand.DisposeAsync();

            Console.WriteLine($"[SUCCESS] Data updated for user: {user.Username}");
            //close connection
            await connector.DisconnectAndDisposeAsync();
        }

        public async Task<MintyBarData> GetData(DiscordUser discordUser, string customQuery = "")
        {
            var connector = new MySqlDbConnector(cachedDbConfig); //open connection
            await connector.ConnectAsync();

            //select user from db with matching id
            var user = MySqlQueryHandler.GetUserFromQuery(discordUser.Id.ToString(), connector);


            if (!user.IsValid())
            {
                user = new MintyBarData();
                user.Username = discordUser.Username;
                user.Id = discordUser.Id.ToString();

                await SendDataCustomQuery(MySqlQueryHandler.GetUserInsertQuery(user), connector);
            }

            await connector.DisconnectAndDisposeAsync(); //close connection

            return user;
        }

        public async Task<List<MintyBarData>> GetAllUsers()
        {
            var connector = new MySqlDbConnector(cachedDbConfig); //open connection
            await connector.ConnectAsync();

            var users = MySqlQueryHandler.GetSelectAllUsersQuery(connector);

            await connector.DisconnectAndDisposeAsync(); //close connection
            return users;
        }

        private async Task SendDataCustomQuery(string customQuery, MySqlDbConnector connector)
        {
            var updateCommand = new MySqlCommand(customQuery, connector.Connection);
            await updateCommand.ExecuteScalarAsync();
        }
    }
}
