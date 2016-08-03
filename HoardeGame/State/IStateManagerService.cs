// <copyright file="IStateManagerService.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

namespace HoardeGame.State
{
    /// <summary>
    /// Service providing access to StateManager
    /// </summary>
    public interface IStateManagerService
    {
        /// <summary>
        /// Gets the StateManager
        /// </summary>
        StateManager StateManager { get; }
    }
}