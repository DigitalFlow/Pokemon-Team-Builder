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
using Pokemon.Team.Builder.Serialization;
using Pokemon.Team.Builder.Interfaces;
using Pokemon.Team.Builder.Logic;
using System.Text;

public partial class MainWindow : Window, IDisposable
{
    private Logger _logger = NLog.LogManager.GetCurrentClassLogger();

    private const string BattleTypeConfigKey = "BattleType";
    private const string SeasonConfigKey = "Season";
    private const string TierConfigKey = "Tier";
    private const string AvailableTiersConfigKey = "PathToAvailableTiersXml";
    private const string TierListConfigKey = "PathToTierListXml";
    private const string RankingPokemonInCountConfigKey = "RankingPokemonInCount";
    private const string RankingPokemonDownCountConfigKey = "RankingPokemonDownCount";
    private const string LanguageConfigKey = "Language";
	private const string ProviderConfigKey = "UsageProvider";

	private readonly List<string> _battleTypes = new List<string> { "Average of all others", "Singles", "Doubles", "Triples", "Rotation", "Specials" };

    private List<Tuple<Image, ComboBoxText, ComboBoxText, ComboBoxText, Button>> _controlSets;

    private Pokedex _pokedex;
    private Itemdex _itemdex;
    private Movedex _movedex;
    private AbilityDex _abilitydex;

    private Builder _builder;
    private bool _pokedexLoadExecuted;

    private Window _loadWindow;
    private ProgressBar _progressBar;
    private Window _waitWindow;
    private Dialog _chooseDialog;
    private Label _chooseLabel;
    private ComboBox _chooseBox;
    private Button _dialogBoxOk;
    private Toolbar _toolbar;

    private Grid _counters;
    private Grid _switchIns;
    private Grid _moves;

    private TierList _tierList;
    private TierHierarchy _tierHierarchy;

    private Team _latestTeam = new Team();
    private List<IPokemonUsageRetriever> _usageRetrievers = new List<IPokemonUsageRetriever>();

    public MainWindow() : base(WindowType.Toplevel)
    {
        try
        {
            _builder = new Builder();
            _builder.AddFromFile("PokeUI.glade");
            _builder.Autoconnect(this);

            _counters = (Grid)_builder.GetObject("OverViewCountersGrid");
            _switchIns = (Grid)_builder.GetObject("OverViewSaveSwitchInsGrid");
            _moves = (Grid)_builder.GetObject("OverViewMovesGrid");

            _loadWindow = (Window)_builder.GetObject("LoadPokedexWindow");
            _progressBar = (ProgressBar)_builder.GetObject("PokedexProgressBar");
            _waitWindow = (Window)_builder.GetObject("WaitWindow");
            _chooseDialog = (Dialog)_builder.GetObject("ChooseDialog");
            _chooseLabel = (Label)_builder.GetObject("ChooseLabel");
            _chooseBox = (ComboBox)_builder.GetObject("ChooseBox");
            _dialogBoxOk = (Button)_builder.GetObject("ChooseOk");
            _toolbar = (Toolbar)_builder.GetObject("StatusBar");

            _controlSets = GetComboBoxes(new List<Tuple<string, string, string, string, string>>
            {
                Tuple.Create("PokeImage1", "PokeNrBox1", "PokeFormBox1", "PokeNameBox1", "PokeButton1"),
                Tuple.Create("PokeImage2", "PokeNrBox2", "PokeFormBox2", "PokeNameBox2", "PokeButton2"),
                Tuple.Create("PokeImage3", "PokeNrBox3", "PokeFormBox3", "PokeNameBox3", "PokeButton3"),
                Tuple.Create("PokeImage4", "PokeNrBox4", "PokeFormBox4", "PokeNameBox4", "PokeButton4"),
                Tuple.Create("PokeImage5", "PokeNrBox5", "PokeFormBox5", "PokeNameBox5", "PokeButton5"),
                Tuple.Create("PokeImage6", "PokeNrBox6", "PokeFormBox6", "PokeNameBox6", "PokeButton6")
            });

            CreateStatusItems();
        }
        catch (Exception ex)
        {
            _logger.Error(ex);
        }
    }

