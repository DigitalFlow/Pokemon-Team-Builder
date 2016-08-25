using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Logic
{
    public class Move
    {
        public class Generation
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class Language
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class Name
        {
            public string name { get; set; }
            public Language language { get; set; }
        }

        public class Super
        {
            public object use_after { get; set; }
            public object use_before { get; set; }
        }

        public class UseBefore
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class UseAfter
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class Normal
        {
            public List<UseAfter> use_after { get; set; }
            public List<UseBefore> use_before { get; set; }
        }

        public class ContestCombos
        {
            public Super super { get; set; }
            public Normal normal { get; set; }
        }

        public class EffectEntry
        {
            public string short_effect { get; set; }
            public string effect { get; set; }
            public Language language { get; set; }
        }

        public class ContestType
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class ContestEffect
        {
            public string url { get; set; }
        }

        public class Type
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class Target
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class SuperContestEffect
        {
            public string url { get; set; }
        }

        public class VersionGroup
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class FlavorTextEntry
        {
            public string flavor_text { get; set; }
            public Language language { get; set; }
            public VersionGroup version_group { get; set; }
        }

        public class DamageClass
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class Category
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class Ailment
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class Meta
        {
            public Category category { get; set; }
            public int healing { get; set; }
            public int? max_turns { get; set; }
            public int drain { get; set; }
            public Ailment ailment { get; set; }
            public int stat_chance { get; set; }
            public int flinch_chance { get; set; }
            public int? min_hits { get; set; }
            public int ailment_chance { get; set; }
            public int crit_rate { get; set; }
            public int? min_turns { get; set; }
            public int? max_hits { get; set; }
        }

        // public object effect_chance { get; set; }
        public Generation generation { get; set; }
        // public List<object> stat_changes { get; set; }
        // public List<object> effect_changes { get; set; }
        public List<Name> Names { get; set; }
        public int Id { get; set; }
        // public List<object> machines { get; set; }
        public int? pp { get; set; }
        public ContestCombos Contest_Combos { get; set; }
        public List<EffectEntry> Effect_Entries { get; set; }
        public ContestType Contest_Type { get; set; }
        public int? Priority { get; set; }
        public ContestEffect Contest_Effect { get; set; }
        public Type type { get; set; }
        public int? Accuracy { get; set; }
        public int? Power { get; set; }
        // public List<object> past_values { get; set; }
        public Target target { get; set; }
        public SuperContestEffect Super_Contest_Effect { get; set; }
        public string name { get; set; }
        public List<FlavorTextEntry> Flavor_Text_Entries { get; set; }
        public DamageClass Damage_Class { get; set; }
        public Meta meta { get; set; }
    }
}
