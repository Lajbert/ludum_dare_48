using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Physics.Collision;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Enemies
{
    class EnemyTest : AbstractEnemy
    {

        private Vector2 offset = new Vector2(-Config.GRID, -Config.GRID);

        public EnemyTest(AbstractScene scene, Vector2 position, Direction direction) : base(scene, position, direction)
        {
            AddComponent(new Sprite(this, AssetUtil.CreateRectangle(Config.GRID * 2, Color.Brown), new Rectangle(0, 0, Config.GRID * 2, Config.GRID * 2), offset));
            AddComponent(new BoxCollisionComponent(this, Config.GRID * 2, Config.GRID * 2, offset));

            DEBUG_SHOW_COLLIDER = true;
        }
    }
}
