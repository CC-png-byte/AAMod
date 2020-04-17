using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace AAMod.Tiles.Inferno
{
	public class AncientStonebreaker : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.addTile(Type);
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ancient Stonebreaker");
			drop = ModContent.ItemType<Items.Loot.Inferno.AncientStonebreaker>();
			dustType = ModContent.DustType<Dusts.RazewoodDust>();
			AddMapEntry(new Color(70, 40, 30));
        }
	}
}