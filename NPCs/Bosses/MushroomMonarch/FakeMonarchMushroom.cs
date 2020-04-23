﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace AAMod.NPCs.Bosses.MushroomMonarch
{
    internal class FakeMonarchMushroom : ModProjectile
    {
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushroom");
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 24;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 14400;
        }

        private bool isGrabbing = false;

        public override bool OnTileCollide(Vector2 oldVelocity)
		{
            if(isGrabbing)
            {
                projectile.velocity.X = 0f;
                projectile.velocity.Y = 0f;
            }
			return false;
		}

        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<MushroomMonarch>()))
            {
                projectile.Kill();
                return;
            }

            float num = 0.1f;
            float num2 = 7f;

            projectile.velocity.Y = projectile.velocity.Y + num;
            if (projectile.velocity.Y > num2)
            {
                projectile.velocity.Y = num2;
            }
            projectile.velocity.X = projectile.velocity.X * 0.95f;
            if ((double)projectile.velocity.X < 0.1 && (double)projectile.velocity.X > -0.1)
            {
                projectile.velocity.X = 0f;
            }

            Vector2 tile = new Vector2(projectile.Center.X, projectile.Center.Y + projectile.height / 2);
            bool tileCheck = TileID.Sets.Platforms[Main.tile[(int)(tile.X / 16), (int)(tile.Y / 16)].type];
            if (tileCheck) 
            {
                projectile.velocity.X = 0f;
                projectile.velocity.Y = 0f;
            }

            for(int i = 0; i < 200; i++)
            {
                if(Main.player[i].active && (Main.player[i].Center - projectile.Center).Length() < 88)
                {
                    if ((double)Main.player[i].position.X + (double)Main.player[i].width * 0.5 > (double)projectile.position.X + (double)projectile.width * 0.5)
                    {
                        if (projectile.velocity.X < 4f + Main.player[i].velocity.X)
                        {
                            projectile.velocity.X = projectile.velocity.X + 0.45f;
                        }
                        if (projectile.velocity.X < 0f)
                        {
                            projectile.velocity.X = projectile.velocity.X + 0.45f * 0.75f;
                        }
                    }
                    else
                    {
                        if (projectile.velocity.X > -4f + Main.player[i].velocity.X)
                        {
                            projectile.velocity.X = projectile.velocity.X - 0.45f;
                        }
                        if (projectile.velocity.X > 0f)
                        {
                            projectile.velocity.X = projectile.velocity.X - 0.45f * 0.75f;
                        }
                    }
                    if ((double)Main.player[i].position.Y + (double)Main.player[i].height * 0.5 > (double)projectile.position.Y + (double)projectile.height * 0.5)
                    {
                        if (projectile.velocity.Y < 4f)
                        {
                            projectile.velocity.Y = projectile.velocity.Y + 0.45f;
                        }
                        if (projectile.velocity.Y < 0f)
                        {
                            projectile.velocity.Y = projectile.velocity.Y + 0.45f * 0.75f;
                        }
                    }
                    else
                    {
                        if (projectile.velocity.Y > -4f)
                        {
                            projectile.velocity.Y = projectile.velocity.Y - 0.45f;
                        }
                        if (projectile.velocity.Y > 0f)
                        {
                            projectile.velocity.Y = projectile.velocity.Y - 0.45f * 0.75f;
                        }
                    }
                    isGrabbing = true;
                }

                if(Main.player[i].active && (Main.player[i].Center - projectile.Center).Length() < 10)
                {
                    Main.PlaySound(SoundID.Item6, projectile.position);
                    Main.player[i].HealEffect(-15, false);
                    Main.player[i].statLife -= 15;
                    NetMessage.SendData(66, -1, -1, null, i, -15, 0f, 0f, 0, 0, 0);
                    projectile.Kill();
                }
            }
            
        }
    }
}