using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace xnaplatformer
{
    public class Tile
    {
        public enum State { Solid, Passive };
        public enum Motion { Static, Horizontal, Vertical };

        State state;
        Motion motion;
        Vector2 position, prevPosition, velocity;
        Texture2D tileImage;

        float range;
        int counter;
        bool increase;
        float moveSpeed;
        bool containsEntity;

        Animation animation;

        private Texture2D CropImage(Texture2D tileSheet, Rectangle tileArea)
        {
            Texture2D croppedImage = new Texture2D(tileSheet.GraphicsDevice, tileArea.Width, tileArea.Height);

            Color[] tileSheetData = new Color[tileSheet.Width * tileSheet.Height];
            Color[] croppedImageData = new Color[croppedImage.Width * croppedImage.Height];

            tileSheet.GetData<Color>(tileSheetData);

            int index = 0;
            for (int y = tileArea.Y; y < tileArea.Y + tileArea.Height; y++)
            {
                for (int x = tileArea.X; x < tileArea.X + tileArea.Width; x++)
                {
                    croppedImageData[index] = tileSheetData[y * tileSheet.Width + x];
                    index++;
                }
            }

            croppedImage.SetData<Color>(croppedImageData);

            return croppedImage;
        }

        public void SetTile(State state, Motion motion, Vector2 position, Texture2D tileSheet, Rectangle tileArea)
        {
            this.state = state;
            this.motion = motion;
            this.position = position;
            increase = true;
            containsEntity = false;

            tileImage = CropImage(tileSheet, tileArea);
            range = 50;
            counter = 0;
            moveSpeed = 100f;
            animation = new Animation();
            animation.LoadContent(ScreenManager.Instance.Content, tileImage, "", position);
            velocity = Vector2.Zero;
        }

        public void Update(GameTime gameTime)
        {
            counter++;
            prevPosition = position;
            if (counter >= range)
            {
                counter = 0;
                increase = !increase;
            }

            if (motion == Motion.Horizontal)
            {
                if (increase)
                    velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    velocity.X = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (motion == Motion.Vertical)
            {
                if (increase)
                    velocity.Y = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    velocity.Y = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            position += velocity;
            animation.Position = position;



        }

        public void UpdateCollision(ref Entity e)
        {
            FloatRect rect = new FloatRect(position.X, position.Y, Layer.TileDimensions.X, Layer.TileDimensions.Y);

            FloatRect preve = new FloatRect(e.PrevPostion.X, e.PrevPostion.Y, e.TempAnimation.FrameWidth(), e.TempAnimation.FrameHeight());
            FloatRect prevTile = new FloatRect(prevPosition.X, prevPosition.Y, Layer.TileDimensions.X, Layer.TileDimensions.Y);

            if (e.RecTile.intersects(rect) && state == State.Solid)
            {


                if (e.RecTile.Bottom >= rect.Top && preve.Bottom <= prevTile.Top)
                {
                    {
                        e.Position = new Vector2(e.Position.X, position.Y - e.TempAnimation.FrameHeight());
                        e.ActivateGravity = false;
                        e.OnTile = true;
                        e.Velocity = new Vector2(e.Velocity.X, 0);
                        containsEntity = true;
                    }
                }
                else if (e.RecTile.Top <= rect.Bottom && preve.Top >= prevTile.Bottom)
                {
                    e.Position = new Vector2(e.Position.X, e.Position.Y + Layer.TileDimensions.Y);
                    e.Velocity = new Vector2(e.Velocity.X, 0);
                    e.ActivateGravity = true;
                }

                else if (e.RecTile.Right >= rect.Left && e.Velocity.X > 0)
                {
                    e.Position = new Vector2(position.X - e.TempAnimation.FrameWidth() - 3, e.Position.Y);
                    e.Velocity = new Vector2(0, e.Velocity.Y);
                    if (e.Direction == 1)
                        e.Direction = 2;
                    else if (e.Direction == 2)
                        e.Direction = 1;
                    //e.ActivateGravity = true;

                }
                else if (e.RecTile.Left <= rect.Right && preve.Left >= prevTile.Left)
                {
                    e.Position = new Vector2(position.X + Layer.TileDimensions.X + 3, e.Position.Y);
                    e.Velocity = new Vector2(0, e.Velocity.Y);
                    if (e.Direction == 1)
                        e.Direction = 2;
                    else if (e.Direction == 2)
                        e.Direction = 1;
                    //e.ActivateGravity = true;
                }
            }
            else
            {
                e.ActivateGravity = true;
            }

            e.Animation.Position = e.Position + e.AnimationOffset;

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}