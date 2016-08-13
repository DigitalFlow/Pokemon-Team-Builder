using System;

namespace Pokemon.Team.Builder
{
	[Serializable]
	public class FlavorTextEntry
	{
		public Version version { get; set; }
		public string flavor_text { get; set; }
		public Language language { get; set; }
	}
}

