using lurum_dare_48.Source.Entities.Environment;
using Microsoft.Xna.Framework;
using MonolithEngine;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Entities.Abstract;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Physics.Collision;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Items
{
    class DoorKey : PhysicalEntity
    {
        public DoorKey(AbstractScene scene, Vector2 position) : base (scene.LayerManager.EntityLayer, null, position)
        {
            AddTag("Key");

            AddComponent(new Sprite(this, Assets.GetTexture("Keycard")));

            AddComponent(new BoxCollisionComponent(this, Config.GRID, Config.GRID));
            CollisionsEnabled = false;


            Timer.TriggerAfter(500, () =>
            {
                CollisionsEnabled = true;
            });

            //DEBUG_SHOW_COLLIDER = true;
        }
    }
}
