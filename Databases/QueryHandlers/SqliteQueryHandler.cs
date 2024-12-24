using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
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
        public static UserData GetUserFromQuery(string userId, SqliteDbConnector connector)
        {
            Console.WriteLine($"[LOAD] Trying to get user from Query | {userId}");
            string query = GetSelectUserQuery(userId);
            var selectCommand = new SqliteCommand(query, connector.Connection);

            var user = ReadUserFromQuery(selectCommand);
            selectCommand.Dispose();
            return user;
        }

        public static UserData ReadUserFromQuery(SqliteCommand command)
        {
            var user = new UserData();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                user.Id = reader.GetString(1); //"userid"
                user.Username = reader.GetString(2); //"username"
                user.Wallet = JsonConvert.DeserializeObject<Wallet>(reader.GetString(3)); //"wallet"
                user.Stats = JsonConvert.DeserializeObject<Stats>(reader.GetString(4)); //"stats"
                user.Inventory = JsonConvert.DeserializeObject<Dictionary<long,long>>(reader.GetString(5)); //"inventory"
            }

            Console.WriteLine($"[SUCCESS] Got user from db: {user.Username} | {user.Id}");
            reader.Close();
            return user;
        }

        public static List<UserData> ReadUsersFromQuery(SqliteCommand command)
        {
            var users = new List<UserData>();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var user = new UserData();

                user.Id = reader.GetString(1); //"userid"
                user.Username = reader.GetString(2); //"username"
                user.Wallet = JsonConvert.DeserializeObject<Wallet>(reader.GetString(3)); //"wallet"
                user.Stats = JsonConvert.DeserializeObject<Stats>(reader.GetString(4)); //"stats"
                user.Inventory = JsonConvert.DeserializeObject<Dictionary<long, long>>(reader.GetString(5)); //"inventory"

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
        public static string GetUserInsertQuery(UserData user)
        {
            string query = $"INSERT INTO `users`(`userid`, `username`, `wallet`, `stats`, `inventory`) " +
                $"VALUES ('{user.Id}','{user.Username}','{JsonConvert.SerializeObject(user.Wallet)}'," +
                $"'{JsonConvert.SerializeObject(user.Stats)}','{JsonConvert.SerializeObject(user.Inventory)}')";

            return query;
        }

        public static List<UserData> GetSelectAllUsersQuery(SqliteDbConnector connector)
        {
            var users = new List<UserData>();
            string query = GetSelectAllUsersQuery();
            var selectCommand = new SqliteCommand(query, connector.Connection);

            users = ReadUsersFromQuery(selectCommand);
            selectCommand.Dispose();
            return users;
        }
    }
}
