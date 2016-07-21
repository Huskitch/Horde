using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics;
using HoardeGame.Entities;
using HoardeGame.Level;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Gameplay
{
    public class EntityWeapon : EntityBase
    {
        private List<EntityBullet> bullets;
        private float fireTimer;

        public int FireRate = 100;

        private readonly DungeonLevel level;
        private readonly IResourceProvider resourceProvider;
        private readonly EntityBase owner;

        public EntityWeapon(DungeonLevel level, IResourceProvider resourceProvider, EntityBase owner) : base(level)
        {
            this.level = level;
            this.resourceProvider = resourceProvider;
            this.owner = owner;

            bullets = new List<EntityBullet>();
        }

        public void Shoot(Vector2 direction)
        {
            if (fireTimer > FireRate)
            {
                bullets.Add(new EntityBullet(level, resourceProvider, ConvertUnits.ToDisplayUnits(owner.Position), direction));
                fireTimer = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            fireTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].Removed)
                {
                    bullets[i].Body.Dispose();
                    bullets.Remove(bullets[i]);
                    continue;
                }

                bullets[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (EntityBullet bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }
    }
}
