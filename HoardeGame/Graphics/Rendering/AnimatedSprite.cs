// <copyright file="AnimatedSprite.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Graphics.Rendering
{
    public class Animation
    {
        public int FrameSize;
        private int Layer;
        private int NumFrames;
        private int Speed;
        public Rectangle animRect;
        private int framePosition;
        private float frameTimer;

        public Animation(int frameSize, int layer, int numFrames, int speed)
        {
            FrameSize = frameSize;
            Layer = layer;
            NumFrames = numFrames;
            Speed = speed;

            animRect = new Rectangle(framePosition * FrameSize, Layer * FrameSize, FrameSize, FrameSize);
        }

        public void Update(GameTime gameTime)
        {
            frameTimer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (frameTimer > Speed)
            {
                if (framePosition < NumFrames)
                {
                    framePosition++;
                }
                else
                {
                    framePosition = 1;
                }

                frameTimer = 0;
            }

            animRect = new Rectangle(framePosition * FrameSize, Layer * FrameSize, FrameSize, FrameSize);
        }
    }

    public class AnimatedSprite
    {
        private Texture2D sheet;
        private Dictionary<string, Animation> animations;

        public AnimatedSprite(Texture2D spriteSheet)
        {
            sheet = spriteSheet;
            animations = new Dictionary<string, Animation>();
        }

        public void AddAnimation(string name, int frameSize, int layer, int numFrames, int speed)
        {
            animations.Add(name, new Animation(frameSize, layer, numFrames, speed));
        }

        public void DrawAnimation(string animation, Vector2 position, SpriteBatch spriteBatch)
        {
            Animation selectedAnim = animations[animation];
            spriteBatch.Draw(sheet, new Rectangle((int)position.X, (int)position.Y, selectedAnim.FrameSize, selectedAnim.FrameSize), selectedAnim.animRect, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            foreach (Animation anim in animations.Values)
            {
                anim.Update(gameTime);
            }
        }
    }
}