    private void CreateStatusItems()
    {
        Application.Invoke(delegate
        {
            _toolbar.Children.ToList().ForEach(child => child.Destroy());

          	var activeTierLabel = new ToolItem();
			activeTierLabel.Add(new Label { Text = "Active Tier", MarginLeft = 10, MarginRight = 10 });
			
			var activeTier = new ToolItem();
			var activeTierButton = new Button { Label = ConfigManager.GetSetting(TierConfigKey), MarginLeft = 10, MarginRight = 10 };

			activeTierButton.Clicked += OnSelectTier;

			activeTier.Add(activeTierButton);

			var activeProposerLabel = new ToolItem();
			activeProposerLabel.Add(new Label { Text = "Active Provider", MarginLeft = 10, MarginRight = 10 });
            
			var activeProposer = new ToolItem();
			var activeProposerButton = new Button { Label = ConfigManager.GetSetting(ProviderConfigKey), MarginLeft = 10, MarginRight = 10 };

			activeProposerButton.Clicked += OnChooseProvider;

			activeProposer.Add(activeProposerButton);

			var activeLanguageLabel = new ToolItem();
			activeLanguageLabel.Add(new Label { Text = "Active Language", MarginLeft = 10, MarginRight = 10 });
            
			var activeLanguage = new ToolItem();
			var activeLanguageButton = new Button { Label = ConfigManager.GetSetting(LanguageConfigKey), MarginLeft = 10, MarginRight = 10 };

			activeLanguageButton.Clicked += OnSelectLanguage;

			activeLanguage.Add(activeLanguageButton);

			var activeBattleTypeLabel = new ToolItem();
				activeBattleTypeLabel.Add(new Label { Text = "Active Battle Type", MarginLeft = 10, MarginRight = 10 });
            
			var activeBattleType = new ToolItem();
			var battleType = 0;
			int.TryParse(ConfigManager.GetSetting(BattleTypeConfigKey), out battleType);
			var activeBattleTypeButton = new Button { Label = _battleTypes[battleType], MarginLeft = 10, MarginRight = 10 };

			activeBattleTypeButton.Clicked += OnChooseBattleType;

			activeBattleType.Add(activeBattleTypeButton);

            _toolbar.Add(activeTierLabel);
            _toolbar.Add(activeTier);

            _toolbar.Add(activeProposerLabel);
            _toolbar.Add(activeProposer);

            _toolbar.Add(activeLanguageLabel);
            _toolbar.Add(activeLanguage);

            _toolbar.Add(activeBattleTypeLabel);
			_toolbar.Add(activeBattleType);

            _toolbar.ShowAll();
        });
    }

