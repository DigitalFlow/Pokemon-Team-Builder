using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon.Team.Builder
{
	public static class DictionaryExtensions
	{
		public static Dictionary<int, int> MergeDictionaries(this Dictionary<int, int> first, IEnumerable<Dictionary<int, int>> second)
		{
			var dictionaries = new List<Dictionary<int, int>> (second);
			dictionaries.Add (first);

			return dictionaries
				.SelectMany (dict => dict)
				.GroupBy(pair => pair.Key)
				.ToDictionary(pair => pair.Key, pair => pair.Sum(kvp => kvp.Value));
		}
	}
}

