﻿using System;
using System.IO;
using BaseMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Serpent
{
    [AutoloadBossHead]	
	public class SerpentHead : ModNPC
	{
        public int damage = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Subzero Serpent");
            Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.npcSlots = 5f;
            npc.width = 38;
            npc.height = 38;
            npc.damage = 35;
            npc.defense = 25;
            npc.lifeMax = 6000;
            npc.value = Item.buyPrice(0, 5, 0, 0);
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            animationType = 10;
            npc.behindTiles = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit5;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.netAlways = true;
            npc.boss = true;
            bossBag = mod.ItemType<Items.Boss.Serpent.SerpentBag>();
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Boss6");
            npc.alpha = 50;
        }

        private bool fireAttack;
        private int attackCounter;
        private int attackTimer;

		public bool tongueFlick = false;
		public bool tongueFlickDir = false;
		public int tongueFlickCounter = 0;
        private int RunOnce = 0;
        private int StopSnow = 0;

        public float[] internalAI = new float[5];
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
                writer.Write(internalAI[4]);
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
                internalAI[4] = reader.ReadFloat();
            }
        }


        public override void AI()
        {
            bool isHead = npc.type == mod.NPCType("SerpentHead");
            bool isBody = npc.type == mod.NPCType("SerpentBody");

            if (Main.expertMode)
            {
                damage = npc.damage / 4;
            }
            else
            {
                damage = npc.damage / 2;
            }

            Player player = Main.player[npc.target];

            if (player.dead || !player.active || !player.ZoneSnow)
            {
                npc.TargetClosest(true);
                if (player.dead || !player.active || !player.ZoneSnow)
                {
                    internalAI[0]++;
                    npc.velocity.Y = npc.velocity.Y + 0.8f;
                    if (internalAI[0] >= 300)
                    {
                        npc.active = false;
                    }
                }
                else
                {
                    internalAI[0] = 0;
                }
            }
            else
            {
                if (npc.alpha > 0)
                {
                    npc.alpha -= 4;
                }
                else
                {
                    npc.alpha = 0;
                }
            }

            if (RunOnce == 0)
            {
                RainStart();
                RunOnce = 1;
            }
            if (Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 6000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 6000f || Main.player[npc.target].dead)
            {
                if (StopSnow == 0)
                {
                    RainStop();
                    StopSnow = 1;
                }
            }
            WormAI();
            int tileX = (int)(npc.position.X / 16f) - 1;
            int tileCenterX = (int)((npc.Center.X) / 16f) + 2;
            int tileY = (int)(npc.position.Y / 16f) - 1;
            int tileCenterY = (int)((npc.Center.Y) / 16f) + 2;
            if (tileX < 0) { tileX = 0; }
            if (tileCenterX > Main.maxTilesX) { tileCenterX = Main.maxTilesX; }
            if (tileY < 0) { tileY = 0; }
            if (tileCenterY > Main.maxTilesY) { tileCenterY = Main.maxTilesY; }
            for (int tX = tileX; tX < tileCenterX; tX++)
            {
                for (int tY = tileY; tY < tileCenterY; tY++)
                {
                    Tile checkTile = BaseWorldGen.GetTileSafely(tX, tY);
                    if (checkTile != null && ((checkTile.nactive() && (Main.tileSolid[checkTile.type] || (Main.tileSolidTop[checkTile.type] && checkTile.frameY == 0))) || checkTile.liquid > 64))
                    {
                        Vector2 tPos;
                        tPos.X = tX * 16;
                        tPos.Y = tY * 16;
                        if (npc.position.X + npc.width > tPos.X && npc.position.X < tPos.X + 16f && npc.position.Y + npc.height > tPos.Y && npc.position.Y < tPos.Y + 16f)
                        {
                            if (Main.rand.Next(100) == 0 && checkTile.nactive())
                            {
                                WorldGen.KillTile(tX, tY, true, true, false);
                            }
                        }
                    }
                }
            }
            if (isHead)
            {
                if (Main.netMode != 2 && !tongueFlick && Main.rand.Next(20) == 0)
                {
                    tongueFlick = true;
                }

                internalAI[1]++;
                if (internalAI[1] > 300 && Main.netMode != 1)
                {
                    internalAI[1] = 0;
                    internalAI[2] = Main.rand.Next(3);
                    npc.netUpdate = true;
                }

                if (Main.netMode != 1) //frost breath attack
                {
                    if (internalAI[2] == 0)
                    {
                        IceSentry();
                    }
                    else if (internalAI[2] == 1)
                    {
                        Iceball();
                    }
                    else
                    {
                        FrostAttack();
                    }

                    if (internalAI[3]++ > 400 && NPC.CountNPCS(mod.NPCType<Enemies.Snow.SnakeHead>()) < 3)
                    {
                        for (int i = 0; i < 3 - NPC.CountNPCS(mod.NPCType<Enemies.Snow.SnakeHead>()); i++)
                        {
                            AAModGlobalNPC.SpawnBoss(player, mod.NPCType<Enemies.Snow.SnakeHead>(), false, 0, 0, "Snake", false);
                        }
                        internalAI[3] = 0;
                    }
                }

                if (tongueFlick)
                {
                    if (tongueFlickDir)
                    {
                        tongueFlickCounter--;
                        if (tongueFlickCounter <= 0)
                        {
                            tongueFlickCounter = 8;
                            npc.frame.Y -= npc.frame.Height;
                            if (npc.frame.Y <= 0)
                                tongueFlick = tongueFlickDir = false;
                        }
                    }
                    else
                    {
                        tongueFlickCounter--;
                        if (tongueFlickCounter <= 0)
                        {
                            tongueFlickCounter = 8;
                            npc.frame.Y += npc.frame.Height;
                            if (npc.frame.Y >= (npc.frame.Height * 3))
                                tongueFlickDir = true;
                        }
                    }
                }
            }
            else
            if (isBody)
            {
                if (npc.localAI[0] == 0)
                {
                    npc.localAI[0] = 1;
                    npc.localAI[1] = Main.rand.Next(4);
                }
                npc.frame.Y = (int)npc.localAI[1] * npc.frame.Height;
            }
        }

        private void RainStart()
        {
            if (!Main.raining)
            {
                int num = 86400;
                int num2 = num / 24;
                Main.rainTime = Main.rand.Next(num2 * 8, num);
                if (Main.rand.Next(3) == 0)
                {
                    Main.rainTime += Main.rand.Next(0, num2);
                }
                if (Main.rand.Next(4) == 0)
                {
                    Main.rainTime += Main.rand.Next(0, num2 * 2);
                }
                if (Main.rand.Next(5) == 0)
                {
                    Main.rainTime += Main.rand.Next(0, num2 * 2);
                }
                if (Main.rand.Next(6) == 0)
                {
                    Main.rainTime += Main.rand.Next(0, num2 * 3);
                }
                if (Main.rand.Next(7) == 0)
                {
                    Main.rainTime += Main.rand.Next(0, num2 * 4);
                }
                if (Main.rand.Next(8) == 0)
                {
                    Main.rainTime += Main.rand.Next(0, num2 * 5);
                }
                float num3 = 1f;
                if (Main.rand.Next(2) == 0)
                {
                    num3 += 0.05f;
                }
                if (Main.rand.Next(3) == 0)
                {
                    num3 += 0.1f;
                }
                if (Main.rand.Next(4) == 0)
                {
                    num3 += 0.15f;
                }
                if (Main.rand.Next(5) == 0)
                {
                    num3 += 0.2f;
                }
                Main.rainTime = (int)(Main.rainTime * num3);
                Main.raining = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
                }
            }
        }

        private void RainStop()
        {
            if (Main.raining)
            {
                Main.rainTime = 0;
                Main.raining = false;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
                }
            }
        }

        public void Iceball()
        {
            attackCounter++;
            if (attackCounter >= 180 && fireAttack == false)
            {
                attackCounter = 0;
                fireAttack = true;
                npc.netUpdate = true;
            }
            if (fireAttack == true && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                attackTimer++;
                if (attackTimer == 20 || attackTimer == 50 || attackTimer == 79)
                {
                    BaseAI.FireProjectile(Main.player[npc.target].Center, npc, mod.ProjectileType<IceBall2>(), damage, 3, 14f, 0, 0, -1);
                    npc.netUpdate = true;
                }
                if (attackTimer >= 80)
                {
                    fireAttack = false;
                    attackTimer = 0;
                    attackCounter = 0;
                    npc.netUpdate = true;
                }
            }
        }

        public void IceSentry()
        {
            Player player = Main.player[npc.target];
            if (NPC.CountNPCS(mod.NPCType<IceCrystal>()) < 3)
            {
                NPC.NewNPC((int)player.position.X + Main.rand.Next(-500, 500), (int)player.position.Y + 500, mod.NPCType<IceCrystal>(), 0, 0, 0, 0, 0, npc.target);
            }
            internalAI[2] = 2;
            npc.netUpdate = true;
        }

        public void FrostAttack()
        {
            attackCounter++;
            if (attackCounter == 400 && fireAttack == false)
            {
                attackCounter = 0;
                fireAttack = true;
                npc.netUpdate = true;
            }
            if (fireAttack == true)
            {
                attackTimer++;
                if ((attackTimer == 8 || attackTimer == 16 || attackTimer == 24 || attackTimer == 32 || attackTimer == 40 || attackTimer == 48 || attackTimer == 56 || attackTimer == 64 || attackTimer == 72 || attackTimer == 79) && !npc.HasBuff(103))
                {
                    for (int i = 0; i < 5; ++i)
                    {
                        float num433 = 6f;
                        Vector2 PlayerDistance = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                        float PlayerPosX = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2) - PlayerDistance.X;
                        float PlayerPosY = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - PlayerDistance.Y;
                        float PlayerPos = (float)Math.Sqrt(PlayerPosX * PlayerPosX + PlayerPosY * PlayerPosY);
                        PlayerPos = num433 / PlayerPos;
                        PlayerPosX *= PlayerPos;
                        PlayerPosY *= PlayerPos;
                        PlayerPosY += Main.rand.Next(-40, 41) * 0.01f;
                        PlayerPosX += Main.rand.Next(-40, 41) * 0.01f;
                        PlayerPosY += npc.velocity.Y * 0.5f;
                        PlayerPosX += npc.velocity.X * 0.5f;
                        PlayerDistance.X -= PlayerPosX * 1f;
                        PlayerDistance.Y -= PlayerPosY * 1f;
                        Projectile.NewProjectile(PlayerDistance.X, PlayerDistance.Y, npc.velocity.X * 2f, npc.velocity.Y * 2f, mod.ProjectileType("SnowBreath"), damage, 0, Main.myPlayer);
                    }
                }
                if (attackTimer >= 80)
                {
                    fireAttack = false;
                    attackTimer = 0;
                    attackCounter = 0;
                    npc.netUpdate = true;
                }
            }
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			if (Main.expertMode)
			{
				player.AddBuff(BuffID.Chilled, 200, true);
			}
			else
			{
				player.AddBuff(BuffID.Chilled, 100, true);
			}
		}

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;   //boss drops
            AAWorld.downedSerpent = true;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.85f);
		}

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType<Dusts.IceDust>(), hitDirection, -1f, 0, default, 1f);
            }
            if (npc.life == 0)
            {
                for (int k = 0; k < 5; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType<Dusts.SnowDustLight>(), hitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void NPCLoot()
        {
            if (!Main.expertMode)
            {
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SerpentMask"));
                }
                AAWorld.downedSerpent = true;
                npc.DropLoot(mod.ItemType("SnowMana"), 10, 15);
                string[] lootTable = { "BlizardBuster", "SerpentSpike", "Icepick", "SerpentSting", "Sickle", "SickleShot", "SnakeStaff", "SubzeroSlasher" };
                int loot = Main.rand.Next(lootTable.Length);
                npc.DropLoot(Items.Vanity.Mask.SerpentMask.type, 1f / 7);
                if (Main.rand.Next(9) == 0)
                {
                    npc.DropLoot(mod.ItemType("SnowflakeShuriken"), 90, 120);
                }
                else
                {
                    npc.DropLoot(mod.ItemType(lootTable[loot]));
                }
            }
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SerpentTrophy"));
            }
            npc.value = 0f;
            npc.boss = false;
        }

        public void WormAI()
        {
            bool isHead = npc.type == mod.NPCType("SerpentHead");

            Player player = Main.player[npc.target];
            float speed = 17;
			float turnSpeed = .2f;
            if (isHead)
            {
                if (npc.ai[3] > 0f)
                {
                    npc.realLife = (int)npc.ai[3];
                }
                if (npc.target < 0 || npc.target == 255 || player.dead)
                {
                    npc.TargetClosest(true);
                }
                npc.velocity.Length();
                npc.alpha -= 42;
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
                if (Main.netMode != 1)
                {
                    if (internalAI[4] == 0)
                    {
                        int Previous = npc.whoAmI;
                        for (int num36 = 0; num36 < 12; num36++)
                        {
                            int Segment;
                            if (num36 >= 0 && num36 < 12)
                            {
                                Segment = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height / 2), mod.NPCType<SerpentBody>(), npc.whoAmI);
                            }
                            else
                            {
                                Segment = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height / 2), mod.NPCType<SerpentTail>(), npc.whoAmI);
                            }
                            Main.npc[Segment].realLife = npc.whoAmI;
                            Main.npc[Segment].ai[2] = npc.whoAmI;
                            Main.npc[Segment].ai[1] = Previous;
                            Main.npc[Previous].ai[0] = Segment;
                            NetMessage.SendData(23, -1, -1, null, Segment, 0f, 0f, 0f, 0);
                            Previous = Segment;
                        }
                        internalAI[4] = 1;
                        npc.netUpdate = true;
                    }
                }
                int num180 = (int)(npc.position.X / 16f) - 1;
                int num181 = (int)((npc.position.X + npc.width) / 16f) + 2;
                int num182 = (int)(npc.position.Y / 16f) - 1;
                int num183 = (int)((npc.position.Y + npc.height) / 16f) + 2;
                if (num180 < 0)
                {
                    num180 = 0;
                }
                if (num181 > Main.maxTilesX)
                {
                    num181 = Main.maxTilesX;
                }
                if (num182 < 0)
                {
                    num182 = 0;
                }
                if (num183 > Main.maxTilesY)
                {
                    num183 = Main.maxTilesY;
                }

                bool flag94 = false;
                for (int num952 = num180; num952 < num181; num952++)
                {
                    for (int num953 = num182; num953 < num183; num953++)
                    {
                        if (Main.tile[num952, num953] != null && ((Main.tile[num952, num953].nactive() && (Main.tileSolid[Main.tile[num952, num953].type] || (Main.tileSolidTop[Main.tile[num952, num953].type] && Main.tile[num952, num953].frameY == 0))) || Main.tile[num952, num953].liquid > 64))
                        {
                            Vector2 vector105;
                            vector105.X = num952 * 16;
                            vector105.Y = num953 * 16;
                            if (npc.position.X + npc.width > vector105.X && npc.position.X < vector105.X + 16f && npc.position.Y + npc.height > vector105.Y && npc.position.Y < vector105.Y + 16f)
                            {
                                flag94 = true;
                                break;
                            }
                        }
                    }
                }
                npc.localAI[1] = 1f;
                Rectangle rectangle12 = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                int num954 = 1000;
                bool flag95 = true;
                if (npc.position.Y > player.position.Y)
                {
                    for (int num955 = 0; num955 < 255; num955++)
                    {
                        if (Main.player[num955].active)
                        {
                            Rectangle rectangle13 = new Rectangle((int)Main.player[num955].position.X - num954, (int)Main.player[num955].position.Y - num954, num954 * 2, num954 * 2);
                            if (rectangle12.Intersects(rectangle13))
                            {
                                flag95 = false;
                                break;
                            }
                        }
                    }
                    if (flag95)
                    {
                        flag94 = true;
                    }
                }

                float num188 = speed;
                float num189 = turnSpeed;
                Vector2 vector18 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float num191 = player.position.X + player.width / 2;
                float num192 = player.position.Y + player.height / 2;
                num191 = (int)(num191 / 16f) * 16;
                num192 = (int)(num192 / 16f) * 16;
                vector18.X = (int)(vector18.X / 16f) * 16;
                vector18.Y = (int)(vector18.Y / 16f) * 16;
                num191 -= vector18.X;
                num192 -= vector18.Y;
                float num193 = (float)Math.Sqrt(num191 * num191 + num192 * num192);
                if (npc.ai[1] > 0f && npc.ai[1] < Main.npc.Length)
                {
                    try
                    {
                        vector18 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                        num191 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - vector18.X;
                        num192 = Main.npc[(int)npc.ai[1]].position.Y + Main.npc[(int)npc.ai[1]].height / 2 - vector18.Y;
                    }
                    catch
                    {
                    }
                    npc.rotation = (float)Math.Atan2(num192, num191) + 1.57f;
                    num193 = (float)Math.Sqrt(num191 * num191 + num192 * num192);
                    int num194 = npc.width;
                    num193 = (num193 - num194) / num193;
                    num191 *= num193;
                    num192 *= num193;
                    npc.velocity = Vector2.Zero;
                    npc.position.X = npc.position.X + num191;
                    npc.position.Y = npc.position.Y + num192;
                }
                else
                {
                    if (!flag94)
                    {
                        npc.TargetClosest(true);
                        npc.velocity.Y = npc.velocity.Y + (turnSpeed * 0.75f);
                        if (npc.velocity.Y > num188)
                        {
                            npc.velocity.Y = num188;
                        }
                        if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num188 * 0.4)
                        {
                            if (npc.velocity.X < 0f)
                            {
                                npc.velocity.X = npc.velocity.X - num189 * 1.1f;
                            }
                            else
                            {
                                npc.velocity.X = npc.velocity.X + num189 * 1.1f;
                            }
                        }
                        else if (npc.velocity.Y == num188)
                        {
                            if (npc.velocity.X < num191)
                            {
                                npc.velocity.X = npc.velocity.X + num189;
                            }
                            else if (npc.velocity.X > num191)
                            {
                                npc.velocity.X = npc.velocity.X - num189;
                            }
                        }
                        else if (npc.velocity.Y > 4f)
                        {
                            if (npc.velocity.X < 0f)
                            {
                                npc.velocity.X = npc.velocity.X + num189 * 0.9f;
                            }
                            else
                            {
                                npc.velocity.X = npc.velocity.X - num189 * 0.9f;
                            }
                        }
                    }
                    else
                    {
                        if (npc.behindTiles && npc.soundDelay == 0)
                        {
                            float num195 = num193 / 40f;
                            if (num195 < 10f)
                            {
                                num195 = 10f;
                            }
                            if (num195 > 20f)
                            {
                                num195 = 20f;
                            }
                            npc.soundDelay = (int)num195;
                            Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 1);
                        }
                        num193 = (float)Math.Sqrt(num191 * num191 + num192 * num192);
                        float num196 = Math.Abs(num191);
                        float num197 = Math.Abs(num192);
                        float num198 = num188 / num193;
                        num191 *= num198;
                        num192 *= num198;
                        bool flag21 = false;
                        if (!flag21)
                        {
                            if ((npc.velocity.X > 0f && num191 > 0f) || (npc.velocity.X < 0f && num191 < 0f) || (npc.velocity.Y > 0f && num192 > 0f) || (npc.velocity.Y < 0f && num192 < 0f))
                            {
                                if (npc.velocity.X < num191)
                                {
                                    npc.velocity.X = npc.velocity.X + num189;
                                }
                                else
                                {
                                    if (npc.velocity.X > num191)
                                    {
                                        npc.velocity.X = npc.velocity.X - num189;
                                    }
                                }
                                if (npc.velocity.Y < num192)
                                {
                                    npc.velocity.Y = npc.velocity.Y + num189;
                                }
                                else
                                {
                                    if (npc.velocity.Y > num192)
                                    {
                                        npc.velocity.Y = npc.velocity.Y - num189;
                                    }
                                }
                                if (Math.Abs(num192) < num188 * 0.2 && ((npc.velocity.X > 0f && num191 < 0f) || (npc.velocity.X < 0f && num191 > 0f)))
                                {
                                    if (npc.velocity.Y > 0f)
                                    {
                                        npc.velocity.Y = npc.velocity.Y + num189 * 2f;
                                    }
                                    else
                                    {
                                        npc.velocity.Y = npc.velocity.Y - num189 * 2f;
                                    }
                                }
                                if (Math.Abs(num191) < num188 * 0.2 && ((npc.velocity.Y > 0f && num192 < 0f) || (npc.velocity.Y < 0f && num192 > 0f)))
                                {
                                    if (npc.velocity.X > 0f)
                                    {
                                        npc.velocity.X = npc.velocity.X + num189 * 2f;
                                    }
                                    else
                                    {
                                        npc.velocity.X = npc.velocity.X - num189 * 2f;
                                    }
                                }
                            }
                            else
                            {
                                if (num196 > num197)
                                {
                                    if (npc.velocity.X < num191)
                                    {
                                        npc.velocity.X = npc.velocity.X + num189 * 1.1f;
                                    }
                                    else if (npc.velocity.X > num191)
                                    {
                                        npc.velocity.X = npc.velocity.X - num189 * 1.1f;
                                    }
                                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num188 * 0.5)
                                    {
                                        if (npc.velocity.Y > 0f)
                                        {
                                            npc.velocity.Y = npc.velocity.Y + num189;
                                        }
                                        else
                                        {
                                            npc.velocity.Y = npc.velocity.Y - num189;
                                        }
                                    }
                                }
                                else
                                {
                                    if (npc.velocity.Y < num192)
                                    {
                                        npc.velocity.Y = npc.velocity.Y + num189 * 1.1f;
                                    }
                                    else if (npc.velocity.Y > num192)
                                    {
                                        npc.velocity.Y = npc.velocity.Y - num189 * 1.1f;
                                    }
                                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num188 * 0.5)
                                    {
                                        if (npc.velocity.X > 0f)
                                        {
                                            npc.velocity.X = npc.velocity.X + num189;
                                        }
                                        else
                                        {
                                            npc.velocity.X = npc.velocity.X - num189;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
                    if (flag94)
                    {
                        if (npc.localAI[0] != 1f)
                        {
                            npc.netUpdate = true;
                        }
                        npc.localAI[0] = 1f;
                    }
                    else
                    {
                        if (npc.localAI[0] != 0f)
                        {
                            npc.netUpdate = true;
                        }
                        npc.localAI[0] = 0f;
                    }
                    if (((npc.velocity.X > 0f && npc.oldVelocity.X < 0f) || (npc.velocity.X < 0f && npc.oldVelocity.X > 0f) || (npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f) || (npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f)) && !npc.justHit)
                    {
                        npc.netUpdate = true;
                    }
                }
            }
            else
            {
                if (!Main.npc[(int)npc.ai[1]].active)
                {
                    npc.life = 0;
                    npc.HitEffect(0, 10.0);
                    npc.active = false;
                }
            }
        }
    }
}