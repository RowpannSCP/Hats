using System;
using Exiled.API.Features;
using MapEditorReborn.API.Features.Objects;
using UnityEngine;

namespace hats.Components
{
    public class HatComponent : MonoBehaviour
    {
        public Hat Hat;
        public SchematicObject Schematic;

        public void DoDestroy()
        {
            Schematic.Destroy();

            try
            {
                Destroy(this);
            }
            catch (Exception)
            {
                // ignore
            }
        }
    }
}