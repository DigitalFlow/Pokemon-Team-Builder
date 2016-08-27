using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder
{
    public static class MathExtensions
    {
        public static double PercentageOf (this double value, double baseValue)
        {
            return value / baseValue * 100;
        }
    }
}
