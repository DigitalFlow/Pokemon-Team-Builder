using System;
using Gtk;
using Pokemon.Team.Builder.Model;

namespace Pokemon.Team.Builder.UI
{
	public class PokemonDetailWindow : Window
	{
		private Pokedex _pokedex;
		private DetailedPokemonInformation _pokeInfo;

		public PokemonDetailWindow(DetailedPokemonInformation pokeInfo, Pokedex pokedex) : base(WindowType.Toplevel) {
			_pokeInfo = pokeInfo;
			_pokedex = pokedex;

			Initialize ();

			this.ShowAll ();
		}

		public void Initialize()
		{
			var image = new Image ();
			var pokemon = _pokedex.GetById (_pokeInfo.RankingPokemonInfo.MonsNo);

			image.SetPicture (pokemon);

			var box = new Box (Orientation.Vertical, 0);

			box.Add (image);

			this.Add (box);
		}
	}
}

