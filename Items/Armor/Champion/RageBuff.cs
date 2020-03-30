using Terraria;
using Terraria.ModLoader;

namespace AAMod.Items.Armor.Champion
{
    public class RageBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Rajah's Rage");
            Description.SetDefault("A champion of Terraria never backs down");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.statLife < (int)(player.statLifeMax2 * .25f))
            {
                player.statDefense += 30;
                return;
            }
            if (player.statLife < (int)(player.statLifeMax2 * .5f))
            {
                player.statDefense += 20;
                return;
            }
            if (player.statLife < (int)(player.statLifeMax2 * .75f))
            {
                player.statDefense += 10;
                return;
            }
            if (player.statLife > (int)(player.statLifeMax2 * .75f))
            {
                player.statDefense += 5;
                return;
            }
        }
    }
}