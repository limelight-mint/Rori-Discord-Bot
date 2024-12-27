using LLM.Rori.Discord.Data;
using LLM.Rori.Discord.Data.Bot;
using LLM.Rori.Discord.Extension;
using LLM.Rori.Discord.Base.Interfaces;

namespace LLM.Rori.Discord.Actions
{
    public class ConnectAction
    {
        public User DiscordUser {  get; private set; }

        private IDatabaseHandler<User, User> dataHandler;

        public ConnectAction(User user, IDatabaseHandler<User, User> dataHandler)
        {
            DiscordUser = user;
            this.dataHandler = dataHandler;
        }

        /// <summary>
        /// Checks basic validation
        /// </summary>
        /// <returns>TRUE - valid, FALSE - not</returns>
        public static bool CheckIfUidValid(string uid)
        {
            return !string.IsNullOrEmpty(uid);
        }

        public async Task ConnectMintyUID(User user, string uid)
        {
            user.MintyBarData = new Data.Starchild.MintyBarData() { Id = uid };
            await dataHandler.SendData(user, QueryElement.All.GetCorrespondingQuery(user));
        }
    }
}
