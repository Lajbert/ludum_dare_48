using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Global;
using MonolithEngine.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Weapons.Guns
{
    class Machinegun : Handgun
    {

        private readonly int FIRE_RATE = 50;

        private bool firing = false;

        private Vector2 target;

        public Machinegun(AbstractScene scene, Hero hero) : base(scene, hero)
        {

        }

        public override void TriggerPulled(Vector2 worldPosition)
        {
            if (Timer.IsSet("Firing"))
            {
                return;
            }
            Timer.SetTimer("Firing", FIRE_RATE);
            SpawnBullet(worldPosition);
            hero.WeaponKickback(0.5f);
        }

        public override void TriggerReleased(Vector2 worldPosition)
        {
            firing = false;
        }

        private void SpawnBullet(Vector2 worldPosition)
        {
            new MachineGunBullet(Scene, Transform.Position + Offset, worldPosition);
        }
    }
}
