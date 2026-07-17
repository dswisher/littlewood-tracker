// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using AwesomeAssertions;
using LittleWoodTracker.Cli;
using LittleWoodTracker.Cli.Models;
using Xunit;

namespace LittleWoodTracker.UnitTests
{
    public class DonationTests
    {
        // Save: bruno-d130-s104285.json
        //   Tavern        index 20, exp=9  -> level 4, next: Dewdrops x250
        //   Museum        index 21, exp=9  -> level 4, next: Dewdrops x250
        //   Lumber Mill   index 22, exp=22 -> level 8, next: Perfect Plank x4
        //   My Office     index 23, exp=27 -> level 10 (max)
        //   General Shop  index 25, exp=8  -> level 3, next: Plain Brick x8
        //   Smelter       index 27, exp=23 -> level 8, next: Perfect Brick x4
        //   Decor Shop    index 31, exp=1  -> level 1, next: Wooden Plank x2
        //   Endless Forest index 50, exp=13 -> level 5, next: Fancy Plank x4
        //   Dust Cavern   index 51, exp=7  -> level 3, next: Wooden Plank x8
        //   Master Forge  index 54, exp=3  -> level 2, next: Dewdrops x150


        [Fact]
        public void CanLoadStructureUpgradeExpFromSave()
        {
            // Act
            SaveFile save;
            using (var stream = OpenEmbedded("bruno-d130-s104285"))
            {
                save = SaveLoader.LoadSave(stream);
            }

            // Assert - spot-check several structures
            save.StructureUpgradeExp.Should().HaveCountGreaterThan(50);
            save.StructureUpgradeExp[20].Should().Be(9);    // Tavern: level 4
            save.StructureUpgradeExp[22].Should().Be(22);   // Lumber Mill: level 8
            save.StructureUpgradeExp[23].Should().Be(27);   // My Office: level 10 (max)
            save.StructureUpgradeExp[54].Should().Be(3);    // Master Forge: level 2
        }


        [Fact]
        public void GameDataLoadsDonationTargetList()
        {
            // Act
            var gameData = GameDataLoader.Load();

            // Assert - structures have blueprint IDs; locations do not
            gameData.DonationTargetsByIndex.Should().ContainKey(20);
            gameData.DonationTargetsByIndex[20].Name.Should().Be("Tavern");
            gameData.DonationTargetsByIndex[20].BlueprintId.Should().Be(160);

            gameData.DonationTargetsByIndex.Should().ContainKey(54);
            gameData.DonationTargetsByIndex[54].Name.Should().Be("Master Forge");
            gameData.DonationTargetsByIndex[54].BlueprintId.Should().BeNull();
        }


        [Fact]
        public void IsUnlockedReturnsTrueForBuiltStructure()
        {
            // Arrange - bruno save has Tavern built (discoverLevel[160] == 2)
            var gameData = GameDataLoader.Load();
            SaveFile save;
            using (var stream = OpenEmbedded("bruno-d130-s104285"))
            {
                save = SaveLoader.LoadSave(stream);
            }

            var tavern = gameData.DonationTargetsByIndex[20];

            // Act / Assert
            GameData.IsUnlocked(tavern, save).Should().BeTrue();
        }


        [Fact]
        public void IsUnlockedReturnsFalseForUnbuiltStructure()
        {
            // Arrange - bruno save has Beauty Salon not yet built (discoverLevel[177] == 0)
            var gameData = GameDataLoader.Load();
            SaveFile save;
            using (var stream = OpenEmbedded("bruno-d130-s104285"))
            {
                save = SaveLoader.LoadSave(stream);
            }

            var beautySalon = gameData.DonationTargetsByIndex[37];

            // Act / Assert
            GameData.IsUnlocked(beautySalon, save).Should().BeFalse();
        }


        [Fact]
        public void IsUnlockedReturnsTrueForVisitedLocation()
        {
            // Arrange - bruno save has visited Endless Forest (structureUpgradeEXP[50] == 13)
            var gameData = GameDataLoader.Load();
            SaveFile save;
            using (var stream = OpenEmbedded("bruno-d130-s104285"))
            {
                save = SaveLoader.LoadSave(stream);
            }

            var forest = gameData.DonationTargetsByIndex[50];

            // Act / Assert
            GameData.IsUnlocked(forest, save).Should().BeTrue();
        }


        [Fact]
        public void IsUnlockedReturnsFalseForUnvisitedLocation()
        {
            // Arrange - bruno save has Grand Library at exp=0 (never visited)
            var gameData = GameDataLoader.Load();
            SaveFile save;
            using (var stream = OpenEmbedded("bruno-d130-s104285"))
            {
                save = SaveLoader.LoadSave(stream);
            }

            var grandLibrary = gameData.DonationTargetsByIndex[53];

            // Act / Assert
            GameData.IsUnlocked(grandLibrary, save).Should().BeFalse();
        }


        [Fact]
        public void GameDataLoadsUpgradeCosts()
        {
            // Act
            var gameData = GameDataLoader.Load();

            // Assert - verify a few representative entries
            gameData.UpgradeCosts.Should().ContainKey((5, 0));
            gameData.UpgradeCosts[(5, 0)].ItemName.Should().Be("Dewdrops");
            gameData.UpgradeCosts[(5, 0)].Quantity.Should().Be(250);

            gameData.UpgradeCosts.Should().ContainKey((9, 1));
            gameData.UpgradeCosts[(9, 1)].ItemName.Should().Be("Perfect Plank");
            gameData.UpgradeCosts[(9, 1)].Quantity.Should().Be(4);
        }


        [Fact]
        public void GetNextUpgradeCostReturnsCorrectCostForUpgradableTarget()
        {
            var gameData = GameDataLoader.Load();

            // exp=9 -> level 4, remainder 0, next donation is slot 0 for level 5
            var cost = gameData.GetNextUpgradeCost(9);

            cost.Should().NotBeNull();
            cost.ItemName.Should().Be("Dewdrops");
            cost.Quantity.Should().Be(250);
        }


        [Fact]
        public void GetNextUpgradeCostReturnsNullAtMaxLevel()
        {
            var gameData = GameDataLoader.Load();

            // exp=27 -> level 10 (max)
            var cost = gameData.GetNextUpgradeCost(27);

            cost.Should().BeNull();
        }


        [Fact]
        public void GetNextUpgradeCostReturnsSecondSlotCost()
        {
            var gameData = GameDataLoader.Load();

            // exp=22 -> level 8, remainder 1, next donation is slot 1 for level 9 -> Perfect Plank x4
            var cost = gameData.GetNextUpgradeCost(22);

            cost.Should().NotBeNull();
            cost.ItemName.Should().Be("Perfect Plank");
            cost.Quantity.Should().Be(4);
        }


        [Fact]
        public void GetNextUpgradeCostForDecorShop()
        {
            var gameData = GameDataLoader.Load();

            // exp=1 -> level 1, remainder 1, next donation is slot 1 for level 2 -> Wooden Plank x2
            var cost = gameData.GetNextUpgradeCost(1);

            cost.Should().NotBeNull();
            cost.ItemName.Should().Be("Wooden Plank");
            cost.Quantity.Should().Be(2);
        }


        private static Stream OpenEmbedded(string name)
        {
            var assembly = typeof(DonationTests).Assembly;
            var resourceName = $"LittleWoodTracker.UnitTests.SaveFiles.{name}.json";
            var stream = assembly.GetManifestResourceStream(resourceName);

            return stream ?? throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");
        }
    }
}
