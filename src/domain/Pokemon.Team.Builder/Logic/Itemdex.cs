using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pokemon.Team.Builder.Logic
{
    [Serializable]
    [XmlRoot("Itemdex")]
    [XmlInclude(typeof(Item))]
    public class Itemdex : IEnumerable<Item>
    {
        public List<Item> Items;

        public Itemdex()
        {
            Items = new List<Item>();
        }

        public Itemdex(List<Item> items)
        {
            Items = items;
        }

        public void Add(object o)
        {
            Items.Add(o as Item);
        }

        IEnumerator<Item> IEnumerable<Item>.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Gets a pokemon by its name, all languages are searched for the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Item GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            name = name
                .Replace("-", string.Empty)
                .Replace(" ", string.Empty);

            return Items.SingleOrDefault(item =>
                item.Names.Any(n =>
                    n.name
                        .Replace("-", string.Empty)
                        .Replace(" ", string.Empty)
                    .Equals(name, StringComparison.InvariantCultureIgnoreCase)));
        }

        public List<Item.FlavorTextEntry> GetPokedexDescriptions(Item item, string languageCode)
        {
            return item.Flavor_Text_Entries.Where(text => text.language != null && text.language.name == languageCode)
                .ToList();
        }

        public List<string> GetAvailableLanguages()
        {
            return Items.FirstOrDefault()?.Names?.Select(name => name.language?.name).ToList();
        }
    }
}
