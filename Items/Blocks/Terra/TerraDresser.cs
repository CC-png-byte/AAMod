using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Items.Blocks.Terra
{
    public class TerraDresser : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terra Dresser");
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
            item.createTile = mod.TileType("TerraDresser");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("TerraShard"), 16);
            recipe.AddIngredient(ItemID.HallowedBar, 4);
            recipe.AddTile(TileID.Sawmill);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }

    }
}