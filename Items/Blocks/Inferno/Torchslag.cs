namespace AAMod.Items.Blocks.Inferno
{
    public class Torchslag : BaseAAItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Torchslag");
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.rare = 1;
            item.consumable = true;
            item.createTile = mod.TileType("Torchslag");
        }
    }
}
