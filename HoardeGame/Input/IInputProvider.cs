// <copyright file="IInputProvider.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.Input
{
    /// <summary>
    /// Defines a provider for user input (mouse + keyboard)
    /// </summary>
    public interface IInputProvider
    {
        /// <summary>
        /// Gets current <see cref="KeyboardState"/>
        /// </summary>
        KeyboardState KeyboardState { get; }

        /// <summary>
        /// Gets last frame's <see cref="KeyboardState"/>
        /// </summary>
        KeyboardState LastKeyboardState { get; }

        /// <summary>
        /// Gets current <see cref="MouseState"/>
        /// </summary>
        MouseState MouseState { get; }

        /// <summary>
        /// Gets last frame's <see cref="MouseState"/>
        /// </summary>
        MouseState LastMouseState { get; }

        /// <summary>
        /// Gets current <see cref="GamePadState"/>
        /// </summary>
        GamePadState GamePadState { get; }

        /// <summary>
        /// Gets the last frame's <see cref="GamePadState"/>
        /// </summary>
        GamePadState LastGamePadState { get; }

        /// <summary>
        /// Gets a value indicating whether left mouse button was pressed this frame
        /// </summary>
        bool LeftClicked { get; }

        /// <summary>
        /// Gets a value indicating whether right mouse button was pressed this frame
        /// </summary>
        bool RightClicked { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the cursor should be locked to the center of the screen
        /// </summary>
        bool LockCursor { get; set; }

        /// <summary>
        /// Gets the movement of the cursor in the last frame
        /// </summary>
        Vector2 CursorDelta { get; }

        /// <summary>
        /// Update internal state
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Check if a key was pressed this frame
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>If key was pressed this frame</returns>
        bool KeyPressed(Keys key);

        /// <summary>
        /// Check if a button was pressed this frame
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns>If button was pressed this frame</returns>
        bool ButtonPressed(Buttons button);

        /// <summary>
        /// Check if a keybing was pressed this frame
        /// </summary>
        /// <param name="keybind">Name of the keybing</param>
        /// <returns>Whether the keybing was pressed this frame</returns>
        bool KeybindPressed(string keybind);
    }
}