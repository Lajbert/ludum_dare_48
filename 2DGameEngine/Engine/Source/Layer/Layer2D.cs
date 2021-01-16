﻿using GameEngine2D.Entities;
using GameEngine2D.Entities.Interfaces;
using GameEngine2D.Global;
using GameEngine2D.Source.Camera2D;
using GameEngine2D.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine2D.Source.Layer
{
    public class Layer2D
    {
        private List<Entity> rootObjects = new List<Entity>();
        private List<Entity> newObjects = new List<Entity>();
        private List<Entity> removedObjects = new List<Entity>();
        private float scrollSpeedModifier;
        private bool lockY;
        private bool ySorting = false;
        private SpriteBatch spriteBatch;

        public static GraphicsDeviceManager GraphicsDeviceManager;

        public int Priority = 0;

        public float Depth { get; set; } = 0;

        private Camera camera;

        internal Layer2D(Camera camera, int priority = 0, bool ySorting = false, float scrollSpeedModifier = 1f, bool lockY = true)
        {
            if (camera == null)
            {
                throw new Exception("Camera not provided for layer!");
            }
            this.scrollSpeedModifier = scrollSpeedModifier;
            this.camera = camera;
            Priority = priority;
            this.lockY = lockY;
            this.ySorting = ySorting;
            spriteBatch = new SpriteBatch(GraphicsDeviceManager.GraphicsDevice);
        }

        public void AddRootObject(Entity gameObject)
        {
            newObjects.Add(gameObject);
        }

        private void RemoveObject(Entity gameObject)
        {
            removedObjects.Add(gameObject);
        }

        public IEnumerable<Entity> GetAll()
        {
            return rootObjects;
        }

        public void RemoveRoot(Entity gameObject)
        {
            RemoveObject(gameObject);
        }

        public void DrawAll(GameTime gameTime)
        {
            if (ySorting)
            {
                rootObjects.Sort((a, b) => a.Position.Y.CompareTo(b.Position.Y));
            }
            
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.GetTransformMatrix(scrollSpeedModifier, lockY));

            foreach (Entity entity in rootObjects)
            {
                entity.PreDraw(spriteBatch, gameTime);
                entity.Draw(spriteBatch, gameTime);
                entity.PostDraw(spriteBatch, gameTime);
            }
            spriteBatch.End();
            if (newObjects.Count > 0)
            {
                rootObjects.AddRange(newObjects);
                newObjects.Clear();
            }
            if (removedObjects.Count > 0)
            {
                foreach (Entity toRemove in removedObjects)
                {
                    rootObjects.Remove(toRemove);
                }
                removedObjects.Clear();
            }
        }

        public void UpdateAll(GameTime gameTime)
        {
            foreach (Entity entity in rootObjects)
            {
                entity.PreUpdate(gameTime);
                entity.Update(gameTime);
                entity.PostUpdate(gameTime);
            }
            if (newObjects.Count > 0)
            {
                rootObjects.AddRange(newObjects);
                newObjects.Clear();
            }
            if (removedObjects.Count > 0)
            {
                foreach (Entity toRemove in removedObjects)
                {
                    rootObjects.Remove(toRemove);
                }
                removedObjects.Clear();
            }
        }

        public void Destroy()
        {
            foreach (Entity entity in rootObjects)
            {
                entity.Destroy();
            }
        }

    }
}