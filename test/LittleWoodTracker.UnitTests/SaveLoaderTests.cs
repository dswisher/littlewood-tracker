// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using AwesomeAssertions;
using LittleWoodTracker.Cli;
using LittleWoodTracker.Cli.Models;
using Xunit;

namespace LittleWoodTracker.UnitTests
{
    public class SaveLoaderTests
    {
        // NOTEs on saves:
        // bruno-d125-s98901.json
        //      Baseline for my main save
        // bruno-d125-s99042.json
        //      (I think this is correct, but I may have conflated the saves)
        //      Caught two minnows
        //      Caught 2 flutterfly (1 catch, bonus item?)
        //      Picked one weed
        // myla-d1-s0.json
        //      Started the game and saved without even moving.
        // myla-d1-s15.json
        //      Talked to Willow
        //      Opened two chests
        //      Obtained blueprint for My House
        // shadow-d1-s3.json
        //      Started the game, took 3 steps, and saved


        [Fact]
        public void CanLoadMylaDay1Step15()
        {
            // Act
            SaveFile save;
            using (var stream = OpenEmbedded("myla-d1-s15"))
            {
                save = SaveLoader.LoadSave(stream);
            }

            // Assert
            save.PlayerName.Should().Be("Myla");
            save.DaysPlayed.Should().Be(1);
            save.Steps.Should().Be(15);

            // We were just given the blueprint for our house
            save.DiscoverLevel[120].Should().Be(1);
        }


        [Fact]
        public void CanLoadShadowDay1Step3()
        {
            // Act
            SaveFile save;
            using (var stream = OpenEmbedded("shadow-d1-s3"))
            {
                save = SaveLoader.LoadSave(stream);
            }

            // Assert
            save.PlayerName.Should().Be("Shadow");
            save.DaysPlayed.Should().Be(1);
            save.Steps.Should().Be(3);
        }


        [Fact]
        public void CanLoadBrunoDay125Step98901()
        {
            // Act
            SaveFile save;
            using (var stream = OpenEmbedded("bruno-d125-s98901"))
            {
                save = SaveLoader.LoadSave(stream);
            }

            // Assert
            save.PlayerName.Should().Be("Bruno");
            save.DaysPlayed.Should().Be(125);
            save.Steps.Should().Be(98901);

            save.ItemsGathered.Should().Be(1230);
            save.TreesChopped.Should().Be(1372);
            save.OresMined.Should().Be(1293);
            save.FishCaught.Should().Be(428);
            save.BugsCaught.Should().Be(400);
            save.CropsHarvested.Should().Be(960);
            save.ItemsCrafted.Should().Be(925);
            save.ItemsSold.Should().Be(2049);
            save.QuestsCompleted.Should().Be(61);

            save.MapObjectString.Length.Should().BeGreaterThan(100);    // arbitrary number
        }


        [Fact]
        public void CanLoadBrunoDay125Step99042()
        {
            // Act
            SaveFile save;
            using (var stream = OpenEmbedded("bruno-d125-s99042"))
            {
                save = SaveLoader.LoadSave(stream);
            }

            // Assert
            save.PlayerName.Should().Be("Bruno");
            save.DaysPlayed.Should().Be(125);
            save.Steps.Should().Be(99042);
        }


        private static Stream OpenEmbedded(string name)
        {
            var assembly = typeof(SaveLoaderTests).Assembly;
            var resourceName = $"LittleWoodTracker.UnitTests.SaveFiles.{name}.json";
            var stream = assembly.GetManifestResourceStream(resourceName);

            return stream ?? throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");
        }
    }
}
