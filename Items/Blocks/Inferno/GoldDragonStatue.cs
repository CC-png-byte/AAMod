namespace AAMod.Items.Blocks.Inferno
{
    public class GoldDragonStatue : BaseAAItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golden Dragon Statue");
			Tooltip.SetDefault("It's covered in ash. Must not have been cleaned in a while.");
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
			item.createTile = mod.TileType("GoldDragonStatue");
		}
	}
}