using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Scene.Transition;
using MonolithEngine.Engine.Source.UI;
using MonolithEngine.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Levels
{

    class EndGameScene : AbstractScene
    {
        public EndGameScene() : base("EndGame")
        {
            BackgroundColor = Color.DimGray;
        }

        public override ICollection<object> ExportData()
        {
            return null;
        }

        public override ISceneTransitionEffect GetTransitionEffect()
        {
            return null;
        }

        public override void ImportData(ICollection<object> state)
        {

        }

        public override void Load()
        {
            Image newGame = new Image(Assets.GetTexture("HUDWinningBase"), new Vector2(250, 150), scale: 0.25f);

            SelectableImage quit = new SelectableImage(Assets.GetTexture("HUDQuitBase"), Assets.GetTexture("HUDQuitSelected"), new Vector2(450, 300), scale: 0.25f);

            quit.OnClick = Config.ExitAction;

            UI.AddUIElement(newGame);
            UI.AddUIElement(quit);

            UI.AddUIElement(newGame);
        }

        public override void OnEnd()
        {

        }

        public override void OnStart()
        {
            LD48Game.Paused = false;
        }

        public override void OnFinished()
        {

        }
    }
}