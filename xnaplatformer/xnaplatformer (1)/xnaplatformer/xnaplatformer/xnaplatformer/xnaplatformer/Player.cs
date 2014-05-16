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
        

        public override void LoadContent(ContentManager content, List<string> attributes, List<string> contents, InputManager input)
        {
            base.LoadContent(content, attributes, contents, input);
            currentAttackAnimations = new AttackAnimation[4];
            isAttacking = false;
            jumpSpeed = 4;
            health = 50;
            currentWeapon = "ShortSword";
            isHit = false;
            recoilTime = 150;
            hitTimer = recoilTime;
            damage = 5;
            attackRecoil = 100;
            attackRecoilTimer = 0;
            exp = 0;
            
        }
       
        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input, Collision col, Layer layer)
        {
            if (isHit == true)
            {
                hitTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (hitTimer <= 0)
                {
                    hitTimer = recoilTime;
                    isHit = false;
                }
            }



            else
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
                    attackRecoilTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                if (input.KeyPressed(Keys.W) && onTile)
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

                if (input.KeyDown(Keys.Down) && !IsAttacking && attackRecoilTimer == 0)
                {
                    moveAnimation = currentAttackAnimations[2];
                    moveAnimation.LoadContent(content);
                    moveAnimation.IsActive = true;
                    moveAnimation.Effect = tempAnimation.Effect;
                    isAttacking = false;
                    velocity.X = 0;
                }
                else if (input.KeyReleased(Keys.Down) && !IsAttacking)
                {
                    moveAnimation = tempAnimation;
                }

                if (input.KeyPressed(Keys.Right) && attackRecoilTimer == 0)
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



                else if (input.KeyPressed(Keys.Left) && attackRecoilTimer == 0)
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
                else if (input.KeyPressed(Keys.Up) && attackRecoilTimer == 0)
                {
                    if (!isAttacking)
                    {
                        isAttacking = true;
                        moveAnimation = currentAttackAnimations[3];
                        moveAnimation.LoadContent(content);
                        moveAnimation.IsActive = true;
                        moveAnimation.Effect = tempAnimation.Effect;
                    }
                }

                if (input.KeyPressed(Keys.P))
                {
                    equipWeapon();
                }
            }
            if (!(isAttacking && onTile))
            {
                position.X += velocity.X;

            }

            if (attackRecoilTimer != 0)
            {
                attackRecoilTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (attackRecoilTimer >= attackRecoil)
                {
                    attackRecoilTimer = 0;
                }
            }
           
            position.Y += velocity.Y;
            
            moveAnimation.Update(gameTime);
            prevOffset = animationOffset;
            animationOffset = moveAnimation.Offset;
            moveAnimation.Position = position + moveAnimation.Offset;


            Camera.Instance.setFocalPoint(new Vector2(position.X, ScreenManager.Instance.Dimensions.Y / 2));
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
            newClass = Type.GetType("xnaplatformer." + currentWeapon + "Up");
            currentAttackAnimations[3] = (AttackAnimation)Activator.CreateInstance(newClass);
        }

        public override void OnCollision(Entity e)
        {
            Type type = e.GetType();
            if (type == typeof(Enemy))
            {
                if (isAttacking )
                {
                    if (!e.IsHit)
                    {
                       
                    }
                }
                else
                {
                    if (!isHit)
                    {
                        health -= 10;

                        if (e.Position.X > position.X)
                        {
                            velocity.X = -3;
                            velocity.Y -= 1;
                        }
                        else
                        {
                            velocity.X = 3;
                            velocity.Y -= 1;
                        }
                    }

                    isHit = true;

                    

                    moveAnimation.DrawColor = Color.Red;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(content.Load<SpriteFont>("Font1"), velocity.X + ", " + velocity.Y, new Vector2(20, 20), Color.White);
            spriteBatch.DrawString(content.Load<SpriteFont>("Font1"), "" + health, new Vector2(20, 5), Color.Red);
            moveAnimation.Draw(spriteBatch);
        }
    }
}
