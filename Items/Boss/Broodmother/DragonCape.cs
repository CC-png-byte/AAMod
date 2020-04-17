using Terraria;
using Microsoft.Xna.Framework; using Microsoft.Xna.Framework.Graphics; using Terraria.ModLoader;
using Terraria.ID;

namespace AAMod.Items.Boss.Broodmother
{
    [AutoloadEquip(EquipType.Back, EquipType.Front)]
    public class DragonCape : BaseAAItem
    {
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dragontamer's Cloak");
            Tooltip.SetDefault(
@"3% Increased Damage Resistance");
        }
        public override void SetDefaults()
        {
            item.width = 66;
            item.height = 78;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.accessory = true;
            item.expert = true; item.expertOnly = true;
            item.defense = 3;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.endurance += .03f;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (slot < 10)
            {
                int maxAccessoryIndex = 5 + player.extraAccessorySlots;
                for (int i = 3; i < 3 + maxAccessoryIndex; i++)
                {
                    if (slot != i && player.armor[i].type == ModContent.ItemType<DragonSerpentNecklace>())
                    {
                        return false;
                    }
                    if (slot != i && player.armor[i].type == ItemID.WormScarf)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
    
}