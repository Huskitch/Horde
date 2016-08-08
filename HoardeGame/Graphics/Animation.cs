// <copyright file="Animation.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using Microsoft.Xna.Framework;

namespace HoardeGame.Graphics
{
    /// <summary>
    /// Definition of an animation for a sprite
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// Gets or sets the size of 1 frame in pixels
        /// </summary>
        public int FrameSize { get; set; }

        /// <summary>
        /// Gets or sets the animation <see cref="Rectangle"/>
        /// </summary>
        public Rectangle AnimRect { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the animation is looping
        /// </summary>
        public bool Looping { get; set; }

        private readonly int layer;
        private readonly int numFrames;
        private readonly int speed;
        private int framePosition;
        private float frameTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        /// <param name="frameSize">Size of 1 frame in pixels</param>
        /// <param name="layer">Y coordinate on the animation sheet</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="speed">Duration of a single frame in ms</param>
        public Animation(int frameSize, int layer, int numFrames, int speed)
        {
            FrameSize = frameSize;
            this.layer = layer;
            this.numFrames = numFrames;
            this.speed = speed;

            AnimRect = new Rectangle(framePosition * FrameSize, this.layer * FrameSize, FrameSize, FrameSize);
        }

        /// <summary>
        /// Updates the animation
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public void Update(GameTime gameTime)
        {
            frameTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (frameTimer > speed)
            {
                if (framePosition < numFrames)
                {
                    framePosition++;
                }
                else
                {
                    if (!Looping)
                    {
                        framePosition = 0;
                    }
                }

                frameTimer = 0;
            }

            AnimRect = new Rectangle(framePosition * FrameSize, layer * FrameSize, FrameSize, FrameSize);
        }
    }
}