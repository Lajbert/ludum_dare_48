using lurum_dare_48.Source.Entities.Enemies;
using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Entities.Abstract;
using MonolithEngine.Engine.Source.Physics.Trigger;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Triggers
{
    class EnemyPatrolTrigger : Entity
    {
        private float width;

        public EnemyPatrolTrigger(AbstractScene scene, int width, int height, Vector2 position) : base (scene.LayerManager.EntityLayer, null, position)
        {
            AddComponent(new BoxTrigger(this, width, height, Vector2.Zero, tag: ""));
            Visible = true;
            Active = true;
            DEBUG_SHOW_COLLIDER = true;
            this.width = width;
        }

        public override void OnLeaveTrigger(string triggerTag, IGameObject otherEntity)
        {
            if (otherEntity is AbstractEnemy)
            {
                AbstractEnemy enemy = (otherEntity as AbstractEnemy);
                if (enemy.Transform.Position.X > Transform.Position.X + (width/2))
                {
                    enemy.CurrentFaceDirection = Direction.WEST;
                }
                else
                {
                    enemy.CurrentFaceDirection = Direction.EAST;
                }
            }
            base.OnLeaveTrigger(triggerTag, otherEntity);
        }
    }
}
