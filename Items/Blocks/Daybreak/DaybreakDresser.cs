using Terraria.ModLoader;

namespace AAMod.Items.Blocks.Daybreak
{
    public class DaybreakDresser : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Daybreak Dresser");
        }

        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 24;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 250;
            item.createTile = mod.TileType("DaybreakDresser");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("DaybreakIncinerite"), 16);
            recipe.AddTile(null, "ACS");
            recipe.SetResult(this);
            recipe.AddRecipe();

        }

    }
}