namespace TexasHoldem.Bridges
{
    public interface IBridge
    {
        bool register(string s1, string s2);
        bool isUserExist(string s);
        bool deleteUser(string s);
        bool login(string s1, string s2);
        bool isLoggedIn(string s);
        bool logOut(string s);
        bool editUserName(string s);
        bool editPassword(string s);
        bool editAvatar(string s);
    }
}