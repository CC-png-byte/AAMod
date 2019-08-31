using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace AAMod.Items.Armor.TrueDeathly
{
    [AutoloadEquip(EquipType.Legs)]
	public class TrueDeathlyGreaves : BaseAAItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deathly Ghast Greaves");
            Tooltip.SetDefault(@"5% Increased ranged damage
15% Increased movement speed
");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 100000;
            item.rare = 8;
            item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage *= 1.05f;
            player.moveSpeed *= 1.15f;
        }

        public override void AddRecipes()
        {
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemID.Ectoplasm, 15);
                recipe.AddIngredient(ItemID.Bone, 45);
                recipe.AddIngredient(null, "DeathlyGreaves", 1);
                recipe.AddTile(null, "TruePaladinsSmeltery");
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}