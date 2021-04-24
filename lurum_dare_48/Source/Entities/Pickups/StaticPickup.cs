using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Pickups
{
    class StaticPickup : Entity
    {
        public StaticPickup(AbstractScene scene, Vector2 position) : base (scene.LayerManager.EntityLayer, null, position)
        {
            AddTag("Pickup");
        }
    }
}
