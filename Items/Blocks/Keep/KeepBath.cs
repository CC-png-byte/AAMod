using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Items.Blocks.Keep
{
    public class KeepBath : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Keep Bathtub");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 26;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 250;
            item.createTile = mod.TileType("KeepBath");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("TerraShard"), 14);
            recipe.AddTile(TileID.Sawmill);
            recipe.SetResult(this);
            recipe.AddRecipe();
            
        }

    }
}