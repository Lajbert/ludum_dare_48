using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Physics.Collision;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Pickups
{
    class HealthPickup : AbstractPickup
    {

        Vector2 offset = new Vector2(-16, -16);

        public HealthPickup(AbstractScene scene, Vector2 position) : base(scene, position)
        {
            DrawPriority = 7;

            AddComponent(new Sprite(this, Assets.GetTexture("UIHealth"), new Rectangle(0, 0, 32, 32), offset));

            AddComponent(new BoxCollisionComponent(this, 32, 32, offset));

#if DEBUG
            //(GetCollisionComponent() as AbstractCollisionComponent).DEBUG_DISPLAY_COLLISION = true;
            //DEBUG_SHOW_COLLIDER = true;
#endif
        }
    }
}
