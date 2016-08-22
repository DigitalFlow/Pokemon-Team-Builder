using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Interfaces
{
    public interface IHappiness : IRankable
    {
        string Name { get; set; }
        double UsageRate { get; set; }
    }
}
