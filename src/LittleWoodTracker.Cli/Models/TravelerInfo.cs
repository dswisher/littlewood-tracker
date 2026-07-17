// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LittleWoodTracker.Cli.Models
{
    /// <summary>
    /// Information about a traveler who visits the town and eventually donates a book
    /// to the Grand Library.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Deserialized")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Deserialized")]
    public class TravelerInfo
    {
        /// <summary>
        /// The traveler's index (0–23), shared with the travelerLevel and libraryBook arrays.
        /// </summary>
        public required int Index { get; set; }

        /// <summary>
        /// The traveler's full in-game name (e.g. "Wedge, the Merchant").
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The title of the book this traveler donates on their third visit.
        /// </summary>
        public required string BookTitle { get; set; }

        /// <summary>
        /// Where this traveler appears: "Marketplace", "Dust Cavern", or "Endless Forest".
        /// </summary>
        public required string Source { get; set; }

        /// <summary>
        /// The sale requirements for each of the three visits (indices 0, 1, 2).
        /// Null for Dust Cavern and Endless Forest travelers, who are not triggered by sales.
        /// Each visit has a list of one or two requirements that must all be met.
        /// </summary>
        public List<List<TravelerRequirement>>? Requirements { get; set; }
    }
}
