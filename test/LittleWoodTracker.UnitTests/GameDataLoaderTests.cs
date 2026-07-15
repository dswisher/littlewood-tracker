// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Linq;
using AwesomeAssertions;
using LittleWoodTracker.Cli;
using Xunit;

namespace LittleWoodTracker.UnitTests
{
    public class GameDataLoaderTests
    {
        [Fact]
        public void CanLoadGameData()
        {
            // Act
            var gameData = GameDataLoader.Load();

            // Assert
            gameData.Houses.Should().Contain("Willow's House");
            gameData.Crops.Should().Contain("Motato");
            gameData.Structures.Should().Contain("Lumber Mill");

            var woodCutter = gameData.NumericAchievements.FirstOrDefault(a => a.Name == "Woodcutter");

            woodCutter.Should().NotBeNull();
            woodCutter.Thresholds.Should().HaveCount(5);
        }
    }
}
