// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace LittleWoodTracker.Cli.Models
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Deserialized")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Deserialized")]
    public class ItemListItem
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }
}
