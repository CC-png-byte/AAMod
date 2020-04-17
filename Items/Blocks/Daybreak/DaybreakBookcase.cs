using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Items.Blocks.Daybreak
{
    public class DaybreakBookcase : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Daybreak Bookcase");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 34;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 250;
            item.createTile = mod.TileType("DaybreakBookcase");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("DaybreakIncinerite"), 20);
            recipe.AddIngredient(ItemID.Book, 10);
            recipe.AddTile(null, "ACS");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}