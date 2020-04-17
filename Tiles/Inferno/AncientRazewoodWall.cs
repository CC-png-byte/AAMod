using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace AAMod.Tiles.Inferno
{
    public class AncientRazewoodWall : ModWall
	{
		public override void SetDefaults()
		{
            dustType = mod.DustType("AshDust");
			AddMapEntry(new Color(50, 25, 0));
		}

        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
    }
}