namespace hats.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommandSystem;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using hats.Components;
    using RemoteAdmin;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class GetHat : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is not PlayerCommandSender)
            {
                response = "You must be a player to use this command!";
                return false;
            }

            var ply = Player.Get(sender);

            if (!ply.GameObject.TryGetComponent(out HatComponent _))
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

            HatConfig foundHatConfig;
            var hatName = arguments.At(0);
            // Idk why but somehow spaces get added
            if (Plugin.Singleton.Config.TrimHatNamesInGetHat)
            {
                hatName = hatName.TrimStart().TrimEnd();
                foundHatConfig = hats.FirstOrDefault(x => x.Name.TrimStart().TrimEnd() == hatName);
            }
            else
            {
                foundHatConfig = hats.FirstOrDefault(x => x.Name == hatName);
            }

            if (foundHatConfig == null)
            {
                response = $"You don't have access to {hatName}, or it dosent exist";
                return false;
            }

            try
            {
                ply.AddHat(API.Hats[foundHatConfig.Name]);
            }
            catch (Exception e)
            {
                response = $"Error: {e}, contact the developer";
                Log.Error("Error while adding hat:" + e);
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