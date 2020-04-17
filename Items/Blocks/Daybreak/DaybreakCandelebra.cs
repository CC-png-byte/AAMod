using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Items.Blocks.Daybreak
{
    public class DaybreakCandelabra : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Daybreak Candelabra");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 32;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 250;
            item.createTile = mod.TileType("DaybreakCandelabra");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("DaybreakIncinerite"), 5);
            recipe.AddIngredient(ItemID.Torch, 3);
            recipe.AddTile(null, "ACS");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}