using DSharpPlus.Entities;
using Version = LLM.Rori.Discord.Data.Bot.Version;

namespace LLM.Rori.Discord.Actions
{
    internal class HelpAction
    {
        public const string Site = "https://minty.bar/";
        public const string Github = "https://github.com/limelight-mint/Rori-Discord-Bot";

        public static Version Version = new Version(0, 0, 1);

        public static string GetMessage(Version currentVersion)
        {
            return $"**Discord bot** for verification, events, activity tracking in Minty Universe and related products. Just start typing slash `/` and you will see all the commands." +
                    $"\n\n> 🍋 MintyBar: {Site}\n\n> 💾 Open-Source: {Github}\n\n> ☕ *Current Release: v{Version.Major}.{Version.Minor}.{Version.Build}*";
        }

        public static DiscordEmbedBuilder GetEmbed(string bannerUrl, Version currentVersion)
        {
            return new DiscordEmbedBuilder()
            {
                Color = DiscordColor.CornflowerBlue,
                ImageUrl = bannerUrl,
                Description = GetMessage(currentVersion)
            };
        }

        public static DiscordInteractionResponseBuilder GetInteractionResponse(string bannerUrl, Version currentVersion)
        {
            return new DiscordInteractionResponseBuilder().AddEmbed(GetEmbed(bannerUrl, currentVersion));
        }
    }
}
