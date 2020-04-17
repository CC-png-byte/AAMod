using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Items.Pets
{
    /// <summary>
    /// ALPHA THIS IS NOT AN ITEM
    /// </summary>
    public class Lunamini : ModProjectile
    {
        
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunamini"); // Automatic from .lang files
			Main.projFrames[projectile.type] = 4;
			Main.projPet[projectile.type] = true;
        }

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.BabyHornet);
			aiType = ProjectileID.BabyHornet;
            projectile.width = 48;
            projectile.height = 42;
            
        }

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.hornet = false; // Relic from aiType
			return true;
		}


        public override void AI()
		{
			Player player = Main.player[projectile.owner];
			AAPlayer modPlayer = player.GetModPlayer<AAPlayer>();
			if (player.dead)
			{
				modPlayer.Lunamini = false;
			}
			if (modPlayer.Lunamini)
			{
				projectile.timeLeft = 2;
			}
        }
	}
}