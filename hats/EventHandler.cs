using hats.Components;

namespace hats
{
    using System;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs.Player;
    using MapEditorReborn.API.Extensions;
    using PlayerRoles;

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
            if (!cfg.RemoveHatOnDeath)
                return;
            if (ev.Player.GameObject.TryGetComponent<HatComponent>(out _))
                ev.Player.RemoveHat();
        }

        public void OnLeave(LeftEventArgs ev)
        {
            if (ev.Player.GameObject.TryGetComponent<HatComponent>(out _))
                ev.Player.RemoveHat();
        }

        public void UsedItem(UsedItemEventArgs ev)
        {
            if (!cfg.RemoveHatWhenUsing268)
                return;
            if (ev.Item.Type != ItemType.SCP268)
                return;
            try
            {
                ev.Player.RemoveHat();
                ev.Player.ShowHint("Removed hat since you used 268.");
            }
            catch (ArgumentException) {}
        }

        public void Spawned(SpawnedEventArgs ev)
        {
            if (!cfg.EnableAutoGiveHat)
                return;
            if (!cfg.RolesWithHats.TryGetValue(ev.Player.Role.Type, out var hatName))
                return;

            if (!API.Hats.TryGetValue(hatName, out var hat))
            {
                Log.Warn($"Could not find hat: {hatName} for role: {ev.Player.Role.Type}");
                return;
            }
 
            ev.Player.AddHat(hat);
        }

        public void OnSpectate(ChangingSpectatedPlayerEventArgs ev)
        {
            if (ev.OldTarget != null && ev.OldTarget.GameObject.TryGetComponent(out HatComponent hat))
            {
                if (hat.Hat.ShowHatToOtherSpectators)
                {
                    ev.Player.SpawnSchematic(hat.Schematic);
                }
                else
                {
                    if (!hat.Hat.ShowHatToOwnerSpectators)
                        ev.Player.DestroySchematic(hat.Schematic);
                }
            }

            if (ev.NewTarget != null && ev.NewTarget.GameObject.TryGetComponent(out hat))
            {
                if (hat.Hat.ShowHatToOwnerSpectators)
                {
                    ev.Player.SpawnSchematic(hat.Schematic);
                }
                else
                {
                    if (!hat.Hat.ShowHatToOtherSpectators)
                        ev.Player.DestroySchematic(hat.Schematic);
                }
            }
        }
    }
}