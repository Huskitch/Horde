// <copyright file="EntityChest.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Entities.Base;
using HoardeGame.Entities.Drops;
using HoardeGame.Extensions;
using HoardeGame.Gameplay.Level;
using HoardeGame.Gameplay.Player;
using HoardeGame.Gameplay.Themes;
using HoardeGame.Gameplay.Weapons;
using HoardeGame.Graphics;
using HoardeGame.Input;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities.World
{
    /// <summary>
    /// Chest entity
    /// </summary>
    public class EntityChest : EntityBase
    {
        private readonly IResourceProvider resourceProvider;
        private readonly IPlayerProvider playerProvider;
        private readonly IInputProvider inputProvider;
        private readonly IWeaponProvider weaponProvider;
        private readonly GameServiceContainer serviceContainer;
        private readonly ChestInfo info;
        private readonly List<string> contentsList = new List<string>();
        private readonly AnimatedSprite animator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityChest"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        /// <param name="info"><see cref="ChestInfo"/> loot info</param>
        public EntityChest(DungeonLevel level, GameServiceContainer serviceContainer, ChestInfo info) : base(level, serviceContainer)
        {
            if (info == null)
            {
                throw new ArgumentException("Chest info cannot be null!", nameof(info));
            }

            this.info = info;
            this.serviceContainer = serviceContainer;

            resourceProvider = serviceContainer.GetService<IResourceProvider>();
            playerProvider = serviceContainer.GetService<IPlayerProvider>();
            inputProvider = serviceContainer.GetService<IInputProvider>();
            weaponProvider = serviceContainer.GetService<IWeaponProvider>();

            FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(25), ConvertUnits.ToSimUnits(25), 1f, Vector2.Zero, Body);
            Body.Position = level.GetSpawnPosition(2);
            Body.CollisionCategories = Category.Cat3;
            Body.CollidesWith = ~Category.Cat3;
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 70f;
            Body.FixedRotation = true;

            animator = new AnimatedSprite(resourceProvider.GetTexture("ChestSheet"), serviceContainer);
            animator.AddAnimation("Open", 32, 0, 2, 500);

            Health = 3;

            if (info.Gems.MaxDrop.RedGems > 0)
            {
                contentsList.Add("Red gems");
            }

            if (info.Gems.MaxDrop.GreenGems > 0)
            {
                contentsList.Add("Green gems");
            }

            if (info.Gems.MaxDrop.BlueGems > 0)
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

            if (info.Gems.MaxDrop.RedGems > 0)
            {
                int rubyDrop = info.Gems.GetRandomRedGems();

                for (int i = 0; i < rubyDrop; i++)
                {
                    EntityGem gem = new EntityGem(Level, serviceContainer)
                    {
                        Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.Vector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                    };

                    Level.AddEntity(gem);
                }
            }

            if (info.Gems.MaxDrop.GreenGems > 0)
            {
                int emeraldDrop = info.Gems.GetRandomGreenGems();

                for (int i = 0; i < emeraldDrop; i++)
                {
                    EntityGem2 gem = new EntityGem2(Level, serviceContainer)
                    {
                        Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.Vector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                    };

                    Level.AddEntity(gem);
                }
            }

            if (info.Gems.MaxDrop.BlueGems > 0)
            {
                int diamondDrop = info.Gems.GetRandomBlueGems();

                for (int i = 0; i < diamondDrop; i++)
                {
                    EntityGem3 gem = new EntityGem3(Level, serviceContainer)
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
                    EntityHealth health = new EntityHealth(Level, serviceContainer)
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
                    EntityArmour armour = new EntityArmour(Level, serviceContainer)
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
                    EntityAmmo ammo = new EntityAmmo(Level, serviceContainer)
                    {
                        Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.Vector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                    };

                    Level.AddEntity(ammo);
                }
            }

            if (info.WeaponDropType != ChestInfo.WeaponChance.None && random.Next(1, 101) < info.WeaponDropChance)
            {
                switch (info.WeaponDropType)
                {
                    case ChestInfo.WeaponChance.Random:
                        WeaponType type;
                        Enum.TryParse(info.WeaponType, out type);
                        EntityWeaponDrop weapon = new EntityWeaponDrop(Level, serviceContainer)
                        {
                            Weapon = weaponProvider.GetRandomWeapon(type),
                            Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.Vector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                        };
                        Level.AddEntity(weapon);
                        break;
                    case ChestInfo.WeaponChance.Specific:
                        EntityWeaponDrop specificWeapon = new EntityWeaponDrop(Level, serviceContainer)
                        {
                            Weapon = weaponProvider.GetWeapon(info.WeaponType),
                            Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.Vector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                        };
                        Level.AddEntity(specificWeapon);
                        break;
                }
            }
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (contentsList.Count > 0 && Vector2.Distance(playerProvider.Player.Position, Position) < 3 && inputProvider.KeybindPressed("Activate"))
            {
                if (playerProvider.Player.Gems.Keys > 0)
                {
                    playerProvider.Player.Gems.Keys--;
                    Open();
                }
                else
                {
                    playerProvider.Player.Gems.Keys = -1;
                }
            }

            animator.Update(gameTime);
            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(EffectParameter parameter)
        {
            int positionOnTheYAxisOfTheCoordinateSystem = Math.Max(contentsList.Count, 1) * 10 + 20;

            if (Vector2.Distance(playerProvider.Player.Position, Position) < 3)
            {
                animator.DrawAnimation("Open", ScreenPosition, Color.White);
                SpriteBatch.Draw(resourceProvider.GetTexture("OneByOneEmpty"), new Rectangle((int)ScreenPosition.X - 14, (int)ScreenPosition.Y - positionOnTheYAxisOfTheCoordinateSystem, 60, positionOnTheYAxisOfTheCoordinateSystem), new Color(0, 0, 5, 0.3f));
                SpriteBatch.DrawString(resourceProvider.GetFont("BigFont"), "Contents", new Vector2((int)ScreenPosition.X - 8, (int)ScreenPosition.Y - positionOnTheYAxisOfTheCoordinateSystem + 3), Color.White, 0f, Vector2.Zero, new Vector2(0.35f, 0.35f), SpriteEffects.None, 0f);

                if (contentsList.Count == 0)
                {
                    SpriteBatch.DrawString(resourceProvider.GetFont("BigFont"), "Empty", new Vector2((int)ScreenPosition.X - 8, (int)ScreenPosition.Y - positionOnTheYAxisOfTheCoordinateSystem + 19), Color.White, 0f, Vector2.Zero, new Vector2(0.3f, 0.3f), SpriteEffects.None, 0f);
                }
                else
                {
                    for (int i = 0; i < contentsList.Count; i++)
                    {
                        SpriteBatch.DrawString(resourceProvider.GetFont("BigFont"), contentsList[i], new Vector2((int)ScreenPosition.X - 8, (int)ScreenPosition.Y - positionOnTheYAxisOfTheCoordinateSystem + 19 + i * 10), Color.White, 0f, Vector2.Zero, new Vector2(0.3f, 0.3f), SpriteEffects.None, 0f);
                    }
                }
            }
            else
            {
                SpriteBatch.Draw(resourceProvider.GetTexture("Chest"), ScreenPosition, Color.White);
            }
        }
    }
}