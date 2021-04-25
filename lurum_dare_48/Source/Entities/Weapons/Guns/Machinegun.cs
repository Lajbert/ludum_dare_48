using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Entities.Animations;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Global;
using MonolithEngine.Source.Entities;
using MonolithEngine.Source.Util;
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
            Animations = new AnimationStateMachine();
            RemoveComponent<AnimationStateMachine>();
            AddComponent(Animations);

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

            SpriteSheetAnimation animLeft = new SpriteSheetAnimation(this, Assets.GetTexture("Machinegun"), 32, 32, 1);
            SpriteSheetAnimation animRight = animLeft.CopyFlipped();
            animLeft = animLeft.CopyFlipped();
            animLeft.FlipVertical();
            Animations.RegisterAnimation("MachineGunLeft", animLeft, () => CurrentFaceDirection == Direction.WEST);
            Animations.RegisterAnimation("MachineGunRight", animRight, () => CurrentFaceDirection == Direction.EAST);
        }

        public override void TriggerPulled(Vector2 worldPosition)
        {
            if (Timer.IsSet("Firing"))
            {
                return;
            }
            Timer.SetTimer("Firing", FIRE_RATE);
            SpawnBullet(worldPosition);
            hero.WeaponKickback(worldPosition, 0.5f);
        }

        public override void TriggerReleased(Vector2 worldPosition)
        {
            firing = false;
        }

        private void SpawnBullet(Vector2 worldPosition)
        {
            SpawnShellsAndBullet(worldPosition, 2000, 5, new Vector2(0.1f, -0.2f));
        }

        private void SpawnShellsAndBullet(Vector2 worldPosition, int lifetime, int rotationOffset, Vector2 force)
        {
            Vector2 shellPos = MathUtil.RadToVector(AnimationRotation);
            shellPos.Normalize();
            if (CurrentFaceDirection == Direction.EAST)
            {
                force.X *= -1;
            }
            new BulletShell(Scene, Transform.Position + shellPos * rotationOffset, lifetime, force);
            new HandgunBullet(Scene, Transform.Position + shellPos * rotationOffset, worldPosition);
        }
    }
}
