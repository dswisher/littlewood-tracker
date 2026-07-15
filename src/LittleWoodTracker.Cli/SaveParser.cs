// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using LittleWoodTracker.Cli.Models;

namespace LittleWoodTracker.Cli
{
    public static class SaveParser
    {
        public static ParsedSaveFile ParseSave(SaveFile save)
        {
            // Create the parsed save file structure that we'll fill in
            var parsedSave = new ParsedSaveFile();

            // Load the static lists of things
            // TODO

            // Go through and extract out the list of things on the map. Note that the game
            // cannot be saved while in other zones (Port Deluca, Endless Forest, etc). But
            // the upgrade (rebuild) status in Deluca must be stored somewhere.
            if (save.MapObjectString != null)
            {
                parsedSave.MapItems.UnionWith(save.MapObjectString.Split('/'));
            }


            // TODO - implement me!

            return parsedSave;
        }
    }
}
