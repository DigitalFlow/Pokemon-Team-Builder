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
    [XmlRoot("Abilitydex")]
    [XmlInclude(typeof(Ability))]
    public class AbilityDex : IEnumerable<Ability>
    {
        public List<Ability> Abilities;

        public AbilityDex()
        {
            Abilities = new List<Ability>();
        }

        public AbilityDex(List<Ability> items)
        {
            Abilities = items;
        }

        public void Add(object o)
        {
            Abilities.Add(o as Ability);
        }

        IEnumerator<Ability> IEnumerable<Ability>.GetEnumerator()
        {
            return Abilities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Abilities.GetEnumerator();
        }

        /// <summary>
        /// Gets a pokemon by its name, all languages are searched for the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Ability GetByName(string name)
        {
            name = name
                .Replace("-", string.Empty)
                .Replace(" ", string.Empty);

            return Abilities.SingleOrDefault(ability =>
                ability.Names.Any(n =>
                    n.name
                        .Replace("-", string.Empty)
                        .Replace(" ", string.Empty)
                    .Equals(name, StringComparison.InvariantCultureIgnoreCase)));
        }

        public List<Move.FlavorTextEntry> GetDescriptions(Move item, string languageCode)
        {
            return item.Flavor_Text_Entries.Where(text => text.language != null && text.language.name == languageCode)
                .ToList();
        }

        public List<string> GetAvailableLanguages()
        {
            return Abilities.FirstOrDefault()?.Names?.Select(name => name.language?.name).ToList();
        }
    }
}
