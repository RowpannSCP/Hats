using System;
using System.Collections.Generic;
using System.Linq;
using hats.Components;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MapEditorReborn.API.Features.Serializable;
using MapEditorReborn.Commands.Scale;
using UnityEngine;
using Object = UnityEngine.Object;

namespace hats
{
    public class Hat
    {
        public string Name;
        public Vector3 Offset;
        public Quaternion Rotation;
        public Vector3 Scale;
        public SchematicObjectDataList Schematic;
        public bool HideOwner;
        public bool ShowToOwner;

        internal SchematicObject SpawnHat(Vector3 Position, Quaternion? Rotation = null, Vector3? scale = null)
        {
            SchematicObject obj = ObjectSpawner.SpawnSchematic(Name, Position, Rotation, scale ?? Scale, Schematic);
            return obj;
        }

        public void DestroyInstances()
        {
            if (Plugin.Singleton.hats.All(x => x.Value.hat != this))
                return;
            List<KeyValuePair<string, HatComponent>> toRemove = new List<KeyValuePair<string, HatComponent>>();
            foreach (var kvp in Plugin.Singleton.hats.Where(x => x.Value.hat == this))
            {
                if (kvp.Value is null  || kvp.Value.schem.gameObject == null || !kvp.Value.schem.isActiveAndEnabled)
                {
                    toRemove.Add(kvp);
                    continue;
                }
                kvp.Value.DoDestroy();
                toRemove.Add(kvp);
            }

            foreach (var kvp in toRemove)
            {
                Plugin.Singleton.hats.Remove(kvp.Key);
            }
        }

        public Hat(string Name, SchematicObjectDataList data, Vector3 offset, Vector3 rotation, Vector3 scale, bool hideOwner, bool showToOwner)
        {
            this.Name = Name;
            Schematic = data;
            this.Offset = offset;
            this.Rotation = Quaternion.Euler(rotation);
            this.Scale = scale;
            this.HideOwner = hideOwner;
            this.ShowToOwner = showToOwner;
        }
    }
}