using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace AAMod.Items.Vanity.VoidEye
{
    [AutoloadEquip(EquipType.Body)]
    public class VoidEyePlate : BaseAAItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Void Eye's Chestplate");
            Tooltip.SetDefault(@"'Great for impersonating Ancients Awakened Contributors!'");
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = new Color(148, 18, 142);
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 20;
            item.rare = 9;
            item.vanity = true;
        }
    }
}