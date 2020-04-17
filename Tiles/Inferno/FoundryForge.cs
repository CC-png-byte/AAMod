using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace AAMod.Tiles.Inferno
{
	public class FoundryForge : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.addTile(Type);
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ancient Spear Rack");
			dustType = ModContent.DustType<Dusts.RazewoodDust>();
			AddMapEntry(new Color(70, 40, 30));
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = Glow(Color.DarkRed).R / 2;
			g = Glow(Color.DarkRed).G / 2;
			b = Glow(Color.DarkRed).B / 2;
		}

		public Color Glow(Color color)
		{
			return BaseUtility.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Transparent, Color.Transparent, Color.DarkRed, Color.Transparent, Color.Transparent);
		}

		public override void PostDraw(int x, int y, SpriteBatch sb)
		{
			Tile tile = Main.tile[x, y];
			Texture2D glowTex = mod.GetTexture("Glowmasks/FoundryForge_Glow");
			int frameY = tile != null && tile.active() ? tile.frameY + (Main.tileFrame[Type] * 54) : 0;

			BaseDrawing.DrawTileTexture(sb, glowTex, x, y, 16, 16, tile.frameX, frameY, false, false, false, null, Glow);
		}
	}
}