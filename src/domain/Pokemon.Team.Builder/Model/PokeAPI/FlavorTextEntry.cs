using System;

namespace Pokemon.Team.Builder
{
	[Serializable]
	public class FlavorTextEntry
	{
		public Version version { get; set; }

        private string _flavor_text;
		public string flavor_text { get
            {
                return _flavor_text;
            }
            set {
                var replaced = value?.Replace("\f", "\n");
                _flavor_text = replaced;
            }
        }
		public Language language { get; set; }
	}
}

