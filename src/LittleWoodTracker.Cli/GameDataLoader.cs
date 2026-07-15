// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LittleWoodTracker.Cli.Models;

namespace LittleWoodTracker.Cli
{
    public static class GameDataLoader
    {
        public static GameData Load()
        {
            // Create the empty class
            var data = new GameData();

            // Load all the bits and bobs
            data.Houses.UnionWith(LoadEmbeddedList("Houses"));
            data.Structures.UnionWith(LoadEmbeddedList("Structures"));
            data.Crops.UnionWith(LoadEmbeddedList("Crops"));

            // Return what we've built
            return data;
        }


        private static IEnumerable<string> LoadEmbeddedList(string resourceName)
        {
            using (var stream = OpenEmbedded(resourceName))
            using (var reader = new StreamReader(stream))
            {
                var content = reader.ReadToEnd();
                return JsonSerializer.Deserialize<IEnumerable<string>>(content) ?? new List<string>();
            }
        }


        private static Stream OpenEmbedded(string name)
        {
            var assembly = typeof(GameDataLoader).Assembly;
            var resourceName = $"LittleWoodTracker.Cli.Data.{name}.json";
            var stream = assembly.GetManifestResourceStream(resourceName);

            return stream ?? throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");
        }
    }
}
