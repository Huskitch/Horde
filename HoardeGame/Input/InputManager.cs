// <copyright file="InputManager.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.Input
{
    /// <summary>
    /// Manager to get input from keyboard and mouse
    /// </summary>
    public class InputManager : IInputProvider
    {
        /// <inheritdoc/>
        public KeyboardState KeyboardState { get; private set; }

        /// <inheritdoc/>
        public KeyboardState LastKeyboardState { get; private set; }

        /// <inheritdoc/>
        public MouseState MouseState { get; private set; }

        /// <inheritdoc/>
        public MouseState LastMouseState { get; private set; }

        /// <inheritdoc/>
        public GamePadState GamePadState { get; private set; }

        /// <inheritdoc/>
        public GamePadState LastGamePadState { get; private set; }

        /// <inheritdoc/>
        public bool LeftClicked => LastMouseState.LeftButton == ButtonState.Released && MouseState.LeftButton == ButtonState.Pressed;

        /// <inheritdoc/>
        public bool RightClicked => LastMouseState.RightButton == ButtonState.Released && MouseState.RightButton == ButtonState.Pressed;

        /// <inheritdoc/>
        public bool Activate => KeyPressed(Keys.E) || ButtonPressed(Buttons.A);

        /// <inheritdoc/>
        public bool Back => KeyPressed(Keys.Escape) || ButtonPressed(Buttons.Back);

        /// <inheritdoc/>
        public void Update(GameTime gameTime)
        {
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            LastMouseState = MouseState;
            MouseState = Mouse.GetState();

            LastGamePadState = GamePadState;
            GamePadState = GamePad.GetState(PlayerIndex.One);
        }

        /// <inheritdoc/>
        public bool KeyPressed(Keys key) => LastKeyboardState.IsKeyUp(key) && KeyboardState.IsKeyDown(key);

        /// <inheritdoc/>
        public bool ButtonPressed(Buttons button) => LastGamePadState.IsButtonUp(button) && GamePadState.IsButtonDown(button);
    }
}