using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace spaceship
{
    public class earth
    {
         protected List<Texture2D> frames;
         protected int index,updatecalls,next,x,y,width,height;
         protected Vector2 position;
        
        public earth(List<Texture2D> t)
        {
            frames = new List<Texture2D>();
            frames = t;
            index = 0;
            next = 10;
            updatecalls = 0;
            position = new Vector2(350, 200);
            x = Convert.ToInt32(position.X);
            y = Convert.ToInt32(position.Y);
            width = 100;
            height = 100;
        }
        public earth()
        {

        }

        public virtual void update()
        {
            if (updatecalls == next)
            {
                if ((index + 1) == frames.Count)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
                updatecalls = 0;
            }
            updatecalls++;
        }

        public void draw(SpriteBatch sp)
        {
          sp.Draw(frames[index], new Rectangle(x, y,width,height), Color.White);
           
        }
    }
}
