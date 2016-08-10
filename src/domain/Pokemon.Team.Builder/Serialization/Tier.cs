using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pokemon.Team.Builder.Serialization
{
    public class Tier
    {
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public List<Tier> SubTiers { get; set; }

        public override bool Equals(object obj)
        {
            var otherTier = obj as Tier;

            if(otherTier == null)
            {
                return false;
            }

            if(SubTiers.Count != otherTier.SubTiers.Count)
            {
                return false;
            }

            var tuples = new List<Tuple<Tier, Tier>>();

            for (var i = 0; i < SubTiers.Count; i++)
            {
                tuples.Add(Tuple.Create(SubTiers[i], otherTier.SubTiers[i]));   
            }

            return FullName == otherTier.FullName
                && ShortName == otherTier.ShortName
                && tuples.All(tuple => tuple.Item1.Equals(tuple.Item2));
        }

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
