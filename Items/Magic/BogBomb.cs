using Terraria;
using Terraria.ID;

namespace AAMod.Items.Magic        //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class BogBomb : BaseAAItem
    {
        public override void SetDefaults()
        {

            item.damage = 25;                      
            item.magic = true;  
            item.width = 32;     
            item.height = 28;    
            item.useTime = 20; 
            item.useAnimation = 20; 
            item.useStyle = 5;        
            item.noMelee = true;   
            item.knockBack = 1; 
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 8;   
            item.mana = 9;
            item.UseSound = SoundID.Item1; 
            item.autoReuse = true; 
            item.shoot = mod.ProjectileType("BogBomb");  
            item.shootSpeed = 30f;    
        }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bog Bomb");
			Tooltip.SetDefault("Fires an explosive bomb that inflicts venom upon whatever it strikes");
		}
    }
}
