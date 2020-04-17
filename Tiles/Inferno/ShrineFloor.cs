using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.Tiles.Inferno
{
    public class ShrineFloor : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;
            Terraria.ID.TileID.Sets.Conversion.Stone[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            soundType = 21;
            dustType = mod.DustType("RazewoodDust");
            drop = mod.ItemType("Torchstone");
            AddMapEntry(new Color(54, 54, 68));
            minPick = 65;
        }
    }
}