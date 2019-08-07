using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Items.Ranged
{
    public class YtriumCrossbow : BaseAAItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yttrium Repeater");
			Tooltip.SetDefault("20% chance not to consume arrows");
        }

		public override void SetDefaults()
		{
			item.damage = 27;
			item.ranged = true;
			item.width = 48;
			item.height = 22;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 3;
            item.value = BaseMod.BaseUtility.CalcValue(0, 5, 0, 0);
            item.rare = 2;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 12f;
			item.useAmmo = AmmoID.Arrow;
		}
		
		public override bool ConsumeAmmo(Player player)
		{
		    return Main.rand.NextFloat() >= .20;
		}
		
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 0);
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod, "YtriumBar", 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
