using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace spaceship
{
    class debris
    {
        public List<asteroid> asteroids;
        const int maxno = 5;
        int i,x,y,currlevelrow=0,index,width,height,xdir,ydir;
        const int ttl=2000;
        List<Texture2D> tex;
        LevelLibrary.Level leveldetails;

        
        public debris(List<Texture2D> t,LevelLibrary.Level l)
        {
            asteroids = new List<asteroid>(maxno);
            tex = new List<Texture2D>();
            tex = t;
            leveldetails = l;
        }

        public void update()
        {
            for (int i = asteroids.Count; i < 21; i++)
            {
                asteroids.Add(generateasteroid(tex, ttl));
            }
            for (int i = 0; i < asteroids.Count; i++)
            {
                asteroids[i].update();
                if (asteroids[i].checkloc()==true)//change to check if crossed screen
                 {
                    asteroids.RemoveAt(i);
                    i--;
                 }
            }
           
       }

        public void  draw(SpriteBatch sp)
        {
            for (i = 0; i < asteroids.Count; i++)
            {
                asteroids[i].draw(sp);
            }
        }

        private asteroid generateasteroid(List<Texture2D> t,int ttl)
        {
            //get x and y from level info
            if (currlevelrow < leveldetails.Values.GetLength(0))
            {
                x = leveldetails.GetValue(currlevelrow, 0);
                y = leveldetails.GetValue(currlevelrow, 1);
                index = leveldetails.GetValue(currlevelrow, 2);//get index of img from level
                width = leveldetails.GetValue(currlevelrow, 3);
                height=leveldetails.GetValue(currlevelrow, 4);
                xdir = leveldetails.GetValue(currlevelrow, 5);
                ydir = leveldetails.GetValue(currlevelrow, 6);
                currlevelrow += 1;
            }
            else
            {
                currlevelrow = 0;
            }
            return new asteroid(t,x,y,index,width,height,xdir,ydir);//add index
        }

       

    }
}
