using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Items.Materials
{
    public class CovetiteBar : BaseAAItem
    {
        public override void SetDefaults()
        {

            item.width = 30;
            item.height = 24;
            item.maxStack = 99;
			item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.value = 16000;
            item.rare = 2;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = mod.TileType("CovetiteBar");
			
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Covetite Bar");
            Tooltip.SetDefault("It's somehow shiny but not at the same time. How did greed fall for this?");
        }

		public override void AddRecipes()
        {                                                   
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CovetiteOre", 3);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
