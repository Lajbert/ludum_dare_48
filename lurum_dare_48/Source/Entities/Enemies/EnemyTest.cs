using ForestPlatformerExample.Source.Entities.Enemies;
using lurum_dare_48.Source.Entities.Items;
using lurum_dare_48.Source.Entities.Traps;
using lurum_dare_48.Source.Entities.Weapons.Guns;
using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Entities.Abstract;
using MonolithEngine.Engine.Source.Entities.Animations;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Physics.Collision;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Global;
using MonolithEngine.Source.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Enemies
{
    class EnemyTest : AbstractEnemy
    {
        public List<string> CarriedItems = new List<string>();

        private Vector2 offset = new Vector2(0, -16);

        public EnemyTest(AbstractScene scene, Vector2 position, Direction direction) : base(scene, position, direction)
        {

            AddCollisionAgainst("Spikes");
            CurrentFaceDirection = Direction.EAST;
            AnimationStateMachine animations = new AnimationStateMachine();
            animations.Offset = offset;
            AddComponent(animations);
            SpriteSheetAnimation runLeft = new SpriteSheetAnimation(this, Assets.GetTexture("EnemyRun"), 32, 32, 40);
            animations.RegisterAnimation("RunLeft", runLeft, () => Velocity.X < 0 && CurrentFaceDirection == Direction.WEST);

            SpriteSheetAnimation runRight = runLeft.CopyFlipped();
            animations.RegisterAnimation("RunRight", runRight, () => Velocity.X > 0 && CurrentFaceDirection == Direction.EAST);

            animations.AddFrameTransition("RunLeft", "RunRight");

            SpriteSheetAnimation idleLeft = new SpriteSheetAnimation(this, Assets.GetTexture("EnemyIdle"), 32, 32, 1);
            animations.RegisterAnimation("IdleLeft", idleLeft, () => Velocity.X == 0 && CurrentFaceDirection == Direction.WEST);

            SpriteSheetAnimation idleRight = idleLeft.CopyFlipped();
            animations.RegisterAnimation("IdleRight", idleRight, () => Velocity.X == 0 && CurrentFaceDirection == Direction.EAST);

            SpriteSheetAnimation jumpLeft = new SpriteSheetAnimation(this, Assets.GetTexture("EnemyRun"), 1);
            jumpLeft.StartFrame = 6;
            jumpLeft.EndFrame = 7;
            animations.RegisterAnimation("JumpLeft", jumpLeft, () => !IsOnGround && CurrentFaceDirection == Direction.WEST, 1);

            SpriteSheetAnimation jumpRight = jumpLeft.CopyFlipped();
            animations.RegisterAnimation("JumpRight", jumpRight, () => !IsOnGround && CurrentFaceDirection == Direction.EAST, 1);

            CollisionOffsetBottom = 1;

            AddComponent(new BoxCollisionComponent(this, 18, 30, new Vector2(-8, -30)));

            if (!Tutorial)
            {
                //AddComponent(new BoxTrigger)
            }

            DEBUG_SHOW_COLLIDER = true;
        }

        public override void Destroy()
        {
            foreach (string item in CarriedItems)
            {
                if (item == "Key")
                {
                    DoorKey key = new DoorKey(Scene, Transform.Position - new Vector2(0, 10));
                    key.VelocityY -= 2;
                }
            }
            base.Destroy();
        }

        public override void FixedUpdate()
        {
            if (Patrol)
            {
                AIUtil.Patrol(true, this);
            }
            base.FixedUpdate();
        }

        public override void OnCollisionStart(IGameObject otherCollider)
        {
            if (otherCollider is Spikes)
            {
                Destroy();
            }
            base.OnCollisionStart(otherCollider);
        }
    }
}
