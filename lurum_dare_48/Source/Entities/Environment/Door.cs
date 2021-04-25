using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonolithEngine;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Physics.Collision;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Environment
{
    class Door : PhysicalEntity
    {
        private bool isOpen = false;

        private Sprite closedSprite;
        private Sprite openSprite;

        public Door(AbstractScene scene, Vector2 position, int height, bool locked) : base (scene.LayerManager.EntityLayer, null, position)
        {
            AddTag("Door");
            CheckGridCollisions = false;
            HasGravity = false;
            /*TileGroup tg = new TileGroup();
            Texture2D tileSet = AssetUtil.CreateRectangle(Config.GRID, Color.Bisque);
            Color[] data = new Color[Config.GRID * Config.GRID];
            HasGravity = false;

            for (int i = 0; i < height; i += Config.GRID)
            {
                if (i == 0)
                {
                    tileSet.GetData(0, new Rectangle(0, 0, Config.GRID, Config.GRID), data, 0, data.Length);
                }
                else if (i == height - Config.GRID)
                {
                    data = new Color[Config.GRID * Config.GRID];
                    tileSet.GetData(0, new Rectangle(0, 0, Config.GRID, Config.GRID), data, 0, data.Length);
                }
                else
                {
                    data = new Color[Config.GRID * Config.GRID];
                    tileSet.GetData(0, new Rectangle(0, 0, Config.GRID, Config.GRID), data, 0, data.Length);
                }
                for (int j = 0; j < Config.GRID; j += Config.GRID)
                {
                    tg.AddColorData(data, new Vector2(i, j));
                }
            }
            Sprite sprite = new Sprite(this, tg.GetTexture(), new Rectangle(0, 0, Config.GRID, height));
            AddComponent(sprite);*/

            closedSprite = new Sprite(this, Assets.GetTexture("Door"), new Rectangle(0, 0, Config.GRID, 112));
            openSprite = new Sprite(this, Assets.GetTexture("Door"), new Rectangle(Config.GRID, 0, Config.GRID, 112));
            AddComponent(closedSprite);
            AddComponent(new BoxCollisionComponent(this, Config.GRID, height));

            //DEBUG_SHOW_COLLIDER = true;
        }

        public void Open()
        {
            if (isOpen)
            {
                return;
            }

            isOpen = true;

            RemoveComponent<Sprite>();
            AddComponent(openSprite);
            Timer.Repeat(1300, (elapsedTime) =>
            {
                VelocityY -= 0.001f * elapsedTime;
            });
        }
    }
}
