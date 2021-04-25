﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonolithEngine.Engine.Source.UI
{
    public interface IUIElement
    {

        public void Draw(SpriteBatch spriteBatch);

        public void Update(Point mousePosition = default);

        public IUIElement GetParent();

        public Vector2 GetPosition();

        public bool Visible { get; set; }
    }
}
