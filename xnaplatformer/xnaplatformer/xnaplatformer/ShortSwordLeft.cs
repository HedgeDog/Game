﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace xnaplatformer
{
    class ShortSwordLeft : AttackAnimation
    {
        public override void LoadContent(ContentManager Content)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            image = content.Load<Texture2D>("ShortSwordRight");
            position = Vector2.Zero;
            drawColor = Color.White;
            frameCounter = 0;
            switchFrame = 50;
            currentFrame = new Vector2(0, 0);
            rotation = 0.0f;
            axis = 0.0f;
            scale = alpha = 1.0f;
            isActive = false;
            text = string.Empty;

            frameDimensions = new List<Vector2>();

            frameDimensions.Add(new Vector2(28, 32));
            frameDimensions.Add(new Vector2(32, 32));
            frameDimensions.Add(new Vector2(28, 32));

            sourceRect = new Rectangle(SourceX(), 0, (int)frameDimensions[(int)currentFrame.X].X, (int)frameDimensions[(int)currentFrame.X].Y);
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

                    if (currentFrame.X >= frameDimensions.Count)
                    {
                        currentFrame.X = frameDimensions.Count - 1;
                        isActive = false;
                    }
                }
            }
            else
            {
                frameCounter = 0;
                currentFrame.X = 0;
            }
            sourceRect = new Rectangle(SourceX(), 0, (int)frameDimensions[(int)currentFrame.X].X, (int)frameDimensions[(int)currentFrame.X].Y);
           offset = new Vector2(-(frameDimensions[(int)currentFrame.X].X - 17), 0);
        }
    }
}
