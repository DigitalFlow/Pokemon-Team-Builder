using System;

namespace Pokemon.Team.Builder.Model.Tiers
{
    [Serializable]
    public class BaseStats
    {
        public int hp { get; set; }
        public int atk { get; set; }
        public int def { get; set; }
        public int spa { get; set; }
        public int spd { get; set; }
        public int spe { get; set; }
    }
}
