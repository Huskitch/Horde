﻿// <copyright file="ResourceManager.cs" company="Kuub Studios">
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
    public class ResourceManager
    {
        /// <summary>
        /// Dictionary of all loaded textures
        /// </summary>
        private static Dictionary<string, Texture2D> textures;

        /// <summary>
        /// Dictionary of all loaded fonts
        /// </summary>
        private static Dictionary<string, SpriteFont> fonts;

        /// <summary>
        /// Dictionary of all loaded sounds
        /// </summary>
        private static Dictionary<string, SoundEffect> sounds;

        /// <summary>
        /// Dictionary of all loaded songs
        /// </summary>
        private static Dictionary<string, Song> songs;

        /// <summary>
        /// Current graphics device
        /// </summary>
        private static GraphicsDevice device;

        /// <summary>
        /// Initializes the resource manager and prepares it for loading
        /// </summary>
        /// <param name="graphicsDevice"></param>
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
        /// <param name="content">MonoGame content manager</param>
        public static void LoadContent(ContentManager content)
        {
            LoadTextures(content);
            LoadFonts(content);
            LoadSounds(content);
            LoadSongs(content);
        }

        /// <summary>
        /// Loads all textures
        /// </summary>
        /// <param name="content">MonoGame content manager</param>
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
        /// Loads all fonts
        /// </summary>
        /// <param name="content">MonoGame content manager</param>
        public static void LoadFonts(ContentManager content)
        {
            fonts.Add("BasicFont", content.Load<SpriteFont>("BasicFont"));
        }

        /// <summary>
        /// Loads all sounds
        /// </summary>
        /// <param name="content">MonoGame content manager</param>
        public static void LoadSounds(ContentManager content)
        {

        }

        /// <summary>
        /// Loads all songs
        /// </summary>
        /// <param name="content">MonoGame content manager</param>
        public static void LoadSongs(ContentManager content)
        {

        }

        /// <summary>
        /// Gets a texture with provided key
        /// </summary>
        /// <param name="key">Name of the texture</param>
        /// <returns></returns>
        public static Texture2D Texture(string key)
        {
            return !textures.ContainsKey(key) ? null : textures[key];
        }

        /// <summary>
        /// Gets a font with provided key
        /// </summary>
        /// <param name="key">Name of the font</param>
        /// <returns></returns>
        public static SpriteFont Font(string key)
        {
            return !fonts.ContainsKey(key) ? null : fonts[key];
        }

        /// <summary>
        /// Gets a sound effect with provided key
        /// </summary>
        /// <param name="key">Name of the sound effect</param>
        /// <returns></returns>
        public static SoundEffect SoundEffect(string key)
        {
            return !sounds.ContainsKey(key) ? null : sounds[key];
        }

        /// <summary>
        /// Gets a song with provided key
        /// </summary>
        /// <param name="key">Name of the song</param>
        /// <returns></returns>
        public static Song Song(string key)
        {
            return !songs.ContainsKey(key) ? null : songs[key];
        }
    }
}


