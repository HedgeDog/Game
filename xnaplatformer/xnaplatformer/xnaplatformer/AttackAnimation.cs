using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace xnaplatformer
{
    public class AttackAnimation : SpriteSheetAnimation
    {
        protected List<Vector2> frameDimensions;

        public int SourceX()
        {
            int total = 0;
            for (int i = 0; i < currentFrame.X; i++)
            {
                total += (int)frameDimensions[i].X;
            }
            return total;
        }

        public override int FrameWidth()
        {
            return sourceRect.Width;
        }

        public override int FrameHeight()
        {
            return sourceRect.Height;
        }

        public override void LoadContent(ContentManager Content)
        {

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
                currentFrame.X = 0;
            }
            sourceRect = new Rectangle((int)currentFrame.X * FrameWidth(), (int)currentFrame.Y * FrameHeight(), FrameWidth(), FrameHeight());
        }
    
    }
}
