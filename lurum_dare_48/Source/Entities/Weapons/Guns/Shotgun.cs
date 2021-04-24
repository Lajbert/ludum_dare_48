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

        public Shotgun(AbstractScene scene, Hero hero) : base (scene, hero)
        {
            CurrentFaceDirection = hero.CurrentFaceDirection;
        }

        public override void TriggerPulled(Vector2 worldPosition)
        {
            if (shotsFired)
            {
                return;
            }
            shotsFired = true;
            SpawnBullets(worldPosition);
            hero.WeaponKickback(worldPosition, 3);
        }

        public override void TriggerReleased(Vector2 worldPosition)
        {
            shotsFired = false;
        }

        private void SpawnBullets(Vector2 worldPosition)
        {
            float degree = MathUtil.DegreeFromVectors(Transform.Position + Offset, worldPosition);
            for (float i = degree - 6; i <= degree + 6; i += 3)
            {
                ShotgunBullet bullet = new ShotgunBullet(Scene, Transform.Position + Offset, MathUtil.RadToVector(MathUtil.DegreesToRad(degree)));
                bullet.Velocity = MathUtil.RadToVector(MathUtil.DegreesToRad(i));
            }
        }
    }
}
