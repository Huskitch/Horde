// <copyright file="AnimatedSprite.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Graphics.Rendering
{
    /// <summary>
    /// A sprite consisting of multiple frames that can change after a certain period of time
    /// </summary>
    public class AnimatedSprite
    {
        private readonly Texture2D sheet;
        private readonly Dictionary<string, Animation> animations;
        private Animation lastUsedAnimation;
        private Animation defaultAnimation;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedSprite"/> class.
        /// </summary>
        /// <param name="spriteSheet"><see cref="Texture2D"/> to be used as the animation sheet</param>
        public AnimatedSprite(Texture2D spriteSheet)
        {
            sheet = spriteSheet;
            animations = new Dictionary<string, Animation>();
        }

        /// <summary>
        /// Adds an <see cref="Animation"/> to the manager"/>
        /// </summary>
        /// <param name="name">Name of the <see cref="Animation"/></param>
        /// <param name="frameSize">Size of the frame in pixels</param>
        /// <param name="layer">Y coordinate on the animation sheet</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="speed">Duration of a single frame in ms</param>
        public void AddAnimation(string name, int frameSize, int layer, int numFrames, int speed)
        {
            animations.Add(name, new Animation(frameSize, layer, numFrames, speed));
        }

        /// <summary>
        /// Sets the default animation
        /// </summary>
        /// <param name="name">Name</param>
        public void SetDefaultAnimation(string name)
        {
            defaultAnimation = animations[name];
        }

        /// <summary>
        /// Draw the last used animation
        /// </summary>
        /// <param name="position">Position where to draw the <see cref="Animation"/></param>
        /// <param name="spriteBatch"><see cref="SpriteFont"/> to draw the animation with</param>
        public void DrawLastUsedAnimation(Vector2 position, SpriteBatch spriteBatch)
        {
            Animation selectedAnim;

            if (lastUsedAnimation == null)
            {
                selectedAnim = defaultAnimation;
            }
            else
            {
                selectedAnim = lastUsedAnimation;
            }

            spriteBatch.Draw(sheet, new Rectangle((int)position.X, (int)position.Y, selectedAnim.FrameSize, selectedAnim.FrameSize), selectedAnim.AnimRect, Color.White);
        }

        /// <summary>
        /// Draws an <see cref="Animation"/>
        /// </summary>
        /// <param name="animation">Name of the <see cref="Animation"/></param>
        /// <param name="position">Position where to draw the <see cref="Animation"/></param>
        /// <param name="spriteBatch"><see cref="SpriteFont"/> to draw the animation with</param>
        /// <param name="color"><see cref="Color"/> tint of the sprite</param>
        /// <param name="looping">Whether the animation should loop</param>
        public void DrawAnimation(string animation, Vector2 position, SpriteBatch spriteBatch, Color color, bool looping = true)
        {
            Animation selectedAnim = animations[animation];
            selectedAnim.Looping = looping;
            lastUsedAnimation = selectedAnim;
            spriteBatch.Draw(sheet, new Rectangle((int)position.X, (int)position.Y, selectedAnim.FrameSize, selectedAnim.FrameSize), selectedAnim.AnimRect, color);
        }

        /// <summary>
        /// Draws an <see cref="Animation"/>
        /// </summary>
        /// <param name="animation">Name of the <see cref="Animation"/></param>
        /// <param name="position">Position where to draw the <see cref="Animation"/></param>
        /// <param name="spriteBatch"><see cref="SpriteFont"/> to draw the animation with</param>
        /// <param name="color"><see cref="Color"/> tint of the sprite</param>
        /// <param name="parameter"><see cref="EffectParameter"/> for flashing</param>
        /// <param name="parameterValue"><see cref="Color"/> for flashing</param>
        public void DrawAnimation(string animation, Vector2 position, SpriteBatch spriteBatch, Color color, EffectParameter parameter, Color parameterValue)
        {
            Animation selectedAnim = animations[animation];
            lastUsedAnimation = selectedAnim;

            parameter.SetValue(parameterValue.ToVector4());

            spriteBatch.Draw(sheet, new Rectangle((int)position.X, (int)position.Y, selectedAnim.FrameSize, selectedAnim.FrameSize), selectedAnim.AnimRect, color);

            parameter.SetValue(Color.Black.ToVector4());
        }

        /// <summary>
        /// Updates all the <see cref="Animation"/>
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public void Update(GameTime gameTime)
        {
            foreach (Animation anim in animations.Values)
            {
                anim.Update(gameTime);
            }
        }
    }
}