using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace AAMod.Walls
{
    public class LivingBogwoodWall : ModWall
	{
		public override void SetDefaults()
		{
			dustType = mod.DustType("BogwoodDust");
			AddMapEntry(new Color(100, 0, 30));
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
    }
}