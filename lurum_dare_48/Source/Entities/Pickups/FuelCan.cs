using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Physics.Collision;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Entities;
using MonolithEngine.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Pickups
{
    class FuelCan : StaticPickup
    {
        public float Amount;

        public FuelCan(AbstractScene scene, Vector2 position, float amount) : base(scene, position)
        {
            DrawPriority = 8;

            AddComponent(new Sprite(this, Assets.GetTexture("FuelCan")));

            AddComponent(new BoxCollisionComponent(this, Config.GRID, Config.GRID));

#if DEBUG
            //(GetCollisionComponent() as AbstractCollisionComponent).DEBUG_DISPLAY_COLLISION = true;
            //DEBUG_SHOW_PIVOT = true;
#endif
        }
    }
}
