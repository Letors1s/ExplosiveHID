using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodeHID
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "ExplodeHID";

        public override string Author => "Letors1s";

        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(9, 14, 2);

        public static Plugin pl;
        public int ChanceToExplode;

        public override void OnEnabled()
        {
            pl = this;
            Exiled.Events.Handlers.Server.RoundStarted += Start;
            Exiled.Events.Handlers.Player.ChangingMicroHIDState += HID;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.ChangingMicroHIDState -= HID;
            Exiled.Events.Handlers.Server.RoundStarted -= Start;

            pl = null;
            base.OnDisabled();
        }

        public void Start()
        {
            ChanceToExplode = Config.ChanceToExplode;
            if(Config.ChanceToExplode > 100)
            {
                ChanceToExplode = 100;
            }
            if(Config.ChanceToExplode < 0)
            {
                ChanceToExplode = 1;
            }


        }

        public void HID(ChangingMicroHIDStateEventArgs ar)
        {
            if(ar.NewPhase == InventorySystem.Items.MicroHID.Modules.MicroHidPhase.Firing)
            {
                int ver = UnityEngine.Random.Range(0, 100);
                if (ChanceToExplode > ver)
                {
                    var grenade = (ExplosiveGrenade)Exiled.API.Features.Items.Item.Create(ItemType.GrenadeHE);
                    grenade.FuseTime = 0.05f;
                    grenade.SpawnActive(ar.Player.Position, ar.Player);
                }
            }
        }
    }
}
