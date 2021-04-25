using lurum_dare_48.Source.Entities;
using lurum_dare_48.Source.Entities.Enemies;
using lurum_dare_48.Source.Entities.Environment;
using lurum_dare_48.Source.Entities.Pickups;
using lurum_dare_48.Source.Entities.Traps;
using lurum_dare_48.Source.Entities.Triggers;
using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Level;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Source.Level;
using MonolithEngine.Util;
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
                else if (entity.Identifier.Equals("Lava"))
                {
                    new Lava(scene, (int)entity.Width, (int)entity.Height, position);
                }
                else if (entity.Identifier.Equals("Spikes"))
                {
                    Direction dir = default;
                    foreach (FieldInstance field in entity.FieldInstances)
                    {
                        if (field.Identifier == "Direction")
                        {
                            dir = Enum.Parse(typeof(Direction), field.Value);
                        }
                    }
                    float size = entity.Width > entity.Height ? entity.Width : entity.Height;
                    new Spikes(scene, position, (int)size, dir);
                }
                else if (entity.Identifier.Equals("Fuel"))
                {
                    float amount = 0;
                    foreach (FieldInstance field in entity.FieldInstances)
                    {
                        if (field.Identifier == "Amount")
                        {
                            amount = (float) field.Value;
                        }
                    }
                    new FuelCan(scene, position, amount);
                }
                else if (entity.Identifier.Equals("EnemyPatrolTrigger"))
                {
                    new EnemyPatrolTrigger(scene, (int)entity.Width, (int)entity.Height, position);
                }
                else if (entity.Identifier.Equals("Enemy"))
                {
                    Direction dir = Direction.WEST;
                    bool patrol = true;
                    bool tutorial = false;
                    Newtonsoft.Json.Linq.JArray array = null;
                    foreach (FieldInstance field in entity.FieldInstances)
                    {
                        if (field.Identifier == "Direction")
                        {
                            dir = Enum.Parse(typeof(Direction), field.Value);
                        }
                        else if (field.Identifier == "Patrol")
                        {
                            patrol = field.Value;
                        }
                        else if (field.Identifier == "Tutorial")
                        {
                            tutorial = field.Value;
                        }
                        else if (field.Identifier == "CarriedItems")
                        {
                            array = field.Value;
                        }
                    }
                    List<string> carriedItems = array.ToObject<List<string>>();
                    EnemyTest enemy = new EnemyTest(scene, position, dir);
                    enemy.Patrol = patrol;
                    enemy.Tutorial = tutorial;
                    if (carriedItems != null && carriedItems.Count > 0)
                    {
                        enemy.CarriedItems = carriedItems;
                    }
                }
                else if (entity.Identifier.Equals("Door"))
                {
                    bool locked = false;
                    foreach (FieldInstance field in entity.FieldInstances)
                    {
                        if (field.Identifier == "Locked")
                        {
                            locked = field.Value;
                        }
                    }
                    new Door(scene, position, (int)entity.Height, locked);
                }
                else if (entity.Identifier.Equals("MountedGun"))
                {
                    Direction dir = default;
                    Newtonsoft.Json.Linq.JArray array = null;
                    foreach (FieldInstance field in entity.FieldInstances)
                    {
                        if (field.Identifier == "Direction")
                        {
                            dir = Enum.Parse(typeof(Direction), field.Value);
                        }
                        else if(field.Identifier == "Integer")
                        {
                            array = field.Value;
                        }
                    }
                    List<int> param = array.ToObject<List<int>>();
                    new MountedGun(scene, position, dir, param[0], param[1], param[2], param[3]);
                }
                else if (entity.Identifier.Equals("Saw"))
                {
                    bool horizontal = false;
                    foreach (FieldInstance field in entity.FieldInstances)
                    {
                        if (field.Identifier == "Horizontal")
                        {
                            horizontal = field.Value;
                        }
                    }
                    new Saw(scene, position, horizontal);
                }
                else if (entity.Identifier.Equals("TrapTrigger"))
                {
                    new TrapTrigger(scene, position, (int)entity.Width, (int)entity.Height);
                }
            }
        }

        public Hero GetHero()
        {
            return hero;
        }
    }
}
