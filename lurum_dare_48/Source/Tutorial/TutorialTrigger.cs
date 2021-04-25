using lurum_dare_48.Source.Entities;
using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Entities.Abstract;
using MonolithEngine.Engine.Source.Physics.Trigger;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Tutorial
{
    class TutorialTrigger : Entity 
    {

        private string tutorialName = "";

        public TutorialTrigger(AbstractScene scene, Vector2 position, int width, int height, string tutorialName) : base(scene.LayerManager.EntityLayer, null, position)
        {
            AddComponent(new BoxTrigger(this, width, height, Vector2.Zero, tag: ""));
            this.tutorialName = tutorialName;
            DEBUG_SHOW_COLLIDER = true;
            Active = true;
        }

        public override void OnEnterTrigger(string triggerTag, IGameObject otherEntity)
        {
            if (otherEntity is Hero)
            {
                Hero hero = (otherEntity as Hero);
                if (tutorialName == "Intro")
                {
                    hero.DisplayIntro();
                }
                else if (tutorialName == "Kick")
                {
                    hero.THIS_IS_SPARTAAAAA = true;
                    hero.DisplayKickTutorial();
                }
                else if (tutorialName == "Controls")
                {
                    hero.DisplayControlsTutorial();
                }
                Destroy();
            }
            base.OnEnterTrigger(triggerTag, otherEntity);
        }
    }
}
