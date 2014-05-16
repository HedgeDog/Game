using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace xnaplatformer
{
    public class SpriteSheetAnimation : Animation
    {
        protected int frameCounter, switchFrame;
        
        protected Vector2 frames, currentFrame;

        protected Vector2 offset;

        public Vector2 Frames
        {
            set { frames = value; }
        }

        public Vector2 Offset
        {
            get { return this.offset; }
        }
        public Vector2 CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }

        public override int FrameWidth()
        {
           { return image.Width / (int)frames.X; }
        }
        public int FrameCounter
        {
            get { return frameCounter; }
            set { frameCounter = value; }
        }
        public override int FrameHeight()
        {
           { return image.Height / (int)frames.Y; }
        }

        public override void LoadContent(ContentManager Content)
        {

        }

        public override void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position)
        {
            base.LoadContent(Content, image, text, position);
            frameCounter = 0;
            switchFrame = 100;
            offset = Vector2.Zero;
            //frames = new Vector2(3, 4);
            currentFrame = new Vector2(0, 0);
            sourceRect = new Rectangle((int)currentFrame.X * FrameWidth(), (int)currentFrame.Y * FrameHeight(), FrameWidth(), FrameHeight());
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameCounter >= switchFrame)
                {
                    frameCounter = 0;
                    currentFrame.X++;

                    if (currentFrame.X * FrameWidth() >= image.Width)
                        currentFrame.X = 0;
                }
            }
            else
            {
                frameCounter = 0;
                currentFrame.X = 2;
            }
            sourceRect = new Rectangle((int)currentFrame.X * FrameWidth(), (int)currentFrame.Y * FrameHeight(), FrameWidth(), FrameHeight());
        }
    }
}
