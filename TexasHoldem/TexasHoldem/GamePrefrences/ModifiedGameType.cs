namespace TexasHoldem.GamePrefrences
{
    public class ModifiedGameType : PrefrencesDecorator
    {
        public ModifiedGameType(Gametype gt, IPreferences gp)
        {
            ModifiedPrefrences = gp;
            SetGameType(gt);
        }

        public sealed override void SetGameType(Gametype gt)
        {
            ModifiedPrefrences.SetGameType(gt);
        }
    }
}
