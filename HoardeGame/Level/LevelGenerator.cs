using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoardeGame.Level
{
    public class LevelGenerator
    {
        Random rng = new Random();

        private int[,] map;
        private int mapWidth;
        private int mapHeight;
        private int wallPercent;

        public LevelGenerator()
        {

        }

        public void MakeCaverns()
        {
            for (int column = 0, row = 0; row <= mapHeight - 1; row++)
            {
                for (column = 0; column <= mapWidth - 1; column++)
                {
                    map[column, row] = PlaceWallLogic(column, row);
                }
            }
        }

        public int PlaceWallLogic(int x, int y)
        {
            int numWalls = GetAdjacentWalls(x, y, 1, 1);


            if (map[x, y] == 1)
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

        public int GetAdjacentWalls(int x, int y, int scopeX, int scopeY)
        {
            int startX = x - scopeX;
            int startY = y - scopeY;
            int endX = x + scopeX;
            int endY = y + scopeY;

            int iX = startX;
            int iY = startY;

            int wallCounter = 0;

            for (iY = startY; iY <= endY; iY++)
            {
                for (iX = startX; iX <= endX; iX++)
                {
                    if (!(iX == x && iY == y))
                    {
                        if (IsWall(iX, iY))
                        {
                            wallCounter += 1;
                        }
                    }
                }
            }
            return wallCounter;
        }

        bool IsWall(int x, int y)
        {
            if (IsOutOfBounds(x, y))
            {
                return true;
            }

            if (map[x, y] == 1)
            {
                return true;
            }

            if (map[x, y] == 0)
            {
                return false;
            }
            return false;
        }

        bool IsOutOfBounds(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return true;
            }
            else if (x > mapWidth - 1 || y > mapHeight - 1)
            {
                return true;
            }
            return false;
        }

        public void RandomFillMap()
        {
            map = new int[mapWidth, mapHeight];

            int mapMiddle = 0;
            for (int column = 0, row = 0; row < mapHeight; row++)
            {
                for (column = 0; column < mapWidth; column++)
                {
                    if (column == 0)
                    {
                        map[column, row] = 1;
                    }
                    else if (row == 0)
                    {
                        map[column, row] = 1;
                    }
                    else if (column == mapWidth - 1)
                    {
                        map[column, row] = 1;
                    }
                    else if (row == mapHeight - 1)
                    {
                        map[column, row] = 1;
                    }
                    else
                    {
                        mapMiddle = (mapHeight / 2);

                        if (row == mapMiddle)
                        {
                            map[column, row] = 0;
                        }
                        else
                        {
                            map[column, row] = RandomPercent(wallPercent);
                        }
                    }
                }
            }
        }

        int RandomPercent(int percent)
        {
            if (percent >= rng.Next(1, 101))
            {
                return 1;
            }
            return 0;
        }
    }
}
