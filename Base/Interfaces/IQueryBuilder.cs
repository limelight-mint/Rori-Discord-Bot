using LLM.Rori.Discord.Data;

namespace LLM.Rori.Discord.Base.Interfaces
{
    public interface IQueryBuilder
    {
        public string QueryString { get; }
        public QueryElement[] ElementsUsed { get; }

        /// <summary>
        /// Builds a query into an actual string from QueryElements, returns table-ready query
        /// </summary>
        /// <returns>sql table-ready query</returns>
        public string Build();
    }
}
