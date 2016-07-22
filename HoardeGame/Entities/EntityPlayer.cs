// <copyright file="EntityPlayer.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Graphics.Rendering;
using HoardeGame.Input;
using HoardeGame.Level;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Player entity
    /// </summary>
    public class EntityPlayer : EntityBase
    {
        /// <summary>
        /// Gets the amount of gems the player has
        /// </summary>
        public int[] Gems { get; } = new int[4];

        /// <summary>
        /// Gets or sets a value indicating whether the player is dead
        /// Can't just remove the entity because that would break everything
        /// </summary>
        public bool Dead { get; set; }

        /// <summary>
        /// Gets or sets the amount of ammo the player has
        /// </summary>
        public int Ammo { get; set; }

        /// <summary>
        /// Gets or sets the armour of the player
        /// </summary>
        public int Armour { get; set; }

        /// <summary>
        /// Gets or sets the weapon of the player
        /// </summary>
        public EntityWeapon Weapon { get; set; }

        private AnimatedSprite animator;
        private IInputProvider inputProvider;

        private enum Directions
        {
            NORTH,
            EAST,
            SOUTH,
            WEST,
            NORTHEAST,
            NORTHWEST,
            SOUTHEAST,
            SOUTHWEST
        }

        private readonly IResourceProvider resourceProvider;
        private Directions direction;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityPlayer"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> in which the player will spawn</param>
        /// <param name="inputProvider"><see cref="IInputProvider"/> to use for input</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        public EntityPlayer(DungeonLevel level, IInputProvider inputProvider, IResourceProvider resourceProvider) : base(level)
        {
            this.inputProvider = inputProvider;
            this.resourceProvider = resourceProvider;

            FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(10), 1f, Body);
            Body.Position = Level.GetSpawnPosition();
            Body.CollisionCategories = Category.Cat1;
            Body.CollidesWith = Category.Cat3 | Category.Cat4;

            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;

            Health = 10;
            Armour = 10;
            Ammo = 100;
            BlinkMultiplier = 1;

            animator = new AnimatedSprite(resourceProvider.GetTexture("PlayerSheet"));
            animator.AddAnimation("South", 32, 0, 5, 100);
            animator.AddAnimation("West", 32, 2, 5, 100);
            animator.AddAnimation("East", 32, 6, 5, 100);
            animator.AddAnimation("North", 32, 4, 5, 100);
            animator.AddAnimation("SouthWest", 32, 1, 5, 100);
            animator.AddAnimation("SouthEast", 32, 7, 5, 100);
            animator.AddAnimation("NorthWest", 32, 3, 5, 100);
            animator.AddAnimation("NorthEast", 32, 5, 5, 100);
            animator.AddAnimation("Idle", 32, 8, 5, 100);
            animator.SetDefaultAnimation("Idle");

            Weapon = new EntityWeapon(level, resourceProvider, this);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (Dead)
            {
                return;
            }

            Vector2 velocity = Vector2.Zero;

            if (inputProvider.KeyboardState.IsKeyDown(Keys.S))
            {
                direction = Directions.SOUTH;
                velocity.Y++;
            }

            if (inputProvider.KeyboardState.IsKeyDown(Keys.W))
            {
                direction = Directions.NORTH;
                velocity.Y--;
            }

            if (inputProvider.KeyboardState.IsKeyDown(Keys.D))
            {
                if (direction == Directions.SOUTH)
                {
                    direction = Directions.SOUTHEAST;
                }
                else if (direction == Directions.NORTH)
                {
                    direction = Directions.NORTHEAST;
                }
                else
                {
                    direction = Directions.EAST;
                }

                velocity.X++;
            }

            if (inputProvider.KeyboardState.IsKeyDown(Keys.A))
            {
                if (direction == Directions.SOUTH)
                {
                    direction = Directions.SOUTHWEST;
                }
                else if (direction == Directions.NORTH)
                {
                    direction = Directions.NORTHWEST;
                }
                else
                {
                    direction = Directions.WEST;
                }

                velocity.X--;
            }

            if (inputProvider.KeyboardState.IsKeyDown(Keys.Space) && Ammo > 0)
            {
                Vector2 direction = Vector2.Zero;

                switch (this.direction)
                {
                    case Directions.NORTH:
                        direction = new Vector2(0, -1);
                        break;
                    case Directions.SOUTH:
                        direction = new Vector2(0, 1);
                        break;
                    case Directions.WEST:
                        direction = new Vector2(-1, 0);
                        break;
                    case Directions.EAST:
                        direction = new Vector2(1, 0);
                        break;
                    case Directions.NORTHEAST:
                        direction = new Vector2(1, -1);
                        break;
                    case Directions.NORTHWEST:
                        direction = new Vector2(-1, -1);
                        break;
                    case Directions.SOUTHEAST:
                        direction = new Vector2(1, 1);
                        break;
                    case Directions.SOUTHWEST:
                        direction = new Vector2(-1, 1);
                        break;
                }

                if (Weapon.Shoot(direction))
                {
                    Ammo--;
                }
            }

            Body.ApplyForce(velocity * 50);

            animator.Update(gameTime);
            Weapon.Update(gameTime);

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, Effect effect)
        {
            if (Dead)
            {
                return;
            }

            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);

            if (Velocity.Length() < 0.1f)
            {
                animator.DrawAnimation("Idle", screenPos, spriteBatch, CurrentBlinkFrame);
            }
            else
            {
                switch (direction)
                {
                    case Directions.NORTH:
                        animator.DrawAnimation("North", screenPos, spriteBatch, CurrentBlinkFrame);
                        break;
                    case Directions.SOUTH:
                        animator.DrawAnimation("South", screenPos, spriteBatch, CurrentBlinkFrame);
                        break;
                    case Directions.WEST:
                        animator.DrawAnimation("West", screenPos, spriteBatch, CurrentBlinkFrame);
                        break;
                    case Directions.EAST:
                        animator.DrawAnimation("East", screenPos, spriteBatch, CurrentBlinkFrame);
                        break;
                    case Directions.NORTHEAST:
                        animator.DrawAnimation("NorthEast", screenPos, spriteBatch, CurrentBlinkFrame);
                        break;
                    case Directions.NORTHWEST:
                        animator.DrawAnimation("NorthWest", screenPos, spriteBatch, CurrentBlinkFrame);
                        break;
                    case Directions.SOUTHEAST:
                        animator.DrawAnimation("SouthEast", screenPos, spriteBatch, CurrentBlinkFrame);
                        break;
                    case Directions.SOUTHWEST:
                        animator.DrawAnimation("SouthWest", screenPos, spriteBatch, CurrentBlinkFrame);
                        break;
                }
            }

            Weapon.Draw(spriteBatch, effect);

            base.Draw(spriteBatch, effect);
        }
    }
}