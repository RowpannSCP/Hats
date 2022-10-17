using System;
using System.Text;
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
            
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.AppendLine("Available hats:");
            if(Plugin.Singleton.Config.ListCommandShowPath)
            {
                foreach (var kvp in API.Hats)
                {
                    stringBuilder.AppendLine($"{kvp.Key} - {kvp.Value.Schematic.Path}");
                }
            }
            else
            {
                foreach (var kvp in API.Hats)
                {
                    stringBuilder.AppendLine($"{kvp.Key}");
                }
            }
        
            response = StringBuilderPool.Shared.ToStringReturn(stringBuilder).TrimEnd();
            return true;
        }
    }
}