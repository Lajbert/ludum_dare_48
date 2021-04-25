using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Entities.Animations;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Source.Entities;
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

            Animations = new AnimationStateMachine();
            RemoveComponent<AnimationStateMachine>();
            AddComponent(Animations);
            AllAmmo = 0;
            //LeftFacingOffset = new Vector2(50, 0);
            //RightFacingOffset = new Vector2(-50, 0);

            if (hero.CurrentFaceDirection == Direction.EAST)
            {
                Transform.Position = RightFacingOffset;
            }
            else
            {
                Transform.Position = LeftFacingOffset;
            }

            /*SpriteSheetAnimation machineGunLeft = new SpriteSheetAnimation(this, Assets.GetTexture("Machinegun"), 1);
            Animations.RegisterAnimation("MachineGunLeft", machineGunLeft, () => CurrentFaceDirection == Direction.WEST);

            SpriteSheetAnimation machineGunRight = machineGunLeft.CopyFlipped();
            machineGunLeft.FlipVertical();
            Animations.RegisterAnimation("MachineGunRight", machineGunRight, () => CurrentFaceDirection == Direction.EAST);*/

            SpriteSheetAnimation animLeft = new SpriteSheetAnimation(this, Assets.GetTexture("Shotgun"), 32, 32, 1);
            SpriteSheetAnimation animRight = animLeft.CopyFlipped();
            animLeft = animLeft.CopyFlipped();
            animLeft.FlipVertical();
            Animations.RegisterAnimation("MachineGunLeft", animLeft, () => CurrentFaceDirection == Direction.WEST);
            Animations.RegisterAnimation("MachineGunRight", animRight, () => CurrentFaceDirection == Direction.EAST);
        }

        public override void TriggerPulled(Vector2 worldPosition)
        {
            if (AllAmmo == 0)
            {
                return;
            }
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
            SpawnShells(5000, 8, new Vector2(0.1f, -0.2f));
            AllAmmo--;
        }

        private void SpawnShells(int lifetime, int rotationOffset, Vector2 force)
        {
            Vector2 shellPos = MathUtil.RadToVector(AnimationRotation);
            shellPos.Normalize();
            if (CurrentFaceDirection == Direction.EAST)
            {
                force.X *= -1;
            }
            new ShotgunShell(Scene, Transform.Position + shellPos * rotationOffset, lifetime, force);
        }
    }
}
