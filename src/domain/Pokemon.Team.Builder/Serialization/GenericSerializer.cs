using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder
{
    public static class GenericSerializer<T>
    {
        public async static Task<string> Serialize(T values)
        {
            return await Task.Run(() =>
            {
                var serializer = new XmlSerializer(typeof(T));

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
                        serializer.Serialize(writer, values);

                        return stringWriter.ToString();
                    }
                }
            });
        }

        public async static Task SaveToFile(T values, string filePath)
        {
            var xml = await Serialize(values).ConfigureAwait(false);

            File.WriteAllText(filePath, xml, Encoding.Unicode);
        }

        public async static Task<T> LoadFromFile(string filePath)
        {
            return await Deserialize(filePath).ConfigureAwait(false);
        }

        public async static Task<T> Deserialize(string filePath)
        {
            return await Task.Run(() =>
            {
                var file = new FileInfo(filePath);

                if (!file.Exists)
                {
                    return default(T);
                }

                using (var fileStream = file.OpenRead())
                {
                    var serializer = new XmlSerializer(typeof(T));

                    // Use this to not fail on invalid characters
                    var xmlReader = new XmlTextReader(fileStream)
                    {
                        Normalization = true
                    };

                    return (T)serializer.Deserialize(xmlReader);
                }
            }).ConfigureAwait(false);
        }
    }
}
