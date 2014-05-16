using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace xnaplatformer
{
    class Enemy : Entity
    {
        int rangeCounter;
        public override void LoadContent(ContentManager content, List<string> attributes, List<string> contents, InputManager input)
        {
            base.LoadContent(content, attributes, contents, input);
            rangeCounter = 0;
            direction = 1;
            moveAnimation.IsActive = true;
            origPosition = position;
            hitTimer = 0;
            recoilTime = 200;
            exp = 10;

            if (direction == 1)
            {
                destPosition.X = origPosition.X + range;
            }
            else
            {
                destPosition.X = origPosition.X - range;    
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input, Collision col, Layer layer)
        {
            base.Update(gameTime, input, col, layer);
            if (direction == 1)
            {
                velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 2);
            }
            else if (direction == 2)
            {
                velocity.X = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);
            }

            if (activateGravity)
            {
                velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                //velocity.Y = 0;
            }

            position += velocity;

            if (direction == 1 && position.X >= destPosition.X)
            {
                direction = 2;
                destPosition.X = origPosition.X - range;
            }
            else if (direction == 2 && position.X <= destPosition.X)
            {
                direction = 1;
                destPosition.X = origPosition.X + range;
            }

            if (isHit)
            {
                hitTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (hitTimer >= recoilTime)
                {
                    isHit = false;
                    hitTimer = 0;
                    this.moveAnimation.DrawColor = Color.White;
                }
            }

            moveAnimation.Update(gameTime);
            moveAnimation.Position = position;
        }

        public override void OnCollision(Entity e)
        {
            if (e.IsAttacking)
            {
                if (!isHit)
                {
                    Health = Health - e.Damage;
                    isHit = true;
                    this.moveAnimation.DrawColor = Color.Red;
                }

                if (health <= 0)
                {
                    e.Exp += exp;
                }
            }
        }

        public override void OnDeath()
        {
            this.UnloadContent();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(content.Load<SpriteFont>("Font1"), "" + health, new Vector2(20, 45), Color.Red);
            moveAnimation.Draw(spriteBatch);
        }
    }
}
