using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Entities;
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
    class Handgun : AbstractWeapon
    {

        private bool shotFired = false;

        public Handgun(AbstractScene scene, Hero hero) : base(scene, hero) 
        {
            if (hero.CurrentFaceDirection == Direction.EAST)
            {
                Offset = new Vector2(8, -16);
            }
            else
            {
                Offset = new Vector2(-24, -16);
            }
            ClipSize = 7;
            AmmoInClip = ClipSize;
            AllAmmo = 100;
            Texture = new Sprite(this, AssetUtil.CreateRectangle(Config.GRID / 2, Color.Aquamarine), new Rectangle(0, 0, Config.GRID, Config.GRID / 2));
            Texture.DrawOffset = Offset;
            AddComponent(Texture);
            
        }

        public override void TriggerPulled()
        {
            if (shotFired)
            {
                return;
            }
            shotFired = true;
            SpawnBullet();

            hero.WeaponKickback(1);
        }

        public override void TriggerReleased()
        {
            shotFired = false;
        }

        private void SpawnBullet()
        {
            new HandgunBullet(Scene, Transform.Position + Offset, CurrentFaceDirection);
        }
    }
}
