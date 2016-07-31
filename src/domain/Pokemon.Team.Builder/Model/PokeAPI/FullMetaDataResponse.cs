using System;
using System.Collections.Generic;

namespace Pokemon.Team.Builder
{
	public class FullMetaDataResponse
	{
		public class Form
		{
			public string url { get; set; }
			public string name { get; set; }
		}

		public class Ability2
		{
			public string url { get; set; }
			public string name { get; set; }
		}

		public class Ability
		{
			public int slot { get; set; }
			public bool is_hidden { get; set; }
			public Ability2 ability { get; set; }
		}

		public class Stat2
		{
			public string url { get; set; }
			public string name { get; set; }
		}

		public class Stat
		{
			public Stat2 stat { get; set; }
			public int effort { get; set; }
			public int base_stat { get; set; }
		}

		public class MoveLearnMethod
		{
			public string url { get; set; }
			public string name { get; set; }
		}

		public class VersionGroup
		{
			public string url { get; set; }
			public string name { get; set; }
		}

		public class VersionGroupDetail
		{
			public MoveLearnMethod move_learn_method { get; set; }
			public int level_learned_at { get; set; }
			public VersionGroup version_group { get; set; }
		}

		public class Move2
		{
			public string url { get; set; }
			public string name { get; set; }
		}

		public class Move
		{
			public List<VersionGroupDetail> version_group_details { get; set; }
			public Move2 move { get; set; }
		}

		public class Sprites
		{
			public object back_female { get; set; }
			public object back_shiny_female { get; set; }
			public string back_default { get; set; }
			public object front_female { get; set; }
			public object front_shiny_female { get; set; }
			public string back_shiny { get; set; }
			public string front_default { get; set; }
			public string front_shiny { get; set; }
		}

		public class Species
		{
			public string url { get; set; }
			public string name { get; set; }
		}

		public class Version
		{
			public string url { get; set; }
			public string name { get; set; }
		}

		public class GameIndice
		{
			public Version version { get; set; }
			public int game_index { get; set; }
		}

		public class Type2
		{
			public string url { get; set; }
			public string name { get; set; }
		}

		public class Type
		{
			public int slot { get; set; }
			public Type2 type { get; set; }
		}

		public List<Form> forms { get; set; }
		public List<Ability> abilities { get; set; }
		public List<Stat> stats { get; set; }
		public string name { get; set; }
		public int weight { get; set; }
		public List<Move> moves { get; set; }
		public Sprites sprites { get; set; }
		public List<object> held_items { get; set; }
		public string location_area_encounters { get; set; }
		public int height { get; set; }
		public bool is_default { get; set; }
		public Species species { get; set; }
		public int id { get; set; }
		public int order { get; set; }
		public List<GameIndice> game_indices { get; set; }
		public int base_experience { get; set; }
		public List<Type> types { get; set; }
	}
}

