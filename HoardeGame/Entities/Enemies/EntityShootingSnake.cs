﻿// <copyright file="EntityShootingSnake.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Entities.Base;
using HoardeGame.Entities.Misc;
using HoardeGame.Gameplay.Gems;
using HoardeGame.Gameplay.Level;
using HoardeGame.Gameplay.Player;
using HoardeGame.Gameplay.Weapons;
using HoardeGame.Graphics;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities.Enemies
{
    /// <summary>
    /// Snake entity
    /// </summary>
    public class EntityShootingSnake : EntityBaseShootingEnemy
    {
        private readonly AnimatedSprite animator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityShootingSnake"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to load resources with</param>
        /// <param name="playerProvider"><see cref="IPlayerProvider"/> for acessing the player entity</param>
        /// <param name="weaponProvider"><see cref="IWeaponProvider"/> for loading weapons</param>
        public EntityShootingSnake(DungeonLevel level, IResourceProvider resourceProvider, IPlayerProvider playerProvider, IWeaponProvider weaponProvider) : base(level, resourceProvider, playerProvider)
        {
            FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(14f), 1f, Body);
            Body.CollisionCategories = Category.Cat3;
            Body.CollidesWith = Category.All;
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;
            Health = 5;

            GemDrop = new GemDropInfo
            {
                MinDrop =
                {
                    RedGems = 3,
                    GreenGems = 2,
                    BlueGems = 1,
                    Keys = 1
                },
                MaxDrop =
                {
                    RedGems = 6,
                    GreenGems = 4,
                    BlueGems = 2,
                    Keys = 1
                }
            };

            Weapon = new EntityWeapon(Level, resourceProvider, null, this, weaponProvider.GetWeapon("snakeWeapon"));

            animator = new AnimatedSprite(resourceProvider.GetTexture("SnakeSheet"));
            animator.AddAnimation("North", 48, 1, 3, 100);
            animator.AddAnimation("East", 48, 2, 3, 100);
            animator.AddAnimation("South", 48, 3, 3, 100);
            animator.AddAnimation("West", 48, 0, 3, 100);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            UpdateAI(gameTime);
            animator.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, EffectParameter parameter)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);

            if (Direction == new Vector2(0, -1))
            {
                animator.DrawAnimation("North", screenPos, spriteBatch, Color.White, parameter, CurrentBlinkFrame);
            }
            else if (Direction == new Vector2(1, 0))
            {
                animator.DrawAnimation("East", screenPos, spriteBatch, Color.White, parameter, CurrentBlinkFrame);
            }
            else if (Direction == new Vector2(0, 1))
            {
                animator.DrawAnimation("South", screenPos, spriteBatch, Color.White, parameter, CurrentBlinkFrame);
            }
            else if (Direction == new Vector2(-1, 0))
            {
                animator.DrawAnimation("West", screenPos, spriteBatch, Color.White, parameter, CurrentBlinkFrame);
            }
            else
            {
                animator.DrawAnimation("South", screenPos, spriteBatch, Color.White, parameter, CurrentBlinkFrame);
            }

            base.Draw(spriteBatch, parameter);
        }
    }
}