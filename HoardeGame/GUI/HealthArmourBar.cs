// <copyright file="HealthArmourBar.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using HoardeGame.Resources;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GUI
{
    /// <summary>
    /// A control made out of 2 bars and 2 labels
    /// </summary>
    public class HealthArmourBar : GuiBase
    {
        /// <summary>
        /// Gets or sets the maximum health
        /// </summary>
        public int MaxHealth { get; set; }

        /// <summary>
        /// Gets or sets the maximum armour
        /// </summary>
        public int MaxArmour { get; set; }

        /// <summary>
        /// Gets or sets the current health
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Gets or sets the current armour
        /// </summary>
        public int Armour { get; set; }

        /// <summary>
        /// Gets or sets the width or the health progress bar
        /// </summary>
        public int HealthWidth { get; set; }

        /// <summary>
        /// Gets or sets the width of the armour progress bar
        /// </summary>
        public int ArmourWidth { get; set; }

        /// <summary>
        /// Gets or sets the offset of the armour bar
        /// </summary>
        public Vector2 ArmourOffset { get; set; }

        /// <summary>
        /// Gets or sets the health magic offset value
        /// Tamper with this when the label overlaps when the bar is full
        /// </summary>
        public float HealthMagicValue { get; set; }

        /// <summary>
        /// Gets or sets the armour magic offset value
        /// Tamper with this when the label overlaps when the bar is full
        /// </summary>
        public float ArmourMagicValue { get; set; }

        /// <summary>
        /// Gets or sets the colour of the health progress bar
        /// </summary>
        public Color HealthColor { get; set; }

        /// <summary>
        /// Gets or sets the colour of the armour progress bar
        /// </summary>
        public Color ArmourColor { get; set; }

        private ProgressBar healthBar;
        private ProgressBar armourBar;
        private Label healthLabel;
        private Label armorLabel;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthArmourBar"/> class.
        /// </summary>
        /// <param name="state"><see cref="GameState"/> to which this bar belongs</param>
        /// <param name="id">ID of this control for debugging</param>
        /// <param name="noSub">Whether to subscribe to events in gamestate</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to use for loading resources</param>
        public HealthArmourBar(GameState state, IResourceProvider resourceProvider, string id, bool noSub = false) : base(state, id, noSub)
        {
            healthBar = new ProgressBar(state, resourceProvider, "healthProgressBar" + id, true)
            {
                Position = new Vector2(30, 20),
                ProgressBarTexture = resourceProvider.GetTexture("BasicProgressBar"),
                BarHeight = 45,
                BarStart = new Vector2(1, 1),
                BarWidth = 200,
                TargetRectangle = new Rectangle(0, 0, 0, 0),
                Color = new Color(190, 74, 57),
                Progress = 1f
            };

            healthLabel = new Label(state, "healthLabel" + id, true)
            {
                Position = new Vector2(176, 29),
                Color = Color.LightGray,
                Text = "10"
            };

            armourBar = new ProgressBar(state, resourceProvider, "armorProgressBar" + id, true)
            {
                Position = new Vector2(15, 45),
                ProgressBarTexture = resourceProvider.GetTexture("BasicProgressBar"),
                BarHeight = 45,
                BarStart = new Vector2(1, 1),
                BarWidth = 144,
                TargetRectangle = new Rectangle(0, 0, 0, 0),
                Color = new Color(82, 141, 156),
                Progress = 1f
            };

            armorLabel = new Label(state, "armorLabel" + id, true)
            {
                Position = new Vector2(130 * armourBar.Progress, 55),
                Color = Color.LightGray,
                Text = "10"
            };
        }

        /// <summary>
        /// Applies the fields to the control
        /// </summary>
        public void ApplyChanges()
        {
            armorLabel.Color = Color;
            healthLabel.Color = Color;

            armourBar.Color = ArmourColor;
            healthBar.Color = HealthColor;

            healthBar.BarWidth = HealthWidth;
            armourBar.BarWidth = ArmourWidth;

            healthBar.Position = Position;
            armourBar.Position = healthBar.Position + ArmourOffset;
        }

        /// <inheritdoc/>
        public override void Check(GameTime gameTime, Point point, bool click)
        {
            healthLabel.Position = healthBar.Position + new Vector2(8, 10) + new Vector2(healthBar.BarWidth * Math.Max(healthBar.Progress - HealthMagicValue, 0f), 0);
            armorLabel.Position = armourBar.Position + new Vector2(8, 10) + new Vector2(armourBar.BarWidth * Math.Max(armourBar.Progress - ArmourMagicValue, 0f), 0);

            healthLabel.Text = Health.ToString();
            healthBar.Progress = Health / (float)MaxHealth;

            armorLabel.Text = Armour.ToString();
            armourBar.Progress = Armour / (float)MaxArmour;
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float interpolation)
        {
            healthBar.Draw(gameTime, spriteBatch, interpolation);
            healthLabel.Draw(gameTime, spriteBatch, interpolation);

            armourBar.Draw(gameTime, spriteBatch, interpolation);
            armorLabel.Draw(gameTime, spriteBatch, interpolation);
        }
    }
}