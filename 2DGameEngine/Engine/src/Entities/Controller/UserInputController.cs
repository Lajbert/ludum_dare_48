﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine2D.Engine.src.Entities.Controller
{
    public class UserInputController
    {

        private Dictionary<Keys, bool> pressedKeys = new Dictionary<Keys, bool>();
        private Dictionary<KeyMapping, Action> keyActions = new Dictionary<KeyMapping, Action>();
        private KeyboardState currentKeyboardState;
        private KeyboardState prevKeyboardState;

        public void RegisterControllerState(Keys key, Action action, bool singlePressOnly = false) {
            keyActions.Add(new KeyMapping(key, singlePressOnly), action);
            pressedKeys.Add(key, false);
        }

        public bool IsKeyPressed(Keys key)
        {
            return pressedKeys[key];
        }

        public void Update()
        {
            currentKeyboardState = Keyboard.GetState();

            foreach (KeyValuePair<KeyMapping, Action> mapping in keyActions)
            {
                Keys key = mapping.Key.Key;
                if (currentKeyboardState.IsKeyDown(key))
                {
                    if(mapping.Key.SinglePressOnly && (prevKeyboardState != null && prevKeyboardState == currentKeyboardState)) {
                        continue;
                    }
                    pressedKeys[key] = true;
                    mapping.Value.Invoke();
                } else
                {
                    pressedKeys[key] = false;
                }
            }

            prevKeyboardState = currentKeyboardState;
        }

        private class KeyMapping
        {
            public Keys Key;
            public bool SinglePressOnly;

            public KeyMapping(Keys key, bool singlePressOnly)
            {
                Key = key;
                SinglePressOnly = singlePressOnly;
            }
        }
    }
}
