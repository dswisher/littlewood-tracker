# Obtaining Item and Blueprint IDs

The item and blueprint IDs used by Littlewood's save files are embedded in the
game binary. This document describes how to extract them from the installed game
so that `ItemList.json` and `BlueprintList.json` can be updated when new content
is added.

## Prerequisites

- Littlewood installed via Steam on macOS
- .NET SDK installed
- `ilspycmd` installed as a global dotnet tool:
  ```
  dotnet tool install -g ilspycmd
  ```

## Step 1 — Locate the game DLL

The game logic is compiled into a single assembly at:

```
~/Library/Application Support/Steam/steamapps/common/Littlewood/Littlewood.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll
```

## Step 2 — Decompile

Run `ilspycmd` to decompile the assembly into C# source files:

```
ilspycmd \
  "$HOME/Library/Application Support/Steam/steamapps/common/Littlewood/Littlewood.app/Contents/Resources/Data/Managed/Assembly-CSharp.dll" \
  -p -o /tmp/littlewood-decompile
```

This produces a full C# project under `/tmp/littlewood-decompile/`. The relevant
file is `GameScript.cs`.

## Step 3 — Find the mapping functions

Search `GameScript.cs` for these two private methods:

- **`GetItemName(int id)`** — maps item IDs to display names
- **`GetBuildItemName(int id)`** — maps blueprint IDs to display names

Both are large `switch` statements. Entries where `result = ""` are unused or
reserved slots and should be omitted from the JSON files.

## Step 4 — Understand the category boundaries

The game's own category logic is expressed as a single ternary expression near
the line containing `"Grand Library Book"` and `"Important Town Object"`. Search
for `Important Town Object` in `GameScript.cs` to find it. The blueprint ID
ranges are:

| Category               | ID Range |
|------------------------|----------|
| Path                   | 0–39     |
| Tree                   | 40–79    |
| Crop                   | 80–119   |
| House                  | 120–159  |
| Structure              | 160–199  |
| Important Town Object  | 200–239  |
| Wallpaper              | 240–279  |
| Flooring               | 280–319  |
| Bed                    | 320–359  |
| Table                  | 360–399  |
| Chair                  | 400–439  |
| Lamp                   | 440–479  |
| Indoor Furniture       | 480–599  |
| Painting               | 600–639  |
| Outdoor Town Decor     | 640–719  |
| Tool                   | 720–799  |
| Flower                 | 800+     |

Items use a separate ID namespace with their own natural groupings; see
`GetItemName` for the full list.

## Step 5 — Update the JSON files

Rewrite `ItemList.json` and `BlueprintList.json` by reading the updated switch
statements, applying the category ranges above, and omitting empty-string
entries.

## Notes on casino save data

The save file fields `casinoSlotId` and `casinoSpinId` mix item IDs and blueprint
IDs in the same array using a simple encoding:

- Value `0` — empty slot
- Value `< 1000` — item ID (look up in `ItemList.json`)
- Value `>= 1000` — blueprint ID, where `blueprintId = savedValue - 1000`
  (look up in `BlueprintList.json`)
