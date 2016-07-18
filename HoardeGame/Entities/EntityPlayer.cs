using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoardeGame.Graphics.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.Entities
{
    public class EntityPlayer : EntityBase
    {
        private KeyboardState keyboardState;
        private AnimatedSprite animator;

        public EntityPlayer()
        {
            Speed = 2f;
            animator = new AnimatedSprite(ResourceManager.Texture("PlayerSheet"));
            animator.AddAnimation("WalkLeft", 32, 7, 5, 120);
        }

        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            Velocity = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.S))
                Velocity.Y++;

            if (keyboardState.IsKeyDown(Keys.W))
                Velocity.Y--;

            if (keyboardState.IsKeyDown(Keys.D))
                Velocity.X++;

            if (keyboardState.IsKeyDown(Keys.A))
                Velocity.X--;

            if (Velocity != Vector2.Zero)
                Position += Velocity * Speed;

            animator.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animator.DrawAnimation("WalkLeft", Position, spriteBatch);
            //spriteBatch.Draw(, new Rectangle((int)Position.X, (int)Position.Y, 32, 32), new Rectangle(0, 0, 32, 32), Color.White);
            base.Draw(spriteBatch);
        }
    }
}
