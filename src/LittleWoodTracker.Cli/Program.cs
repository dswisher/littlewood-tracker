// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;

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

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
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
