using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Xml;

namespace Pokemon.Team.Builder
{
	public static class TierListSerializer
	{
		public static string SerializeTierList(TierList tierList)
		{
            var serializer = new XmlSerializer(typeof(TierList));

            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, tierList);

                return stringWriter.ToString();
            }
        }

		public static void SaveTierListToFile(TierList tierList, string filePath){
			File.WriteAllText(filePath, SerializeTierList(tierList), System.Text.Encoding.Unicode);
		}

		public static TierList LoadTierListFromFile(string filePath){
			return DeserializeTierList (filePath);
		}

		public static TierList DeserializeTierList(string filePath)
		{
            var file = new FileInfo(filePath);

            if (!file.Exists)
            {
                return null;
            }

            using (var fileStream = file.OpenRead())
            {
                var serializer = new XmlSerializer(typeof(TierList));

				// Use this to not fail on invalid characters
				var xmlReader = new XmlTextReader (fileStream) {
					Normalization = false
				};

				return (TierList) serializer.Deserialize(xmlReader);
            }
        }
	}
}

