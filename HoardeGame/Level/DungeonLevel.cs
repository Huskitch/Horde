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
    public class DungeonLevel
    {
        private LevelGenerator levelGen;
        public List<Tile> MapTiles { get; private set; }
        private List<EntityBase> _entities;
        private World _world = new World(Vector2.Zero);

        private int[,] map;
        private int mapWidth = 1;
        private int mapHeight = 1;
        private int percWalls = 40;

        public DungeonLevel()
        {
            levelGen = new LevelGenerator();
            map = new int[mapWidth, mapHeight];
            MapTiles = new List<Tile>();
            _entities = new List<EntityBase>();
        }

        public int[,] GetMap()
        {
            return map;
        }

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

        internal EntityBase AddEntity<T>()
        {
            EntityBase entity = (EntityBase) Activator.CreateInstance(typeof(T), _world);
            entity.Level = this;
            _entities.Add(entity);

            return entity;
        }

        public void RemoveEntity(EntityBase entity)
        {
            _entities.Remove(entity);
        }

        public Tile GetTile(Vector2 position)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    var tile = MapTiles[x + y * mapWidth];
                    if (new Rectangle((int) tile.Position.X, (int) tile.Position.Y, (int) tile.Scale.X, (int) tile.Scale.Y).Contains(position)) return tile;
                }
            }

            return null;
        }

        public void LoadTiles()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (map[x, y] != 1)
                        MapTiles.Add(new Tile(new Vector2((x * 32), (y * 32) + 24), new Vector2(32, 32), ResourceManager.Texture("BasicFloor"), false, _world));

                }
            }

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (map[x, y] == 1)
                        MapTiles.Add(new Tile(new Vector2((x * 32), (y * 32)), new Vector2(32, 56), ResourceManager.Texture("BasicWall"), true, _world));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            _world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));

            foreach (Tile tile in MapTiles)
            {
                tile.Update(gameTime);
            }

            foreach (EntityBase entity in _entities)
            {
                entity.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in MapTiles)
            {
                tile.Draw(spriteBatch);
            }

            foreach (EntityBase entity in _entities)
            {
                entity.Draw(spriteBatch);
            }
        }
    }
}