using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Items.Blocks.Doom
{
    public class DoomPiano : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doom Piano");
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
            item.createTile = mod.TileType("DoomPiano");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ApocalyptitePlate"), 15);
            recipe.AddIngredient(ItemID.Book);
            recipe.AddIngredient(ItemID.Bone, 4);
            recipe.AddTile(null, "ACS");
            recipe.SetResult(this);
            recipe.AddRecipe();

        }

    }
}