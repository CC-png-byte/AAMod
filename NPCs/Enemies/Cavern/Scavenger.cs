using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BaseMod;

namespace AAMod.NPCs.Enemies.Cavern
{
    public class Scavenger : ModNPC
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Scavenger");	
		}		
		
		public override void SetDefaults()
		{
            npc.width = 24;
            npc.height = 24;
            npc.value = BaseUtility.CalcValue(0, 0, 0, 90);
            npc.npcSlots = 5;
            npc.aiStyle = -1;
            npc.lifeMax = 300;
            npc.defense = 15;
            npc.damage = 20;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.behindTiles = true;		
			npc.netAlways = true;
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.playerSafe || !NPC.downedPlantBoss || spawnInfo.player.ZoneJungle)
            {
                return 0f;
            }
            return SpawnCondition.Cavern.Chance * 0.02f;
        }

        public override void NPCLoot()
        {
            BaseAI.DropItem(npc, mod.ItemType("CovetiteCrystal"), Main.expertMode ? 1 + Main.rand.Next(1) : 1, 5, Main.expertMode ? 40 : 30, true);
            npc.DropLoot(mod.ItemType<Items.Usable.GreedKey>(), .05f);
        }

		public override void AI()
        {
            int[] types = new int[] { mod.NPCType("Scavenger"), mod.NPCType("ScavengerBody"), mod.NPCType("ScavengerTail") };
            BaseAI.AIWorm(npc, types, 7, 0, 12f, 0.1f, false, false, true, true, false);
        }

		public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            BaseDrawing.DrawTexture(sb, Main.npcTexture[npc.type], 0, npc, dColor, true);
            return false;
        }
	}


    public class ScavengerBody : Scavenger
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.value = BaseUtility.CalcValue(0, 0, 0, 75);
            npc.npcSlots = 3;
            npc.lifeMax = 300;
            npc.defense = 10;
            npc.damage = 15;
        }
    }

    public class ScavengerTail : Scavenger
    { 
        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.value = BaseUtility.CalcValue(0, 0, 0, 60);
            npc.npcSlots = 3;
            npc.lifeMax = 300;
            npc.defense = 10;
            npc.damage = 15;
        }
    }
}

