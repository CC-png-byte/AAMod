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

        public override void SetDefaults()
        {
            npc.width = 152;
            npc.height = 114;
            npc.value = BaseUtility.CalcValue(0, 10, 0, 0);
            npc.npcSlots = 1000;
            npc.aiStyle = -1;
            npc.lifeMax = 40000;
            npc.defense = 20;
            npc.damage = 60;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.boss = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Athena");
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

        private Vector2 MoveVector2;

        public override void AI()
        {
            npc.TargetClosest();
            Player player = Main.player[npc.target];
            AAPlayer modPlayer = player.GetModPlayer<AAPlayer>(mod);

            Vector2 Acropolis = new Vector2(Origin.X + (76 * 16), Origin.Y + (72 * 16));
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

            Vector2[] Cloud = new Vector2[]
            {
                Cloud1, Cloud2, Cloud3, Cloud4, Cloud5, Cloud6, Cloud7, Cloud8, Cloud9, Cloud10, Cloud11, Cloud12
            };

            //Preamble Shite 
            if (internalAI[2] != 1) 
            {
                npc.Center = Acropolis;
                if (Main.netMode != 1)
                {
                    if (!AAWorld.downedAthena)
                    {
                        music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
                        if (internalAI[3]++ < 420)
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
                                CloudSet Clouds = new CloudSet();
                                Clouds.Place(CloudPoint, WorldGen.structures);
                                internalAI[2] = 1;
                                npc.netUpdate = true;
                            }
                        }
                    }
                    else
                    {
                        if (Main.netMode != 1) BaseUtility.Chat("Hmpf...fine, let's get this overwith. I don't have all day.", Color.CornflowerBlue);
                        CloudSet Clouds = new CloudSet();
                        Clouds.Place(CloudPoint, WorldGen.structures);
                        internalAI[2] = 1;
                        npc.netUpdate = true;
                    }
                }
                
            }
            else
            {
                if (player.dead || !player.active || Vector2.Distance(npc.position, player.position) > 5000 || !modPlayer.ZoneAcropolis)
                {
                    npc.TargetClosest();
                    if (player.dead || !player.active || Math.Abs(Vector2.Distance(npc.position, player.position)) > 5000 || !modPlayer.ZoneAcropolis)
                    {
                        Main.NewText(Math.Abs(Vector2.Distance(npc.position, player.position)));
                        if (Main.netMode != 1) BaseUtility.Chat("And stay away...idiot.", Color.CornflowerBlue);
                        int p = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType<AthenaFlee>());
                        Main.npc[p].Center = npc.Center;
                        CloudKill Clouds = new CloudKill();
                        Clouds.Place(CloudPoint, WorldGen.structures);
                        npc.active = false;
                        npc.netUpdate = true;
                    }
                }
                music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Athena");

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
                        MoveVector2 = Cloud[Main.rand.Next(12)];
                    }
                }
                else //Cloud Phase
                {
                    if (Main.netMode != 1)
                    {
                        npc.ai[1]++;

                        if (npc.ai[0] >= 600)
                        {
                            npc.ai[0] = 0;
                            if (Main.rand.Next(5) == 0)
                            {
                                internalAI[1] = 0;
                                npc.ai[1] = 0;
                                npc.ai[2] = 0;
                                npc.ai[3] = 0;
                                npc.netUpdate = true;
                                return;
                            }
                            MoveVector2 = Cloud[Main.rand.Next(12)];
                            npc.netUpdate = true;
                        }
                    }
                    if(Vector2.Distance(npc.Center, MoveVector2) < 10 && Main.netMode != 1)
                    {
                        if (npc.ai[2] == 1 && Main.netMode != 1)
                        {
                            npc.ai[2] = 0;
                            npc.netUpdate = true;
                        }
                        npc.velocity *= 0;

                        if (npc.ai[1] % 90 == 0)
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
            if (npc.ai[2] == 1)
            {
                npc.noTileCollide = true;
            }
            else
            {
                npc.noTileCollide = false;
            }
            npc.rotation = 0;
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
                int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), projType, npc.damage / 4, 2, Main.myPlayer);
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
            float moveSpeed = 30f;
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
            CloudKill Clouds = new CloudKill();
            Clouds.Place(CloudPoint, WorldGen.structures);
            if (Main.netMode != 1) BaseUtility.Chat("OW! Fine, fine..! I'll leave you alone! Geez, you don't let up, do you.", Color.CornflowerBlue);
            int p = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType<AthenaFlee>());
            Main.npc[p].Center = npc.Center;
            AAWorld.downedAthena = true;
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D tex = Main.npcTexture[npc.type];
            Color lightColor = BaseDrawing.GetLightColor(npc.Center);

            if (npc.ai[2] == 1)
            {
                BaseDrawing.DrawAfterimage(sb, tex, 0, npc.position, npc.width, npc.height, npc.oldPos, npc.scale, npc.rotation, npc.spriteDirection, 7, npc.frame, 1f, 1f, 5, false, 0f, 0f, Color.CornflowerBlue);
            }
            BaseDrawing.DrawTexture(sb, tex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 7, npc.frame, lightColor);
            return false;
        }

        public class CloudSet : MicroBiome
        {
            public override bool Place(Point origin, StructureMap structures)
            {
                Mod mod = AAMod.instance;

                Dictionary<Color, int> colorToTile = new Dictionary<Color, int>
                {
                    [new Color(255, 255, 0)] = mod.TileType("AcropolisClouds"),
                    [Color.Black] = -1 //don't touch when genning		
                };

                TexGen gen = BaseWorldGenTex.GetTexGenerator(mod.GetTexture("Worldgeneration/AcropolisArena"), colorToTile);

                gen.Generate(origin.X, origin.Y, true, true);

                return true;
            }
        }

        public class CloudKill : MicroBiome
        {
            public override bool Place(Point origin, StructureMap structures)
            {
                Mod mod = AAMod.instance;

                Dictionary<Color, int> colorToTile = new Dictionary<Color, int>
                {
                    [new Color(255, 255, 0)] = -2,
                    [Color.Black] = -1 //don't touch when genning		
                };

                TexGen gen = BaseWorldGenTex.GetTexGenerator(mod.GetTexture("Worldgeneration/AcropolisArena"), colorToTile);

                gen.Generate(origin.X, origin.Y, true, true);

                return true;
            }
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
            npc.life = 1;
            npc.dontTakeDamage = true;
            npc.damage = 60;
            npc.width = 152;
            npc.height = 114;
            npc.friendly = false;
            npc.timeLeft = 900;
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
            BaseDrawing.DrawAfterimage(spriteBatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.oldPos, npc.scale, npc.rotation, npc.spriteDirection, 7, npc.frame, 1f, 1f, 5, false, 0f, 0f, Color.CornflowerBlue);
            BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 7, npc.frame, npc.GetAlpha(lightColor), false);
            return false;
        }
    }
}