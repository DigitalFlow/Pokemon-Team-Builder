using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pokemon.Team.Builder.Serialization
{
	public static class TierSerializer
    {
       public static List<Tier> ParseFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            var stream = File.OpenRead(filePath);

            return ParseFromStream(stream);
        }

		public static List<Tier> ParseFromStream(Stream stream)
        {
            var xmlDoc = XDocument.Load(stream);

            return xmlDoc.Root.Elements().Select(element => Parse(element)).ToList();
        }

        public static Tier Parse(XElement element)
        {
            return new Tier
            {
                FullName = element.Element("FullName").Value,
                ShortName = element.Element("ShortName").Value,
                SubTiers = element.Elements("Tier")?.Select(e => e != null ? Parse(e) : null).ToList()
            };
        }
    }
}
