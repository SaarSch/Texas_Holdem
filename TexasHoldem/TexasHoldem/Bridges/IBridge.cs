using System.Collections;

namespace TexasHoldem.Bridges
{
    public interface IBridge
    {
        bool register(string username, string pass);
        bool isUserExist(string username);
        bool deleteUser(string username, string password);
        bool login(string username, string pass);
        bool isLoggedIn(string username, string pass);
        bool logOut(string username);
        bool editUsername(string username, string password, string newName);
        bool editPassword(string username, string password, string newPass);
        bool editAvatar(string username, string password, string newPath);
        bool createNewGame(string gameName, int numOfPlayers);
        bool isGameExist(string gameName);
        ArrayList getActiveGames(int rank);
        bool joinGame(object activeGame);
        ArrayList getActiveGames();
        bool SpectateGame(object activeGame);
        bool leaveGame(string goodGameName);
        ArrayList getAllGamesReplay();
        int getRank(string username);
        void setRank(string gameName, int rank);
    }
}