using Newtonsoft.Json;
using LLM.Rori.Discord.Extension;
using LLM.Rori.Discord.Data.Localization;

namespace LLM.Rori.Discord.Services
{
    public class Localization
    {
        private const string LocalizationPacketName = "local.json";

        public const string PillsEmoji = "<:pillwhite:1119700330259693629>";

        public const string Limes = "currency.limes";

        public const string NoCurrencyKey = "currency.not_enough";

        protected static LocalizationPacket localizationPacket = new LocalizationPacket();

        internal async Task InitPacketAsync() => await LoadJson();

        /// <summary>
        /// Gets Text based on the <see cref="Language"/> provided
        /// </summary>
        /// <param name="key">Key to search</param>
        /// <returns>language-ready string</returns>
        public static string GetText(Language lang, string key) => localizationPacket.GetText(lang, key);

        /// <summary>
        /// Gets <see cref="Language"/> based on lang code provided
        /// </summary>
        /// <returns>language-ready string</returns>
        public static Language LanguageFromCode(string lang)
        {
            if (string.IsNullOrEmpty(lang)) return Language.English;
            lang = lang.ToLower();

            return lang switch
            {
                "ua" => Language.Ukrainian,
                "fr" => Language.French,
                "de" => Language.German,
                "pt" => Language.Portuguese,
                "es" => Language.Spanish,
                "jp" => Language.Japanese,
                "ru" => Language.Russian,
                _ => Language.English,
            };
        }

        public async Task LoadJson()
        {
            var jsonRawData = await DataGrabber.GrabFromConfigs(LocalizationPacketName);

            jsonRawData.LogStatus(LocalizationPacketName);

            localizationPacket = JsonConvert.DeserializeObject<LocalizationPacket>(jsonRawData);
        }

        //Generate JSON (USE TO CREATE A FIRST-TIME DUMMY)
        internal void GenerateDummyJson()
        {
            localizationPacket.Add(Language.English, "dummy_key", "dummy text about this key");
            localizationPacket.Add(Language.English, "dummy_key2", "dummy text about this key");

            localizationPacket.Add(Language.French, "dummy_key", "dummy text about this key");
            localizationPacket.Add(Language.French, "dummy_key2", "dummy text about this key");

            localizationPacket.Add(Language.Russian, "dummy_key", "dummy text about this key");
            localizationPacket.Add(Language.Russian, "dummy_key2", "dummy text about this key");

            localizationPacket.Add(Language.Japanese, "dummy_key", "dummy text about this key");
            localizationPacket.Add(Language.Japanese, "dummy_key2", "dummy text about this key");

            localizationPacket.Add(Language.Portuguese, "dummy_key", "dummy text about this key");
            localizationPacket.Add(Language.Portuguese, "dummy_key2", "dummy text about this key");

            localizationPacket.Add(Language.German, "dummy_key", "dummy text about this key");

            localizationPacket.Add(Language.Spanish, "dummy_key", "dummy text about this key");

            localizationPacket.Add(Language.Ukrainian, "dummy_key", "dummy text about this key");

            var rawJson = JsonConvert.SerializeObject(localizationPacket, Formatting.Indented);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), LocalizationPacketName), rawJson);
        }
    }
}
