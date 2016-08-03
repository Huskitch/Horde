// <copyright file="ResourceManager.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace HoardeGame.Resources
{
    /// <summary>
    /// Caches resources for the game
    /// </summary>
    public class ResourceManager : IResourceProvider
    {
        /// <summary>
        /// Dictionary of all loaded <see cref="Texture2D"/>
        /// </summary>
        private Dictionary<string, Texture2D> textures;

        /// <summary>
        /// Dictionary of all loaded <see cref="SpriteFont"/>
        /// </summary>
        private Dictionary<string, SpriteFont> fonts;

        /// <summary>
        /// Dictionary of all loaded <see cref="SoundEffect"/>
        /// </summary>
        private Dictionary<string, SoundEffect> sounds;

        /// <summary>
        /// Dictionary of all loaded <see cref="Song"/>
        /// </summary>
        private Dictionary<string, Song> songs;

        /// <summary>
        /// Dictionary of all loaded <see cref="Effect"/>
        /// </summary>
        private Dictionary<string, Effect> effects;

        /// <summary>
        /// Current <see cref="GraphicsDevice"/>
        /// </summary>
        private GraphicsDevice device;

        /// <summary>
        /// Initializes the resource manager and prepares it for loading
        /// </summary>
        /// <param name="graphicsDevice"><see cref="GraphicsDevice"/></param>
        public void Init(GraphicsDevice graphicsDevice)
        {
            device = graphicsDevice;

            textures = new Dictionary<string, Texture2D>();
            fonts = new Dictionary<string, SpriteFont>();
            sounds = new Dictionary<string, SoundEffect>();
            songs = new Dictionary<string, Song>();
            effects = new Dictionary<string, Effect>();
        }

        /// <summary>
        /// Loads all content
        /// </summary>
        /// <param name="content"><see cref="ContentManager"/></param>
        public void LoadContent(ContentManager content)
        {
            LoadTextures(content);
            LoadFonts(content);
            LoadSounds(content);
            LoadSongs(content);
            LoadEffects(content);
        }

        /// <summary>
        /// Loads all <see cref="Effect"/>
        /// </summary>
        /// <param name="content"><see cref="ContentManager"/></param>
        public void LoadEffects(ContentManager content)
        {
            effects.Add("SpriteEffect", content.Load<Effect>("sprite"));
        }

        /// <summary>
        /// Loads all <see cref="Texture2D"/>
        /// </summary>
        /// <param name="content"><see cref="ContentManager"/></param>
        public void LoadTextures(ContentManager content)
        {
            Texture2D oneByOne = new Texture2D(device, 1, 1);
            oneByOne.SetData(new[] { Color.White });

            textures.Add("OneByOneEmpty", oneByOne);

            textures.Add("BasicFloor", content.Load<Texture2D>("Art/basicfloor"));
            textures.Add("PlayerTemp", content.Load<Texture2D>("Art/PlayerTemp"));
            textures.Add("PlayerSheet", content.Load<Texture2D>("Art/PlayerSheet"));
            textures.Add("GemAnimation", content.Load<Texture2D>("Art/gemAnimation"));
            textures.Add("BatSheet", content.Load<Texture2D>("Art/BatSheet"));
            textures.Add("SnakeSheet", content.Load<Texture2D>("Art/SnakeSheet"));
            textures.Add("DiamondSheet", content.Load<Texture2D>("Art/DiamondSheet"));
            textures.Add("EmeraldSheet", content.Load<Texture2D>("Art/EmeraldSheet"));
            textures.Add("Chest", content.Load<Texture2D>("Art/Chest"));
            textures.Add("Bullet", content.Load<Texture2D>("Art/playerBullet"));
            textures.Add("Drill", content.Load<Texture2D>("Art/drill"));
            textures.Add("EntityKey", content.Load<Texture2D>("Art/KeyEntity"));
            textures.Add("Health", content.Load<Texture2D>("Art/Heart"));
            textures.Add("Armour", content.Load<Texture2D>("Art/Shield"));
            textures.Add("Ammo", content.Load<Texture2D>("Art/Bullets"));

            textures.Add("Cursor", content.Load<Texture2D>("Art/cursor"));

            textures.Add("BasicButton", content.Load<Texture2D>("Art/button"));
            textures.Add("BasicButtonHover", content.Load<Texture2D>("Art/buttonHover"));

            textures.Add("BasicProgressBar", content.Load<Texture2D>("Art/progressBar"));

            textures.Add("TestCard", content.Load<Texture2D>("Art/testCard"));

            textures.Add("CommonCard", content.Load<Texture2D>("Art/commonCard"));

            textures.Add("healthBG", content.Load<Texture2D>("Art/healthBG"));
            textures.Add("ChestSheet", content.Load<Texture2D>("Art/ChestSheet"));

            textures.Add("testweapon", content.Load<Texture2D>("Art/testweapon"));

            textures.Add("BasicWall", content.Load<Texture2D>("Art/walls"));
        }

        /// <summary>
        /// Loads all <see cref="SpriteFont"/>
        /// </summary>
        /// <param name="content"><see cref="ContentManager"/></param>
        public void LoadFonts(ContentManager content)
        {
            fonts.Add("BasicFont", content.Load<SpriteFont>("BasicFont"));
            fonts.Add("BigFont", content.Load<SpriteFont>("BigFont"));
            fonts.Add("SmallFont", content.Load<SpriteFont>("SmallerFont"));
        }

        /// <summary>
        /// Loads all <see cref="SoundEffect"/>
        /// </summary>
        /// <param name="content"><see cref="ContentManager"/></param>
        public void LoadSounds(ContentManager content)
        {
            sounds.Add("Fire", content.Load<SoundEffect>("SFX/Fire"));
            sounds.Add("Death", content.Load<SoundEffect>("SFX/Death"));
            sounds.Add("Gem", content.Load<SoundEffect>("SFX/Gem"));
            sounds.Add("Hurt", content.Load<SoundEffect>("SFX/Hurt"));
            sounds.Add("PlayerDeath", content.Load<SoundEffect>("SFX/PlayerDeath"));
            sounds.Add("ArmourGone", content.Load<SoundEffect>("SFX/ArmourGone"));
        }

        /// <summary>
        /// Loads all <see cref="Song"/>
        /// </summary>
        /// <param name="content"><see cref="ContentManager"/>r</param>
        public void LoadSongs(ContentManager content)
        {
        }

        /// <summary>
        /// Gets a <see cref="Texture2D"/> with provided key
        /// </summary>
        /// <param name="key">Name of the <see cref="Texture2D"/></param>
        /// <returns><see cref="Texture2D"/> with matching key or null</returns>
        public Texture2D GetTexture(string key)
        {
            return !textures.ContainsKey(key) ? null : textures[key];
        }

        /// <summary>
        /// Gets a <see cref="SpriteFont"/> with provided key
        /// </summary>
        /// <param name="key">Name of the <see cref="SpriteFont"/></param>
        /// <returns><see cref="SpriteFont"/> with matching key or null</returns>
        public SpriteFont GetFont(string key)
        {
            return !fonts.ContainsKey(key) ? null : fonts[key];
        }

        /// <summary>
        /// Gets a <see cref="SoundEffect"/> with provided key
        /// </summary>
        /// <param name="key">Name of the <see cref="SoundEffect"/></param>
        /// <returns><see cref="SoundEffect"/> with matching key or null</returns>
        public SoundEffect GetSoundEffect(string key)
        {
            return !sounds.ContainsKey(key) ? null : sounds[key];
        }

        /// <summary>
        /// Gets an instance of <see cref="SoundEffect"/> with provided key
        /// </summary>
        /// <param name="key">Name of the <see cref="SoundEffect"/></param>
        /// <returns><see cref="SoundEffectInstance"/> with matching key or null</returns>
        public SoundEffectInstance GetSoundEffectInstance(string key)
        {
            return !sounds.ContainsKey(key) ? null : sounds[key].CreateInstance();
        }

        /// <summary>
        /// Gets a <see cref="Song"/> with provided key
        /// </summary>
        /// <param name="key">Name of the <see cref="Song"/></param>
        /// <returns><see cref="Song"/> with matching key or null</returns>
        public Song GetSong(string key)
        {
            return !songs.ContainsKey(key) ? null : songs[key];
        }

        /// <inheritdoc/>
        public Effect GetEffect(string key)
        {
            return !effects.ContainsKey(key) ? null : effects[key];
        }
    }
}