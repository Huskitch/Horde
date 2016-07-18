using System.Diagnostics;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
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

        public EntityPlayer(World world) : base(world)
        {
            Body = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(10), 1f, ConvertUnits.ToSimUnits(new Vector2(400, 400)));
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;

            animator = new AnimatedSprite(ResourceManager.Texture("PlayerSheet"));
            animator.AddAnimation("Down", 32, 0, 5, 150);
            animator.AddAnimation("Left", 32, 2, 5, 150);
            animator.AddAnimation("Right", 32, 7, 5, 150);
            animator.AddAnimation("Up", 32, 5, 5, 150);

            Player = this;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 velocity = Vector2.Zero;

            if (Main.KState.IsKeyDown(Keys.S))
            {
                velocity.Y++;
            }
            if (Main.KState.IsKeyDown(Keys.W))
            {
                velocity.Y--;
            }
            if (Main.KState.IsKeyDown(Keys.D))
            {
                velocity.X++;
            }
            if (Main.KState.IsKeyDown(Keys.A))
            {
                velocity.X--;
            }

            Body.ApplyForce(velocity * 50);

            animator.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);

            if (Main.KState.IsKeyDown(Keys.W))
            {
                animator.DrawAnimation("Up", screenPos, spriteBatch);
            }
            else if (Main.KState.IsKeyDown(Keys.S))
            {
                animator.DrawAnimation("Down", screenPos, spriteBatch);
            }
            else if (Main.KState.IsKeyDown(Keys.A))
            {
                animator.DrawAnimation("Left", screenPos, spriteBatch);
            }
            else if (Main.KState.IsKeyDown(Keys.D))
            {
                animator.DrawAnimation("Right", screenPos, spriteBatch);
            }

            base.Draw(spriteBatch);
        }
    }
}
