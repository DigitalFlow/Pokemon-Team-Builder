using System;

namespace Pokemon.Team.Builder
{
	public class PokemonIdentifier : IEquatable<PokemonIdentifier>
	{
		public int MonsNo{ get; set; }
		public string FormNo { get; set; }
        public string Name { get; set; }

		public PokemonIdentifier()
		{
			MonsNo = 0;
			FormNo = "0";
		}

        public PokemonIdentifier(string name)
        {
            Name = name;
        }

		public PokemonIdentifier(int monsNo)
		{
			MonsNo = monsNo;
			FormNo = "0";
		}

		public PokemonIdentifier(int monsNo, string formNo)
		{
			MonsNo = monsNo;
			FormNo = "0";
		}

		public override string ToString ()
		{
			return $"{MonsNo}-{FormNo}";
		}

		public static bool operator == (PokemonIdentifier fst, PokemonIdentifier snd) {
			if (object.ReferenceEquals(fst, null)) {
				if (object.ReferenceEquals(snd, null)) {
					return true;
				}
				return false;
			}

			return fst.Equals (snd);
		}

		public static bool operator != (PokemonIdentifier fst, PokemonIdentifier snd) {
			if (object.ReferenceEquals(fst, null)) {
				if (object.ReferenceEquals(snd, null)) {
					return false;
				}
				return true;
			}

			return !fst.Equals (snd);
		}

		public bool Equals (PokemonIdentifier otherId) {
			if (otherId == null) {
				return false;
			}

			return MonsNo == otherId.MonsNo && FormNo == otherId.FormNo && Name == otherId.Name;
		}

		public override bool Equals (object obj)
		{
			var otherId = obj as PokemonIdentifier;

			if (otherId == null) {
				return false;
			}

			return Equals(otherId);
		}

		public override int GetHashCode ()
		{
			return $"{MonsNo}-{FormNo}".GetHashCode();
		}
	}
}

