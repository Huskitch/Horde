// <copyright file="EnemySpawnInfo.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

namespace HoardeGame.Themes
{
    /// <summary>
    /// Contains information about an enemy spawner
    /// </summary>
    public class EnemySpawnInfo
    {
        /// <summary>
        /// Gets or sets the type of the enemy
        /// </summary>
        public string EnemyType { get; set; }

        /// <summary>
        /// Gets or sets the minimal size of the cluster
        /// </summary>
        public int MinClusterSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum size of the cluster
        /// </summary>
        public int MaxClusterSize { get; set; }

        /// <summary>
        /// Gets or sets the enemy spawn rate
        /// </summary>
        public int SpawnRate { get; set; }
    }
}