    private void InitializeUsageRetrievers()
    {
        var glHttpClient = new HttpClientWrapper(new Uri(ConfigManager.GetSetting("PokeGLUrl")));
        var glPokemonUsageRetriever = new PokemonGlUsageRetriever(glHttpClient);
        var smogonHttpClient = new HttpClientWrapper(new Uri("http://www.smogon.com/stats/"));
        var smogonPokemonUsageRetriever = new SmogonStatManager(_pokedex, smogonHttpClient);

        _usageRetrievers.Add(glPokemonUsageRetriever);
        _usageRetrievers.Add(smogonPokemonUsageRetriever);
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

    private void UpdateProgressBar(int count, int progress)
    {
        // We need to use Application.Invoke since we receive events on another thread.
        // Only the main thread may access UI components.
        // By using Application.Invoke, we're sure that the main thread is used for the actions.
        Application.Invoke(delegate
        {
            _progressBar.Text = $"{progress} / {count}";
            _progressBar.Fraction = progress / count;
            _progressBar.Pulse();
        });
    }

    protected async Task<TierList> GetTierList()
    {
        using (var httpClient = new HttpClientWrapper(new Uri(ConfigManager.GetSetting("PokeShowdownUrl"))))
        {
            using (var tierRetriever = new TierListRetriever(httpClient))
            {
                var tierManager = new TierListManager(tierRetriever);

                return await tierManager.GetTierList(ConfigManager.GetSetting(TierListConfigKey)).ConfigureAwait(false);
            }
        }
    }

    protected List<Tier> GetTiers()
    {
        return TierSerializer.ParseFromFile(ConfigManager.GetSetting(AvailableTiersConfigKey));
    }

    protected async Task InitializeItemdex()
    {
        Application.Invoke(delegate
        {
            _loadWindow.Title = "Loading Itemdex";
            _loadWindow.Show();
        });

        await Task.Run(async () =>
        {
            using (var httpClient = new HttpClientWrapper(new Uri(ConfigManager.GetSetting("PokeApiUrl"))))
            {
                using (var pokemonMetaDataRetriever = new PokemonMetaDataRetriever(httpClient))
                {
                    pokemonMetaDataRetriever.PokemonDataRetrievedEvent += UpdateProgressBar;
                    var itemdexManager = new ItemdexManager(pokemonMetaDataRetriever);

                    _itemdex = await itemdexManager.GetItems().ConfigureAwait(false);
                }
            }
        }).ConfigureAwait(false);

        Application.Invoke(delegate
        {
            _loadWindow.Hide();
        });
    }

    protected async Task InitializeMovedex()
    {
        Application.Invoke(delegate
        {
            _loadWindow.Title = "Loading Movedex";
            _loadWindow.Show();
        });

        await Task.Run(async () =>
        {
            using (var httpClient = new HttpClientWrapper(new Uri(ConfigManager.GetSetting("PokeApiUrl"))))
            {
                using (var pokemonMetaDataRetriever = new PokemonMetaDataRetriever(httpClient))
                {
                    pokemonMetaDataRetriever.PokemonDataRetrievedEvent += UpdateProgressBar;
                    var movedexManager = new MovedexManager(pokemonMetaDataRetriever);

                    _movedex = await movedexManager.GetMoves().ConfigureAwait(false);
                }
            }
        }).ConfigureAwait(false);

        Application.Invoke(delegate
        {
            _loadWindow.Hide();
        });
    }

    protected async Task InitializeAbilitydex()
    {
        Application.Invoke(delegate
        {
            _loadWindow.Title = "Loading Abilitydex";
            _loadWindow.Show();
        });

        await Task.Run(async () =>
        {
            using (var httpClient = new HttpClientWrapper(new Uri(ConfigManager.GetSetting("PokeApiUrl"))))
            {
                using (var pokemonMetaDataRetriever = new PokemonMetaDataRetriever(httpClient))
                {
                    pokemonMetaDataRetriever.PokemonDataRetrievedEvent += UpdateProgressBar;
                    var abilitydexManager = new AbilitydexManager(pokemonMetaDataRetriever);

                    _abilitydex = await abilitydexManager.GetAbilities().ConfigureAwait(false);
                }
            }
        }).ConfigureAwait(false);

        Application.Invoke(delegate
        {
            _loadWindow.Hide();
        });
    }

    protected async void InitializePokemonComboBoxes(IEnumerable<Tuple<Image, ComboBoxText, ComboBoxText, ComboBoxText, Button>> comboBoxes)
    {
        Application.Invoke(delegate
        {
            _loadWindow.Title = "Loading Pokedex";
            _loadWindow.Show();
        });

        await Task.Run(async () =>
        {
            using (var httpClient = new HttpClientWrapper(new Uri(ConfigManager.GetSetting("PokeApiUrl"))))
            {
                using (var pokemonMetaDataRetriever = new PokemonMetaDataRetriever(httpClient))
                {
                    pokemonMetaDataRetriever.PokemonDataRetrievedEvent += UpdateProgressBar;
                    var pokedexManager = new PokedexManager(pokemonMetaDataRetriever);

                    _pokedex = await pokedexManager.GetPokemon().ConfigureAwait(false);
                }
            }
        }).ConfigureAwait(false);

        FillComboBoxes(comboBoxes);
        InitializeUsageRetrievers();

        Application.Invoke(delegate
        {
            _loadWindow.Hide();
        });
    }

    protected void FillComboBoxes(IEnumerable<Tuple<Image, ComboBoxText, ComboBoxText, ComboBoxText, Button>> comboBoxes)
    {
        foreach (var comboBox in comboBoxes)
        {
            Application.Invoke(delegate
            {
                comboBox.Item2.Entry.Completion = new EntryCompletion
                {
                    Model = new ListStore(typeof(string)),
                    TextColumn = 0
                };

                // Initialize explicitly for clearing on language selection
                comboBox.Item4.Model = new ListStore(typeof(string));

                comboBox.Item4.Entry.Completion = new EntryCompletion
                {
                    Model = new ListStore(typeof(string)),
                    TextColumn = 0
                };

                var language = ConfigManager.GetSetting(LanguageConfigKey);

                foreach (var pokemon in _pokedex)
                {
                    ((ListStore)comboBox.Item2.Entry.Completion.Model).AppendValues(pokemon.Id);
                    comboBox.Item2.AppendText(pokemon.Id.ToString());

                    var name = pokemon.GetName(language);

                    ((ListStore)comboBox.Item4.Entry.Completion.Model).AppendValues(name);
                    comboBox.Item4.AppendText(name);
                }
            });
        }

        UpdateProgressBar(1, 1);
    }

    protected void OnPokemonSelectionById(object sender, EventArgs e)
    {
        var senderBox = _controlSets.Single(box => box.Item2 == sender);

        var value = senderBox.Item2.Entry.Text.Trim();
        var pokemonId = 0;

        var parseResult = int.TryParse(value, out pokemonId);

        // Exit on no or invalid input
        if (!parseResult)
        {
            ClearControlTuple(senderBox);
            return;
        }

        var pokemon = _pokedex.GetById(pokemonId);

        if (pokemon == null)
        {
            return;
        }

        var language = ConfigManager.GetSetting(LanguageConfigKey);
        var name = pokemon.GetName(language);
        var englishName = pokemon.GetName("en");

        // Set name box to pokemon name, subtract one since box entry is zero-based whereas pokemon IDs are not
        if (senderBox.Item4.Active != pokemonId - 1)
        {
            senderBox.Item4.Active = pokemonId - 1;
        }

        senderBox.Item3.Model = new ListStore(typeof(string));

        senderBox.Item3.Entry.Completion = new EntryCompletion
        {
            Model = new ListStore(typeof(string)),
            TextColumn = 0
        };

        foreach (var variety in pokemon.Varieties)
        {
            var varietyName = variety.pokemon.name;

            varietyName = varietyName.Replace(englishName.ToLowerInvariant(), name);

            ((ListStore)senderBox.Item3.Entry.Completion.Model).AppendValues(varietyName);
            senderBox.Item3.AppendText(varietyName);
        }

        if (pokemon.Varieties.Count > 0)
        {
            senderBox.Item3.Active = 0;
        }

        senderBox.Item1.SetPicture(pokemon);
    }

    protected void ClearControlTuple(Tuple<Image, ComboBoxText, ComboBoxText, ComboBoxText, Button> controlTuple)
    {
        controlTuple.Item1.SetFromIconName("gtk-missing-image", IconSize.LargeToolbar);
        controlTuple.Item2.Entry.Text = string.Empty;
        controlTuple.Item3.Entry.Text = string.Empty;
        controlTuple.Item4.Entry.Text = string.Empty;
    }

    protected void OnMoreInformationClicked(object sender, EventArgs e)
    {
        var senderTuple = _controlSets.Single(ctrl => ctrl.Item5 == sender);

        // Zero-Based in ComboBox
        var selectedPokemonId = senderTuple.Item2.Active + 1;

        var pokemonToShow = _latestTeam
            .TeamMembers
            .Where(poke => poke.Identifier.MonsNo == selectedPokemonId)
            .ToList();

        if (pokemonToShow.Count != 1)
        {
            return;
        }

        new PokemonDetailWindow(pokemonToShow.Single(), _pokedex, _itemdex, _movedex, _abilitydex, _tierList, ConfigManager.GetSetting(LanguageConfigKey));
    }

    protected void OnSelectTier(object sender, EventArgs e)
    {
        var tiers = _tierHierarchy.GetAllWithSubTiers();

        var activeTier = ConfigManager.GetSetting(TierConfigKey);

        var listStore = new ListStore(typeof(string));

        _chooseLabel.Text = "Select your Tier";
        _chooseBox.Model = listStore;

        var renderer = new CellRendererText();
        _chooseBox.PackStart(renderer, false);
        _chooseBox.AddAttribute(renderer, "text", 0);

        for (var i = 0; i < tiers.Count; i++)
        {
            listStore.AppendValues(tiers[i]);

            if (tiers[i].Equals(activeTier, StringComparison.InvariantCultureIgnoreCase))
            {
                _chooseBox.Active = i;
            }
        }

        _dialogBoxOk.Clicked += OnChooseTierOk;

        _chooseDialog.Show();
    }

	protected void OnChooseProvider(object sender, EventArgs e)
	{
		var providers = _usageRetrievers
			.Select(retriever => retriever.GetType().Name)
			.ToList();

		var activeProvider = ConfigManager.GetSetting(ProviderConfigKey);

		var listStore = new ListStore(typeof(string));

		_chooseLabel.Text = "Select Usage Provider";
		_chooseBox.Model = listStore;

		var renderer = new CellRendererText();
		_chooseBox.PackStart(renderer, false);
		_chooseBox.AddAttribute(renderer, "text", 0);

		for (var i = 0; i < providers.Count; i++)
		{
			listStore.AppendValues(providers[i]);

			if (providers[i].Equals(activeProvider, StringComparison.InvariantCultureIgnoreCase))
			{
				_chooseBox.Active = i;
			}
		}

		_dialogBoxOk.Clicked += OnChooseProviderOk;

		_chooseDialog.Show();
	}


    protected void OnSelectLanguage(object sender, EventArgs e)
    {
        var availableLanguages = _pokedex.GetAvailableLanguages();
        var activeLanguage = ConfigManager.GetSetting(LanguageConfigKey);

        var listStore = new ListStore(typeof(string));

        _chooseLabel.Text = "Select your Language";
        _chooseBox.Model = listStore;

        var renderer = new CellRendererText();
        _chooseBox.PackStart(renderer, false);
        _chooseBox.AddAttribute(renderer, "text", 0);

        for (var i = 0; i < availableLanguages.Count; i++)
        {
            listStore.AppendValues(availableLanguages[i]);

            if (availableLanguages[i].Equals(activeLanguage, StringComparison.InvariantCultureIgnoreCase))
            {
                _chooseBox.Active = i;
            }
        }

        _dialogBoxOk.Clicked += OnChooseLanguageOk;

        _chooseDialog.Show();
    }

    protected void OnPokemonSelectionByName(object sender, EventArgs e)
    {
        var senderBox = _controlSets.Single(box => box.Item4 == sender);

        var value = senderBox.Item4.Entry.Text.Trim();

        // Exit on no or invalid input
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        var pokemon = _pokedex.GetByName(value);

        if (pokemon == null)
        {
            return;
        }

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
        var listStore = new ListStore(typeof(string));

        _chooseLabel.Text = "Select your Battle Type";
        _chooseBox.Model = listStore;

        var renderer = new CellRendererText();
        _chooseBox.PackStart(renderer, false);
        _chooseBox.AddAttribute(renderer, "text", 0);

        foreach (var battleType in _battleTypes)
        {
            listStore.AppendValues(battleType);
        }

        _chooseBox.Active = int.Parse(ConfigManager.GetSetting(BattleTypeConfigKey));

        _dialogBoxOk.Clicked += OnChooseBattleTypeOk;

        _chooseDialog.Show();
    }

    protected void OnChooseTierOk(object sender, EventArgs e)
    {
        TreeIter tree;
        _chooseBox.GetActiveIter(out tree);

        var tierName = (string)_chooseBox.Model.GetValue(tree, 0);

        ConfigManager.WriteSetting(TierConfigKey, tierName);
        ResetChooseDialog();
        CreateStatusItems();
    }

	protected void OnChooseProviderOk(object sender, EventArgs e)
	{
		TreeIter tree;
		_chooseBox.GetActiveIter(out tree);

		var providername = (string)_chooseBox.Model.GetValue(tree, 0);

		ConfigManager.WriteSetting(ProviderConfigKey, providername);
		ResetChooseDialog();
        CreateStatusItems();
    }

    protected void OnChooseLanguageOk(object sender, EventArgs e)
    {
        TreeIter tree;
        _chooseBox.GetActiveIter(out tree);

        var languageName = (string)_chooseBox.Model.GetValue(tree, 0);

        ConfigManager.WriteSetting(LanguageConfigKey, languageName);

        FillComboBoxes(_controlSets);

        ResetChooseDialog();
        CreateStatusItems();
    }

    protected void OnChooseBattleTypeOk(object sender, EventArgs e)
    {
        var value = _chooseBox.Active;

        ConfigManager.WriteSetting(BattleTypeConfigKey, value.ToString());

        ResetChooseDialog();
        CreateStatusItems();
    }

    private void ResetChooseDialog()
    {
        _dialogBoxOk.Clicked -= OnChooseTierOk;
        _dialogBoxOk.Clicked -= OnChooseLanguageOk;
        _dialogBoxOk.Clicked -= OnChooseBattleTypeOk;
		_dialogBoxOk.Clicked -= OnChooseProviderOk;
        _chooseBox.Clear();
        _chooseDialog.Hide();
    }

    protected void OnChooseDialogCancel(object sender, EventArgs e)
    {
        ResetChooseDialog();
    }

    protected async void OnStateEvent(object sender, WindowStateEventArgs e)
    {
        if (!_pokedexLoadExecuted)
        {
            _pokedexLoadExecuted = true;
            _tierList = await GetTierList().ConfigureAwait(false);
            _tierHierarchy = new TierHierarchy(GetTiers());

            await InitializeItemdex().ConfigureAwait(false);
            await InitializeMovedex().ConfigureAwait(false);
            await InitializeAbilitydex().ConfigureAwait(false);

            InitializePokemonComboBoxes(_controlSets);
        }
    }

    protected void OnClear(object sender, EventArgs e)
    {
        foreach (var ctrlSet in _controlSets)
        {
            ClearControlTuple(ctrlSet);

            _counters.Children.ToList().ForEach(child => child.Destroy());
            _switchIns.Children.ToList().ForEach(child => child.Destroy());
            _moves.Children.ToList().ForEach(child => child.Destroy());
        }
    }

    protected void OnExit(object sender, EventArgs e)
    {
        Application.Quit();
    }

    protected async Task ProposeTeam(List<PokemonIdentifier> initialTeam)
    {
		var pokemonUsageRetriever = _usageRetrievers
			.SingleOrDefault(retriever => retriever.GetType().Name.Equals(ConfigManager.GetSetting(ProviderConfigKey), StringComparison.InvariantCultureIgnoreCase));

        var activeTierName = ConfigManager.GetSetting(TierConfigKey);

        var activeTier = _tierHierarchy.GetByShortName(activeTierName);

        if (activeTier == null)
        {
            _logger.Error($"Team {activeTierName} is invalid, can't propose team");
            return;
        }

        var battleType = int.Parse(ConfigManager.GetSetting(BattleTypeConfigKey));
        var season = int.Parse(ConfigManager.GetSetting(SeasonConfigKey));
        var rankingPokemonInCount = int.Parse(ConfigManager.GetSetting(RankingPokemonInCountConfigKey));
        var rankingPokemonDownCount = int.Parse(ConfigManager.GetSetting(RankingPokemonDownCountConfigKey));
        var languageCode = ConfigManager.GetSetting(LanguageConfigKey);

        var languageId = languageCode.ToLanguageId();

        var pokemonProposer = new PokemonProposer(pokemonUsageRetriever, battleType, season, rankingPokemonInCount, rankingPokemonDownCount,
            languageId, _tierList, activeTier, _pokedex);

        _latestTeam = new Team(await pokemonProposer.GetProposedPokemonByUsage(initialTeam).ConfigureAwait(false));

        Application.Invoke(delegate
        {
            for (var i = 0; i < _latestTeam.TeamMembers.Count; i++)
            {
                _controlSets[i].Item2.Active = _latestTeam.TeamMembers[i].Identifier.MonsNo - 1;

                var formNo = 0;

                int.TryParse(_latestTeam.TeamMembers[i].Identifier.FormNo, out formNo);

                _controlSets[i].Item3.Active = formNo;
            }
        });

        var mostDangerousCounters = PokemonAnalyzer.GetRanking(_latestTeam.TeamMembers, poke => poke.GetCounters(), 10);

        Application.Invoke(delegate
        {
            _counters
            .AddItems(mostDangerousCounters,
                new List<Func<ICounter, Widget>> {
                            rank => new Image ().SetPicture (_pokedex.GetById (rank.Identifier.MonsNo), 48, 48),
                            rank => new Label (rank.Identifier.MonsNo.ToString ()),
                            rank => new Label (_pokedex.GetByIdentifier(rank.Identifier).GetName(ConfigManager.GetSetting(LanguageConfigKey)))
                });

            _counters.ShowAll();
        });

        //var saveSwitchIns = PokemonAnalyzer.GetRanking(_latestTeam, poke => poke.RankingPokemonSufferer, 10,
        //    poke => mostDangerousCounters.All(counter => (PokemonIdentifier)poke != (PokemonIdentifier)counter));

        //Application.Invoke(delegate
        //{
        //    _switchIns
        //    .AddItems(saveSwitchIns,
        //        new List<Func<RankingPokemonSufferer, Widget>> {
        //            rank => new Image ().SetPicture (_pokedex.GetById (rank.MonsNo), 48, 48),
        //            rank => new Label (rank.MonsNo.ToString ()),
        //            rank => new Label (rank.FormNo),
        //            rank => new Label (rank.Name)
        //        });

        //    _switchIns.ShowAll();
        //});

        //var dangerousMoves = PokemonAnalyzer.GetRanking(_latestTeam, poke => poke.RankingPokemonDownWaza, 10, rank => !string.IsNullOrEmpty(rank.WazaName));

        //Application.Invoke(delegate
        //{
        //    _moves
        //    .AddItems(dangerousMoves,
        //        new List<Func<RankingPokemonDownWaza, Widget>> {
        //            rank => new Label (rank.WazaName)
        //        });

        //    _moves.ShowAll();
        //});
    }

    protected async void OnProposeTeam(object sender, EventArgs e)
    {
        Application.Invoke(delegate
        {
            _waitWindow.Show();
        });

        var initialTeam = _controlSets
            .Where(ctrl => !string.IsNullOrEmpty(ctrl.Item2.ActiveText) && Regex.IsMatch(ctrl.Item2.ActiveText, "^[0-9]+$"))
            .Select(ctrl =>
                {
                    var pokemonId = new PokemonIdentifier(int.Parse(ctrl.Item2.ActiveText));

                    if (ctrl.Item3.Active != -1)
                    {
                        pokemonId.FormNo = ctrl.Item3.Active.ToString();
                    }

                    if (!string.IsNullOrEmpty(ctrl.Item4.ActiveText))
                    {
                        var name = ctrl.Item4.ActiveText;

                        pokemonId.Name = _pokedex.GetByName(name)?.GetName("en");
                    }

                    return pokemonId;
                })
            .ToList();

        await ProposeTeam(initialTeam).ConfigureAwait(false);

        Application.Invoke(delegate
        {
            _waitWindow.Hide();
        });
    }

	public void OnExportToShowdown(object sender, EventArgs e)
    {
        var showdownExporter = new ShowdownExporter(_pokedex, _itemdex, _movedex, _abilitydex);

        var export = showdownExporter.ExportTeam(_latestTeam);

        Application.Invoke(delegate
        {
            new ShowdownWindow(export);
        });
    }

    public new void Dispose()
    {
        _usageRetrievers.ForEach(retriever => retriever.Dispose());
    }
}
