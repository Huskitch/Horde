﻿// <copyright file="ThemeManager.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;

namespace HoardeGame.Gameplay.Themes
{
    /// <summary>
    /// Stores themes for levels
    /// </summary>
    public class ThemeManager : IThemeProvider
    {
        private readonly Dictionary<string, Theme> themes = new Dictionary<string, Theme>();

        /// <summary>
        /// Load cards from a file
        /// </summary>
        /// <param name="filename">XML file to load from</param>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        public void LoadXmlFile(string filename, GameServiceContainer serviceContainer)
        {
            Debug.WriteLine("Loading themes...");
            themes.Clear();

            XmlSerializer ser = new XmlSerializer(typeof(List<Theme>), new XmlRootAttribute("Themes"));
            TextReader reader = new StreamReader(filename);

            List<Theme> themeList = ser.Deserialize(reader) as List<Theme>;
            if (themeList == null)
            {
                throw new InvalidDataException("Provided theme file is not valid! ({" + filename + "})");
            }

            var resourceProvider = serviceContainer.GetService<IResourceProvider>();

            foreach (var theme in themeList)
            {
                theme.Validate(resourceProvider);
                themes.Add(theme.ID, theme);

                Debug.WriteLine($"Loaded theme: {theme.Name} ({theme.ID})");
            }

            reader.Close();
            Debug.WriteLine("Done loading themes...");
        }

        /// <summary>
        /// Save cards to file
        /// </summary>
        /// <param name="filename">XML file to save to</param>
        public void SaveXmlFile(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Theme>), new XmlRootAttribute("Themes"));
            TextWriter writer = new StreamWriter(filename);

            ser.Serialize(writer, themes.Values.ToList());

            writer.Close();
        }

        /// <inheritdoc/>
        public Theme GetTheme(string name)
        {
            return !themes.ContainsKey(name) ? null : themes[name];
        }
    }
}