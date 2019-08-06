﻿using Terraria;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using BaseMod;
using System.IO;
using Terraria.Graphics.Shaders;

namespace AAMod.NPCs.Bosses.Akuma.Awakened
{
    [AutoloadBossHead]
    public class AkumaA : ModNPC
    {
        public bool Loludided;
        private bool weakness = false;
        public int fireTimer = 0;
        public int damage = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Oni Akuma");
            NPCID.Sets.TechnicallyABoss[npc.type] = true;
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.noTileCollide = true;
            npc.width = 80;
            npc.height = 80;
            npc.aiStyle = -1;
            npc.netAlways = true;
            npc.damage = 110;
            npc.defense = 270;
            npc.lifeMax = 600000;
            npc.value = Item.sellPrice(0, 40, 0, 0);
            npc.knockBackResist = 0f;
            npc.boss = true;
            npc.aiStyle = -1;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.behindTiles = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = mod.GetLegacySoundSlot(SoundType.NPCKilled, "Sounds/Sounds/AkumaRoar");
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Akuma2");
            musicPriority = MusicPriority.BossHigh;
            bossBag = mod.ItemType("AkumaBag");
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            npc.buffImmune[103] = false;
            npc.alpha = 255;
            musicPriority = MusicPriority.BossHigh;
            if (AAWorld.downedAllAncients)
            {
                npc.damage = 140;
                npc.defense = 300;
                npc.lifeMax = 750000;
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }


        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            if (Main.expertMode)
            {
                potionType = ItemID.SuperHealingPotion;
            }
            else
            {
                potionType = 0;
            }
        }

        private int attackTimer;
        public static int MinionCount = 0;

