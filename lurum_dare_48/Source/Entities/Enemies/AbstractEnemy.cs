using Microsoft.Xna.Framework;
using MonolithEngine;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Physics.Collision;
using MonolithEngine.Engine.Source.Physics.Trigger;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Enemies
{
    class AbstractEnemy : PhysicalEntity
    {

        public Vector2 speed = new Vector2(0.05f, 0);

        public AbstractEnemy(AbstractScene scene, Vector2 position, Direction direction) : base(scene.LayerManager.EntityLayer, null, position)
        {
            AddTag("Enemy");
            CanFireTriggers = true;
            CurrentFaceDirection = direction;
        }

        public override void FixedUpdate()
        {
            if (CurrentFaceDirection == Direction.WEST)
            {
                Velocity += speed * -1;
            }
            else
            {
                Velocity += speed;
            }
            base.FixedUpdate();
        }
    }
}
