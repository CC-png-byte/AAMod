using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace AAMod.Tiles.Inferno
{
    public class InfernoGrassWall1 : ModWall
	{
		public override void SetDefaults()
		{
			dustType = mod.DustType("RazeleafDust");
			AddMapEntry(new Color(200, 150, 0));
            Terraria.ID.WallID.Sets.Conversion.Grass[Type] = true;
			drop = mod.ItemType("InfernoGrassWall");
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
    }
}