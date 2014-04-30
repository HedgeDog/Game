﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace xnaplatformer
{
    public class Entity
    {
        protected int health;
        protected SpriteSheetAnimation moveAnimation;
        protected float moveSpeed;

        protected ContentManager content;
        protected Texture2D tempImage;
        protected Texture2D image;
        protected Vector2 position;
        protected float gravity;
        protected int range;
        protected int direction;
        protected Vector2 destPosition, origPosition;
        protected Vector2 prevPosition, velocity;

        protected bool activateGravity, syncTilePosition;
        protected bool onTile;
        protected List<List<string>> attributes, contents;
        protected List<SpriteSheetAnimation> attackAnimations;
        protected Vector2 animationOffset;


        public Vector2 AnimationOffset
        {
            get { return animationOffset; }
        }
        public bool OnTile
        {
            get { return onTile; }
            set { onTile = value; }
        }

        public int Direction
        {
            get { return direction; }
            set { 
                
                direction = value;
                destPosition.X = (direction == 2) ? destPosition.X = origPosition.X - range : destPosition.X = origPosition.X + range;

            }
        }
        public FloatRect Rect
        {
            get { return new FloatRect(position.X, position.Y, moveAnimation.FrameWidth(), moveAnimation.FrameHeight()); }
        }

        public Vector2 PrevPostion
        {
            get { return prevPosition; }
        }

        public bool ActivateGravity
        {
            set { activateGravity = value; }
        }

        public bool SyncTilePosition
        {
            get { return syncTilePosition; }
            set { syncTilePosition = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public SpriteSheetAnimation Animation
        {
            get { return moveAnimation; }
        }
        public virtual void LoadContent(ContentManager content, List<string> attributes, List<string> contents, InputManager input)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");

            moveAnimation = new SpriteSheetAnimation();
            gravity = 8f;
            velocity = Vector2.Zero;
            syncTilePosition = false;
            activateGravity = true;
            attackAnimations = new List<SpriteSheetAnimation>();
            animationOffset = Vector2.Zero;
            

            for (int i = 0; i < attributes.Count; i++)
            {
                switch (attributes[i])
                {
                    case "Health":
                        health = int.Parse(contents[i]);
                        break;
                    case "Frames":
                        string[] frames = contents[i].Split(' ');
                        moveAnimation.Frames = new Vector2(int.Parse(frames[0]), int.Parse(frames[1]));
                        break;
                    case "Image":
                        image = this.content.Load<Texture2D>(contents[i]);
                        break;
                    case "Position":
                        frames = contents[i].Split(' ');
                        position = new Vector2(int.Parse(frames[0]), int.Parse(frames[1]));
                        break;
                    case "MoveSpeed" :
                        moveSpeed = float.Parse(contents[i]);
                        break;
                    case "Range" :
                        range = int.Parse(contents[i]);
                       break;

                }
            }

            moveAnimation.LoadContent(content, image, "", position);
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime, InputManager input, Collision col, Layer layer)
        {
            syncTilePosition = false;
            prevPosition = position;
        }

        public virtual void OnCollision(Entity e)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
