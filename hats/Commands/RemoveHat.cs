using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace hats.Commands
{
    public class RemoveHat : ICommand
    {
        public string Command { get; } = "RemoveHat";
        public string[] Aliases { get; } = {"remove", "clear"};
        public string Description { get; } = "Removes a players hat";
    
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var ply = Player.Get(sender);

            if (!(sender.CheckPermission("hats.remove") || ply.UserId == Plugin.OwnerSteamid))
            {
                response = "no perms cringe (hats.remove)";
                return false;
            }

            if (arguments.Count > 0)
            {
                var plyArgument = arguments.At(0);
                if (!Player.TryGet(plyArgument, out ply))
                {
                    response = $"Unable to find player: {plyArgument}";
                    return false;
                }
            }

            try
            {
                ply.RemoveHat();
            }
            catch (ArgumentException)
            {
                response = "Player is not wearing a hat!";
                return false;
            }

            response = $"Removed hat from {ply.Nickname}!";
            return true;
        }
    }
}