using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace xnaplatformer
{
    public class Player : Entity
    {
        private float jumpSpeed;
        private string currentWeapon;
        private AttackAnimation[] currentAttackAnimations;
        private bool isAttacking;
        private SpriteSheetAnimation tempAnimation;
        
        public bool IsAttacking
        {
            get { return isAttacking; }
        }

        public override void LoadContent(ContentManager content, List<string> attributes, List<string> contents, InputManager input)
        {
            base.LoadContent(content, attributes, contents, input);
            currentAttackAnimations = new AttackAnimation[4];
            isAttacking = false;
            jumpSpeed = 5;
            currentWeapon = "ShortSword";

            tempAnimation = moveAnimation;
        }
       
        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input, Collision col, Layer layer)
        {
            base.Update(gameTime, input, col, layer);
            moveAnimation.DrawColor = Color.White;
            if (!isAttacking)
            {
                if (input.KeyDown(Keys.D))
                {
                    moveAnimation.IsActive = true;
                    moveAnimation.Effect = SpriteEffects.None;
                    velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                else if (input.KeyDown(Keys.A))
                {
                    moveAnimation.Effect = SpriteEffects.FlipHorizontally;
                    moveAnimation.IsActive = true;
                    velocity.X = -1 * moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    if (!isAttacking)
                    {
                        moveAnimation.IsActive = false;
                        moveAnimation.CurrentFrame = new Vector2(2, 0);
                    }
                    velocity.X = 0;
                }
            }

           
            if (isAttacking && moveAnimation.IsActive == false)
            {
                isAttacking = false;
                moveAnimation = tempAnimation;
            }
            if(input.KeyPressed(Keys.W) && onTile)
            {
                velocity.Y = -jumpSpeed;
                onTile = false;
            }

            if (activateGravity)
            {
                velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                //velocity.Y = 0;
            }

            if (input.KeyDown(Keys.Down) && !IsAttacking)
            {
                moveAnimation = currentAttackAnimations[2];
                moveAnimation.LoadContent(content);
                moveAnimation.IsActive = true;
                moveAnimation.Effect = tempAnimation.Effect;
                isAttacking = false;
                velocity.X = 0;
            }
            else if (input.KeyReleased(Keys.Down))
            {
                moveAnimation = tempAnimation;
            }

            if (input.KeyPressed(Keys.Right))
            {
                if (!isAttacking)
                {
                    isAttacking = true;
                    moveAnimation = currentAttackAnimations[0];
                    moveAnimation.LoadContent(content);
                    moveAnimation.IsActive = true;
                    moveAnimation.Effect = SpriteEffects.None;
                }
            }

            
            
            else if (input.KeyPressed(Keys.Left))
            {
                if (!isAttacking)
                {
                    isAttacking = true;
                    moveAnimation = currentAttackAnimations[1];
                    moveAnimation.LoadContent(content);
                    moveAnimation.IsActive = true;
                    moveAnimation.Effect = SpriteEffects.FlipHorizontally;
                }
            }

            
            if (input.KeyPressed(Keys.P))
            {
                equipWeapon();
            }
            position.X += velocity.X;
            position.Y += velocity.Y;
            
            moveAnimation.Update(gameTime);
            animationOffset = moveAnimation.Offset;
            moveAnimation.Position = position + moveAnimation.Offset;
        }

        public void equipWeapon()
        {
            this.currentWeapon = "ShortSword";
            Type newClass = Type.GetType("xnaplatformer." + currentWeapon + "Right");
            currentAttackAnimations[0] = (AttackAnimation)Activator.CreateInstance(newClass);
            newClass = Type.GetType("xnaplatformer." + currentWeapon + "Left");
            currentAttackAnimations[1] = (AttackAnimation)Activator.CreateInstance(newClass);
            newClass = Type.GetType("xnaplatformer." + currentWeapon + "Block");
            currentAttackAnimations[2] = (AttackAnimation)Activator.CreateInstance(newClass);

        }

        public override void OnCollision(Entity e)
        {
            Type type = e.GetType();
            if (type == typeof(Enemy))
            {
                health--;
                moveAnimation.DrawColor = Color.Red;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(content.Load<SpriteFont>("Font1"), velocity.X + ", " + velocity.Y, new Vector2(20, 20), Color.White);
            moveAnimation.Draw(spriteBatch);
        }
    }
}
