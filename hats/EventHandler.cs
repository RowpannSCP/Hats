using Exiled.Events.EventArgs;
using hats.Components;
using MapEditorReborn.API.Features.Objects;

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

        public void OnLeave(LeftEventArgs ev)
        {
            if(ev.Player.GameObject.TryGetComponent<HatComponent>(out _))
                ev.Player.RemoveHat();
        }
    }
}