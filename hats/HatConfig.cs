using UnityEngine;

namespace hats
{
    using System.Collections.Generic;

    public class HatConfig
    {
        public string SchematicName { get; set; }
        public string Name { get; set; }
        public Vector3 Offset { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public bool MakePlayerInvisible { get; set; } = false;
        public bool ShowHatToOwner { get; set; } = true;
        public List<string> UsersWithAccess { get; set; } = new List<string>();
        public List<string> GroupsWithAccess { get; set; } = new List<string>();
    }
}