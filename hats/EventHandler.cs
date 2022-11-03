using Exiled.Events.EventArgs;
using hats.Components;
using MapEditorReborn.API.Features.Objects;

namespace hats
{
    using Exiled.API.Enums;
    using Exiled.API.Features;

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
            if (ev.Handler.Type == DamageType.Unknown)
                return;
            if(ev.Target.GameObject.TryGetComponent<HatComponent>(out _))
                ev.Target.RemoveHat();
        }
        
        public void OnLeave(LeftEventArgs ev)
        {
            if(ev.Player.GameObject.TryGetComponent<HatComponent>(out _))
                ev.Player.RemoveHat();
        }
        
        public void UsedItem(UsedItemEventArgs ev)
        {
            if(!cfg.RemoveHatWhenUsing268)
                return;
            if (ev.Item.Type != ItemType.SCP268)
                return;
            if (ev.Player.GameObject.TryGetComponent<HatComponent>(out _))
            {
                ev.Player.RemoveHat();
                ev.Player.ShowHint("Removed hat since you used 268.");
            }
        }

        public void Spawned(SpawnedEventArgs ev)
        {
            if (!cfg.EnableAutoGiveHat)
                return;
            if (!cfg.RolesWithHats.ContainsKey(ev.Player.Role.Type))
                return;
            if(!API.Hats.ContainsKey(cfg.RolesWithHats[ev.Player.Role.Type]))
            {
                Log.Warn($"Could not find hat: {cfg.RolesWithHats[ev.Player.Role.Type]} for role: {ev.Player.Role.Type}");
                return;
            }
            ev.Player.AddHat(API.Hats[cfg.RolesWithHats[ev.Player.Role.Type]]);
        }
    }
}