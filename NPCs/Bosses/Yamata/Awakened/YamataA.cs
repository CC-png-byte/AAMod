using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using BaseMod;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AAMod.NPCs.Bosses.Yamata.Awakened
{
    [AutoloadBossHead]
    public class YamataA : YamataBoss
    {
        public NPC TrueHead;
        public NPC Head2;
        public NPC Head3;
        public NPC Head4;
        public NPC Head5;
        public NPC Head6;
        public NPC Head7;
        public bool HeadsSpawned = false;
        private bool threeQuarterHealth = false;
        private bool HalfHealth = false;
        private bool tenthHealth = false;
        public bool loludide = false;
        public bool flag;

        public float[] internalAI = new float[4];
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                internalAI[0] = reader.ReadFloat();
                internalAI[1] = reader.ReadFloat();
                internalAI[2] = reader.ReadFloat();
                internalAI[3] = reader.ReadFloat();
            }
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            displayName = "Yamata no Orochi";
            //Main.npcFrameCount[npc.type] = 7;
        }

        public override void SetDefaults()
        {
            npc.npcSlots = 100;
            npc.width = 80;
            npc.height = 90;
            npc.aiStyle = -1;
            npc.damage = 0;
            npc.lifeMax = 550000;
            npc.defense = 999999;
            npc.knockBackResist = 0f;
            npc.boss = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Yamata");
            npc.noGravity = true;
            npc.netAlways = true;
            frameWidth = 324;
            frameHeight = 236;
            npc.alpha = 255;
            npc.frame = BaseDrawing.GetFrame(0, frameWidth, frameHeight, 0, 2);
            frameBottom = BaseDrawing.GetFrame(frameCount, frameWidth, 54, 0, 2);
            frameHead = BaseDrawing.GetFrame(frameCount, frameWidth, 118, 0, 2);
            npc.DeathSound = mod.GetLegacySoundSlot(SoundType.NPCKilled, "Sounds/Sounds/YamataRoar");
            npc.chaseable = false;
            npc.value = Item.sellPrice(0, 40, 0, 0);
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Yamata2");
            bossBag = mod.ItemType("YamataBag");
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            musicPriority = MusicPriority.BossHigh;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
            npc.damage = (int)(npc.damage * .7f);
        }
        
        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            npc.damage = 0;
            return false;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            if (Main.expertMode)
            {
                potionType = ItemID.SuperHealingPotion;
            }
            else
            {
                if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA5"), new Color(146, 30, 68));
            }
            if (!AAWorld.downedYamata)
            {
                if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA2"), new Color(146, 30, 68));
                if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA3"), Color.Indigo);
            }
            else
            {
                if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA4"), new Color(146, 30, 68));
            }
        }

        public override void NPCLoot()
        {
            if (Main.expertMode)
            {
                if (!AAWorld.downedYamata)
                {
                    Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, mod.ItemType("DreadRune"));
                }
                if (Main.rand.Next(10) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("YamataATrophy"));
                }
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("YamataAMask"));
                }

                BaseAI.DropItem(npc, mod.ItemType("YamataATrophy"), 1, 1, 15, true);
                
                npc.DropBossBags();
                AAWorld.downedYamata = true;
                if (AAWorld.downedShen)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("EXSoul"));
                }
            }
        }


        public int playerTooFarDist = 800;
        public Rectangle frameBottom = new Rectangle(0, 0, 1, 1), frameHead = new Rectangle(0, 0, 1, 1);
        public bool prevHalfHPLeft = false, halfHPLeft = false, prevFourthHPLeft = false, fourthHPLeft = false;
        public Player playerTarget = null;
        public static int flyingTileCount = 6, totalMinionCount = 0;
        public int MinionTimer = 0;

        //clientside stuff
        public Vector2 bottomVisualOffset = default;
        public Vector2 topVisualOffset = default;
        public LegInfo[] legs = null;
        public bool[] headsSaidOw = new bool[7];
        public bool Tag = false;
        public bool TeleportMe1 = false;
        public bool TeleportMe2 = false;
        public bool TeleportMe3 = false;
        public bool TeleportMe4 = false;
        public bool TeleportMe5 = false;
        public bool TeleportMe6 = false;
        public static bool TeleportMeBitch = false;

        public int SayTheLineYamata = 300;
        public bool FirstLine = false;
        public bool NoFly4U = false;
        public int NoFlyCountDown = 60;

        public void HandleHeads()
        {
            if (Main.netMode != 1)
            {
                if (!HeadsSpawned)
                {
                    const int headX = 300;
                    const int headY = -500;

                    TrueHead = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("YamataAHead"), 0)];
                    TrueHead.ai[0] = npc.whoAmI;
                    TrueHead.ai[1] = 0;
                    TrueHead.ai[2] = headY;
                    Head2 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("YamataAHeadF"), 0)];
                    Head2.ai[0] = npc.whoAmI;
                    Head2.ai[1] = headX * -3f;
                    Head2.ai[2] = headY * 0.7f;
                    Head2.ai[3] = 3f;
                    Head3 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("YamataAHeadF"), 0)];
                    Head3.ai[0] = npc.whoAmI;
                    Head3.ai[1] = headX * -2f;
                    Head3.ai[2] = headY * 0.8f;
                    Head3.ai[3] = 2f;
                    Head4 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("YamataAHeadF"), 0)];
                    Head4.ai[0] = npc.whoAmI;
                    Head4.ai[1] = headX * -1f;
                    Head4.ai[2] = headY * 0.9f;
                    Head4.ai[3] = 1f;
                    Head5 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("YamataAHeadF"), 0)];
                    Head5.ai[0] = npc.whoAmI;
                    Head5.ai[1] = headX * 1f;
                    Head5.ai[2] = headY * 0.9f;
                    Head5.ai[3] = 1f;
                    Head6 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("YamataAHeadF"), 0)];
                    Head6.ai[0] = npc.whoAmI;
                    Head6.ai[1] = headX * 2f;
                    Head6.ai[2] = headY * 0.8f;
                    Head6.ai[3] = 2f;
                    Head7 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("YamataAHeadF"), 0)];
                    Head7.ai[0] = npc.whoAmI;
                    Head7.ai[1] = headX * 3f;
                    Head7.ai[2] = headY * 0.7f;
                    Head7.ai[3] = 3f;

                    TrueHead.netUpdate = true;
                    Head2.netUpdate = true;
                    Head3.netUpdate = true;
                    Head4.netUpdate = true;
                    Head5.netUpdate = true;
                    Head6.netUpdate = true;
                    Head7.netUpdate = true;
                    HeadsSpawned = true;
                }
            }
            else
            {
                if (!HeadsSpawned)
                {
                    int[] npcs = BaseAI.GetNPCs(npc.Center, -1, default, 200f, null);
                    if (npcs != null && npcs.Length > 0)
                    {
                        foreach (int npcID in npcs)
                        {
                            NPC npc2 = Main.npc[npcID];
                            if (npc2 != null)
                            {
                                if (TrueHead == null && npc2.type == mod.NPCType("YamataAHead") && npc2.ai[0] == npc.whoAmI)
                                {
                                    TrueHead = npc2;
                                }
                                else
                                if (Head2 == null && npc2.type == mod.NPCType("YamataAHeadF") && npc2.ai[0] == npc.whoAmI)
                                {
                                    Head2 = npc2;
                                }
                                else
                                if (Head3 == null && npc2.type == mod.NPCType("YamataAHeadF") && npc2.ai[0] == npc.whoAmI)
                                {
                                    Head3 = npc2;
                                }
                                else
                                if (Head4 == null && npc2.type == mod.NPCType("YamataAHeadF") && npc2.ai[0] == npc.whoAmI)
                                {
                                    Head4 = npc2;
                                }
                                else
                                if (Head5 == null && npc2.type == mod.NPCType("YamataAHeadF") && npc2.ai[0] == npc.whoAmI)
                                {
                                    Head5 = npc2;
                                }
                                else
                                if (Head6 == null && npc2.type == mod.NPCType("YamataAHeadF") && npc2.ai[0] == npc.whoAmI)
                                {
                                    Head6 = npc2;
                                }
                                else
                                if (Head7 == null && npc2.type == mod.NPCType("YamataAHeadF") && npc2.ai[0] == npc.whoAmI)
                                {
                                    Head7 = npc2;
                                }
                            }
                        }
                    }
                    if (TrueHead != null && Head2 != null && Head3 != null && Head4 != null && Head5 != null && Head6 != null && Head7 != null)
                    {
                        HeadsSpawned = true;
                    }
                }
            }
        }

        public override void AI()
        {
            TargetClosest();
            HandleHeads();

            if (Tag)
            {
                npc.life = 0;
                npc.netUpdate = true;
            }
            if (SayTheLineYamata <= 0)
            {
                SayTheLineYamata = 300;
            }

            if (Main.dayTime)
            {
                if (Main.netMode != 1 && !flag)
                {
                    flag = true;
                    AAMod.Chat(Lang.BossChat("Yamata3"), new Color(146, 30, 68));
                }
                Main.dayTime = false;
                Main.time = 0;
                return;
            }

            prevHalfHPLeft = halfHPLeft;
            prevFourthHPLeft = fourthHPLeft;
            halfHPLeft = halfHPLeft || npc.life <= npc.lifeMax / 2;
            fourthHPLeft = fourthHPLeft || npc.life <= npc.lifeMax / 4;

            for (int m = npc.oldPos.Length - 1; m > 0; m--)
            {
                npc.oldPos[m] = npc.oldPos[m - 1];
            }
            npc.oldPos[0] = npc.position;

            bool foundTarget = TargetClosest();
            if (foundTarget)
            {
                NoFlyCountDown--;

                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player t = Main.player[p];
                    if (t.active && !t.dead)
                    {
                        Main.player[p].AddBuff(mod.BuffType<Buffs.YamataAGravity>(), 10, true);
                    }
                }

                float dist = npc.Distance(playerTarget.Center);
                if (dist > 1200 || !Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height)
                    || Main.player[npc.target].position.Y < npc.position.Y - 500)
                {
                    if (Main.netMode != 1 && SayTheLineYamata == 300)
                    {
                        if (!FirstLine)
                        {
                            if (Main.netMode != 1) AAMod.Chat(Lang.BossChat("Yamata7"), new Color(146, 30, 68));
                            FirstLine = true;
                        }
                    }
                    SayTheLineYamata--;
                    npc.alpha += 1;
                    if (npc.alpha >= 255)
                    {
                        npc.alpha = 255;
                        Vector2 tele = new Vector2(playerTarget.Center.X, playerTarget.Center.Y);
                        TeleportMe1 = true;
                        TeleportMe2 = true;
                        TeleportMe3 = true;
                        TeleportMe4 = true;
                        TeleportMe5 = true;
                        TeleportMe6 = true;
                        TeleportMeBitch = true;
                        npc.Center = tele;
                    }
                }
                else
                {
                    npc.alpha -= 8;
                    SayTheLineYamata = 300;
                    if (npc.alpha <= 0)
                    {
                        npc.alpha = 0;
                    }
                }
                npc.timeLeft = 300;
                float playerDistance = Vector2.Distance(playerTarget.Center, npc.Center);
                if ((playerDistance < playerTooFarDist - 100f) && Math.Abs(npc.velocity.X) > 12f) npc.velocity.X *= 0.8f;
                if ((playerDistance < playerTooFarDist - 100f) && Math.Abs(npc.velocity.Y) > 12f) npc.velocity.Y *= 0.8f;
                if (npc.velocity.Y > 7f) npc.velocity.Y *= 0.75f;
                AIMovementNormal();
            }
            else
            {
                AIMovementRunAway();
            }
            bottomVisualOffset = new Vector2(Math.Min(3f, Math.Abs(npc.velocity.X)), 0f) * (npc.velocity.X < 0 ? 1 : -1);
            UpdateLimbs();
        }

        public void AIMovementRunAway()
        {
            if ((Main.netMode != 1) && !loludide)
            {
                if (Main.netMode != 1) AAMod.Chat(Lang.BossChat("Yamata9"), new Color(146, 30, 68));
                loludide = true;
            }

            npc.alpha += 10;
            if (npc.alpha >= 255)
            {
                npc.active = false;
            }
        }

        public void AIMovementNormal(float playerDistance = -1f)
        {
            bool playerTooFar = playerDistance > playerTooFarDist;
            YamataBody(npc, ref npc.ai, true, 0.2f, 3.5f, 8f, 0.07f, 1.5f, 4);
            if (playerTooFar) npc.position += playerTarget.position - playerTarget.oldPosition;
            npc.rotation = 0f;
        }

        public void YamataBody(NPC npc, ref float[] ai, bool ignoreWet = true, float moveInterval = 0.2f, float maxSpeedX = 2f, float maxSpeedY = 1.5f, float hoverInterval = 0.04f, float hoverMaxSpeed = 1.5f, int hoverHeight = 3)
        {
            bool flyUpward = false;
            if (npc.justHit) { ai[2] = 0f; }
            if (ai[2] >= 0f)
            {
                int tileDist = 16;
                bool inRangeX = false;
                bool inRangeY = false;
                if (npc.position.X > ai[0] - tileDist && npc.position.X < ai[0] + tileDist) { inRangeX = true; }
                else
                    if ((npc.velocity.X < 0f && npc.direction > 0) || (npc.velocity.X > 0f && npc.direction < 0)) { inRangeX = true; }
                tileDist += 24;
                if (npc.position.Y > ai[1] - tileDist && npc.position.Y < ai[1] + tileDist)
                {
                    inRangeY = true;
                }
                if (inRangeX && inRangeY)
                {
                    ai[2] += 1f;
                    if (ai[2] >= 30f && tileDist == 16)
                    {
                        flyUpward = true;
                    }
                    if (ai[2] >= 60f)
                    {
                        ai[2] = -200f;
                        npc.direction *= -1;
                        npc.velocity.X *= -1f;
                        npc.collideX = false;
                    }
                }
                else
                {
                    ai[0] = npc.position.X;
                    ai[1] = npc.position.Y;
                    ai[2] = 0f;
                }
                npc.TargetClosest(true);
            }
            else
            {
                ai[2] += 1f;
                if (Main.player[npc.target].position.X + Main.player[npc.target].width / 2 > npc.position.X + npc.width / 2)
                {
                    npc.direction = -1;
                }
                else
                {
                    npc.direction = 1;
                }
            }

            int tileX = (int)(npc.Center.X / 16f) + npc.direction * 2;
            int tileY = (int)((npc.position.Y + npc.height) / 16f);
            bool tileBelowEmpty = true;

            for (int tY = tileY; tY < tileY + hoverHeight; tY++)
            {
                if (Main.tile[tileX, tY] == null)
                {
                    Main.tile[tileX, tY] = new Tile();
                }
                if ((Main.tile[tileX, tY].nactive() && Main.tileSolid[Main.tile[tileX, tY].type]) || Main.tile[tileX, tY].liquid > 0)
                {
                    tileBelowEmpty = false;
                    break;
                }
            }
            if (flyUpward)
            {
                tileBelowEmpty = true;
            }

            if (tileBelowEmpty)
            {
                npc.velocity.Y += moveInterval;
                if (npc.velocity.Y > 9f)
                {
                    npc.velocity.Y = 9f;
                }
            }
            else
            {
                if (npc.directionY < 0 && npc.velocity.Y > 0f) { npc.velocity.Y -= moveInterval; }
                if (npc.velocity.Y < -maxSpeedY) { npc.velocity.Y = -maxSpeedY; }
            }


            if (!ignoreWet && npc.wet)
            {
                npc.velocity.Y -= moveInterval;
                if (npc.velocity.Y < -maxSpeedY * 0.75f) { npc.velocity.Y = -maxSpeedY * 0.75f; }
            }


            if (npc.collideY)
            {
                npc.velocity.Y = npc.oldVelocity.Y * -0.25f;
                if (npc.velocity.Y > 0f && npc.velocity.Y < 1f) { npc.velocity.Y = 1f; }
                if (npc.velocity.Y < 0f && npc.velocity.Y > -1f) { npc.velocity.Y = -1f; }
            }

            if (!tileBelowEmpty && npc.target > -1 && Main.player[npc.target].active && !Main.player[npc.target].dead && Math.Abs(Main.player[npc.target].Center.X - npc.Center.X) < 50) //force a hover
            {
                if (Math.Abs(npc.velocity.X) > 0.3f) npc.velocity.X *= 0.9f;
                if (Math.Abs(npc.velocity.Y) > 0.3f) npc.velocity.Y *= 0.9f;
            }
            else
            if (npc.direction == -1 && npc.velocity.X > -maxSpeedX)
            {
                npc.velocity.X -= moveInterval * 0.5f;
                if (npc.velocity.X > maxSpeedX) { npc.velocity.X -= 0.1f; }
                else
                    if (npc.velocity.X > 0f) { npc.velocity.X += 0.05f; }
                if (npc.velocity.X < -maxSpeedX) { npc.velocity.X = -maxSpeedX; }
            }
            else
            if (npc.direction == 1 && npc.velocity.X < maxSpeedX)
            {
                npc.velocity.X += moveInterval * 0.5f;
                if (npc.velocity.X < -maxSpeedX) { npc.velocity.X += 0.1f; }
                else
                    if (npc.velocity.X < 0f) { npc.velocity.X -= 0.05f; }
                if (npc.velocity.X > maxSpeedX) { npc.velocity.X = maxSpeedX; }
            }


            if (npc.directionY == -1 && (double)npc.velocity.Y > -hoverMaxSpeed)
            {
                npc.velocity.Y -= hoverInterval;
                if ((double)npc.velocity.Y > hoverMaxSpeed) { npc.velocity.Y -= 0.05f; }
                else
                    if (npc.velocity.Y > 0f) { npc.velocity.Y += hoverInterval - 0.01f; }
                if ((double)npc.velocity.Y < -hoverMaxSpeed) { npc.velocity.Y = -hoverMaxSpeed; }
            }
            else
            if (npc.directionY == 1 && (double)npc.velocity.Y < hoverMaxSpeed)
            {
                npc.velocity.Y += hoverInterval;
                if ((double)npc.velocity.Y < -hoverMaxSpeed) { npc.velocity.Y += 0.05f; }
                else
                if (npc.velocity.Y < 0f) { npc.velocity.Y -= hoverInterval - 0.01f; }
                if ((double)npc.velocity.Y > hoverMaxSpeed) { npc.velocity.Y = hoverMaxSpeed; }
            }
        }

        public bool TargetClosest()
        {
            int[] players = BaseAI.GetPlayers(npc.Center, 4200f);
            float dist = 999999999f;
            int foundPlayer = -1;
            if (foundPlayer != -1)
            {
                BaseAI.SetTarget(npc, foundPlayer);
                playerTarget = Main.player[foundPlayer];
                return true;
            }
            else
            {
                for (int m = 0; m < players.Length; m++)
                {
                    Player p = Main.player[players[m]];
                    if (Vector2.Distance(p.Center, npc.Center) < dist)
                    {
                        dist = Vector2.Distance(p.Center, npc.Center);
                        foundPlayer = p.whoAmI;
                    }
                }
            }
            if (foundPlayer != -1)
            {
                BaseAI.SetTarget(npc, foundPlayer);
                playerTarget = Main.player[foundPlayer];
                return true;
            }
            return false;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale *= 2;
            return true;
        }

        public void UpdateLimbs()
        {
            if (legs == null || legs.Length < 4)
            {
                legs = new LegInfo[4];
                legs[0] = new LegInfo(0, npc.Bottom + new Vector2(60, 0), this);
                legs[1] = new LegInfo(1, npc.Bottom + new Vector2(-82, 0), this);
                legs[2] = new LegInfo(2, npc.Bottom + new Vector2(80, 0), this);
                legs[3] = new LegInfo(3, npc.Bottom + new Vector2(-102, 0), this);
            }
            for (int m = 0; m < 4; m++)
            {
                legs[m].UpdateLeg(npc);
            }
        }

        public Vector2 position, oldPosition;

        public void DrawHead(SpriteBatch spriteBatch, string headTexture, string glowMaskTexture, NPC head, Color drawColor, bool DrawUnder)
        {
            Color lightColor = npc.GetAlpha(BaseDrawing.GetLightColor(npc.Center));
            Color GlowColor = AAColor.COLOR_WHITEFADE1;
            if (head != null && head.active && head.modNPC != null && (head.modNPC is YamataAHead || head.modNPC is YamataAHeadF))
            {
                Texture2D neckTex2D = mod.GetTexture("NPCs/Bosses/Yamata/Awakened/YamataANeck");
                Vector2 connector = head.Center;
                Vector2 neckOrigin = new Vector2(npc.Center.X, npc.Center.Y - 110 * npc.scale);
                BaseDrawing.DrawChain(spriteBatch, new Texture2D[] { neckTex2D, neckTex2D, neckTex2D }, 0, neckOrigin, connector, neckTex2D.Height - 10f, drawColor, 1f, DrawUnder, null);
                BaseDrawing.DrawTexture(spriteBatch, mod.GetTexture(headTexture), 0, head.position, head.width, head.height, head.scale, head.rotation, head.spriteDirection, Main.npcFrameCount[head.type], head.frame, drawColor, false);
                BaseDrawing.DrawTexture(spriteBatch, mod.GetTexture(glowMaskTexture), 0, head.position, head.width, head.height, head.scale, head.rotation, head.spriteDirection, Main.npcFrameCount[head.type], head.frame, GlowColor, false);
            }
        }

        public override void PostDraw(SpriteBatch sb, Color dColor)
        {
            Color lightColor = npc.GetAlpha(BaseDrawing.GetLightColor(npc.Center));
            BaseDrawing.DrawTexture(sb, mod.GetTexture("NPCs/Bosses/Yamata/Awakened/YamataATail"), 0, npc.position + new Vector2(0f, npc.gfxOffY) + bottomVisualOffset + new Vector2(0, -32), npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, Main.npcFrameCount[npc.type], frameBottom, lightColor, false);
            if (legs != null && legs.Length == 4)
            {
                legs[2].DrawLeg(sb, npc); //back legs
                legs[3].DrawLeg(sb, npc);
                legs[0].DrawLeg(sb, npc); //front legs
                legs[1].DrawLeg(sb, npc);
            }

            DrawHead(sb, "NPCs/Bosses/Yamata/Awakened/YamataAHeadF", "Glowmasks/YamataAHeadF_Glow", Head2, dColor, false);
            DrawHead(sb, "NPCs/Bosses/Yamata/Awakened/YamataAHeadF", "Glowmasks/YamataAHeadF_Glow", Head3, dColor, false);
            DrawHead(sb, "NPCs/Bosses/Yamata/Awakened/YamataAHeadF", "Glowmasks/YamataAHeadF_Glow", Head4, dColor, false);
            DrawHead(sb, "NPCs/Bosses/Yamata/Awakened/YamataAHeadF", "Glowmasks/YamataAHeadF_Glow", Head5, dColor, false);
            DrawHead(sb, "NPCs/Bosses/Yamata/Awakened/YamataAHeadF", "Glowmasks/YamataAHeadF_Glow", Head6, dColor, false);
            DrawHead(sb, "NPCs/Bosses/Yamata/Awakened/YamataAHeadF", "Glowmasks/YamataAHeadF_Glow", Head7, dColor, false);

            BaseDrawing.DrawTexture(sb, Main.npcTexture[npc.type], 0, npc.position + new Vector2(0f, npc.gfxOffY) + topVisualOffset, npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, Main.npcFrameCount[npc.type], npc.frame, lightColor, false);

            BaseDrawing.DrawTexture(sb, mod.GetTexture("Glowmasks/YamataA_Glow"), 0, npc.position + new Vector2(0f, npc.gfxOffY) + topVisualOffset, npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, Main.npcFrameCount[npc.type], npc.frame, AAColor.COLOR_WHITEFADE1, false);
            BaseDrawing.DrawAfterimage(sb, mod.GetTexture("Glowmasks/YamataA_Glow"), 0, npc, 0.8f, 1f, 4, false, 0f, 0f, AAColor.COLOR_WHITEFADE1);

            DrawHead(sb, "NPCs/Bosses/Yamata/Awakened/YamataAHead", "Glowmasks/YamataAHead_Glow", TrueHead, dColor, false);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            int dust1 = mod.DustType<Dusts.YamataADust>();
            int dust2 = mod.DustType<Dusts.YamataADust>();
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
            if (!AAWorld.downedYamata)
            {
                if (npc.life <= (npc.lifeMax / 4 * 3) && threeQuarterHealth == false)
                {
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA6"), new Color(146, 30, 68));
                    threeQuarterHealth = true;
                }
                if (npc.life <= npc.lifeMax / 2 && HalfHealth == false)
                {
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA7"), new Color(146, 30, 68));
                    HalfHealth = true;
                }
                if (npc.life <= npc.lifeMax / 10 && tenthHealth == false)
                {
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA8"), new Color(146, 30, 68));
                    tenthHealth = true;
                }
            }
            if (AAWorld.downedYamata)
            {
                if (npc.life <= (npc.lifeMax / 4 * 3) && threeQuarterHealth == false)
                {
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA9"), new Color(146, 30, 68));
                    threeQuarterHealth = true;
                }
                if (npc.life <= npc.lifeMax / 2 && HalfHealth == false)
                {
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA10"), new Color(146, 30, 68));
                    HalfHealth = true;
                }
                if (npc.life <= npc.lifeMax / 10 && tenthHealth == false)
                {
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA11"), new Color(146, 30, 68));
                    tenthHealth = true;
                }
            }

            if (npc.life <= npc.lifeMax / 2 && !spawnHaruka)
            {
                spawnHaruka = true;
                if (AAWorld.downedYamata)
                {
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA14"), new Color(72, 78, 117));
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA15"), new Color(146, 30, 68));
                    AAModGlobalNPC.SpawnBoss(playerTarget, mod.NPCType("HarukaY"), false, 0, 0);
                    return;
                }
                if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA16"), new Color(146, 30, 68));
                if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA17"), new Color(72, 78, 117));
                AAModGlobalNPC.SpawnBoss(playerTarget, mod.NPCType("HarukaY"), false, 0, 0);
            }
        }

        public bool spawnHaruka = false;

        public override void FindFrame(int frameHeight)
        {
            //npc.frameCounter++;
            if (npc.frameCounter < 5)
            {
                npc.frame.Y = 0 * frameHeight;
            }
            else if (npc.frameCounter < 10)
            {
                npc.frame.Y = 1 * frameHeight;
            }
            else if (npc.frameCounter < 15)
            {
                npc.frame.Y = 2 * frameHeight;
            }
            else if (npc.frameCounter < 20)
            {
                npc.frame.Y = 3 * frameHeight;
            }
            else if (npc.frameCounter < 25)
            {
                npc.frame.Y = 4 * frameHeight;
            }
            else if (npc.frameCounter < 30)
            {
                npc.frame.Y = 5 * frameHeight;
            }
            else if (npc.frameCounter < 35)
            {
                npc.frame.Y = 6 * frameHeight;
            }
            else
            {
                npc.frameCounter = 0;
            }
        }
    }

    public class AnimationInfo
    {
        public int animType = 0;
        public float movementRatio = 0f, movementRate = 0.01f, animMult = 1f;
        public float halfPI = (float)Math.PI / 2f;
        public bool[] fired = new bool[4];
        public float[] hitRatios = null;
        public bool flatJoint = false;

        public AnimationInfo(int type, float aMult = 1f)
        {
            animType = type;
            animMult = aMult;
        }
    }

    public class LimbInfo
    {
        public int limbType = 0;
        public Vector2 position, oldPosition;
        public Vector2 Center
        {
            get { return new Vector2(position.X + (Hitbox.Width * 0.5f), position.Y + (Hitbox.Height * 0.5f)); }
            set { position = new Vector2(value.X - (Hitbox.Width * 0.5f), value.Y - (Hitbox.Height * 0.5f)); }
        }
        public Rectangle Hitbox;
        public float rotation = 0f, movementRatio = 0f;
        public AnimationInfo overrideAnimation = null;
        public YamataA yamataA = null;
    }

    public class LegInfo : LimbInfo
    {
        Vector2 velocity, legOrigin;
        private float velOffsetY = 0f;
        private readonly float distanceToMove = 120f, distanceToMoveX = 50f;
        private readonly bool flying = false;
        private bool leftLeg = false;

        Vector2 pointToStandOn = default;
        Vector2 legJoint = default;
        public Texture2D[] textures = null;

        public LegInfo(int lType, Vector2 initialPos, YamataA m)
        {
            yamataA = m;
            position = initialPos;
            pointToStandOn = position;
            limbType = lType;
            Hitbox = new Rectangle(0, 0, 140, 76);
            legOrigin = new Vector2(limbType == 1 || limbType == 3 ? Hitbox.Width - 12 : 12, 12);
        }

        public void MoveLegFlying(NPC npc)
        {
            Vector2 movementSpot = GetBodyConnector(npc) + new Vector2(limbType == 3 ? (-35f - Hitbox.Width) : limbType == 2 ? 35f : limbType == 1 ? (-15f - Hitbox.Width) : 15f, limbType == 1 || limbType == 0 ? 40f : 50f);
            float velLength = (npc.position - npc.oldPos[1]).Length();
            if (velLength > 8f)
            {
                position = movementSpot;
                velocity = default;
            }
            else
            if (Vector2.Distance(movementSpot, position) > (40 + (int)npc.velocity.Length()))
            {
                Vector2 velAddon = movementSpot - position; velAddon.Normalize(); velAddon *= 2f + (velLength * 0.25f);
                velocity += velAddon;
                float velMax = 4f + velLength;
                if (velocity.Length() > velMax) { velocity.Normalize(); velocity *= velMax; }
                position += velocity;
            }
            else
            {
                position = movementSpot;
                velocity = default;
            }
        }

        public void UpdateVelOffsetY()
        {
            movementRatio += 0.04f;
            movementRatio = Math.Max(0f, Math.Min(1f, movementRatio));
            velOffsetY = BaseUtility.MultiLerp(movementRatio, 0f, 30f, 0f);
        }

        public void MoveLegWalking(NPC npc, Vector2 standOnPoint)
        {
            UpdateVelOffsetY();
            if (pointToStandOn != default)
            {
                Vector2 velAddon = pointToStandOn - position; velAddon.Normalize(); velAddon *= 1.6f + (npc.velocity.Length() * 0.5f);
                velocity += velAddon;
                float velMax = 4f + npc.velocity.Length();
                if (velocity.Length() > velMax) { velocity.Normalize(); velocity *= velMax; }
                if (Vector2.Distance(pointToStandOn, position) <= 15) { position = pointToStandOn; velocity = default; }
                position += velocity;
                if (position == pointToStandOn || Vector2.Distance(standOnPoint, position + new Vector2(Hitbox.Width * 0.5f, 0f)) > distanceToMove || Math.Abs(position.X - standOnPoint.X) > distanceToMoveX)
                {
                    pointToStandOn = default;
                }
            }
            if (pointToStandOn == default)
            {
                if (Vector2.Distance(standOnPoint, position + new Vector2(Hitbox.Width * 0.5f, 0f)) > distanceToMove || Math.Abs(position.X - standOnPoint.X) > distanceToMoveX)
                {
                    movementRatio = 0f;
                    pointToStandOn = standOnPoint;
                }
            }
        }

        public void UpdateLeg(NPC npc)
        {
            leftLeg = limbType == 1 || limbType == 3;
            if (Vector2.Distance(Center, npc.Center) > 499 || YamataA.TeleportMeBitch) position = npc.Center; //prevent issues when the legs are WAY off.
            if (overrideAnimation != null)
            {
                if (overrideAnimation.movementRatio >= 1f) overrideAnimation = null;
            }
            else
            {
                rotation = 0f;
                Vector2 standOnPoint = GetStandOnPoint(npc);
                if (standOnPoint == default) //'flying' behavior but per leg
                {
                    MoveLegFlying(npc);
                }
                else
                {
                    MoveLegWalking(npc, standOnPoint);
                }
            }
            Vector2 bodyConnector = GetBodyConnector(npc);
            legJoint = Vector2.Lerp(position, bodyConnector, 0.3f) + new Vector2(leftLeg ? 30 : 0f, -30);
            oldPosition = position;
        }

        public Vector2 GetStandOnPoint(NPC npc)
        {
            float scalar = npc.velocity.Length();
            float outerLegDefault = 150f + (0.5f * scalar);
            float innerLegDefault = 120f + (0.5f * scalar);
            float standOnX = npc.Center.X + yamataA.topVisualOffset.X + (limbType == 3 ? (-outerLegDefault - Hitbox.Width) : limbType == 2 ? (outerLegDefault + Hitbox.Width) : limbType == 1 ? (-innerLegDefault - Hitbox.Width) : (innerLegDefault + Hitbox.Width));

            int defaultTileY = (int)(npc.Bottom.Y / 16f);
            int tileY = BaseWorldGen.GetFirstTileFloor((int)(standOnX / 16f), (int)(npc.Bottom.Y / 16f));
            if (tileY - defaultTileY > YamataA.flyingTileCount) { return default; } //'flying' behavior
            if (!flying)
            {
                tileY = (int)(tileY * 16f) / 16;
                float tilePosY = tileY * 16f;
                if (Main.tile[(int)(standOnX / 16f), tileY] == null || !Main.tile[(int)(standOnX / 16f), tileY].nactive() || !Main.tileSolid[Main.tile[(int)(standOnX / 16f), tileY].type]) tilePosY += 16f;
                return new Vector2(standOnX - (Hitbox.Width * 0.5f), tilePosY - Hitbox.Height);
            }
            return default;
        }

        public Vector2 GetBodyConnector(NPC npc)
        {
            return npc.Center + yamataA.topVisualOffset + new Vector2(limbType == 3 || limbType == 1 ? -40f : 40f, 0f);
        }

        public void DrawLeg(SpriteBatch sb, NPC npc)
        {
            Mod mod = AAMod.instance;
            if (textures == null)
            {
                string texRoot = "NPCs/Bosses/Yamata/Awakened/YamataA";
                textures = new Texture2D[5];
                textures[0] = mod.GetTexture(texRoot + "LegCap");
                textures[1] = mod.GetTexture(texRoot + "LegSegment");
                textures[2] = mod.GetTexture(texRoot + "LegCapR");
                textures[3] = mod.GetTexture(texRoot + "LegSegmentR");
                textures[4] = mod.GetTexture(texRoot + "Foot");
            }
            Vector2 drawPos = position - new Vector2(0f, velOffsetY);
            Color lightColor = npc.GetAlpha(BaseDrawing.GetLightColor(Center));
            if (!leftLeg)
            {
                BaseDrawing.DrawChain(sb, new Texture2D[] { null, textures[3], null }, 0, drawPos + new Vector2(Hitbox.Width * 0.5f, 6f), legJoint, 0f, null, 1f, false, null);
                BaseDrawing.DrawChain(sb, new Texture2D[] { textures[2], textures[3], textures[2] }, 0, legJoint, GetBodyConnector(npc), 0f, null, 1f, false, null);
            }
            else
            {
                BaseDrawing.DrawChain(sb, new Texture2D[] { null, textures[1], null }, 0, drawPos + new Vector2(Hitbox.Width * 0.5f, 6f), legJoint, 0f, null, 1f, false, null);
                BaseDrawing.DrawChain(sb, new Texture2D[] { textures[0], textures[1], textures[0] }, 0, legJoint, GetBodyConnector(npc), 0f, null, 1f, false, null);
            }
            BaseDrawing.DrawTexture(sb, textures[4], 0, drawPos, Hitbox.Width, Hitbox.Height, npc.scale, rotation, limbType == 1 || limbType == 3 ? 1 : -1, 1, Hitbox, lightColor, false, legOrigin);
        }
    }
}