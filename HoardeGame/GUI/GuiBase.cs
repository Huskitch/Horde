// <copyright file="GuiBase.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GUI
{
    /// <summary>
    /// Definition of visibility states for controls
    /// </summary>
    public enum HiddenState
    {
        Visible,
        Disabled,
        Hidden
    }

    /// <summary>
    /// Delegate used for clicking / hovering
    /// </summary>
    public delegate void ButtonDelegate();

    /// <summary>
    /// Delegate used for overriding draw of a control
    /// </summary>
    /// <param name="gameTime">Current game time</param>
    /// <param name="spriteBatch">Current spirtebatch to use for drawing</param>
    /// <param name="interpolation">No idea</param>
    public delegate void CustomDrawDelegate(GameTime gameTime, SpriteBatch spriteBatch, float interpolation);

    /// <summary>
    /// Delegate used for overriding update of a control
    /// </summary>
    /// <param name="gameTime">Current gametime</param>
    /// <param name="clickPoint">Point where the mouse is located</param>
    /// <param name="click">Whether the user clicked this frame</param>
    public delegate void CustomUpdateDelegate(GameTime gameTime, Point clickPoint, bool click);

    /// <summary>
    /// Base class for all GUI elements (controls)
    /// </summary>
    public class GuiBase
    {
        /// <summary>
        /// Font that is used in all controls
        /// </summary>
        public static SpriteFont Font;

        /// <summary>
        /// ID of this control for debugging purposes
        /// </summary>
        public readonly string ID;

        /// <summary>
        /// Position of the control
        /// This value is passed directly to the spribatch so the position may not always be the same
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Colour of the control
        /// </summary>
        public Color Color = Color.White;

        /// <summary>
        /// Color of the control when it's disabled / grayed out
        /// </summary>
        public Color DisabledColor = Color.Gray;

        /// <summary>
        /// Current visibility state of the control
        /// </summary>
        public HiddenState Visibility = HiddenState.Visible;

        /// <summary>
        /// Custom handler for clicking
        /// </summary>
        public ButtonDelegate OnClick = () => { };

        /// <summary>
        /// Custom handler for hovering
        /// </summary>
        public ButtonDelegate OnMouseOver = () => { };

        /// <summary>
        /// Custom handler for drawing
        /// </summary>
        public CustomDrawDelegate CustomDraw = (time, spriteBatch, interpolation) => { };

        /// <summary>
        /// Custom handle for update / click check
        /// </summary>
        public CustomUpdateDelegate CustomUpdate = (time, point, click) => { };

        /// <summary>
        /// Creates a new empty GUI element
        /// This should never happen
        /// </summary>
        /// <param name="state">State to which this control belongs</param>
        /// <param name="id">ID of this control for debugging</param>
        /// <param name="noSub">Whether to subscribe to events in gamestate</param>
        public GuiBase(GameState state, string id, bool noSub = false)
        {
            ID = id;

            if (noSub) return;

            state.OnCheck += Check;
            state.OnDraw += Draw;
        }

        /// <summary>
        /// Sets the visibility of the control
        /// </summary>
        /// <param name="state">Target visibility</param>
        public virtual void SetVisibility(HiddenState state)
        {
            Visibility = state;
        }

        /// <summary>
        /// Checks if the user has clicked / hovered over the control and runs the delegate in case the user did
        /// This method is used only for overriding
        /// </summary>
        /// <param name="gameTime">Current gametime</param>
        /// <param name="point">Point where the mouse is located</param>
        /// <param name="click">Whether the user clicked this frame</param>
        public virtual void Check(GameTime gameTime, Point point, bool click)
        {
        }

        /// <summary>
        /// Draws the control
        /// This method is used only for overriding
        /// </summary>
        /// <param name="gameTime">Current gametime</param>
        /// <param name="spriteBatch">Current spirtebatch to use for drawing</param>
        /// <param name="interpolation">No idea</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, float interpolation)
        {
        }
    }
}