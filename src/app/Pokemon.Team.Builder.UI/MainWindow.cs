using System;
using Gtk;
using Pokemon.Team.Builder;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public partial class MainWindow: Gtk.Window
{
	private List<ComboBoxEntry> _comboBoxes;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();

		_comboBoxes = new List<ComboBoxEntry>{ PokemonSlot1, PokemonSlot2, PokemonSlot3, PokemonSlot4, PokemonSlot5, PokemonSlot6 };

		InitializePokemonComboBoxes (_comboBoxes);
	}

	protected void InitializePokemonComboBoxes(IEnumerable<ComboBoxEntry> comboBoxes){
		var pokedex = PokedexSerializer.DeserializePokedex ("pokedex.xml");

		foreach (var comboBox in comboBoxes) {
			comboBox.Entry.Completion = new EntryCompletion {
				Model = new ListStore(typeof(string)),
				TextColumn = 0
			};

			foreach (var pokemon in pokedex) {
				var text = pokemon.ToString();

				((ListStore)comboBox.Entry.Completion.Model).AppendValues (text);
				comboBox.AppendText (text);
			}
		}
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnExit (object sender, EventArgs e)
	{
		Application.Quit ();
	}

	protected void OnProposeTeam (object sender, EventArgs e)
	{
		var comboBoxValues = _comboBoxes.Select(box => box.Entry.Text)
			.Where(value => !string.IsNullOrEmpty(value))
			.Select(value => value.Substring(0, value.IndexOf('-')).Trim())
			.Where(value => Regex.IsMatch(value, "^[0-9]+$"))
			.Select(value => int.Parse(value))
			.ToList();

		using (var httpClient = new HttpClientWrapper(new Uri("http://3ds.pokemon-gl.com")))
		{
			using(var pokemonUsageRetriever = new PokemonUsageRetriever(httpClient))
			{
				var initialTeam = comboBoxValues;

				var pokemonProposer = new PokemonProposer (pokemonUsageRetriever);

				var proposedTeam = pokemonProposer.GetProposedPokemon (initialTeam);

				for (var i = 0; i < proposedTeam.Count; i++) {
					_comboBoxes [i].Entry.Text = proposedTeam [i].ToString();
				}
			}
		}
	}
}
