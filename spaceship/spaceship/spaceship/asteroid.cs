using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace spaceship
{
    class asteroid:earth
    {
        const int timedelay = 10;
        int check = timedelay,xdirection,ydirection;
       
        public asteroid(List<Texture2D> t,int x1,int y1,int indx,int width1,int height1,int xdir,int ydir)//add ttl to constructor
        {
            frames = new List<Texture2D>();
            frames = t;
            index = 0;
            next = 10;
            updatecalls = 0;
            x = x1;
            y = y1;
            index = indx;//get value for start index from level
            width = width1;
            height = height1;
            xdirection = xdir;
            ydirection = ydir;
        }
         private void changeloc()
        {
            if (check == 0)
            {
                check = timedelay;
                x = x + 1 * xdirection;
                y = y + 1 * ydirection;
            }
            else
            {
                check -= 1;
            }
        }

        public override void update()
         {
             changeloc();
             base.update();
         }
       
        public bool checkloc()
        {
            if ((x < -100) || (y < -100) || (x > 900) || (y > 600))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
