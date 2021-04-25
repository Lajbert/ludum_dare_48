using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Entities.Animations;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Entities;
using MonolithEngine.Global;
using MonolithEngine.Source.Util;
using MonolithEngine.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Weapons
{
    abstract class AbstractWeapon : Entity, IWeapon
    {

        public int ClipSize;
        public int AmmoInClip;
        public int AllAmmo;
        public int MaxAmmo;

        public Hero hero;

        public Vector2 Offset = Vector2.Zero;

        //public Sprite Texture;

        private Vector2 breathOffset;

        protected AnimationStateMachine Animations;

        protected Vector2 LeftFacingOffset = Vector2.Zero;
        protected Vector2 RightFacingOffset = Vector2.Zero;

        public AbstractWeapon(AbstractScene scene, Hero owner) : base(scene.LayerManager.EntityLayer, owner, Vector2.Zero)
        {
            hero = owner;
            CurrentFaceDirection = hero.CurrentFaceDirection;
            Animations = new AnimationStateMachine();
            AddComponent(Animations);

            if (hero.CurrentFaceDirection == Direction.EAST)
            {
                Transform.Position = RightFacingOffset;
            }
            else
            {
                Transform.Position = LeftFacingOffset;
            }
        }

        public void AddAmmo(int amount)
        {
            AllAmmo += amount;
        }

        public bool IsEmpty()
        {
            return AmmoInClip == 0;
        }

        public void Reload()
        {
            int bulletCount = ClipSize - AmmoInClip;
            if (bulletCount > AllAmmo)
            {
                bulletCount = AllAmmo;
            }
            AllAmmo -= bulletCount;
            AmmoInClip += bulletCount;
        }

        public void SetAnimationBreathingOffset(Vector2 breathOffset)
        {
            this.breathOffset = breathOffset;
            SetOffset();
        }

        public void SetDirection(Direction direction)
        {
            if (CurrentFaceDirection != direction)
            {
                SetOffset();
                CurrentFaceDirection = direction;
                //Texture.DrawOffset = Offset;
            }
        }

        public abstract void TriggerPulled(Vector2 worldPosition);

        public abstract void TriggerReleased(Vector2 worldPosition);

        void IWeapon.FollowMouse(Vector2 mouseWorldPosition)
        {
            if (!hero.IsKicking)
            {
                AnimationRotation = (float)Math.Atan2((double)mouseWorldPosition.Y - Transform.Y, (double)mouseWorldPosition.X - Transform.X);
            }
        }

        public void SetLeftFacingOffset(Vector2 offset)
        {
            LeftFacingOffset = offset;
            SetOffset();
        }

        public void SetRightFacingOffset(Vector2 offset)
        {
            RightFacingOffset = offset;
            SetOffset();
        }

        private void SetOffset()
        {
            if (hero.CurrentFaceDirection == Direction.EAST)
            {
                Transform.Position = RightFacingOffset + breathOffset;
            }
            else
            {
                Transform.Position = LeftFacingOffset + breathOffset;
            }
        }

        public int GetAmmo()
        {
            return AllAmmo;
        }
    }
}
