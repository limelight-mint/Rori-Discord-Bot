namespace LLM.Rori.Discord.Data.Localization
{
    [Serializable]
    public class LocalizationPacket
    {
        public List<LocalizedText> english = new List<LocalizedText>();
        public List<LocalizedText> french = new List<LocalizedText>();
        public List<LocalizedText> ukrainian = new List<LocalizedText>();
        public List<LocalizedText> german = new List<LocalizedText>();
        public List<LocalizedText> japanese = new List<LocalizedText>();
        public List<LocalizedText> portuguese = new List<LocalizedText>();
        public List<LocalizedText> russian = new List<LocalizedText>();
        public List<LocalizedText> spanish = new List<LocalizedText>();
    }
}
