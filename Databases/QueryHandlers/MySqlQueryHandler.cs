using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using LLM.Rori.Discord.Data.Starchild;
using LLM.Rori.Discord.Databases.Connectors;

namespace LLM.Rori.Discord.Databases.QueryHandlers
{
    // TODO: Query builder
    //
    // Make a Query builder with interfaces to drop, each interface will provide GetQueryInfo() method with their strings for update
    // or do like a enums with foreach
    internal class MySqlQueryHandler
    {
        /// <summary>
        /// Gets full user data from entire table
        /// </summary>
        /// <returns>user with filled data</returns>
        public static MintyBarData GetUserFromQuery(string userId, MySqlDbConnector connector)
        {
            Console.WriteLine($"[LOAD] Trying to get user from Query | {userId}");
            string query = GetSelectUserQuery(userId);
            var selectCommand = new MySqlCommand(query, connector.Connection);

            var user = ReadUserFromQuery(selectCommand);
            selectCommand.Dispose();
            return user;
        }

        public static MintyBarData ReadUserFromQuery(MySqlCommand command)
        {
            var user = new MintyBarData();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                user.Id = reader.GetString("userid");
                user.Username = reader.GetString("username");
                user.Wallet = JsonConvert.DeserializeObject<Wallet>(reader.GetString("wallet"));
                user.Stats = JsonConvert.DeserializeObject<Stats>(reader.GetString("stats"));
                user.Inventory = JsonConvert.DeserializeObject<Dictionary<long,long>>(reader.GetString("inventory"));
            }

            Console.WriteLine($"[SUCCESS] Got user from db: {user.Username} | {user.Id}");
            reader.Close();
            return user;
        }

        public static List<MintyBarData> ReadUsersFromQuery(MySqlCommand command)
        {
            var users = new List<MintyBarData>();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var user = new MintyBarData();

                user.Id = reader.GetString("userid");
                user.Username = reader.GetString("username");
                user.Wallet = JsonConvert.DeserializeObject<Wallet>(reader.GetString("wallet"));
                user.Stats = JsonConvert.DeserializeObject<Stats>(reader.GetString("stats"));
                user.Inventory = JsonConvert.DeserializeObject<Dictionary<long, long>>(reader.GetString("inventory"));

                users.Add(user);
            }

            reader.Close();
            return users;
        }

        /// <returns>Query string with full user selection (select all rows) of a single user</returns>
        public static string GetSelectUserQuery(string userId) => $"SELECT * FROM users WHERE userid = {userId}";

        /// <returns>Query string with full user selection (select all rows) of all users</returns>
        public static string GetSelectAllUsersQuery() => "SELECT * FROM users";

        /// <returns>Query string for insertion for a completely NEW user (insert all rows)</returns>
        public static string GetUserInsertQuery(MintyBarData user)
        {
            string query = $"INSERT INTO `users`(`userid`, `username`, `wallet`, `stats`, `inventory`) " +
                $"VALUES ('{user.Id}','{user.Username}','{JsonConvert.SerializeObject(user.Wallet)}'," +
                $"'{JsonConvert.SerializeObject(user.Stats)}','{JsonConvert.SerializeObject(user.Inventory)}')";

            return query;
        }

        public static List<MintyBarData> GetSelectAllUsersQuery(MySqlDbConnector connector)
        {
            var users = new List<MintyBarData>();
            string query = GetSelectAllUsersQuery();
            var selectCommand = new MySqlCommand(query, connector.Connection);

            users = ReadUsersFromQuery(selectCommand);
            selectCommand.Dispose();
            return users;
        }
    }
}
