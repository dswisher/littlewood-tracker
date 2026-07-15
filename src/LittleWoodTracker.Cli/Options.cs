// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using CommandLine;

namespace LittleWoodTracker.Cli
{
    public class Options
    {
        [Option("save-num", Required = true, HelpText = "The save number to load.")]
        public int SaveNumber { get; set; }
    }
}
