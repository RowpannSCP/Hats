using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace hats.Commands;

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

        if (!ply.SessionVariables.ContainsKey("HatWearer"))
        {
            response = "Player isn't wearing a hat!";
            return false;
        }
        
        ply.AddHat(API.Hats[arguments.At(0)]);
        
        response = $"Gave hat to {ply.Nickname}!";
        return true;
    }
}