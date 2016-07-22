﻿// <copyright file="DungeonLevel.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using HoardeGame.Entities;
using HoardeGame.Extensions;
using HoardeGame.Graphics.Rendering;
using HoardeGame.Resources;
using HoardeGame.Themes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace HoardeGame.Level
{
    /// <summary>
    /// Representation of a dungeon
    /// </summary>
    public class DungeonLevel
    {
        /// <summary>
        /// Gets the <see cref="List{Tile}"/> containing all the <see cref="Tile"/> in this <see cref="DungeonLevel"/>
        /// </summary>
        public List<Tile> MapTiles { get; private set; }

        /// <summary>
        /// Gets the physics world for all <see cref="EntityBase"/>
        /// </summary>
        public World World { get; private set; } = new World(Vector2.Zero);

        /// <summary>
        /// Gets the current theme of the level
        /// </summary>
        public Theme Theme { get; private set; }

        private readonly LevelGenerator levelGen;
        private readonly List<EntityBase> entities;
        private readonly IResourceProvider resourceProvider;
        private IPlayerProvider playerProvider;

        private int[,] map;
        private int mapWidth = 1;
        private int mapHeight = 1;
        private int percWalls = 40;

        /// <summary>
        /// Initializes a new instance of the <see cref="DungeonLevel"/> class.
        /// </summary>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        /// <param name="theme"><see cref="Theme"/> of this level</param>
        public DungeonLevel(IResourceProvider resourceProvider, IPlayerProvider playerProvider, Theme theme)
        {
            if (theme == null)
            {
                throw new ArgumentNullException(nameof(theme));
            }

            this.resourceProvider = resourceProvider;
            this.playerProvider = playerProvider;
            Theme = theme;

            levelGen = new LevelGenerator();
            map = new int[mapWidth, mapHeight];
            MapTiles = new List<Tile>();
            entities = new List<EntityBase>();
            PlaceChests();
            SpawnEnemies();
        }

        /// <summary>
        /// Finds an entity with a matching <see cref="Body"/>
        /// </summary>
        /// <param name="body"><see cref="Body"/> to search for</param>
        /// <returns>Entity or null</returns>
        public EntityBase FindEntityByBody(Body body)
        {
            foreach (var entity in entities)
            {
                if (entity.Body == body)
                {
                    return entity;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the raw map data from <see cref="LevelGenerator"/>
        /// </summary>
        /// <returns>Raw map data</returns>
        public int[,] GetMap()
        {
            return map;
        }

        /// <summary>
        /// Finds an epty 5x5 space for the player to spawn
        /// </summary>
        /// <param name="size">Size of the spawn area in tiles</param>
        /// <param name="center">Whether to return the center of the area</param>
        /// <returns>Spawn point for player</returns>
        public Vector2 GetSpawnPosition(int size = 5, bool center = true)
        {
            return levelGen.GetSpawnPosition(size, center);
        }

        public void PlaceChests()
        {
            for (int i = 0; i < 5; i++)
            {
                EntityChest chest = new EntityChest(this, resourceProvider, playerProvider);
                chest.Body.Position = GetSpawnPosition(2, true);
                entities.Add(chest);
            }
        }

        public void SpawnEnemies()
        {
            Random rng = new Random();

            foreach (EnemySpawnInfo spawns in Theme.EntitySpawns)
            {
                for (int i = 0; i < spawns.SpawnRate; i++)
                {
                    List<EntityBaseEnemy> enemies = new List<EntityBaseEnemy>();
                    int clusterSize = rng.Next(spawns.MinClusterSize, spawns.MaxClusterSize);

                    for (int j = 0; j < clusterSize; j++)
                    {
                        Type enemyType = Type.GetType(spawns.EnemyType);
                        var instance = Activator.CreateInstance(enemyType, this, resourceProvider, playerProvider);
                        enemies.Add((EntityBaseEnemy) instance);
                    }

                    Vector2 spawnPos = GetSpawnPosition(3);
                    for (int j = 0; j < enemies.Count; j++)
                    {
                        EntityBaseEnemy enemy = enemies[j];

                        enemy.Body.Position = (spawnPos + new Vector2(j%5 * 0.5f, j/5 * 0.5f));

                        AddEntity(enemy);
                    }
                }
            }
        }

        /// <summary>
        /// Generates a <see cref="DungeonLevel"/>
        /// </summary>
        /// <param name="width">Width of the <see cref="DungeonLevel"/></param>
        /// <param name="height">Height of the <see cref="DungeonLevel"/></param>
        /// <param name="percentWalls">Percent of walls in the <see cref="DungeonLevel"/></param>
        public void GenerateLevel(int width, int height, int percentWalls)
        {
            mapWidth = width;
            mapHeight = height;
            percWalls = percentWalls;
            map = new int[mapWidth, mapHeight];

            levelGen.MapWidth = mapWidth;
            levelGen.MapHeight = mapHeight;
            levelGen.PercentAreWalls = percWalls;
            levelGen.MakeCaverns();
            map = levelGen.Map;

            LoadTiles();
        }

        /// <summary>
        /// Adds a new prespawned entity
        /// </summary>
        /// <param name="entity">Entity to be added</param>
        public void AddEntity(EntityBase entity)
        {
            entity.Start();
            entities.Add(entity);
        }

        /// <summary>
        /// Removes an <see cref="EntityBase"/> from this <see cref="DungeonLevel"/>
        /// </summary>
        /// <param name="entity"><see cref="EntityBase"/> to remove</param>
        public void RemoveEntity(EntityBase entity)
        {
            entities.Remove(entity);
        }

        /// <summary>
        /// Gets the tile at a specified world (in meters) position
        /// </summary>
        /// <param name="position"><see cref="Vector2"/> position of the tile</param>
        /// <returns>Tile at the postition or null</returns>
        public Tile GetTile(Vector2 position)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    var tile = MapTiles[x + y * mapWidth];
                    if (new Rectangle((int)tile.Position.X, (int)tile.Position.Y, (int)tile.Scale.X, (int)tile.Scale.Y).Contains(position))
                    {
                        return tile;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Updates the <see cref="DungeonLevel"/> tiles, updates all <see cref="EntityBase"/> in it and steps the physics simulation
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public void Update(GameTime gameTime)
        {
            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, 1f / 30f));

            foreach (Tile tile in MapTiles)
            {
                tile.Update(gameTime);
            }

            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].Removed)
                {
                    entities[i].Body.Dispose();
                    RemoveEntity(entities[i]);
                }
            }

            foreach (EntityBase entity in entities)
            {
                entity.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws the <see cref="DungeonLevel"/> and all <see cref="EntityBase"/> in it
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> to draw the <see cref="DungeonLevel"/> with</param>
        /// <param name="camera"><see cref="Camera"/></param>
        /// <param name="graphicsDevice"><see cref="GraphicsDevice"/></param>
        public void Draw(SpriteBatch spriteBatch, Camera camera, GraphicsDevice graphicsDevice)
        {
            using (spriteBatch.Use(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, camera.Transformation(graphicsDevice)))
            {
                foreach (Tile tile in MapTiles)
                {
                    tile.Draw(spriteBatch);
                }
            }

            Effect effect = resourceProvider.GetEffect("SpriteEffect");
            using (spriteBatch.Use(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, effect, camera.Transformation(graphicsDevice)))
            {
                foreach (EntityBase entity in entities)
                {
                    entity.Draw(spriteBatch, effect);
                }
            }
        }

        /// <summary>
        /// Loads tiles from the <see cref="LevelGenerator"/> and instantiates a <see cref="Tile"/> for each block
        /// </summary>
        private void LoadTiles()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (map[x, y] != 1)
                    {
                        MapTiles.Add(new Tile(new Vector2(x * 32, y * 32 + 24), new Vector2(32, 32), resourceProvider.GetTexture(Theme.FloorTextureName), false, World));
                    }
                }
            }

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (map[x, y] == 1)
                    {
                        MapTiles.Add(new Tile(new Vector2(x * 32, y * 32), new Vector2(32, 56), resourceProvider.GetTexture(Theme.WallTextureName), true, World));
                    }
                }
            }
        }
    }
}