using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace VozBotLibrary.DataAccess
{
    /// <summary>
    /// Read and write Note
    /// </summary>
    class AccessNote
    {
        public static List<Models.Note> GetNote()
        {
            using var reader = File.OpenText(ConfigGetter.GetNotePath());
            return JsonConvert.DeserializeObject<List<Models.Note>>(reader.ReadToEnd());
        }

        public void AddNote(Models.Note note)
        {
            List<Models.Note> Notes = GetNote();
            Notes.Add(note);

            using var writer = File.CreateText(ConfigGetter.GetNotePath());
            writer.Write(JsonConvert.SerializeObject(Notes, Formatting.Indented));
        }
    }
}
