using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.Walls.Bricks
{
    public class EventideBricks : ModWall
	{
		public override void SetDefaults()
        {
            Main.wallLight[Type] = true;
            dustType = mod.DustType("AbyssiumDust");
			AddMapEntry(new Color(33, 37, 96));
            soundType = 21;
            drop = mod.ItemType("DoomsdayWall");
            Main.wallHouse[Type] = true;
        }

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = tile.frameY == 36 ? 18 : 16;
            BaseMod.BaseDrawing.DrawWallTexture(spriteBatch, mod.GetTexture("Glowmasks/DoomsdayWall_Glow"), i, j, false, AAGlobalTile.GetZeroColorDim);
        }
    }
}