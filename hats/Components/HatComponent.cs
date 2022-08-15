using System;
using Exiled.API.Features;
using MapEditorReborn.API.Features.Objects;
using UnityEngine;

namespace hats.Components
{
    public class HatComponent : MonoBehaviour
    {
        public Hat hat;
        public Player ply;
        public SchematicObject schem;

        public void DoDestroy()
        {
            schem.Destroy();
            ply = null;
            hat = null;
            Destroy(this);
        }
    }
}