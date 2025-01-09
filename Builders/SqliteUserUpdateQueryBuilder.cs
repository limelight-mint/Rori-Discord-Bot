using System.Text;
using LLM.Rori.Discord.Data;
using LLM.Rori.Discord.Extension;
using LLM.Rori.Discord.Data.Starchild;
using LLM.Rori.Discord.Base.Interfaces;
using LLM.Rori.Discord.Data.Bot;

namespace LLM.Rori.Discord.Builders
{
    /// <summary>
    /// Class used to build update queries to update only needed elements instead of all tables/rows
    /// </summary>
    internal class SqliteUserUpdateQueryBuilder : IQueryBuilder
    {
        public string QueryString { get; private set; }
        public QueryElement[] ElementsUsed { get; private set; }

        public string UserIdUsed { get; protected set; }

        public SqliteUserUpdateQueryBuilder(User user, params QueryElement[] elements)
        {
            ElementsUsed = elements;
            UserIdUsed = user.Id;

            if (ElementsUsed.Contains(QueryElement.All))
            {
                //no need to go down below since we already know query will include all modules to update
                QueryString = QueryElement.All.GetCorrespondingQuery(user);
                return;
            }

            StringBuilder queryBuilder = new StringBuilder("");

            for (int i = 0; i < ElementsUsed.Length; i++)
            {
                queryBuilder.Append(ElementsUsed[i].GetCorrespondingQuery(user));
                if (i < ElementsUsed.Length - 1) queryBuilder.Append(",");
            }

            QueryString = $"UPDATE `users` SET {queryBuilder.ToString()} WHERE `id`='{UserIdUsed}'";
        }

        public string Build() => QueryString;
    }
}
