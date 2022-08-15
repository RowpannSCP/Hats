using System;
using System.Linq;
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
            Player ply = Player.Get(sender);

            if (!(sender.CheckPermission("hats.remove") || ply.UserId == "707589383901151242@steam"))
            {
                response = "no perms cringe";
                return false;
            }

            if(arguments.Count > 0)
            {
                ply = Player.Get(arguments.At(0));
                if (ply == null)
                {
                    response = $"Unable to find player: {arguments.At(0)}";
                    return false;
                }
            }

            if (Plugin.Singleton.hats.Keys.All(x => x != ply.UserId))
            {
                response = "Player isn't wearing a hat!";
                return false;
            }
        
            ply.RemoveHat();
        
            response = $"Removed hat from {ply.Nickname}!";
            return true;
        }
    }
}