using lurum_dare_48.Source.Entities.Weapons.Guns;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonolithEngine;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Entities.Abstract;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Physics.Bresenham;
using MonolithEngine.Engine.Source.Physics.Collision;
using MonolithEngine.Engine.Source.Physics.Trigger;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Global;
using MonolithEngine.Source.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Enemies
{
    class MountedGun : AbstractEnemy
    {
        private bool seesHero = false;

        private List<Vector2> line = new List<Vector2>();

        private Hero hero;

        private Vector2 middleOffset = new Vector2(0, 0);

        private Sprite sprite;

        public MountedGun(AbstractScene scene, Vector2 position, Direction direction, int triggerWidth, int triggerHeight, int triggerXoffset, int triggerYoffset) : base (scene, position, direction)
        {
            AddTag("Enemy");
            Static = true;
            sprite = new Sprite(this, Assets.GetTexture("MountedGun"));
            sprite.Origin = new Vector2(4, 8);
            if (direction == Direction.WEST)
            {
                sprite.BaseRotation = MathUtil.DegreesToRad(180);
            }
            else if (direction == Direction.SOUTH)
            {
                sprite.BaseRotation = MathUtil.DegreesToRad(90);
            }
            AddComponent(sprite);

            AddComponent(new BoxTrigger(this, triggerWidth, triggerHeight, new Vector2(triggerXoffset, triggerYoffset), tag: ""));

            AddComponent(new BoxCollisionComponent(this, 16, 16, new Vector2(-8, -8)));
            //DEBUG_SHOW_COLLIDER = true;

            HasGravity = false;

            //(GetComponent<ITrigger>() as BoxTrigger).DEBUG_DISPLAY_TRIGGER = true;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (BeingDestroyed || Destroyed)
            {
                return;
            }

            if (hero != null)
            {
                line.Clear();
                Bresenham.GetLine(Transform.Position + middleOffset, hero.Transform.Position + new Vector2(0, -8), line);
                seesHero = Bresenham.CanLinePass(Transform.Position + middleOffset, hero.Transform.Position + new Vector2(0, -8), (x, y) => {
                    return Scene.GridCollisionChecker.HasBlockingColliderAt(new Vector2(x / Config.GRID, y / Config.GRID), Direction.CENTER);
                });

                if (seesHero)
                {
                    //seesHero &= (CurrentFaceDirection == Direction.EAST && hero.Transform.X > Transform.X || CurrentFaceDirection == Direction.WEST && hero.Transform.X < Transform.X);
                    if (hero.Transform.X < Transform.X)
                    {
                        CurrentFaceDirection = Direction.WEST;
                    }
                    else
                    {
                        CurrentFaceDirection = Direction.EAST;
                    }
                }

            }
            else
            {
                seesHero = false;
            }

            if (seesHero)
            {
                sprite.Rotation = MathUtil.RadFromVectors(Transform.Position, hero.Transform.Position) - sprite.BaseRotation;
                Fire();
            }

        }

        public void Fire()
        {
            if (Timer.IsSet("MountedGunFiring" + GetID()))
            {
                return;
            }
            Timer.SetTimer("MountedGunFiring" + GetID(), 1000);
            new MountedGunBullet(Scene, Transform.Position + middleOffset, hero.Transform.Position);
        }

        public override void OnEnterTrigger(string triggerTag, IGameObject otherEntity)
        {
            if (otherEntity is Hero)
            {
                   hero = otherEntity as Hero;
            }

        }

        public override void OnLeaveTrigger(string triggerTag, IGameObject otherEntity)
        {
            line.Clear();
            if (otherEntity is Hero)
            {
                hero = null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
#if DEBUG
            if (seesHero)
            {
                foreach (Vector2 point in line)
                {
                    spriteBatch.Draw(Assets.CreateRectangle(1, Color.Red), point, Color.White);
                }
            }
            else
            {
                foreach (Vector2 point in line)
                {
                    spriteBatch.Draw(Assets.CreateRectangle(1, Color.Blue), point, Color.White);
                }
            }
            //line.Clear();
#endif
        }
    }
}
