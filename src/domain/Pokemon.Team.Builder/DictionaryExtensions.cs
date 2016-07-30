using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon.Team.Builder
{
	public static class DictionaryExtensions
	{
		public static Dictionary<T, int> MergeDictionaries<T>(this Dictionary<T, int> first, IEnumerable<Dictionary<T, int>> second)
		{
			var dictionaries = new List<Dictionary<T, int>> (second);
			dictionaries.Add (first);

			return dictionaries
				.SelectMany (dict => dict)
				.GroupBy(pair => pair.Key)
				.ToDictionary(pair => pair.Key, pair => pair.Sum(kvp => kvp.Value));
		}
	}
}

