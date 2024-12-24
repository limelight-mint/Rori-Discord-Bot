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
            await context.CreateResponseAsync(HelpAction.GetEmbed(Config.DefaultProfileImageUrl));
        }
    }
}