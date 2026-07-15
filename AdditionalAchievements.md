# Additional Achievements

Non-numeric Steam achievements for Littlewood that may be implementable given save file data.

| Name | Description | Notes |
|---|---|---|
| Pet Lover | Adopt a pet | `adoptedPet` array in save file |
| A Lovely Wedding | Get married | `isMarried` bool in save file |
| The Edge | Visit the Edge of Solemn | `unlockedEdge` bool in save file |
| Dewdrop Upgrader | Upgrade all structures and areas to 10 stars | Requires understanding `structureUpgradeEXP` index mapping |
| Ultimate Upgrader | Unlock all ultimate upgrades at the Crafting Workshop | `ultimateUpgraded` bool array in save file |
| So It Is Written | Unlock all town wishes | `townWishLevel` array in save file |
| Master Chef | Discover all Tavern recipes | `bubblePotRecipeUnlocked` array in save file |
| A Perfect Museum | Donate all items to Museum | `museumDonations` / `museumDonatedItem` in save file |
| Tarott Collector | Find all Tarott Monster cards | `tarottUnlock` bool array in save file |
| Goal Completionist | Complete all Personal Goals | `curPersonalGoalBook` / `curPersonalGoalPage` in save file |
