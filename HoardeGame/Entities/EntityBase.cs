using FarseerPhysics.Dynamics;
using HoardeGame.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities
{
    public abstract class EntityBase
    {
        public DungeonLevel Level;
        public Vector2 Position => Body.Position;
        public Vector2 Velocity => Body.LinearVelocity;
        public Body Body;

        public EntityBase(World world)
        {
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
