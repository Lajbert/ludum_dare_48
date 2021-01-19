﻿using ForestPlatformerExample.Source.Items;
using GameEngine2D;
using GameEngine2D.Engine.Source.Entities;
using GameEngine2D.Engine.Source.Entities.Animations;
using GameEngine2D.Engine.Source.Entities.Controller;
using GameEngine2D.Engine.Source.Util;
using GameEngine2D.Entities;
using GameEngine2D.Global;
using GameEngine2D.Source.Entities;
using GameEngine2D.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForestPlatformerExample.Source.Hero
{
    class Hero : ControllableEntity
    {

        private readonly float JUMP_RATE = 0.1f;
        private static double lastJump = 0f;
        private bool doubleJumping = false;

        public Hero(Vector2 position, SpriteFont font = null) : base(LayerManager.Instance.EntityLayer, null, position, null, true, font)
        {

            //DEBUG_SHOW_PIVOT = true;

            SetupAnimations();

            SetupController();

            foreach (GridDirection direction in new List<GridDirection>() { GridDirection.CENTER, GridDirection.UP, GridDirection.DOWN, GridDirection.LEFT, GridDirection.RIGHT })
            {
                CollisionCheckDirections.Add(direction);
            }

        }

        private void SetupAnimations()
        {
            Animations = new AnimationStateMachine();
            //Animations.Offset = new Vector2(3, -20);
            Animations.Offset = new Vector2(0, -27);

            CollisionOffsetRight = 0.5f;
            CollisionOffsetLeft = 0f;
            CollisionOffsetBottom = 0.4f;
            CollisionOffsetTop = 0.5f;

            Texture2D spriteSheet = SpriteUtil.LoadTexture("Green_Greens_Forest_Pixel_Art_Platformer_Pack/Character-Animations/Main-Character/Sprite-Sheets/main-character@idle-sheet");
            SpriteSheetAnimation idleRight = new SpriteSheetAnimation(this, spriteSheet, 3, 10, 24, 64, 64, 24);
            
            Func<bool> isIdleRight = () => CurrentFaceDirection == GridDirection.RIGHT;
            Animations.RegisterAnimation("IdleRight", idleRight, isIdleRight);

            SpriteSheetAnimation idleLeft = new SpriteSheetAnimation(this, spriteSheet, 3, 10, 24, 64, 64, 24, SpriteEffects.FlipHorizontally);
            Func<bool> isIdleLeft = () => CurrentFaceDirection == GridDirection.LEFT;
            Animations.RegisterAnimation("IdleLeft", idleLeft, isIdleLeft);

            spriteSheet = SpriteUtil.LoadTexture("Green_Greens_Forest_Pixel_Art_Platformer_Pack/Character-Animations/Main-Character/Sprite-Sheets/main-character@run-sheet");
            SpriteSheetAnimation runningRight = new SpriteSheetAnimation(this, spriteSheet, 1, 10, 10, 64, 64, 24);
            Func<bool> isRunningRight = () => Direction.X > 0.5f && !CollisionChecker.HasBlockingColliderAt(GridUtil.GetRightGrid(GridCoordinates));
            Animations.RegisterAnimation("RunningRight", runningRight, isRunningRight, 1);

            SpriteSheetAnimation runningLeft = new SpriteSheetAnimation(this, spriteSheet, 1, 10, 10, 64, 64, 24, SpriteEffects.FlipHorizontally);
            Func<bool> isRunningLeft = () => Direction.X < -0.5f && !CollisionChecker.HasBlockingColliderAt(GridUtil.GetLeftGrid(GridCoordinates));
            Animations.RegisterAnimation("RunningLeft", runningLeft, isRunningLeft, 1);

            spriteSheet = SpriteUtil.LoadTexture("Green_Greens_Forest_Pixel_Art_Platformer_Pack/Character-Animations/Main-Character/Sprite-Sheets/main-character@jump-sheet");
            SpriteSheetAnimation jumpRight = new SpriteSheetAnimation(this, spriteSheet, 2, 10, 11, 64, 64, 24);
            jumpRight.Looping = false;
            Func<bool> isJumpingRight = () => FallStartedAt > 0f && CurrentFaceDirection == GridDirection.RIGHT;
            Animations.RegisterAnimation("JumpingRight", jumpRight, isJumpingRight, 2);

            SpriteSheetAnimation jumpLeft = new SpriteSheetAnimation(this, spriteSheet, 2, 10, 11, 64, 64, 24, SpriteEffects.FlipHorizontally);
            jumpLeft.Looping = false;
            Func<bool> isJumpingLeft = () => FallStartedAt > 0f && CurrentFaceDirection == GridDirection.LEFT;
            Animations.RegisterAnimation("JumpingLeft", jumpLeft, isJumpingLeft, 2);

            Animations.AddFrameTransition("JumpingRight", "JumpingLeft");

            spriteSheet = SpriteUtil.LoadTexture("Green_Greens_Forest_Pixel_Art_Platformer_Pack/Character-Animations/Main-Character/Sprite-Sheets/main-character@wall-slide-sheet");
            SpriteSheetAnimation wallSlideRight = new SpriteSheetAnimation(this, spriteSheet, 1, 6, 6, 64, 64, 12, SpriteEffects.FlipHorizontally);
            Func<bool> isSlidingRight = () => JumpModifier != Vector2.Zero && CurrentFaceDirection == GridDirection.RIGHT;
            Animations.RegisterAnimation("WallSlideRight", wallSlideRight, isSlidingRight, 6);

            SpriteSheetAnimation wallSlideLeft = new SpriteSheetAnimation(this, spriteSheet, 1, 6, 6, 64, 64, 12);
            Func<bool> isSlidingLeft = () => JumpModifier != Vector2.Zero && CurrentFaceDirection == GridDirection.LEFT;
            Animations.RegisterAnimation("WallSlideLeft", wallSlideLeft, isSlidingLeft, 6);

            spriteSheet = SpriteUtil.LoadTexture("Green_Greens_Forest_Pixel_Art_Platformer_Pack/Character-Animations/Main-Character/Sprite-Sheets/main-character@double-jump-sheet");
            SpriteSheetAnimation doubleJumpRight = new SpriteSheetAnimation(this, spriteSheet, 3, 10, 16, 64, 64, 12);
            doubleJumpRight.StartFrame = 12;
            doubleJumpRight.EndFrame = 16;
            Func<bool> isDoubleJumpingRight = () => doubleJumping && CurrentFaceDirection == GridDirection.RIGHT;
            Animations.RegisterAnimation("DoubleJumpingRight", doubleJumpRight, isDoubleJumpingRight, 3);

            SpriteSheetAnimation doubleJumpLeft = new SpriteSheetAnimation(this, spriteSheet, 3, 10, 16, 64, 64, 12, SpriteEffects.FlipHorizontally);
            doubleJumpLeft.StartFrame = 12;
            doubleJumpLeft.EndFrame = 16;
            Func<bool> isDoubleJumpingLeft = () => doubleJumping && CurrentFaceDirection == GridDirection.LEFT;
            Animations.RegisterAnimation("DoubleJumpingLeft", doubleJumpLeft, isDoubleJumpingLeft, 3);

            Animations.AddFrameTransition("DoubleJumpingRight", "DoubleJumpingLeft");

            spriteSheet = SpriteUtil.LoadTexture("Green_Greens_Forest_Pixel_Art_Platformer_Pack/Character-Animations/Main-Character/Sprite-Sheets/main-character@climb-sheet");
            SpriteSheetAnimation climb = new SpriteSheetAnimation(this, spriteSheet, 2, 10, 12, 64, 64, 60);
            Func<bool> isClimbing = () => !HasGravity;
            Func<bool> isHangingOnLadder = () => (Math.Abs(Direction.X) < 0.5f && Math.Abs(Direction.Y) < 0.5f);
            climb.AnimationPauseCondition = isHangingOnLadder;
            Animations.RegisterAnimation("HangingOnLadder", climb, isClimbing, 6);

            spriteSheet = SpriteUtil.LoadTexture("Green_Greens_Forest_Pixel_Art_Platformer_Pack/Character-Animations/Main-Character/Sprite-Sheets/main-character@jump-sheet");
            SpriteSheetAnimation fallingRight = new SpriteSheetAnimation(this, spriteSheet, 2, 10, 13, 64, 64, 24);
            fallingRight.StartFrame = 9;
            fallingRight.EndFrame = 11;
            Func<bool> isFallingRight = () => HasGravity && Direction.Y > 0.1 && CurrentFaceDirection == GridDirection.RIGHT;
            Animations.RegisterAnimation("FallingRight", fallingRight, isFallingRight, 5);

            SpriteSheetAnimation fallingLeft = new SpriteSheetAnimation(this, spriteSheet, 2, 10, 13, 64, 64, 24, SpriteEffects.FlipHorizontally);
            fallingLeft.StartFrame = 9;
            fallingLeft.EndFrame = 11;
            Func<bool> isFallingLeft = () => HasGravity && Direction.Y > 0.1 && CurrentFaceDirection == GridDirection.LEFT;
            Animations.RegisterAnimation("FallingLeft", fallingLeft, isFallingLeft, 5);

            Animations.AddFrameTransition("FallingRight", "FallingLeft");

            //SetSprite(spriteSheet);
            /*SetSprite(SpriteUtil.CreateRectangle(Config.GRID, Color.Red));
            DrawOffset = new Vector2(15, 15);*/
        }

        private void SetupController()
        {
            UserInput = new UserInputController();

            UserInput.RegisterControllerState(Keys.Right, () => {
                Direction.X += MovementSpeed * elapsedTime;
                CurrentFaceDirection = GridDirection.RIGHT;
            });

            UserInput.RegisterControllerState(Keys.Left, () => {
                Direction.X -= MovementSpeed * elapsedTime;
                CurrentFaceDirection = GridDirection.LEFT;
            });

            UserInput.RegisterControllerState(Keys.Up, () => {
                if (!HasGravity || (!canJump && !canDoubleJump))
                {
                    return;
                }
                if (canJump)
                {
                    canDoubleJump = true;
                    canJump = false;
                }
                else
                {
                    if (lastJump < JUMP_RATE)
                    {
                        return;
                    }
                    lastJump = 0f;
                    canDoubleJump = false;
                    doubleJumping = true;
                }

                Direction.Y -= Config.JUMP_FORCE + JumpModifier.Y;
                Direction.X += JumpModifier.X;
                if (JumpModifier.X < 0)
                {
                    CurrentFaceDirection = GridDirection.LEFT;
                } else if (JumpModifier.X > 0)
                {
                    CurrentFaceDirection = GridDirection.RIGHT;
                }
                JumpModifier = Vector2.Zero;
                FallStartedAt = (float)GameTime.TotalGameTime.TotalSeconds;
            }, true);

            UserInput.RegisterControllerState(Keys.Down, () => {
                if (HasGravity)
                {
                    if (CollisionChecker.HasObjectAtWithTag(GridUtil.GetBelowGrid(GridCoordinates), "Platform") && CollisionChecker.GetColliderAt(GridUtil.GetBelowGrid(GridCoordinates)).BlocksMovement) {
                        CollisionChecker.GetColliderAt(GridUtil.GetBelowGrid(GridCoordinates)).BlocksMovement = false;
                    }
                }
                //CurrentFaceDirection = GridDirection.DOWN;
            }, true);

            UserInput.RegisterControllerState(Keys.Down, () => {
                if (HasGravity)
                {
                    return;
                }
                Direction.Y += MovementSpeed * elapsedTime;
                //CurrentFaceDirection = GridDirection.DOWN;
            });

            UserInput.RegisterControllerState(Keys.Up, () => {
                if (HasGravity)
                {
                    return;
                }
                Direction.Y -= MovementSpeed * elapsedTime;
                //CurrentFaceDirection = GridDirection.UP;
            });

            UserInput.RegisterMouseActions(() => { Config.ZOOM += 0.1f; /*Globals.Camera.Recenter(); */ }, () => { Config.ZOOM -= 0.1f; /*Globals.Camera.Recenter(); */});
        }

        public override void Update(GameTime gameTime)
        {
            if (FallStartedAt > 0)
            {
                lastJump += gameTime.ElapsedGameTime.TotalSeconds;
            } else
            {
                doubleJumping = false;
            }
            base.Update(gameTime);
        }

        protected override void OnCollisionStart(Entity otherCollider)
        {
            if (otherCollider is Coin)
            {
                otherCollider.Destroy();
            }
        }

        protected override void OnCollisionEnd(Entity otherCollider)
        {
            if (otherCollider.HasTag("Platform") && !otherCollider.BlocksMovement)
            {
                otherCollider.BlocksMovement = true;
            }
        }
    }
}
