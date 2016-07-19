using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Graphics.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities
{
    public class EntityBat : EntityBase
    {
        private AnimatedSprite animator;

        public EntityBat(World world) : base(world)
        {
            Body = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(10), 1f, ConvertUnits.ToSimUnits(new Vector2(500, 500)));
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;

            animator = new AnimatedSprite(ResourceManager.Texture("BatSheet"));
            animator.AddAnimation("Flap", 32, 0, 2, 100);
        }

        public override void Update(GameTime gameTime)
        {
            animator.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);
            animator.DrawAnimation("Flap", screenPos, spriteBatch);
        }
    }
}