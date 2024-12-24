using Microsoft.Data.Sqlite;
using LLM.Rori.Discord.Data.Database;
using LLM.Rori.Discord.Base.Interfaces;

namespace LLM.Rori.Discord.Databases.Connectors
{

    public class SqliteDbConnector : IDatabaseConnector
    {
        protected string debugInfo = "[SqliteDbConnector]";

        public SqliteConnection Connection { get; protected set; }

        public event Action<SqliteConnection> OnConnectionOpened;
        public event Action OnConnectionClosed;

        public SqliteDbConnector(string dbPath = "usersdata.db", string mode = "ReadWriteCreate")
        {
            string connection = $"Data Source={dbPath};Mode={mode}";
            Connection = new SqliteConnection(connection);
        }

        public SqliteDbConnector(SqliteDatabaseConfig config) : this(config.DbPath, config.Mode) { }

        public async Task ConnectAsync()
        {
            if (Connection == null)
            {
                Console.WriteLine($"{debugInfo} Connection credentials not set, try calling SqliteDbConnector constructor");
                return;
            }

            if (Connection.State == System.Data.ConnectionState.Open) return;

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

            if (Connection.State == System.Data.ConnectionState.Closed) return;

            await Connection.CloseAsync();
            await Connection.DisposeAsync();

            OnConnectionClosed?.Invoke();
        }
    }
}
