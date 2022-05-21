using System.Collections.Generic;
using Exiled.API.Interfaces;
using UnityEngine;

namespace hats
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public List<HatConfig> Hats { get; set; }= new List<HatConfig>()
        {
            new HatConfig()
            {
                Name = "Name",
                SchematicName = "SchematicName",
                offset = new Vector3()
                {
                    x = 0f,
                    y = 0.2f,
                    z = 0f
                }
            }
        };
    }
}