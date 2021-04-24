using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Weapons.Guns
{
    class Machinegun : Handgun
    {

        private readonly int FIRE_RATE = 50;

        public Machinegun(AbstractScene scene, Hero hero) : base(scene, hero)
        {

        }

        public override void TriggerPulled()
        {
            if (Timer.IsSet("Firing"))
            {
                return;
            }
            Timer.SetTimer("Firing", FIRE_RATE);

            SpawnBullet();
            hero.WeaponKickback(0.5f);
        }

        private void SpawnBullet()
        {
            new MachineGunBullet(Scene, Transform.Position + Offset, CurrentFaceDirection);
        }
    }
}
