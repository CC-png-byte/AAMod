using Terraria;
using Terraria.ModLoader;

namespace AAMod.Items.Armor.Champion.Carrot
{
    public class CBoost1 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Champion Boost");
            Description.SetDefault("Increased stats");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] == 2)
            {
				player.DelBuff(buffIndex);
				buffIndex--;
            }
        }
    }
}