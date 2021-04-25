using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonolithEngine;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Entities.Abstract;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Physics.Collision;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Entities;
using MonolithEngine.Global;
using MonolithEngine.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities
{
    class Lava : PhysicalEntity
    {
        

        public Lava(AbstractScene scene, int width, int height, Vector2 position) : base(scene.LayerManager.LavaLayer, null, position)
        {
            AddCollisionAgainst("Hero");
            int spriteSize = 32;
            TileGroup tg = new TileGroup(32);
            Texture2D tileSet = Assets.GetTexture("Lava");
            Color[] data = new Color[spriteSize * spriteSize];
            tileSet.GetData(0, new Rectangle(0, 0, spriteSize, spriteSize), data, 0, data.Length);
            for (int i = 0; i < width; i += spriteSize)
            {
                for (int j = 0; j < height; j += spriteSize)
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

            AddComponent(new BoxCollisionComponent(this, width, height));
            DEBUG_SHOW_COLLIDER = true;
        }

        public override void Update()
        {
            base.Update();
            Scene.Camera.CameraLimitY = DrawPosition.Y;
        }

        public override void OnCollisionStart(IGameObject otherCollider)
        {
            if (otherCollider is Hero)
            {
                Logger.Info("The game should end now");
            }
            base.OnCollisionStart(otherCollider);
        }
    }
}
