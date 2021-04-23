using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonolithEngine;
using MonolithEngine.Engine.Source.Entities;
using MonolithEngine.Engine.Source.Entities.Controller;
using MonolithEngine.Engine.Source.Global;
using MonolithEngine.Engine.Source.Graphics;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Util;
using MonolithEngine.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities
{
    class Hero : PhysicalEntity
    {
        public Hero(AbstractScene scene, Vector2 position) : base(scene.LayerManager.EntityLayer, null, position)
        {

            //DEBUG_SHOW_PIVOT = true;

            SetupController();

            CollisionOffsetBottom = 1;
            CollisionOffsetLeft = 0.5f;
            CollisionOffsetRight = 0.5f;

            AddComponent(new Sprite(this, AssetUtil.CreateRectangle(Config.GRID, Color.Red), drawOffset: new Vector2(-8, -16)));

            Visible = true;
            Active = true;
        }

        private void SetupController()
        {
            UserInput = new UserInputController();

            UserInput.RegisterKeyPressAction(Keys.Right, (Vector2 thumbStickPosition) =>
            {
                VelocityX += MovementSpeed * Globals.FixedUpdateMultiplier * Config.TIME_OFFSET;
                CurrentFaceDirection = Direction.EAST;
            });

            UserInput.RegisterKeyPressAction(Keys.Left, Buttons.LeftThumbstickLeft, (Vector2 thumbStickPosition) =>
            {
                VelocityX -= MovementSpeed * Globals.FixedUpdateMultiplier * Config.TIME_OFFSET;
                CurrentFaceDirection = Direction.WEST;
            });

            UserInput.RegisterKeyPressAction(Keys.Up, (Vector2 thumbStickPosition) =>
            {
                VelocityY -= Config.JUMP_FORCE;
            }, true, 300);

            /*UserInput.RegisterKeyPressAction(Keys.Down, Buttons.LeftThumbstickLeft, (Vector2 thumbStickPosition) =>
            {
                VelocityY += MovementSpeed * Globals.FixedUpdateMultiplier * Config.TIME_OFFSET;
            });*/

            UserInput.RegisterMouseActions(
                () =>
                {
                    Timer.Repeat(300, (elapsedTime) =>
                    {
                        Scene.Camera.Zoom += 0.002f * elapsedTime;
                    });
                },
                () =>
                {
                    Timer.Repeat(300, (elapsedTime) =>
                    {
                        Scene.Camera.Zoom -= 0.002f * elapsedTime;
                    });
                }
            , 100);
        }
    }
}
