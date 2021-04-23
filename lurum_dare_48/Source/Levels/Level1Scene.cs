using Microsoft.Xna.Framework.Graphics;
using MonolithEngine.Engine.Source.Scene;
using MonolithEngine.Engine.Source.Scene.Transition;
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

        public Level1Scene(LDTKMap world, SpriteFont font) : base ("Level_1")
        {
            this.font = font;
            this.world = world;
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
            
        }

        public override void OnEnd()
        {
            
        }

        public override void OnFinished()
        {
            
        }

        public override void OnStart()
        {
            
        }
    }
}
