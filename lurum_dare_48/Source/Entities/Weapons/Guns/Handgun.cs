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
    class Handgun : AbstractWeapon
    {

        private bool shotFired = false;

        public Handgun(AbstractScene scene, Hero hero) : base(scene, hero)
        {
            LeftFacingOffset = new Vector2(-3, -17);
            RightFacingOffset = new Vector2(3, -17);
            if (hero.CurrentFaceDirection == Direction.EAST)
            {
                Transform.Position = RightFacingOffset;
            }
            else
            {
                Transform.Position = LeftFacingOffset;
            }
            ClipSize = 7;
            AmmoInClip = ClipSize;
            AllAmmo = 100;
            Animations.Offset = Offset;
            SpriteSheetAnimation animLeft = new SpriteSheetAnimation(this, Assets.GetTexture("Gun"), 40);
            animLeft.StartFrame = 1;
            animLeft.EndFrame = 2;
            SpriteSheetAnimation animRight = animLeft.CopyFlipped();
            animLeft = animLeft.CopyFlipped();
            animLeft.FlipVertical();
            Animations.RegisterAnimation("GunLeft", animLeft, () => CurrentFaceDirection == Direction.WEST);

            Animations.RegisterAnimation("GunRight", animRight, () => CurrentFaceDirection == Direction.EAST);

            //AddComponent(Texture);

            //DEBUG_SHOW_PIVOT = true;

        }

        public override void TriggerPulled(Vector2 worldPosition)
        {
            if (AllAmmo == 0)
            {
                return;
            }

            if (shotFired)
            {
                return;
            }
            shotFired = true;
            SpawnBullet(worldPosition);

            hero.WeaponKickback(worldPosition, 1);
        }

        public override void TriggerReleased(Vector2 worldPosition)
        {
            shotFired = false;
        }

        private void SpawnBullet(Vector2 worldPosition)
        {
            SpawnShellsAndBullet(worldPosition, 5000, 8, new Vector2(0.1f, -0.2f));
            AllAmmo--;
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
