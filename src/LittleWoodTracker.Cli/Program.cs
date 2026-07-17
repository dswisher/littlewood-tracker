// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using LittleWoodTracker.Cli.Models;

namespace LittleWoodTracker.Cli
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    Run,
                    errors =>
                    {
                        foreach (var e in errors)
                        {
                            Console.Error.WriteLine(e);
                        }

                        return 1;
                    });
        }


        private static int Run(Options options)
        {
            try
            {
                var save = SaveLoader.LoadSave(options.SaveNumber);

                Console.WriteLine($"Player name: {save.PlayerName}");

                var parsed = SaveParser.ParseSave(save);
                var gameData = GameDataLoader.Load();

                PrintMissing("Houses", gameData.Houses, parsed.MapItems);
                PrintMissing("Structures", gameData.Structures, parsed.MapItems);
                PrintMissing("Crops", gameData.Crops, parsed.MapItems);

                PrintStructureUpgrades(gameData, save);

                PrintMissingBooks(gameData, save);

                Console.WriteLine("Achievements:");
                PrintNumericAchievement(gameData, "itemsGathered", save.ItemsGathered);
                PrintNumericAchievement(gameData, "treesChopped", save.TreesChopped);
                PrintNumericAchievement(gameData, "oresMined", save.OresMined);
                PrintNumericAchievement(gameData, "fishCaught", save.FishCaught);
                PrintNumericAchievement(gameData, "bugsCaught", save.BugsCaught);
                PrintNumericAchievement(gameData, "cropsHarvested", save.CropsHarvested);
                PrintNumericAchievement(gameData, "itemsCrafted", save.ItemsCrafted);
                PrintNumericAchievement(gameData, "itemsSold", save.ItemsSold);
                PrintNumericAchievement(gameData, "questsCompleted", save.QuestsCompleted);

                // TODO - parse the number of townsfolk met, and add that achievement

                PrintCasinoInfo(gameData, save);

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }


        private static void PrintMissingBooks(GameData gameData, SaveFile save)
        {
            // Only show this section if any traveler has visited (proxy for library being accessible)
            if (save.TravelerLevel.All(v => v == 0))
            {
                return;
            }

            var missing = gameData.TravelersByIndex.Values
                .Where(t => t.Index < save.TravelerLevel.Length && save.TravelerLevel[t.Index] < 3)
                .OrderBy(t => t.Index)
                .ToList();

            if (missing.Count == 0)
            {
                return;
            }

            var donated = gameData.TravelersByIndex.Values.Count(t => t.Index < save.TravelerLevel.Length && save.TravelerLevel[t.Index] >= 3);

            Console.WriteLine($"Missing Library Books ({donated}/{gameData.TravelersByIndex.Count}):");

            foreach (var traveler in missing)
            {
                var visits = save.TravelerLevel[traveler.Index];
                Console.WriteLine($"    \"{traveler.BookTitle}\" - {traveler.Name} ({visits}/3 visits)");

                if (traveler.Requirements != null && visits < traveler.Requirements.Count)
                {
                    var reqs = traveler.Requirements[visits];
                    var parts = reqs.Select(r =>
                    {
                        var itemName = gameData.GetItemName(r.ItemId);
                        var sold = r.ItemId < save.InventorySold.Length ? save.InventorySold[r.ItemId] : 0;
                        return $"{itemName} ({sold}/{r.Quantity} sold)";
                    });

                    Console.WriteLine($"        Next visit: sell {string.Join(" and ", parts)}");
                }
                else if (traveler.Requirements == null)
                {
                    Console.WriteLine($"        Next visit: explore {traveler.Source}");
                }
            }
        }


        private static void PrintStructureUpgrades(GameData gameData, SaveFile save)
        {
            var upgradable = gameData.DonationTargetsByIndex
                .OrderBy(kvp => kvp.Value.Name)
                .Where(kvp => GameData.IsUnlocked(kvp.Value, save))
                .Select(kvp => new
                {
                    kvp.Value.Name,
                    Exp = kvp.Key < save.StructureUpgradeExp.Length ? save.StructureUpgradeExp[kvp.Key] : 0,
                    Cost = gameData.GetNextUpgradeCost(kvp.Key < save.StructureUpgradeExp.Length ? save.StructureUpgradeExp[kvp.Key] : 0)
                })
                .Where(x => x.Cost != null)
                .ToList();

            if (upgradable.Count == 0)
            {
                return;
            }

            Console.WriteLine("Structure Upgrades:");

            foreach (var s in upgradable)
            {
                var level = 1 + s.Exp / 3;
                Console.WriteLine($"    {s.Name}: Level {level} -> {s.Cost!.ItemName} x{s.Cost.Quantity}");
            }
        }


        private static string GetCasinoName(GameData gameData, int id)
        {
            if (id > 0 && id < 1000)
            {
                return gameData.GetItemName(id);
            }

            if (id >= 1000)
            {
                var name = gameData.GetBlueprintName(id - 1000);

                return $"{name} blueprint";
            }

            return "Dewdrops";
        }


        private static void PrintCasinoInfo(GameData gameData, SaveFile save)
        {
            if (save.CasinoSlotId.Length > 0)
            {
                Console.WriteLine("Casino Slot Machine Items");

                foreach (var id in save.CasinoSlotId.OrderBy(x => x))
                {
                    var name = GetCasinoName(gameData, id);

                    Console.WriteLine($"    {id,4} - {name}");
                }
            }

            if (save.CasinoSpinId.Length > 0)
            {
                Console.WriteLine("Casino Spinner Items");

                foreach (var id in save.CasinoSpinId.OrderBy(x => x))
                {
                    var name = GetCasinoName(gameData, id);

                    Console.WriteLine($"    {id,4} - {name}");
                }
            }
        }


        private static void PrintNumericAchievement(GameData gameData, string key, int val)
        {
            var entry = gameData.NumericAchievements.FirstOrDefault(x => x.Key == key)
                        ?? throw new KeyNotFoundException($"Numeric achievement key {key} not found");

            var currentThreshold = entry.Thresholds.LastOrDefault(x => x.Threshold <= val);
            var nextThreshold = entry.Thresholds.FirstOrDefault(x => x.Threshold > val);

            if (nextThreshold == null && currentThreshold != null)
            {
                Console.WriteLine($"    {entry.Name}: {currentThreshold.Name} (max, {val}/{currentThreshold.Threshold})");
            }
            else if (nextThreshold != null && currentThreshold != null)
            {
                Console.WriteLine($"    {entry.Name}: {currentThreshold.Name} ({currentThreshold.Threshold}) -> {nextThreshold.Name} ({val}/{nextThreshold.Threshold})");
            }
            else if (nextThreshold != null && currentThreshold == null)
            {
                Console.WriteLine($"    {entry.Name}: {nextThreshold.Name} ({val}/{nextThreshold.Threshold})");
            }
            else
            {
                // Both null?
                Console.WriteLine($"    {entry.Name}: both thresholds null!?!");
            }
        }


        private static void PrintMissing(string title, IEnumerable<string> gameData, IEnumerable<string> parsed)
        {
            var diff = gameData.Except(parsed).ToList();
            if (diff.Count != 0)
            {
                Console.WriteLine($"Missing {title}:");

                foreach (var item in diff.OrderBy(x => x))
                {
                    Console.WriteLine($"    {item}");
                }
            }
        }
    }
}
