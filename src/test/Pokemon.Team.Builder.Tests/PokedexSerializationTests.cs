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
                    Names = new List<Name>
                    {
                        new Name
                        {
                            name = "Bulbasaur",
                            language = new Language
                            {
                                name = "en",
                                url = "http://pokeapi.co/api/v2/language/9/"
                            }
                        }
                    },
                    Varieties = new List<Variety>
                    {
                        new Variety
                        {
                            is_default = true,
                            pokemon = new PokemonDescriptor
                            {
                                name = "bulbasaur",
                                url = "http://pokeapi.co/api/v2/pokemon/1/"
                            }
                        }
                    },
                    Url = "http://pokeapi.co/api/v2/pokemon/1/",
                }
            };

            var pokedex = new Pokedex(pokemon);

            var expectedXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
                                <Pokedex xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
                                  <Pokemon>
                                    <Id>1</Id>
                                    <Names>
                                      <Name>
                                        <name>Bulbasaur</name>
                                        <language>
                                          <url>http://pokeapi.co/api/v2/language/9/</url>
                                          <name>en</name>
                                        </language>
                                      </Name>
                                    </Names>
                                    <Varieties>
                                      <Variety>
                                        <is_default>true</is_default>
                                        <pokemon>
                                          <url>http://pokeapi.co/api/v2/pokemon/1/</url>
                                          <name>bulbasaur</name>
                                        </pokemon>
                                      </Variety>
                                    </Varieties>
                                    <Url>http://pokeapi.co/api/v2/pokemon/1/</Url>
                                    <Identifier>
                                        <MonsNo>1</MonsNo>
                                        <Name>Bulbasaur</Name>
                                    </Identifier>
                                  </Pokemon>
                                </Pokedex>".Replace("\n", "").Replace("\r", "").Replace(" ", "").Replace("\t", "");

            // Act
            var xml = GenericSerializer<Pokedex>.Serialize(pokedex)
                        .Replace("\n", "").Replace("\r", "").Replace(" ", "");

            // Assert
            xml.Should().Be(expectedXml);
        }
    }
}
