﻿using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Shen
{
    [AutoloadBossHead]
    public class ShenA : ShenDoragon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shen Doragon Awakened; Unyielding Chaos Incarnate");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.damage = 260;
            npc.defense = 240;
            npc.lifeMax = 1200000;
            npc.value = Item.sellPrice(1, 0, 0, 0);
            bossBag = mod.ItemType("ShenCache");
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/ShenA");
            musicPriority = (MusicPriority)11;
            isAwakened = true;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
            npc.defense = (int)(npc.defense * 1.2f);
            npc.damage = (int)(npc.damage * .8f);
            damageDiscordianInferno = (int)(damageDiscordianInferno * 1.2f);
        }

        public override void AI()
        {
            Player player = Main.player[npc.target];
            Vector2 targetPos;
            switch ((int)npc.ai[0])
            {
                case 0: //target for first time, navigate beside player
                    if (!npc.HasPlayerTarget)
                        npc.TargetClosest();
                    if (!AliveCheck(Main.player[npc.target]))
                        break;
                    targetPos = player.Center;
                    targetPos.X += 500 * (npc.Center.X < targetPos.X ? -1 : 1);
                    Movement(targetPos, 0.5f);
                    if (++npc.ai[2] > 300)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.netUpdate = true;
                        npc.velocity.X = 4 * (npc.Center.X < targetPos.X ? -1 : 1);
                        npc.velocity.Y *= 0.1f;
                        if (Main.netMode != 1)
                            Main.NewText("spawn mega ray");
                    }
                    if (++npc.ai[1] > 60)
                    {
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                        if (Main.netMode != 1)
                            Main.NewText("spawn predictive delayed fireballs");
                    }
                    break;

                case 1: //firing mega ray
                    if (++npc.ai[1] > 120)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 2: //fly to corner for dash
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center;
                    targetPos.X += 600 * (npc.Center.X < targetPos.X ? -1 : 1);
                    targetPos.Y -= 600;
                    Movement(targetPos, 0.8f);
                    if (++npc.ai[1] > 180 || npc.Distance(targetPos) < 50) //initiate dash
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                        npc.velocity = npc.DirectionTo(player.Center) * 30;
                    }
                    break;

                case 3: //dashing
                    if (++npc.ai[1] > 30)
                    {
                        npc.ai[1] = 0;
                        if (++npc.ai[2] > 3) //repeat three times
                        {
                            npc.ai[0]++;
                            npc.ai[2] = 0;
                        }
                        else
                            npc.ai[0]--;
                        npc.netUpdate = true;
                    }
                    break;

                case 4: //prepare for queen bee dashes
                    if (!AliveCheck(player))
                        break;
                    if (++npc.ai[1] > 30)
                    {
                        targetPos = player.Center;
                        targetPos.X += 900 * (npc.Center.X < targetPos.X ? -1 : 1);
                        Movement(targetPos, 0.8f);
                        if (npc.ai[1] > 180 || Math.Abs(npc.Center.Y - targetPos.Y) < 50) //initiate dash
                        {
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.netUpdate = true;
                            npc.velocity.X = -40 * (npc.Center.X < targetPos.X ? -1 : 1);
                            npc.velocity.Y *= 0.1f;
                        }
                    }
                    else
                    {
                        npc.velocity *= 0.9f; //decelerate briefly
                    }
                    break;

                case 5: //dashing, leave trail of vertical deathrays
                    if (npc.ai[3] == 0 && ++npc.ai[2] > 5) //spawn rays on first dash only
                    {
                        npc.ai[2] = 0;
                        if (Main.netMode != 1)
                            Main.NewText("spawn vertical rays");
                    }
                    if (++npc.ai[1] > 240 || (Math.Sign(npc.velocity.X) > 0 ? npc.Center.X > player.Center.X + 900 : npc.Center.X < player.Center.X - 900))
                    {
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        if (++npc.ai[3] > 3) //repeat dash three times
                        {
                            npc.ai[0]++;
                            npc.ai[3] = 0;
                        }
                        else
                            npc.ai[0]--;
                        npc.netUpdate = true;
                    }
                    break;

                case 6: //fly over player, spit mega balls
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center;
                    targetPos.X += 600 * (npc.Center.X < targetPos.X ? -1 : 1);
                    targetPos.Y -= 200;
                    Movement(targetPos, 0.5f);
                    if (++npc.ai[2] > 60)
                    {
                        npc.ai[2] = 0;
                        npc.netUpdate = true;
                        if (Main.netMode != 1)
                            Main.NewText("spit mega ball");
                    }
                    if (++npc.ai[1] > 210)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 7: //prepare for fishron dash
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center + player.DirectionTo(npc.Center) * 600;
                    Movement(targetPos, 0.8f);
                    if (++npc.ai[1] > 30)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                        npc.velocity = npc.DirectionTo(player.Center) * 30;
                    }
                    break;

                case 8: //dashing
                    if (++npc.ai[2] > 6)
                    {
                        npc.ai[2] = 0;
                        if (Main.netMode != 1)
                            Main.NewText("spawn dash fireballs");
                    }
                    if (++npc.ai[1] > 30)
                    {
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        if (++npc.ai[3] > 3) //dash three times
                        {
                            npc.ai[0]++;
                            npc.ai[3] = 0;
                        }
                        else
                            npc.ai[0]--;
                        npc.netUpdate = true;
                    }
                    break;

                case 9: //fly up, prepare to spit mega homing and dash
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center;
                    targetPos.X += 600 * (npc.Center.X < targetPos.X ? -1 : 1);
                    targetPos.Y -= 600;
                    Movement(targetPos, 0.8f);
                    if (++npc.ai[1] > 180 || npc.Distance(targetPos) < 50)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                        npc.velocity.X = -40 * (npc.Center.X < targetPos.X ? -1 : 1);
                        npc.velocity.Y *= 0.1f;
                        if (Main.netMode != 1)
                            Main.NewText("spawn mega homing ball");
                    }
                    break;

                case 10: //dashing
                    npc.velocity *= 0.99f;
                    if (npc.ai[1] > 30)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 11: //hover nearby, shoot lightning
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center;
                    targetPos.X += 700 * (npc.Center.X < targetPos.X ? -1 : 1);
                    Movement(targetPos, 0.5f);
                    if (++npc.ai[2] > 60)
                    {
                        npc.ai[2] = 0;
                        if (Main.netMode != 1)
                            Main.NewText("spawn lightning");
                    }
                    if (++npc.ai[1] > 300)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = npc.Distance(player.Center);
                        npc.netUpdate = true;
                        npc.velocity = npc.DirectionTo(player.Center).RotatedBy(Math.PI / 2) * 30;
                    }
                    break;

                case 12: //fly in jumbo circle
                    npc.velocity += npc.velocity.RotatedBy(Math.PI / 2) * npc.velocity.Length() / npc.ai[3];
                    if (++npc.ai[2] > 6)
                    {
                        npc.ai[2] = 0;
                        if (Main.netMode != 1)
                            Main.NewText("spawn perpendicular thingies");
                    }
                    if (++npc.ai[1] > 180)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                    }
                    break;

                default:
                    npc.ai[0] = 0;
                    goto case 0;
            }
        }

        private bool AliveCheck(Player player)
        {
            if ((!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 5000f) && npc.localAI[3] > 0)
            {
                npc.TargetClosest();
                if (!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 5000f)
                {
                    if (npc.timeLeft > 60)
                        npc.timeLeft = 60;
                    npc.velocity.Y -= 1f;
                    return false;
                }
            }
            if (npc.timeLeft < 600)
                npc.timeLeft = 600;
            return true;
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

        public bool Health9 = false;
        public bool Health8 = false;
        public bool Health7 = false;
        public bool Health6 = false;
        public bool HealthOneHalf = false;

        public override void HitEffect(int hitDirection, double damage)
        {
            base.HitEffect(hitDirection, damage);
            if (npc.life <= npc.lifeMax * 0.9f && !Health9)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != 1) BaseUtility.Chat("I must say, child. You impress me.", Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != 1) BaseUtility.Chat("Face it, child! You’ll never defeat the living embodiment of disarray itself!", Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health9 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.8f && !Health8)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != 1) BaseUtility.Chat("You fight, even when the odds are stacked against you.", Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != 1) BaseUtility.Chat("You’re still going? How amusing...", Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health8 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.7f && !Health7)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != 1) BaseUtility.Chat("You remind me of myself quite a bit, to be honest...", Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != 1) BaseUtility.Chat("Putting up a fight when you know Death is inevitable...", Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health7 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.6f && !Health6)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != 1) BaseUtility.Chat("Maybe some day, you'll have your own realm to rule over...", Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != 1) BaseUtility.Chat("Now stop making this hard! Stand still and take it like a man!", Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health6 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.4f && !Health4)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != 1) BaseUtility.Chat("But today, we clash! Now show me what you got!", Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != 1) BaseUtility.Chat("DIE ALREADY YOU INSIGNIFICANT LITTLE WORM!!", Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health4 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.3f && !Health3)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != 1) BaseUtility.Chat("Still got it? I'm impressed. Show me your true power!", Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != 1) BaseUtility.Chat("WHAT?! HOW HAVE YOU- ENOUGH! YOU WILL KNOW WHAT IT MEANS TO FEEL UNYIELDING CHAOS!", Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health3 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.2f && !Health2)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != 1) BaseUtility.Chat("Come on! KEEP PUSHING!", Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != 1) BaseUtility.Chat("NO! I WILL NOT LOSE! NOT TO YOU!", Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health2 = true;
                npc.netUpdate = true;
            }
            if (npc.life <= npc.lifeMax * 0.1f && !Health1)
            {
                if (AAWorld.downedShen)
                {
                    if (Main.netMode != 1) BaseUtility.Chat("SHOW ME! SHOW ME THE TRUE POWER YOU HOLD!", Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                else
                {
                    if (Main.netMode != 1) BaseUtility.Chat("GRAAAAAAAAAH!!!", Color.DarkMagenta.R, Color.DarkMagenta.G, Color.DarkMagenta.B);
                }
                Health1 = true;
                npc.netUpdate = true;
            }
            if (Health2)
            {
                music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/LastStand");
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color drawColor)
        {
            Texture2D currentTex = Main.npcTexture[npc.type];
            Texture2D currentWingTex1 = mod.GetTexture("NPCs/Bosses/Shen/ShenWingBack");
            Texture2D currentWingTex2 = mod.GetTexture("NPCs/Bosses/Shen/ShenWingFront");
            Texture2D glowTex = mod.GetTexture("NPCs/Bosses/Shen/ShenA_Glow");

            //offset
            npc.position.Y += 130f;

            //draw body/charge afterimage
            BaseDrawing.DrawTexture(sb, currentWingTex1, 0, npc.position + new Vector2(0, npc.gfxOffY), npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 5, wingFrame, drawColor);
            if (Charging)
            {
                BaseDrawing.DrawAfterimage(sb, currentTex, 0, npc, 1.5f, 1f, 3, false, 0f, 0f, new Color(drawColor.R, drawColor.G, drawColor.B, 150));
            }
            BaseDrawing.DrawTexture(sb, currentTex, 0, npc, drawColor);

            //draw glow/glow afterimage
            BaseDrawing.DrawTexture(sb, glowTex, 0, npc, AAColor.Shen3);
            BaseDrawing.DrawAfterimage(sb, glowTex, 0, npc, 0.3f, 1f, 8, false, 0f, 0f, AAColor.Shen3);

            //draw wings
            BaseDrawing.DrawTexture(sb, currentWingTex2, 0, npc.position + new Vector2(0, npc.gfxOffY), npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 5, wingFrame, drawColor);

            //deoffset
            npc.position.Y -= 130f; // offsetVec;			

            return false;
        }
    }

}
