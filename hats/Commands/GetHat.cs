namespace hats.Commands
{
    using System;
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
                response = "You are already wearing a hat!";
                return false;
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

            if (!hats.Select(x => x.Name).Contains(arguments.At(0)))
            {
                response = $"You don't have access to {arguments.At(0)}";
                return false;
            }
            
            var toSpawnHat = API.Hats.First(x => x.Key == arguments.At(0));

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