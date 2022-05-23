using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MapEditorReborn.API.Features.Serializable;
using Newtonsoft.Json;
using UnityEngine;

namespace hats
{
    public static class API
    {
        public static Dictionary<string, Hat> Hats { get; private set; } = new Dictionary<string, Hat>();

        public static bool IsHat(this GameObject obj, out Hat hat)
        {
            hat = Hats.FirstOrDefault(x => x.Value.SpawnedHats.Any(x => x.gameObject == obj)).Value;
            if (hat != null)
            {
                return true;
            }
            return false;
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
                if(data != null)
                    Hats.Add(cfg.Name, new Hat(cfg.Name, data, cfg.offset));
            }
        }

        public static void ClearHats()
        {
            if(Hats.Count == 0)
                return;
            foreach (var ply in Player.List.Where(x => x.SessionVariables.ContainsKey("HatWearer")))
            {
                ((SchematicObject)ply.SessionVariables["HatWearer"]).Destroy();
                ply.SessionVariables.Remove("HatWearer");
            }
            foreach (var kvp in Hats)
            {
                kvp.Value.DestroyInstances();
            }
            Hats.Clear();
        }
        
        public static void AddHat(this Player ply, Hat hat)
        {
            if (hat == null)
                throw new ArgumentNullException(nameof(hat));
            var obj = hat.SpawnHat(ply.Position);
            obj.gameObject.transform.parent = ply.GameObject.transform;
            obj.gameObject.transform.localPosition = hat.Offset;
            ply.SessionVariables.Add("HatWearer", obj);
        }

        public static void RemoveHat(this Player ply)
        {
            if (!ply.SessionVariables.ContainsKey("HatWearer"))
            {
                throw new ArgumentException("Player isnt wearing a hat!");
            }
            
            if(ply.SessionVariables["HatWearer"] is SchematicObject obj && obj.gameObject.IsHat(out var hat))
            {
                hat.SpawnedHats.Remove(obj);
                obj.Destroy();
            }
        }
    }
}