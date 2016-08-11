using FluentAssertions;
using NUnit.Framework;
using Pokemon.Team.Builder.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Tests
{
    [TestFixture]
    public class ParseTierTests
    {
        [Test]
        public void Should_Parse_Hierarchy()
        {
            var xml = @"<Tiers>
                          <Tier>
                            <FullName>Anything Goes</FullName>
                            <ShortName>AG</ShortName>
                            <Tier>
                              <FullName>Uber</FullName>
                              <ShortName>Uber</ShortName>
                            </Tier>
                          </Tier>
                          <Tier>
                            <FullName>Little Cup</FullName>
                            <ShortName>LC</ShortName>
                          </Tier>
                        </Tiers>";

            var stream = new MemoryStream(Encoding.Unicode.GetBytes(xml));

			var tiers = TierSerializer.ParseFromStream(stream);

            var expected = new List<Tier>
            {
                new Tier
                {
                    FullName = "Anything Goes",
                    ShortName = "AG",
                    SubTiers = new List<Tier>
                    {
                        new Tier
                        {
                            FullName = "Uber",
                            ShortName = "Uber",
                            SubTiers = new List<Tier>()
                        }
                    }
                },
                new Tier
                {
                    FullName = "Little Cup",
                    ShortName = "LC",
                    SubTiers = new List<Tier>()
                }    
            };

            tiers.Should().BeEquivalentTo(expected);
        }
    }
}
