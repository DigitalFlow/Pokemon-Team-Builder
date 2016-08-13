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
					Names = new List<Name>{ new Name { name = "Bulbasaur", language = new Language { name = "en" }}},
                    Url = "http://pokeapi.co/api/v2/pokemon/1/",
                },
                new Pokemon
                {
                    Id = 4,
					Names = new List<Name>{ new Name { name = "Charmander", language = new Language { name = "en" }}},
                    Url = "http://pokeapi.co/api/v2/pokemon/4/",
                },
                new Pokemon
                {
                    Id = 7,
					Names = new List<Name>{ new Name { name = "Squirtle", language = new Language { name = "en" }}},
                    Url = "http://pokeapi.co/api/v2/pokemon/7/",
                }
            };

            var expectedXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
                                <Pokedex>
                                    <Pokemon>
                                        <Id>1</Id>
										<Names>
                                        	<NameByLanguage>
												<Name>Bulbasaur</Name>
												<Language>en</Language>
											</NameByLanguage>
										</Names>
                                        <Image />
                                        <Url>http://pokeapi.co/api/v2/pokemon/1/</Url>
                                    </Pokemon>
                                    <Pokemon>
                                        <Id>4</Id>
                                        <Names>
                                        	<NameByLanguage>
												<Name>Charmander</Name>
												<Language>en</Language>
											</NameByLanguage>
										</Names>
                                        <Image />
                                        <Url>http://pokeapi.co/api/v2/pokemon/4/</Url>
                                    </Pokemon>
                                    <Pokemon>
                                        <Id>7</Id>
                                        <Names>
                                        	<NameByLanguage>
												<Name>Squirtle</Name>
												<Language>en</Language>
											</NameByLanguage>
										</Names>
                                        <Image />
                                        <Url>http://pokeapi.co/api/v2/pokemon/7/</Url>
                                    </Pokemon>
                                </Pokedex>".Replace("\n", "").Replace("\r", "").Replace(" ", "").Replace("\t", "");

            // Act
            var xml = PokedexSerializer.SerializePokedex(pokemon)
                        .Replace("\n", "").Replace("\r", "").Replace(" ", "");

            // Assert
            xml.Should().Be(expectedXml);
        }
    }
}
