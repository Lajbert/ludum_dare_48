﻿using GameEngine2D.Engine.Source.Entities;
using GameEngine2D.Engine.Source.Entities.Animations;
using GameEngine2D.Engine.Source.Entities.Interfaces;
using GameEngine2D.Engine.Source.Physics.Collision;
using GameEngine2D.Engine.Source.Physics.Interface;
using GameEngine2D.Engine.Source.Physics.Raycast;
using GameEngine2D.Engine.Source.Util;
using GameEngine2D.Global;
using GameEngine2D.Source.Entities.Animation;
using GameEngine2D.Source.Layer;
using GameEngine2D.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine2D.Entities
{
    public class Entity : GameObject, IRayBlocker, ICircleCollider
    {

        protected float CollisionOffsetLeft = 0f;
        protected float CollisionOffsetRight = 0f;
        protected float CollisionOffsetBottom = 0f;
        protected float CollisionOffsetTop = 0f;

        public int GridCollisionPriority = 0;

        private readonly string DESTROY_AMINATION = "Destroy";

        private HashSet<string> tags = new HashSet<string>();

        private static List<string> movementBlockerTags = new List<string>()
        {
            "Collider",
            "SlideWall",
            "MovingPlatform"
            //"Platform"
        };

        private static List<string> gridCoordUpdateTags = new List<string>()
        {
            "MovingPlatform",
            "MovingEnemy",
        };

        private bool updateGridPosition = false;
        protected static OnePointCollider CollisionChecker { get; } = new OnePointCollider();

        protected Vector2 StartPosition;

        public bool Visible = true;
        public bool Active = false;

        private Vector2 drawOffset = Vector2.Zero;

        protected Vector2 DrawOffset {
            get => drawOffset;
            set {
                drawOffset = value;
                SetDrawPosition();
            }
        }

        public Vector2 Pivot = Vector2.Zero;

        public Rectangle SourceRectangle;

        public bool BlocksMovement = false;

        private Vector2 position;

        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        public float X
        {
            get => position.X;
            set => position.X = value;
        }

        public float Y
        {
            get => position.Y;
            set => position.Y = value;
        }

        protected Texture2D Sprite { get; set; }
        private HashSet<Entity> children;

        private Entity parent;

        private bool hasCollisions = false;

        public bool ColliderOnGrid {
            get => hasCollisions;
            set {
                hasCollisions = value;
                if (value)
                {
                    CollisionChecker.AddOrUpdate(this);
                } else
                {
                    CollisionChecker.Remove(this);
                }
            }
        }

        protected Layer Layer { get; set; }
        protected List<(Vector2 start, Vector2 end)> RayBlockerLines;

        private bool blocksRay = false;

        public SoundEffect DestroySound;

        protected HashSet<Direction> GridCollisionCheckDirections = new HashSet<Direction>();

        public bool BlocksRay {
            get => blocksRay;
            set
            {
                if (!value)
                {
                    LayerManager.Instance.RayBlockersLayer.RemoveRoot(this);
                }
                else
                {
                    LayerManager.Instance.RayBlockersLayer.AddRootObject(this);
                }
                blocksRay = value;
            }
        }

        protected SpriteFont font;

        //grid coordinates
        public Vector2 GridCoordinates = Vector2.Zero;

        //between 0 and 1: where the object is inside the grid cell
        protected Vector2 InCellLocation;

        protected AnimationStateMachine Animations { get; set; }

        protected Ray2DEmitter RayEmitter { get; set; }

        public CircleCollider CircleCollider { get; set; }

        private Dictionary<(Entity, Direction), bool> circleCollisions = new Dictionary<(Entity, Direction), bool>();

        private Texture2D pivotMarker;

        private Dictionary<(Entity, Direction), bool> collidesWithOnGrid = new Dictionary<(Entity, Direction), bool>();

        public float Depth = 0f;

        public bool DEBUG_SHOW_PIVOT = false;

        protected Vector2 DrawPosition;

        public Entity(Layer layer, Entity parent, Vector2 startPosition, Texture2D sprite = null, SpriteFont font = null)
        {
            this.Layer = layer;
            children = new HashSet<Entity>();
            this.font = font;

            SetSprite(sprite);
            if (parent != null) {
                this.parent = parent;
                this.parent.AddChild(this);
                this.StartPosition = this.Position = startPosition;
                GridCoordinates = CalculateGridCoord(StartPosition + parent.GetPositionWithParent());
            } else
            {
                layer.AddRootObject(this);
                this.StartPosition = this.Position = startPosition;
                GridCoordinates = CalculateGridCoord(StartPosition);
            }

            SetDrawPosition();
        }

        protected void SetDrawPosition()
        {
            if (parent != null)
            {
                DrawPosition = StartPosition + parent.GetPositionWithParent() + drawOffset;
            }
            else
            {
                DrawPosition = Position + drawOffset;
            }
        }


        protected virtual void SetRayBlockers()
        {
            RayBlockerLines.Clear();
            RayBlockerLines.Add((Position, new Vector2(Position.X, Position.Y + Config.GRID))); //0, 1
            RayBlockerLines.Add((Position, new Vector2(Position.X + Config.GRID, Position.Y))); //1, 0
            RayBlockerLines.Add((new Vector2(Position.X + Config.GRID, Position.Y), new Vector2(Position.X + Config.GRID, Position.Y + Config.GRID))); //1
            RayBlockerLines.Add((new Vector2(Position.X, Position.Y + Config.GRID), new Vector2(Position.X + Config.GRID, Position.Y + Config.GRID)));
        }

        public virtual void PreDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Entity child in children)
            {
                child.PreDraw(spriteBatch, gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            if (Sprite != null)
            {
                spriteBatch.Draw(Sprite, DrawPosition, SourceRectangle, Color.White, 0f, Pivot, 1f, SpriteEffects.None, Depth);
            }
            else if (Animations != null)
            {
                Animations.Draw(spriteBatch, gameTime);
            }

            if (DEBUG_SHOW_PIVOT)
            {
                float radius= CircleCollider == null ? 5 : CircleCollider.Radius;
                if (pivotMarker == null)
                {
                    pivotMarker = SpriteUtil.CreateCircle((int)radius, Color.Black);
                }
                if (font != null)
                {
                    //spriteBatch.DrawString(font, "Veloctiy.Y : " + Velocity.Y, DrawPosition, Color.Black);
                }
                spriteBatch.Draw(pivotMarker, Position - new Vector2(radius / 2, radius / 2), Color.White);
            }
            
            if (children.Count > 0)
            {
                foreach (Entity child in children)
                {
                    child.Draw(spriteBatch, gameTime);
                }
            }
            
        }

        public virtual void PostDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            foreach (Entity child in children)
            {
                child.PostDraw(spriteBatch, gameTime);
            }
        }

        protected virtual void OnGridCollision(Entity otherCollider, Direction direction)
        {

        }

        protected virtual void OnGridCollisionStart(Entity otherCollider, Direction direction)
        {

        }

        protected virtual void OnGridCollisionEnd(Entity otherCollider, Direction direction)
        {

        }

        protected virtual void OnCircleCollision(Entity otherCollider, Direction direction, float intersection)
        {

        }

        protected virtual void OnCircleCollisionStart(Entity otherCollider, Direction direction, float intersection)
        {

        }

        protected virtual void OnCircleCollisionEnd(Entity otherCollider, Direction direction)
        {

        }

        public Entity GetSamePositionCollider()
        {
            return CollisionChecker.GetColliderAt(GridCoordinates);
        }

        public virtual void PreUpdate(GameTime gameTime)
        {

            UpdateCollisions();

            foreach (Entity child in children)
            {
                child.PreUpdate(gameTime);
            }
        }

        public virtual void Update(GameTime gameTime)
        {

            if (parent != null)
            {
                DrawPosition = StartPosition + parent.GetPositionWithParent() + DrawOffset;

            }
            else
            {
                DrawPosition = Position + DrawOffset;
            }

            if (updateGridPosition)
            {
                
                if (CalculateGridCoord() != GridCoordinates)
                {
                    CollisionChecker.AddOrUpdate(this);
                }
            }


            if (Animations != null)
            {
                Animations.Update(gameTime);
            }

            foreach (Entity child in children)
            {
                child.Update(gameTime);
            }
        }

        public virtual void PostUpdate(GameTime gameTime)
        {

            if (RayEmitter != null)
            {
                RayEmitter.UpdateRays();
            }

            foreach (Entity child in children)
            {
                child.PostUpdate(gameTime);
            }
        }

        private void UpdateCollisions()
        {

            if (GridCollisionCheckDirections.Count == 0)
            {
                return;
            }

            GridCoordinates = CalculateGridCoord();

            foreach ((Entity, Direction) e in circleCollisions.Keys.ToList())
            {
                circleCollisions[e] = false;
            }

            foreach ((Entity, Direction) e in collidesWithOnGrid.Keys.ToList())
            {
                collidesWithOnGrid[e] = false;
            }

            foreach ((Entity, Direction) collision in CollisionChecker.HasGridCollisionAt(GridCoordinates, GridCollisionCheckDirections))
            {
                if (collision.Item1.HasTag("Hero"))
                {
                    Logger.Log("NOW");
                }
                if (!collidesWithOnGrid.ContainsKey(collision))
                {
                    OnGridCollisionStart(collision.Item1, collision.Item2);
                }
                else
                {
                    OnGridCollision(collision.Item1, collision.Item2);
                }
                collidesWithOnGrid[collision] = true;
                if (collision.Item1.CircleCollider != null)
                {
                    (bool, float) collResult = CircleCollider.CollidesWith(collision.Item1);
                    if (collResult.Item1)
                    {
                        if (!circleCollisions.ContainsKey((collision.Item1, collision.Item2)))
                        {
                            OnCircleCollisionStart(collision.Item1, collision.Item2, collResult.Item2);
                        } else
                        {
                            OnCircleCollision(collision.Item1, collision.Item2, collResult.Item2);
                        }
                        circleCollisions[(collision.Item1, collision.Item2)] = true;
                    }
                }
            }

            foreach (KeyValuePair<(Entity, Direction), bool> e in collidesWithOnGrid.Where(e => !e.Value))
            {
                collidesWithOnGrid.Remove(e.Key);
                OnGridCollisionEnd(e.Key.Item1, e.Key.Item2);
            }

            foreach ((Entity, Direction) e in circleCollisions.Keys.ToList())
            {
                if(!circleCollisions[e])
                {
                    circleCollisions.Remove(e);
                    OnCircleCollisionEnd(e.Item1, e.Item2);
                }
            }
        }

        protected bool HasGridCollision()
        {
            return GridCollisionCheckDirections.Count > 0;
        }

        public HashSet<Entity> GetAllChildren()
        {
            return children;
        }

        public void AddChild(Entity gameObject)
        {
            children.Add(gameObject);
        }

        public void RemoveChild(Entity gameObject)
        {
            children.Remove(gameObject);
        }

        public override void Destroy()
        {

            if (DestroySound != null)
            {
                DestroySound.Play();
            }
            if (Animations != null && Animations.HasAnimation(DESTROY_AMINATION))
            {
                Sprite = null;
                Animations.PlayAnimation(DESTROY_AMINATION);
                return;
            }
            RemoveCollisions();
            Cleanup();
        }

        protected void Cleanup()
        {
            if (Sprite != null)
            {
                Sprite.Dispose();
            }
            if (DestroySound != null)
            {
                //DestroySound.Dispose();
            }
            Layer.RemoveRoot(this);
            if (parent != null)
            {
                parent.RemoveChild(this);
            }
            if (children.Any())
            {
                foreach (Entity o in children)
                {
                    if (o != null)
                    {
                        o.Destroy();
                    }
                }
            }
        }

        private void RemoveCollisions()
        {
            ColliderOnGrid = false;
        }

        public void SetDestroyAnimation(AbstractAnimation destroyAnimation)
        {
            if (Animations == null)
            {
                Animations = new AnimationStateMachine();
            }
            if (destroyAnimation.StartedCallback == null)
            {
                destroyAnimation.StartedCallback = () => RemoveCollisions();
                //destroyAnimation.StoppedAction = () => { };
            }
            if (destroyAnimation.StoppedCallback == null)
            {
                destroyAnimation.StoppedCallback = () => Cleanup();
                //destroyAnimation.StoppedAction = () => { };
            }
            destroyAnimation.Looping = false;
            Animations.RegisterAnimation(DESTROY_AMINATION, destroyAnimation, () => false);
        }

        public void SetSprite(Texture2D sprite)
        {
            if (sprite == null)
            {
                return;
            }

            this.Sprite = sprite;
            SourceRectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
        }

        public Vector2 GetPositionWithParent()
        {
            if (parent != null)
            {
                return Position + parent.GetPositionWithParent();
            }
            else
            {
                return Position;
            }
        }

        protected Vector2 CalculateGridCoord()
        {
            return CalculateGridCoord(Position);
        }

        protected Vector2 CalculateGridCoord(Vector2 position)
        {
            return new Vector2((int)Math.Floor(position.X / Config.GRID), (int)Math.Floor(position.Y / Config.GRID));
        }

        public void AddParent(Entity newParent)
        {
            if (parent != null)
            {
                parent.RemoveChild(this);
                parent.AddChild(newParent);
                newParent.AddChild(this);
                parent = newParent;
            } else
            {
                Layer.RemoveRoot(this);
                newParent.AddChild(this);
                parent = newParent;
            }
        }

        public virtual List<(Vector2 start, Vector2 end)> GetRayBlockerLines()
        {
            if (RayBlockerLines == null)
            {
                RayBlockerLines = new List<(Vector2 start, Vector2 end)>();
            }
            SetRayBlockers();
            return RayBlockerLines;
        }

        public void AddTag(string tag)
        {
            tags.Add(tag);
            if (movementBlockerTags.Contains(tag))
            {
                BlocksMovement = true;
            }
            if (gridCoordUpdateTags.Contains(tag))
            {
                updateGridPosition = true;
            }
        }

        public bool HasTag(string tag)
        {
            return tags.Contains(tag);
        }

        public bool HasAnyTag(ICollection<string> tags)
        {
            foreach (string tag in tags)
            {
                if (tags.Contains(tag))
                {
                    return true;
                }
            }
            return false;
        }

        public void RemoveTag(string tag)
        {
            tags.Remove(tag);
            foreach (string t in tags)
            {
                if (movementBlockerTags.Contains(t))
                {
                    return;
                }
            }
            BlocksMovement = false;
        }
    }
}
