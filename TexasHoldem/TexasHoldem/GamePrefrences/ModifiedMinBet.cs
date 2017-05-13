
namespace TexasHoldem.GamePrefrences
{
    public class ModifiedMinBet : PrefrencesDecorator
    {
        public ModifiedMinBet(int mb, IPreferences gp)
        {
            ModifiedPrefrences = gp;
            SetMinBet(mb);
        }

        public sealed override void SetMinBet(int mb)
        {
            ModifiedPrefrences.SetMinBet(mb);
        }
    }
}
