using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using hats.Components;
using MapEditorReborn.API.Features;
using UnityEngine;

namespace hats
{
    using Exiled.API.Features.Roles;
    using MapEditorReborn.API.Extensions;
    using MEC;

    public static class API
    {
        public static Dictionary<string, Hat> Hats { get; private set; } = new Dictionary<string, Hat>();

        public static bool IsHat(this GameObject obj, out HatComponent hat)
        {
            hat = Plugin.Singleton.HatWearers.FirstOrDefault(x => x.Value.gameObject.Equals(obj)).Value;
            return hat is not null;
        }

        public static void LoadHats()
        {
            ClearHats();

            if (Plugin.Singleton.Config.Hats.Count == 1 && Plugin.Singleton.Config.Hats[0].Name == "Name")
            {
                Log.Error("Please specify atleast 1 hat, disabling plugin!");
                Plugin.Singleton.Config.IsEnabled = false;
                return;
            }

            foreach (var cfg in Plugin.Singleton.Config.Hats)
            {
                var data = MapUtils.GetSchematicDataByName(cfg.SchematicName);
                if (data != null)
                    Hats.Add(cfg.Name, new Hat(cfg, data));
            }
        }

        public static void ClearHats()
        {
            if (Hats.Count == 0)
                return;

            foreach (var ply in Player.List)
            {
                if (ply.GameObject.TryGetComponent<HatComponent>(out _))
                    ply.RemoveHat();
            }

            foreach (var kvp in Hats)
            {
                try
                {
                    kvp.Value.DestroyInstances();
                }
                catch (ArgumentException e)
                {
                    Log.Error($"Unable to destroy hat: {kvp.Key}! {e}");
                }
            }

            Hats.Clear();
        }
        
        public static void AddHat(this Player ply, Hat hat)
        {
            if (hat == null)
                throw new ArgumentNullException(nameof(hat));
            if (ply.GameObject.TryGetComponent<HatComponent>(out _))
                return;

            var obj = hat.SpawnHat(ply.Position, null, hat.Scale);
            var comp = ply.GameObject.AddComponent<HatComponent>();
            comp.Hat = hat;
            comp.Schematic = obj;

            var gameObject = obj.gameObject;
            gameObject.transform.parent = ply.GameObject.transform;
            gameObject.transform.localPosition = hat.Offset;
            gameObject.transform.localRotation = Quaternion.Euler(hat.Offset);

            if (!hat.ShowHatToOwner && (!Plugin.Singleton.Config.RolesToHideHatFrom.Contains(ply.Role.Type) || Plugin.Singleton.Config.ShowHatToOwnerIfRoleHideHatAndHideHatToOwnerFalse))
            {
                Timing.CallDelayed(1f, () =>
                {
                    ply.DestroySchematic(obj);
                });
            }

            Timing.CallDelayed(1f, () =>
            {
                foreach (var player in Player.List)
                {
                    if (!Plugin.Singleton.Config.RolesToHideHatFrom.Contains(player.Role.Type))
                        continue;
                    if (player == ply)
                        continue;
                    player.DestroySchematic(obj);
                }
            });

            if (hat.MakePlayerInvisible && ply.Role is FpcRole fpcRole)
                fpcRole.IsInvisible = true;

            Plugin.Singleton.HatWearers.Add(ply.UserId, comp);
        }

        public static void RemoveHat(this Player ply)
        {
            if (!ply.GameObject.TryGetComponent(out HatComponent hat))
            {
                throw new ArgumentException("Player isn't wearing a hat!");
            }

            try
            {
                if (hat.Hat.MakePlayerInvisible && ply.Role is FpcRole fpcRole)
                    fpcRole.IsInvisible = false;
                hat.DoDestroy();
            }
            catch (Exception)
            {
                // ignored
            }

            Plugin.Singleton.HatWearers.Remove(ply.UserId);
        }
    }
}