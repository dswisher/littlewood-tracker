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

        /// <summary>
        /// All 24 travelers, keyed by their index.
        /// </summary>
        public Dictionary<int, TravelerInfo> TravelersByIndex { get; } = [];

        /// <summary>
        /// All 120 tavern recipes.
        /// </summary>
        public List<RecipeInfo> Recipes { get; } = [];

        /// <summary>
        /// All known donation targets, keyed by their structureUpgradeEXP array index.
        /// </summary>
        public Dictionary<int, DonationTargetInfo> DonationTargetsByIndex { get; } = [];

        /// <summary>
        /// The upgrade cost table, keyed by (targetLevel, slot).
        /// </summary>
        public Dictionary<(int Level, int Slot), UpgradeCostEntry> UpgradeCosts { get; } = [];

        public string GetItemName(int id)
        {
            return ItemNames.GetValueOrDefault(id, "n/a");
        }

        public string GetBlueprintName(int id)
        {
            return BlueprintNames.GetValueOrDefault(id, "n/a");
        }

        /// <summary>
        /// Returns the next upgrade cost for a structure at the given EXP value,
        /// or null if the structure is already at max level.
        /// </summary>
        public UpgradeCostEntry? GetNextUpgradeCost(int exp)
        {
            const int maxLevel = 10;

            var level = 1 + exp / 3;
            var remainder = exp % 3;

            if (level >= maxLevel)
            {
                return null;
            }

            var targetLevel = level + 1;
            var slot = remainder;

            return UpgradeCosts.GetValueOrDefault((targetLevel, slot));
        }

        /// <summary>
        /// Returns true if the given donation target has been unlocked in the provided save.
        /// For structures (those with a BlueprintId), the target must have been built
        /// (discoverLevel == 2). For locations (no BlueprintId), a non-zero
        /// structureUpgradeEXP value indicates the location has been unlocked.
        /// </summary>
        public static bool IsUnlocked(DonationTargetInfo target, SaveFile save)
        {
            if (target.BlueprintId.HasValue)
            {
                var bpId = target.BlueprintId.Value;
                return bpId < save.DiscoverLevel.Length && save.DiscoverLevel[bpId] == 2;
            }

            return target.Index < save.StructureUpgradeExp.Length && save.StructureUpgradeExp[target.Index] > 0;
        }
    }
}
