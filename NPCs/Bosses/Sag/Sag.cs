using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BaseMod;
using System.IO;

namespace AAMod.NPCs.Bosses.Sag
{
    [AutoloadBossHead]
    public class Sag : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sagittarius");
            Main.npcFrameCount[npc.type] = 9;
		}

		public override void SetDefaults()
        {
            npc.lifeMax = 6000;
            npc.boss = true;
            npc.defense = 300;
            npc.damage = 35;
            npc.width = 124;
            npc.height = 206;
            npc.aiStyle = -1;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Sag");
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            bossBag = mod.ItemType("SagBag");
        }

        public float[] internalAI = new float[4];
        Vector2 targetPos;
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(targetPos.X);
                writer.Write(targetPos.Y);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                internalAI[0] = reader.ReadFloat();
                targetPos.X = reader.ReadFloat();
                targetPos.Y = reader.ReadFloat();
            }
        }


        public override void AI()
        {
            if (npc.target == -1)
            {
                npc.TargetClosest(true);
            }

            Player player = Main.player[npc.target];

            #region Direction & Alpha

            if (npc.alpha > 0)
            {
                npc.alpha -= 10;
            }
            if (npc.alpha <= 0)
            {
                npc.alpha = 0;
            }

            if (npc.ai[0] != 2)
            {
                if (player.Center.X > npc.Center.X)
                {
                    npc.direction = 1;
                }
                else
                {
                    npc.direction = -1;
                }
            }
            else
            {
                if (npc.velocity.X > 0)
                {
                    npc.direction = 1;
                }
                else
                {
                    npc.direction = -1;
                }
            }

            for (int m = npc.oldPos.Length - 1; m > 0; m--)
            {
                npc.oldPos[m] = npc.oldPos[m - 1];
            }
            npc.oldPos[0] = npc.position;

            #endregion

            switch ((int)npc.ai[0])
            {
                case 0:
                    if (!DeathCheck())
                        return;

                    int pos;
                    if (player.Center.X > npc.Center.X) //If NPC's X position is less than the player's
                    {
                        pos = 300;
                    }
                    else //If NPC's X position is higher than the player's
                    {
                        pos = -300;
                    }

                    Vector2 wantedVelocity = player.Center - new Vector2(pos, 250);

                    MoveToPoint(wantedVelocity);

                    Shooting();

                    if (npc.ai[1]++ > (Main.expertMode ? 480 : 600))
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                    }
                    break;


                case 1:
                    if (!DeathCheck())
                        return;
                    if (++npc.ai[1] > 30)
                    {
                        targetPos = player.Center;
                        targetPos.X += 1000 * (npc.Center.X < targetPos.X ? -1 : 1);
                        Movement(targetPos, 0.5f);
                        if (npc.ai[1] > 120 || Math.Abs(npc.Center.Y - targetPos.Y) < 16) //initiate dash
                        {
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.netUpdate = true;
                            int speed = npc.life < npc.lifeMax / 3 ? 15 : 18;
                            npc.velocity.X = -speed * (npc.Center.X < player.Center.X ? -1 : 1);
                            npc.velocity.Y *= 0f;
                        }
                    }
                    else
                    {
                        npc.velocity *= 0.9f; //decelerate briefly
                    }
                    npc.rotation = 0;
                    break;

                case 2:

                    if (++npc.ai[1] > 240 || (Math.Sign(npc.velocity.X) > 0 ? npc.Center.X > player.Center.X + 900 : npc.Center.X < player.Center.X - 900))
                    {
                        npc.ai[0] = 0;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] += 1;
                        internalAI[0] = 0;
                        Main.NewText("switching to artillery system set [" + npc.ai[3] + "]");
                        npc.netUpdate = true;
                    }
                    break;
                default:
                    npc.ai[0] = 0;
                    goto case 0;
            }
            if (npc.ai[0] == 2)
            {
                if (npc.rotation < npc.velocity.X * 0.05f)
                {
                    npc.rotation += .01f * npc.direction;
                }
            }
            else
            {
                npc.rotation = 0;
            }
        }

        public void Shooting()
        {
            Player player = Main.player[npc.target];

            switch ((int)npc.ai[3])
            {
                case 0:
                    BaseAI.ShootPeriodic(npc, player.Center, player.width, player.height, ModContent.ProjectileType<SagShot>(), ref npc.ai[2], 60, npc.damage / 4, 9, false, new Vector2(36 * npc.direction, -51));
                    break;
                case 1:
                    if (Main.netMode != 1)
                    {
                        internalAI[0]++;
                        if (internalAI[0] > 180)
                        {
                            internalAI[0] = 0;
                            npc.netUpdate = true;
                        }
                    }
                    if (internalAI[0] > 120)
                    {
                        BaseAI.ShootPeriodic(npc, player.Center + new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10)), player.width, player.height, ModContent.ProjectileType<SagiStar>(), ref npc.ai[2], 20, npc.damage / 4, 9, false, new Vector2(36 * npc.direction, -51));
                    }
                    break;
                case 2:
                    if (Main.netMode != 1)
                    {
                        internalAI[0]++;
                        if (internalAI[0] > 210)
                        {
                            internalAI[0] = 0;
                            npc.netUpdate = true;
                        }
                    }
                    if (internalAI[0] > 120  && internalAI[0] % 30 == 0)
                    {
                        Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.Next(3, 7) * npc.direction, -6f), mod.ProjectileType("SagBomb"), npc.damage / 4, 3);
                    }
                    break;
                case 3:
                    if (Main.netMode != 1)
                    {
                        internalAI[0]++;
                        if (internalAI[0] > 240)
                        {
                            internalAI[0] = 0;
                            npc.netUpdate = true;
                        }
                    }
                    if (internalAI[0] > 120)
                    {
                        BaseAI.ShootPeriodic(npc, player.Center + new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10)), player.width, player.height, ModContent.ProjectileType<SagRocket>(), ref npc.ai[2], 40, npc.damage / 4, 9, false);
                    }
                    break;
                default:
                    npc.ai[3] = 0;
                    goto case 0;
            }

        }

        private void Movement(Vector2 targetPos, float speedModifier)
        {
            if (npc.Center.X < targetPos.X)
            {
                npc.velocity.X += speedModifier;
                if (npc.velocity.X < 0)
                    npc.velocity.X += speedModifier * 2;
            }
            else
            {
                npc.velocity.X -= speedModifier;
                if (npc.velocity.X > 0)
                    npc.velocity.X -= speedModifier * 2;
            }
            if (npc.Center.Y < targetPos.Y)
            {
                npc.velocity.Y += speedModifier;
                if (npc.velocity.Y < 0)
                    npc.velocity.Y += speedModifier * 2;
            }
            else
            {
                npc.velocity.Y -= speedModifier;
                if (npc.velocity.Y > 0)
                    npc.velocity.Y -= speedModifier * 2;
            }
            if (Math.Abs(npc.velocity.X) > 30)
                npc.velocity.X = 30 * Math.Sign(npc.velocity.X);
            if (Math.Abs(npc.velocity.Y) > 30)
                npc.velocity.Y = 30 * Math.Sign(npc.velocity.Y);
        }

        public override void FindFrame(int frameHeight)
        {
            int frameSpeed;
            if (npc.ai[0] != 1)
            {
                frameSpeed = npc.ai[0] == 2 ? 3 : 12 - (int)npc.velocity.X;
                if (npc.velocity.X != 0)
                {
                    if (npc.frameCounter++ > frameSpeed)
                    {
                        npc.frameCounter = 0;
                        npc.frame.Y += frameHeight;
                        if (npc.frame.Y > frameHeight * 3)
                        {
                            npc.frame.Y = 0;
                        }
                    }
                }
            }
            else
            {
                frameSpeed = 7;
                if (npc.frame.Y < frameHeight * 4)
                {
                    npc.frame.Y = frameHeight * 4;
                }
                if (npc.frameCounter++ > frameSpeed)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y += frameHeight;
                    if (npc.frame.Y > frameHeight * 8)
                    {
                        npc.frame.Y = frameHeight * 6;
                    }
                }
            }
        }

        public bool DeathCheck()
        {
            AAPlayer modPlayer = Main.player[npc.target].GetModPlayer<AAPlayer>();
            if (Main.player[npc.target].dead || Vector2.Distance(Main.player[npc.target].Center, npc.Center) > 5000 || !modPlayer.ZoneVoid)
            {
                npc.TargetClosest(true);
                if (Main.player[npc.target].dead || Vector2.Distance(Main.player[npc.target].Center, npc.Center) > 5000 || !modPlayer.ZoneVoid)
                {
                    npc.velocity *= .7f;
                    npc.alpha += 5;
                    if (npc.alpha >= 255)
                    {
                        npc.active = false;
                    }
                    if (!Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) <= 6000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) >= 6000f)
                    {
                        npc.TargetClosest(true);
                    }
                    return false;
                }
            }
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/SagBodyGore"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/SagHeadGore"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/SagLegGore"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/SagLegGore"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/SagNeckGore"), 1f);
                int dust1 = ModContent.DustType<Dusts.VoidDust>();
                int dust2 = ModContent.DustType<Dusts.VoidDust>();
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

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            if (npc.ai[0] == 2)
            {
                BaseDrawing.DrawAfterimage(sb, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.oldPos, npc.scale, npc.rotation, npc.direction, 9, npc.frame, 1f, 1f, 7, true, 0, 0, Color.White);
            }
            BaseDrawing.DrawTexture(sb, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 9, npc.frame, dColor, false);
            BaseDrawing.DrawTexture(sb, mod.GetTexture("Glowmasks/Sag_Glow"), 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 9, npc.frame, ColorUtils.COLOR_GLOWPULSE, false);
            return false;
        }

        public override void NPCLoot()
        {
            AAWorld.downedSag = true;
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SagTrophy"));
            }
            if (!Main.expertMode)
            {
                if (Main.rand.Next(7) == 0)
                {
                    npc.DropLoot(mod.ItemType("SagMask"));
                }
                string[] lootTable = { "SagCore", "NeutronStaff", "Legg" };
                int loot = Main.rand.Next(lootTable.Length);
                npc.DropLoot(mod.ItemType(lootTable[loot]));
                Item.NewItem(npc.Center, ModContent.ItemType<Items.Materials.Doomite>(), Main.rand.Next(30, 40));
            }
            else
            {
                npc.DropBossBags();
            }
        }

        public void MoveToPoint(Vector2 point)
        {
            float moveSpeed = 9f;
            float velMultiplier = 1f;
            Vector2 dist = point - npc.Center;
            float length = dist == Vector2.Zero ? 0f : dist.Length();
            if (length < moveSpeed)
            {
                velMultiplier = MathHelper.Lerp(0f, 1f, length / moveSpeed);
            }
            if (length < 200f)
            {
                moveSpeed *= (length / 200f);
            }
            npc.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            npc.velocity *= moveSpeed;
            npc.velocity *= velMultiplier;
        }

    }
}
