using ForestPlatformerExample.Source.Scenes;
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
            VideoConfiguration.RESOLUTION_WIDTH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            VideoConfiguration.RESOLUTION_HEIGHT = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            VideoConfiguration.FULLSCREEN = false;
            VideoConfiguration.FRAME_LIMIT = 0;
            VideoConfiguration.VSYNC = true;
        }

        protected override void LoadGameContent()
        {

            // UI text generated with: https://fontmeme.com/pixel-fonts/
            // font: KA1
            // base color: 2A2A57
            // selected color: FF0000
            Assets.LoadTexture("HUDNewGameBase", "UI/new_game_base");
            Assets.LoadTexture("HUDNewGameSelected", "UI/new_game_selected");
            Assets.LoadTexture("HUDSettingsBase", "UI/settings_base");
            Assets.LoadTexture("HUDSettingsSelected", "UI/settings_selected");
            Assets.LoadTexture("HUDQuitBase", "UI/quit_base");
            Assets.LoadTexture("HUDQuitSelected", "UI/quit_selected");
            Assets.LoadTexture("HUDContinueBase", "UI/continue_base");
            Assets.LoadTexture("HUDContinueSelected", "UI/continue_selected");
            Assets.LoadTexture("HUDVideoSettingsBase", "UI/video_base");
            Assets.LoadTexture("HUDVideoSettingsSelected", "UI/video_selected");
            Assets.LoadTexture("HUDAudioSettingsBase", "UI/audio_base");
            Assets.LoadTexture("HUDAudioSettingsSelected", "UI/audio_selected");
            Assets.LoadTexture("HUDBackBase", "UI/back_base");
            Assets.LoadTexture("HUDBackSelected", "UI/back_selected");
            Assets.LoadTexture("HUDResolutionLabel", "UI/resolution");
            Assets.LoadTexture("HUDFPSLimitLabel", "UI/fps_limit");
            Assets.LoadTexture("HUDVsyncLabel", "UI/vsync");
            Assets.LoadTexture("HUDWindowModeLabel", "UI/window_mode");
            Assets.LoadTexture("HUD30", "UI/30");
            Assets.LoadTexture("HUD60", "UI/60");
            Assets.LoadTexture("HUD120", "UI/120");
            Assets.LoadTexture("HUDUnlimited", "UI/unlimited");
            Assets.LoadTexture("HUD720p", "UI/720p");
            Assets.LoadTexture("HUD1080p", "UI/1080p");
            Assets.LoadTexture("HUD1440p", "UI/1440p");
            Assets.LoadTexture("HUD4K", "UI/4k");
            Assets.LoadTexture("HUDOn", "UI/on");
            Assets.LoadTexture("HUDOff", "UI/off");
            Assets.LoadTexture("HUDApplyBase", "UI/apply_base");
            Assets.LoadTexture("HUDApplySelected", "UI/apply_selected");
            Assets.LoadTexture("HUDCancelBase", "UI/cancel_base");
            Assets.LoadTexture("HUDCancelSelected", "UI/cancel_selected");
            Assets.LoadTexture("HUDWindowed", "UI/windowed");
            Assets.LoadTexture("HUDFullscreen", "UI/fullscreen");
            Assets.LoadTexture("HUDArrowRightBase", "UI/arrow_right_base");
            Assets.LoadTexture("HUDArrowRightSelected", "UI/arrow_right_selected");
            Assets.LoadTexture("HUDArrowLeftBase", "UI/arrow_right_base", flipHorizontal: true);
            Assets.LoadTexture("HUDArrowLeftSelected", "UI/arrow_right_selected", flipHorizontal: true);
            Assets.LoadTexture("HUDLoading", "UI/loading");
            Assets.LoadTexture("HUDWinningBase", "UI/winning");
            Assets.LoadTexture("HUDWinningSelected", "UI/winning_selected");
            Assets.LoadTexture("HUDRestartBase", "UI/restart");
            Assets.LoadTexture("HUDWRestartSelected", "UI/restart_selected");
            Assets.LoadTexture("HUDLost", "UI/wasted");

            Assets.LoadTexture("HeroIdle", "Hero/Sprites/left_idle");
            Assets.LoadTexture("HeroRun", "Hero/Sprites/run_left");
            Assets.LoadTexture("HeroRunBackwards", "Hero/Sprites/run_backwards_left");
            Assets.LoadTexture("Jetpacking", "Hero/Sprites/jatpack_on");
            Assets.LoadTexture("Kick", "Hero/Sprites/kick_jetpack_off");
            Assets.LoadTexture("KickJetpacking", "Hero/Sprites/kick_jetpack_on");

            Assets.LoadTexture("HandgunBulletPoint", "Hero/Sprites/handgun_bullet_point");
            Assets.LoadTexture("HandgunBulletShell", "Hero/Sprites/handgun_bullet_shell");
            Assets.LoadTexture("ShotgunShell", "Hero/Sprites/shotgun_shelll");

            Assets.LoadTexture("Gun", "Hero/Sprites/handgun_left");
            Assets.LoadTexture("Machinegun", "Hero/Sprites/machine_gun_left");
            Assets.LoadTexture("Shotgun", "Hero/Sprites/shotgun");

            Assets.LoadTexture("Spikes", "Enivornment/Traps/spikes");
            Assets.LoadTexture("Saw", "Enivornment/Traps/saw");
            Assets.LoadTexture("Keycard", "Enivornment/Items/key");
            Assets.LoadTexture("Door", "Enivornment/Items/door");

            Assets.LoadTexture("HandgunAmmo", "Enivornment/Items/pistol_ammo");
            Assets.LoadTexture("MachineGunAmmo", "Enivornment/Items/machine_gun_ammo");
            Assets.LoadTexture("ShotgunAmmo", "Enivornment/Items/shotgun_ammo");
            Assets.LoadTexture("FuelCan", "Enivornment/Items/fuel_can");

            Assets.LoadTexture("Lava", "Enivornment/Tilesheets/lava");

            Assets.LoadTexture("MountedGun", "Enivornment/Traps/mounted_gun");

            Assets.LoadTexture("IntroText", "Text/intro");
            Assets.LoadTexture("KickText", "Text/kick_tutorial");
            Assets.LoadTexture("ControlsText", "Text/controls_tutorial");
            Assets.LoadTexture("SpartaText", "Text/sparta");

            Assets.LoadTexture("FuelAmmoText", "Text/fuel_ammo_pickup");

            Assets.LoadTexture("EnemyRun", "Enemies/enemy_run_left");
            Assets.LoadTexture("EnemyIdle", "Enemies/left_idle");

            Assets.LoadTexture("UIHealth", "Hero/Sprites/UI_health");

            AudioEngine.AddSound("AmmoPickup", "Audio/ammo_pickup");
            AudioEngine.AddSound("FuelPickup", "Audio/fuel_pickup");
            AudioEngine.AddSound("HandgunShot", "Audio/handgun_shot");
            AudioEngine.AddSound("HealthPickup", "Audio/health_pickup");
            AudioEngine.AddSound("HeroJump", "Audio/hero_jump");
            AudioEngine.AddSound("KeyPickup", "Audio/key_pickup");
            AudioEngine.AddSound("MachineGunShot", "Audio/machine_gun_shot");
            AudioEngine.AddSound("MountedGunExplode", "Audio/mounted_gun_explode");
            AudioEngine.AddSound("MountedGunShoot", "Audio/mounted_gun_shoot");
            AudioEngine.AddSound("ShotgunShot", "Audio/shotgun_shot");
            AudioEngine.AddSound("Kick", "Audio/kick");
            AudioEngine.AddSound("HeroHurt", "Audio/hurt");
            AudioEngine.AddSound("EnemyDie", "Audio/enemy_die");
            AudioEngine.AddSound("DoorOpen", "Audio/door_open");
            AudioEngine.AddSound("Jetpack", "Audio/jetpack", true);
            AudioEngine.AddSound("Music", "Audio/8-bit (1)", true);

            MapSerializer mapSerializer = new LDTKJsonMapParser();
            world = mapSerializer.Load("D:/GameDev/MonoGame/lurum_dare_48/lurum_dare_48/lurum_dare_48/Maps/ld_48_world.json");

            MainMenuScene mainMenuScene = new MainMenuScene();
            PauseMenuScene pauseMenuScene = new PauseMenuScene();
            Level1Scene level1 = new Level1Scene(world, font);
            SettingsScene settings = new SettingsScene();
            VideoSettingsScene videoSettings = new VideoSettingsScene();
            LoadingScreenScene loadingScreen = new LoadingScreenScene();
            EndGameScene endGameScene = new EndGameScene();
            LoseGameScreen loseGameScreen = new LoseGameScreen();

            SceneManager.AddScene(mainMenuScene);
            SceneManager.AddScene(settings);
            SceneManager.AddScene(pauseMenuScene);
            SceneManager.AddScene(level1);
            SceneManager.AddScene(videoSettings);
            SceneManager.AddScene(loadingScreen);
            SceneManager.AddScene(endGameScene);
            SceneManager.AddScene(loseGameScreen);

            SceneManager.LoadScene(mainMenuScene);
            SceneManager.SetLoadingScene(loadingScreen);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState state = Keyboard.GetState();

            if (prevKeyboardState != state && state.IsKeyDown(Keys.R))
            {
                SceneManager.LoadScene("Level_1");
            }
            else if (prevKeyboardState != state && state.IsKeyDown(Keys.Escape) && WasGameStarted && !Paused)
            {
                SceneManager.StartScene("PauseMenu");
            }
            else if (prevKeyboardState != state && state.IsKeyDown(Keys.Escape) && WasGameStarted && Paused)
            {
                SceneManager.StartScene("Level_1");
            }
            else if (prevKeyboardState != state && state.IsKeyDown(Keys.Escape) && !WasGameStarted)
            {
                SceneManager.StartScene("MainMenu");
            }

            prevKeyboardState = state;
        }
    }
}
