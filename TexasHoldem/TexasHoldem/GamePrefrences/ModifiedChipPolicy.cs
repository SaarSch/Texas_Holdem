
namespace TexasHoldem.GamePrefrences
{
    public class ModifiedChipPolicy : PrefrencesDecorator
    {

        public ModifiedChipPolicy(int cp, IPreferences gp)
        {
            ModifiedPrefrences = gp;
            SetChipPolicy(cp);
        }

        public sealed override void SetChipPolicy(int cp)
        {
            ModifiedPrefrences.SetChipPolicy(cp);
        }
    }
}
