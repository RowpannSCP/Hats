using Exiled.Events.EventArgs;
using hats.Components;
using MapEditorReborn.API.Features.Objects;

namespace hats
{
    public class EventHandler
    {
        private Config cfg;
        private bool _isLoaded = false;
        
        public EventHandler(Config cfg) => this.cfg = cfg;

        public void WaitingForPlayers()
        {
            if (API.Hats.Count != 0 || _isLoaded)
                return;
            API.LoadHats();
            _isLoaded = true;
        }

        public void Died(DiedEventArgs ev)
        {
            if(!cfg.RemoveHatOnDeath)
                return;
            if(ev.Target.GameObject.TryGetComponent<HatComponent>(out _))
                ev.Target.RemoveHat();
        }
        
        public void OnLeave(LeftEventArgs ev)
        {
            if(ev.Player.GameObject.TryGetComponent<HatComponent>(out _))
                ev.Player.RemoveHat();
        }
    }
}