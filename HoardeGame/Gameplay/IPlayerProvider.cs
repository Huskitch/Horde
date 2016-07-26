﻿// <copyright file="IPlayerProvider.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.Entities;

namespace HoardeGame.Gameplay
{
    /// <summary>
    /// Defines a provider of the player entity
    /// </summary>
    public interface IPlayerProvider
    {
        /// <summary>
        /// Gets the player entity
        /// </summary>
        IPlayer Player { get; }
    }
}