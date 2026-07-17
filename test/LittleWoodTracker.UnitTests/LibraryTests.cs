// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using System.Linq;
using AwesomeAssertions;
using LittleWoodTracker.Cli;
using LittleWoodTracker.Cli.Models;
using Xunit;

namespace LittleWoodTracker.UnitTests
{
    public class LibraryTests
    {
        // Save: bruno-d130-s104285.json
        //   travelerLevel: [3, 0, 2, 0, 1, 0, 0, 0, 0, 0, 0, 0, 3, 1, 1, 3, 3, 3, 0, 0, 3, 3, 0, 0]
        //   Books donated (level >= 3): 0 (Wedge), 12 (Raven), 15 (Pokidoki), 16 (Robin), 17 (Tara), 20 (Woz), 21 (Donno)
        //   Grand Library location (structureUpgradeEXP[53]) is 0 - not yet donated to, but travelers have visited


        [Fact]
        public void CanLoadTravelerLevelFromSave()
        {
            // Act
            SaveFile save;
            using (var stream = OpenEmbedded("bruno-d130-s104285"))
            {
                save = SaveLoader.LoadSave(stream);
            }

            // Assert
            save.TravelerLevel.Should().HaveCount(24);
            save.TravelerLevel[0].Should().Be(3);   // Wedge: book donated
            save.TravelerLevel[1].Should().Be(0);   // Primrose: never visited
            save.TravelerLevel[2].Should().Be(2);   // Datz: 2 visits
            save.TravelerLevel[12].Should().Be(3);  // Raven: book donated
        }


        [Fact]
        public void CanLoadInventorySoldFromSave()
        {
            // Act
            SaveFile save;
            using (var stream = OpenEmbedded("bruno-d130-s104285"))
            {
                save = SaveLoader.LoadSave(stream);
            }

            // Assert
            save.InventorySold.Should().HaveCountGreaterThan(200);
            save.InventorySold[240].Should().Be(459);  // 459 Weeds sold (Wedge's requirement)
            save.InventorySold[160].Should().Be(24);   // 24 Minnows sold
        }


        [Fact]
        public void GameDataLoadsTravelerList()
        {
            // Act
            var gameData = GameDataLoader.Load();

            // Assert
            gameData.TravelersByIndex.Should().HaveCount(24);

            gameData.TravelersByIndex[0].Name.Should().Be("Wedge, the Merchant");
            gameData.TravelersByIndex[0].BookTitle.Should().Be("Floating Continents");
            gameData.TravelersByIndex[0].Source.Should().Be("Marketplace");
            gameData.TravelersByIndex[0].Requirements.Should().NotBeNull();
            gameData.TravelersByIndex[0].Requirements!.Should().HaveCount(3);

            // Cavern travelers have no requirements
            gameData.TravelersByIndex[12].Name.Should().Be("Raven, the Elementalist");
            gameData.TravelersByIndex[12].Source.Should().Be("Dust Cavern");
            gameData.TravelersByIndex[12].Requirements.Should().BeNull();
        }


        [Fact]
        public void WedgeRequirementsMatchGameSource()
        {
            // Arrange - Wedge: sell 10 / 50 / 150 Weeds (item 240) across the three visits
            var gameData = GameDataLoader.Load();
            var wedge = gameData.TravelersByIndex[0];

            // Assert
            wedge.Requirements![0].Should().HaveCount(1);
            wedge.Requirements[0][0].ItemId.Should().Be(240);
            wedge.Requirements[0][0].Quantity.Should().Be(10);

            wedge.Requirements[1][0].ItemId.Should().Be(240);
            wedge.Requirements[1][0].Quantity.Should().Be(50);

            wedge.Requirements[2][0].ItemId.Should().Be(240);
            wedge.Requirements[2][0].Quantity.Should().Be(150);
        }


        [Fact]
        public void DatzRequirementsMatchGameSource()
        {
            // Arrange - Datz third visit: 15 Fire Carp (162) and 15 Plum Loach (163)
            var gameData = GameDataLoader.Load();
            var datz = gameData.TravelersByIndex[2];

            // Assert - visit 0: 10 Minnow
            datz.Requirements![0].Should().HaveCount(1);
            datz.Requirements[0][0].ItemId.Should().Be(160);
            datz.Requirements[0][0].Quantity.Should().Be(10);

            // visit 1: 15 Trout
            datz.Requirements[1][0].ItemId.Should().Be(161);
            datz.Requirements[1][0].Quantity.Should().Be(15);

            // visit 2: 15 Fire Carp + 15 Plum Loach
            datz.Requirements[2].Should().HaveCount(2);
            datz.Requirements[2].Should().Contain(r => r.ItemId == 162 && r.Quantity == 15);
            datz.Requirements[2].Should().Contain(r => r.ItemId == 163 && r.Quantity == 15);
        }


        [Fact]
        public void AllTravelerIndicesAreUnique()
        {
            var gameData = GameDataLoader.Load();

            var indices = gameData.TravelersByIndex.Keys.ToList();
            indices.Should().OnlyHaveUniqueItems();
            indices.Min().Should().Be(0);
            indices.Max().Should().Be(23);
        }


        private static Stream OpenEmbedded(string name)
        {
            var assembly = typeof(LibraryTests).Assembly;
            var resourceName = $"LittleWoodTracker.UnitTests.SaveFiles.{name}.json";
            var stream = assembly.GetManifestResourceStream(resourceName);

            return stream ?? throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");
        }
    }
}
