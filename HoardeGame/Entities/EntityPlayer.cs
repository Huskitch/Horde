using HoardeGame.Graphics.Rendering;
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
        private AnimatedSprite animator;

        public EntityPlayer()
        {

            Player = this;
        }

        public override void Update(GameTime gameTime)
        {
            Velocity = Vector2.Zero;

            if (Main.KState.IsKeyDown(Keys.S))
                Velocity.Y++;

            if (Main.KState.IsKeyDown(Keys.W))
                Velocity.Y--;

            if (Main.KState.IsKeyDown(Keys.D))
                Velocity.X++;

            if (Main.KState.IsKeyDown(Keys.A))
                Velocity.X--;

            if (Velocity != Vector2.Zero)
                Position += Velocity * Speed;

            animator.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            animator.DrawAnimation("WalkLeft", Position, spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
