using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace HoardeGame
{
    public class ResourceManager
    {
        private static Dictionary<string, Texture2D> textures;
        private static Dictionary<string, SpriteFont> fonts;
        private static Dictionary<string, SoundEffect> sounds;
        private static Dictionary<string, Song> songs;
        private static GraphicsDevice device;

        public static void Init()
        {
            textures = new Dictionary<string, Texture2D>();
            fonts = new Dictionary<string, SpriteFont>();
            sounds = new Dictionary<string, SoundEffect>();
            songs = new Dictionary<string, Song>();
        }

        public static void LoadContent(ContentManager content)
        {
            LoadTextures(content);
            LoadFonts(content);
            LoadSounds(content);
            LoadSongs(content);
        }

        public static void LoadTextures(ContentManager content)
        {

        }

        public static void LoadFonts(ContentManager content)
        {

        }

        public static void LoadSounds(ContentManager content)
        {

        }

        public static void LoadSongs(ContentManager content)
        {

        }

        public static Texture2D Texture(string key)
        {
            return textures[key];
        }

        public static SpriteFont Font(string key)
        {
            return fonts[key];
        }

        public static SoundEffect SoundEffect(string key)
        {
            return sounds[key];
        }

        public static Song Song(string key)
        {
            return songs[key];
        }
    }
}


