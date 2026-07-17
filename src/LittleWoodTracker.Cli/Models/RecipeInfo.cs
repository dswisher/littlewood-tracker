// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace LittleWoodTracker.Cli.Models
{
    /// <summary>
    /// A tavern cooking recipe, identifying which appliance it belongs to,
    /// its two required ingredients, and whether it has been discovered.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Deserialized")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Deserialized")]
    public class RecipeInfo
    {
        /// <summary>
        /// The global recipe ID (0–119): 0–39 = Bubble Pot, 40–79 = Sizzle Pan, 80–119 = Chop Board.
        /// </summary>
        public required int GlobalId { get; set; }

        /// <summary>
        /// The recipe's in-game name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The appliance this recipe is cooked on: "BubblePot", "SizzlePan", or "ChopBoard".
        /// </summary>
        public required string Appliance { get; set; }

        /// <summary>
        /// The item ID of the first ingredient.
        /// </summary>
        public required int Ingredient1Id { get; set; }

        /// <summary>
        /// The item ID of the second ingredient.
        /// </summary>
        public required int Ingredient2Id { get; set; }
    }
}
