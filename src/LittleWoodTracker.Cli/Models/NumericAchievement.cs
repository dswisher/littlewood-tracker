// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace LittleWoodTracker.Cli.Models
{
    public class NumericAchievement
    {
        public required string Name { get; set; }
        public required string Key { get; set; }

        public List<NumericThreshold> Thresholds { get; set; } = [];
    }
}
