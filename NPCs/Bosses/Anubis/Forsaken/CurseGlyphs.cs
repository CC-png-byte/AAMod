using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;
using BaseMod;

namespace AAMod.NPCs.Bosses.Anubis.Forsaken
{
    public class CurseGlyphs : ModProjectile
	{				
		public override void SetStaticDefaults()
		{
            Main.projFrames[projectile.type] = 9;
		}

        public override void SetDefaults()
        {
            projectile.tileCollide = false;
            projectile.width = 32;
            projectile.height = 40;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
        }

        public int body = -1;
		public float rotValue = -1f;
		public bool spawnedDust = false;

		public override void AI()
        {
            if (body == -1)
            {
                int npcID = BaseAI.GetNPC(projectile.Center, ModContent.NPCType<ForsakenAnubis>(), 400f, null);
                if (npcID >= 0) body = npcID;
            }
            if (body == -1) return;
            NPC anubis = Main.npc[body];
            if (anubis == null || anubis.life <= 0 || !anubis.active || anubis.type != ModContent.NPCType<ForsakenAnubis>()) { projectile.active = false; return; }

            projectile.rotation += .1f;

            int glyph = ((ForsakenAnubis)anubis.modNPC).RuneCount;

            if (rotValue == -1f) rotValue = projectile.ai[0] % glyph * ((float)Math.PI * 2f / glyph);
            rotValue += 0.04f;
            while (rotValue > (float)Math.PI * 2f) rotValue -= (float)Math.PI * 2f;

            projectile.Center = BaseUtility.RotateVector(anubis.Center, anubis.Center + new Vector2(160, 0f), rotValue);

        }

		public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 9, 0, 0);
            BaseDrawing.DrawTexture(sb, Main.npcTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, projectile.direction, 1, frame, projectile.GetAlpha(ColorUtils.COLOR_GLOWPULSE), true);
            return false;
		}		
	}
}