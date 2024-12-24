using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace LLM.Rori.Discord.Base.Interfaces
{
    public interface IDatabaseConnector
    {
        public Task ConnectAsync();
        public Task DisconnectAndDisposeAsync();
    }
}
