using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace VozBotLibrary.DataAccess
{
    public class AccessFilter
    {
        public static List<Models.Filter> GetFilter()
        {
            using var reader = File.OpenText(ConfigGetter.GetFilterPath());
            return JsonConvert.DeserializeObject<List<Models.Filter>>(reader.ReadToEnd());
        }

        public static void AddFilter(Models.Filter filter)
        {
            List<Models.Filter> Filters = GetFilter();
            Filters.Add(filter);

            using var writer = File.CreateText(ConfigGetter.GetFilterPath());
            writer.Write(JsonConvert.SerializeObject(Filters, Formatting.Indented));
        }
    }
}
