using LLM.Rori.Discord.Data.Starchild;

namespace LLM.Rori.Discord.Data.Bot
{
    [Serializable]
    public class User
    {
        public string Id { get; set; }
        public DiscordData DiscordData { get; set; }
        public MintyBarData MintyBarData { get; set; }
    }
}
