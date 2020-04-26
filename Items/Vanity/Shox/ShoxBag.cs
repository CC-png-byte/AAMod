using Terraria.ModLoader;
using Terraria;

namespace AAMod.Items.Vanity.Shox
{
    public class ShoxBag : BaseAAItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Charged Shock Bag");
            Tooltip.SetDefault("<right> to open \n'All the essentials for impersonating the Shock Lord!'");
        }

        public override void SetDefaults()
        {
            item.maxStack = 1;
            item.consumable = true;
            item.width = 32;
            item.height = 32;
            item.expert = true; item.expertOnly = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            player.QuickSpawnItem(ModContent.ItemType<ShoxVisor>());
            player.QuickSpawnItem(ModContent.ItemType<ShoxPlate>());
            player.QuickSpawnItem(ModContent.ItemType<ShoxPants>());
        }
    }
}