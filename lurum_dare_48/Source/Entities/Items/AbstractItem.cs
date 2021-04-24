using Microsoft.Xna.Framework;
using MonolithEngine;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Items
{
    class AbstractItem : PhysicalEntity
    {

        public Vector2 Offset = Vector2.Zero;

        public Sprite Texture;

        public Hero hero;

        public AbstractItem(AbstractScene scene, Vector2 position) : base(scene.LayerManager.EntityLayer, null, position)
        {
            AddCollisionAgainst("Item");
            CurrentFaceDirection = Direction.EAST;
        }

        public void SetDirection(Direction direction)
        {
            if (CurrentFaceDirection != direction)
            {
                if (direction == Direction.WEST)
                {
                    Offset.X -= Config.GRID * 2;
                }
                else
                {
                    Offset.X += Config.GRID * 2;
                }
                CurrentFaceDirection = direction;
                Texture.DrawOffset = Offset;
            }
        }
    }
}
