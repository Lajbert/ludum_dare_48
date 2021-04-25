using lurum_dare_48.Source.Entities.Enemies;
using Microsoft.Xna.Framework;
using MonolithEngine;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Entities.Abstract;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Physics.Collision;
using MonolithEngine.Engine.Source.Scene;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Weapons.Guns
{
    class MountedGunBullet : PhysicalEntity, IBullet
    {
        private Vector2 drawOffset = new Vector2(0, 1);

        protected Vector2 ImpactForce = new Vector2(0.3f, 0);

        public MountedGunBullet(AbstractScene scene, Vector2 position, Vector2 direction, float speedMultiplier = 1) : base(scene.LayerManager.EntityLayer, null, position)
        {

            Vector2 movement = direction - position;
            if (movement != Vector2.Zero)
            {
                movement.Normalize();
            }

            Velocity = movement * speedMultiplier;

            CollisionOffsetBottom = 1;
            CollisionOffsetLeft = 0;
            CollisionOffsetRight = 0;
            CollisionOffsetTop = 0;

            AddCollisionAgainst("Hero");

            HorizontalFriction = 0f;
            VerticalFriction = 0f;

            HasGravity = false;

            Sprite s = new Sprite(this, Assets.GetTexture("HandgunBulletPoint"), new Rectangle(0, 0, 2, 1), drawOffset);
            AddComponent(s);
            s.Scale = 5f;

            AddComponent(new BoxCollisionComponent(this, 2, 2, drawOffset));

            //DEBUG_SHOW_COLLIDER = true;

            //DEBUG_SHOW_PIVOT = true;
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
            if (otherCollider.HasTag("Hero"))
            {
                (otherCollider as Hero).Hit(this);
                //otherCollider.Destroy();
            }

            Destroy();
            base.OnCollisionStart(otherCollider);
        }

        public Vector2 GetPosition()
        {
            return Transform.Position;
        }
    }
}
