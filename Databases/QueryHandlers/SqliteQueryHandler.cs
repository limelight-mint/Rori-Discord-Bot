using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using LLM.Rori.Discord.Data.Bot;
using LLM.Rori.Discord.Data.Starchild;
using LLM.Rori.Discord.Databases.Connectors;

namespace LLM.Rori.Discord.Databases.QueryHandlers
{
    // TODO: Query builder
    //
    // Make a Query builder with interfaces to drop, each interface will provide GetQueryInfo() method with their strings for update
    // or do like a enums with foreach
    internal class SqliteQueryHandler
    {
        /// <summary>
        /// Gets full user data from entire table
        /// </summary>
        /// <returns>user with filled data</returns>
        public static User GetUserFromQuery(string userId, SqliteDbConnector connector)
        {
            Console.WriteLine($"[LOAD] Trying to get user from Query | {userId}");
            string query = GetSelectUserQuery(userId);
            var selectCommand = new SqliteCommand(query, connector.Connection);

            var user = ReadUserFromQuery(selectCommand);
            selectCommand.Dispose();
            return user;
        }

        public static User ReadUserFromQuery(SqliteCommand command)
        {
            var user = new User();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                user.Id = reader.GetString(1); //"id"
                user.DiscordData = JsonConvert.DeserializeObject<DiscordData>(reader.GetString(2)); //"discord_data"
                user.MintyBarData = JsonConvert.DeserializeObject<MintyBarData>(reader.GetString(3)); //"minty_bar_data"
                //user.MintyBarData = JsonConvert.DeserializeObject<Wallet>(reader.GetString(3)); //"wallet"
            }

            Console.WriteLine($"[SUCCESS] Got user from db: {user.Id} | {user.DiscordData}");
            reader.Close();
            return user;
        }

        public static List<User> ReadUsersFromQuery(SqliteCommand command)
        {
            var users = new List<User>();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var user = new User();

                user.Id = reader.GetString(1); //"id"
                user.DiscordData = JsonConvert.DeserializeObject<DiscordData>(reader.GetString(2)); //"discord_data"
                user.MintyBarData = JsonConvert.DeserializeObject<MintyBarData>(reader.GetString(3)); //"minty_bar_data"
                //user.MintyBarData = JsonConvert.DeserializeObject<Wallet>(reader.GetString(3)); //"wallet"

                users.Add(user);
            }

            reader.Close();
            return users;
        }

        /// <returns>Query string with full user selection (select all rows) of a single user</returns>
        public static string GetSelectUserQuery(string userId) => $"SELECT * FROM users WHERE id = {userId}";

        /// <returns>Query string with full user selection (select all rows) of all users</returns>
        public static string GetSelectAllUsersQuery() => "SELECT * FROM users";

        /// <returns>Query string for insertion for a completely NEW user (insert all rows)</returns>
        public static string GetUserInsertQuery(User user)
        {
            string query = $"INSERT INTO `users`(`id`, `discord_data`, `minty_bar_data`) " +
                $"VALUES ('{user.Id}','{JsonConvert.SerializeObject(user.DiscordData)}','{JsonConvert.SerializeObject(user.MintyBarData)}')";

            return query;
        }

        public static List<User> GetSelectAllUsersQuery(SqliteDbConnector connector)
        {
            var users = new List<User>();
            string query = GetSelectAllUsersQuery();
            var selectCommand = new SqliteCommand(query, connector.Connection);

            users = ReadUsersFromQuery(selectCommand);
            selectCommand.Dispose();
            return users;
        }
    }
}
