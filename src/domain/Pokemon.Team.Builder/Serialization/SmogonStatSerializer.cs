using Pokemon.Team.Builder.Model.Smogon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Pokemon.Team.Builder.Serialization
{
    public static class SmogonStatSerializer
    {
        public static string SerializeStats(List<SmogonPokemonStats> smogonStats)
        {
            var serializer = new XmlSerializer(typeof(List<SmogonPokemonStats>));

            using (var stringWriter = new StringWriter())
            {
                var settings = new XmlWriterSettings
                {
                    CheckCharacters = true,
                    Encoding = Encoding.Unicode,
                    NewLineHandling = NewLineHandling.Entitize,
                    Indent = true
                };

                using (var writer = XmlWriter.Create(stringWriter, settings))
                {
                    serializer.Serialize(writer, smogonStats);

                    return stringWriter.ToString();
                }
            }
        }

        public static void SaveStatsToFile(List<SmogonPokemonStats> stats, string filePath)
        {
            File.WriteAllText(filePath, SerializeStats(stats), Encoding.Unicode);
        }

        public static List<SmogonPokemonStats> LoadStatsFromFile(string filePath)
        {
            return DeserializeStats(filePath);
        }

        public static List<SmogonPokemonStats> DeserializeStats(string filePath)
        {
            var file = new FileInfo(filePath);

            if (!file.Exists)
            {
                return null;
            }

            using (var fileStream = file.OpenRead())
            {
                var serializer = new XmlSerializer(typeof(List<SmogonPokemonStats>));

                // Use this to not fail on invalid characters
                var xmlReader = new XmlTextReader(fileStream)
                {
                    Normalization = true
                };

                return (List<SmogonPokemonStats>)serializer.Deserialize(xmlReader);
            }
        }
    }
}
