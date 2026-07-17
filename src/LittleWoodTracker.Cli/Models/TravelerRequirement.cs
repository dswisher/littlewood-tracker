// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace LittleWoodTracker.Cli.Models
{
    /// <summary>
    /// A single item requirement for a traveler visit.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Deserialized")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Deserialized")]
    public class TravelerRequirement
    {
        /// <summary>
        /// The item ID that must have been sold.
        /// </summary>
        public required int ItemId { get; set; }

        /// <summary>
        /// The cumulative quantity of the item that must have been sold.
        /// </summary>
        public required int Quantity { get; set; }
    }
}
