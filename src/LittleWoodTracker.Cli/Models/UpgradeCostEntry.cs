// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace LittleWoodTracker.Cli.Models
{
    /// <summary>
    /// The cost of a single upgrade donation slot.
    /// </summary>
    /// <remarks>
    /// Each level requires three donations (slots 0, 1, and 2). The Level field is the
    /// level being filled (i.e. the level the structure will reach after all three slots
    /// are filled), and Slot is which of the three donations this entry describes.
    /// </remarks>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Deserialized")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Deserialized")]
    public class UpgradeCostEntry
    {
        /// <summary>
        /// The target level (2–10) that this donation contributes toward.
        /// </summary>
        public required int Level { get; set; }

        /// <summary>
        /// The donation slot within the level (0, 1, or 2).
        /// </summary>
        public required int Slot { get; set; }

        /// <summary>
        /// The name of the item required.
        /// </summary>
        public required string ItemName { get; set; }

        /// <summary>
        /// The quantity of the item required.
        /// </summary>
        public required int Quantity { get; set; }
    }
}
