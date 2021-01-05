﻿using GameEngine2D.Engine.src.Entities;
using GameEngine2D.Engine.src.Entities.Animations;
using GameEngine2D.Engine.src.Entities.Controller;
using GameEngine2D.Engine.src.Util;
using GameEngine2D.Entities;
using GameEngine2D.Global;
using GameEngine2D.src;
using GameEngine2D.src.Entities.Animation;
using GameEngine2D.src.Layer;
using GameEngine2D.src.Util;
using GameEngine2D.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SideScrollerExample.SideScroller.src.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine2D.GameExamples.SideScroller.src.Hero
{
    class Knight : ControllableEntity
    {

        private readonly float SHOOT_RATE = 0.1f;

        private float scale = 2.5f;
        private Rectangle sourceRectangle = new Rectangle(0, 0, 100, 55);
        private Vector2 spriteOffset = new Vector2(70f, 15f) * -1;
        private int animationFps = 30;

        private AnimationStateMachine animations;

        private static double lastBulletInSeconds = 0f;

        public Knight(ContentManager contentManager, Vector2 position, SpriteFont font = null) : base(Scene.Instance.RayBlockersLayer, null, position, font)
        {

            SetupAnimations(contentManager);
            SetupController();
            CollisionOffsetRight = 0.35f;
            CollisionOffsetLeft = 0.5f;
            CollisionOffsetBottom = 0.2f;
            CollisionOffsetTop = 0.7f;
            animations.Offset = new Vector2(-120f, -80f);

        }

        private void SetupController()
        {
            UserInput = new UserInputController();

            UserInput.RegisterControllerState(Keys.Right, () => {
                Direction.X += Config.CHARACTER_SPEED * elapsedTime;
                CurrentFaceDirection = Engine.src.Entities.FaceDirection.RIGHT;
            });

            UserInput.RegisterControllerState(Keys.Left, () => {
                Direction.X -= Config.CHARACTER_SPEED * elapsedTime;
                CurrentFaceDirection = Engine.src.Entities.FaceDirection.LEFT;
            });

            UserInput.RegisterControllerState(Keys.Space, () => {
                if (!HasGravity || (!canJump && !doubleJump))
                {
                    return;
                }
                if (canJump)
                {
                    doubleJump = true;
                }
                else
                {
                    doubleJump = false;
                }
                canJump = false;
                Direction.Y -= Config.JUMP_FORCE;
                JumpStart = (float)gameTime.TotalGameTime.TotalSeconds;
            }, true);

            UserInput.RegisterControllerState(Keys.Down, () => {
                if (HasGravity)
                {
                    return;
                }
                Direction.Y += Config.CHARACTER_SPEED * elapsedTime;
                CurrentFaceDirection = Engine.src.Entities.FaceDirection.DOWN;
            });

            UserInput.RegisterControllerState(Keys.Up, () => {
                if (HasGravity)
                {
                    return;
                }
                Direction.Y -= Config.CHARACTER_SPEED * elapsedTime;
                CurrentFaceDirection = Engine.src.Entities.FaceDirection.UP;
            });

            Action shoot = () =>
            {
                if (lastBulletInSeconds >= SHOOT_RATE)
                {
                    lastBulletInSeconds = 0;
                    new Bullet(this, CurrentFaceDirection);
                }
            };

            UserInput.RegisterControllerState(Keys.RightShift, shoot, true);
            UserInput.RegisterControllerState(Keys.LeftShift, shoot, true);
        }

        public override void Update(GameTime gameTime)
        {
            lastBulletInSeconds += gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        private void SetupAnimations(ContentManager contentManager)
        {
            animations = new AnimationStateMachine();

            string folder = "SideScroller/KnightAssets/HeroKnight/";

            List<Texture2D> knightIdle = SpriteUtil.LoadTextures(folder + "Idle/HeroKnight_Idle_", 7, contentManager);

            AnimatedSpriteGroup knightAnimationIdleRight = new AnimatedSpriteGroup(knightIdle, this, SpriteBatch, sourceRectangle, animationFps);
            knightAnimationIdleRight.Scale = scale;
            knightAnimationIdleRight.Offset = spriteOffset;
            Func<bool> isIdleRight = () => CurrentFaceDirection == Engine.src.Entities.FaceDirection.RIGHT;
            animations.RegisterAnimation("IdleRight", knightAnimationIdleRight, isIdleRight);

            AnimatedSpriteGroup knightAnimationIdleLeft = new AnimatedSpriteGroup(knightIdle, this, SpriteBatch, sourceRectangle, animationFps, SpriteEffects.FlipHorizontally);
            knightAnimationIdleLeft.Scale = scale;
            knightAnimationIdleLeft.Offset = spriteOffset;
            Func<bool> isIdleLeft = () => CurrentFaceDirection == Engine.src.Entities.FaceDirection.LEFT;
            animations.RegisterAnimation("IdleLeft", knightAnimationIdleLeft, isIdleLeft);

            List<Texture2D> knightRun = SpriteUtil.LoadTextures(folder + "Run/HeroKnight_Run_", 7, contentManager);
            AnimatedSpriteGroup knightRunRightAnimation = new AnimatedSpriteGroup(knightRun, this, SpriteBatch, sourceRectangle, animationFps);
            knightRunRightAnimation.Scale = scale;
            knightRunRightAnimation.Offset = spriteOffset;
            Func<bool> isRunningRight = () => Direction.X > 0.5f && !Scene.Instance.HasColliderAt(GridUtil.GetRightGrid(GridCoordinates));
            animations.RegisterAnimation("RunRight", knightRunRightAnimation, isRunningRight, 1);

            AnimatedSpriteGroup knightRunLeftAnimation = new AnimatedSpriteGroup(knightRun, this, SpriteBatch, sourceRectangle, animationFps, SpriteEffects.FlipHorizontally);
            knightRunLeftAnimation.Scale = scale;
            knightRunLeftAnimation.Offset = spriteOffset;
            Func<bool> isRunningleft = () => Direction.X < -0.5f && !Scene.Instance.HasColliderAt(GridUtil.GetLeftGrid(GridCoordinates));
            animations.RegisterAnimation("RunLeft", knightRunLeftAnimation, isRunningleft, 1);

            List<Texture2D> knightJump = SpriteUtil.LoadTextures(folder + "Jump/HeroKnight_Jump_", 2, contentManager);
            AnimatedSpriteGroup knightJumpRightAnimation = new AnimatedSpriteGroup(knightJump, this, SpriteBatch, sourceRectangle, animationFps);
            knightJumpRightAnimation.Scale = scale;
            knightJumpRightAnimation.Offset = spriteOffset;
            Func<bool> isJumpingRight = () => JumpStart > 0f && CurrentFaceDirection == Engine.src.Entities.FaceDirection.RIGHT;
            animations.RegisterAnimation("JumpRight", knightJumpRightAnimation, isJumpingRight, 2);

            AnimatedSpriteGroup knightJumpLeftAnimation = new AnimatedSpriteGroup(knightJump, this, SpriteBatch, sourceRectangle, animationFps, SpriteEffects.FlipHorizontally);
            knightJumpLeftAnimation.Scale = scale;
            knightJumpLeftAnimation.Offset = spriteOffset;
            Func<bool> isJumpingLeft = () => JumpStart > 0f && CurrentFaceDirection == Engine.src.Entities.FaceDirection.LEFT;
            animations.RegisterAnimation("JumpLeft", knightJumpLeftAnimation, isJumpingLeft, 2);

            List<Texture2D> knightFall = SpriteUtil.LoadTextures(folder + "Fall/HeroKnight_Fall_", 3, contentManager);
            AnimatedSpriteGroup knightFallRightAnimation = new AnimatedSpriteGroup(knightFall, this, SpriteBatch, sourceRectangle, animationFps);
            knightFallRightAnimation.Scale = scale;
            knightFallRightAnimation.Offset = spriteOffset;
            Func<bool> isFallingRight = () => Direction.Y > 0f && CurrentFaceDirection == Engine.src.Entities.FaceDirection.RIGHT;
            animations.RegisterAnimation("FallRight", knightFallRightAnimation, isFallingRight, 3);

            AnimatedSpriteGroup knightFallLeftAnimation = new AnimatedSpriteGroup(knightFall, this, SpriteBatch, sourceRectangle, animationFps, SpriteEffects.FlipHorizontally);
            knightFallLeftAnimation.Scale = scale;
            knightFallLeftAnimation.Offset = spriteOffset;
            Func<bool> isFallingLeftt = () => Direction.Y > 0f && CurrentFaceDirection == Engine.src.Entities.FaceDirection.LEFT;
            animations.RegisterAnimation("FallLeft", knightFallLeftAnimation, isFallingLeftt, 3);

            //SetSprite(SpriteUtil.CreateRectangle(graphicsDevice, Constants.GRID, Color.Black));

            Animations = animations;
        }
    }
}
