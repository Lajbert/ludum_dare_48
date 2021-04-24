using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Weapons
{
    interface IWeapon
    {
        public void TriggerPulled(Vector2 worldPosition);

        public void TriggerReleased(Vector2 worldPosition);

        public void Reload();

        public void AddAmmo(int amount);

        public bool IsEmpty();

        public void SetDirection(Direction direction);

        public void Destroy();
    }
}
