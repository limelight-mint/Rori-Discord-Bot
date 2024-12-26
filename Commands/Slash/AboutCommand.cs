using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using LLM.Rori.Discord.Data;
using LLM.Rori.Discord.Actions;

namespace LLM.Rori.Discord.Commands.Slash
{
    public sealed class AboutCommand : ApplicationCommandModule
    {
        public Config Config { get; set; }

        [SlashCommand("about", "Shows info about the app")]
        public async Task ShowAbout(InteractionContext context)
        {
            await context.CreateResponseAsync(HelpAction.GetEmbed(Config.DefaultAboutImageUrl, Config.Version));
        }

        [SlashCommand("version", "Shows current Rori version")]
        public async Task ShowVersion(InteractionContext context)
        {
            DiscordEmbed embed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.CornflowerBlue,
                Description = HelpAction.TryParseVersion(Config.Version)
            }.Build();

            await context.CreateResponseAsync(embed);
        }
    }
}