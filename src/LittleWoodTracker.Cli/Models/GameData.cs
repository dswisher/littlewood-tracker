// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace LittleWoodTracker.Cli.Models
{
    public class GameData
    {
        public HashSet<string> Houses { get; } = [];
        public HashSet<string> Structures { get; } = [];
        public HashSet<string> Crops { get; } = [];

        public Dictionary<int, string> ItemNames { get; } = [];
        public Dictionary<int, string> BlueprintNames { get; } = [];

        public List<NumericAchievement> NumericAchievements { get; } = [];

        public string GetItemName(int id)
        {
            return ItemNames.GetValueOrDefault(id, "n/a");
        }

        public string GetBlueprintName(int id)
        {
            return BlueprintNames.GetValueOrDefault(id, "n/a");
        }
    }
}
