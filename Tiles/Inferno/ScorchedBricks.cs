using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.Tiles.Inferno
{
    class ScorchedBricks : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = true;
            drop = mod.ItemType("ScorchedBricks");   
            AddMapEntry(new Color(48, 51, 68));
            dustType = ModContent.DustType<Dusts.IncineriteDust>();
        }
    }
}
