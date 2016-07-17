using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HoardeGame.State
{
    public abstract class GameState
    {
        public virtual void Start() { }
        public virtual void End() { }
        public virtual void Pause() { }
        public virtual void Resume() { }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime, float interpolation) { }
    }
}
