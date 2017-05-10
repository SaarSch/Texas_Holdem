using TexasHoldem.GameReplay;

namespace TexasHoldem.Services
{
    public class ReplayManager
    {
        private readonly GameCenter gameCenter;

        public ReplayManager()
        {
            gameCenter = GameCenter.GetGameCenter();
        }

        public string ReplayGame(string roomName) // UC 9
        {
            return gameCenter.GetReplayFilename(roomName);
        }

        public string SaveTurn(string roomName, int turnNum) // UC 10
        {
            return Replayer.SaveTurn(gameCenter.GetReplayFilename(roomName), turnNum);
        }
    }
}
