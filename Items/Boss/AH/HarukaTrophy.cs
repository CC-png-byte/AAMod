namespace AAMod.Items.Boss.AH
{
    public class HarukaTrophy : BaseAAItem
    {
        public static int type;
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Haruka Trophy");
		}

        public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
            item.rare = 1;
            item.useStyle = 1;
			item.consumable = true;
			item.value = 2000;
			item.rare = 1;
			item.createTile = mod.TileType("HarukaTrophy");
		}
    }
}