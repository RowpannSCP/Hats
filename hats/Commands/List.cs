using System;
using CommandSystem;
using NorthwoodLib.Pools;

namespace hats.Commands
{
    using Exiled.Permissions.Extensions;

    public class List : ICommand
    {
        public string Command { get; } = "List";
        public string[] Aliases { get; } = new[] {"L"};
        public string Description { get; } = "List all available hats";
    
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("hats.list"))
            {
                response = "no perms cringe (hats.list)";
                return false;
            }

            var sb = StringBuilderPool.Shared.Rent();
            sb.AppendLine("Available hats:");

            if (Plugin.Singleton.Config.ListCommandShowPath)
            {
                foreach (var kvp in API.Hats)
                {
                    sb.AppendLine($"{kvp.Key} - {kvp.Value.Schematic.Path}");
                }
            }
            else
            {
                foreach (var name in API.Hats.Keys)
                {
                    sb.AppendLine(name);
                }
            }

            response = StringBuilderPool.Shared.ToStringReturn(sb).TrimEnd();
            return true;
        }
    }
}