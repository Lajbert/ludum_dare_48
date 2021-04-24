using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Source.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Weapons.Guns
{
    class Shotgun : Handgun
    {

        private bool shotsFired = false;

        private int startAngle = 0;

        public Shotgun(AbstractScene scene, Hero hero) : base (scene, hero)
        {
            CurrentFaceDirection = hero.CurrentFaceDirection;
        }

        public override void TriggerPulled()
        {
            if (shotsFired)
            {
                return;
            }
            shotsFired = true;
            SpawnBullets();
            hero.WeaponKickback(3);
        }

        public override void TriggerReleased()
        {
            shotsFired = false;
        }

        private void SpawnBullets()
        {
            if (CurrentFaceDirection == Direction.EAST)
            {
                startAngle = 0;
            }
            else
            {
                startAngle = 180;
            }
            for (int i = startAngle - 6; i <= startAngle + 6; i += 3)
            {
                ShotgunBullet bullet = new ShotgunBullet(Scene, Transform.Position + Offset, CurrentFaceDirection);
                bullet.Velocity = MathUtil.RadToVector(MathUtil.DegreesToRad(i));
            }
        }
    }
}
