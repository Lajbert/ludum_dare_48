using lurum_dare_48.Source.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Audio;
using MonolithEngine.Engine.Source.MyGame;
using MonolithEngine.Source.Level;

namespace lurum_dare_48
{
    public class LD48Game : MonolithGame
    {

        public static int CoinCount = 0;

        private SpriteFont font;

        private KeyboardState prevKeyboardState;

        public static bool Paused = false;
        public static bool WasGameStarted = false;

        private LDTKMap world;

        protected override void Init()
        {
            font = Content.Load<SpriteFont>("DefaultFont");


            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            VideoConfiguration.RESOLUTION_WIDTH = 2560;
            VideoConfiguration.RESOLUTION_HEIGHT = 1440;
            VideoConfiguration.FULLSCREEN = false;
            VideoConfiguration.FRAME_LIMIT = 200;
            VideoConfiguration.VSYNC = false;
        }

        protected override void LoadGameContent()
        {

            Assets.LoadTexture("HeroIdle", "Hero/Sprites/left_idle");
            Assets.LoadTexture("HeroRun", "Hero/Sprites/run_left");
            Assets.LoadTexture("HeroRunBackwards", "Hero/Sprites/run_backwards_left");
            Assets.LoadTexture("Jetpacking", "Hero/Sprites/jatpack_on");
            Assets.LoadTexture("Kick", "Hero/Sprites/kick_jetpack_off");
            Assets.LoadTexture("KickJetpacking", "Hero/Sprites/kick_jetpack_on");

            Assets.LoadTexture("Gun", "Hero/Sprites/handgun_left");

            MapSerializer mapSerializer = new LDTKJsonMapParser();
            world = mapSerializer.Load("D:/GameDev/MonoGame/lurum_dare_48/lurum_dare_48/lurum_dare_48/Maps/ld_48_world.json");

            Level1Scene level1 = new Level1Scene(world, font);

            SceneManager.AddScene(level1);

            SceneManager.LoadScene(level1);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            /*KeyboardState state = Keyboard.GetState();

            if (prevKeyboardState != state && state.IsKeyDown(Keys.R))
            {
                SceneManager.LoadScene("Level1");
            }
            else if (prevKeyboardState != state && state.IsKeyDown(Keys.Escape) && WasGameStarted && !Paused)
            {
                SceneManager.StartScene("PauseMenu");
            }
            else if (prevKeyboardState != state && state.IsKeyDown(Keys.Escape) && WasGameStarted && Paused)
            {
                SceneManager.StartScene("Level1");
            }
            else if (prevKeyboardState != state && state.IsKeyDown(Keys.Escape) && !WasGameStarted)
            {
                SceneManager.StartScene("MainMenu");
            }

            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            prevKeyboardState = state;*/
        }
    }
}
