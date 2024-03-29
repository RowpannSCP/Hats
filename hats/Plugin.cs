﻿using System;
using System.Collections.Generic;
using Exiled.API.Features;
using hats.Components;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace hats
{
    public class Plugin : Plugin<Config>
    {
        public const string OwnerSteamid = "76561198978359966@steam";

        public override string Author { get; } = "Rowpann SCP";
        public override string Name { get; } = "hats";
        public override Version Version { get; } = new Version(2, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(8, 0, 0);

        public static Plugin Singleton;
        public EventHandler Handler { get; private set; }
        public readonly Dictionary<string, HatComponent> HatWearers = new Dictionary<string, HatComponent>();

        public override void OnEnabled()
        {
            Singleton = this;
            Handler = new EventHandler(Config);

            Server.WaitingForPlayers += Handler.WaitingForPlayers;
            Player.Left += Handler.OnLeave;
            Player.Died += Handler.Died;
            Player.UsedItem += Handler.UsedItem;
            Player.Spawned += Handler.Spawned;
            Player.ChangingSpectatedPlayer += Handler.OnSpectate;
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Server.WaitingForPlayers -= Handler.WaitingForPlayers;
            Player.Left -= Handler.OnLeave;
            Player.Died -= Handler.Died;
            Player.UsedItem -= Handler.UsedItem;
            Player.Spawned -= Handler.Spawned;
            Player.ChangingSpectatedPlayer -= Handler.OnSpectate;

            Singleton = null;
            Handler = null;
            base.OnDisabled();
        }
    }
}