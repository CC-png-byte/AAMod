using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace AAMod.Tiles.Bars
{
    public class RadiumBarTile : ModTile
    {
        public override void SetDefaults()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = false;
            Main.tileSolidTop[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.CoordinateHeights = new[] { 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            drop = mod.ItemType("RadiumBar");   //put your CustomBlock name
            dustType = ModContent.DustType<Dusts.RadiumDust>();
            AddMapEntry(new Color(200, 170, 100));
			minPick = 0;
        }
    }
}