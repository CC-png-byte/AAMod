using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace AAMod.Walls
{
    public class Mushwall : ModWall
	{
		public override void SetDefaults()
		{
            Main.wallHouse[Type] = true;
			drop = mod.ItemType("Mushroom Wall");
			AddMapEntry(new Color(60, 14, 14));
            Terraria.ID.WallID.Sets.Conversion.Grass[Type] = true;
        }
    }
}