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
                    options => Run(options),
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

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }


        private static void PrintNumericAchievement(GameData gameData, string key, int val)
        {
            var entry = gameData.NumericAchievements.FirstOrDefault(x => x.Key == key);

            if (entry == null)
            {
                throw new KeyNotFoundException($"Numeric achievement key {key} not found");
            }

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
