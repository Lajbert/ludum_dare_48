using lurum_dare_48.Source.Entities;
using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Level;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Source.Level;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Levels
{
    class EntityParser
    {
        private LDTKMap world;

        private Hero hero;

        public EntityParser(LDTKMap world)
        {
            this.world = world;
        }

        public void LoadEntities(AbstractScene scene, string levelID)
        {
            foreach (EntityInstance entity in world.ParseLevel(scene, levelID))
            {
                Vector2 position = new Vector2(entity.Px[0], entity.Px[1]);
                if (entity.Identifier.Equals("Hero"))
                {
                    hero = new Hero(scene, position);
                }
            }
        }

        public Hero GetHero()
        {
            return hero;
        }
    }
}
