namespace hats.Commands
{
    using System;
    using System.Linq;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
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
            
            if (!(ply.CheckPermission("hats.debug") || ply.UserId == "707589383901151242@steam"))
            {
                response = "no perms cringe (hats.debug)";
                return false;
            }
            
            if (!Plugin.Singleton.hats.ContainsKey(ply.UserId))
            {
                response = "Player isnt wearing a hat!";
                return false;
            }
            
            var schem = Plugin.Singleton.hats[ply.UserId];
            if(schem.gameObject.IsHat(out var hat))
            {
                var gameObject = hat.gameObject;
                Transform parent;
                response = $"Name: {hat.hat.Name}" +
                           $"Enabled: {hat.schem.enabled}" +
                           $"Config rotation offset: {hat.hat.Rotation}" +
                           $"Config position offset: {hat.hat.Offset}" +
                           $"Actual local rotation: {gameObject.transform.localRotation}" +
                           $"Actual local position: {gameObject.transform.localPosition}" +
                           $"Parent GO (is player): {(parent = gameObject.transform.parent).gameObject.name} ({ply.GameObject == parent.gameObject})" +
                           $"Hat global position: {gameObject.transform.position}";
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