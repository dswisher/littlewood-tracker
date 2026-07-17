// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            // Load the item lists
            var bluePrints = LoadEmbeddedList<ItemListEntry>("BlueprintList").ToList();
            var items = LoadEmbeddedList<ItemListEntry>("ItemList").ToList();

            PopulateDictionary(data.BlueprintNames, bluePrints);
            PopulateDictionary(data.ItemNames, items);

            // Load all the bits and bobs
            data.Houses.UnionWith(ExtractItemList(bluePrints, "House"));
            data.Structures.UnionWith(ExtractItemList(bluePrints, "Structure"));
            data.Crops.UnionWith(ExtractItemList(bluePrints, "Crop"));

            data.NumericAchievements.AddRange(LoadEmbeddedList<NumericAchievement>("NumericAchievements"));

            // Load structure upgrade data
            foreach (var target in LoadEmbeddedList<DonationTargetInfo>("DonationTargetList"))
            {
                data.DonationTargetsByIndex[target.Index] = target;
            }

            foreach (var cost in LoadEmbeddedList<UpgradeCostEntry>("UpgradeCosts"))
            {
                data.UpgradeCosts[(cost.Level, cost.Slot)] = cost;
            }

            // Return what we've built
            return data;
        }


        private static void PopulateDictionary(Dictionary<int, string> dictionary, List<ItemListEntry> itemEntries)
        {
            foreach (var entry in itemEntries)
            {
                foreach (var pair in entry.Items)
                {
                    dictionary.Add(pair.Id, pair.Name);
                }
            }
        }


        private static IEnumerable<string> ExtractItemList(List<ItemListEntry> items, string categoryName)
        {
            var entry = items.FirstOrDefault(e => e.Name == categoryName);

            return entry == null ? throw new KeyNotFoundException($"Category '{categoryName}' not found.") : entry.Items.Select(x => x.Name);
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
