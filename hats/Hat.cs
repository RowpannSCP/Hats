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
        public SchematicObjectDataList Schematic;

        internal SchematicObject SpawnHat(Vector3 Position, Quaternion? Rotation = null, Vector3? scale = null)
        {
            SchematicObject obj = ObjectSpawner.SpawnSchematic(Name, Position, null, scale, Schematic);
            return obj;
        }

        public void DestroyInstances()
        {
            if (Plugin.Singleton.hats.All(x => x.Value.hat != this))
                return;
            List<KeyValuePair<string, HatComponent>> toRemove = new List<KeyValuePair<string, HatComponent>>();
            foreach (var kvp in Plugin.Singleton.hats.Where(x => x.Value.hat == this))
            {
                kvp.Value.DoDestroy();
                toRemove.Add(kvp);
            }

            foreach (var kvp in toRemove)
            {
                Plugin.Singleton.hats.Remove(kvp.Key);
            }
        }

        public Hat(string Name, SchematicObjectDataList data, Vector3 offset)
        {
            this.Name = Name;
            Schematic = data;
            this.Offset = offset;
        }
    }
}