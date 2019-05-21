using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using AAMod.NPCs.Bosses.Shen;
using System.Collections.Generic;
using BaseMod;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AAMod.Items.DevTools
{
    public class HardmodeUndowner : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hardmode Undowner");
            Tooltip.SetDefault(@"Undowns all post-WOF bosses.
Non-Consumable");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.rare = 2;
            item.value = Item.sellPrice(0, 0, 0, 0);
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = 4;
        }

        public override bool UseItem(Player player)
        {
            NPC.downedAncientCultist = false;
            NPC.downedChristmasIceQueen = false;
            NPC.downedChristmasSantank = false;
            NPC.downedChristmasTree = false;
            NPC.downedClown = false;
            NPC.downedFishron = false;
            NPC.downedFrost = false;
            NPC.downedGolemBoss = false;
            NPC.downedHalloweenKing = false;
            NPC.downedHalloweenTree = false;
            NPC.downedMartians = false;
            NPC.downedMechBoss1 = false;
            NPC.downedMechBoss2 = false;
            NPC.downedMechBoss3 = false;
            NPC.downedMechBossAny = false;
            NPC.downedMoonlord = false;
            NPC.downedPirates = false;
            NPC.downedPlantBoss = false;
            NPC.downedTowerNebula = false;
            NPC.downedTowerSolar = false;
            NPC.downedTowerStardust = false;
            NPC.downedTowerVortex = false;
            AAWorld.downedAkuma = false;
            AAWorld.downedAllAncients = false;
            AAWorld.downedAshe = false;
            AAWorld.downedDB = false;
            AAWorld.downedNC = false;
            AAWorld.downedEquinox = false;
            AAWorld.downedHaruka = false;
            AAWorld.downedIZ = false;
            AAWorld.downedKraken = false;
            AAWorld.downedOrthrus = false;
            AAWorld.downedRaider = false;
            AAWorld.downedRajah = false;
            AAWorld.downedRetriever = false;
            AAWorld.downedSAncient = false;
            AAWorld.downedShen = false;
            AAWorld.downedSisters = false;
            AAWorld.downedSoC = false;
            AAWorld.downedStormAll = false;
            AAWorld.downedStormAny = false;
            AAWorld.downedTruffle = false;
            AAWorld.downedYamata = false;
            AAWorld.downedZero = false;
            return true;
        }
    }
}