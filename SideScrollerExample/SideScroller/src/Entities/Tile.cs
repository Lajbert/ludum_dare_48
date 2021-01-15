﻿using GameEngine2D.Engine.Source.Entities.Animations;
using GameEngine2D.Engine.Source.Util;
using GameEngine2D.Entities;
using GameEngine2D.Global;
using GameEngine2D.Source;
using GameEngine2D.Source.Entities.Animation;
using GameEngine2D.Source.Layer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace SideScrollerExample.SideScroller.Source.Entities
{

    public class Tile : Entity
    {

        private float scale = 2.5f;
        private Vector2 animOffset = new Vector2(30, 30);
        private int animationFps = 120;

        public Tile(Layer2D layer, Vector2 position, Color color, bool coolider = true, SpriteFont font = null) : base(layer, null, position, null, coolider, font)
        {
            SetSprite(SpriteUtil.CreateRectangle(Config.GRID, color));
            Animations = new AnimationStateMachine();
            //Animations = new AnimationStateMachine();
            string folder = "PixelSimulations/";
            List<Texture2D> explosion = SpriteUtil.LoadTextures(folder + "Explosion3/000", 1, 9);
            explosion.AddRange(SpriteUtil.LoadTextures(folder + "Explosion3/00", 10, 30));

            SpriteGroupAnimation explode = new SpriteGroupAnimation(this, explosion, animationFps);
            explode.Scale = scale;
            Animations.Offset = animOffset;
            SetDestroyAnimation(explode);

            //DestroySound = contentManager.Load<SoundEffect>("Audio/Effects/Explosion");
            //Func<bool> isExploding = () => true;
            //Animations.RegisterAnimation("Idle", explode, isExploding);
        }

        public override void Destroy()
        {
            base.Destroy();
            //Globals.Camera.Shake(2, 0.3f);
        }
    }
}
