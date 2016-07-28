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
	}
}

