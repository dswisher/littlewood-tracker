// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LittleWoodTracker.Cli.Models
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Deserialized")]
    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Global", Justification = "Deserialized")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Deserialized")]
    public class ItemListEntry
    {
        public required string Name { get; set; }
        public required List<ItemListItem> Items { get; set; }
    }
}
