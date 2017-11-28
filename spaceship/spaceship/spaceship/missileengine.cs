using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace spaceship
{
    public class missileengine
    {
        SoundEffect missilefire;
        bool updatecalled;
        public Vector2 EmitterLocation { get; set; }
        public List<missile> missiles;
        private List<ParticleEngine> mp;
        private Texture2D texture;
        public float angle;
        Vector2 velocity;
        private List<Texture2D> particle;
        public int usedmissiles=0;
        bool emission;
        

         public missileengine(Texture2D textures, Vector2 location,float ang,List<Texture2D> particles,SoundEffect misslfire)
        {
            updatecalled = false;
            EmitterLocation = location;
            this.texture = textures;
            this.missiles = new List<missile>();
            this.mp = new List<ParticleEngine>();
            angle = ang;
            particle = particles;
            missilefire = misslfire;
            emission = true;
           
        }
         public missileengine(Texture2D textures, Vector2 location, float ang, SoundEffect misslfire)
         {
             updatecalled = false;
             EmitterLocation = location;
             this.texture = textures;
             this.missiles = new List<missile>();
             this.mp = new List<ParticleEngine>();
             angle = ang;
             particle = null;
             missilefire = misslfire;
             emission = false;

         }

         private missile GenerateNewMissile(Vector2 vel)
         {            
             Vector2 position = EmitterLocation;
             float size = 0.03f;
             int ttl = 600;
             velocity = vel;
             return new missile(texture, position, velocity, angle, Color.White, size, ttl);
         }

         public void Update(Vector2 vel)
         {
             updatecalled = true;
                 missiles.Add(GenerateNewMissile(vel));
                 usedmissiles += 1;
                 if (emission == true)
                 {
                     mp.Add(new ParticleEngine(particle, EmitterLocation, 0));
                 }
                 for (int missile = 0; missile < missiles.Count; missile++)
                 {
                     missiles[missile].Update();
                     if (emission == true)
                     {
                         missileparticleloc(missile);
                         mp[missile].Update();
                     }
                     if (missiles[missile].TTL <= 0)
                     {
                         missiles.RemoveAt(missile);
                         if (emission == true)
                         {
                             mp.RemoveAt(missile);
                         }
                         missile--;
                     }
                 }
             
            
         }

         private void missileparticleloc(int i)
         {
             if (missiles[i].Angle == 4.7f)
             {
                 //left
                 mp[i].EmitterLocation = new Vector2(missiles[i].Position.X+65,missiles[i].Position.Y-15);
             }
             else if (missiles[i].Angle == 14.1f)
             {
                 //right
                 mp[i].EmitterLocation = new Vector2(missiles[i].Position.X-65, missiles[i].Position.Y + 18);
             }
             else if (missiles[i].Angle == 0.0f)
             {
                 //up
                 mp[i].EmitterLocation = new Vector2(missiles[i].Position.X+18 , missiles[i].Position.Y + 60);
             }
            else if(missiles[i].Angle==3.1f)
             {
            //down
                 mp[i].EmitterLocation = new Vector2(missiles[i].Position.X - 18, missiles[i].Position.Y - 65);
             }
             else if (missiles[i].Angle == 5.5f)
             {
                 //upleft
                 mp[i].EmitterLocation = new Vector2(missiles[i].Position.X + 58, missiles[i].Position.Y + 36);
             }
             else if (missiles[i].Angle == 13.5f)
             {
                 //upright
                 mp[i].EmitterLocation = new Vector2(missiles[i].Position.X -45, missiles[i].Position.Y + 55);
             }
             else if (missiles[i].Angle == 3.8f)
             {
                 //downleft
                 mp[i].EmitterLocation = new Vector2(missiles[i].Position.X + 30, missiles[i].Position.Y - 60);
             }
             else if (missiles[i].Angle == 15.0f)
             {
                 //downright
                 mp[i].EmitterLocation = new Vector2(missiles[i].Position.X - 55, missiles[i].Position.Y - 40);
             }

         }

         public void Draw(SpriteBatch spriteBatch)
         {
             if (updatecalled == true)
             {
                 missilefire.Play(1.0f, -0.5f, 0.0f);
                 updatecalled = false;
             }
             for (int index = 0; index < missiles.Count; index++)
             {
                 spriteBatch.Begin();
                 missiles[index].Draw(spriteBatch);
                 spriteBatch.End();
                 if (emission == true)
                 {
                     mp[index].Draw(spriteBatch);
                 }
             }
             
         }

         public void missiledelete()
         {
             for (int missile = 0; missile < missiles.Count; missile++)
             {
                 missiles[missile].Update();
                 if (emission == true)
                 {
                     missileparticleloc(missile);
                     mp[missile].Update();
                 }
                 if (missiles[missile].TTL <= 0)
                 {
                     missiles.RemoveAt(missile);
                     if (emission == true)
                     {
                         mp.RemoveAt(missile);
                     }
                     missile--;
                 }
             }
         }
    }
}
