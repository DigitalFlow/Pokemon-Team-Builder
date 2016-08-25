using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Logic
{
    public class Item
    {
        public class Category
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class Language
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class EffectEntry
        {
            public string short_effect { get; set; }
            public string effect { get; set; }
            public Language language { get; set; }
        }

        public class Sprites
        {
            public string @default { get; set; }
        }

        public class Generation
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class GameIndice
        {
            public Generation generation { get; set; }
            public int game_index { get; set; }
        }

        public class Name
        {
            public string name { get; set; }
            public Language language { get; set; }
        }

        public class Attribute
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class VersionGroup
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class FlavorTextEntry
        {
            public string text { get; set; }
            public VersionGroup version_group { get; set; }
            public Language language { get; set; }
        }

        public Category category { get; set; }
        public string name { get; set; }
        // public object Fling_Effect { get; set; }
        public List<EffectEntry> Effect_Entries { get; set; }
        // public List<object> Held_By_Pokemon { get; set; }
        public Sprites sprites { get; set; }
        public List<GameIndice> Game_Indices { get; set; }
        // public object Baby_Trigger_For { get; set; }
        public int Cost { get; set; }
        public List<Name> Names { get; set; }
        public List<Attribute> Attributes { get; set; }
        public List<FlavorTextEntry> Flavor_Text_Entries { get; set; }
        public int Id { get; set; }
        // public List<object> Machines { get; set; }
        // public object Fling_Power { get; set; }
    }
}
