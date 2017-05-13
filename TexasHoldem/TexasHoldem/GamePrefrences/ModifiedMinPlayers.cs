using System;

namespace TexasHoldem.GamePrefrences
{
    public class ModifiedMinPlayers : PrefrencesDecorator
    {
        public ModifiedMinPlayers(int mp, IPreferences gp)
        {
            ModifiedPrefrences = gp;
            SetMinPlayers(mp);
        }

        public sealed override void SetMinPlayers(int mp)
        {
            ModifiedPrefrences.SetMinPlayers(mp);
        }
    }
}
