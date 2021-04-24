using lurum_dare_48.Source.Entities.Weapons.Guns;
using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Scene;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Enemies
{
    class MountedGunBullet : HandgunBullet
    {
        public MountedGunBullet(AbstractScene scene, Vector2 position, Vector2 direction) : base(scene, position, direction, 1)
        {
            ClearCollisionAgainst();
            AddTag("MountedGunBullet");
        }
    }
}
