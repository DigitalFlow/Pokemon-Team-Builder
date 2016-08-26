using Pokemon.Team.Builder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model.Smogon
{ 
    [Serializable]
    public class SmogonItem : IItem
    {
        public string Name { get; set; }
        public int Ranking { get; set; }
        public double UsageRate { get; set; }

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
            var otherItem = obj as SmogonItem;

            if (otherItem == null)
            {
                return false;
            }

            return Equals(otherItem);
        }
    }
}
