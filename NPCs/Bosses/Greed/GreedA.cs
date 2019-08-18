﻿using System;
using System.IO;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Greed
{
    [AutoloadBossHead]
	public class GreedA : ModNPC
	{
        public int damage = 0;
        bool loludided = false;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Greed");
		}

		public override void SetDefaults()
		{
			npc.npcSlots = 5f;
            npc.width = 38;
            npc.height = 38;
            npc.damage = 70;
            npc.defense = 30;
            npc.lifeMax = 60000;
            npc.value = Item.buyPrice(0, 5, 0, 0);
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            npc.behindTiles = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit5;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.netAlways = true;
            npc.boss = true;
            bossBag = mod.ItemType("GreedBag");
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/GreedA");
            npc.alpha = 255;
        }

        public float[] internalAI = new float[6];
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == 2 || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
                writer.Write(internalAI[4]);
                writer.Write(internalAI[5]);
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
                internalAI[4] = reader.ReadFloat();
                internalAI[5] = reader.ReadFloat();
            }
        }

        public override bool PreAI()
        {
            npc.TargetClosest();
            Player player = Main.player[npc.target];
            if (Main.expertMode)
            {
                damage = npc.damage / 4;
            }
            else
            {
                damage = npc.damage / 2;
            }

            if (npc.alpha <= 0)
            {
                npc.alpha = 0;
            }
            else
            {
                npc.alpha -= 3;
                if (npc.alpha != 0)
                {
                    for (int spawnDust = 0; spawnDust < 4; spawnDust++)
                    {
                        int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.GoldCoin, 0f, 0f, 100, default, 2f);
                        Main.dust[num935].noGravity = true;
                        Main.dust[num935].noLight = true;
                    }
                }
            }


            if (!Main.gamePaused && Main.rand.Next(60) == 0 && Main.LocalPlayer.findTreasure)
            {
                int num52 = Dust.NewDust(npc.Center, 16, 16, 204, 0f, 0f, 150, default, 0.3f);
                Main.dust[num52].fadeIn = 1f;
                Main.dust[num52].velocity *= 0.1f;
                Main.dust[num52].noLight = true;
            }

            npc.spriteDirection = npc.velocity.X > 0 ? -1 : 1;
            npc.ai[1]++;
            if (npc.ai[1] >= 1200)
                npc.ai[1] = 0;
            npc.TargetClosest(true);
            if (!Main.player[npc.target].active || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
                if (!Main.player[npc.target].active || Main.player[npc.target].dead)
                {
                    npc.ai[3]++;
                    npc.velocity.Y = npc.velocity.Y + 0.11f;
                    if (npc.ai[3] >= 300)
                    {
                        npc.active = false;
                    }
                }
                else
                    npc.ai[3] = 0;
            }
            if (Main.netMode != 1)
            {
                if (npc.ai[0] == 0)
                {
                    npc.realLife = npc.whoAmI;
                    int latestNPC = npc.whoAmI;

                    for (int i = 0; i < 24; ++i)
                    {
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("GreedABody"), npc.whoAmI, 0, latestNPC);
                        Main.npc[latestNPC].realLife = npc.whoAmI;
                        Main.npc[latestNPC].ai[2] = i;
                        Main.npc[latestNPC].ai[3] = npc.whoAmI;
                    }

                    npc.ai[0] = 1;
                    npc.netUpdate = true;
                }
            }

            bool collision = true;

            float speed = 20f;
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

            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            if (!collision)
            {
                npc.TargetClosest(true);
                npc.velocity.Y = npc.velocity.Y + 0.11f;
                if (npc.velocity.Y > speed)
                    npc.velocity.Y = speed;
                if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.4)
                {
                    if (npc.velocity.X < 0.0)
                        npc.velocity.X = npc.velocity.X - acceleration * 1.1f;
                    else
                        npc.velocity.X = npc.velocity.X + acceleration * 1.1f;
                }
                else if (npc.velocity.Y == speed)
                {
                    if (npc.velocity.X < dirX)
                        npc.velocity.X = npc.velocity.X + acceleration;
                    else if (npc.velocity.X > dirX)
                        npc.velocity.X = npc.velocity.X - acceleration;
                }
                else if (npc.velocity.Y > 4.0)
                {
                    if (npc.velocity.X < 0.0)
                        npc.velocity.X = npc.velocity.X + acceleration * 0.9f;
                    else
                        npc.velocity.X = npc.velocity.X - acceleration * 0.9f;
                }
            }
            else
            {
                if (npc.soundDelay == 0)
                {
                    float num1 = length / 40f;
                    if (num1 < 10.0)
                        num1 = 10f;
                    if (num1 > 20.0)
                        num1 = 20f;
                    npc.soundDelay = (int)num1;
                }
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
            }
            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;

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

            if (player.position.Y < (Main.worldSurface * 16.0))
            {
                if (loludided == false)
                {
                    if (Main.netMode != 1) BaseUtility.Chat("EEEEEEEEEEEEEEEGH THE LIGHT OF THE SURFACE! TOO BRIGHT! TOO BRIGHT!", Color.Goldenrod);
                    loludided = true;
                }
                npc.velocity.Y = npc.velocity.Y + 1f;
                if (npc.position.Y - npc.height - npc.velocity.Y >= Main.maxTilesY && Main.netMode != 1) { BaseAI.KillNPC(npc); npc.netUpdate2 = true; }
            }

            if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 6000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 6000f)
            {
                if (loludided == false)
                {
                    if (Main.netMode != 1) BaseUtility.Chat("AND STAY AWAY FROM MY GLORIOUS RICHES YOU LITTLE THIEF!", Color.Goldenrod);
                    loludided = true;
                }
                npc.velocity.Y = npc.velocity.Y - 1f;
                if (npc.position.Y < 0)
                {
                    npc.velocity.Y = npc.velocity.Y - 1f;
                }
                if (npc.position.Y < 0)
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
            else
            {
                if (npc.localAI[0] != 0.0)
                    npc.netUpdate = true;
                npc.localAI[0] = 0.0f;
            }
            if ((npc.velocity.X > 0.0 && npc.oldVelocity.X < 0.0 || npc.velocity.X < 0.0 && npc.oldVelocity.X > 0.0 || npc.velocity.Y > 0.0 && npc.oldVelocity.Y < 0.0 || npc.velocity.Y < 0.0 && npc.oldVelocity.Y > 0.0) && !npc.justHit)
                npc.netUpdate = true;

            return false;
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            if (item.pick > 0)
            {
                npc.StrikeNPC(damage + item.pick, knockback, 0, true);
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.type == mod.NPCType<GreedBody>())
            {
                npc.frame.Y = frameHeight * (int)npc.ai[2];
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;   //boss drops
            AAWorld.downedSerpent = true;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (npc.type != mod.NPCType<Greed>())
            {
                return false;
            }
            scale = 1.5f;
            return true;
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
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Dirt, hitDirection, -1f, 0, default, 1f);
            }
            if (npc.life == 0)
            {
                for (int k = 0; k < 5; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, DustID.Dirt, hitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void NPCLoot()
        {
            if (!Main.expertMode)
            {
                AAWorld.downedSerpent = true;
                npc.DropLoot(mod.ItemType("CovetiteCoin"), 10, 15);
                string[] lootTable = {  };
                int loot = Main.rand.Next(lootTable.Length);
                //npc.DropLoot(Items.Vanity.Mask.GreedMask.type, 1f / 7);
                npc.DropLoot(mod.ItemType(lootTable[loot]));
            }
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GreedTrophy"));
            }
            npc.value = 0f;
            npc.boss = false;
        }

        public override void BossHeadRotation(ref float rotation)
        {
            rotation = npc.rotation;
        }
    }

    [AutoloadBossHead]
    public class GreedABody : Greed
    {
        public override string Texture { get { return "AAMod/NPCs/Bosses/Greed/GreedBody"; } }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Greed");
            NPCID.Sets.TechnicallyABoss[npc.type] = true;
            Main.npcFrameCount[npc.type] = 24;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.dontCountMe = true;
            npc.alpha = 255;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override bool PreNPCLoot()
        {
            return false;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            damage *= .05f;
            return true;
        }

        public override bool PreAI()
        {
            npc.defense = Def();
            Vector2 chasePosition = Main.npc[(int)npc.ai[1]].Center;
            Vector2 directionVector = chasePosition - npc.Center;
            npc.spriteDirection = (directionVector.X > 0f) ? 1 : -1;
            if (npc.ai[3] > 0)
                npc.realLife = (int)npc.ai[3];
            if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
                npc.TargetClosest(true);
            if (Main.player[npc.target].dead && npc.timeLeft > 300)
                npc.timeLeft = 300;

            if (Main.netMode != 1)
            {
                if (!Main.npc[(int)npc.ai[3]].active || Main.npc[(int)npc.ai[3]].type != mod.NPCType("Greed"))
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

            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest(true);
            }
            if (npc.alpha <= 0)
            {
                npc.alpha = 0;
                return false;
            }
            else
            {
                for (int spawnDust = 0; spawnDust < 4; spawnDust++)
                {
                    int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.GoldCoin, 0f, 0f, 100, default, 2f);
                    Main.dust[num935].noGravity = true;
                    Main.dust[num935].noLight = true;
                }
                npc.alpha -= 3;
                return false;
            }
        }

        public override void BossHeadRotation(ref float rotation)
        {
            rotation = npc.rotation;
        }

        public override bool CheckActive()
        {
            if (NPC.AnyNPCs(mod.NPCType<Greed>()))
            {
                return false;
            }
            npc.active = false;
            return true;
        }

        public int Def()
        {
            switch ((int)npc.ai[2])
            {
                case 0:
                    return npc.defense = 6;
                case 1:
                    return npc.defense = 7;
                case 2:
                    return npc.defense = 9;
                case 3:
                    return npc.defense = 11;
                case 4:
                    return npc.defense = 13;
                case 5:
                    return npc.defense = 15;
                case 6:
                    return npc.defense = 16;
                case 7:
                    return npc.defense = 20;
                case 8:
                    return npc.defense = 19;
                case 9:
                    return npc.defense = 19;
                case 10:
                    return npc.defense = 15;
                case 11:
                    return npc.defense = 21;
                case 12:
                    return npc.defense = 18;
                case 13:
                    return npc.defense = 25;
                case 14:
                    return npc.defense = 26;
                case 15:
                    return npc.defense = 32;
                case 16:
                    return npc.defense = 37;
                case 17:
                    return npc.defense = 42;
                case 18:
                    return npc.defense = 50;
                case 19:
                    return npc.defense = 49;
                case 20:
                    return npc.defense = 53;
                case 21:
                    return npc.defense = 56;
                case 22:
                    return npc.defense = 58;
                default:
                    return npc.defense = 30;
            }
        }


        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];

            npc.position.Y += npc.height * 0.5f;

            BaseDrawing.DrawTexture(spritebatch, texture, 0, npc, dColor);
            if (Main.LocalPlayer.findTreasure)
            {
                Color color = dColor;
                byte b2 = 200;
                byte b3 = 170;
                if (color.R < b2)
                {
                    color.R = b2;
                }
                if (color.G < b3)
                {
                    color.G = b3;
                }
                color.A = Main.mouseTextColor;
                BaseDrawing.DrawTexture(spritebatch, texture, 0, npc, color);
            }

            npc.position.Y -= npc.height * 0.5f;
            return false;
        }
    }
}