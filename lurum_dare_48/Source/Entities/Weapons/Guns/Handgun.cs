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
            //animLeft.FlipHorizontal();
            //animLeft.FlipHorizontal();
            Animations.RegisterAnimation("GunLeft", animLeft, () => CurrentFaceDirection == Direction.WEST);

            //SpriteSheetAnimation animRight = animLeft.CopyFlipped();
            Animations.RegisterAnimation("GunRight", animRight, () => CurrentFaceDirection == Direction.EAST);

            //Texture = new Sprite(this, AssetUtil.CreateRectangle(Config.GRID / 2, Color.Aquamarine), new Rectangle(0, 0, Config.GRID, Config.GRID / 2));
            //Texture.DrawOffset = Offset;
            //AddComponent(Texture);

            //DEBUG_SHOW_PIVOT = true;

        }

        public override void TriggerPulled(Vector2 worldPosition)
        {
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
            new HandgunBullet(Scene, Transform.Position + Offset, worldPosition);
        }
    }
}
