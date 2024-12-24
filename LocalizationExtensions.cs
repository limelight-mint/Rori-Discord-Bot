using LLM.Rori.Discord.Services;
using LLM.Rori.Discord.Data.Starchild;
using LLM.Rori.Discord.Data.Localization;

namespace LLM.Rori.Discord.Extension
{
    public static class LocalizationExtensions
    {
        public static void Add(this LocalizationPacket packet, Language language, string key, string text)
        {
            var localizedText = new LocalizedText() { key = key, text = text };

            packet.Add(language, localizedText);
        }

        public static void Add(this LocalizationPacket packet, Language language, LocalizedText text)
        {
            switch (language)
            {
                case Language.French:
                    packet.french.Add(text);
                    break;

                case Language.Ukrainian:
                    packet.ukrainian.Add(text);
                    break;

                case Language.Russian:
                    packet.russian.Add(text);
                    break;

                case Language.Spanish:
                    packet.spanish.Add(text);
                    break;

                case Language.Japanese:
                    packet.japanese.Add(text);
                    break;

                case Language.Portuguese:
                    packet.portuguese.Add(text);
                    break;

                case Language.German:
                    packet.german.Add(text);
                    break;

                default:
                    packet.english.Add(text);
                    break;
            }
        }

        public static string GetText(this LocalizationPacket packet, Language language, string key)
        {
            string? text = null;
            switch (language)
            {
                case Language.French:
                    text = packet.french.Find(match => match.key == key)?.text;
                    break;

                case Language.Ukrainian:
                    text = packet.ukrainian.Find(match => match.key == key)?.text;
                    break;

                case Language.Russian:
                    text = packet.russian.Find(match => match.key == key)?.text;
                    break;

                case Language.Spanish:
                    text = packet.spanish.Find(match => match.key == key)?.text;
                    break;

                case Language.Portuguese:
                    text = packet.portuguese.Find(match => match.key == key)?.text;
                    break;

                case Language.German:
                    text = packet.german.Find(match => match.key == key)?.text;
                    break;

                case Language.Japanese:
                    text = packet.japanese.Find(match => match.key == key)?.text;
                    break;

                default:
                    text = packet.english.Find(match => match.key == key)?.text;
                    break;
            }

            if (text == null) return "[ERROR] No language key found";
            return text;
        }

        public static void Remove(this LocalizationPacket packet, Language language, string key)
        {
            var list = new List<LocalizedText>();
            switch (language)
            {
                case Language.French:
                    list = packet.french;
                    break;

                case Language.Ukrainian:
                    list = packet.ukrainian;
                    break;

                case Language.Russian:
                    list = packet.russian;
                    break;

                case Language.Spanish:
                    list = packet.spanish;
                    break;

                case Language.Japanese:
                    list = packet.japanese;
                    break;

                case Language.Portuguese:
                    list = packet.portuguese;
                    break;

                case Language.German:
                    list = packet.german;
                    break;

                default:
                    list = packet.english;
                    break;

            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].key != key) continue;

                packet.french.Remove(list[i]);
                packet.ukrainian.Remove(list[i]);
                packet.english.Remove(list[i]);
                packet.russian.Remove(list[i]);
                packet.spanish.Remove(list[i]);
                packet.japanese.Remove(list[i]);
                packet.portuguese.Remove(list[i]);
                packet.german.Remove(list[i]);
            }

        }

        public static string GetText(this UserData user, string key) => Localization.GetText(Localization.LanguageFromCode(user.Locale), key);

    }
}
