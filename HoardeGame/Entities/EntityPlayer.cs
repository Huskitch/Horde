using System.Diagnostics;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
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

        public EntityPlayer(World world) : base(world)
        {
            Body = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(10), 1f, ConvertUnits.ToSimUnits(new Vector2(400, 400)));
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;

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

            Body.ApplyForce(velocity * 100);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);
            Debug.WriteLine(screenPos);

            spriteBatch.Draw(ResourceManager.Texture("PlayerTemp"), new Rectangle((int)screenPos.X + 1, (int)screenPos.Y + 8, 32, 32), new Rectangle(0, 0, 32, 32), Color.White);
            base.Draw(spriteBatch);
        }
    }
}
