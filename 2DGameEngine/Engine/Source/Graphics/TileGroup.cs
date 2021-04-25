using MonolithEngine.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonolithEngine.Engine.Source.Graphics
{
    public class TileGroup
    {

        private Dictionary<Vector2, Color[]> tiles = new Dictionary<Vector2, Color[]>();

        private int width = 0;
        private int height = 0;

        public static GraphicsDevice GraphicsDevice;

        private Texture2D texture;

        private int tileSize;

        public TileGroup(int tileSize = 0)
        {
            if (tileSize == 0)
            {
                this.tileSize = Config.GRID;
            }
            else
            {
                this.tileSize = tileSize;
            }
        }

        public void AddTile(Texture2D texture, Vector2 position)
        {
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);
            tiles.Add(position, data);
            width = Math.Max(width, (int)position.X + tileSize);
            height = Math.Max(height, (int)position.Y + tileSize);
        }

        public void AddColorData(Color[] data, Vector2 position)
        {
            tiles.Add(position, data);
            width = Math.Max(width, (int)position.X + tileSize);
            height = Math.Max(height, (int)position.Y + tileSize);
        }

        public Texture2D GetTexture()
        {
            if (texture == null)
            {
                Build();
            }

            return texture;
        }

        private void Build()
        {
            if (width == 0 || height == 0)
            {
                throw new Exception("Incorrect tileset dimensions!");
            }

            texture = new Texture2D(GraphicsDevice, width, height);
            
            foreach(KeyValuePair<Vector2, Color[]> tile in tiles)
            {
                texture.SetData(0, new Rectangle((int)tile.Key.X, (int)tile.Key.Y, tileSize, tileSize), tile.Value, 0, tile.Value.Length);
            }
        }
    }
}
