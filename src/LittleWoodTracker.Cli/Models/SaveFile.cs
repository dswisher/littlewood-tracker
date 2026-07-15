// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace LittleWoodTracker.Cli.Models
{
    public class SaveFile
    {
        /// <summary>
        /// The name of the player.
        /// </summary>
        public string? PlayerName { get; set; }

        /// <summary>
        /// The number of days played so far.
        /// </summary>
        public int? DaysPlayed { get; set; }

        /// <summary>
        /// The number of steps taken so far.
        /// </summary>
        public int? Steps { get; set; }

        /// <summary>
        /// A slash-separated list of objects on the map.
        /// </summary>
        public string? MapObjectString { get; set; }
    }
}
