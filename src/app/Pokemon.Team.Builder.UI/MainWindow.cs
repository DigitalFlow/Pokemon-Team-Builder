using System;
using Gtk;
using Pokemon.Team.Builder;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public partial class MainWindow : Window
{
	private List<ComboBoxText> _comboBoxes;
	private List<Pokemon.Team.Builder.Pokemon> _pokedex;
	private Builder _builder;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		_builder = new Builder ();
		_builder.AddFromFile ("PokeUI.glade");
		_builder.Autoconnect (this);

		_comboBoxes = GetComboBoxes (new List<string>{
			"PokeComboBox1", 
			"PokeComboBox2",
			"PokeComboBox3",
			"PokeComboBox4",
			"PokeComboBox5",
			"PokeComboBox6"
		});
	}

	protected List<ComboBoxText> GetComboBoxes(List<string> controlNames) {
		var comboBoxes = new List<ComboBoxText> ();

		foreach (var name in controlNames) {
			var comboBox = (ComboBoxText) _builder.GetObject (name);

			comboBoxes.Add (comboBox);
		}

		return comboBoxes;
	}

	protected void InitializePokemonComboBoxes(IEnumerable<ComboBoxText> comboBoxes){
		var loadWindow = (Window)_builder.GetObject ("LoadPokedexWindow");

		loadWindow.Show ();

		using (var httpClient = new HttpClientWrapper(new Uri("http://pokeapi.co/")))
		{
			using(var pokemonMetaDataRetriever = new PokemonMetaDataRetriever(httpClient))
			{
				_pokedex = new PokedexManager (pokemonMetaDataRetriever).GetPokedex();

				foreach (var comboBox in comboBoxes) {
					comboBox.Entry.Completion = new EntryCompletion {
						Model = new ListStore(typeof(string)),
						TextColumn = 0
					};

					foreach (var pokemon in _pokedex) {
						var text = pokemon.ToString();

						((ListStore)comboBox.Entry.Completion.Model).AppendValues (text);
						comboBox.AppendText (text);
					}
				}
			}
		}

		loadWindow.Hide ();
	}

	protected void OnPokemonSelection (object sender, EventArgs e) {
		var senderBox = _comboBoxes.Single(box => box == sender);

		// Ok since we only have 6 slots
		var slotNumber = int.Parse(senderBox.Name [senderBox.Name.Length - 1].ToString());

		if (string.IsNullOrEmpty (senderBox.Entry.Text) || senderBox.Entry.Text.IndexOf('-') == -1) {
			return;
		}

		var pokemonId = 0;

		if (!int.TryParse (senderBox.Entry.Text.Substring (0, senderBox.Entry.Text.IndexOf ('-')).Trim (), out pokemonId)) {
			return;
		}

		var pokemon = _pokedex.SingleOrDefault (poke => poke.Id == pokemonId);

		if (pokemon == null || pokemon.Image == null) {
			return;
		}

		var pixBuf = new Gdk.Pixbuf (Convert.FromBase64String (pokemon.Image));

		var image = (Image) _builder.GetObject ($"PokeImage{slotNumber}");

		image.Pixbuf = pixBuf;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnStateEvent (object sender, WindowStateEventArgs e) {
		InitializePokemonComboBoxes (_comboBoxes);
	}

	protected void OnExit (object sender, EventArgs e)
	{
		Application.Quit ();
	}

	protected void OnProposeTeam (object sender, EventArgs e)
	{
		var initialTeam = _comboBoxes.Select(box => box.Entry.Text)
			.Where(value => !string.IsNullOrEmpty(value) && value.IndexOf('-') != -1)
			.Select(value => value.Substring(0, value.IndexOf('-')).Trim())
			.Where(value => Regex.IsMatch(value, "^[0-9]+$"))
			.Select(value => new PokemonIdentifier(int.Parse(value)))
			.ToList();

		Task.Run(() => {
			using (var httpClient = new HttpClientWrapper(new Uri("http://3ds.pokemon-gl.com")))
			{
				using(var pokemonUsageRetriever = new PokemonUsageRetriever(httpClient))
				{
					var pokemonProposer = new PokemonProposer (pokemonUsageRetriever);
					var proposedTeam = pokemonProposer.GetProposedPokemonByUsage (initialTeam);

					for (var i = 0; i < proposedTeam.Count; i++) {
						_comboBoxes [i].Entry.Text = proposedTeam [i].RankingPokemonInfo.ToString();
					}
				}
			}
		});
	}
}
