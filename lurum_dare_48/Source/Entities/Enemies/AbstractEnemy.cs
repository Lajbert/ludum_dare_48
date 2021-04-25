using lurum_dare_48.Source.Entities.Weapons.Guns;
using Microsoft.Xna.Framework;
using MonolithEngine;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Entities.Abstract;
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

        public Vector2 speed = new Vector2(0.02f, 0);

        public float Health = 10;

        public int MoveDirection = 1;

        public float DefaultSpeed = 0.05f;

        public float CurrentSpeed = 0.05f;

        public bool Static = false;

        public bool IsKicked = false;

        public bool Patrol = true;

        public bool Tutorial = false;

        public bool DamageEffect = false;

        public AbstractEnemy(AbstractScene scene, Vector2 position, Direction direction) : base(scene.LayerManager.EntityLayer, null, position)
        {
            AddTag("Enemy");
            CanFireTriggers = true;
            CurrentFaceDirection = direction;

            DrawPriority = 5;
        }

        public override void FixedUpdate()
        {
            if (!Static && Patrol)
            {
                /*if (CurrentFaceDirection == Direction.WEST)
                {
                    Velocity += speed * -1;
                }
                else
                {
                    Velocity += speed;
                }*/
            }
            base.FixedUpdate();
        }

        public virtual void Hit(IBullet bullet)
        {
            if (Health == 0)
            {
                Destroy();
                return;
            }

            if (bullet is HandgunBullet)
            {
                Health -= 2;
            }
            else if (bullet is MachineGunBullet)
            {
                Health = -1;
            }
            else if (bullet is ShotgunBullet)
            {
                Health -= 1;
            }

            if (DamageEffect)
            {
                Visible = false;
                Timer.TriggerAfter(50, () =>
                {
                    Visible = true;
                });
            }

            if (!Static)
            {
                if (bullet.GetPosition().X < Transform.X)
                {
                    Velocity += bullet.GetImpactForce();
                }
                else
                {
                    Velocity -= bullet.GetImpactForce();
                }
            }
        }

        public virtual void Hit(Hero hero)
        {
            if (Health == 0)
            {
                Destroy();
                return;
            }

            if (!Static)
            {
                if (hero.Transform.X < Transform.X)
                {
                    Velocity += new Vector2(3, 1);
                }
                else
                {
                    Velocity += new Vector2(-3, 1);
                }
            }
        }

        public override void Destroy()
        {
            Scene.Camera.Shake(5, 600);
            base.Destroy();
        }
    }
}
