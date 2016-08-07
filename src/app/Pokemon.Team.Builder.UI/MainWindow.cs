using System;
using Gtk;
using Pokemon.Team.Builder;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLog;
using Pokemon.Team.Builder.Model;
using Pokemon.Team.Builder.UI;

public partial class MainWindow : Window
{
	private Logger _logger = NLog.LogManager.GetCurrentClassLogger();

	private const string BattleTypeConfigKey = "BattleType";

    private List<Tuple<Image, ComboBoxText, ComboBoxText, ComboBoxText, Button>> _controlSets;
    private Pokedex _pokedex;
    private Builder _builder;
    private bool _pokedexLoadExecuted;

	private Window _loadWindow;
	private ProgressBar _progressBar;
	private Window _waitWindow;
	private Dialog _chooseDialog;
	private ComboBoxText _chooseBox;

	private Grid _counters;
	private Grid _switchIns;
	private Grid _moves;

	private List<DetailedPokemonInformation> _latestTeam;

    public MainWindow() : base(WindowType.Toplevel)
    {
		try
		{
	        _builder = new Builder();
	        _builder.AddFromFile("PokeUI.glade");
	        _builder.Autoconnect(this);

			_counters = (Grid) _builder.GetObject ("OverViewCountersGrid");
			_switchIns = (Grid) _builder.GetObject ("OverViewSaveSwitchInsGrid");
			_moves = (Grid) _builder.GetObject ("OverViewMovesGrid");

			_loadWindow = (Window)_builder.GetObject("LoadPokedexWindow");
			_progressBar = (ProgressBar) _builder.GetObject ("PokedexProgressBar");
			_waitWindow = (Window)_builder.GetObject("WaitWindow");
			_chooseDialog = (Dialog)_builder.GetObject("ChooseDialog");
			_chooseBox = (ComboBoxText)_builder.GetObject("ChooseBox");

	        _controlSets = GetComboBoxes(new List<Tuple<string, string, string, string, string>>
			{
	            Tuple.Create("PokeImage1", "PokeNrBox1", "PokeFormBox1", "PokeNameBox1", "PokeButton1"),
	            Tuple.Create("PokeImage2", "PokeNrBox2", "PokeFormBox2", "PokeNameBox2", "PokeButton2"),
	            Tuple.Create("PokeImage3", "PokeNrBox3", "PokeFormBox3", "PokeNameBox3", "PokeButton3"),
	            Tuple.Create("PokeImage4", "PokeNrBox4", "PokeFormBox4", "PokeNameBox4", "PokeButton4"),
	            Tuple.Create("PokeImage5", "PokeNrBox5", "PokeFormBox5", "PokeNameBox5", "PokeButton5"),
	            Tuple.Create("PokeImage6", "PokeNrBox6", "PokeFormBox6", "PokeNameBox6", "PokeButton6")
	        });
		}
		catch (Exception ex) {
			_logger.Error (ex);
		}
    }

    protected List<Tuple<Image, ComboBoxText, ComboBoxText, ComboBoxText, Button>> GetComboBoxes(List<Tuple<string, string, string, string, string>> controlNames)
    {
        var comboBoxes = new List<Tuple<Image, ComboBoxText, ComboBoxText, ComboBoxText, Button>>();

        foreach (var name in controlNames)
        {
            var controlSet = Tuple.Create(
                (Image)_builder.GetObject(name.Item1),
                (ComboBoxText)_builder.GetObject(name.Item2),
                (ComboBoxText)_builder.GetObject(name.Item3),
                (ComboBoxText)_builder.GetObject(name.Item4),
                (Button)_builder.GetObject(name.Item5)
            );

            comboBoxes.Add(controlSet);
        }

        return comboBoxes;
    }

	private void UpdateProgressBar (int count, int progress)
	{
		_progressBar.Text = $"{progress} / {count}";
		_progressBar.Fraction = progress / count;
		_progressBar.Pulse ();
	}

