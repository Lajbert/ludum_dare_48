using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Scene;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Weapons.Guns
{
    class ShotgunBullet : HandgunBullet
    {
        public ShotgunBullet(AbstractScene scene, Vector2 position, Vector2 direction) : base(scene, position, direction)
        {

        }
    }
}
