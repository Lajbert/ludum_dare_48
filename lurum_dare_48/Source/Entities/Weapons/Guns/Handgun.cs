using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Global;
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
