using Microsoft.Xna.Framework;
using MonolithEngine;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Weapons.Guns
{
    class ShotgunShell : PhysicalEntity
    {
        private bool isBumping = false;
        public ShotgunShell(AbstractScene scene, Vector2 position, float destoryTime = 2000, Vector2 force = default) : base(scene.LayerManager.EntityLayer, null, position)
        {
            DrawPriority = 20;
            AddComponent(new Sprite(this, Assets.GetTexture("ShotgunShell"), new Rectangle(0, 0, 3, 1), new Vector2(0, -1)));

            Velocity = force;

            CollisionOffsetBottom = 1;

            HorizontalFriction = 0.9f;
            VerticalFriction = 0.9f;

            //Bump(new Vector2(0, -1f));

            Timer.TriggerAfter(destoryTime, Destroy);
        }

        protected override void OnLand(Vector2 velocity)
        {
            if (isBumping)
            {
                return;
            }
            isBumping = true;
            Bump(new Vector2(0, -0.1f), true);
            base.OnLand(velocity);

        }
    }
}
