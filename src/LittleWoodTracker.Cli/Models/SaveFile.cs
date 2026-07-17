// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace LittleWoodTracker.Cli.Models
{
    [SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global", Justification = "Deserialized")]
    public class SaveFile
    {
        /// <summary>
        /// The name of the player.
        /// </summary>
        public required string PlayerName { get; set; }

        /// <summary>
        /// The number of days played so far.
        /// </summary>
        public required int DaysPlayed { get; set; }

        /// <summary>
        /// The number of steps taken so far.
        /// </summary>
        public required int Steps { get; set; }

        /// <summary>
        /// A slash-separated list of objects on the map.
        /// </summary>
        public required string MapObjectString { get; set; }

        /// <summary>
        /// The number of items gathered so far.
        /// </summary>
        public required int ItemsGathered { get; set; }

        /// <summary>
        /// The number of trees chopped so far.
        /// </summary>
        public required int TreesChopped { get; set; }

        /// <summary>
        /// The number of ores mined so far.
        /// </summary>
        public required int OresMined { get; set; }

        /// <summary>
        /// The number of fish caught so far.
        /// </summary>
        public required int FishCaught { get; set; }

        /// <summary>
        /// The number of bugs caught so far.
        /// </summary>
        public required int BugsCaught { get; set; }

        /// <summary>
        /// The number of crops harvested so far.
        /// </summary>
        public required int CropsHarvested { get; set; }

        /// <summary>
        /// The number of items crafted so far.
        /// </summary>
        public required int ItemsCrafted { get; set; }

        /// <summary>
        /// The number of items sold so far.
        /// </summary>
        public required int ItemsSold { get; set; }

        /// <summary>
        /// The number of quests completed so far.
        /// </summary>
        public required int QuestsCompleted { get; set; }

        /// <summary>
        /// The items in the casino spinner.
        /// </summary>
        /// <remarks>
        /// If the ID > 1000, it is a blueprint (subtract 1000 to find it).
        /// </remarks>
        public required int[] CasinoSpinId { get; set; }

        /// <summary>
        /// The items in the casino slot machine.
        /// </summary>
        public required int[] CasinoSlotId { get; set; }

        /// <summary>
        /// Whether a blueprint has been discovered and/or built.
        /// </summary>
        /// <remarks>
        /// The index in the array is the blueprint ID.
        /// A value of 0 indicates that blueprint is unknown, 1 indicates that it has
        /// been discovered but not built, and 2 indicates the blueprint has been built.
        /// </remarks>
        public required int[] DiscoverLevel { get; set; }

        /// <summary>
        /// The upgrade experience for each structure, indexed by structure index.
        /// </summary>
        /// <remarks>
        /// The level is derived as: level = 1 + (exp / 3). Maximum level is 10.
        /// The remainder (exp % 3) indicates progress within the current level (0, 1, or 2 slots filled).
        /// </remarks>
        [JsonPropertyName("structureUpgradeEXP")]
        public required int[] StructureUpgradeExp { get; set; }
    }
}
