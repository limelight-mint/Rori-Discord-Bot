using DSharpPlus.SlashCommands;
using LLM.Rori.Discord.Actions;
using LLM.Rori.Discord.Data.Bot;
using LLM.Rori.Discord.Base.Interfaces;

namespace LLM.Rori.Discord.Commands.Slash
{
    public sealed class ConnectCommand : ApplicationCommandModule
    {
        public IDatabaseHandler<User, User> DataHandler { get; set; }

        [SlashCommand("connect", "Connects your MintyBar UID")]
        public async Task ConnectGenshin(InteractionContext context, [Option("uid", "Your MintyBar UID")] string uid)
        {
            if (!ConnectAction.CheckIfUidValid(uid))
            {
                await context.CreateResponseAsync($"> ✖️ UID Error. Example `/connect sfh20dsFSzdFSz29f9x34`. Pass UID from minty.bar as parameter in Discord pop-up command menu please. ✖️", true);
                return;
            }

            var user = await DataHandler.GetData(context.User);
            var action = new ConnectAction(user, DataHandler);

            await action.ConnectMintyUID(user, uid);

            var response = context.CreateResponseAsync($"> ✨{user.DiscordData.Username} | UID: {user.DiscordData.Id} | Minty Bar ID: {user.MintyBarData.Id} ✅", true);
        }
    }
}
