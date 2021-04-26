using lurum_dare_48.Source.Entities.Weapons.Guns;
using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Audio;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Physics.Collision;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Pickups
{
    class Ammo : AbstractPickup
    {
        public int Amount;

        public Type WeaponType;

        public Ammo(AbstractScene scene, Vector2 position, int amount, Type weaponType) : base(scene, position)
        {
            DrawPriority = 8;

            Amount = amount;

            WeaponType = weaponType;

            if (WeaponType.Equals(typeof(Machinegun)))
            {
                AddComponent(new Sprite(this, Assets.GetTexture("MachineGunAmmo")));
            }
            else if (WeaponType.Equals(typeof(Handgun)))
            {
                AddComponent(new Sprite(this, Assets.GetTexture("HandgunAmmo")));
            }
            else if (WeaponType.Equals(typeof(Shotgun)))
            {
                AddComponent(new Sprite(this, Assets.GetTexture("ShotgunAmmo")));
            }

            AddComponent(new BoxCollisionComponent(this, Config.GRID, Config.GRID));

#if DEBUG
            //(GetCollisionComponent() as AbstractCollisionComponent).DEBUG_DISPLAY_COLLISION = true;
            //DEBUG_SHOW_PIVOT = true;
#endif
        }

        public override void Destroy()
        {
            AudioEngine.Play("AmmoPickup");
            base.Destroy();
        }
    }
}
