namespace AAMod.Items.Blocks.Inferno
{
    public class ShatteredMirror : BaseAAItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shattered Mirror");
			Tooltip.SetDefault("It's dusty. Must have been broken for years.");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 28;
            item.value = 500;
            item.maxStack = 99;
            item.useStyle = 1;
			item.useTime = 10;
            item.useAnimation = 15;
            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;
			item.createTile = mod.TileType("ShatteredMirror");
		}
	}
}