using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonolithEngine;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Entities;
using MonolithEngine.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities
{
    class Lava : PhysicalEntity
    {
        public Lava(AbstractScene scene, int width, int height, Vector2 position) : base(scene.LayerManager.EntityLayer, null, position)
        {
            TileGroup tg = new TileGroup();
            Texture2D tileSet = AssetUtil.CreateRectangle(Config.GRID, Color.Red);
            Color[] data = new Color[Config.GRID * Config.GRID];
            tileSet.GetData(0, new Rectangle(0, 0, Config.GRID, Config.GRID), data, 0, data.Length);
            for (int i = 0; i < width; i += Config.GRID)
            {
                for (int j = 0; j < height; j += Config.GRID)
                {
                    tg.AddColorData(data, new Vector2(i, j));
                }
            }
            AddComponent(new Sprite(this, tg.GetTexture(), new Rectangle(0, 0, width, height)));

            HorizontalFriction = 0;
            VerticalFriction = 0;

            VelocityY = 0.005f;

            HasGravity = false;
            CheckGridCollisions = false;

            Visible = true;
            Active = true;
        }
    }
}
