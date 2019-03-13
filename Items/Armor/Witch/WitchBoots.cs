using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace AAMod.Items.Armor.Witch
{
    [AutoloadEquip(EquipType.Legs)]
	public class WitchBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fury Witch's Boots");
			Tooltip.SetDefault(@"12% increased magic/minion damage
12% increased movement speed
+2 max minions
A robe enchanted with the firey spirit of a supreme dragon acolyte");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 16;
			item.value = 300000;
			item.defense = 20;
		}

		public override void UpdateEquip(Player player)
		{
            player.magicDamage += .12f;
            player.minionDamage += .12f;
            player.moveSpeed += .1f;
            player.maxMinions += 2;
		}

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = AAColor.Yamata;;
                }
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "EventideAbyssium", 18);
            recipe.AddIngredient(null, "DreadScale", 5);
            recipe.AddIngredient(null, "DepthHakama", 1);
            recipe.AddTile(null, "ACS");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}