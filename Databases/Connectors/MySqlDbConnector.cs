using MySql.Data.MySqlClient;
using LLM.Rori.Discord.Base.Interfaces;
using LLM.Rori.Discord.Data.Database;

namespace LLM.Rori.Discord.Databases.Connectors
{

    internal class MySqlDbConnector : IDatabaseConnector
    {
        protected string debugInfo = "[MySqlDbConnector]";

        public MySqlConnection Connection { get; protected set; }

        public event Action<MySqlConnection> OnConnectionOpened;
        public event Action OnConnectionClosed;


        public MySqlDbConnector(string host, int? port, string database, string username, string password)
        {
            string connection = $"Server={host};Port={port};Database={database};Uid={username};Pwd={password}";

            var mySqlConnection = new MySqlConnection(connection);
            Connection = mySqlConnection;
        }

        public MySqlDbConnector(MySqlDatabaseConfig config) : this(config.Host, config.Port, config.Database, config.Username, config.Password) { }

        public async Task ConnectAsync()
        {
            if (Connection == null)
            {
                Console.WriteLine($"{debugInfo} Connection credentials not set, try calling MySqlDbConnector constructor");
                return;
            }

            await Connection.OpenAsync();
            OnConnectionOpened?.Invoke(Connection);
        }

        public async Task DisconnectAndDisposeAsync()
        {
            if (Connection == null)
            {
                Console.WriteLine($"{debugInfo} Nothing to close, have u opened up a connection correctly?");
                return;
            }

            await Connection.CloseAsync();
            await Connection.DisposeAsync();

            OnConnectionClosed?.Invoke();
        }
    }
}
