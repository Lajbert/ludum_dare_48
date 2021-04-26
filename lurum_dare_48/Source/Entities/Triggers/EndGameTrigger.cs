using Microsoft.Xna.Framework;
using MonolithEngine.Engine.Source.Audio;
using MonolithEngine.Engine.Source.Entities.Abstract;
using MonolithEngine.Engine.Source.Physics.Trigger;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace lurum_dare_48.Source.Entities.Triggers
{
    class EndGameTrigger : Entity
    {
        public EndGameTrigger(AbstractScene scene, Vector2 position, int width, int height) : base(scene.LayerManager.EntityLayer, null, position)
        {
            Active = true;

            AddComponent(new BoxTrigger(this, width, height, Vector2.Zero, tag: ""));
            //(GetComponent<ITrigger>() as BoxTrigger).DEBUG_DISPLAY_TRIGGER = true;
            //DEBUG_SHOW_PIVOT = true;
        }

        public override void OnEnterTrigger(string triggerTag, IGameObject otherEntity)
        {
            base.OnEnterTrigger(triggerTag, otherEntity);
            AudioEngine.MuteAll();
            if (otherEntity is Hero)
            {
                Scene.Finish();
            }
        }
    }
}
