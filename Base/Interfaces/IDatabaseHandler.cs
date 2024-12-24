using DSharpPlus.Entities;

namespace LLM.Rori.Discord.Base.Interfaces
{
    public interface IDatabaseHandler<TDataGet, TDataSend>
        where TDataGet : class
        where TDataSend : class
    {

        public Task<TDataGet> GetData(DiscordUser discordUser, string customQuery = "");

        public Task SendData(TDataSend userData, string customQuery = "");

    }
}
