﻿namespace AAMod.Items.Misc
{
    public class GlitchFish : BaseAAItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glitch Fish");
        }
        public override void SetDefaults()
        {
            item.questItem = true;
            item.maxStack = 1;
            item.width = 26;
            item.height = 26;
            item.uniqueStack = true;
            item.rare = -11; 
        }

        public override bool IsQuestFish()
        {
            return true;
        }

        public override bool IsAnglerQuestAvailable()
        {
            return true;
        }

        public override void AnglerQuestChat(ref string description, ref string catchLocation)
        {
            description = Lang.questFish("GlitchFish");
            catchLocation = Lang.questFish("GlitchFishLocation");
        }
    }
}