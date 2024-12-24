using Newtonsoft.Json;
using LLM.Rori.Discord.Data;
using LLM.Rori.Discord.Data.Starchild;

namespace LLM.Rori.Discord.Extension
{
    public static class Extensions
    {
        public static string GetNormalTime(this TimeSpan time) => time.ToString(@"dd\.hh\:mm\:ss");

        public static void LogStatus(this string rawJson, string fileName = "")
        {
            bool isCorrupted = rawJson == null || rawJson.Length <= 0;

            string corruptedMessage = $"[ERROR] Couldnt load {fileName}";
            string successMessage = $"[SUCCESS] {fileName} loaded successfully";

            if(isCorrupted)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(isCorrupted ? corruptedMessage : successMessage);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(isCorrupted ? corruptedMessage : successMessage);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

        }

        public static string GetCorrespondingQuery(this QueryElement element, UserData user)
        {
            switch (element)
            {
                case QueryElement.Username: return $"`username`='{user.Username}'";
                case QueryElement.Wallet: return $"`wallet`='{JsonConvert.SerializeObject(user.Wallet)}'";
                case QueryElement.Stats: return $"`stats`='{JsonConvert.SerializeObject(user.Stats)}'";
                case QueryElement.Inventory: return $"`inventory`='{JsonConvert.SerializeObject(user.Inventory)}'";
                default: return $"`userid='{user.Id}',`username`='{user.Username}',`wallet`='{JsonConvert.SerializeObject(user.Wallet)}'," +
                        $"`stats`='{JsonConvert.SerializeObject(user.Stats)}',`inventory`='{JsonConvert.SerializeObject(user.Inventory)}'";
            }
        }

        public static bool IsValid(this UserData user)
        {
            if (user == null) return false;
            if (string.IsNullOrEmpty(user.Id)) return false;
            return true;
        }
    }
}
