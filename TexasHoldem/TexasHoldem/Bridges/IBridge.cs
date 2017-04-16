using System.Collections;

namespace TexasHoldem.Bridges
{
    public interface IBridge
    {
        bool register(string userName, string pass);
        bool isUserExist(string userName);
        bool deleteUser(string userName);
        bool login(string userName, string pass);
        bool isLoggedIn(string userName, string pass);
        bool logOut(string userName);
        bool editUserName(string newName);
        bool editPassword(string pass);
        bool editAvatar(string path);
        bool createNewGame(string gameName, int numOfPlayers);
        bool isGameExist(string gameName);
        ArrayList getActiveGames(int rank);
        bool joinGame(object activeGame);
        ArrayList getActiveGames();
        bool SpectateGame(object activeGame);
        bool leaveGame(string goodGameName);
        ArrayList getAllGamesReplay();
        int getRank(string userName);
        void setRank(string gameName, int rank);
    }
}