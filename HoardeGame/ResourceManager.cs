// <copyright file="ResourceManager.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace HoardeGame
{
    /// <summary>
    /// Caches resources for the game
    /// </summary>
    public static class ResourceManager
    {
        /// <summary>
        /// Dictionary of all loaded <see cref="Texture2D"/>
        /// </summary>
        private static Dictionary<string, Texture2D> textures;

        /// <summary>
        /// Dictionary of all loaded <see cref="SpriteFont"/>
        /// </summary>
        private static Dictionary<string, SpriteFont> fonts;

        /// <summary>
        /// Dictionary of all loaded <see cref="SoundEffect"/>
        /// </summary>
        private static Dictionary<string, SoundEffect> sounds;

        /// <summary>
        /// Dictionary of all loaded <see cref="Song"/>
        /// </summary>
        private static Dictionary<string, Song> songs;

        /// <summary>
        /// Current <see cref="GraphicsDevice"/>
        /// </summary>
        private static GraphicsDevice device;

        /// <summary>
        /// Initializes the resource manager and prepares it for loading
        /// </summary>
        /// <param name="graphicsDevice"><see cref="GraphicsDevice"/></param>
        public static void Init(GraphicsDevice graphicsDevice)
        {
            device = graphicsDevice;

            textures = new Dictionary<string, Texture2D>();
            fonts = new Dictionary<string, SpriteFont>();
            sounds = new Dictionary<string, SoundEffect>();
            songs = new Dictionary<string, Song>();
        }

        /// <summary>
        /// Loads all content
        /// </summary>
        /// <param name="content"><see cref="ContentManager"/></param>
        public static void LoadContent(ContentManager content)
        {
            LoadTextures(content);
            LoadFonts(content);
            LoadSounds(content);
            LoadSongs(content);
        }

        /// <summary>
        /// Loads all <see cref="Texture2D"/>
        /// </summary>
        /// <param name="content"><see cref="ContentManager"/></param>
        public static void LoadTextures(ContentManager content)
        {
            Texture2D oneByOne = new Texture2D(device, 1, 1);
            oneByOne.SetData(new[] { Color.White });

            textures.Add("OneByOneEmpty", oneByOne);

            textures.Add("BasicFloor", content.Load<Texture2D>("Art/basicfloor"));
            textures.Add("BasicWall", content.Load<Texture2D>("Art/basicwall"));
            textures.Add("PlayerTemp", content.Load<Texture2D>("Art/PlayerTemp"));
            textures.Add("PlayerSheet", content.Load<Texture2D>("Art/PlayerSheet"));
            textures.Add("GemAnimation", content.Load<Texture2D>("Art/gemAnimation"));
            textures.Add("BatSheet", content.Load<Texture2D>("Art/BatSheet"));

            textures.Add("BasicButton", content.Load<Texture2D>("Art/button"));
            textures.Add("BasicButtonHover", content.Load<Texture2D>("Art/buttonHover"));

            textures.Add("BasicProgressBar", content.Load<Texture2D>("Art/progressBar"));

            textures.Add("TestCard", content.Load<Texture2D>("Art/testCard"));

            textures.Add("CommonCard", content.Load<Texture2D>("Art/commonCard"));
        }

        /// <summary>
        /// Loads all <see cref="SpriteFont"/>
        /// </summary>
        /// <param name="content"><see cref="ContentManager"/></param>
        public static void LoadFonts(ContentManager content)
        {
            fonts.Add("BasicFont", content.Load<SpriteFont>("BasicFont"));
        }

        /// <summary>
        /// Loads all <see cref="SoundEffect"/>
        /// </summary>
        /// <param name="content"><see cref="ContentManager"/></param>
        public static void LoadSounds(ContentManager content)
        {
        }

        /// <summary>
        /// Loads all <see cref="Song"/>
        /// </summary>
        /// <param name="content"><see cref="ContentManager"/>r</param>
        public static void LoadSongs(ContentManager content)
        {
        }

        /// <summary>
        /// Gets a <see cref="Texture2D"/> with provided key
        /// </summary>
        /// <param name="key">Name of the <see cref="Texture2D"/></param>
        /// <returns><see cref="Texture2D"/> with matching key or null</returns>
        public static Texture2D GetTexture(string key)
        {
            return !textures.ContainsKey(key) ? null : textures[key];
        }

        /// <summary>
        /// Gets a <see cref="SpriteFont"/> with provided key
        /// </summary>
        /// <param name="key">Name of the <see cref="SpriteFont"/></param>
        /// <returns><see cref="SpriteFont"/> with matching key or null</returns>
        public static SpriteFont GetFont(string key)
        {
            return !fonts.ContainsKey(key) ? null : fonts[key];
        }

        /// <summary>
        /// Gets a <see cref="SoundEffect"/> with provided key
        /// </summary>
        /// <param name="key">Name of the <see cref="SoundEffect"/></param>
        /// <returns><see cref="SoundEffect"/> with matching key or null</returns>
        public static SoundEffect GetSoundEffect(string key)
        {
            return !sounds.ContainsKey(key) ? null : sounds[key];
        }

        /// <summary>
        /// Gets a <see cref="Song"/> with provided key
        /// </summary>
        /// <param name="key">Name of the <see cref="Song"/></param>
        /// <returns><see cref="Song"/> with matching key or null</returns>
        public static Song GetSong(string key)
        {
            return !songs.ContainsKey(key) ? null : songs[key];
        }
    }
}