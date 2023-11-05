namespace hats.Commands
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;

    public class ReloadHats : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("hats.reload"))
            {
                response = "Missing permission: hats.reload.";
                return false;
            }

            API.LoadHats();
            response = "reloaded hats!";
            return true;
        }

        public string Command { get; } = "reload";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Reloads all hats (WILL REMOVE ALL CURRENT HATS!)";
    }
}