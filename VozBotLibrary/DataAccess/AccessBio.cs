using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace VozBotLibrary.DataAccess
{
    public class AccessBio
    {
        public static List<Models.Bio> GetBio()
        {
            using var reader = File.OpenText(ConfigGetter.GetBioPath());
            return JsonConvert.DeserializeObject<List<Models.Bio>>(reader.ReadToEnd());
        }

        public static void AddBio(Models.Bio bio)
        {
            List<Models.Bio> Bios = GetBio();
            foreach (var existedBio in Bios)
            {
                if (existedBio.UserId == bio.UserId)
                {
                    Bios.Remove(existedBio);
                    break;
                }
            }
            Bios.Add(bio);

            using var writer = File.CreateText(ConfigGetter.GetBioPath());
            writer.Write(JsonConvert.SerializeObject(Bios, Formatting.Indented));
        }
    }
}
