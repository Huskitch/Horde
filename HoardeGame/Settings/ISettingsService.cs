// <copyright file="ISettingsService.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

namespace HoardeGame.Settings
{
    /// <summary>
    /// Definition of a service providing settings
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Gets or sets the values of the settings
        /// </summary>
        Settings Settings { get; set; }

        /// <summary>
        /// Saves settings into SETTINGS.xml
        /// </summary>
        void SaveSettings();

        /// <summary>
        /// Loads settings from SETTIGNS.xml
        /// </summary>
        void LoadSettings();
    }
}