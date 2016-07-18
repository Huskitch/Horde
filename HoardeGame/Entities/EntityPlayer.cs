using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.Entities
{
    public class EntityPlayer : EntityBase
    {
        /// <summary>
        /// Singleton of player
        /// </summary>
        public static EntityPlayer Player;

        public EntityPlayer()
        {
            //This makes the character jitter a bit
            Speed = 1.5f;

            Player = this;
        }

        public override void Update(GameTime gameTime)
        {
            Velocity = Vector2.Zero;

            if (Main.KState.IsKeyDown(Keys.S))
            {
                Velocity.Y++;
            }
            if (Main.KState.IsKeyDown(Keys.W))
            {
                Velocity.Y--;
            }
            if (Main.KState.IsKeyDown(Keys.D))
            {
                Velocity.X++;
            }
            if (Main.KState.IsKeyDown(Keys.A))
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
