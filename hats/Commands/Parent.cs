using System;
using System.Text;
using CommandSystem;
using NorthwoodLib.Pools;

namespace hats.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Parent : ParentCommand
    {
        public override string Command { get; } = Plugin.Singleton.Config.CommandPrefix;
        public override string[] Aliases { get; } = { };
        public override string Description { get; } = "Parent command for hats plugin";
        public Parent() => LoadGeneratedCommands();
        
        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new List());
            RegisterCommand(new AddHat());
            RegisterCommand(new RemoveHat());
        }

        protected override bool ExecuteParent(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.AppendLine("Please enter a valid subcommand! Available:");
            foreach (ICommand command in AllCommands)
            {
                stringBuilder.AppendLine(command.Aliases is { Length: > 0 }
                    ? $"{command.Command} | Aliases: {string.Join(", ", command.Aliases)}"
                    : command.Command);
            }

            response = StringBuilderPool.Shared.ToStringReturn(stringBuilder).TrimEnd();
            return false;
        }
    }
}