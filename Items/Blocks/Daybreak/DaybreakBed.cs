using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Items.Blocks.Daybreak
{
    public class DaybreakBed : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Daybreak Bed");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 22;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 250;
            item.createTile = mod.TileType("DaybreakBed");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("DaybreakIncinerite"), 15);
            recipe.AddIngredient(ItemID.Silk, 5);
            recipe.AddTile(null, "ACS");
            recipe.SetResult(this);
            recipe.AddRecipe();

        }

    }
}