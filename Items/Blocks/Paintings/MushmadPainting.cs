using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Items.Blocks.Paintings
{
    public class MushmadPainting : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Madness");
            Tooltip.SetDefault("'So many mushrooms...'");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.rare = ItemRarityID.Blue;
            item.createTile = mod.TileType("MushmadPainting");
        }
    }
}