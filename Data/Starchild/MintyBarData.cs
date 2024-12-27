

namespace LLM.Rori.Discord.Data.Starchild
{
    [Serializable]
    public class MintyBarData
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Profile { get; set; }
        public StarchildData Starchild { get; set; }
        public int[] Badges { get; set; }
        public Wallet Wallet { get; set; }
        public Dictionary<long, long> Inventory { get; set; }
        public Stats Stats { get; set; }
        public string Locale { get; set; }
        public string[] Warnings { get; set; }
        public string[] FriendRequests { get; set; }
    }
}
