using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Walls
{
    public class KeepWall : ModWall
	{
		public override void SetDefaults()
		{
            Main.wallHouse[Type] = true;
            dustType = DustID.Stone;
            AddMapEntry(new Color(25, 30, 25));
		}

        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

        public override void KillWall(int i, int j, ref bool fail)
        {
            fail = true;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}