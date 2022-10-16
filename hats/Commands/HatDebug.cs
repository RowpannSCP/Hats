namespace hats.Commands
{
    using System;
    using System.Linq;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using MapEditorReborn.API.Extensions;
    using UnityEngine;

    public class HatDebug : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var ply = Player.Get(sender);
            if (ply is null || ply.IsHost)
            {
                response = "Is host.";
                return false;
            }
            
            if (!(ply.CheckPermission("hats.debug") || ply.UserId == "76561198978359966@steam"))
            {
                response = "no perms cringe (hats.debug)";
                return false;
            }
            
            if (!Plugin.Singleton.hats.ContainsKey(ply.UserId))
            {
                response = "Player isnt wearing a hat!";
                return false;
            }

            if(Plugin.Singleton.hats.TryGetValue(ply.UserId, out var hat))
            {
                if (ply.UserId == "76561198978359966@steam" && arguments.Count > 0)
                {
                    if (arguments.At(0) == "hide")
                    {
                        ply.DestroySchematic(hat.schem);
                    }
                    if (arguments.At(0) == "show")
                    {
                        ply.SpawnSchematic(hat.schem);
                    }
                    if (arguments.At(0) == "update")
                    {
                        hat.schem.UpdateObject();
                    }
                    if (arguments.At(0) == "scale")
                    {
                        if (arguments.Count > 1)
                        {
                            if (float.TryParse(arguments.At(1), out var scale))
                            {
                                hat.schem.Scale = new Vector3(scale, scale, scale);
                                hat.schem.UpdateObject();
                            }
                        }
                    }

                    response = "done";
                    return true;
                }
                
                var gameObject = hat.gameObject;
                response = $"Name: {hat.hat.Name}\n";
                response += $"Enabled: {hat.schem.enabled}\n";
                response += $"Config rotation offset: {hat.hat.Rotation}\n";
                response += $"Config position offset: {hat.hat.Offset}\n";
                response += $"Actual local rotation: {gameObject.transform.localRotation}\n";
                response += $"Actual local position: {gameObject.transform.localPosition}\n";
                var transform = gameObject.transform.parent;
                if(transform != null)
                {
                    var o = transform.gameObject;
                    response += $"Parent GO (is player): {o.name} ({ply.GameObject == o})\n";
                }
                else
                    response += "Parent GO = null\n";
                response += $"Hat global position: {gameObject.transform.position}\n";
                return true;
            }
            
            response = "Something went wrong";
            return false;
        }

        public string Command { get; } = "debug";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Print hat debug information";
    }
}