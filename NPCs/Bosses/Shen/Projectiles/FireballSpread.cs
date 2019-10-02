using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Shen.Projectiles
{
    public class FireballSpreadR : ModProjectile
    {
        public override string Texture => "AAMod/NPCs/Bosses/Shen/Projectiles/FireballSpreadR";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fireball");
            Main.projFrames[projectile.type] = 4;
        }

        public override void PostAI()
        {
            if (projectile.frameCounter++ > 5)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.hostile = true;
            projectile.timeLeft = 600;
            projectile.aiStyle = -1;
            cooldownSlot = 1;
        }

        public override void AI()
        {
            if (--projectile.ai[0] == 0)
            {
                projectile.netUpdate = true;
                projectile.velocity = Vector2.Zero;
            }
            if (--projectile.ai[1] == 0)
            {
                projectile.netUpdate = true;
                Player target = Main.player[Player.FindClosest(projectile.position, projectile.width, projectile.height)];
                projectile.velocity = projectile.DirectionTo(target.Center + target.velocity * 30) * 30;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType<Buffs.HydraToxin>(), 180);
        }
    }

    public class FireballSpreadB : ModProjectile
    {
        public override string Texture => "AAMod/NPCs/Bosses/Shen/Projectiles/FireballSpreadB";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fireball");
            Main.projFrames[projectile.type] = 4;
        }

        public override void PostAI()
        {
            if (projectile.frameCounter++ > 5)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.hostile = true;
            projectile.timeLeft = 600;
            projectile.aiStyle = -1;
            cooldownSlot = 1;
        }

        public override void AI()
        {
            if (--projectile.ai[0] == 0)
            {
                projectile.netUpdate = true;
                projectile.velocity = Vector2.Zero;
            }
            if (--projectile.ai[1] == 0)
            {
                projectile.netUpdate = true;
                Player target = Main.player[Player.FindClosest(projectile.position, projectile.width, projectile.height)];
                projectile.velocity = projectile.DirectionTo(target.Center + target.velocity * 30) * 30;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType<Buffs.DragonFire>(), 180);
        }
    }
}