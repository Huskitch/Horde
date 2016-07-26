// <copyright file="StateManager.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace HoardeGame.State
{
    /// <summary>
    /// StateManager
    /// </summary>
    public class StateManager : IDisposable
    {
        /// <summary>
        /// Gets currently active <see cref="GameState"/>
        /// </summary>
        public GameState ActiveGameState { get; private set; }

        /// <summary>
        /// Gets list of all <see cref="GameState"/>
        /// </summary>
        public List<GameState> GameStates { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateManager"/> class.
        /// </summary>
        public StateManager()
        {
            GameStates = new List<GameState>();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            while (ActiveGameState != null)
            {
                Pop();
            }
        }

        /// <summary>
        /// Push a <see cref="GameState"/> onto the stack
        /// </summary>
        /// <param name="gameState"><see cref="GameState"/></param>
        /// <returns>New active <see cref="GameState"/></returns>
        public GameState Push(GameState gameState)
        {
            if (gameState == null)
            {
                throw new ArgumentNullException(nameof(gameState));
            }

            ActiveGameState?.Pause();

            GameStates.Add(gameState);
            ActiveGameState = gameState;
            ActiveGameState.Start();

            return ActiveGameState;
        }

        /// <summary>
        /// Pop current <see cref="GameState"/> off the stack
        /// </summary>
        /// <returns>New active <see cref="GameState"/></returns>
        public GameState Pop()
        {
            if (ActiveGameState == null)
            {
                throw new StackOverflowException("There are no GameStates on the stack to pop");
            }

            ActiveGameState.End();
            GameStates.Remove(ActiveGameState);

            if (GameStates.Count > 0)
            {
                ActiveGameState = GameStates[GameStates.Count - 1];
                ActiveGameState.Resume();
            }
            else
            {
                ActiveGameState = null;
            }

            return ActiveGameState;
        }

        /// <summary>
        /// Switch to another <see cref="GameState"/> on the stack
        /// </summary>
        /// <param name="gameState"><see cref="GameState"/> to switch to</param>
        public void Switch(GameState gameState)
        {
            if (gameState == null)
            {
                throw new ArgumentNullException(nameof(gameState));
            }

            ActiveGameState?.Pause();

            if (!GameStates.Remove(gameState))
            {
                throw new ArgumentException(gameState + " is not on the stack");
            }

            GameStates.Add(gameState);

            gameState.Resume();
            ActiveGameState = gameState;
        }

        /// <summary>
        /// Update all <see cref="GameState"/>
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < GameStates.Count; i++)
            {
                GameStates[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Draw all <see cref="GameState"/>
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        /// <param name="interpolation">Interpolation</param>
        public void Draw(GameTime gameTime, float interpolation)
        {
            for (int i = 0; i < GameStates.Count; i++)
            {
                GameStates[i].Draw(gameTime, interpolation);
            }
        }
    }
}