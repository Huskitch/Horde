// <copyright file="EntityChest.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Extensions;
using HoardeGame.Gameplay;
using HoardeGame.Graphics.Rendering;
using HoardeGame.Input;
using HoardeGame.Level;
using HoardeGame.Resources;
using HoardeGame.Themes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Chest entity
    /// </summary>
    public class EntityChest : EntityBase
    {
        private readonly IResourceProvider resourceProvider;
        private readonly IPlayerProvider playerProvider;
        private readonly IInputProvider inputProvider;
        private readonly ChestInfo info;
        private readonly List<string> contentsList = new List<string>();
        private readonly AnimatedSprite animator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityChest"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to use for loading resources</param>
        /// <param name="playerProvider"><see cref="IPlayerProvider"/> to use for acessing the player entity</param>
        /// <param name="inputProvider"><see cref="IInputProvider"/> for chest interactions</param>
        /// <param name="info"><see cref="ChestInfo"/> loot info</param>
        public EntityChest(DungeonLevel level, IResourceProvider resourceProvider, IPlayerProvider playerProvider, IInputProvider inputProvider, ChestInfo info) : base(level)
        {
            if (info == null)
            {
                throw new ArgumentException("Chest info cannot be null!", nameof(info));
            }

            this.resourceProvider = resourceProvider;
            this.playerProvider = playerProvider;
            this.inputProvider = inputProvider;
            this.info = info;

            FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(25), ConvertUnits.ToSimUnits(25), 1f, Vector2.Zero, Body);
            Body.Position = level.GetSpawnPosition(2);
            Body.CollisionCategories = Category.Cat3;
            Body.CollidesWith = ~Category.Cat3;
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 70f;
            Body.FixedRotation = true;

            animator = new AnimatedSprite(resourceProvider.GetTexture("ChestSheet"));
            animator.AddAnimation("Open", 32, 0, 2, 500);

            Health = 3;

            if (info.MaxGems[0] > 0)
            {
                contentsList.Add("Red gems");
            }

            if (info.MaxGems[1] > 0)
            {
                contentsList.Add("Green gems");
            }

            if (info.MaxGems[2] > 0)
            {
                contentsList.Add("Blue gems");
            }

            if (info.MaxAmmo > 0)
            {
                contentsList.Add("Ammo");
            }

            if (info.MaxArmour > 0)
            {
                contentsList.Add("Armour");
            }

            if (info.MaxHealth > 0)
            {
                contentsList.Add("Health");
            }

            if (info.WeaponDropType != ChestInfo.WeaponChance.None && info.WeaponDropChance > 0)
            {
                contentsList.Add("A weapon");
            }
        }

        /// <summary>
        /// Opens the chest and throws the contents on the ground
        /// </summary>
        public void Open()
        {
            contentsList.Clear();

            Random random = new Random();

            if (info.MaxGems[0] > 0)
            {
                int rubyDrop = random.Next(info.MinGems[0], info.MaxGems[0] + 1);

                for (int i = 0; i < rubyDrop; i++)
                {
                    EntityGem gem = new EntityGem(Level, resourceProvider, playerProvider)
                    {
                        Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.Vector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                    };

                    Level.AddEntity(gem);
                }
            }

            if (info.MaxGems[1] > 0)
            {
                int emeraldDrop = random.Next(info.MinGems[1], info.MaxGems[1] + 1);

                for (int i = 0; i < emeraldDrop; i++)
                {
                    EntityGem2 gem = new EntityGem2(Level, resourceProvider, playerProvider)
                    {
                        Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.Vector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                    };

                    Level.AddEntity(gem);
                }
            }

            if (info.MaxGems[2] > 0)
            {
                int diamondDrop = random.Next(info.MinGems[2], info.MaxGems[2] + 1);

                for (int i = 0; i < diamondDrop; i++)
                {
                    EntityGem3 gem = new EntityGem3(Level, resourceProvider, playerProvider)
                    {
                        Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.Vector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                    };

                    Level.AddEntity(gem);
                }
            }

            if (info.MaxHealth > 0)
            {
                int healthDrop = random.Next(info.MinHealth, info.MaxHealth + 1);

                for (int i = 0; i < healthDrop; i++)
                {
                    EntityHealth health = new EntityHealth(Level, resourceProvider, playerProvider)
                    {
                        Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.Vector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                    };

                    Level.AddEntity(health);
                }
            }

            if (info.MaxArmour > 0)
            {
                int armourDrop = random.Next(info.MinArmour, info.MaxArmour + 1);

                for (int i = 0; i < armourDrop; i++)
                {
                    EntityArmour armour = new EntityArmour(Level, resourceProvider, playerProvider)
                    {
                        Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.Vector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                    };

                    Level.AddEntity(armour);
                }
            }

            if (info.MaxAmmo > 0)
            {
                int ammoDrop = random.Next(info.MinAmmo / 10, info.MaxAmmo / 10 + 1);

                for (int i = 0; i < ammoDrop; i++)
                {
                    EntityAmmo ammo = new EntityAmmo(Level, resourceProvider, playerProvider)
                    {
                        Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.Vector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                    };

                    Level.AddEntity(ammo);
                }
            }

            // TODO: IMPLEMENT WEAPON DROPS
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (contentsList.Count > 0 && Vector2.Distance(playerProvider.Player.Position, Position) < 3 && inputProvider.Activate)
            {
                if (playerProvider.Player.ChestKeys > 0)
                {
                    playerProvider.Player.ChestKeys--;
                    Open();
                }
                else
                {
                    playerProvider.Player.ChestKeys = -1;
                }
            }

            animator.Update(gameTime);
            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, EffectParameter parameter)
        {
            int positionOnTheYAxisOfTheCoordinateSystem = Math.Max(contentsList.Count, 1) * 10 + 20;

            if (Vector2.Distance(playerProvider.Player.Position, Position) < 3)
            {
                animator.DrawAnimation("Open", ScreenPosition, spriteBatch, Color.White);
                spriteBatch.Draw(resourceProvider.GetTexture("OneByOneEmpty"), new Rectangle((int)ScreenPosition.X - 14, (int)ScreenPosition.Y - positionOnTheYAxisOfTheCoordinateSystem, 60, positionOnTheYAxisOfTheCoordinateSystem), new Color(0, 0, 5, 0.3f));
                spriteBatch.DrawString(resourceProvider.GetFont("BigFont"), "Contents", new Vector2((int)ScreenPosition.X - 8, (int)ScreenPosition.Y - positionOnTheYAxisOfTheCoordinateSystem + 3), Color.White, 0f, Vector2.Zero, new Vector2(0.35f, 0.35f), SpriteEffects.None, 0f);

                if (contentsList.Count == 0)
                {
                    spriteBatch.DrawString(resourceProvider.GetFont("BigFont"), "Empty", new Vector2((int)ScreenPosition.X - 8, (int)ScreenPosition.Y - positionOnTheYAxisOfTheCoordinateSystem + 19), Color.White, 0f, Vector2.Zero, new Vector2(0.3f, 0.3f), SpriteEffects.None, 0f);
                }
                else
                {
                    for (int i = 0; i < contentsList.Count; i++)
                    {
                        spriteBatch.DrawString(resourceProvider.GetFont("BigFont"), contentsList[i], new Vector2((int)ScreenPosition.X - 8, (int)ScreenPosition.Y - positionOnTheYAxisOfTheCoordinateSystem + 19 + i * 10), Color.White, 0f, Vector2.Zero, new Vector2(0.3f, 0.3f), SpriteEffects.None, 0f);
                    }
                }
            }
            else
            {
                spriteBatch.Draw(resourceProvider.GetTexture("Chest"), ScreenPosition, Color.White);
            }
        }
    }
}