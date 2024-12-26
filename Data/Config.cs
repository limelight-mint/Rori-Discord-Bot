namespace LLM.Rori.Discord.Data
{
    [Serializable]
    public class Config
    {
        public Bot.Version Version = new Bot.Version(0, 0, 1);
        public int LimesPerMessage = 1;
        public int MaxLimes = 999999999;

        //Discord level (min 1) (max 100) * multiplier (min 1)
        public Dictionary<int, float> LevelToMultiplierTable = new Dictionary<int, float>(); //KEY:Value eg. Discord Activity Level:Limes multiplier

        //Decorations and its cost for Limes
        public Dictionary<Decoration, int> Decorations = new Dictionary<Decoration, int>(); //KEY:Value eg. Decoration:Amount in Limes

        //URLs
        public string DefaultProfileImageUrl = "https://raw.githubusercontent.com/limelight-mint/Rori-Discord-Bot/main/content/default-profile-banner.png";
        public string DefaultAboutImageUrl = "https://raw.githubusercontent.com/limelight-mint/Rori-Discord-Bot/main/content/default-about-banner.png";
    }
}
