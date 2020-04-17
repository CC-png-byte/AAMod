namespace AAMod.Items.Boss.Broodmother
{
    public class BroodScale : BaseAAItem
    {
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.maxStack = 99;
            item.rare = 1;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scorched Scale");
            Tooltip.SetDefault("The scale of a formidable foe");
        }
    }
}
