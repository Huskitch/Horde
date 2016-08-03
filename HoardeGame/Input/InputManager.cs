// <copyright file="InputManager.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public bool LockCursor { get; set; }

        /// <inheritdoc/>
        public Vector2 CursorDelta
        {
            get
            {
                if (LockCursor)
                {
                    return (MouseState.Position - screenCenter).ToVector2();
                }

                return (MouseState.Position - LastMouseState.Position).ToVector2();
            }
        }

        private readonly Dictionary<string, Func<bool>> keybinds = new Dictionary<string, Func<bool>>();
        private Point screenCenter;

        /// <summary>
        /// Initializes this instance of the <see cref="InputManager"/> class.
        /// </summary>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        public void Init(GameServiceContainer serviceContainer)
        {
            keybinds.Add("Activate", () => KeyPressed(Keys.E) || ButtonPressed(Buttons.A));
            keybinds.Add("Back", () => KeyPressed(Keys.Escape) || ButtonPressed(Buttons.B));
            keybinds.Add("StartGame", () => KeyPressed(Keys.Enter) || ButtonPressed(Buttons.A));
            keybinds.Add("PauseGame", () => KeyPressed(Keys.Escape) || ButtonPressed(Buttons.Start));
            keybinds.Add("ExitPausedGame", () => ButtonPressed(Buttons.B));
            keybinds.Add("ExitGame", () => ButtonPressed(Buttons.B) || KeyPressed(Keys.Escape));
            keybinds.Add("Weapon1", () => ButtonPressed(Buttons.DPadUp) || KeyPressed(Keys.D1));
            keybinds.Add("Weapon2", () => ButtonPressed(Buttons.DPadRight) || KeyPressed(Keys.D2));
            keybinds.Add("Weapon3", () => ButtonPressed(Buttons.DPadDown) || KeyPressed(Keys.D3));
            keybinds.Add("Weapon4", () => ButtonPressed(Buttons.DPadLeft) || KeyPressed(Keys.D4));

            Rectangle viewport = serviceContainer.GetService<IGraphicsDeviceService>().GraphicsDevice.Viewport.Bounds;
            screenCenter = new Point(viewport.Width / 2, viewport.Height / 2);
        }

        /// <inheritdoc/>
        public void Update(GameTime gameTime)
        {
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            LastMouseState = MouseState;
            MouseState = Mouse.GetState();

            LastGamePadState = GamePadState;
            GamePadState = GamePad.GetState(PlayerIndex.One);

            if (LockCursor)
            {
                Mouse.SetPosition(screenCenter.X, screenCenter.Y);
            }
        }

        /// <inheritdoc/>
        public bool KeyPressed(Keys key) => LastKeyboardState.IsKeyUp(key) && KeyboardState.IsKeyDown(key);

        /// <inheritdoc/>
        public bool ButtonPressed(Buttons button) => LastGamePadState.IsButtonUp(button) && GamePadState.IsButtonDown(button);

        /// <inheritdoc/>
        public bool KeybindPressed(string keybind)
        {
            if (keybinds.ContainsKey(keybind))
            {
                return keybinds[keybind]();
            }

            throw new ArgumentException($"Keybind {keybind} is not valid", nameof(keybind));
        }
    }
}