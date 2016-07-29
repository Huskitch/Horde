// <copyright file="DungeonLevel.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Entities.Base;
using HoardeGame.Entities.World;
using HoardeGame.Extensions;
using HoardeGame.Gameplay.Player;
using HoardeGame.Gameplay.Themes;
using HoardeGame.Graphics;
using HoardeGame.Input;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = HoardeGame.Graphics.IDrawable;

namespace HoardeGame.Gameplay.Level
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
        /// Gets or sets the body of the whole world
        /// </summary>
        public Body Body { get; set; }

        /// <summary>
        /// Gets the current theme of the level
        /// </summary>
        public Theme Theme { get; private set; }

        private readonly LevelGenerator levelGen;
        private readonly List<EntityBase> entities;
        private readonly IResourceProvider resourceProvider;
        private readonly IPlayerProvider playerProvider;
        private readonly IInputProvider inputProvider;
        private readonly List<IDrawable> renderList = new List<IDrawable>();

        private int[,] map;
        private int mapWidth = 1;
        private int mapHeight = 1;
        private int percWalls = 40;

        /// <summary>
        /// Initializes a new instance of the <see cref="DungeonLevel"/> class.
        /// </summary>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        /// <param name="playerProvider"><see cref="IPlayerProvider"/> for accessing the player entity</param>
        /// <param name="inputProvider"><see cref="IInputProvider"/> for providing input to <see cref="EntityChest"/></param>
        /// <param name="theme"><see cref="Theme"/> of this level</param>
        /// <param name="generateEntities">Whether to generate default entites (enemies and chests)</param>
        public DungeonLevel(IResourceProvider resourceProvider, IPlayerProvider playerProvider, IInputProvider inputProvider, Theme theme, bool generateEntities = true)
        {
            if (theme == null)
            {
                throw new ArgumentNullException(nameof(theme));
            }

            this.resourceProvider = resourceProvider;
            this.playerProvider = playerProvider;
            this.inputProvider = inputProvider;
            Theme = theme;

            Body = BodyFactory.CreateBody(World);
            Body.CollisionCategories = Category.Cat4;
            Body.CollidesWith = Category.All;
            Body.IsStatic = true;

            levelGen = new LevelGenerator();
            map = new int[mapWidth, mapHeight];
            MapTiles = new List<Tile>();
            entities = new List<EntityBase>();

            if (generateEntities)
            {
                PlaceChests();
                SpawnEnemies();
            }
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
        /// Gets the raw map search data from <see cref="LevelGenerator"/>
        /// </summary>
        /// <returns>Raw map search data</returns>
        public int[,] GetSearchMap()
        {
            return levelGen.SearchMap;
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

        /// <summary>
        /// Places chests
        /// </summary>
        public void PlaceChests()
        {
            for (int i = 0; i < 5; i++)
            {
                EntityChest chest = new EntityChest(this, resourceProvider, playerProvider, inputProvider, Theme.ChestInfo);
                AddEntity(chest);
            }
        }

        /// <summary>
        /// Spawns enemies
        /// </summary>
        public void SpawnEnemies()
        {
            Random rng = new Random();

            foreach (EntitySpawnInfo spawns in Theme.EntitySpawns)
            {
                for (int i = 0; i < spawns.SpawnRate; i++)
                {
                    int clusterSize = rng.Next(spawns.MinClusterSize, spawns.MaxClusterSize);

                    Vector2 spawnPos = GetSpawnPosition(3);
                    Random random = new Random();

                    for (int j = 0; j < clusterSize; j++)
                    {
                        Type enemyType = Type.GetType(spawns.EntityType);
                        var instance = Activator.CreateInstance(enemyType, this, resourceProvider, playerProvider) as EntityBaseEnemy;
                        Vector2 randomVector2 = random.Vector2(0, 0, 0.25f, 0.25f);
                        instance.Body.Position = spawnPos + randomVector2;
                        World.Step(10);
                        Console.WriteLine($"{j + 1} / {clusterSize} ({spawnPos})");

                        AddEntity(instance);
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

            for (int i = 0; i < 10; i++)
            {
                levelGen.MakeCaverns();
            }

            map = levelGen.Map;

            LoadTiles();

            int maxIters = 300;

            while (World.ContactList.Count > 1000 && maxIters > 0)
            {
                World.Step(10);
                Console.WriteLine($"Contacts: {World.ContactList.Count} / 1000");
                maxIters--;
            }
        }

        /// <summary>
        /// Loads an existing<see cref="DungeonLevel"/>
        /// </summary>
        /// <param name="width">Width of the <see cref="DungeonLevel"/></param>
        /// <param name="height">Height of the <see cref="DungeonLevel"/></param>
        /// <param name="map">Map data</param>
        public void LoadLevel(int width, int height, int[,] map)
        {
            this.map = map;

            mapWidth = width;
            mapHeight = height;

            LoadTiles();
        }

        /// <summary>
        /// Adds a new prespawned entity
        /// </summary>
        /// <param name="entity">Entity to be added</param>
        public void AddEntity(EntityBase entity)
        {
            entity.Start();
            renderList.Add(entity);
            entities.Add(entity);
        }

        /// <summary>
        /// Removes an <see cref="EntityBase"/> from this <see cref="DungeonLevel"/>
        /// </summary>
        /// <param name="entity"><see cref="EntityBase"/> to remove</param>
        public void RemoveEntity(EntityBase entity)
        {
            renderList.Remove(entity);
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

            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update(gameTime);
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
            Effect effect = resourceProvider.GetEffect("SpriteEffect");
            EffectParameter parameter = effect.Parameters["Blink"];

            renderList.Sort((a, b) => a.Depth.CompareTo(b.Depth));

            using (spriteBatch.Use(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, effect, camera.Transformation(graphicsDevice)))
            {
                renderList.ForEach(drawable =>
                {
                    drawable.Draw(spriteBatch, parameter);
                });
            }
        }

        /// <summary>
        /// Loads tiles from the <see cref="LevelGenerator"/> and instantiates a <see cref="Tile"/> for each block
        /// </summary>
        private void LoadTiles()
        {
            int[,] edgedMap = GenerateEdges();

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    // Empty
                    if (edgedMap[x, y] == 0)
                    {
                        Tile tile = new Tile(new Vector2(x * 32, y * 32 + 24), new Vector2(32, 32), resourceProvider.GetTexture(Theme.FloorTextureName), false, this, 0);
                        MapTiles.Add(tile);
                        renderList.Add(tile);
                    }

                    // Wall
                    if (edgedMap[x, y] >= 1)
                    {
                        Tile tile = new Tile(new Vector2(x * 32, y * 32), new Vector2(32, 56), resourceProvider.GetTexture(Theme.WallTextureName), true, this, edgedMap[x, y] - 1);
                        MapTiles.Add(tile);
                        renderList.Add(tile);
                    }
                }
            }
        }

        private int[,] GenerateEdges()
        {
            int[,] edgedMap = new int[mapWidth, mapHeight];

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (map[x, y] == 0)
                    {
                        continue;
                    }

                    if (TryGet(x, y - 1) == 1 && TryGet(x, y + 1) == 1 && TryGet(x + 1, y) == 1 && TryGet(x - 1, y) == 1)
                    {
                        edgedMap[x, y] = 1;
                    }
                    else if (TryGet(x, y - 1) == 0 && TryGet(x, y + 1) == 0 && TryGet(x + 1, y) == 0 && TryGet(x - 1, y) == 0)
                    {
                        edgedMap[x, y] = 16;
                    }
                    else if (TryGet(x, y - 1) == 1 && TryGet(x - 1, y) == 1 && TryGet(x + 1, y) == 1 && TryGet(x - 1, y - 1) == 1 && TryGet(x + 1, y - 1) == 1)
                    {
                        edgedMap[x, y] = 17;
                    }
                    else if (TryGet(x, y + 1) == 1 && TryGet(x - 1, y) == 1 && TryGet(x + 1, y) == 1 && TryGet(x - 1, y + 1) == 1 && TryGet(x + 1, y + 1) == 1)
                    {
                        edgedMap[x, y] = 18;
                    }
                    else if (TryGet(x, y + 1) == 1 && TryGet(x, y - 1) == 1 && TryGet(x + 1, y) == 1 && TryGet(x + 1, y + 1) == 1 && TryGet(x + 1, y - 1) == 1)
                    {
                        edgedMap[x, y] = 19;
                    }
                    else if (TryGet(x, y + 1) == 1 && TryGet(x, y - 1) == 1 && TryGet(x - 1, y) == 1 && TryGet(x - 1, y + 1) == 1 && TryGet(x - 1, y - 1) == 1)
                    {
                        edgedMap[x, y] = 20;
                    }
                    else if (TryGet(x, y - 1) == 1 && TryGet(x - 1, y) == 1 && TryGet(x + 1, y) == 1 && TryGet(x + 1, y - 1) == 1)
                    {
                        edgedMap[x, y] = 21;
                    }
                    else if (TryGet(x, y + 1) == 1 && TryGet(x - 1, y) == 1 && TryGet(x + 1, y) == 1 && TryGet(x + 1, y + 1) == 1)
                    {
                        edgedMap[x, y] = 22;
                    }
                    else if (TryGet(x, y + 1) == 1 && TryGet(x, y - 1) == 1 && TryGet(x + 1, y) == 1 && TryGet(x + 1, y + 1) == 1)
                    {
                        edgedMap[x, y] = 23;
                    }
                    else if (TryGet(x, y + 1) == 1 && TryGet(x, y - 1) == 1 && TryGet(x - 1, y) == 1 && TryGet(x - 1, y + 1) == 1)
                    {
                        edgedMap[x, y] = 24;
                    }
                    else if (TryGet(x, y - 1) == 1 && TryGet(x - 1, y) == 1 && TryGet(x + 1, y) == 1 && TryGet(x - 1, y - 1) == 1)
                    {
                        edgedMap[x, y] = 25;
                    }
                    else if (TryGet(x, y + 1) == 1 && TryGet(x - 1, y) == 1 && TryGet(x + 1, y) == 1 && TryGet(x - 1, y + 1) == 1)
                    {
                        edgedMap[x, y] = 26;
                    }
                    else if (TryGet(x, y + 1) == 1 && TryGet(x, y - 1) == 1 && TryGet(x + 1, y) == 1 && TryGet(x + 1, y - 1) == 1)
                    {
                        edgedMap[x, y] = 27;
                    }
                    else if (TryGet(x, y + 1) == 1 && TryGet(x, y - 1) == 1 && TryGet(x - 1, y) == 1 && TryGet(x - 1, y - 1) == 1)
                    {
                        edgedMap[x, y] = 28;
                    }
                    else if (TryGet(x, y - 1) == 1 && TryGet(x - 1, y) == 1 && TryGet(x + 1, y) == 1)
                    {
                        edgedMap[x, y] = 5;
                    }
                    else if (TryGet(x, y + 1) == 1 && TryGet(x - 1, y) == 1 && TryGet(x + 1, y) == 1)
                    {
                        edgedMap[x, y] = 6;
                    }
                    else if (TryGet(x, y + 1) == 1 && TryGet(x + 1, y) == 1 && TryGet(x, y - 1) == 1)
                    {
                        edgedMap[x, y] = 7;
                    }
                    else if (TryGet(x, y + 1) == 1 && TryGet(x - 1, y) == 1 && TryGet(x, y - 1) == 1)
                    {
                        edgedMap[x, y] = 10;
                    }
                    else if (TryGet(x, y - 1) == 1 && TryGet(x, y + 1) == 1)
                    {
                        edgedMap[x, y] = 2;
                    }
                    else if (TryGet(x + 1, y) == 1 && TryGet(x - 1, y) == 1)
                    {
                        edgedMap[x, y] = 3;
                    }
                    else if (TryGet(x, y - 1) == 1 && TryGet(x + 1, y) == 1)
                    {
                        edgedMap[x, y] = 4;
                    }
                    else if (TryGet(x, y + 1) == 1 && TryGet(x - 1, y) == 1)
                    {
                        edgedMap[x, y] = 9;
                    }
                    else if (TryGet(x, y + 1) == 1 && TryGet(x + 1, y) == 1)
                    {
                        edgedMap[x, y] = 8;
                    }
                    else if (TryGet(x, y - 1) == 1 && TryGet(x - 1, y) == 1)
                    {
                        edgedMap[x, y] = 11;
                    }
                    else if (TryGet(x + 1, y) == 1)
                    {
                        edgedMap[x, y] = 14;
                    }
                    else if (TryGet(x - 1, y) == 1)
                    {
                        edgedMap[x, y] = 13;
                    }
                    else if (TryGet(x, y + 1) == 1)
                    {
                        edgedMap[x, y] = 15;
                    }
                    else if (TryGet(x, y - 1) == 1)
                    {
                        edgedMap[x, y] = 12;
                    }
                }
            }

            return edgedMap;
        }

        private bool IsOutOfBounds(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return true;
            }

            return x > mapWidth - 1 || y > mapHeight - 1;
        }

        private int TryGet(int x, int y)
        {
            if (IsOutOfBounds(x, y))
            {
                return 0;
            }

            return map[x, y];
        }

        private bool[] GetEdges(int x, int y)
        {
            bool[] edges = new bool[9];

            for (int xpos = x - 1; xpos < x + 1; xpos++)
            {
                for (int ypos = y - 1; ypos < y + 1; ypos++)
                {
                    edges[ypos * 3 + xpos] = TryGet(xpos, ypos) == 0;
                }
            }

            return edges;
        }
    }
}