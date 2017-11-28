using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace spaceship
{
    class shuttle
    {
        SoundEffect shipsound;
        public Texture2D img;
        public int shuttle_speed=0;
        public int topspeed = 140;
        int up=-1;
        int down=1;
        int left=-1;
        int right=1;
        int acceleration=1;
        int deacceleration = 1;
        public float rotation;
        const float const_left=4.7f;
        const float const_right=14.1f;
        const float const_up=0.0f;
        const float const_down=3.1f;
        const float const_upright=13.5f;
        const float const_upleft=5.5f;
        const float const_downleft=3.8f;
        const float const_downright=14.5f;

        enum State
        {
          moving,idle
        }
        enum movingstate
        {
            up, down, left, right, upleft, upright, downleft, downright,idle
        }
        movingstate mstate;//for directions
        State currentstate ;//for id if moving or not
        public Vector2 direction,speed,position;
        KeyboardState PreviousKeyboardState;

        public shuttle(Texture2D im,SoundEffect shipsnd)
        {
            img = im;
            position=new Vector2(420,420);
            currentstate=State.idle;
            direction=Vector2.Zero;
            speed=Vector2.Zero;
            rotation = 0;
            mstate = movingstate.idle;
            shipsound = shipsnd;
        }

        public void update(GameTime gametime)
        {
            KeyboardState CurrentKeyboardState = Keyboard.GetState();
            if(CurrentKeyboardState.IsKeyUp(Keys.Up)&&CurrentKeyboardState.IsKeyUp(Keys.Left)&&CurrentKeyboardState.IsKeyUp(Keys.Right)&&CurrentKeyboardState.IsKeyUp(Keys.Down))
            {
                currentstate = State.idle;
                mstate=movingstate.idle;
                
            }
            else if (CurrentKeyboardState.IsKeyDown(Keys.Up) || CurrentKeyboardState.IsKeyDown(Keys.Left) || CurrentKeyboardState.IsKeyDown(Keys.Right) || CurrentKeyboardState.IsKeyDown(Keys.Down))
            {
                currentstate = State.moving;
            }
            if ((CurrentKeyboardState.IsKeyDown(Keys.B) == true)&&((speed.Y!=0)||(speed.X!=0)))
            {

                brakeshuttle();
            }
            UpdateMovement(CurrentKeyboardState); 
            PreviousKeyboardState = CurrentKeyboardState; 
            position += direction * speed * (float)gametime.ElapsedGameTime.TotalSeconds;
        }

        public void draw(SpriteBatch sp)
        {
           sp.Draw(img, position, new Rectangle(0, 0, img.Width, img.Height), Color.WhiteSmoke, rotation, new Vector2(img.Width/2,img.Height/2), 0.3f, SpriteEffects.None, 0); 
        }

        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
            
            if ((currentstate==State.moving))
            {
                
                speed = Vector2.Zero;
                direction = Vector2.Zero;
                if ((aCurrentKeyboardState.IsKeyDown(Keys.Left) == true)&&(aCurrentKeyboardState.IsKeyDown(Keys.Up))==true)
                    {
                        if (mstate == movingstate.upleft)
                        {
                            if (shuttle_speed <= topspeed)
                            {
                                shuttle_speed += acceleration;
                            }
                        }
                        rotation = 5.5f;
                        speed.Y = shuttle_speed;
                        speed.X = shuttle_speed;
                        direction.X = left;
                        direction.Y = up;
                         mstate = movingstate.upleft;
                         calibrateshipsound();//takeout
                    }
                else if ((aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)&&(aCurrentKeyboardState.IsKeyDown(Keys.Up)==true))
                    {
                        if (mstate == movingstate.upright)
                        {
                            if (shuttle_speed <= topspeed)
                            {
                                shuttle_speed += acceleration;
                            }
                        }
                        
                        rotation = 13.5f;
                        speed.Y = shuttle_speed;
                        speed.X = shuttle_speed;
                        direction.X = right;
                        direction.Y = up;
                        calibrateshipsound();//takeout
                        mstate = movingstate.upright;
                    }
                else if ((aCurrentKeyboardState.IsKeyDown(Keys.Left) == true)&&(aCurrentKeyboardState.IsKeyDown(Keys.Down)==true))
                    {
                        if (mstate == movingstate.downleft)
                        {
                            if (shuttle_speed <= topspeed)
                            {
                                shuttle_speed += acceleration;
                            }
                        }
                        
                        rotation = 3.8f;
                        speed.Y = shuttle_speed;
                        speed.X = shuttle_speed;
                        direction.X = left;
                        direction.Y = down;
                        calibrateshipsound();//takeout
                        mstate = movingstate.downleft;
                    }
                else if ((aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)&&(aCurrentKeyboardState.IsKeyDown(Keys.Down)==true))
                    {
                        if (mstate == movingstate.downright)
                        {
                            if (shuttle_speed <= topspeed)
                            {
                                shuttle_speed += acceleration;
                            }
                        }
                        
                        rotation = 15.0f;
                        speed.Y = shuttle_speed;
                        speed.X = shuttle_speed;
                        direction.X = right;
                        direction.Y = down;
                        calibrateshipsound();//takeout
                        mstate = movingstate.downright;
                    }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true)
                {
                    if (mstate == movingstate.left)
                    {
                        if (shuttle_speed <= topspeed)
                        {
                            shuttle_speed += acceleration;
                        }
                    }
                    else
                    
                    rotation = 4.7f;
                    speed.X = shuttle_speed;
                    direction.X = left;
                    mstate = movingstate.left;
                    calibrateshipsound();//takeout
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
                {
                    if (mstate == movingstate.right)
                    {
                        if (shuttle_speed <= topspeed)
                        {
                            shuttle_speed += acceleration;
                        }
                    }
                   
                    rotation = 14.1f;
                    speed.X = shuttle_speed;
                    direction.X = right;
                    mstate = movingstate.right;
                    calibrateshipsound();//takeout
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Up) == true)
                {
                    if (mstate == movingstate.up)
                    {
                        if (shuttle_speed <= topspeed)
                        {
                            shuttle_speed += acceleration;
                        }
                    }
                   
                     speed.Y = shuttle_speed;
                    direction.Y = up;
                    rotation = 0.0f;
                    mstate = movingstate.up;
                    calibrateshipsound();//takeout
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Down) == true)
                {
                    if (mstate == movingstate.down)
                    {
                        if (shuttle_speed <= topspeed)
                        {
                            shuttle_speed += acceleration;
                        }
                    }
                   
                    speed.Y = shuttle_speed;
                    direction.Y = down;
                    rotation = 3.1f;
                    mstate = movingstate.down;
                    calibrateshipsound();//takeout
                }
            }
           

        }

        private void brakeshuttle()
        {
            if (speed.Y > 0)
            {
                speed.Y -= deacceleration;
            }
            else if (speed.Y < 0)
            {
                speed.Y += deacceleration;
            }
            if (speed.X > 0)
            {
                speed.X -= deacceleration;
            }
            else if (speed.X < 0)
            {
                speed.X += deacceleration;
            }
            if (shuttle_speed != 0)
            {
                shuttle_speed -= deacceleration;
            }
        }

        private void calibrateshipsound()
        {
            float pan;
            if (this.position.X < 400)
            {
                /*420=0.0
                410=-0.025
                 430=0.025*/
                pan = ((this.position.X / 10.0f) * 0.025f)-1;
            }
            else
            {
                pan = (((this.position.X-400)/10.0f) * 0.025f);
            }
            shipsound.Play(0.3f, 0.0f, pan);
        }

    }
}
