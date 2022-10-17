using System.Collections.Generic;
using Exiled.API.Interfaces;
using UnityEngine;

namespace hats
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool RemoveHatOnDeath { get; set; } = true;
        public bool ListCommandShowPath { get; set; } = false;
        public string CommandPrefix { get; set; } = "hatplugin";
        
        public List<HatConfig> Hats { get; set; }= new List<HatConfig>()
        {
            new HatConfig()
            {
                Name = "Name",
                SchematicName = "SchematicName",
                Offset = new Vector3()
                {
                    x = 0f,
                    y = 0.2f,
                    z = 0f
                },
                Rotation = new Vector3()
                {
                    x = 0,
                    y = 0,
                    z = 0
                },
                Scale = new Vector3()
                {
                    x = 1,
                    y = 1,
                    z = 1
                },
                MakePlayerInvisible = false,
                ShowHatToOwner = true,
                UsersWithAccess = new List<string>(),
                GroupsWithAccess = new List<string>()
            }
        };
    }
}