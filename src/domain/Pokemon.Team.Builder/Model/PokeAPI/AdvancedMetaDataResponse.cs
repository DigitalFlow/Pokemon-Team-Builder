using System;
using System.Collections.Generic;

namespace Pokemon.Team.Builder
{
	public class AdvancedMetaDataResponse
	{
		public int capture_rate { get; set; }
		public Habitat habitat { get; set; }
		public Color color { get; set; }
		public bool forms_switchable { get; set; }
		public Shape shape { get; set; }
		public List<Name> names { get; set; }
		public int id { get; set; }
		public List<EggGroup> egg_groups { get; set; }
		public int base_happiness { get; set; }
		public Generation generation { get; set; }
		public List<FlavorTextEntry> flavor_text_entries { get; set; }
		public GrowthRate growth_rate { get; set; }
		public int hatch_counter { get; set; }
		public List<Genera> genera { get; set; }
		public EvolvesFromSpecies evolves_from_species { get; set; }
		public List<object> form_descriptions { get; set; }
		public List<Variety> varieties { get; set; }
		public string name { get; set; }
		public EvolutionChain evolution_chain { get; set; }
		public bool has_gender_differences { get; set; }
		public bool is_baby { get; set; }
		public int gender_rate { get; set; }
		public List<PalParkEncounter> pal_park_encounters { get; set; }
		public int order { get; set; }
		public List<PokedexNumber> pokedex_numbers { get; set; }
	}
}

