using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Level
{
    static class Minimap
    {
        public static Color FloorColor = Color.FromNonPremultiplied(113, 88, 71, 255);
        public static Color WallColor = Color.FromNonPremultiplied(194, 137, 64, 255);

        public static Texture2D CurrentMinimap;

        public static Texture2D GenerateMinimap(GraphicsDevice device, int[,] map)
        {
            CurrentMinimap?.Dispose();

            int width = map.GetLength(0);
            int height = map.GetLength(1);

            Texture2D minimap = new Texture2D(device, width, height);

            //RIP MEMORY
            Color[] colorData = new Color[width * height];
            for (int i = 0; i < colorData.Length; i++)
            {
                if (map[i % width, i / width] == 1)
                {
                    colorData[i] = WallColor;
                }
                else
                {
                    colorData[i] = FloorColor;
                }
            }

            minimap.SetData(colorData);

            CurrentMinimap = minimap;
            return minimap;
        }
    }
}