    protected async void InitializePokemonComboBoxes(IEnumerable<Tuple<Image, ComboBoxText, ComboBoxText, ComboBoxText, Button>> comboBoxes)
    {
		_loadWindow.Show();

        await Task.Run(() =>
        {
            using (var httpClient = new HttpClientWrapper(new Uri("http://pokeapi.co/")))
            {
                using (var pokemonMetaDataRetriever = new PokemonMetaDataRetriever(httpClient))
                {
					pokemonMetaDataRetriever.PokemonDataRetrievedEvent += UpdateProgressBar;

                    _pokedex = new PokedexManager(pokemonMetaDataRetriever).GetPokemon();

                    foreach (var comboBox in comboBoxes)
                    {
                        comboBox.Item2.Entry.Completion = new EntryCompletion
                        {
                            Model = new ListStore(typeof(string)),
                            TextColumn = 0
                        };

                        comboBox.Item4.Entry.Completion = new EntryCompletion
                        {
                            Model = new ListStore(typeof(string)),
                            TextColumn = 0
                        };

                        foreach (var pokemon in _pokedex)
                        {
                            ((ListStore)comboBox.Item2.Entry.Completion.Model).AppendValues(pokemon.Id);
                            comboBox.Item2.AppendText(pokemon.Id.ToString());

                            ((ListStore)comboBox.Item4.Entry.Completion.Model).AppendValues(pokemon.Name);
                            comboBox.Item4.AppendText(pokemon.Name);
                        }
                    }

					UpdateProgressBar(1, 1);
                }
            }
        });

		_loadWindow.Hide();
    }

    protected void OnPokemonSelectionById(object sender, EventArgs e)
    {
        var senderBox = _controlSets.Single(box => box.Item2 == sender);

        var value = senderBox.Item2.Entry.Text.Trim();
        var pokemonId = 0;

        var parseResult = int.TryParse(value, out pokemonId);

        // Exit on no or invalid input
        if (!parseResult || _pokedex.All(poke => poke.Id != pokemonId))
        {
            ClearControlTuple(senderBox);
            return;
        }

        var pokemon = _pokedex.GetById(pokemonId);

        // Set ID box to pokemon ID, subtract one since box entry is zero-based whereas pokemon IDs are not
        if (senderBox.Item4.Entry.Text != pokemon.Name)
        {
            senderBox.Item4.Entry.Text = pokemon.Name;
        }

        SetPicture(senderBox.Item1, pokemon);

    }

	protected void SetPicture(Image image, Pokemon.Team.Builder.Pokemon pokemon, int width = 96, int height = 96)
    {
        var pixBuf = new Gdk.Pixbuf(Convert.FromBase64String(pokemon.Image));
        image.Pixbuf = pixBuf.ScaleSimple (width, height, Gdk.InterpType.Nearest);
    }

    protected void ClearControlTuple(Tuple<Image, ComboBoxText, ComboBoxText, ComboBoxText, Button> controlTuple)
    {
		controlTuple.Item1.SetFromIconName("gtk-missing-image", IconSize.LargeToolbar);
        controlTuple.Item2.Entry.Text = string.Empty;
        controlTuple.Item3.Entry.Text = string.Empty;
        controlTuple.Item4.Entry.Text = string.Empty;
    }

