
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Yamata
{
    public class YamataTransition : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spirit of Wrath");
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = 20;
            npc.height = 32;
            npc.scale *= 1.2f;
            npc.friendly = false;
            npc.scale *= 1.5f;
            npc.life = 1;
        }
        public int timer;


        public int RVal = 125;
        public int BVal = 255;

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(RVal, 0, BVal);
        }

        public override void AI()
        {
            timer++;
            npc.frameCounter++;
            if (npc.frameCounter >= 7)
            {
                npc.frameCounter = 0;
                npc.frame.Y += Main.npcTexture[npc.type].Height / 4 ;
            }

            if (npc.frame.Y > (Main.npcTexture[npc.type].Height / 4) * 3)
            {
                npc.frame.Y = 0 ;
            }
            if (timer == 375)    
            {
                Main.NewText("NYEHEHEHEHEHEHEHEH~!", new Color(45, 46, 70));
                AAMod.YamataMusic = true;
            }
            if (timer == 650)
            {
                Main.NewText("You thought I was DONE..?!", new Color(45, 46, 70));
            }
            if (timer == 900)
            {
                Main.NewText("HAH! AS IF!", new Color(45, 46, 70));
            }

            if (timer >= 900)
            {
                RVal += 5;
                BVal -= 5;
                if (RVal <= 90)
                {
                    BVal = 90;
                }
                if (RVal >= 255)
                {
                    RVal = 255;
                }
            }

            if (timer == 1100)
            {
                Main.NewText("I HOPE YOU ARE READY...", new Color(146, 30, 68));
            }
            if (timer == 1455)
            {
                npc.life = 0;             
            }
        }

        public override bool PreNPCLoot()
        {
            Dust dust1;
            Dust dust2;
            Dust dust3;
            Dust dust4;
            Dust dust5;
            Dust dust6;
            Vector2 position = npc.position;
            dust1 = Main.dust[Dust.NewDust(position, npc.width, npc.height, mod.DustType<Dusts.YamataADust>(), 0, 0, 0, default(Color), 1f)];
            dust2 = Main.dust[Dust.NewDust(position, npc.width, npc.height, mod.DustType<Dusts.YamataADust>(), 0, 0, 0, default(Color), 1f)];
            dust3 = Main.dust[Dust.NewDust(position, npc.width, npc.height, mod.DustType<Dusts.YamataADust>(), 0, 0, 0, default(Color), 1f)];
            dust4 = Main.dust[Dust.NewDust(position, npc.width, npc.height, mod.DustType<Dusts.YamataADust>(), 0, 0, 0, default(Color), 1f)];
            dust5 = Main.dust[Dust.NewDust(position, npc.width, npc.height, mod.DustType<Dusts.YamataADust>(), 0, 0, 0, default(Color), 1f)];
            dust6 = Main.dust[Dust.NewDust(position, npc.width, npc.height, mod.DustType<Dusts.YamataADust>(), 0, 0, 0, default(Color), 1f)];
            dust1.noGravity = true;
            dust1.velocity.Y -= 1;
            dust2.noGravity = true;
            dust2.velocity.Y -= 1;
            dust3.noGravity = true;
            dust3.velocity.Y -= 1;
            dust4.noGravity = true;
            dust4.velocity.Y -= 1;
            dust5.noGravity = true;
            dust5.velocity.Y -= 1;
            dust6.noGravity = true;
            dust6.velocity.Y -= 1;

            SpawnBoss(npc.Center, "YamataA", "Yamata Awakened");
            Main.NewText("Yamata has been Awakened!", Color.Magenta.R, Color.Magenta.G, Color.Magenta.B);
            Main.NewText("...TO FACE MY TRUE ABYSSAL WRATH, YOU LITTLE WRETCH!!!", new Color(146, 30, 68));
            AAMod.YamataMusic = false;
            return false;
        }

        public void SpawnBoss(Vector2 center, string name, string displayName)
        {
            if (Main.netMode != 1)
            {
                int bossType = mod.NPCType(name);
                if (NPC.AnyNPCs(bossType)) { return; } //don't spawn if there's already a boss!
                int npcID = NPC.NewNPC((int)center.X, (int)center.Y, bossType, 0);
                Main.npc[npcID].Center = center - new Vector2(MathHelper.Lerp(-100f, 100f, (float)Main.rand.NextDouble()), 0f);
                Main.npc[npcID].netUpdate2 = true;			
                string npcName = (!string.IsNullOrEmpty(Main.npc[npcID].GivenName) ? Main.npc[npcID].GivenName : displayName);
                /*f (Main.netMode == 0) { Main.NewText(Language.GetTextValue("Announcement.HasAwoken", npcName), 175, 75, 255, false); }
                else
                if (Main.netMode == 2)
                {
                    NetMessage.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", new object[]
                    {
                        NetworkText.FromLiteral(npcName)
                    }), new Color(175, 75, 255), -1);
                }*/
            }
        }

    }
}