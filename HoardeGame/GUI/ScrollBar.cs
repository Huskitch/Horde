// <copyright file="ScrollBar.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.State;

namespace HoardeGame.GUI
{
    /// <summary>
    /// Scrollbar control
    /// </summary>
    public class ScrollBar : GuiBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBar"/> class.
        /// </summary>
        /// <param name="state"><see cref="GameState"/> to which this progress bar belongs</param>
        /// <param name="id">ID of this control for debugging</param>
        /// <param name="noSub">Whether to subscribe to events in gamestate</param>
        public ScrollBar(GameState state, string id, bool noSub = false) : base(state, id, noSub)
        {
        }
    }
}