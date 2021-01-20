﻿using GameEngine2D.Entities;
using GameEngine2D.Entities.Interfaces;
using GameEngine2D.Global;
using GameEngine2D.Source.Camera2D;
using GameEngine2D.Source.Util;
using GameEngine2D.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine2D.Source.Layer
{
    public class Layer
    {
        private List<Entity> activeObjects = new List<Entity>();
        private List<Entity> visibleObjects = new List<Entity>();

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

        internal Layer(Camera camera, int priority = 0, bool ySorting = false, float scrollSpeedModifier = 1f, bool lockY = true)
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
            return activeObjects;
        }

        public void RemoveRoot(Entity gameObject)
        {
            RemoveObject(gameObject);
        }

        public void DrawAll(GameTime gameTime)
        {

            if (ySorting)
            {
                activeObjects.Sort((a, b) => a.Position.Y.CompareTo(b.Position.Y));
            }
            
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.GetTransformMatrix(scrollSpeedModifier, lockY));

            foreach (Entity entity in visibleObjects)
            {
                entity.PreDraw(spriteBatch, gameTime);
                entity.Draw(spriteBatch, gameTime);
                entity.PostDraw(spriteBatch, gameTime);
            }
            spriteBatch.End();
            if (newObjects.Count > 0)
            {
                foreach (Entity e in newObjects)
                {
                    if (e.Visible)
                    {
                        visibleObjects.Add(e);
                    }
                    if (e.Active)
                    {
                        activeObjects.Add(e);
                    }
                }
                newObjects.Clear();
            }
            if (removedObjects.Count > 0)
            {
                foreach (Entity toRemove in removedObjects)
                {
                    if (toRemove.Active)
                    {
                        activeObjects.Remove(toRemove);
                    }

                    if (toRemove.Visible)
                    {
                        visibleObjects.Remove(toRemove);
                    }
                }
                removedObjects.Clear();
            }
        }

        public void UpdateAll(GameTime gameTime)
        {
            // in case of skipped frame, we should just recalculate everything
            if (TimeUtil.GetElapsedTime(gameTime) > 1)
            {
                return;
            }

            foreach (Entity entity in activeObjects)
            {
                entity.PreUpdate(gameTime);
                entity.Update(gameTime);
                entity.PostUpdate(gameTime);
            }
            if (newObjects.Count > 0)
            {
                foreach (Entity e in newObjects)
                {
                    if (e.Visible)
                    {
                        visibleObjects.Add(e);
                    }
                    if (e.Active)
                    {
                        activeObjects.Add(e);
                    }
                }
                newObjects.Clear();
            }
            if (removedObjects.Count > 0)
            {
                foreach (Entity toRemove in removedObjects)
                {
                    if (toRemove.Active)
                    {
                        activeObjects.Remove(toRemove);
                    }

                    if (toRemove.Visible)
                    {
                        visibleObjects.Remove(toRemove);
                    }
                }
                removedObjects.Clear();
            }
        }

        public void Destroy()
        {
            foreach (Entity entity in activeObjects)
            {
                entity.Destroy();
            }
        }

    }
}