using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace AAMod.Items.Armor.TrueFleshrend
{
    [AutoloadEquip(EquipType.Head)]
	public class TrueFleshrendHelm : BaseAAItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Fleshrend Helm");
			Tooltip.SetDefault("13% increased melee damage");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 26;
			item.value = 100000;
			item.rare = 7;
			item.defense = 29;
		}
		
		public override void UpdateEquip(Player player)
		{
            player.meleeDamage *= 1.13f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("TrueFleshrendPlate") && legs.type == mod.ItemType("TrueFleshrendGreaves");
		}

		public override void UpdateArmorSet(Player player)
		{

            player.setBonus = Lang.ArmorBonus("TrueFleshrendHelmBonus");
            
			player.GetModPlayer<AAPlayer>(mod).fleshrendSet = true;
            player.GetModPlayer<AAPlayer>(mod).trueFlesh = true;
        }



        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FleshrendHelm", 1);
            recipe.AddIngredient(null, "CrimsonCrystal", 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}