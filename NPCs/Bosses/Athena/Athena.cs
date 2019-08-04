using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BaseMod;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using AAMod.NPCs.Enemies.Sky;
using Terraria.World.Generation;
using System.Collections.Generic;

namespace AAMod.NPCs.Bosses.Athena
{
    [AutoloadBossHead]
    public class Athena : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 7;
        }

        public static Point CloudPoint = new Point((int)(Main.maxTilesX * 0.65f), 100);
        public Vector2 Origin = new Vector2((int)(Main.maxTilesX * 0.65f), 100) * 16;
        public int damage = 0;

        public override void SetDefaults()
        {
            npc.width = 152;
            npc.height = 114;
            npc.value = BaseUtility.CalcValue(0, 10, 0, 0);
            npc.npcSlots = 1000;
            npc.aiStyle = -1;
            npc.lifeMax = 40000;
            npc.defense = 20;
            npc.damage = 90;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.boss = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Athena");
            npc.alpha = 255;
            npc.noTileCollide = true;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public float[] internalAI = new float[4];
        public float[] FlyAI = new float[2];

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == 2 || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
                writer.Write(FlyAI[0]);
                writer.Write(FlyAI[1]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == 1)
            {
                internalAI[0] = reader.ReadFloat();
                internalAI[1] = reader.ReadFloat();
                internalAI[2] = reader.ReadFloat();
                internalAI[3] = reader.ReadFloat();
                FlyAI[0] = reader.ReadFloat();
                FlyAI[1] = reader.ReadFloat();
            }
        }
        public Vector2 MoveVector2;
        public override void AI()
        {
            if (Main.expertMode)
            {
                damage = npc.damage / 4;
            }
            else
            {
                damage = npc.damage / 2;
            }
            npc.TargetClosest();
            Player player = Main.player[npc.target];
            AAPlayer modPlayer = player.GetModPlayer<AAPlayer>(mod);

            Vector2 Acropolis = new Vector2(Origin.X + (76 * 16), Origin.Y + (72 * 16));

            //Preamble Shite 
            if (internalAI[2] != 1)
            {
                npc.dontTakeDamage = true;
                if (Main.netMode != 1)
                {
                    music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
                    if (Vector2.Distance(npc.Center, Acropolis) < 10)
                    {
                        npc.velocity *= 0;
                        if (internalAI[3]++ < 420)
                        {
                            if (!AAWorld.downedAthena)
                            {
                                if (internalAI[3] == 60)
                                {
                                    if (Main.netMode != 1) BaseUtility.Chat("Hmpf..!", Color.CornflowerBlue);
                                }

                                if (internalAI[3] == 180)
                                {
                                    string s = "";
                                    if (Main.ActivePlayersCount > 1)
                                    {
                                        s = "s";
                                    }
                                    if (Main.netMode != 1) BaseUtility.Chat("You! Earthwalker" + s + "!", Color.CornflowerBlue);
                                }

                                if (internalAI[3] == 300)
                                {
                                    if (Main.netMode != 1) BaseUtility.Chat("My seraphs tell me you've been attacking them! Why?!", Color.CornflowerBlue);
                                }

                                if (internalAI[3] == 420)
                                {
                                    if (Main.netMode != 1) BaseUtility.Chat("I'm gonna teach you a lesson, you little brat!", Color.CornflowerBlue);
                                }

                                if (internalAI[3] >= 420)
                                {
                                    if (Main.netMode != 1) BaseUtility.Chat("En Garde!", Color.CornflowerBlue);
                                    internalAI[2] = 1;
                                    npc.netUpdate = true;
                                }
                            }
                            else
                            {
                                if (internalAI[3] == 60)
                                {
                                    if (Main.netMode != 1) BaseUtility.Chat("Sigh...", Color.CornflowerBlue);
                                }

                                if (internalAI[3] >= 180)
                                {
                                    if (Main.netMode != 1) BaseUtility.Chat("Lets just get this overwith. I don't have all day.", Color.CornflowerBlue);
                                    internalAI[2] = 1;
                                    npc.netUpdate = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        MoveToVector2(Acropolis);
                    }
                }
            }
            else
            {
                npc.dontTakeDamage = false;
                if (player.dead || !player.active || Vector2.Distance(npc.position, player.position) > 5000 || !modPlayer.ZoneAcropolis)
                {
                    npc.TargetClosest();
                    if (player.dead || !player.active || Math.Abs(Vector2.Distance(npc.position, player.position)) > 5000 || !modPlayer.ZoneAcropolis)
                    {
                        Main.NewText(Math.Abs(Vector2.Distance(npc.position, player.position)));
                        if (Main.netMode != 1) BaseUtility.Chat("And stay away...idiot.", Color.CornflowerBlue);
                        int p = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType<AthenaFlee>());
                        Main.npc[p].Center = npc.Center;
                        npc.active = false;
                        npc.netUpdate = true;
                    }
                }

                music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Athena");

                if (internalAI[2]++ > 300 && Main.netMode != 1)
                {
                    int pChoice = Main.rand.Next(2);
                    if (pChoice == 0)
                    {
                        NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType<OwlRune>());
                    }
                }

                if (internalAI[1] == 0) //Acropolis Phase
                {
                    if (Main.netMode != 1)
                    {
                        npc.ai[3]++;
                    }

                    if (Vector2.Distance(player.Center, Acropolis) > 480)
                    {
                        if (npc.ai[2] == 0 && Main.netMode != 1)
                        {
                            npc.ai[2] = 1;
                            npc.netUpdate = true;
                        }
                        MoveToVector2(Acropolis);
                    }
                    else
                    {
                        if (npc.ai[2] == 1 && Main.netMode != 1)
                        {
                            npc.ai[2] = 0;
                            npc.netUpdate = true;
                        }
                        BaseAI.AISpaceOctopus(npc, ref FlyAI, Main.player[npc.target].Center, 0.1f, 8f, 220f, 70f, ShootFeather);
                    }

                    if (npc.ai[3] > 600)
                    {
                        internalAI[1] = 1;
                        npc.ai[0] = 0;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        MoveVector2 = CloudPick();
                    }
                }
                else //Cloud Phase
                {
                    if (Main.netMode != 1)
                    {
                        npc.ai[1]++;
                        if (npc.ai[1] == 300)
                        {
                            if (Main.rand.Next(5) == 0)
                            {
                                internalAI[1] = 0;
                                npc.ai[0] = 0;
                                npc.ai[1] = 0;
                                npc.ai[2] = 0;
                                npc.ai[3] = 0;
                                npc.netUpdate = true;
                                return;
                            }
                            npc.ai[0] = 0;
                            MoveVector2 = CloudPick();
                            npc.netUpdate = true;
                        }
                    }
                    if(Vector2.Distance(npc.Center, MoveVector2) < 10)
                    {
                        if (npc.ai[2] == 1 && Main.netMode != 1)
                        {
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.netUpdate = true;
                        }
                        npc.velocity *= 0;

                        if (npc.ai[1] % 200 == 0 && Main.netMode != 1)
                        {
                            int Choice = Main.rand.Next(2);
                            if (Choice == 0)
                            {
                                NPC.NewNPC((int)npc.Center.X + 100, (int)npc.Center.Y, mod.NPCType<OlympianDragon>());
                                NPC.NewNPC((int)npc.Center.X - 100, (int)npc.Center.Y, mod.NPCType<OlympianDragon>());
                            }
                            else
                            {
                                NPC Seraph1 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + 100, mod.NPCType<SeraphA>())];
                                for (int i = 0; i < 3; i++)
                                {
                                   Dust d = Main.dust[Dust.NewDust(Seraph1.position, Seraph1.height, Seraph1.width, mod.DustType<Feather>(), Main.rand.Next(-1, 2), 1, 0)];
                                }
                                NPC Seraph2 = Main.npc[NPC.NewNPC((int)npc.Center.X + 100, (int)npc.Center.Y - 50, mod.NPCType<SeraphA>())];
                                for (int i = 0; i < 3; i++)
                                {
                                    Dust d = Main.dust[Dust.NewDust(Seraph2.position, Seraph2.height, Seraph2.width, mod.DustType<Feather>(), Main.rand.Next(-1, 2), 1, 0)];
                                }
                                NPC Seraph3 = Main.npc[NPC.NewNPC((int)npc.Center.X + 100, (int)npc.Center.Y - 50, mod.NPCType<SeraphA>())];
                                for (int i = 0; i < 3; i++)
                                {
                                    Dust d = Main.dust[Dust.NewDust(Seraph3.position, Seraph3.height, Seraph3.width, mod.DustType<Feather>(), Main.rand.Next(-1, 2), 1, 0)];
                                }
                            }
                            npc.netUpdate = true;
                        }

                        if (npc.ai[1] % 60 == 0)
                        {
                            if (Vector2.Distance(player.Center, npc.Center) < 900)
                            {
                                ShootFeather(npc, npc.velocity);
                            }
                        }
                    }
                    else
                    {
                        if (npc.ai[2] == 0 && Main.netMode != 1)
                        {
                            npc.ai[2] = 1;
                            npc.netUpdate = true;
                        }
                        MoveToVector2(MoveVector2);
                    }
                }
            }

            if (player.Center.X < npc.Center.X)
            {
                npc.direction = -1;
            }
            else
            {
                npc.direction = 1;
            }

            npc.rotation = 0;
        }

        public Vector2 CloudPick()
        {
            int CloudChoice = Main.rand.Next(12);
            Vector2 Cloud1 = new Vector2(Origin.X + (73 * 16), Origin.Y + (8 * 16));
            Vector2 Cloud2 = new Vector2(Origin.X + (43 * 16), Origin.Y + (19 * 16));
            Vector2 Cloud3 = new Vector2(Origin.X + (25 * 16), Origin.Y + (39 * 16));
            Vector2 Cloud4 = new Vector2(Origin.X + (14 * 16), Origin.Y + (61 * 16));
            Vector2 Cloud5 = new Vector2(Origin.X + (20 * 16), Origin.Y + (93 * 16));
            Vector2 Cloud6 = new Vector2(Origin.X + (45 * 16), Origin.Y + (114 * 16));
            Vector2 Cloud7 = new Vector2(Origin.X + (73 * 16), Origin.Y + (122 * 16));
            Vector2 Cloud8 = new Vector2(Origin.X + (110 * 16), Origin.Y + (112 * 16));
            Vector2 Cloud9 = new Vector2(Origin.X + (128 * 16), Origin.Y + (92 * 16));
            Vector2 Cloud10 = new Vector2(Origin.X + (135 * 16), Origin.Y + (63 * 16));
            Vector2 Cloud11 = new Vector2(Origin.X + (122 * 16), Origin.Y + (38 * 16));
            Vector2 Cloud12 = new Vector2(Origin.X + (101 * 16), Origin.Y + (18 * 16));
            if (CloudChoice == 1)
            {
                return Cloud2;
            }
            else if (CloudChoice == 2)
            {
                return Cloud3;
            }
            else if (CloudChoice == 3)
            {
                return Cloud4;
            }
            else if (CloudChoice == 4)
            {
                return Cloud5;
            }
            else if (CloudChoice == 5)
            {
                return Cloud6;
            }
            else if (CloudChoice == 6)
            {
                return Cloud7;
            }
            else if (CloudChoice == 7)
            {
                return Cloud8;
            }
            else if (CloudChoice == 8)
            {
                return Cloud9;
            }
            else if (CloudChoice == 9)
            {
                return Cloud10;
            }
            else if (CloudChoice == 10)
            {
                return Cloud11;
            }
            else if (CloudChoice == 11)
            {
                return Cloud12;
            }
            else
            {
                return Cloud1;
            }

        }

        public void ShootFeather(NPC npc, Vector2 velocity)
        {
            Player player = Main.player[npc.target];
            int projType = mod.ProjectileType<SeraphFeather>();
            float spread = 30f * 0.0174f;
            Vector2 dir = Vector2.Normalize(player.Center - npc.Center);
            dir *= 14f;
            float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
            double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
            double deltaAngle = spread / 6f;
            for (int i = 0; i < 3; i++)
            {
                double offsetAngle = startAngle + (deltaAngle * i);
                int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), projType, damage, 2, Main.myPlayer);
                Main.projectile[p].tileCollide = false;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter >= 6)
            {
                npc.frame.Y += frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * 7)
            {
                npc.frame.Y = 0;
            }
        }

        public void MoveToVector2(Vector2 p)
        {
            float moveSpeed = 25f;
            if (internalAI[2] != 1)
            {
                moveSpeed = 14f;
            }
            float velMultiplier = 1f;
            Vector2 dist = p - npc.Center;
            float length = dist == Vector2.Zero ? 0f : dist.Length();
            if (length < moveSpeed)
            {
                velMultiplier = MathHelper.Lerp(0f, 1f, length / moveSpeed);
            }
            if (length < 200f)
            {
                moveSpeed *= 0.5f;
            }
            if (length < 100f)
            {
                moveSpeed *= 0.5f;
            }
            if (length < 50f)
            {
                moveSpeed *= 0.5f;
            }
            npc.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            npc.velocity *= moveSpeed;
            npc.velocity *= velMultiplier;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void NPCLoot()
        {
            if (NPC.downedMoonlord)
            {
                if (!AAWorld.downedAthenaA)
                {
                    int a = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType<AthenaDefeat>());
                    Main.npc[a].Center = npc.Center;
                }
                else
                {
                    int a = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType<AthenaA>());
                    Main.npc[a].Center = npc.Center;

                    int b = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("ShockwaveBoom"), 0, 1, Main.myPlayer, 0, 0);
                    Main.projectile[b].Center = npc.Center;

                    if (Main.netMode != 1) BaseUtility.Chat("No more kidding around, the storms are calling, and they're coming for you!", Color.Cyan);

                    Main.projectile[b].netUpdate = true;
                }
                return;
            }
            if (Main.netMode != 1) BaseUtility.Chat("OW! Fine, fine..! I'll leave you alone! Geez, you don't let up, do you.", Color.CornflowerBlue);
            int p = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType<AthenaFlee>());
            Main.npc[p].Center = npc.Center;
            AAWorld.downedAthena = true;
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D tex = internalAI[2] != 1 ? mod.GetTexture("NPCs/Bosses/Athena/SassyBitch") : Main.npcTexture[npc.type];
            Color lightColor = BaseDrawing.GetLightColor(npc.Center);

            if (npc.ai[2] == 1)
            {
                BaseDrawing.DrawAfterimage(sb, tex, 0, npc.position, npc.width, npc.height, npc.oldPos, npc.scale, npc.rotation, npc.direction, 7, npc.frame, 1f, 1f, 5, false, 0f, 0f, Color.CornflowerBlue);
            }
            BaseDrawing.DrawTexture(sb, tex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 7, npc.frame, lightColor);
            return false;
        }
    }

    public class AthenaFlee : ModNPC
    {
        public override string Texture => "AAMod/NPCs/Bosses/Athena/Athena";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Athena");
            Main.npcFrameCount[npc.type] = 7;
        }
        public override void SetDefaults()
        {
            npc.width = 152;
            npc.height = 114;
            npc.npcSlots = 1000;
            npc.aiStyle = -1;
            npc.defense = 1;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.noTileCollide = true;
            npc.damage = 0;
            npc.value = 0;
        }

        public override void AI()
        {
            if (Main.netMode != 1 && npc.ai[0]++ >= 120)
            {
                if (npc.ai[0] == 120)
                {
                    npc.velocity.Y = .4f;
                    npc.netUpdate = true;
                }
                else if (npc.ai[0] == 130)
                {
                    npc.velocity.Y = -3f;
                    npc.netUpdate = true;
                }
                else if (npc.ai[0] > 130)
                {
                    npc.velocity.Y = -3f;
                }
                if (npc.position.Y + npc.velocity.Y <= 0f && Main.netMode != 1) { BaseAI.KillNPC(npc); npc.netUpdate = true; }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter >= 6)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * Main.npcFrameCount[npc.type])
            {
                npc.frame.Y = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            BaseDrawing.DrawAfterimage(spriteBatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.oldPos, npc.scale, npc.rotation, npc.direction, 7, npc.frame, 1f, 1f, 5, false, 0f, 0f, Color.CornflowerBlue);
            BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 7, npc.frame, npc.GetAlpha(lightColor), false);
            return false;
        }
    }
}