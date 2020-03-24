using Terraria;
using Terraria.ModLoader;
using AAMod.Items.Vanity.Hallam.Shiny;
using Terraria.ID;

namespace AAMod.Items.Vanity.Hallam
{
    public class MagiciansHat : BaseAAItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magician's Top Hat");
            Tooltip.SetDefault("<right> to open \n'All the essentials for impersonating the Mad Cat!'");
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
            if (player.GetModPlayer<AAPlayer>().ShinyCheck())
            {
                player.QuickSpawnItem(ModContent.ItemType<ShinyHalHat>());
                player.QuickSpawnItem(ModContent.ItemType<ShinyHalTux>());
                player.QuickSpawnItem(ModContent.ItemType<ShinyHalTux>());
                if (Main.rand.Next(10) == 0)
                {
                    player.QuickSpawnItem(ItemID.GoldBunny);
                }
                return;
            }
			player.QuickSpawnItem(ModContent.ItemType<HalHat>());
            player.QuickSpawnItem(ModContent.ItemType<HalTux>());
            player.QuickSpawnItem(ModContent.ItemType<HalTrousers>());

            if (Main.rand.Next(10) == 0)
            {
                player.QuickSpawnItem(ItemID.Bunny);
            }
        }
    }
}