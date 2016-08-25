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
    [XmlRoot("Movedex")]
    [XmlInclude(typeof(Move))]
    public class Movedex : IEnumerable<Move>
    {
        public List<Move> Moves;

        public Movedex()
        {
            Moves = new List<Move>();
        }

        public Movedex(List<Move> moves)
        {
            Moves = moves;
        }

        public void Add(object o)
        {
            Moves.Add(o as Move);
        }

        IEnumerator<Move> IEnumerable<Move>.GetEnumerator()
        {
            return Moves.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Moves.GetEnumerator();
        }

        /// <summary>
        /// Gets a pokemon by its name, all languages are searched for the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Move GetByName(string name)
        {
            name = name
                .Replace("-", string.Empty)
                .Replace(" ", string.Empty);

            return Moves.SingleOrDefault(item =>
                item.Names.Any(n =>
                    n.name
                        .Replace("-", string.Empty)
                        .Replace(" ", string.Empty)
                    .Equals(name, StringComparison.InvariantCultureIgnoreCase)));
        }

        public List<Move.FlavorTextEntry> GetDescriptions(Move move, string languageCode)
        {
            return move.Flavor_Text_Entries.Where(text => text.language != null && text.language.name == languageCode)
                .ToList();
        }

        public List<string> GetAvailableLanguages()
        {
            return Moves.FirstOrDefault()?.Names?.Select(name => name.language?.name).ToList();
        }
    }
}
