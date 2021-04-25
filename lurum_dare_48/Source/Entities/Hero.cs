﻿using lurum_dare_48.Source.Entities.Enemies;
using lurum_dare_48.Source.Entities.Environment;
using lurum_dare_48.Source.Entities.Items;
using lurum_dare_48.Source.Entities.Pickups;
using lurum_dare_48.Source.Entities.Weapons;
using lurum_dare_48.Source.Entities.Weapons.Guns;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonolithEngine;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Entities.Abstract;
using MonolithEngine.Engine.Source.Entities.Animations;
using MonolithEngine.Engine.Source.Entities.Controller;
using MonolithEngine.Engine.Source.Global;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Physics.Collision;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Global;
using MonolithEngine.Source.Entities;
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

        private Vector2 offset = new Vector2(0, -16);

        public IWeapon CurrentWeapon;

        public List<IGameObject> CarriedItems = new List<IGameObject>();

        public bool IsKicking = false;

        public Hero(AbstractScene scene, Vector2 position) : base(scene.LayerManager.EntityLayer, null, position)
        {

            //DEBUG_SHOW_PIVOT = true;

            CanFireTriggers = true;

            DrawPriority = 10;

            AddCollisionAgainst("Pickup");
            AddCollisionAgainst("Door", false);
            AddCollisionAgainst("Key");
            AddCollisionAgainst("MountedGunBullet");

            SetupController();

            CollisionOffsetBottom = 1;
            CollisionOffsetLeft = 0.5f;
            CollisionOffsetRight = 0.5f;
            CollisionOffsetTop = 1;

            CurrentFaceDirection = Direction.EAST;

            AnimationStateMachine animations = new AnimationStateMachine();
            animations.Offset = offset;
            AddComponent(animations);

            SpriteSheetAnimation idleLeft = new SpriteSheetAnimation(this, Assets.GetTexture("HeroIdle"), 24);
            /*idleLeft.AnimationSwitchCallback = () => { if (CurrentWeapon != null) CurrentWeapon.SetAnimationBreathingOffset(Vector2.Zero); };
            idleLeft.EveryFrameAction = (frame) =>
                {
                    if (CurrentWeapon == null) return;
                    float unit = 0.5f;
                    float offsetY = 0;
                    if (frame == 2 || frame == 3 || frame == 4)
                    {
                        offsetY += unit;
                    }
                    else if (frame == 6 || frame == 7)
                    {
                        offsetY -= unit;
                    }
                    CurrentWeapon.SetAnimationBreathingOffset(new Vector2(0, offsetY));
                };*/
            idleLeft.StartedCallback = () =>
            {
                if (CurrentWeapon == null)
                {
                    return;
                }
                if (CurrentWeapon is Shotgun)
                {
                    (CurrentWeapon as Handgun).SetLeftFacingOffset(new Vector2(-3, -17));
                    (CurrentWeapon as Handgun).SetRightFacingOffset(new Vector2(3, -17));
                }
                else if (CurrentWeapon is Machinegun)
                {
                    (CurrentWeapon as Handgun).SetLeftFacingOffset(new Vector2(-3, -17));
                    (CurrentWeapon as Handgun).SetRightFacingOffset(new Vector2(3, -17));
                }
                else if (CurrentWeapon is Handgun)
                {
                    (CurrentWeapon as Handgun).SetLeftFacingOffset(new Vector2(-3, -17));
                    (CurrentWeapon as Handgun).SetRightFacingOffset(new Vector2(3, -17));
                }
            };
            animations.RegisterAnimation("IdleLeft", idleLeft, () => Velocity.X == 0 && CurrentFaceDirection == Direction.WEST);

            SpriteSheetAnimation idleRight = idleLeft.CopyFlipped();
            animations.RegisterAnimation("IdleRight", idleRight, () => Velocity.X == 0 && CurrentFaceDirection == Direction.EAST);

            SpriteSheetAnimation runLeft = new SpriteSheetAnimation(this, Assets.GetTexture("HeroRun"), 40);
            runLeft.StartedCallback = () =>
            {
                if (CurrentWeapon == null)
                {
                    return;
                }
                if (CurrentWeapon is Shotgun)
                {
                    (CurrentWeapon as Handgun).SetLeftFacingOffset(new Vector2(-3, -17));
                    (CurrentWeapon as Handgun).SetRightFacingOffset(new Vector2(3, -17));
                }
                else if (CurrentWeapon is Machinegun)
                {
                    (CurrentWeapon as Handgun).SetLeftFacingOffset(new Vector2(-3, -17));
                    (CurrentWeapon as Handgun).SetRightFacingOffset(new Vector2(3, -17));
                }
                else if (CurrentWeapon is Handgun)
                {
                    (CurrentWeapon as Handgun).SetLeftFacingOffset(new Vector2(1, -17));
                    (CurrentWeapon as Handgun).SetRightFacingOffset(new Vector2(-1, -17));
                }
            };
            animations.RegisterAnimation("RunLeft", runLeft, () => Velocity.X < 0 && CurrentFaceDirection == Direction.WEST);

            SpriteSheetAnimation runRight = runLeft.CopyFlipped();
            animations.RegisterAnimation("RunRight", runRight, () => Velocity.X > 0 && CurrentFaceDirection == Direction.EAST);

            SpriteSheetAnimation runBackwardsLeft = new SpriteSheetAnimation(this, Assets.GetTexture("HeroRunBackwards"), 40);
            runBackwardsLeft.StartedCallback = () =>
            {
                if (CurrentWeapon == null)
                {
                    return;
                }
                if (CurrentWeapon is Shotgun)
                {
                    (CurrentWeapon as Handgun).SetLeftFacingOffset(new Vector2(-3, -17));
                    (CurrentWeapon as Handgun).SetRightFacingOffset(new Vector2(3, -17));
                }
                else if (CurrentWeapon is Machinegun)
                {
                    (CurrentWeapon as Handgun).SetLeftFacingOffset(new Vector2(-3, -17));
                    (CurrentWeapon as Handgun).SetRightFacingOffset(new Vector2(3, -17));
                }
                else if (CurrentWeapon is Handgun)
                {
                    (CurrentWeapon as Handgun).SetLeftFacingOffset(new Vector2(1, -17));
                    (CurrentWeapon as Handgun).SetRightFacingOffset(new Vector2(-1, -17));
                }
            };
            animations.RegisterAnimation("RunBackwardsLeft", runBackwardsLeft, () => Velocity.X > 0 && CurrentFaceDirection == Direction.WEST);

            SpriteSheetAnimation runRighrunBackwardsRight = runBackwardsLeft.CopyFlipped();
            animations.RegisterAnimation("RunBackwardsRight", runRighrunBackwardsRight, () => Velocity.X < 0 && CurrentFaceDirection == Direction.EAST);

            SpriteSheetAnimation jumpLeft = new SpriteSheetAnimation(this, Assets.GetTexture("HeroRun"), 1);
            jumpLeft.StartFrame = 6;
            jumpLeft.EndFrame = 7;
            animations.RegisterAnimation("JumpLeft", jumpLeft, () => !IsOnGround && CurrentFaceDirection == Direction.WEST, 1);

            SpriteSheetAnimation jumpRight = jumpLeft.CopyFlipped();
            animations.RegisterAnimation("JumpRight", jumpRight, () => !IsOnGround && CurrentFaceDirection == Direction.EAST, 1);

            SpriteSheetAnimation jetpackLeft = new SpriteSheetAnimation(this, Assets.GetTexture("Jetpacking"), 40);
            animations.RegisterAnimation("JetpackLeft", jetpackLeft, () => flying && CurrentFaceDirection == Direction.WEST, 2);

            SpriteSheetAnimation jetpackRight = jetpackLeft.CopyFlipped();
            animations.RegisterAnimation("JetpackRight", jetpackRight, () => flying && CurrentFaceDirection == Direction.EAST, 2);

            SpriteSheetAnimation kickLeft = new SpriteSheetAnimation(this, Assets.GetTexture("Kick"), 40);
            kickLeft.Looping = false;
            kickLeft.StartedCallback = () =>
            {
                IsKicking = true;
                if (IsOnGround)
                {
                    MovementSpeed = 0;
                }

                if (CurrentWeapon == null)
                {
                    return;
                }
                if (CurrentWeapon is Shotgun)
                {
                    (CurrentWeapon as Handgun).SetLeftFacingOffset(new Vector2(-3, -17));
                    (CurrentWeapon as Handgun).SetRightFacingOffset(new Vector2(3, -17));
                }
                else if (CurrentWeapon is Machinegun)
                {
                    (CurrentWeapon as Handgun).SetLeftFacingOffset(new Vector2(-3, -17));
                    (CurrentWeapon as Handgun).SetRightFacingOffset(new Vector2(3, -17));
                }
                else if (CurrentWeapon is Handgun)
                {
                    (CurrentWeapon as Handgun).SetLeftFacingOffset(new Vector2(1, -17));
                    (CurrentWeapon as Handgun).SetRightFacingOffset(new Vector2(-1, -17));
                }
            };

            kickLeft.StoppedCallback = () =>
            {
                IsKicking = false;
                MovementSpeed = Config.CHARACTER_SPEED;
            };
            animations.RegisterAnimation("KickLeft", kickLeft, () => false);

            SpriteSheetAnimation kickRight = kickLeft.CopyFlipped();
            animations.RegisterAnimation("KickRight", kickRight, () => false);

            SpriteSheetAnimation kickJetpackLeft = new SpriteSheetAnimation(this, Assets.GetTexture("KickJetpacking"), 40);
            kickJetpackLeft.Looping = false;
            kickJetpackLeft.StartedCallback = () =>
            {
                IsKicking = true;

                if (CurrentWeapon == null)
                {
                    return;
                }
                if (CurrentWeapon is Shotgun)
                {
                    (CurrentWeapon as Handgun).SetLeftFacingOffset(new Vector2(-3, -17));
                    (CurrentWeapon as Handgun).SetRightFacingOffset(new Vector2(3, -17));
                }
                else if (CurrentWeapon is Machinegun)
                {
                    (CurrentWeapon as Handgun).SetLeftFacingOffset(new Vector2(-3, -17));
                    (CurrentWeapon as Handgun).SetRightFacingOffset(new Vector2(3, -17));
                }
                else if (CurrentWeapon is Handgun)
                {
                    (CurrentWeapon as Handgun).SetLeftFacingOffset(new Vector2(1, -17));
                    (CurrentWeapon as Handgun).SetRightFacingOffset(new Vector2(-1, -17));
                }
            };
            kickJetpackLeft.StoppedCallback = () =>
            {
                IsKicking = false;
            };
            animations.RegisterAnimation("KickJetpackLeft", kickJetpackLeft, () => false);

            SpriteSheetAnimation kickJetpackRight = kickJetpackLeft.CopyFlipped();
            animations.RegisterAnimation("KickJetpackRight", kickJetpackRight, () => false);

            animations.AddFrameTransition("RunLeft", "RunBackwardsLeft");
            animations.AddFrameTransition("RunRight", "RunBackwardsRight");
            animations.AddFrameTransition("RunLeft", "RunRight");
            animations.AddFrameTransition("RunBackwardsLeft", "RunBackwardsRight");
            animations.AddFrameTransition("JetpackLeft", "JetpackRight");

            Visible = true;
            Active = true;

            DebugFunction = () =>
            {
                return "Fuel: " + (int)(((float)(fuel / TankCapacity)) * 100) + " %";
            };

            AddComponent(new BoxCollisionComponent(this, 18, 30, new Vector2(-8, -30)));
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
                if (CurrentWeapon != null && !CurrentWeapon.IsEmpty() && !IsKicking)
                {
                    CurrentWeapon.TriggerPulled(Scene.Camera.ScreenToWorldSpace(mousePosition));
                }
            };

            UserInput.LeftClickUpAction = (mousePosition) =>
            {
                if (CurrentWeapon != null && !CurrentWeapon.IsEmpty() && !IsKicking)
                {
                    CurrentWeapon.TriggerReleased(Scene.Camera.ScreenToWorldSpace(mousePosition));
                }
            };


            UserInput.RightClickDownAction = (mousePosition) =>
            {
                if (CurrentFaceDirection == Direction.WEST)
                {
                    if (flying)
                    {
                        GetComponent<AnimationStateMachine>().PlayAnimation("KickJetpackLeft");
                    }
                    else
                    {
                        GetComponent<AnimationStateMachine>().PlayAnimation("KickLeft");
                    }
                }
                else
                {
                    if (flying)
                    {
                        GetComponent<AnimationStateMachine>().PlayAnimation("KickJetpackRight");
                    }
                    else
                    {
                        GetComponent<AnimationStateMachine>().PlayAnimation("KickRight");
                    }
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
                    if (!IsKicking)
                    {
                        CurrentFaceDirection = Direction.EAST;
                    }
                }
                else
                {
                    if (!IsKicking)
                    {
                        CurrentFaceDirection = Direction.WEST;
                    }
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
            if (otherCollider is FuelCan)
            {
                AddFuel((otherCollider as FuelCan).Amount);
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
            else if (otherCollider is MountedGunBullet)
            {
                otherCollider.Destroy();
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

            if (IsKicking && MovementSpeed > 0)
            {
                MovementSpeed = 0;
            }
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