    protected void OnPokemonSelectionByName(object sender, EventArgs e)
    {
        var senderBox = _controlSets.Single(box => box.Item4 == sender);

        var value = senderBox.Item4.Entry.Text.Trim();

        // Exit on no or invalid input
        if (string.IsNullOrEmpty(value) || _pokedex.All(poke =>  
            !poke.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
        {
            if (_pokedex.All(poke => !poke.Name.StartsWith(value, StringComparison.InvariantCultureIgnoreCase)))
            {
                ClearControlTuple(senderBox); 
            }

            return;
        }

        var pokemon = _pokedex.GetByName(value);

        // Set ID box to pokemon ID, subtract one since box entry is zero-based whereas pokemon IDs are not
        senderBox.Item2.Active = pokemon.Id - 1;
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

	protected void OnChooseBattleType(object sender, EventArgs e)
	{
		var battleTypes = new List<string>{ "Average of all others", "Singles", "Doubles", "Triples", "Rotation", "Specials" };

		foreach (var battlyType in battleTypes) {
			_chooseBox.AppendText (battlyType);	
		}

		_chooseBox.Active =int.Parse(ConfigManager.GetSetting (BattleTypeConfigKey));

		_chooseDialog.Show ();
	}

	protected void OnChooseDialogOk(object sender, EventArgs e)
	{
		var value = _chooseBox.Active;

		ConfigManager.WriteSetting (BattleTypeConfigKey, value.ToString ());
		_chooseDialog.Hide ();
	}

	protected void OnChooseDialogCancel(object sender, EventArgs e)
	{
		_chooseBox.RemoveAll ();
		_chooseDialog.Hide ();
	}

    protected void OnStateEvent(object sender, WindowStateEventArgs e)
    {
        if (!_pokedexLoadExecuted)
        {
            _pokedexLoadExecuted = true;
            InitializePokemonComboBoxes(_controlSets);
        }
    }

    protected void OnClear(object sender, EventArgs e)
    {
        foreach (var ctrlSet in _controlSets)
        {
            ClearControlTuple(ctrlSet);

			_counters.Children.ToList ().ForEach (child => child.Destroy ());
			_switchIns.Children.ToList ().ForEach (child => child.Destroy ());
			_moves.Children.ToList ().ForEach (child => child.Destroy ());
        }
    }

    protected void OnExit(object sender, EventArgs e)
    {
        Application.Quit();
    }

	protected void ProposeTeam(List<PokemonIdentifier> initialTeam)
	{
		using (var httpClient = new HttpClientWrapper(new Uri("http://3ds.pokemon-gl.com")))
		{
			using (var pokemonUsageRetriever = new PokemonUsageRetriever(httpClient))
			{
				var pokemonProposer = new PokemonProposer(pokemonUsageRetriever);

				var battleType = int.Parse(ConfigManager.GetSetting (BattleTypeConfigKey));

				_latestTeam = pokemonProposer.GetProposedPokemonByUsage(initialTeam, battleType);

				for (var i = 0; i < _latestTeam.Count; i++)
				{
					_controlSets[i].Item2.Active = _latestTeam[i].RankingPokemonInfo.MonsNo - 1;
					_controlSets[i].Item3.Active = int.Parse(_latestTeam[i].RankingPokemonInfo.FormNo);
				}

				var mostDangerousCounters = PokemonAnalyzer.GetRanking(_latestTeam, poke => poke.RankingPokemonDown, 10);

				for (var i = 0; i < mostDangerousCounters.Count; i++) {
					var counter = mostDangerousCounters [i];

					var j = 0;

					var image = new Image ();
					SetPicture (image, _pokedex.GetById(counter.MonsNo), 48, 48);

					_counters.Attach (image, ++j, i, 1, 1);
					_counters.Attach(new Label {
						Text = $"{counter.MonsNo}",
						Visible = true
					}, ++j, i, 1, 1);
					_counters.Attach(new Label {
						Text = $"{counter.FormNo}",
						Visible = true
					}, ++j, i, 1, 1);
					_counters.Attach(new Label {
						Text = $"{counter.Name}",
						Visible = true
					}, ++j, i, 1, 1);
				}

				_counters.ShowAll();

				var saveSwitchIns = PokemonAnalyzer.GetRanking(_latestTeam, poke => poke.RankingPokemonSufferer, 10,
					poke => mostDangerousCounters.All(counter => (PokemonIdentifier) poke != (PokemonIdentifier) counter));

				for (var i = 0; i < saveSwitchIns.Count; i++) {
					var switchIn = saveSwitchIns [i];

					var j = 0;

					var image = new Image ();
					SetPicture (image, _pokedex.GetById(switchIn.MonsNo), 48, 48);

					_switchIns.Attach (image, ++j, i, 1, 1);
					_switchIns.Attach(new Label {
						Text = $"{switchIn.MonsNo}",
						Visible = true
					}, ++j, i, 1, 1);
					_switchIns.Attach(new Label {
						Text = $"{switchIn.FormNo}",
						Visible = true
					}, ++j, i, 1, 1);
					_switchIns.Attach(new Label {
						Text = $"{switchIn.Name}",
						Visible = true
					}, ++j, i, 1, 1);
				}

				_switchIns.ShowAll();

				var dangerousMoves = PokemonAnalyzer.GetRanking(_latestTeam, poke => poke.RankingPokemonDownWaza, 10, rank => !string.IsNullOrEmpty(rank.WazaName));

				for (var i = 0; i < dangerousMoves.Count; i++) {
					var move = dangerousMoves [i];
					var j = 0;

					_moves.Attach(new Label {
						Text = $"{move.WazaName}",
						Visible = true
					}, ++j, i, 1, 1);
				}

				_moves.ShowAll();
			}
		}
	}

    protected async void OnProposeTeam(object sender, EventArgs e)
    {
		_waitWindow.Show ();

        var initialTeam = _controlSets
            .Where(ctrl => !string.IsNullOrEmpty(ctrl.Item2.ActiveText) && Regex.IsMatch(ctrl.Item2.ActiveText, "^[0-9]+$"))
            .Select(ctrl =>
                {
                    var pokemonId = new PokemonIdentifier(int.Parse(ctrl.Item2.ActiveText));

                    if (!string.IsNullOrEmpty(ctrl.Item3.ActiveText))
                    {
                        pokemonId.FormNo = ctrl.Item3.ActiveText;
                    }

                    return pokemonId;
                })
            .ToList();

        await Task.Run(() =>
        {
			ProposeTeam(initialTeam);
    	});

		_waitWindow.Hide();
    }
}
