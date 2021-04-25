using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonolithEngine.Engine.Source.Entities.Controller;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Tutorial
{
    class TextPopup : Entity
    {
        UserInputController input;

        public TextPopup(AbstractScene scene, Texture2D texture, Vector2 position, float scale = 1, float timeout = 0) : base(scene.LayerManager.TutorialLayer, null, position)
        {
            input = new UserInputController();

            if (timeout == 0)
            {
                Scene.LayerManager.PauseForUserInput();

                Timer.TriggerAfter(1000, () =>
                {
                    input.RegisterKeyPressAction(Keys.Space, (thumbstickPos) =>
                    {
                        Scene.LayerManager.Continue();
                        Destroy();
                    });
                });
            }
            else
            {
                Timer.TriggerAfter(timeout, () => {
                    Destroy();
                });
            }

            Sprite s = new Sprite(this, texture, new Rectangle(0, 0, texture.Width, texture.Height));
            s.Scale = scale;
            AddComponent(s);

            Active = true;
            Visible = true;
        }

        public override void Update()
        {
            input.Update();
            base.Update();
        }
    }
}
