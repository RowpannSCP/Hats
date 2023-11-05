namespace hats.Commands
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using hats.Components;
    using MapEditorReborn.API.Extensions;
    using RemoteAdmin;
    using UnityEngine;

    public class HatDebug : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is not PlayerCommandSender)
            {
                response = "You must be a player to use this command!";
                return false;
            }

            var ply = Player.Get(sender);

            if (!(ply.CheckPermission("hats.debug") || ply.UserId == Plugin.OwnerSteamid))
            {
                response = "no perms cringe (hats.debug)";
                return false;
            }

            if (!ply.GameObject.TryGetComponent(out HatComponent hat))
            {
                response = "Player isnt wearing a hat!";
                return false;
            }

            if (arguments.Count > 0)
            {
                var firstArgument = arguments.At(0);
    
                switch (firstArgument)
                {
                    case "hide":
                        ply.DestroySchematic(hat.Schematic);
                        break;
                    case "show":
                        ply.SpawnSchematic(hat.Schematic);
                        break;
                    case "update":
                        hat.Schematic.UpdateObject();
                        break;
                    case "scale":
                    {
                        if (arguments.Count > 1)
                        {
                            if (float.TryParse(arguments.At(1), out var scale))
                            {
                                hat.Schematic.Scale = new Vector3(scale, scale, scale);
                            }
                        }

                        break;
                    }
                }

                response = "done";
                return true;
            }

            var gameObject = hat.gameObject;
            response = $"Name: {hat.Hat.Name}\n";
            response += $"Enabled: {hat.Schematic.enabled}\n";
            response += $"Config rotation offset: {hat.Hat.Rotation}\n";
            response += $"Config position offset: {hat.Hat.Offset}\n";
            response += $"Actual local rotation: {gameObject.transform.localRotation}\n";
            response += $"Actual local position: {gameObject.transform.localPosition}\n";

            var transform = gameObject.transform.parent;
            if(transform != null)
            {
                var o = transform.gameObject;
                response += $"Parent GO (is player): {o.name} ({ply.GameObject == o})\n";
            }
            else
            {
                response += "Parent GO = null\n";
            }

            response += $"Hat global position: {gameObject.transform.position}\n";
            return true;
        }

        public string Command { get; } = "debug";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Print hat debug information";
    }
}