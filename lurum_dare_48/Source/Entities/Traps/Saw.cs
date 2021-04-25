using Microsoft.Xna.Framework;
using MonolithEngine;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Physics.Collision;
using MonolithEngine.Engine.Source.Scene;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Traps
{
    class Saw : PhysicalEntity
    {
        private Sprite sprite;

        private Vector2 offset;

        public Vector2 Movement = Vector2.Zero;

        private bool horizontal;

        public Saw(AbstractScene scene, Vector2 position, bool horizontal) : base(scene.LayerManager.EntityLayer, null, position)
        {
            AddTag("Trap");
            this.horizontal = horizontal;
            HorizontalFriction = 0;
            VerticalFriction = 0;

            if (horizontal)
            {
                Movement = new Vector2(0.1f, 0);
                offset = new Vector2(0, 8);
            }
            else
            {
                Movement = new Vector2(0, 0.1f);
                offset = new Vector2(8, 0);
            }
            Velocity = Movement;
            HasGravity = false;
            CheckGridCollisions = false;
            sprite = new Sprite(this, Assets.GetTexture("Saw"), drawOffset: offset, origin: new Vector2(8, 8));
            AddComponent(sprite);
            CanFireTriggers = true;

            AddComponent(new CircleCollisionComponent(this, 8, offset));
            //DEBUG_SHOW_COLLIDER = true;
            //DEBUG_SHOW_PIVOT = true;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (horizontal)
            {
                if (Velocity.X < 0)
                {
                    sprite.Rotation -= 0.1f;
                }
                else
                {
                    sprite.Rotation += 0.1f;
                }
            }
            else
            {
                if (Velocity.Y < 0)
                {
                    sprite.Rotation += 0.1f;
                }
                else
                {
                    sprite.Rotation -= 0.1f;
                }
            }
        }
    }
}
