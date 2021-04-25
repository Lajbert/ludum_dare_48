﻿using MonolithEngine.Engine.Source.Entities.Controller;
using MonolithEngine.Engine.Source.UI.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonolithEngine.Engine.Source.UI
{
    public class UserInterface
    {
        private List<IUIElement> elements = new List<IUIElement>();

        private List<IUIElement> newElements = new List<IUIElement>();
        private List<IUIElement> removedElements = new List<IUIElement>();

        private IUIElement selectedElement;
        private MouseState currentMouseState;
        private MouseState prevMouseState;

        public void OnResolutionChanged()
        {
            foreach (IUIElement element in elements)
            {
                if (element is SelectableUIElement)
                {
                    (element as SelectableUIElement).OnResolutionChanged();
                }
            }
        }

        public void AddUIElement(IUIElement newElement)
        {
            newElements.Add(newElement);
            if (newElement is SelectableUIElement)
            {
                (newElement as SelectableUIElement).SetUserInterface(this);
            }
        }

        public void RemoveUIElement(IUIElement toRemove)
        {
            removedElements.Add(toRemove);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            HandleNewElements();

            foreach (IUIElement element in elements)
            {
                if (element.Visible)
                {
                    element.Draw(spriteBatch);
                }
            }
        }

        public void SelectElement(IUIElement selectedElement)
        {
            this.selectedElement = selectedElement;
        }

        public void DeselectElement(IUIElement deselected)
        {
            if (selectedElement != null && selectedElement.Equals(deselected)) {
                selectedElement = null;
            }
        }

        public void Update()
        {
            currentMouseState = Mouse.GetState();
            
            if (selectedElement != null && currentMouseState.LeftButton == ButtonState.Pressed && (prevMouseState == null || prevMouseState.LeftButton != ButtonState.Pressed))
            {
                if (selectedElement is SelectableUIElement)
                {
                    (selectedElement as SelectableUIElement).OnClick();
                }
            }

            foreach (IUIElement element in elements)
            {
                element.Update(currentMouseState.Position);
            }

            HandleNewElements();

            prevMouseState = currentMouseState;
        }

        internal void HandleNewElements()
        {
            if (newElements.Count > 0)
            {
                foreach (IUIElement element in newElements)
                {
                    elements.Add(element);
                }

                newElements.Clear();
            }

            if (removedElements.Count > 0)
            {
                foreach (IUIElement element in removedElements)
                {
                    elements.Remove(element);
                }

                removedElements.Clear();
            }
        }
    }
}
