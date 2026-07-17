// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace LittleWoodTracker.Cli.Models
{
    /// <summary>
    /// Maps a donation target's save-file index to its in-game name.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Deserialized")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Deserialized")]
    public class DonationTargetInfo
    {
        /// <summary>
        /// The index into the structureUpgradeEXP save array.
        /// </summary>
        public required int Index { get; set; }

        /// <summary>
        /// The in-game display name of the donation target.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The blueprint ID used to check whether this target has been built, via the
        /// discoverLevel save array. A value of 2 means built.
        /// Null for locations (Endless Forest, Dust Cavern, etc.) which have no blueprint;
        /// for those, a non-zero structureUpgradeEXP value indicates the location is unlocked.
        /// </summary>
        public int? BlueprintId { get; set; }
    }
}
