using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace spaceship
{
    class background
    {

        public Texture2D img1,img2;
        public Vector2 position1,position2,Direction,Speed;
        public Rectangle size1,size2;
        float scale;


        public background(Texture2D im,Texture2D im2,Vector2 pos1)
        {
            img1 = im;
            img2 = im2;
            scale = 1.0f;
            size1=new Rectangle(0,0,(int)(img1.Width*scale),(int)(img2.Height*scale));
            size2 = new Rectangle(0, 0, (int)(img1.Width * scale), (int)(img2.Height * scale));
            position1 = pos1;
            position2 = new Vector2(position1.X + img1.Width, 0);
            Direction = new Vector2(-1, 0);
            Speed=new Vector2(3, 0);
        }


        public void update(GameTime gametime)
        {
            if (position1.X < -size1.Width)
            {
                position1.X = position2.X + size2.Width;
            }
            if ( position2.X < - size2.Width)
            {
                 position2.X =  position1.X + size1.Width;
            }
            position1 += Speed * Direction * (float)(gametime.ElapsedGameTime.TotalSeconds);
            position2 += Speed * Direction * (float)(gametime.ElapsedGameTime.TotalSeconds);
        }

        public void draw(SpriteBatch sp)
        {
            sp.Draw(img1, position1, new Rectangle(0, 0,img1.Width,img1.Height), Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
            sp.Draw(img2, position2, new Rectangle(0, 0,img1.Width,img1.Height), Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

    }
}
