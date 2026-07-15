// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
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

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }
    }
}
