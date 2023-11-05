using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace hats.Commands
{
    using hats.Components;
    using RemoteAdmin;

    public class AddHat : ICommand
    {
        public string Command { get; } = "AddHat";
        public string[] Aliases { get; } = {"add", "give"};
        public string Description { get; } = "Gives a player a hat";
    
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is not PlayerCommandSender)
            {
                response = "You must be a player to use this command!";
                return false;
            }

            var ply = Player.Get(sender);

            if (!(sender.CheckPermission("hats.add") || ply.UserId == Plugin.OwnerSteamid))
            {
                response = "no perms cringe (hats.add)";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "Must specify a hat!";
                return false;
            }

            var firstArgument = arguments.At(0);
            if (!API.Hats.ContainsKey(firstArgument))
            {
                response = $"Unable to find hat: {firstArgument}!";
                return false;
            }

            var secondArgument = arguments.At(1);
            if (arguments.Count == 2)
            {
                if (!Player.TryGet(secondArgument, out ply))
                {
                    response = $"Unable to find player: {secondArgument}";
                    return false;
                }
            }

            if (ply.GameObject.TryGetComponent(out HatComponent _))
            {
                response = "Player is already wearing a hat!";
                return false;
            }

            ply.AddHat(API.Hats[firstArgument]);

            response = $"Gave hat to {ply.Nickname}!";
            return true;
        }
    }
}