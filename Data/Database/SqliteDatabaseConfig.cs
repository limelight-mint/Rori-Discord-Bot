namespace LLM.Rori.Discord.Data.Database
{
    [Serializable]
    public class SqliteDatabaseConfig
    {
        public string Mode { get; set; } = "ReadWriteCreate";
        public string DbPath { get; set; } = "usersdata.db";
    }
}
