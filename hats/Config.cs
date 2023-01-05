namespace hats
{
    using System.ComponentModel;
    using System.Collections.Generic;
    using Exiled.API.Interfaces;
    using PlayerRoles;
    using UnityEngine;

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }
        public bool RemoveHatOnDeath { get; set; } = true;
        public bool ListCommandShowPath { get; set; } = false;
        [Description("Whether or not the hat owner will bypass role blacklist (hat-specific config used instead)")]
        public bool ShowHatToOwnerIfRoleHideHatAndHideHatToOwnerFalse { get; set; } = true;
        public bool AllowGetHatToRemoveHat { get; set; } = true;
        public bool TrimHatNamesInGetHat { get; set; } = true;
        public bool RemoveHatWhenUsing268 { get; set; } = true;
        public string CommandPrefix { get; set; } = "hatplugin";

        public List<RoleTypeId> RolesToHideHatFrom { get; set; } = new List<RoleTypeId>()
        {
            RoleTypeId.Scp939,
            RoleTypeId.Scp096
        };

        public bool EnableAutoGiveHat { get; set; } = false;
        [Description("Roles that will be given a hat on spawn, if EnableAutoGiveHat is true")]
        public Dictionary<RoleTypeId, string> RolesWithHats { get; set; } = new Dictionary<RoleTypeId, string>()
        {
            [RoleTypeId.ClassD] = "Name",
        };

        public List<HatConfig> Hats { get; set; }= new List<HatConfig>()
        {
            new HatConfig()
            {
                Name = "Name",
                SchematicName = "SchematicName",
                Offset = new Vector3()
                {
                    x = 0f,
                    y = 0.2f,
                    z = 0f
                },
                Rotation = new Vector3()
                {
                    x = 0,
                    y = 0,
                    z = 0
                },
                Scale = new Vector3()
                {
                    x = 1,
                    y = 1,
                    z = 1
                },
                MakePlayerInvisible = false,
                ShowHatToOwner = true,
                UsersWithAccess = new List<string>(),
                GroupsWithAccess = new List<string>()
            }
        };
    }
}