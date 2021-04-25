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
    class LoseGameScreen : AbstractScene
    {
        public LoseGameScreen() : base("LoseGameScreen")
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
            Image lostMessage = new Image(Assets.GetTexture("HUDLost"), new Vector2(250, 150), scale: 0.25f);

            SelectableImage restart = new SelectableImage(Assets.GetTexture("HUDRestartBase"), Assets.GetTexture("HUDWRestartSelected"), new Vector2(250, 250), scale: 0.25f);
            restart.OnClick = () =>
            {
                SceneManager.LoadScene("Level_1");
            };

            SelectableImage quit = new SelectableImage(Assets.GetTexture("HUDQuitBase"), Assets.GetTexture("HUDQuitSelected"), new Vector2(250, 350), scale: 0.25f);

            quit.OnClick = Config.ExitAction;

            UI.AddUIElement(lostMessage);
            //UI.AddUIElement(restart);
            UI.AddUIElement(quit);
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
