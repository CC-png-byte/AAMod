using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.Tiles.Inferno
{
    class AncientRazewood : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileLighted[Type] = false;
            Main.tileBlockLight[Type] = true;
            drop = mod.ItemType("AncientRazewood");   
            AddMapEntry(new Color(80, 60, 20));
            dustType = ModContent.DustType<Dusts.AshRain>();
        }
    }
}
