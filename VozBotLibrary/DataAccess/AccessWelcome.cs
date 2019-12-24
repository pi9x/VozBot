using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace VozBotLibrary.DataAccess
{
    /// <summary>
    /// Read and write welcome messages
    /// </summary>
    public class AccessWelcome
    {
        public static List<Models.Welcome> GetWelcome()
        {
            using var reader = File.OpenText(ConfigGetter.GetWelcomePath());
            return JsonConvert.DeserializeObject<List<Models.Welcome>>(reader.ReadToEnd());
        }

        public static void AddWelcome(Models.Welcome welcome)
        {
            List<Models.Welcome> Welcomes = GetWelcome();
            Welcomes.Add(welcome);

            using var writer = File.CreateText(ConfigGetter.GetWelcomePath());
            writer.Write(JsonConvert.SerializeObject(Welcomes, Formatting.Indented));
        }
    }
}
