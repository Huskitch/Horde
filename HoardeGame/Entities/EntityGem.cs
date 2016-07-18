using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoardeGame.Graphics.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities
{
    public class EntityGem : EntityBase
    {
        private AnimatedSprite animator;

        public EntityGem()
        {
            animator = new AnimatedSprite(ResourceManager.Texture("GemAnimation"));
            animator.AddAnimation("Bounce", 12, 0, 7, 150);
        }

        public override void Update(GameTime gameTime)
        {
            animator.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animator.DrawAnimation("Bounce", Position, spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
