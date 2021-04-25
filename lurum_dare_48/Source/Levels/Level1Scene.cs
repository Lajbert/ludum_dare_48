using lurum_dare_48.Source.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonolithEngine.Engine.Source.Asset;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Scene.Transition;
using MonolithEngine.Engine.Source.UI;
using MonolithEngine.Source.Level;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Levels
{
    class Level1Scene : AbstractScene
    {
        private SpriteFont font;
        private LDTKMap world;
        private Hero hero;

        public Image HandgunImage;
        public Image MachinegunImage;
        public Image ShotgunImage;

        public Image CurrentlyDisplayed;

        public Level1Scene(LDTKMap world, SpriteFont font) : base ("Level_1", useLoadingScreen: true)
        {
            this.font = font;
            this.world = world;
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

            EntityParser parser = new EntityParser(world);

            parser.LoadEntities(this, sceneName);

            hero = parser.GetHero();

            HandgunImage = new Image(Assets.GetTexture("HandgunAmmo"), new Vector2(5, 5), scale: 2);
            MachinegunImage = new Image(Assets.GetTexture("MachineGunAmmo"), new Vector2(5, 5), scale: 2);
            ShotgunImage = new Image(Assets.GetTexture("ShotgunAmmo"), new Vector2(5, 5), scale: 2);

            HandgunImage.Visible = false;
            ShotgunImage.Visible = false;
            MachinegunImage.Visible = false;

            CurrentlyDisplayed = HandgunImage;
            CurrentlyDisplayed.Visible = true;

            UI.AddUIElement(HandgunImage);
            UI.AddUIElement(MachinegunImage);
            UI.AddUIElement(ShotgunImage);
            UI.AddUIElement(new TextField(font, () => hero.CurrentWeapon.GetAmmo() + "", new Vector2(50, 5), scale: 2f));
            UI.AddUIElement(new Image(Assets.GetTexture("FuelCan"), new Vector2(5, 50), scale: 1.5f));
            UI.AddUIElement(new TextField(font, () => (int)(((float)(hero.Fuel / hero.TankCapacity)) * 100) + "%", new Vector2(50, 45), scale: 2f));
            UI.AddUIElement(new Image(Assets.GetTexture("UIHealth"), new Vector2(-10, 85), scale: 1.5f));
            UI.AddUIElement(new TextField(font, () => hero.Health + "", new Vector2(50, 95), scale: 2f));
        }

        public void NextIcon()
        {
            CurrentlyDisplayed.Visible = false;
            if (CurrentlyDisplayed.Equals(HandgunImage))
            {
                CurrentlyDisplayed = MachinegunImage;
            } 
            else if (CurrentlyDisplayed.Equals(MachinegunImage))
            {
                CurrentlyDisplayed = ShotgunImage;
            }
            else if (CurrentlyDisplayed.Equals(ShotgunImage))
            {
                CurrentlyDisplayed = HandgunImage;
            }
            CurrentlyDisplayed.Visible = true;
        }

        public void PrevIcon()
        {
            CurrentlyDisplayed.Visible = false;
            if (CurrentlyDisplayed.Equals(HandgunImage))
            {
                CurrentlyDisplayed = ShotgunImage;
            }
            else if (CurrentlyDisplayed.Equals(MachinegunImage))
            {
                CurrentlyDisplayed = HandgunImage;
            }
            else if (CurrentlyDisplayed.Equals(ShotgunImage))
            {
                CurrentlyDisplayed = MachinegunImage;
            }
            CurrentlyDisplayed.Visible = true;
        }

        public override void OnEnd()
        {
            
        }

        public override void OnFinished()
        {
            SceneManager.LoadScene("EndGame");
        }

        public override void OnStart()
        {
            Camera.TrackTarget(hero, true);
            LD48Game.Paused = false;
            LD48Game.WasGameStarted = true;
        }
    }
}
