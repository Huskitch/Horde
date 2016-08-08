// <copyright file="SettingsManager.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Audio;

namespace HoardeGame.Settings
{
    /// <summary>
    /// Loads / saves and stores settings
    /// </summary>
    public class SettingsManager : ISettingsService
    {
        /// <inheritdoc/>
        public Settings Settings { get; set; }

        /// <inheritdoc/>
        public void LoadSettings()
        {
            Debug.WriteLine("Loading settings...");

            XmlSerializer ser = new XmlSerializer(typeof(Settings), new XmlRootAttribute("Settings"));
            TextReader reader = new StreamReader("Content/SETTINGS.xml");

            Settings settings = ser.Deserialize(reader) as Settings;
            if (settings == null)
            {
                throw new InvalidDataException("Provided settings file is not valid!");
            }

            reader.Close();

            Settings = settings;

            SoundEffect.MasterVolume = Settings.Volume;

            Debug.WriteLine("Done loading settings...");
        }

        /// <inheritdoc/>
        public void SaveSettings()
        {
            XmlSerializer ser = new XmlSerializer(typeof(Settings), new XmlRootAttribute("Settings"));
            TextWriter writer = new StreamWriter("Content/SETTINGS.xml");
            ser.Serialize(writer, Settings);
            writer.Close();
            Debug.WriteLine("Done saving settings...");
        }
    }
}