        public float[] internalAI = new float[4];
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == 2 || Main.dedServ)
            {
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
            }
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == 1)
            {
                internalAI[1] = reader.ReadFloat();
                internalAI[2] = reader.ReadFloat();
                internalAI[3] = reader.ReadFloat();
            }
        }
        public Texture2D AkumaTex = null;

        public override bool PreAI()
        {
            Player player = Main.player[npc.target];
            if (Main.expertMode)
            {
                damage = npc.damage / 4;
            }
            else
            {
                damage = npc.damage / 2;
            }

            npc.frameCounter++;
            if (npc.frameCounter > 8)
            {
                npc.frameCounter = 0;
                npc.frame.Y += 146;
            }
            if (npc.frame.Y > 146 * 2)
            {
                npc.frame.Y = 0;
            }
            float dist = npc.Distance(player.Center);
            npc.ai[2]++;
            if (npc.ai[2] == 300)
            {
                QuoteSaid = false;
                Roar(roarTimerMax, false);
                internalAI[1] += 1;
            }
            if (npc.ai[2] > 300)
            {
                Attack(npc);
            }
            if (npc.ai[2] >= 400)
            {
                npc.ai[2] = 0;
            }

            if (npc.life <= npc.lifeMax / 2 && !spawnAshe)
            {
                spawnAshe = true;
                if (AAWorld.downedAkuma)
                {
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("AkumaA1"), Color.DeepSkyBlue);
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("AkumaA2"), new Color(102, 20, 48));
                    AAModGlobalNPC.SpawnBoss(player, mod.NPCType("AsheA"), false, 0, 0);
                }
                else
                {
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("AkumaA3"), new Color(102, 20, 48));
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("AkumaA4"), Color.DeepSkyBlue);
                    AAModGlobalNPC.SpawnBoss(player, mod.NPCType("AsheA"), false, 0, 0);
                }
            }

            if (dist > 400 & Main.rand.Next(20) == 1 && npc.ai[1] == 0 && npc.ai[2] < 300)
            {
                npc.ai[1] = 1;
            }
            if (npc.ai[1] == 1)
            {
                attackTimer++;
                if ((attackTimer == 20 || attackTimer == 50 || attackTimer == 79) && npc.HasBuff(BuffID.Wet))
                {
                    for (int spawnDust = 0; spawnDust < 2; spawnDust++)
                    {
                        int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, mod.DustType("MireBubbleDust"), 0f, 0f, 90, default, 2f);
                        Main.dust[num935].noGravity = true;
                        Main.dust[num935].velocity.Y -= 1f;
                    }
                    if (weakness == false)
                    {
                        weakness = true;
                        if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("AkumaA7"), Color.DeepSkyBlue);
                    }
                }
                else if (!npc.HasBuff(BuffID.Wet))
                {
                    Main.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 20);
                    AAAI.BreatheFire(npc, true, mod.ProjectileType<AkumaABreath>(), 2, 2);
                }
                if (attackTimer >= 80)
                {
                    npc.ai[1] = 0;
                    attackTimer = 0;
                }
            }

            if (npc.alpha != 0)
            {
                for (int spawnDust = 0; spawnDust < 2; spawnDust++)
                {
                    int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, mod.DustType("AkumaADust"), 0f, 0f, 100, default, 2f);
                    Main.dust[num935].noGravity = true;
                    Main.dust[num935].noLight = true;
                }
            }
            npc.alpha -= 12;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }

            if (Main.netMode != 1)
            {
                if (npc.ai[0] == 0)
                {
                    npc.realLife = npc.whoAmI;
                    int latestNPC = npc.whoAmI;
                    int segment = 0;
                    int AkumaALength = 9;
                    for (int i = 0; i < AkumaALength; ++i)
                    {
                        if (segment == 0 || segment == 2 || segment == 3 || segment == 5 || segment == 6 || segment == 8)
                        {
                            latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("AkumaABody"), npc.whoAmI, 0, latestNPC);
                            Main.npc[latestNPC].realLife = npc.whoAmI;
                            Main.npc[latestNPC].ai[3] = npc.whoAmI;
                            Main.npc[latestNPC].netUpdate2 = true;
                            segment += 1;
                        }
                        if (segment == 1 || segment == 4 || segment == 7)
                        {
                            latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("AkumaAArms"), npc.whoAmI, 0, latestNPC);
                            Main.npc[latestNPC].realLife = npc.whoAmI;
                            Main.npc[latestNPC].ai[3] = npc.whoAmI;
                            Main.npc[latestNPC].netUpdate2 = true;
                            segment += 1;
                        }
                    }
                    latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("AkumaABody"), npc.whoAmI, 0, latestNPC);
                    Main.npc[latestNPC].realLife = npc.whoAmI;
                    Main.npc[latestNPC].ai[3] = npc.whoAmI;
                    Main.npc[latestNPC].netUpdate2 = true;

                    latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("AkumaABody1"), npc.whoAmI, 0, latestNPC);
                    Main.npc[latestNPC].realLife = npc.whoAmI;
                    Main.npc[latestNPC].ai[3] = npc.whoAmI;
                    Main.npc[latestNPC].netUpdate2 = true;

                    latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("AkumaATail"), npc.whoAmI, 0, latestNPC);
                    Main.npc[latestNPC].realLife = npc.whoAmI;
                    Main.npc[latestNPC].ai[3] = npc.whoAmI;
                    Main.npc[latestNPC].netUpdate2 = true;

                    npc.ai[0] = 1;
                    npc.netUpdate2 = true;
                }
            }
            bool collision = true;

            float speed = 15f;
            float acceleration = 0.13f;

            Vector2 npcCenter = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float targetXPos = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2);
            float targetYPos = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2);

            float targetRoundedPosX = (int)(targetXPos / 16.0) * 16;
            float targetRoundedPosY = (int)(targetYPos / 16.0) * 16;
            npcCenter.X = (int)(npcCenter.X / 16.0) * 16;
            npcCenter.Y = (int)(npcCenter.Y / 16.0) * 16;
            float dirX = targetRoundedPosX - npcCenter.X;
            float dirY = targetRoundedPosY - npcCenter.Y;
            npc.TargetClosest(true);
            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

            float absDirX = Math.Abs(dirX);
            float absDirY = Math.Abs(dirY);
            float newSpeed = speed / length;
            dirX *= newSpeed;
            dirY *= newSpeed;
            if (npc.velocity.X > 0.0 && dirX > 0.0 || npc.velocity.X < 0.0 && dirX < 0.0 || npc.velocity.Y > 0.0 && dirY > 0.0 || npc.velocity.Y < 0.0 && dirY < 0.0)
            {
                if (npc.velocity.X < dirX)
                    npc.velocity.X = npc.velocity.X + acceleration;
                else if (npc.velocity.X > dirX)
                    npc.velocity.X = npc.velocity.X - acceleration;
                if (npc.velocity.Y < dirY)
                    npc.velocity.Y = npc.velocity.Y + acceleration;
                else if (npc.velocity.Y > dirY)
                    npc.velocity.Y = npc.velocity.Y - acceleration;
                if (Math.Abs(dirY) < speed * 0.2 && (npc.velocity.X > 0.0 && dirX < 0.0 || npc.velocity.X < 0.0 && dirX > 0.0))
                {
                    if (npc.velocity.Y > 0.0)
                        npc.velocity.Y = npc.velocity.Y + acceleration * 2f;
                    else
                        npc.velocity.Y = npc.velocity.Y - acceleration * 2f;
                }
                if (Math.Abs(dirX) < speed * 0.2 && (npc.velocity.Y > 0.0 && dirY < 0.0 || npc.velocity.Y < 0.0 && dirY > 0.0))
                {
                    if (npc.velocity.X > 0.0)
                        npc.velocity.X = npc.velocity.X + acceleration * 2f;
                    else
                        npc.velocity.X = npc.velocity.X - acceleration * 2f;
                }
            }
            else if (absDirX > absDirY)
            {
                if (npc.velocity.X < dirX)
                    npc.velocity.X = npc.velocity.X + acceleration * 1.1f;
                else if (npc.velocity.X > dirX)
                    npc.velocity.X = npc.velocity.X - acceleration * 1.1f;

                if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
                {
                    if (npc.velocity.Y > 0.0)
                        npc.velocity.Y = npc.velocity.Y + acceleration;
                    else
                        npc.velocity.Y = npc.velocity.Y - acceleration;
                }
            }
            else
            {
                if (npc.velocity.Y < dirY)
                    npc.velocity.Y = npc.velocity.Y + acceleration * 1.1f;
                else if (npc.velocity.Y > dirY)
                    npc.velocity.Y = npc.velocity.Y - acceleration * 1.1f;

                if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
                {
                    if (npc.velocity.X > 0.0)
                        npc.velocity.X = npc.velocity.X + acceleration;
                    else
                        npc.velocity.X = npc.velocity.X - acceleration;
                }
            }

            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
            if (npc.velocity.X < 0f)
            {
                npc.spriteDirection = 1;

            }
            else
            {
                npc.spriteDirection = -1;
            }

            if (!Main.dayTime)
            {
                if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("AkumaA8"), Color.DeepSkyBlue);
                Main.dayTime = true;
                Main.time = 0;
            }

            if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 6000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 6000f)
            {
                if (Loludided == false)
                {
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("AkumaA9"), new Color(180, 41, 32));
                    Loludided = true;
                }
                npc.velocity.Y = npc.velocity.Y + 1f;
                if (npc.position.Y > Main.rockLayer * 16.0)
                {
                    npc.velocity.Y = npc.velocity.Y + 1f;
                }
                if (npc.position.Y > Main.rockLayer * 16.0)
                {
                    for (int num957 = 0; num957 < 200; num957++)
                    {
                        if (Main.npc[num957].aiStyle == npc.aiStyle)
                        {
                            Main.npc[num957].active = false;
                        }
                    }
                }
            }

            if (collision)
            {
                if (npc.localAI[0] != 1)
                    npc.netUpdate = true;
                npc.localAI[0] = 1f;
            }
            if ((npc.velocity.X > 0.0 && npc.oldVelocity.X < 0.0 || npc.velocity.X < 0.0 && npc.oldVelocity.X > 0.0 || npc.velocity.Y > 0.0 && npc.oldVelocity.Y < 0.0 || npc.velocity.Y < 0.0 && npc.oldVelocity.Y > 0.0) && !npc.justHit)
                npc.netUpdate = true;

            return false;
        }

        public override void NPCLoot()
        {
            if (Main.expertMode)
            {
                if (!AAWorld.downedAkuma)
                {
                    Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, mod.ItemType("DraconianRune"));
                }
                if (Main.netMode != 1) BaseUtility.Chat(AAWorld.downedAkuma ? Lang.BossChat("AkumaA10") : Lang.BossChat("AkumaA11"), Color.DeepSkyBlue.R, Color.DeepSkyBlue.G, Color.DeepSkyBlue.B);
                AAWorld.downedAkuma = true;
                if (Main.rand.Next(50) == 0 && AAWorld.downedAllAncients)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PowerStone"));
                }
                if (Main.rand.Next(10) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AkumaATrophy"));
                }
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AkumaAMask"));
                }
                if (AAWorld.downedShen)
                {
                    Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, mod.ItemType("EXSoul"));
                }
                npc.DropBossBags();
                return;
            }
            if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("AkumaA12"), Color.DeepSkyBlue.R, Color.DeepSkyBlue.G, Color.DeepSkyBlue.B);
            return;
        }


        public bool Quote1;
        public bool Quote2;
        public bool Quote3;
        public bool Quote4;
        public bool Quote5;
        public bool QuoteSaid;

        public void Attack(NPC npc)
        {
            Player player = Main.player[npc.target];
            if (internalAI[1] == 1 || internalAI[1] == 7 || internalAI[1] == 15 || internalAI[1] == 18 || internalAI[1] == 21)
            {
                if (!QuoteSaid)
                {
                    if (Main.netMode != 1) BaseUtility.Chat((!Quote1) ? Lang.BossChat("AkumaA13") : Lang.BossChat("AkumaA14"), Color.DeepSkyBlue);
                    QuoteSaid = true;
                    Quote1 = true;
                }
                if (npc.ai[2] == 320 || npc.ai[2] == 340 || npc.ai[2] == 360 || npc.ai[2] == 380)
                {
                    int Fireballs = Main.expertMode ? 20 : 14;
                    for (int Loops = 0; Loops < Fireballs; Loops++)
                    {
                        AkumaAttacks.Dragonfire(npc, mod, true);
                    }
                }
            }

            if (internalAI[1] == 2 || internalAI[1] == 6 || internalAI[1] == 12 || internalAI[1] == 16 || internalAI[1] == 24)
            {
                if (!QuoteSaid)
                {
                    if (Main.netMode != 1) BaseUtility.Chat((!Quote1) ? Lang.BossChat("AkumaA15") : Lang.BossChat("AkumaA16"), Color.DeepSkyBlue);
                    QuoteSaid = true;
                    Quote1 = true;
                }
                if (npc.ai[2] == 350)
                {
                    int Fireballs = Main.expertMode ? 5 : 3;
                    float spread = 45f * 0.0174f;
                    float baseSpeed = (float)Math.Sqrt((npc.velocity.X * npc.velocity.X) + (npc.velocity.Y * npc.velocity.Y));
                    double startAngle = Math.Atan2(npc.velocity.X, npc.velocity.Y) - .1d;
                    double deltaAngle = spread / 6f;
                    double offsetAngle;
                    for (int i = 0; i < Fireballs; i++)
                    {
                        offsetAngle = startAngle + (deltaAngle * i);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle) * 2, baseSpeed * (float)Math.Cos(offsetAngle) * 2, mod.ProjectileType<AkumaABomb>(), damage, 3, Main.myPlayer);
                    }
                }
            }

            if (internalAI[1] == 3 || internalAI[1] == 8 || internalAI[1] == 11 || internalAI[1] == 17 || internalAI[1] == 23)
            {
                int Fireballs = Main.expertMode ? 20 : 15;
                if (!QuoteSaid)
                {
                    if (Main.netMode != 1) BaseUtility.Chat((!Quote1) ? Lang.BossChat("AkumaA17") : Lang.BossChat("AkumaA18"), Color.DeepSkyBlue);
                    QuoteSaid = true;
                    Quote1 = true;
                }
                if (npc.ai[2] == 330 || npc.ai[2] == 360 || npc.ai[2] == 390)
                {
                    for (int Loops = 0; Loops < Fireballs; Loops++)
                    {
                        AkumaAttacks.Eruption(npc, mod);
                    }
                }
            }

            if (internalAI[1] == 4 || internalAI[1] == 10 || internalAI[1] == 13 || internalAI[1] == 20 || internalAI[1] == 25)
            {
                if (npc.ai[2] == 350)
                {
                    if (NPC.CountNPCS(mod.NPCType<AncientLung>()) < (Main.expertMode ? 3 : 4))
                    {
                        AkumaAttacks.SpawnLung(player, mod, true);
                        MinionCount += 1;
                    }
                }
            }

            if (internalAI[1] == 5 || internalAI[1] == 9 || internalAI[1] == 14 || internalAI[1] == 19 || internalAI[1] == 22)
            {
                if (!QuoteSaid)
                {
                    if (Main.netMode != 1) BaseUtility.Chat((!Quote1) ? Lang.BossChat("AkumaA19") : Lang.BossChat("AkumaA20"), Color.DeepSkyBlue);
                    QuoteSaid = true;
                    Quote1 = true;
                }
                if (npc.ai[2] == 350)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, npc.velocity.X * 2, npc.velocity.Y, mod.ProjectileType<AFireProjHostile>(), damage, 3, Main.myPlayer);
                }
            }

            if (internalAI[1] > 25)
            {
                internalAI[1] = 0;
            }
        }


        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.penetrate > 1)
            {
                damage = (int)(damage * .5f);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            AkumaTex = Main.npcTexture[npc.type];
            if (npc.type == mod.NPCType<AkumaA>())
            {
                if (npc.ai[1] == 1 || npc.ai[2] >= 400)
                {
                    AkumaTex = mod.GetTexture("NPCs/Bosses/Akuma/Awakened/AkumaA1");
                }
                else
                {
                    AkumaTex = mod.GetTexture("NPCs/Bosses/Akuma/Awakened/AkumaA");
                }
            }

            Texture2D glowTex = mod.GetTexture("Glowmasks/AkumaA_Glow");
            Texture2D glowTex1 = mod.GetTexture("Glowmasks/AkumaA1_Glow");
            Texture2D glowTex2 = mod.GetTexture("Glowmasks/AkumaAArms_Glow");
            Texture2D glowTex3 = mod.GetTexture("Glowmasks/AkumaABody_Glow");
            Texture2D glowTex4 = mod.GetTexture("Glowmasks/AkumaABody1_Glow");
            Texture2D glowTex5 = mod.GetTexture("Glowmasks/AkumaATail_Glow");
            
            int shader;
            if (npc.ai[1] == 1 || npc.ai[2] >= 470 || Main.npc[(int)npc.ai[3]].ai[1] == 1 || Main.npc[(int)npc.ai[3]].ai[2] >= 500)
            {
                shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
            }
            else
            {
                shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingOceanDye);
            }

            Texture2D HeadGlow = (npc.ai[1] == 1 || npc.ai[2] >= 500) ? glowTex1 : glowTex;

            Texture2D myGlowTex = npc.type == mod.NPCType<AkumaA>() ? HeadGlow : npc.type == mod.NPCType<AkumaAArms>() ? glowTex2 : npc.type == mod.NPCType<AkumaABody>() ? glowTex3 : npc.type == mod.NPCType<AkumaABody1>() ? glowTex4 : glowTex5;
            BaseDrawing.DrawTexture(spriteBatch, AkumaTex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 3, npc.frame, npc.GetAlpha(drawColor), true);
            BaseDrawing.DrawTexture(spriteBatch, myGlowTex, shader, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 3, npc.frame, npc.GetAlpha(Color.White), true);
            return false;
        }


        public override void HitEffect(int hitDirection, double damage)
        {
            int dust1 = mod.DustType<Dusts.AkumaADust>();
            int dust2 = mod.DustType<Dusts.AkumaDust>();
            if (npc.life <= 0)
            {
                Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, dust1, 0f, 0f, 0);
                Main.dust[dust1].velocity *= 0.5f;
                Main.dust[dust1].scale *= 1.3f;
                Main.dust[dust1].fadeIn = 1f;
                Main.dust[dust1].noGravity = false;
                Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, dust2, 0f, 0f, 0);
                Main.dust[dust2].velocity *= 0.5f;
                Main.dust[dust2].scale *= 1.3f;
                Main.dust[dust2].fadeIn = 1f;
                Main.dust[dust2].noGravity = true;

            }
        }

        public bool spawnAshe = false;


        public int roarTimer = 0;
        public int roarTimerMax = 120;
        public bool Roaring
        {
            get
            {
                return roarTimer > 0;
            }
        }

        public void Roar(int timer, bool fireSound)
        {
            roarTimer = timer;
            if (fireSound)
            {
                Main.PlaySound(4, (int)npc.Center.X, (int)npc.Center.Y, 60);
            }
            else
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/AkumaRoar"), npc.Center);
            }
        }

        public override void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
        {
            spriteEffects = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        public override void BossHeadRotation(ref float rotation)
        {
            rotation = npc.rotation;
        }

        public override bool CheckActive()
        {
            if (NPC.AnyNPCs(mod.NPCType<AkumaA>()))
            {
                return false;
            }
            return true;
        }
    }

    [AutoloadBossHead]
    public class AkumaAArms : AkumaA
    {
        public override string Texture => "AAMod/NPCs/Bosses/Akuma/Awakened/AkumaAArms";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Akuma Awakened; Blazing Fury Incarnate");
            Main.npcFrameCount[npc.type] = 1;
            NPCID.Sets.TechnicallyABoss[npc.type] = true;
        }

        public override bool PreAI()
        {
            Vector2 chasePosition = Main.npc[(int)npc.ai[1]].Center;
            Vector2 directionVector = chasePosition - npc.Center;
            npc.spriteDirection = (directionVector.X > 0f) ? 1 : -1;
            if (npc.ai[3] > 0)
                npc.realLife = (int)npc.ai[3];
            if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
                npc.TargetClosest(true);
            if (Main.player[npc.target].dead && npc.timeLeft > 300)
                npc.timeLeft = 300;
            if (npc.alpha != 0)
            {
                for (int spawnDust = 0; spawnDust < 2; spawnDust++)
                {
                    int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, mod.DustType("AkumaADust"), 0f, 0f, 100, default, 2f);
                    Main.dust[num935].noGravity = true;
                    Main.dust[num935].noLight = true;
                }
            }
            npc.alpha -= 12;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }


            if (Main.netMode != 1)
            {
                if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[3]].type != mod.NPCType("AkumaA"))
                {
                    npc.life = 0;
                    npc.HitEffect(0, 10.0);
                    npc.active = false;
                    NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
                }
            }

            if (npc.ai[1] < (double)Main.npc.Length)
            {
                Vector2 npcCenter = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float dirX = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - npcCenter.X;
                float dirY = Main.npc[(int)npc.ai[1]].position.Y + Main.npc[(int)npc.ai[1]].height / 2 - npcCenter.Y;
                npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float dist = (length - npc.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;

                if (dirX < 0f)
                {
                    npc.spriteDirection = 1;

                }
                else
                {
                    npc.spriteDirection = -1;
                }

                npc.velocity = Vector2.Zero;
                npc.position.X = npc.position.X + posX;
                npc.position.Y = npc.position.Y + posY;
            }

            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest(true);
            }
            npc.netUpdate = true;
            return false;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.boss = false;
            npc.width = 80;
            npc.height = 80;
            npc.dontCountMe = true;
            npc.chaseable = false;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            damage *= .05f;
            return true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override bool PreNPCLoot()
        {
            return false;
        }

        public override bool CheckActive()
        {
            if (NPC.AnyNPCs(mod.NPCType<AkumaA>()))
            {
                return false;
            }
            npc.active = false;
            return true;
        }
    }

    [AutoloadBossHead]
    public class AkumaABody : AkumaAArms
    {
        public override string Texture => "AAMod/NPCs/Bosses/Akuma/Awakened/AkumaABody";
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPCID.Sets.TechnicallyABoss[npc.type] = true;
        }
    }

    [AutoloadBossHead]
    public class AkumaABody1 : AkumaAArms
    {
        public override string Texture => "AAMod/NPCs/Bosses/Akuma/Awakened/AkumaABody1";
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPCID.Sets.TechnicallyABoss[npc.type] = true;
        }
    }

    [AutoloadBossHead]
    public class AkumaATail : AkumaAArms
    {
        public override string Texture => "AAMod/NPCs/Bosses/Akuma/Awakened/AkumaATail";
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPCID.Sets.TechnicallyABoss[npc.type] = true;
        }
    }
}
