using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Raider
{
    [AutoloadBossHead]
    public class RaidEgg : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Raider Egg");

        }
        public override void SetDefaults()
        {
            npc.width = 34;
            npc.height = 34;
            npc.aiStyle = 0;
            npc.damage = 0;
            npc.defense = 30;
            npc.lavaImmune = true;
            npc.lifeMax = 50;
            npc.HitSound = new LegacySoundStyle(3, 4, Terraria.Audio.SoundType.Sound);
            npc.DeathSound = new LegacySoundStyle(4, 14, Terraria.Audio.SoundType.Sound);
            npc.value = 0f;
            npc.knockBackResist = 2f;
            npc.npcSlots = 0f;
        }

        public Color color;

        public override void NPCLoot()
        {
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/RaidEggGore1"), 1f);
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/RaidEggGore2"), 1f);
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {

            Texture2D glowTex = mod.GetTexture("Glowmasks/RaidEgg_Glow");
            color = BaseUtility.MultiLerpColor(Main.player[Main.myPlayer].miscCounter % 100 / 100f, BaseDrawing.GetLightColor(npc.position), BaseDrawing.GetLightColor(npc.position), Color.Violet, BaseDrawing.GetLightColor(npc.position), Color.Violet, BaseDrawing.GetLightColor(npc.position));
            BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc, dColor, true);
            BaseDrawing.DrawTexture(spritebatch, glowTex, 0, npc, color, true);
            return false;
        }
        
        public override void AI()
        {
            if (npc.velocity.Y == 0f)
            {
                npc.velocity.X = npc.velocity.X * 0.9f;
                npc.rotation += npc.velocity.X * 0.02f;
            }
            else
            {
                npc.velocity.X = npc.velocity.X * 0.99f;
                npc.rotation += npc.velocity.X * 0.04f;
            }
            int num1326 = 900;
            if (Main.expertMode)
            {
                num1326 = 600;
            }
            if (npc.justHit)
            {
                npc.ai[0] -= Main.rand.Next(10, 21);
                if (!Main.expertMode)
                {
                    npc.ai[0] -= Main.rand.Next(10, 21);
                }
            }
            npc.ai[0] += 1f;
            if (npc.ai[0] >= num1326 || npc.velocity.Y == 0)
            {
                Projectile.NewProjectile((int)npc.position.X, (int)npc.position.Y, 0, 0, mod.ProjectileType<RaidStrike>(), 30, 10, Main.myPlayer, 0, 0);
                npc.Transform(mod.NPCType("Raidmini"));
            }
            if (Main.netMode != 1 && npc.velocity.Y == 0f && Math.Abs(npc.velocity.X) < 0.2 && npc.ai[0] >= num1326 * 0.75)
            {
                float num1327 = npc.ai[0] - (num1326 * 0.75f);
                num1327 /= num1326 * 0.25f;
                if (Main.rand.Next(-10, 120) < num1327 * 100f)
                {
                    npc.velocity.Y = npc.velocity.Y - (Main.rand.Next(20, 40) * 0.025f);
                    npc.velocity.X = npc.velocity.X + (Main.rand.Next(-20, 20) * 0.025f);
                    npc.velocity *= 1f + (num1327 * 2f);
                    npc.netUpdate = true;
                    return;
                }
            }
        }
    }
}