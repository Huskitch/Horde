// <copyright file="IThemeProvider.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

namespace HoardeGame.Gameplay.Themes
{
    /// <summary>
    /// Defines a provider for level themes
    /// </summary>
    public interface IThemeProvider
    {
        /// <summary>
        /// Gets a <see cref="Theme"/> with a specified name
        /// </summary>
        /// <param name="name">Name of the theme</param>
        /// <returns><see cref="Theme"/> or null</returns>
        Theme GetTheme(string name);
    }
}