namespace TexasHoldem.GamePrefrences
{
    public class ModifiedSpectating : PrefrencesDecorator
    {
        public ModifiedSpectating(bool s, IPreferences gp)
        {
            ModifiedPrefrences = gp;
            SetSpectating(s);
        }

        public sealed override void SetSpectating(bool s)
        {
            ModifiedPrefrences.SetSpectating(s);
        }
    }
}
