
namespace LLM.Rori.Discord.Data.Starchild
{
    [Serializable]
    public class StarchildData
    {
        public int Level { get; set; }
        public int Exp { get; set; }
        public int Cap { get; set; } = 1000;
        public int Supernova { get; set; }
        public string[] Friends { get; set; }
        public string[] Blacklist { get; set; }
        public int[] Badges { get; set; }
        public long SupernovaBirthTimestamp { get; set; }
        public long SupernovaChargeSeconds { get; set; }
        public long SupernovaLastRewardTimestamp { get; set; }
    }
}
