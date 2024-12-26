
namespace LLM.Rori.Discord.Data.Bot
{
    [Serializable]
    public class Version
    {
        public int Major = 0;
        public int Minor = 0;
        public int Build = 1;

        public Version(int major, int minor, int build)
        {
            Major = major;
            Minor = minor;
            Build = build;
        }
    }
}
