﻿using ForestPlatformerExample.Source.Entities.Enemies;
using lurum_dare_48.Source.Entities.Items;
using lurum_dare_48.Source.Entities.Traps;
using lurum_dare_48.Source.Entities.Weapons.Guns;
using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Entities.Abstract;
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

        public List<string> CarriedItems = new List<string>();

        public EnemyTest(AbstractScene scene, Vector2 position, Direction direction) : base(scene, position, direction)
        {

            AddCollisionAgainst("Spikes");
            CurrentFaceDirection = Direction.EAST;
            AddComponent(new Sprite(this, Assets.CreateRectangle(Config.GRID * 2, Color.Brown), new Rectangle(0, 0, Config.GRID * 2, Config.GRID * 2), offset));
            AddComponent(new BoxCollisionComponent(this, Config.GRID * 2, Config.GRID * 2, offset));

            DEBUG_SHOW_COLLIDER = true;
        }

        public override void Destroy()
        {
            foreach (string item in CarriedItems)
            {
                if (item == "Key")
                {
                    DoorKey key = new DoorKey(Scene, Transform.Position - new Vector2(0, 10));
                    key.VelocityY -= 2;
                }
            }
            base.Destroy();
        }

        public override void FixedUpdate()
        {
            if (Patrol)
            {
                AIUtil.Patrol(true, this);
            }
            base.FixedUpdate();
        }

        public override void OnCollisionStart(IGameObject otherCollider)
        {
            if (otherCollider is Spikes)
            {
                Destroy();
            }
            base.OnCollisionStart(otherCollider);
        }
    }
}
