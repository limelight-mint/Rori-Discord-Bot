
using DSharpPlus;

namespace LLM.Rori.Discord.Data
{
    [Serializable]
    public class StartupConfig
    {
        public string Token = "your token goes here";
        public TokenType TokenType = TokenType.Bot;
        public string[] Prefixes = new[] { "!" };
    }
}
