using System;
using Gtk;
using Pokemon.Team.Builder;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public partial class MainWindow : Window
{
	private List<Tuple<Image, ComboBoxText, ComboBoxText, ComboBoxText, Button>> _controlSets;
	private Pokedex _pokedex;
	private Builder _builder;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		_builder = new Builder ();
		_builder.AddFromFile ("PokeUI.glade");
		_builder.Autoconnect (this);

		_controlSets = GetComboBoxes (new List<Tuple<string, string, string, string, string>>{
			Tuple.Create("PokeImage1", "PokeNrBox1", "PokeFormBox1", "PokeNameBox1", "PokeButton1"), 
			Tuple.Create("PokeImage2", "PokeNrBox2", "PokeFormBox2", "PokeNameBox2", "PokeButton2"),
			Tuple.Create("PokeImage3", "PokeNrBox3", "PokeFormBox3", "PokeNameBox3", "PokeButton3"),
			Tuple.Create("PokeImage4", "PokeNrBox4", "PokeFormBox4", "PokeNameBox4", "PokeButton4"),
			Tuple.Create("PokeImage5", "PokeNrBox5", "PokeFormBox5", "PokeNameBox5", "PokeButton5"),
			Tuple.Create("PokeImage6", "PokeNrBox6", "PokeFormBox6", "PokeNameBox6", "PokeButton6")
		});
	}

	protected List<Tuple<Image, ComboBoxText, ComboBoxText, ComboBoxText, Button>> GetComboBoxes(List<Tuple<string, string, string, string, string>> controlNames) {
		var comboBoxes = new List<Tuple<Image, ComboBoxText, ComboBoxText, ComboBoxText, Button>> ();

		foreach (var name in controlNames) {
			var controlSet = Tuple.Create(
				(Image) _builder.GetObject (name.Item1), 
				(ComboBoxText) _builder.GetObject (name.Item2),
				(ComboBoxText) _builder.GetObject (name.Item3), 
				(ComboBoxText) _builder.GetObject (name.Item4), 
				(Button) _builder.GetObject (name.Item5)
			);

			comboBoxes.Add (controlSet);
		}

		return comboBoxes;
	}

	protected void InitializePokemonComboBoxes(IEnumerable<Tuple<Image, ComboBoxText, ComboBoxText, ComboBoxText, Button>> comboBoxes){
		// Makes problems in windows, will have to investigate
		var loadWindow = (Window)_builder.GetObject ("LoadPokedexWindow");

		loadWindow.Show ();

		using (var httpClient = new HttpClientWrapper(new Uri("http://pokeapi.co/")))
		{
			using(var pokemonMetaDataRetriever = new PokemonMetaDataRetriever(httpClient))
			{
				_pokedex = new Pokedex(new PokedexManager (pokemonMetaDataRetriever).GetPokemon());

				foreach (var comboBox in comboBoxes) {
					
					comboBox.Item2.Entry.Completion = new EntryCompletion {
						Model = new ListStore(typeof(string)),
						TextColumn = 0
					};

					comboBox.Item4.Entry.Completion = new EntryCompletion {
						Model = new ListStore(typeof(string)),
						TextColumn = 0
					};

					foreach (var pokemon in _pokedex) {
						((ListStore)comboBox.Item2.Entry.Completion.Model).AppendValues (pokemon.Id);
						comboBox.Item2.AppendText (pokemon.Id.ToString());

						((ListStore)comboBox.Item4.Entry.Completion.Model).AppendValues (pokemon.Name);
						comboBox.Item4.AppendText (pokemon.Name);
					}
				}
			}
		}

		loadWindow.Hide ();
	}

	protected void OnPokemonSelectionById (object sender, EventArgs e) {
		var senderBox = _controlSets.Single(box => box.Item2 == sender);

		var value = senderBox.Item2.Entry.Text.Trim();
		var pokemonId = 0;

		var parseResult = int.TryParse (value, out pokemonId);

		// Exit on no or invalid input
		if (!parseResult ||  _pokedex.All(poke => poke.Id != pokemonId)) {
			ClearControlTuple (senderBox);
			return;
		}

		var pokemon = _pokedex.GetById (pokemonId);

		// Set ID box to pokemon ID, subtract one since box entry is zero-based whereas pokemon IDs are not
		if (senderBox.Item4.Entry.Text != pokemon.Name) 
		{
			senderBox.Item4.Entry.Text = pokemon.Name;
		}

		SetPicture (senderBox.Item1, pokemon);

	}

	protected void SetPicture(Image image, Pokemon.Team.Builder.Pokemon pokemon)
	{
		var pixBuf = new Gdk.Pixbuf (Convert.FromBase64String (pokemon.Image));
		image.Pixbuf = pixBuf;
	}

	protected void ClearControlTuple(Tuple<Image, ComboBoxText, ComboBoxText, ComboBoxText, Button> controlTuple) {
		controlTuple.Item1.Pixbuf = null;
		controlTuple.Item2.Entry.Text = string.Empty;
		controlTuple.Item3.Entry.Text = string.Empty;
		controlTuple.Item4.Entry.Text = string.Empty;
	}

	protected void OnPokemonSelectionByName (object sender, EventArgs e) {
		var senderBox = _controlSets.Single(box => box.Item4 == sender);

		var value = senderBox.Item4.Entry.Text.Trim();

		// Exit on no or invalid input
		if (string.IsNullOrEmpty (value) || _pokedex.All(poke => 
			!poke.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase))) {
			ClearControlTuple (senderBox);
			return;
		}

		var pokemon = _pokedex.GetByName (value);

		// Set ID box to pokemon ID, subtract one since box entry is zero-based whereas pokemon IDs are not
		senderBox.Item2.Active = pokemon.Id - 1;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnStateEvent (object sender, WindowStateEventArgs e) {
		InitializePokemonComboBoxes (_controlSets);
	}

	protected void OnClear (object sender, EventArgs e)
	{
		foreach (var ctrlSet in _controlSets) {
			ClearControlTuple (ctrlSet);
		}
	}

	protected void OnExit (object sender, EventArgs e)
	{
		Application.Quit ();
	}

	protected void OnProposeTeam (object sender, EventArgs e)
	{
		var initialTeam = _controlSets
			.Where(ctrl => !string.IsNullOrEmpty(ctrl.Item2.ActiveText) && Regex.IsMatch(ctrl.Item2.ActiveText, "^[0-9]+$"))
			.Select(ctrl => 
				{
					var pokemonId = new PokemonIdentifier(int.Parse(ctrl.Item2.ActiveText));

					if(!string.IsNullOrEmpty(ctrl.Item3.ActiveText)) {
						pokemonId.FormNo = ctrl.Item3.ActiveText;
					}

					return pokemonId;
				})
			.ToList();

		Task.Run(() => {
			using (var httpClient = new HttpClientWrapper(new Uri("http://3ds.pokemon-gl.com")))
			{
				using(var pokemonUsageRetriever = new PokemonUsageRetriever(httpClient))
				{
					var pokemonProposer = new PokemonProposer (pokemonUsageRetriever);
					var proposedTeam = pokemonProposer.GetProposedPokemonByUsage (initialTeam);

					for (var i = 0; i < proposedTeam.Count; i++) {
						_controlSets [i].Item2.Active = proposedTeam [i].RankingPokemonInfo.MonsNo - 1;
						_controlSets [i].Item3.Active = int.Parse(proposedTeam [i].RankingPokemonInfo.FormNo);
					}
				}
			}
		});
	}
}
