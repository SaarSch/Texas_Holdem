namespace TexasHoldem.GamePrefrences
{
    public interface IPreferences
    {
        int GetBuyInPolicy();
        int GetChipPolicy();
        Gametype GetGameType();
        int GetMaxPlayers();
        int GetMinPlayers();
        int GetMinBet();
        bool GetSpectating();

        void SetBuyInPolicy(int bp);
        void SetChipPolicy(int cp);
        void SetGameType(Gametype gt);
        void SetMaxPlayers(int mp);
        void SetMinPlayers(int mp);
        void SetMinBet(int mb);
        void SetSpectating(bool s);
    }
}