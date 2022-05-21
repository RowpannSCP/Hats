using System;
using System.Text;
using CommandSystem;
using NorthwoodLib.Pools;

namespace hats.Commands;

public class List : ICommand
{
    public string Command { get; } = "List";
    public string[] Aliases { get; } = new[] {"L"};
    public string Description { get; } = "List all available hats";
    
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
        stringBuilder.AppendLine("Available hats:");
        foreach (var kvp in API.Hats)
        {
            stringBuilder.AppendLine($"{kvp.Key} - {kvp.Value.Schematic.Path}");
        }
        
        response = StringBuilderPool.Shared.ToStringReturn(stringBuilder).TrimEnd();
        return true;
    }
}