// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using System.Linq;
using AwesomeAssertions;
using LittleWoodTracker.Cli;
using LittleWoodTracker.Cli.Models;
using Xunit;

namespace LittleWoodTracker.UnitTests
{
    public class RecipeTests
    {
        // Save: bruno-d130-s104285.json
        //   Tavern: structureUpgradeExp[20] = 9 -> level 4 (Sizzle Pan unlocked, Chop Board not yet)
        //   bubblePotRecipeUnlocked: 19/40 unlocked
        //   sizzlePanRecipeUnlocked: 7/40 unlocked
        //   chopBoardRecipeUnlocked: 0/40 unlocked
        //   recipeHintID: 40 -> Sizzle Pan local index 0 -> "Flaky Fillets" (locked)


        [Fact]
        public void RecipeListHas120Entries()
        {
            var gameData = GameDataLoader.Load();

            gameData.Recipes.Should().HaveCount(120);
            gameData.Recipes.Count(r => r.Appliance == "BubblePot").Should().Be(40);
            gameData.Recipes.Count(r => r.Appliance == "SizzlePan").Should().Be(40);
            gameData.Recipes.Count(r => r.Appliance == "ChopBoard").Should().Be(40);
        }


        [Fact]
        public void AllRecipeGlobalIdsAreUnique()
        {
            var gameData = GameDataLoader.Load();

            gameData.Recipes.Select(r => r.GlobalId).Should().OnlyHaveUniqueItems();
        }


        [Fact]
        public void BubblePotRecipe0IsSlimePuddingWithCorrectIngredients()
        {
            // Recipe 0: Slime Pudding = Slimeapple (120) + Slimeapple (120)
            var gameData = GameDataLoader.Load();

            var recipe = gameData.Recipes.Single(r => r.GlobalId == 0);

            recipe.Name.Should().Be("Slime Pudding");
            recipe.Appliance.Should().Be("BubblePot");
            recipe.Ingredient1Id.Should().Be(120);
            recipe.Ingredient2Id.Should().Be(120);
        }


        [Fact]
        public void SizzlePanRecipe40IsFlakyFilletsWithCorrectIngredients()
        {
            // Recipe 40: Flaky Fillets = Minnow (160) + Minnow (160)
            var gameData = GameDataLoader.Load();

            var recipe = gameData.Recipes.Single(r => r.GlobalId == 40);

            recipe.Name.Should().Be("Flaky Fillets");
            recipe.Appliance.Should().Be("SizzlePan");
            recipe.Ingredient1Id.Should().Be(160);
            recipe.Ingredient2Id.Should().Be(160);
        }


        [Fact]
        public void ChopBoardRecipe80IsCarrotCakeWithCorrectIngredients()
        {
            // Recipe 80: Carrot Cake = Carrot (140) + Carrot (140)
            var gameData = GameDataLoader.Load();

            var recipe = gameData.Recipes.Single(r => r.GlobalId == 80);

            recipe.Name.Should().Be("Carrot Cake");
            recipe.Appliance.Should().Be("ChopBoard");
            recipe.Ingredient1Id.Should().Be(140);
            recipe.Ingredient2Id.Should().Be(140);
        }


        [Fact]
        public void AllRecipeIngredientIdsResolve()
        {
            // Verify no ingredient ID maps to "n/a" (would indicate a bad ID in RecipeList.json)
            var gameData = GameDataLoader.Load();

            foreach (var recipe in gameData.Recipes)
            {
                gameData.GetItemName(recipe.Ingredient1Id).Should().NotBe("n/a",
                    because: $"recipe {recipe.GlobalId} '{recipe.Name}' Ingredient1Id {recipe.Ingredient1Id} should resolve");
                gameData.GetItemName(recipe.Ingredient2Id).Should().NotBe("n/a",
                    because: $"recipe {recipe.GlobalId} '{recipe.Name}' Ingredient2Id {recipe.Ingredient2Id} should resolve");
            }
        }


        [Fact]
        public void CanLoadRecipeUnlockArraysFromSave()
        {
            SaveFile save;
            using (var stream = OpenEmbedded("bruno-d130-s104285"))
            {
                save = SaveLoader.LoadSave(stream);
            }

            save.BubblePotRecipeUnlocked.Should().HaveCount(40);
            save.SizzlePanRecipeUnlocked.Should().HaveCount(40);
            save.ChopBoardRecipeUnlocked.Should().HaveCount(40);

            save.BubblePotRecipeUnlocked.Sum().Should().Be(19);
            save.SizzlePanRecipeUnlocked.Sum().Should().Be(7);
            save.ChopBoardRecipeUnlocked.Sum().Should().Be(0);
        }


        [Fact]
        public void CanLoadRecipeHintIdFromSave()
        {
            SaveFile save;
            using (var stream = OpenEmbedded("bruno-d130-s104285"))
            {
                save = SaveLoader.LoadSave(stream);
            }

            save.RecipeHintId.Should().Be(40);
        }


        [Fact]
        public void RecipeHintMapsToLockedSizzlePanRecipe()
        {
            // recipeHintID=40 -> Sizzle Pan local index 0 -> "Flaky Fillets", which is locked
            var gameData = GameDataLoader.Load();
            SaveFile save;
            using (var stream = OpenEmbedded("bruno-d130-s104285"))
            {
                save = SaveLoader.LoadSave(stream);
            }

            var hintedRecipe = gameData.Recipes.Single(r => r.GlobalId == save.RecipeHintId);

            hintedRecipe.Name.Should().Be("Flaky Fillets");
            hintedRecipe.Appliance.Should().Be("SizzlePan");

            var localId = save.RecipeHintId - 40;
            save.SizzlePanRecipeUnlocked[localId].Should().Be(0);  // still locked
        }


        private static Stream OpenEmbedded(string name)
        {
            var assembly = typeof(RecipeTests).Assembly;
            var resourceName = $"LittleWoodTracker.UnitTests.SaveFiles.{name}.json";
            var stream = assembly.GetManifestResourceStream(resourceName);

            return stream ?? throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");
        }
    }
}
