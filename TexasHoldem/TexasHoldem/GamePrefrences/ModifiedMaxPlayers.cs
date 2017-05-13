
namespace TexasHoldem.GamePrefrences
{
    public class ModifiedMaxPlayers : PrefrencesDecorator
    {
        public ModifiedMaxPlayers(int mp, IPreferences gp)
        {
            ModifiedPrefrences = gp;
            SetMaxPlayers(mp);
        }

        public sealed override void SetMaxPlayers(int mp)
        {
            ModifiedPrefrences.SetMaxPlayers(mp);
        }
    }
}
