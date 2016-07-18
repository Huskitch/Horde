using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.Entities
{
    public class EntityPlayer : EntityBase
    {
        private KeyboardState keyboardState;

        public EntityPlayer()
        {
            Speed = 1.5f;
        }

        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            Velocity = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.S))
            {
                Velocity.Y++;
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                Velocity.Y--;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                Velocity.X++;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                Velocity.X--;
            }

            if (Velocity != Vector2.Zero)
            {
                Position += Velocity * Speed;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ResourceManager.Texture("PlayerTemp"), new Rectangle((int)Position.X, (int)Position.Y, 32, 32), new Rectangle(0, 0, 32, 32), Color.White);
            base.Draw(spriteBatch);
        }
    }
}
