﻿using Microsoft.Xna.Framework;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace AAMod.Projectiles
{
    // to investigate: Projectile.Damage, (8843)
    public class Stars : ModProjectile
	{
        public override void SetDefaults()
		{
            projectile.width = 26;
            projectile.height = 26;
            projectile.alpha = 30;
            projectile.light = 0.2f;
            projectile.aiStyle = 0;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.tileCollide = false;
            projectile.scale = 0.1f;
            projectile.timeLeft = 300;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            int stardust = mod.DustType<Dusts.StarDust>();
            int dustId = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height + 5, stardust, projectile.velocity.X * 0.2f,
                projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
            Main.dust[dustId].noGravity = true;
            int dustId3 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height + 5, stardust, projectile.velocity.X * 0.2f,
                projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
            Main.dust[dustId3].noGravity = true;

            if (projectile.ai[1] == 0f)
            {
                projectile.rotation += .05f;
                projectile.scale += .05f;
            }
            else
            {
                projectile.rotation -= .05f;
                projectile.scale -= .05f;
            }

            if (projectile.scale > 1f)
            {
                projectile.scale = 1f;
                projectile.ai[1] = 1f;
            }
        }

        public override void Kill(int timeleft)
        {
            int stardust = mod.DustType("StarDust");
            for (int num468 = 0; num468 < 20; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, stardust, -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, default(Color), 1f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
                num469 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, stardust, -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, default(Color));
                Main.dust[num469].velocity *= 2f;
            }
        }
    }
}