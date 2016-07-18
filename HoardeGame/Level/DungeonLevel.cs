using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoardeGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Level
{
    public class DungeonLevel
    {
        private LevelGenerator levelGen;
        private List<Tile> mapTiles;

        private int[,] map;
        private int mapWidth = 64;
        private int mapHeight = 64;
        private int percWalls = 40;

        public DungeonLevel()
        {
            levelGen = new LevelGenerator();
            map = new int[mapWidth, mapHeight];
            mapTiles = new List<Tile>();
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

        public void LoadTiles()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (map[x, y] != 1)
                        mapTiles.Add(new Tile(new Vector2((x * 32), (y * 32) + 24), new Vector2(32, 32), ResourceManager.Texture("BasicFloor")));

                }
            }

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (map[x, y] == 1)
                        mapTiles.Add(new Tile(new Vector2((x * 32), (y * 32)), new Vector2(32, 56), ResourceManager.Texture("BasicWall")));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Tile tile in mapTiles)
            {
                tile.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in mapTiles)
            {
                tile.Draw(spriteBatch);
            }
        }
    }
}
