using System;
using System.Linq;
using Gtk;
using Pokemon.Team.Builder.Model;
using System.Collections.Generic;
using Pokemon.Team.Builder.Interfaces;

namespace Pokemon.Team.Builder.UI
{
	public class PokemonDetailWindow : Window
	{
		private Pokedex _pokedex;
		private IPokemonInformation _pokeInfo;
        private string _languageCode;

		public PokemonDetailWindow(IPokemonInformation pokeInfo, Pokedex pokedex, string languageCode) : base(WindowType.Toplevel) {
			_pokeInfo = pokeInfo;
			_pokedex = pokedex;
            _languageCode = languageCode;

            Initialize ();

			this.ShowAll ();
		}

		public void Initialize()
		{
			var vBox = new Box (Orientation.Vertical, 0);
			var hBox = new Box (Orientation.Horizontal, 0);

            var hvGrid = new Grid
            {
                Hexpand = true,
                Vexpand = true,
                ColumnHomogeneous = true,
                WidthRequest = 200 
            };

			var image = new Image ();
			var pokemon = _pokedex.GetById (_pokeInfo.Identifier.MonsNo);
			image.SetPicture (pokemon);

            var nameLabel = new Label("Name");
			var pokeName = new Label(pokemon.GetName("en"));

            var type1Label = new Label("Type 1");
            var type2Label = new Label("Type 2");

            var pokeType1 = new Label(_pokeInfo.GetType1());
			var pokeType2 = new Label(_pokeInfo.GetType2());

            var gridLines = new Dictionary<Label, Label>
            {
                { nameLabel, pokeName },
                { type1Label, pokeType1 },
                { type2Label, pokeType2 }
            };

            hvGrid.AddItems(gridLines.ToList(),
                new List<Func<KeyValuePair<Label, Label>, Widget>> {
                    label => label.Key,
                    label => label.Value
            });

            var pokedexNotebook = new Notebook
            {
                Scrollable = true
            };

            var pokedexEntries = _pokedex.GetPokedexDescriptions(pokemon, _languageCode);

            foreach (var pokedexEntry in pokedexEntries)
            {
                var textBuffer = new TextBuffer(new TextTagTable());

                textBuffer.Text = pokedexEntry.flavor_text;

                var text = new TextView(textBuffer)
                {
                    Editable = false,
                    Expand = true,
                    Hexpand = true,
                    Vexpand = true,
                    
                };
                var label = new Label(pokedexEntry.version?.name);

                pokedexNotebook.AppendPage(text, label);
            }

            hBox.Add (image);
			hBox.Add (hvGrid);

            hBox.Add(pokedexNotebook);

            vBox.Add (hBox);

			var noteBook = new Notebook ();

			var moveGrid = new Grid { ColumnHomogeneous = true, Hexpand = true };
			var itemGrid = new Grid { ColumnHomogeneous = true, Hexpand = true };
			var natureGrid = new Grid { ColumnHomogeneous = true, Hexpand = true };
			var abilityGrid = new Grid { ColumnHomogeneous = true, Hexpand = true };

			var movesForFaintingGrid = new Grid { ColumnHomogeneous = true, Hexpand = true };
			var defeatedPokemonGrid = new Grid { ColumnHomogeneous = true, Hexpand = true };
			var counterPokemonGrid = new Grid { ColumnHomogeneous = true, Hexpand = true };
			var effectiveOpponentMoves = new Grid { ColumnHomogeneous = true, Hexpand = true };

            var items = _pokeInfo.GetItems();

            if (items != null) {
				itemGrid
					.AddItems (items.OrderBy (poke => poke.Ranking).ToList (),
						new List<Func<IItem, Widget>> {
							item => new Label (item.Name),
                            item => new Label ($"{Math.Round(item.UsageRate, 2)} %")
						}
				);
			}

            var moves = _pokeInfo.GetMoves();

			if (moves != null) {
				moveGrid
					.AddItems (moves.OrderBy (poke => poke.Ranking).ToList (),
						new List<Func<IMove, Widget>> {
							move => new Label (move.Name),
                            move => new Label ($"{Math.Round(move.UsageRate, 2)} %")
					}
				);
			}

            var natures = _pokeInfo.GetNatures();

			if (natures != null) {
				natureGrid
					.AddItems (natures.OrderBy (poke => poke.Ranking).ToList (),
						new List<Func<INature, Widget>> {
							nature => new Label (nature.Name),
                            nature => new Label ($"{Math.Round(nature.UsageRate, 2)} %")
						}
				);
			}

            var abilities = _pokeInfo.GetAbilities();

			if (abilities != null) {
				abilityGrid
					.AddItems (abilities.OrderBy (poke => poke.Ranking).ToList (),
						new List<Func<IAbility, Widget>> {
							ability => new Label (ability.Name),
                            ability => new Label ($"{Math.Round(ability.UsageRate, 2)} %")
						}
				);
			}

            //if (_pokeInfo.RankingPokemonSuffererWaza != null) {
            //	movesForFaintingGrid
            //		.AddItems (_pokeInfo.RankingPokemonSuffererWaza.OrderBy (poke => poke.Ranking).ToList (),
            //			new List<Func<RankingPokemonSuffererWaza, Widget>> {
            //				poke => new Label (((RankingPokemonSuffererWaza)poke).WazaName),
            //				poke => new Label ($"{Math.Round(((RankingPokemonSuffererWaza) poke).UsageRate, 2)} %")
            //			}
            //	);
            //}

            //if (_pokeInfo.RankingPokemonSufferer != null) {
            //	defeatedPokemonGrid
            //		.AddItems (_pokeInfo.RankingPokemonSufferer.OrderBy (poke => poke.Ranking).ToList (),
            //			new List<Func<RankingPokemonSufferer, Widget>> {
            //				poke => new Image ().SetPicture (_pokedex.GetById (((RankingPokemonSufferer)poke).MonsNo), 48, 48),
            //				poke => new Label (((RankingPokemonSufferer)poke).MonsNo.ToString ()),
            //				poke => new Label (((RankingPokemonSufferer)poke).FormNo),
            //				poke => new Label (((RankingPokemonSufferer)poke).Name)
            //			}
            //	);
            //}

            var counters = _pokeInfo.GetCounters();

			if (counters != null) {
				counterPokemonGrid
					.AddItems (counters.OrderBy (poke => poke.Ranking).ToList (),
						new List<Func<ICounter, Widget>> {
							poke => new Image ().SetPicture (_pokedex.GetById (poke.Identifier.MonsNo), 48, 48),
							poke => new Label (poke.Identifier.MonsNo.ToString ()),
							poke => new Label (_pokedex.GetById (poke.Identifier.MonsNo).GetName("en"))
						}
				);
			}

			//if (_pokeInfo.RankingPokemonDownWaza != null) {
			//	effectiveOpponentMoves
			//		.AddItems (_pokeInfo.RankingPokemonDownWaza.OrderBy (poke => poke.Ranking).ToList (),
			//			new List<Func<RankingPokemonDownWaza, Widget>> {
			//				poke => new Label (((RankingPokemonDownWaza)poke).WazaName),
			//				poke => new Label ($"{Math.Round(((RankingPokemonDownWaza) poke).UsageRate, 2)} %")
			//			}
			//	);
			//}

			noteBook.AppendPage (moveGrid, new Label { Text = "Move Ranking", TooltipText = "Most often used moves" });
			noteBook.AppendPage (itemGrid, new Label { Text = "Item Ranking", TooltipText = "Most often used items" });
			noteBook.AppendPage (natureGrid, new Label { Text = "Nature Ranking", TooltipText = "Most often used natures" });
			noteBook.AppendPage (abilityGrid, new Label { Text = "Ability Ranking", TooltipText = "Most often used abilities" });
			noteBook.AppendPage (movesForFaintingGrid, new Label { Text = "Own Move Faint Ranking", TooltipText = "Moves that this pokemon most often uses for fainting opponents"});
			noteBook.AppendPage (defeatedPokemonGrid, new Label { Text = "Defeated Pokemon Ranking", TooltipText = "Pokemon that this pokemon most often faints" });
			noteBook.AppendPage (counterPokemonGrid, new Label { Text = "Counter Ranking", TooltipText = "Pokemon that most often counter this pokemon" });
			noteBook.AppendPage (effectiveOpponentMoves, new Label { Text = "Effective Opponent Moves", TooltipText = "Moves that most often kill this pokemon" });

			vBox.Add (noteBook);

			this.Add (vBox);
		}
	}
}

