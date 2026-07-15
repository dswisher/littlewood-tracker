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
            data.Houses.UnionWith(LoadEmbeddedList<string>("Houses"));
            data.Structures.UnionWith(LoadEmbeddedList<string>("Structures"));
            data.Crops.UnionWith(LoadEmbeddedList<string>("Crops"));

            data.NumericAchievements.AddRange(LoadEmbeddedList<NumericAchievement>("NumericAchievements"));

            // Return what we've built
            return data;
        }


        private static IEnumerable<T> LoadEmbeddedList<T>(string resourceName)
        {
            using (var stream = OpenEmbedded(resourceName))
            using (var reader = new StreamReader(stream))
            {
                var content = reader.ReadToEnd();
                return JsonSerializer.Deserialize<IEnumerable<T>>(content) ?? new List<T>();
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
