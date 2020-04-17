using Terraria.ID;

namespace AAMod.Items.Loot.Inferno
{
    public class AncientStonebreaker : BaseAAItem
    {
        public override void SetDefaults()
        {
            item.damage = 15;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useAnimation = 30;
            item.useTime = 10;
            item.pick = 110;
            item.useStyle = 1;
            item.knockBack = 1;
            item.value = Terraria.Item.sellPrice(0, 1, 8, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTurn = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Stonebreaker");
        }
    }
}
