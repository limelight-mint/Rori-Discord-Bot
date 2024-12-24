namespace LLM.Rori.Discord.Data.Database
{
    [Serializable]
    public class BaseDatabaseConfig
    {
        public string Host = "localhost";
        public int? Port = null;
        public string Database;
        public string Username = "root";
        public string Password = "root";
    }
}
