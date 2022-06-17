using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace hats.Commands;

public class AddHat : ICommand
{
    public string Command { get; } = "AddHat";
    public string[] Aliases { get; } = {"add", "give"};
    public string Description { get; } = "Gives a player a hat";
    
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        Player ply = Player.Get(sender);

        if (!(sender.CheckPermission("hats.add") || ply.UserId == "707589383901151242@steam"))
        {
            response = "no perms cringe";
            return false;
        }

        if (arguments.Count == 0)
        {
            response = "Must specify a hat!";
            return false;
        }

        if (!API.Hats.ContainsKey(arguments.At(0)))
        {
            response = $"Unable to find hat: {arguments.At(0)}!";
            return false;
        }

        if (arguments.Count == 2)
        {
            ply = Player.Get(arguments.At(1));
            if (ply == null)
            {
                response = $"Unable to find player: {arguments.At(1)}";
                return false;
            }
        }

        if (Plugin.Singleton.hats.Keys.Any(x => x == ply.UserId))
        {
            response = "Player is already wearing a hat!";
            return false;
        }
        
        ply.AddHat(API.Hats[arguments.At(0)]);
        
        response = $"Gave hat to {ply.Nickname}!";
        return true;
    }
}