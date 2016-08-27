using Pokemon.Team.Builder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Logic
{
    public class ShowdownExporter
    {
        private Pokedex _pokedex;
        private Itemdex _itemdex;
        private Movedex _movedex;
        private AbilityDex _abilitydex;

        public ShowdownExporter (Pokedex pokedex, Itemdex itemdex, Movedex movedex, AbilityDex abilitydex)
        {
            _pokedex = pokedex;
            _itemdex = itemdex;
            _movedex = movedex;
            _abilitydex = abilitydex;
        }

        public string ExportTeam (Team team)
        {
            var teamBuilder = new StringBuilder();

            var builds = new Dictionary<IPokemonInformation, IItem>();

            foreach (var member in team.TeamMembers)
            {
                var rawItem = member
                    .GetItems()
                    .Where(i => !builds.ContainsValue(i))
                    .First();

                builds.Add(member, rawItem);
            }

            foreach (var build in builds)
            {
                var member = build.Key;
                var memberItem = build.Value;

                var pokemon = _pokedex.GetByIdentifier(member.Identifier);

                var name = pokemon.GetName("en");

				var item = _itemdex.GetByName(memberItem.Name)?.GetName("en") ?? memberItem.Name;

                var abilityRaw = member.GetAbilities().First().Name;
				var ability = _abilitydex.GetByName(abilityRaw)?.GetName("en") ?? abilityRaw;

                var spread = member.GetNatures().First().Name;

                // Truncate EV Split Part
                var nature = spread;

                if (spread.Contains(":"))
                {
                    nature = spread.Substring(0, spread.IndexOf(':'));
                }

                var split = spread;

                if (split.Contains(":"))
                {
                    split = spread.Substring(spread.IndexOf(':') + 1);
                }

                teamBuilder.Append($"{name} @ {item}\n");
                teamBuilder.Append($"Ability: {ability}\n");

                var splitted = split.Split('/').Select(s => s.Trim());
                var labels = new List<string> { "HP", "Atk", "Def", "SpA", "SpD", "Spe" };

                var withLabels = splitted.Zip(labels, (s, label) => Tuple.Create(s, label))
                    .Where(tuple => tuple.Item1 != "0");

                teamBuilder.Append($"EVs: {string.Join(" / ", withLabels.Select(tuple => $"{tuple.Item1} {tuple.Item2}"))}\n");

                teamBuilder.Append($"{nature} Nature\n");

                var moves = member.GetMoves().Take(4);

                foreach (var move in moves)
                {
                    var moveRaw = move.Name;

					var moveFromDex = _movedex.GetByName(moveRaw)?.GetName("en") ?? moveRaw;

                    teamBuilder.Append($"- {moveFromDex}\n");
                }

                teamBuilder.Append("\n");
            }

            var export = teamBuilder.ToString();

            return export;
        }
    }
}
