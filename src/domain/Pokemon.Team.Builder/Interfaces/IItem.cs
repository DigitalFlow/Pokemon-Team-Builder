using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Interfaces
{
    public interface IItem : IRankable, IEquatable<IItem>
    {
        string Name { get; set; }
        double UsageRate { get; set; }
    }
}
