using Terraria;
using Terraria.ModLoader;

namespace AAMod.Buffs
{
    public class ZeroBab : ModBuff
	{
		public override void SetDefaults()
		{
			// DisplayName and Description are automatically set from the .lang files, but below is how it is done normally.
			DisplayName.SetDefault("ZER0 lite");
			Description.SetDefault("LIFE DETECTION SYSTEMS ENGAGED");
			Main.buffNoTimeDisplay[Type] = true;
			Main.lightPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 1800000;
			player.detectCreature = true;
            player.GetModPlayer<AAPlayer>().ZeroBab = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("ZeroBab")] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + player.width / 2, player.position.Y + player.height / 2, 0f, 0f, mod.ProjectileType("ZeroBab"), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}