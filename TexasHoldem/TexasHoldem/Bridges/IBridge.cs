using System.Collections;
using TexasHoldem.Game;
using TexasHoldem.Services;

namespace TexasHoldem.Bridges
{
    public interface IBridge
    {
        bool Register(string username, string pass);
        bool IsUserExist(string username);
        bool DeleteUser(string username, string password);
        bool Login(string username, string pass);
        bool IsLoggedIn(string username, string pass);
        bool LogOut(string username);
        bool EditUsername(string username, string newName);
        bool EditPassword(string username, string newPass);
        bool EditAvatar(string username, string newPath);
        bool CreateNewGame(string gameName, string username, string creatorName);
        bool CreateNewGameWithPrefrences(string gameName, string username, string creatorName, string gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayer, bool spectating);
        bool IsGameExist(string gameName);
        bool JoinGame(string username, string roomName, string playerName);
        bool SpectateGame(string username, string roomName, string playerName);
        bool LeaveGame(string username, string roomName, string playerName);
        IList FindGames(string username, RoomFilter filter);
        int GetRank(string username);
        bool RaiseInGame(int raiseamount, string gamename, string playername);
        bool CallInGame(string gamename, string playername);
        bool FoldInGame(string goodGameName, string legalPlayer);

        bool SendMessageToEveryone(string roomName, bool isSpectator, string senderPlayerName, string message);
        bool SendWhisper(string roomName, bool isSpectator, string senderPlayerName, string receiverPlayerName, string message);
        IList GetMessages(string roomName, string username);

        bool RestartGameCenter();

        bool StartGame(string roomName);
        bool SetBet(string roomName, string PlayerName, int bet);
    }
}