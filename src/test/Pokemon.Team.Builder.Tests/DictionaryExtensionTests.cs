using System;
using NUnit.Framework;
using System.Collections.Generic;
using FluentAssertions;

namespace Pokemon.Team.Builder.Tests
{
	[TestFixture]
	public class DictionaryExtensionTests
	{
		[Test]
		public void Should_Merge_Dictionaries_Properly()
		{
			var fst = new Dictionary<int, int> {
				{ 94, 10 },
				{ 212, 5 },
				{ 12, 3 }
			};

			var snd = new Dictionary<int, int> {
				{ 94, 8 },
				{ 214, 2 }
			};

			var merged = fst.MergeDictionaries (new []{ snd });

			merged.ShouldAllBeEquivalentTo (new Dictionary<int, int>{
				{94, 18 },
				{212, 5 },
				{12, 3 },
				{214, 2}
			});
		}

		[Test]
		public void Should_Also_Work_With_Object_Keys()
		{
			var fst = new Dictionary<PokemonIdentifier, int> {
				{ new PokemonIdentifier(94), 10 },
				{ new PokemonIdentifier(212), 5 },
				{ new PokemonIdentifier(12), 3 }
			};

			var snd = new Dictionary<PokemonIdentifier, int> {
				{ new PokemonIdentifier(94), 8 },
				{ new PokemonIdentifier(214), 2 }
			};

			var merged = fst.MergeDictionaries (new []{ snd });

			merged.ShouldAllBeEquivalentTo (new Dictionary<PokemonIdentifier, int>{
				{new PokemonIdentifier(94), 18 },
				{new PokemonIdentifier(212), 5 },
				{new PokemonIdentifier(12), 3 },
				{new PokemonIdentifier(214), 2}
			});
		}
	}
}

