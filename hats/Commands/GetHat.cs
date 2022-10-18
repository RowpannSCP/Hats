namespace hats.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommandSystem;
    using Exiled.API.Extensions;
    using Exiled.API.Features;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class GetHat : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var ply = Player.Get(sender);
            if (Plugin.Singleton.hats.ContainsKey(ply.UserId))
            {
                if (!Plugin.Singleton.Config.AllowGetHatToRemoveHat)
                {
                    response = "Already wearing a hat (removing hat disabled in config)";
                    return false;
                }
                ply.RemoveHat();
                response = "Removed hat!";
                return true;
            }
            
            var hats = Plugin.Singleton.Config.Hats
                .Where(x => x.UsersWithAccess
                    .Any(y => y == ply.UserId) || x.GroupsWithAccess
                    .Any(y => y == ply.Group.GetKey()))
                .ToArray();
            if (arguments.Count < 1)
            {
                response = "You have access to: \n";
                if (!hats.Any())
                {
                    response += "Nothing lol";
                    return true;
                }

                foreach (var hat in hats)
                {
                    response += hat.Name += " \n";
                }

                return true;
            }

            if (string.IsNullOrEmpty(arguments.At(0)))
            {
                response = $"Specify a hat name, available: {string.Join(", ", hats.Select(x => x.Name))}";
                return false;
            }

            KeyValuePair<string, Hat> toSpawnHat = new KeyValuePair<string, Hat>();
            // Idk why but somehow spaces get added
            if (Plugin.Singleton.Config.TrimHatNamesInGetHat)
            {
                if (hats.Select(x => x.Name).All(x => x.TrimStart().TrimEnd() != arguments.At(0).TrimStart().TrimEnd()))
                {
                    response = $"You don't have access to {arguments.At(0).TrimStart().TrimEnd()}";
                    return false;
                }
                
                toSpawnHat = API.Hats.First(x => x.Key.TrimStart().TrimEnd() == arguments.At(0).TrimStart().TrimEnd());
            }
            else
            {
                if (!hats.Select(x => x.Name).Contains(arguments.At(0)))
                {
                    response = $"You don't have access to {arguments.At(0)}";
                    return false;
                }
                
                toSpawnHat = API.Hats.First(x => x.Key == arguments.At(0));
            }

            try
            {
                ply.AddHat(toSpawnHat.Value);
            }
            catch (Exception e)
            {
                response = $"Error: {e}, contact the developer";
                Log.Error(e);
                return false;
            }
            
            response = "Spawned hat";
            return true;
        }

        public string Command { get; } = "gethat";
        public string[] Aliases { get; } = new[] { "myhat" };
        public string Description { get; } = "Get your hat!";
    }
}