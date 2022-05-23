using System;
using System.Collections.Generic;
using System.Linq;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MapEditorReborn.API.Features.Serializable;
using MapEditorReborn.Commands.Scale;
using UnityEngine;

namespace hats
{
    public class Hat
    {
        public List<SchematicObject> SpawnedHats = new List<SchematicObject>();
        public string Name;
        public Vector3 Offset;
        public SchematicObjectDataList Schematic;

        public SchematicObject SpawnHat(Vector3 Position, Quaternion? Rotation = null, Vector3? scale = null)
        {
            SchematicObject obj = ObjectSpawner.SpawnSchematic(Name, Position, null, scale, Schematic);
            SpawnedHats.Append(obj);
            return obj;
        }

        public void DestroyInstances()
        {
            foreach (var hat in SpawnedHats.Where(x => x != null))
            {
                hat.Destroy();
            }
            SpawnedHats.Clear();
        }

        public Hat(string Name, SchematicObjectDataList data, Vector3 offset)
        {
            this.Name = Name;
            Schematic = data;
            this.Offset = offset;
        }
    }
}