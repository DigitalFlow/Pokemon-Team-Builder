using System;
using System.Linq;
using Gtk;
using Pokemon.Team.Builder.Model;
using System.Collections.Generic;

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
			var vBox = new Box (Orientation.Vertical, 0);
			var hBox = new Box (Orientation.Horizontal, 0);
			var hvBox = new Box (Orientation.Vertical, 0);

			var image = new Image ();
			var pokemon = _pokedex.GetById (_pokeInfo.RankingPokemonInfo.MonsNo);
			image.SetPicture (pokemon);

			var pokeName = new Label(_pokeInfo.RankingPokemonInfo.Name);
			var pokeType1 = new Label(_pokeInfo.RankingPokemonInfo.TypeName1);
			var pokeType2 = new Label(_pokeInfo.RankingPokemonInfo.TypeName2);

			hvBox.Add (pokeName);
			hvBox.Add (pokeType1);
			hvBox.Add (pokeType2);

			hBox.Add (image);
			hBox.Add (hvBox);

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

			if (_pokeInfo.RankingPokemonTrend != null && _pokeInfo.RankingPokemonTrend.ItemInfo != null) {
				itemGrid
					.AddItems (_pokeInfo.RankingPokemonTrend.ItemInfo.OrderBy (poke => poke.Ranking).ToList (),
						new List<Func<ItemInfo, Widget>> {
							poke => new Label (((ItemInfo)poke).Name),
							poke => new Label ($"{Math.Round(((ItemInfo) poke).UsageRate, 2)} %")
						}
				);
			}

			if (_pokeInfo.RankingPokemonTrend != null && _pokeInfo.RankingPokemonTrend.WazaInfo != null) {
				moveGrid
					.AddItems (_pokeInfo.RankingPokemonTrend.WazaInfo.OrderBy (poke => poke.Ranking).ToList (),
						new List<Func<WazaInfo, Widget>> {
							poke => new Label (((WazaInfo)poke).Name),
							poke => new Label ($"{Math.Round(((WazaInfo) poke).UsageRate, 2)} %")
					}
				);
			}

			if (_pokeInfo.RankingPokemonTrend != null && _pokeInfo.RankingPokemonTrend.SeikakuInfo != null) {
				natureGrid
					.AddItems (_pokeInfo.RankingPokemonTrend.SeikakuInfo.OrderBy (poke => poke.Ranking).ToList (),
						new List<Func<SeikakuInfo, Widget>> {
							poke => new Label (((SeikakuInfo)poke).Name),
							poke => new Label ($"{Math.Round(((SeikakuInfo) poke).UsageRate, 2)} %")
						}
				);
			}

			if (_pokeInfo.RankingPokemonTrend != null && _pokeInfo.RankingPokemonTrend.TokuseiInfo != null) {
				abilityGrid
					.AddItems (_pokeInfo.RankingPokemonTrend.TokuseiInfo.OrderBy (poke => poke.Ranking).ToList (),
						new List<Func<TokuseiInfo, Widget>> {
							poke => new Label (((TokuseiInfo)poke).Name),
							poke => new Label ($"{Math.Round(((TokuseiInfo) poke).UsageRate, 2)} %")
						}
				);
			}

			if (_pokeInfo.RankingPokemonSuffererWaza != null) {
				movesForFaintingGrid
					.AddItems (_pokeInfo.RankingPokemonSuffererWaza.OrderBy (poke => poke.Ranking).ToList (),
						new List<Func<RankingPokemonSuffererWaza, Widget>> {
							poke => new Label (((RankingPokemonSuffererWaza)poke).WazaName),
							poke => new Label ($"{Math.Round(((RankingPokemonSuffererWaza) poke).UsageRate, 2)} %")
						}
				);
			}

			if (_pokeInfo.RankingPokemonSufferer != null) {
				defeatedPokemonGrid
					.AddItems (_pokeInfo.RankingPokemonSufferer.OrderBy (poke => poke.Ranking).ToList (),
						new List<Func<RankingPokemonSufferer, Widget>> {
							poke => new Image ().SetPicture (_pokedex.GetById (((RankingPokemonSufferer)poke).MonsNo), 48, 48),
							poke => new Label (((RankingPokemonSufferer)poke).MonsNo.ToString ()),
							poke => new Label (((RankingPokemonSufferer)poke).FormNo),
							poke => new Label (((RankingPokemonSufferer)poke).Name)
						}
				);
			}

			if (_pokeInfo.RankingPokemonDown != null) {
				counterPokemonGrid
					.AddItems (_pokeInfo.RankingPokemonDown.OrderBy (poke => poke.Ranking).ToList (),
						new List<Func<RankingPokemonDown, Widget>> {
							poke => new Image ().SetPicture (_pokedex.GetById (((RankingPokemonDown)poke).MonsNo), 48, 48),
							poke => new Label (((RankingPokemonDown)poke).MonsNo.ToString ()),
							poke => new Label (((RankingPokemonDown)poke).FormNo),
							poke => new Label (((RankingPokemonDown)poke).Name)
						}
				);
			}

			if (_pokeInfo.RankingPokemonDownWaza != null) {
				effectiveOpponentMoves
					.AddItems (_pokeInfo.RankingPokemonDownWaza.OrderBy (poke => poke.Ranking).ToList (),
						new List<Func<RankingPokemonDownWaza, Widget>> {
							poke => new Label (((RankingPokemonDownWaza)poke).WazaName),
							poke => new Label ($"{Math.Round(((RankingPokemonDownWaza) poke).UsageRate, 2)} %")
						}
				);
			}

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

