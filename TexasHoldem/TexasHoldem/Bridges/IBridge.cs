using System;
using System.Collections;
using System.Collections.Generic;

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
        bool editUsername(string username, string newName);
        bool editPassword(string username, string newPass);
        bool editAvatar(string username, string newPath);
        bool createNewGame(string gameName, string username, string creatorName, string gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayer, Boolean spectating);
        bool isGameExist(string gameName);
        bool joinGame(string username, string roomName, string playerName);
        bool spectateGame(string username, string roomName, string playerName);
        bool leaveGame(string username, string roomName, string playerName);

        IList findGames(string username, string playerName, bool playerFlag, int potSize, bool potFlag,
            Gametype gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayers,
            bool spectating, bool prefFlag, bool leagueFlag);
        IList findGames(string username);
        IList getAllGameReplays();
        int getRank(string username);
        bool raiseInGame(int raiseamount, string gamename, string playername);
        bool callInGame(string gamename, string playername);
        bool foldInGame(string goodGameName, string legalPlayer);

        bool setExpCriteria(string username, int exp);
        bool setDefaultRank(string username, int rank);
        bool setUserLeague(string username, string usernameToSet, int rank);

        bool restartGameCenter();
    }
}