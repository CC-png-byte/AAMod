using Terraria.ModLoader;
namespace AAMod.NPCs.Bosses.Hydra
{
    [AutoloadBossHead]
    public class HydraHead2 : HydraHead1
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            leftHead = false;
			middleHead = false;
			distFromBodyX = 80;
			distFromBodyY = 90;
        }
    }
}