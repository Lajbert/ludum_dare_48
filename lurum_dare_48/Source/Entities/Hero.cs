using lurum_dare_48.Source.Entities.Environment;
using lurum_dare_48.Source.Entities.Items;
using lurum_dare_48.Source.Entities.Pickups;
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
using MonolithEngine.Source.Util;
using MonolithEngine.Util;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<IGameObject> CarriedItems = new List<IGameObject>();

        public Hero(AbstractScene scene, Vector2 position) : base(scene.LayerManager.EntityLayer, null, position)
        {

            //DEBUG_SHOW_PIVOT = true;

            CanFireTriggers = true;

            AddCollisionAgainst("Pickup");
            AddCollisionAgainst("Door", false);
            AddCollisionAgainst("Key");

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

            UserInput.RegisterKeyPressAction(Keys.D, (Vector2 thumbStickPosition) =>
            {
                VelocityX += MovementSpeed * Globals.FixedUpdateMultiplier * Config.TIME_OFFSET;
            });

            UserInput.RegisterKeyPressAction(Keys.A, Buttons.LeftThumbstickLeft, (Vector2 thumbStickPosition) =>
            {
                VelocityX -= MovementSpeed * Globals.FixedUpdateMultiplier * Config.TIME_OFFSET;
            });

            UserInput.RegisterKeyPressAction(Keys.W, (Vector2 thumbStickPosition) =>
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

            UserInput.RegisterKeyReleaseAction(Keys.W, () =>
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

            UserInput.LeftClickPressedAction = (mousePosition) =>
            {
                if (CurrentWeapon != null && !CurrentWeapon.IsEmpty())
                {
                    CurrentWeapon.TriggerPulled(Scene.Camera.ScreenToWorldSpace(mousePosition));
                }
            };

            UserInput.LeftClickUpAction = (mousePosition) =>
            {
                if (CurrentWeapon != null && !CurrentWeapon.IsEmpty())
                {
                    CurrentWeapon.TriggerReleased(Scene.Camera.ScreenToWorldSpace(mousePosition));
                }
            };



            /*UserInput.RegisterKeyPressAction(Keys.Down, Buttons.LeftThumbstickLeft, (Vector2 thumbStickPosition) =>
            {
                VelocityY += MovementSpeed * Globals.FixedUpdateMultiplier * Config.TIME_OFFSET;
            });*/

            UserInput.RegisterKeyPressAction(Keys.Space, (Vector2 thumbStickPosition) =>
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

            UserInput.RegisterKeyReleaseAction(Keys.Space, () =>
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

            /*UserInput.RegisterKeyPressAction(Keys.Space, (Vector2 thumbStickPosition) =>
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

            });*/

            UserInput.RegisterKeyPressAction(Keys.D1, (Vector2 thumbStickPosition) =>
            {
                CurrentWeapon?.Destroy();
                CurrentWeapon = new Handgun(Scene, this);

            }, true);

            UserInput.RegisterKeyPressAction(Keys.D2, (Vector2 thumbStickPosition) =>
            {
                CurrentWeapon?.Destroy();
                CurrentWeapon = new Machinegun(Scene, this);

            }, true);

            UserInput.RegisterKeyPressAction(Keys.D3, (Vector2 thumbStickPosition) =>
            {
                CurrentWeapon?.Destroy();
                CurrentWeapon = new Shotgun(Scene, this);

            }, true);

            UserInput.MouseMovedAction = (prevMousePos, currentMousePos) => {
                Vector2 worldPos = Scene.Camera.ScreenToWorldSpace(currentMousePos);
                if (Transform.Position.X < worldPos.X)
                {
                    CurrentFaceDirection = Direction.EAST;
                }
                else
                {
                    CurrentFaceDirection = Direction.WEST;
                }
                CurrentWeapon?.SetDirection(CurrentFaceDirection);
                CurrentWeapon?.FollowMouse(worldPos);
            };

            UserInput.RegisterKeyPressAction(Keys.Escape, (Vector2 thumbstickPosition) =>
            {
                Config.ExitAction.Invoke();
            });
        }

        public override void OnCollisionStart(IGameObject otherCollider)
        {
            if (otherCollider is Fuel)
            {
                AddFuel((otherCollider as Fuel).Amount);
                otherCollider.Destroy();
            }
            else if (otherCollider is Door && IsCarryingItem("Key"))
            {
                (otherCollider as Door).Open();
                RemoveItemWithTag("Key");
            }
            else if (otherCollider is DoorKey)
            {
                DoorKey key = (otherCollider as DoorKey);
                CarriedItems.Add(key);
                key.Visible = false;
                key.Active = false;
            }

            base.OnCollisionStart(otherCollider);
        }

        private bool IsCarryingItem(string tag)
        {
            foreach (IGameObject carriedItem in CarriedItems)
            {
                if (carriedItem.HasTag(tag)) return true;
            }
            return false;
        }

        private void RemoveItemWithTag(string tag)
        {
            foreach (IGameObject carriedItem in CarriedItems.ToList())
            {
                if (carriedItem.HasTag(tag)) CarriedItems.Remove(carriedItem);
            }
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

        public void WeaponKickback(Vector2 target, float force)
        {
            if (IsOnGround)
            {
                return;
            }

            Vector2 movement = Transform.Position - target;
            if (movement != Vector2.Zero)
            {
                movement.Normalize();
            }

            Velocity = movement * force;
        }
    }
}
