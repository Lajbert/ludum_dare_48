﻿using lurum_dare_48.Source.Entities.Pickups;
using lurum_dare_48.Source.Entities.Weapons;
using lurum_dare_48.Source.Entities.Weapons.Guns;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonolithEngine;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Entities.Abstract;
using MonolithEngine.Engine.Source.Entities.Controller;
using MonolithEngine.Engine.Source.Global;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Physics.Collision;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities
{
    class Hero : PhysicalEntity
    {

        private readonly float JETPACK_SPEED = 0.1f;
        private readonly float JUMP_FORCE = 0.15f;
        private readonly float MAX_JETPACK_SPEED = 0;

        private readonly float MAX_JUMP = 0.6f;
        private static float TankCapacity = 1000;
        private float currentJump = 0;
        private float fuel = TankCapacity;

        private int jumpCount = 0;

        private bool flying = false;

        private Vector2 offset = new Vector2(-8, -16);

        public IWeapon CurrentWeapon;

        public Hero(AbstractScene scene, Vector2 position) : base(scene.LayerManager.EntityLayer, null, position)
        {

            //DEBUG_SHOW_PIVOT = true;

            AddCollisionAgainst("Pickup");

            SetupController();

            CollisionOffsetBottom = 1;
            CollisionOffsetLeft = 0.5f;
            CollisionOffsetRight = 0.5f;
            CollisionOffsetTop = 1;

            CurrentFaceDirection = Direction.EAST;

            AddComponent(new Sprite(this, AssetUtil.CreateRectangle(Config.GRID, Color.Red), drawOffset: offset));

            Visible = true;
            Active = true;

            DebugFunction = () =>
            {
                return "Fuel: " + (int)(((float)(fuel / TankCapacity)) * 100) + " %";
            };

            AddComponent(new BoxCollisionComponent(this, Config.GRID, Config.GRID, offset));

            CurrentWeapon = new Handgun(scene, this);
        }

        private void SetupController()
        {
            UserInput = new UserInputController();

            UserInput.RegisterKeyPressAction(Keys.Right, (Vector2 thumbStickPosition) =>
            {
                VelocityX += MovementSpeed * Globals.FixedUpdateMultiplier * Config.TIME_OFFSET;
                CurrentFaceDirection = Direction.EAST;
                CurrentWeapon?.SetDirection(CurrentFaceDirection);
            });

            UserInput.RegisterKeyPressAction(Keys.Left, Buttons.LeftThumbstickLeft, (Vector2 thumbStickPosition) =>
            {
                VelocityX -= MovementSpeed * Globals.FixedUpdateMultiplier * Config.TIME_OFFSET;
                CurrentFaceDirection = Direction.WEST;
                CurrentWeapon?.SetDirection(CurrentFaceDirection);
            });

            UserInput.RegisterKeyPressAction(Keys.Up, (Vector2 thumbStickPosition) =>
            {
                if (jumpCount < 2 && currentJump < MAX_JUMP)
                {
                    VelocityY -= JUMP_FORCE;
                    currentJump += JUMP_FORCE;
                    VelocityY -= JUMP_FORCE;
                    FallSpeed = 0;
                    //jumpCount++;
                }
            });

            UserInput.RegisterKeyReleaseAction(Keys.Up, () =>
            {
                //if (jumpCount < 2)
                {
                    currentJump = 0;
                    jumpCount++;
                }
                if (IsOnGround)
                {
                    //jumpCount = 0;
                }
            });

            /*UserInput.RegisterKeyPressAction(Keys.Down, Buttons.LeftThumbstickLeft, (Vector2 thumbStickPosition) =>
            {
                VelocityY += MovementSpeed * Globals.FixedUpdateMultiplier * Config.TIME_OFFSET;
            });*/

            UserInput.RegisterKeyPressAction(new List<Keys> { Keys.LeftShift, Keys.RightShift }, (Vector2 thumbStickPosition) =>
            {
                if (fuel > 0)
                {
                    VelocityY -= JETPACK_SPEED;
                    fuel -= JETPACK_SPEED;
                    flying = true;
                    if (fuel < 0)
                    {
                        fuel = 0;
                    }
                } else
                {
                    flying = false;
                }
                
            });

            UserInput.RegisterKeyReleaseAction(new List<Keys> { Keys.LeftShift, Keys.RightShift }, () =>
            {
                flying = false;
            });

            UserInput.RegisterMouseActions(
                () =>
                {
                    Timer.Repeat(300, (elapsedTime) =>
                    {
                        Scene.Camera.Zoom += 0.002f * elapsedTime;
                    });
                },
                () =>
                {
                    Timer.Repeat(300, (elapsedTime) =>
                    {
                        Scene.Camera.Zoom -= 0.002f * elapsedTime;
                    });
                }
            , 100);

            UserInput.RegisterKeyPressAction(Keys.Space, (Vector2 thumbStickPosition) =>
            {
                if (CurrentWeapon != null && !CurrentWeapon.IsEmpty())
                {
                    CurrentWeapon.TriggerPulled();
                }

            });

            UserInput.RegisterKeyReleaseAction(Keys.Space, () =>
            {
                if (CurrentWeapon != null && !CurrentWeapon.IsEmpty())
                {
                    CurrentWeapon.TriggerReleased();
                }

            });

            UserInput.RegisterKeyPressAction(Keys.NumPad1, (Vector2 thumbStickPosition) =>
            {
                CurrentWeapon?.Destroy();
                CurrentWeapon = new Handgun(Scene, this);

            }, true);

            UserInput.RegisterKeyPressAction(Keys.NumPad2, (Vector2 thumbStickPosition) =>
            {
                CurrentWeapon?.Destroy();
                CurrentWeapon = new Machinegun(Scene, this);

            }, true);

            UserInput.RegisterKeyPressAction(Keys.NumPad3, (Vector2 thumbStickPosition) =>
            {
                CurrentWeapon?.Destroy();
                CurrentWeapon = new Shotgun(Scene, this);

            }, true);
        }

        public override void OnCollisionStart(IGameObject otherCollider)
        {
            if (otherCollider is Fuel)
            {
                AddFuel((otherCollider as Fuel).Amount);
                otherCollider.Destroy();
            }

            base.OnCollisionStart(otherCollider);
        }

        public override void FixedUpdate()
        {
            if (flying)
            {
                FallSpeed = 0;
            }

            base.FixedUpdate();
        }

        protected override void OnLand(Vector2 velocity)
        {
            base.OnLand(velocity);
            jumpCount = 0;
            currentJump = 0;
        }

        public void AddFuel(float amount)
        {
            fuel = Math.Max(TankCapacity, fuel + amount);
        }

        public void WeaponKickback(float forceX)
        {
            if (IsOnGround)
            {
                return;
            }
            if (CurrentFaceDirection == Direction.EAST)
            {
                VelocityX -= forceX;
            }
            else
            {
                VelocityX += forceX;
            }
        }
    }
}