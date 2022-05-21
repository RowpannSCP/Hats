using System;
using Exiled.API.Features;
using Server = Exiled.Events.Handlers.Server;

namespace hats
{
    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "Rowpann SCP";
        public override string Name { get; } = "hats";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(5, 2, 0);

        public static Plugin Singleton;
        public EventHandler Handler { get; private set; }
        
        public override void OnEnabled()
        {
            Singleton = this;
            Handler = new EventHandler(Config);

            Server.WaitingForPlayers += Handler.WaitingForPlayers;
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Server.WaitingForPlayers -= Handler.WaitingForPlayers;
            
            Singleton = null;
            Handler = null;
            base.OnDisabled();
        }
    }
}