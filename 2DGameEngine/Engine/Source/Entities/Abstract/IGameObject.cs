﻿using GameEngine2D.Engine.Source.Entities.Transform;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine2D.Engine.Source.Entities.Abstract
{
    public interface IGameObject
    {
        public AbstractTransform Transform { get; set; }

        public IGameObject Parent { get; }

        public ICollection<string> GetTags();

        public void AddChild(IGameObject gameObject);

        public void RemoveChild(IGameObject gameObject);
    }
}
