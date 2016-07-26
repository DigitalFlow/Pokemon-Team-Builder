using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Tests
{
    [TestFixture]
    public class PokedexSerializationTests
    {
        [Test]
        public void Should_Serialize_Pokemon_Properly()
        {
            // Arrange
            var pokemon = new List<Pokemon>
            {
                new Pokemon
                {
                    Id = 1,
                    Name = "Bulbasaur",
                    Url = "http://pokeapi.co/api/v2/pokemon/1/",
                },
                new Pokemon
                {
                    Id = 4,
                    Name = "Charmander",
                    Url = "http://pokeapi.co/api/v2/pokemon/4/",
                },
                new Pokemon
                {
                    Id = 7,
                    Name = "Squirtle",
                    Url = "http://pokeapi.co/api/v2/pokemon/7/",
                }
            };

            var expectedXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
                                <Pokedex>
                                    <Pokemon>
                                        <Id>1</Id>
                                        <Name>Bulbasaur</Name>
                                        <Url>http://pokeapi.co/api/v2/pokemon/1/</Url>
                                    </Pokemon>
                                    <Pokemon>
                                        <Id>4</Id>
                                        <Name>Charmander</Name>
                                        <Url>http://pokeapi.co/api/v2/pokemon/4/</Url>
                                    </Pokemon>
                                    <Pokemon>
                                        <Id>7</Id>
                                        <Name>Squirtle</Name>
                                        <Url>http://pokeapi.co/api/v2/pokemon/7/</Url>
                                    </Pokemon>
                                </Pokedex>".Replace("\n", "").Replace("\r", "").Replace(" ", "");

            // Act
            var xml = PokedexSerializer.SerializePokedex(pokemon)
                        .Replace("\n", "").Replace("\r", "").Replace(" ", "");

            // Assert
            xml.Should().Be(expectedXml);
        }
    }
}
