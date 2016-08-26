using Pokemon.Team.Builder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
	/// <summary>
	/// Most often used items
	/// </summary>
	public class ItemInfo : IRankable, IItem
    {
        public int Ranking { get; set; }
        public double UsageRate { get; set; }
        public string Name { get; set; }
        public int SequenceNumber { get; set; }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public bool Equals(IItem otherItem)
        {
            if (otherItem == null)
            {
                return false;
            }

            return Name.Equals(otherItem.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            var otherItem = obj as ItemInfo;

            if (otherItem == null)
            {
                return false;
            }

            return Equals(otherItem);
        }
    }
}
