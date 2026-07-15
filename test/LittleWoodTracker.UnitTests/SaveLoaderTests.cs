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
        [Fact]
        public void CanLoadFreshStart()
        {
            // Act
            SaveFile save;
            using (var stream = OpenEmbedded("fresh-start"))
            {
                save = SaveLoader.LoadSave(stream);
            }

            // Assert
            save.PlayerName.Should().Be("Shadow");
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
