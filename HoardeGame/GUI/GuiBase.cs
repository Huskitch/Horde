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
        /// <summary>
        /// Element is visible and will render regularly
        /// </summary>
        Visible,

        /// <summary>
        /// Element is visible but will be grayed out
        /// </summary>
        Disabled,

        /// <summary>
        /// Element will not render at all
        /// </summary>
        Hidden
    }

    /// <summary>
    /// Delegate used for clicking / hovering
    /// </summary>
    public delegate void ButtonDelegate();

    /// <summary>
    /// Delegate used for overriding draw of a control
    /// </summary>
    /// <param name="gameTime"><see cref="GameTime"/></param>
    /// <param name="spriteBatch">Current <see cref="SpriteBatch"/> to use for drawing</param>
    /// <param name="interpolation">No idea</param>
    public delegate void CustomDrawDelegate(GameTime gameTime, SpriteBatch spriteBatch, float interpolation);

    /// <summary>
    /// Delegate used for overriding update of a control
    /// </summary>
    /// <param name="gameTime"><see cref="GameTime"/></param>
    /// <param name="clickPoint"><see cref="Point"/> where the mouse is located</param>
    /// <param name="click">Whether the user clicked this frame</param>
    public delegate void CustomUpdateDelegate(GameTime gameTime, Point clickPoint, bool click);

    /// <summary>
    /// Base class for all GUI elements (controls)
    /// </summary>
    public class GuiBase
    {
        /// <summary>
        /// Gets or sets the font that is used in all controls
        /// </summary>
        public static SpriteFont Font { get; set; }

        /// <summary>
        /// Gets the ID of this control for debugging purposes
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Gets or sets the position of the control
        /// This value is passed directly to the spribatch so the position may not always be the same
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Microsoft.Xna.Framework.Color"/> of the control
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets the <see cref="Microsoft.Xna.Framework.Color"/> of the control when it's disabled / grayed out
        /// </summary>
        public Color DisabledColor { get; set; } = Color.Gray;

        /// <summary>
        /// Gets or sets the visibility state of the control
        /// </summary>
        public HiddenState Visibility { get; set; } = HiddenState.Visible;

        /// <summary>
        /// Gets or sets the handler for clicking
        /// </summary>
        public ButtonDelegate OnClick { get; set; }

        /// <summary>
        /// Gets or sets the handler for hovering
        /// </summary>
        public ButtonDelegate OnMouseOver { get; set; }

        /// <summary>
        /// Gets or sets the handler for drawing
        /// </summary>
        public CustomDrawDelegate CustomDraw { get; set; }

        /// <summary>
        /// Gets or sets the handler for update / click check
        /// </summary>
        public CustomUpdateDelegate CustomUpdate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiBase"/> class.
        /// This should never happen
        /// </summary>
        /// <param name="state">State to which this control belongs</param>
        /// <param name="id">ID of this control for debugging</param>
        /// <param name="noSub">Whether to subscribe to events in gamestate</param>
        public GuiBase(GameState state, string id, bool noSub = false)
        {
            ID = id;

            if (noSub)
            {
                return;
            }

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