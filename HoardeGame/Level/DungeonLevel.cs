// <copyright file="DungeonLevel.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using HoardeGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        private readonly LevelGenerator levelGen;
        private readonly List<EntityBase> entities;
        private readonly World world = new World(Vector2.Zero);

        private int[,] map;
        private int mapWidth = 1;
        private int mapHeight = 1;
        private int percWalls = 40;

        /// <summary>
        /// Initializes a new instance of the <see cref="DungeonLevel"/> class.
        /// </summary>
        public DungeonLevel()
        {
            levelGen = new LevelGenerator();
            map = new int[mapWidth, mapHeight];
            MapTiles = new List<Tile>();
            entities = new List<EntityBase>();
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
        /// Adds an entiy of the type T
        /// </summary>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <returns>Entity that can be catsed to T</returns>
        public EntityBase AddEntity<T>()
        {
            EntityBase entity = (EntityBase)Activator.CreateInstance(typeof(T), world);
            entity.Level = this;
            entities.Add(entity);

            return entity;
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
            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, 1f / 30f));

            foreach (Tile tile in MapTiles)
            {
                tile.Update(gameTime);
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
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in MapTiles)
            {
                tile.Draw(spriteBatch);
            }

            foreach (EntityBase entity in entities)
            {
                entity.Draw(spriteBatch);
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
                        MapTiles.Add(new Tile(new Vector2(x * 32, y * 32 + 24), new Vector2(32, 32), ResourceManager.GetTexture("BasicFloor"), false, world));
                    }
                }
            }

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (map[x, y] == 1)
                    {
                        MapTiles.Add(new Tile(new Vector2(x * 32, y * 32), new Vector2(32, 56), ResourceManager.GetTexture("BasicWall"), true, world));
                    }
                }
            }
        }
    }
}