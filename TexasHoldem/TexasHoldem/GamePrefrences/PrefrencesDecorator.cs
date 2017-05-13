
namespace TexasHoldem.GamePrefrences
{
    public abstract class PrefrencesDecorator : IPreferences
    {
        protected IPreferences ModifiedPrefrences;

        public virtual int GetBuyInPolicy()
        {
            return ModifiedPrefrences.GetBuyInPolicy();
        }

        public virtual int GetChipPolicy()
        {
            return ModifiedPrefrences.GetChipPolicy();
        }

        public virtual Gametype GetGameType()
        {
            return ModifiedPrefrences.GetGameType();
        }

        public virtual int GetMaxPlayers()
        {
            return ModifiedPrefrences.GetMaxPlayers();
        }

        public virtual int GetMinPlayers()
        {
            return ModifiedPrefrences.GetMinPlayers();
        }

        public virtual int GetMinBet()
        {
            return ModifiedPrefrences.GetMinBet();
        }

        public virtual bool GetSpectating()
        {
            return ModifiedPrefrences.GetSpectating();
        }

        public virtual void SetBuyInPolicy(int bp)
        {
            ModifiedPrefrences?.SetBuyInPolicy(bp);
        }

        public virtual void SetChipPolicy(int cp)
        {
            ModifiedPrefrences?.SetChipPolicy(cp);
        }

        public virtual void SetGameType(Gametype gt)
        {
            ModifiedPrefrences?.SetGameType(gt);
        }

        public virtual void SetMaxPlayers(int mp)
        {
            ModifiedPrefrences?.SetMaxPlayers(mp);
        }

        public virtual void SetMinPlayers(int mp)
        {
            ModifiedPrefrences?.SetMinPlayers(mp);
        }

        public virtual void SetMinBet(int mb)
        {
            ModifiedPrefrences?.SetMinBet(mb);
        }

        public virtual void SetSpectating(bool s)
        {
            ModifiedPrefrences?.SetSpectating(s);
        }
    }
}
