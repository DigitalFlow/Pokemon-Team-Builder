using System;
using Gtk;
using Pokemon.Team.Builder;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public partial class MainWindow : Gtk.Window
{
	private List<ComboBoxText> _comboBoxes;

	public MainWindow () : base (WindowType.Toplevel)
	{
		var builder = new Builder ();
		builder.AddFromFile ("PokeUI.glade");
		builder.Autoconnect (this);

		_comboBoxes = GetComboBoxes (new List<string>{
			"PokeComboBox1", 
			"PokeComboBox2",
			"PokeComboBox3",
			"PokeComboBox4",
			"PokeComboBox5",
			"PokeComboBox6"
		}, builder);

		InitializePokemonComboBoxes (_comboBoxes);
	}

	protected List<ComboBoxText> GetComboBoxes(List<string> controlNames, Builder builder) {
		var comboBoxes = new List<ComboBoxText> ();

		foreach (var name in controlNames) {
			var comboBox = (ComboBoxText)builder.GetObject (name);

			comboBoxes.Add (comboBox);
		}

		return comboBoxes;
	}

	protected void InitializePokemonComboBoxes(IEnumerable<ComboBoxText> comboBoxes){
		using (var httpClient = new HttpClientWrapper(new Uri("http://pokeapi.co/api/v2/")))
		{
			using(var pokemonMetaDataRetriever = new PokemonMetaDataRetriever(httpClient))
			{
				var pokedex = new PokedexManager (pokemonMetaDataRetriever).GetPokedex();

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
			.Select(value => new PokemonIdentifier(int.Parse(value)))
			.ToList();

		Task.Run(() => {
			using (var httpClient = new HttpClientWrapper(new Uri("http://3ds.pokemon-gl.com")))
			{
				using(var pokemonUsageRetriever = new PokemonUsageRetriever(httpClient))
				{
					var initialTeam = comboBoxValues;

					var pokemonProposer = new PokemonProposer (pokemonUsageRetriever);
					var proposedTeam = pokemonProposer.GetProposedPokemonByUsage (comboBoxValues);

					for (var i = 0; i < proposedTeam.Count; i++) {
						_comboBoxes [i].Entry.Text = proposedTeam [i].RankingPokemonInfo.ToString();
					}
				}
			}
		});
	}
}
