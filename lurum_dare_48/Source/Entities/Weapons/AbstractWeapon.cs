using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Entities;
using MonolithEngine.Global;
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

        public Sprite Texture;

        public AbstractWeapon(AbstractScene scene, Hero owner) : base(scene.LayerManager.EntityLayer, owner, Vector2.Zero)
        {
            hero = owner;
            CurrentFaceDirection = hero.CurrentFaceDirection;
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

        public void SetDirection(Direction direction)
        {
            if (CurrentFaceDirection != direction)
            {
                if (direction == Direction.WEST)
                {
                    Offset.X -= Config.GRID * 2;
                }
                else
                {
                    Offset.X += Config.GRID * 2;
                }
                CurrentFaceDirection = direction;
                Texture.DrawOffset = Offset;
            }
        }

        public abstract void TriggerPulled(Vector2 worldPosition);

        public abstract void TriggerReleased(Vector2 worldPosition);
    }
}
