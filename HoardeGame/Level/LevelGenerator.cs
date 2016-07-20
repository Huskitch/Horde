// <copyright file="LevelGenerator.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using FarseerPhysics;
using Microsoft.Xna.Framework;

namespace HoardeGame.Level
{
    /// <summary>
    /// Cavern level generator
    /// </summary>
    public class LevelGenerator
    {
        /// <summary>
        /// Gets or sets the level data
        /// </summary>
        public int[,] Map { get; set; }

        /// <summary>
        /// Gets or sets the width of the level
        /// </summary>
        public int MapWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the level
        /// </summary>
        public int MapHeight { get; set; }

        /// <summary>
        /// Gets or sets the percentile of walls in generated levels
        /// </summary>
        public int PercentAreWalls { get; set; }

        private readonly Random rand = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelGenerator"/> class.
        /// </summary>
        public LevelGenerator()
        {
            MapWidth = 64;
            MapHeight = 64;
            PercentAreWalls = 45;

            RandomFillMap();
        }

        /// <summary>
        /// Creates caverns on the level
        /// </summary>
        public void MakeCaverns()
        {
            for (int column = 0, row = 0; row <= MapHeight - 1; row++)
            {
                for (column = 0; column <= MapWidth - 1; column++)
                {
                    Map[column, row] = PlaceWallLogic(column, row);
                }
            }
        }

        /// <summary>
        /// Finds an epty 5x5 space for the player to spawn
        /// </summary>
        /// <returns>Spawn point for player</returns>
        public Vector2 GetSpawnPosition()
        {
            int[,] map = Map.Clone() as int[,];
            Point? nextWall = FindWall(ref map);

            while (nextWall != null)
            {
                nextWall = FindWall(ref map);
                FillInWall(ref map, nextWall);
            }

            List<Point> emptySpaces = FindEmptySpaces(ref map);
            Random rnd = new Random();

            Vector2 position = emptySpaces[rnd.Next(emptySpaces.Count)].ToVector2() * 32;

            return ConvertUnits.ToSimUnits(position);
        }

        /// <summary>
        /// Determines whether to place a wall on tile with coordinates [X, Y]
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Whether a wall should be placed</returns>
        private int PlaceWallLogic(int x, int y)
        {
            int numWalls = GetAdjacentWalls(x, y, 1, 1);

            if (Map[x, y] == 1)
            {
                if (numWalls >= 4)
                {
                    return 1;
                }

                if (numWalls < 2)
                {
                    return 0;
                }
            }
            else
            {
                if (numWalls >= 5)
                {
                    return 1;
                }
            }

            return 0;
        }

        /// <summary>
        /// Counts walls around a specified tile within a specified scope
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="scopeX">Width of the scope</param>
        /// <param name="scopeY">Height of the scope</param>
        /// <returns>Number of adjacent walls</returns>
        private int GetAdjacentWalls(int x, int y, int scopeX, int scopeY)
        {
            int startX = x - scopeX;
            int startY = y - scopeY;
            int endX = x + scopeX;
            int endY = y + scopeY;

            int wallCounter = 0;

            for (int searchY = startY; searchY <= endY; searchY++)
            {
                for (int searchX = startX; searchX <= endX; searchX++)
                {
                    if (!(searchX == x && searchY == y))
                    {
                        if (IsWall(searchX, searchY))
                        {
                            wallCounter += 1;
                        }
                    }
                }
            }

            return wallCounter;
        }

        /// <summary>
        /// Determines whether a tile is a wall or not
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Whether is a wall</returns>
        private bool IsWall(int x, int y)
        {
            return IsOutOfBounds(x, y) || Map[x, y] == 1;
        }

        /// <summary>
        /// Determines if a tile is outside of the level
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Whether is outside the level</returns>
        private bool IsOutOfBounds(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return true;
            }

            return x > MapWidth - 1 || y > MapHeight - 1;
        }

        /// <summary>
        /// Clears out the level
        /// </summary>
        private void BlankMap()
        {
            for (int row = 0; row < MapHeight; row++)
            {
                for (int column = 0; column < MapWidth; column++)
                {
                    Map[column, row] = 0;
                }
            }
        }

        /// <summary>
        /// Randomly fills in the map
        /// </summary>
        private void RandomFillMap()
        {
            Map = new int[MapWidth, MapHeight];

            for (int row = 0; row < MapHeight; row++)
            {
                for (int column = 0; column < MapWidth; column++)
                {
                    if (column == 0)
                    {
                        Map[column, row] = 1;
                    }
                    else if (row == 0)
                    {
                        Map[column, row] = 1;
                    }
                    else if (column == MapWidth - 1)
                    {
                        Map[column, row] = 1;
                    }
                    else if (row == MapHeight - 1)
                    {
                        Map[column, row] = 1;
                    }
                    else
                    {
                        var mapMiddle = MapHeight / 2;

                        if (row == mapMiddle)
                        {
                            Map[column, row] = 0;
                        }
                        else
                        {
                            Map[column, row] = RandomPercent(PercentAreWalls);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns 1 or 0 based on a percentile chance
        /// </summary>
        /// <param name="percent">Chance that this method will return 1</param>
        /// <returns>1 or 0</returns>
        private int RandomPercent(int percent)
        {
            if (percent >= rand.Next(1, 101))
            {
                return 1;
            }

            return 0;
        }

        private Point? FindWall(ref int[,] map)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] == 1)
                    {
                        return new Point(x, y);
                    }
                }
            }

            return null;
        }

        private void FillInWall(ref int[,] map, Point? wallPosition)
        {
            if (wallPosition == null)
            {
                return;
            }

            for (int x = wallPosition.Value.X - 2; x < wallPosition.Value.X + 2; x++)
            {
                for (int y = wallPosition.Value.Y - 2; y < wallPosition.Value.Y + 2; y++)
                {
                    if (IsOutOfBounds(x, y))
                    {
                        continue;
                    }

                    map[x, y] = 2;
                }
            }
        }

        /// <summary>
        /// Finds all empty space on a map
        /// </summary>
        /// <param name="map">Map to be used for seach</param>
        /// <returns><see cref="List{Point}"/> of all free space on provided map</returns>
        private List<Point> FindEmptySpaces(ref int[,] map)
        {
            List<Point> spaceList = new List<Point>();

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] == 0)
                    {
                        spaceList.Add(new Point(x, y));
                    }
                }
            }

            return spaceList;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelGenerator"/> class.
        /// </summary>
        /// <param name="mapWidth">Width of the level</param>
        /// <param name="mapHeight">Height of the level</param>
        /// <param name="map">Preexisting level data</param>
        /// <param name="percentWalls">Percent of walls</param>
        public LevelGenerator(int mapWidth, int mapHeight, int[,] map, int percentWalls = 40)
        {
            MapWidth = mapWidth;
            MapHeight = mapHeight;
            PercentAreWalls = percentWalls;
            Map = new int[MapWidth, MapHeight];
            Map = map;
        }
    }
}