namespace TexasHoldem.Services
{
    public class ReplayFacade
    {
        private readonly GameCenter _gameCenter;

        public ReplayFacade()
        {
            _gameCenter = GameCenter.GetGameCenter();
        }

        public string ReplayGame(string roomName) // UC 9
        {
            return _gameCenter.GetReplayFilename(roomName);
        }
    }
}