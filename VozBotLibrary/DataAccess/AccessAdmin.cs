using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace VozBotLibrary.DataAccess
{
    public class AccessAdmin
    {
        public static List<Models.Admin> GetAdmin()
        {
            using var reader = File.OpenText(ConfigGetter.GetAdminPath());
            return JsonConvert.DeserializeObject<List<Models.Admin>>(reader.ReadToEnd());
        }

        public static void UpdateAdmin(List<Models.Admin> admins)
        {
            using var writer = File.CreateText(ConfigGetter.GetAdminPath());
            writer.Write(JsonConvert.SerializeObject(admins, Formatting.Indented));
        }
    }
}
