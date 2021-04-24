using lurum_dare_48.Source.Entities.Enemies;
using Microsoft.Xna.Framework;
using MonolithEngine;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Entities.Abstract;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Physics.Collision;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Global;
using MonolithEngine.Source.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Weapons.Guns
{
    class HandgunBullet : PhysicalEntity, IBullet
    {
        private Vector2 drawOffset = new Vector2(-4, -4);

        protected Vector2 ImpactForce = new Vector2(0.3f, 0);

        public HandgunBullet(AbstractScene scene, Vector2 position, Vector2 direction) : base(scene.LayerManager.EntityLayer, null, position)
        {

            Vector2 movement = direction - position;
            movement *= 0.01f;
            if (movement != Vector2.Zero )
            {
                movement.Normalize();
            }

            Velocity = movement;

            AddCollisionAgainst("Enemy");

            HorizontalFriction = 0f;
            VerticalFriction = 0f;

            HasGravity = false;

            AddComponent(new Sprite(this, AssetUtil.CreateRectangle(Config.GRID / 2, Color.Yellow), new Rectangle(0, 0, Config.GRID / 2, Config.GRID / 2), drawOffset));

            AddComponent(new BoxCollisionComponent(this, Config.GRID / 2, Config.GRID / 2, drawOffset));

            DEBUG_SHOW_COLLIDER = true;

            DEBUG_SHOW_PIVOT = true;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (CollidesOnGrid)
            {
                Destroy();
            }
        }

        public Vector2 GetImpactForce()
        {
            return ImpactForce;
        }

        public override void OnCollisionStart(IGameObject otherCollider)
        {
            if (otherCollider.HasTag("Enemy"))
            {
                Destroy();
                (otherCollider as AbstractEnemy).Hit(this);
                //otherCollider.Destroy();
            }

            base.OnCollisionStart(otherCollider);
        }
    }
}
