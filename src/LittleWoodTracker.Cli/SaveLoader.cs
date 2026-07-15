// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.IO;
using System.Text.Json;
using LittleWoodTracker.Cli.Models;

namespace LittleWoodTracker.Cli
{
    public static class SaveLoader
    {
        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };


        public static SaveFile LoadSave(int saveNumber)
        {
            string path;

            if (OperatingSystem.IsMacOS())
            {
                var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                path = Path.Combine(home, "Library", "Application Support", "SmashGames", "Littlewood", $"games{saveNumber}.json");
            }
            else
            {
                throw new NotImplementedException($"Save file path is not implemented for this operating system.");
            }

            using var stream = File.OpenRead(path);
            return LoadSave(stream);
        }


        public static SaveFile LoadSave(Stream stream)
        {
            return JsonSerializer.Deserialize<SaveFile>(stream, jsonOptions)
                ?? throw new JsonException("Deserialization returned null.");
        }
    }
}
