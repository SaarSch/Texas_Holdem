
namespace TexasHoldem.GamePrefrences
{
    public class ModifiedBuyInPolicy : PrefrencesDecorator
    {

        public ModifiedBuyInPolicy(int bp, IPreferences gp)
        {
            ModifiedPrefrences = gp;
            SetBuyInPolicy(bp);
        }

        public sealed override void SetBuyInPolicy(int bp)
        {
            ModifiedPrefrences.SetBuyInPolicy(bp);
        }
    }
}
