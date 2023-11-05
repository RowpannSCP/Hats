using System.Collections.Generic;
using System.Linq;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MapEditorReborn.API.Features.Serializable;
using UnityEngine;

namespace hats
{
    public class Hat
    {
        public virtual string Name => Config.Name;
        public virtual Vector3 Offset => Config.Offset;
        public virtual Vector3 Rotation => Config.Rotation;
        public virtual Vector3 Scale => Config.Scale;
        public virtual bool MakePlayerInvisible => Config.MakePlayerInvisible;
        public virtual bool ShowHatToOwner => Config.ShowHatToOwner;
        public virtual bool ShowHatToOwnerSpectators => Config.ShowHatToOwnerSpectators;
        public virtual bool ShowHatToOtherSpectators => Config.ShowHatToOtherSpectators;

        public virtual SchematicObjectDataList Schematic { get; }
        public virtual HatConfig Config { get; }

        public virtual SchematicObject SpawnHat(Vector3 position, Quaternion? rotation = null, Vector3? scale = null)
        {
            return ObjectSpawner.SpawnSchematic(Name, position, rotation, scale, Schematic);
        }

        public virtual void DestroyInstances()
        {
            var toRemove = new List<string>();

            foreach (var kvp in Plugin.Singleton.HatWearers
                         .Where(x => x.Value.Hat == this))
            {
                if (kvp.Value is null  || kvp.Value.Schematic.gameObject == null || !kvp.Value.Schematic.isActiveAndEnabled)
                {
                    toRemove.Add(kvp.Key);
                    continue;
                }

                kvp.Value.DoDestroy();
                toRemove.Add(kvp.Key);
            }

            foreach (var key in toRemove)
            {
                Plugin.Singleton.HatWearers.Remove(key);
            }
        }

        public Hat(HatConfig config, SchematicObjectDataList data)
        {
            Config = config;
            Schematic = data;
        }
    }
}