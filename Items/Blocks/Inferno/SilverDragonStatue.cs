namespace AAMod.Items.Blocks.Inferno
{
    public class SilverDragonStatue : BaseAAItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Silver Dragon Statue");
			Tooltip.SetDefault("You can see your reflection. It's been very well taken care of.");
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
			item.createTile = mod.TileType("SilverDragonStatue");
		}
	}
}