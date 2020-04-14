using Terraria;

namespace AAMod.Items.Vanity.Gibs
{
    public class GibsBag : BaseAAItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Angry Revenant's Sarcophagus");
            Tooltip.SetDefault("<right> to open \n'All the essentials for impersonating the Raging Revenant!'");
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
            player.QuickSpawnItem(mod.ItemType("GibsSkull"));
            player.QuickSpawnItem(mod.ItemType("GibsPlate"));
            player.QuickSpawnItem(mod.ItemType("GibsShorts"));
            if (Main.hardMode)
            {
                player.QuickSpawnItem(mod.ItemType("GibsJet"));
            }
        }
    }
}