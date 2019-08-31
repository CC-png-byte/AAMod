using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Items.Armor.TrueAbyssal
{
    [AutoloadEquip(EquipType.Head)]
	public class TrueAbyssalFukumen : BaseAAItem
	{
		public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Perfect Abyssal Fukumen");
            Tooltip.SetDefault(@"35% increased movement speed
13% increased ranged damage");
        }

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.value = 50000;
			item.rare = 7;
			item.defense = 15;
		}

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += .13f;
            player.moveSpeed += .35f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("TrueAbyssalGi") && legs.type == mod.ItemType("TrueAbyssalHakama");
		}

		public override void UpdateArmorSet(Player player)
		{
            player.setBonus = Lang.ArmorBonus("TrueAbyssalFukumenBonus");
            player.aggro -= 6;
            player.GetModPlayer<AAPlayer>(mod).trueAbyssal = true;
            player.ammoCost75 = true;
            player.nightVision = true;
			player.detectCreature = true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("AbyssalFukumen"));
			recipe.AddIngredient(null, "MireCrystal", 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}