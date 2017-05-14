using TexasHoldem.GameReplay;

namespace TexasHoldem.Services
{
    public class ReplayManager
    {
        private readonly GameCenter _gameCenter;

        public ReplayManager()
        {
            _gameCenter = GameCenter.GetGameCenter();
        }

        public string ReplayGame(string roomName) // UC 9
        {
            return _gameCenter.GetReplayFilename(roomName);
        }
    }
}
