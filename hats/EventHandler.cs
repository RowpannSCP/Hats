namespace hats
{
    public class EventHandler
    {
        private Config cfg;

        public EventHandler(Config cfg) => this.cfg = cfg;

        public void WaitingForPlayers()
        {
            API.LoadHats();
        }
    }
}