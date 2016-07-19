// <copyright file="InputManager.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.Input
{
    /// <summary>
    /// Manager to get input from keyboard and mouse
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// Gets current <see cref="KeyboardState"/>
        /// </summary>
        public static KeyboardState KeyboardState { get; private set; }

        /// <summary>
        /// Gets last frame's <see cref="KeyboardState"/> 
        /// </summary>
        public static KeyboardState LastKeyboardState { get; private set; }

        /// <summary>
        /// Gets current <see cref="MouseState"/>
        /// </summary>
        public static MouseState MouseState { get; private set; }

        /// <summary>
        /// Gets last frame's <see cref="MouseState"/>
        /// </summary>
        public static MouseState LastMouseState { get; private set; }

        /// <summary>
        /// Gets a value indicating whether left mouse button was pressed this frame
        /// </summary>
        public static bool LeftClicked => LastMouseState.LeftButton == ButtonState.Released && MouseState.LeftButton == ButtonState.Pressed;

        /// <summary>
        /// Gets a value indicating whether right mouse button was pressed this frame
        /// </summary>
        public static bool RightClicked => LastMouseState.RightButton == ButtonState.Released && MouseState.RightButton == ButtonState.Pressed;

        /// <summary>
        /// Update internal state
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public static void Update(GameTime gameTime)
        {
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            LastMouseState = MouseState;
            MouseState = Mouse.GetState();
        }

        /// <summary>
        /// Check if a key was pressed this frame
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>If key was pressed this frame</returns>
        public static bool KeyPressed(Keys key) => LastKeyboardState.IsKeyUp(key) && KeyboardState.IsKeyDown(key);
    }
}