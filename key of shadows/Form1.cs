using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace key_of_shadows
{
    class ball
    {
        public int x, y;
        public int w = 0, d;
        public int cf;
        public List<Bitmap> charge = new List<Bitmap>();
    }
    class actor
    {
        public int x, y;
        public int w, d;
        public int state;
        public int cf;
        public int health;
        public bool gothit = false;
        public bool isdead = false;
        public List<Bitmap> idle = new List<Bitmap>();
        public List<Bitmap> walk = new List<Bitmap>();
        public List<Bitmap> jump = new List<Bitmap>();
        public List<Bitmap> attack1 = new List<Bitmap>();
        public List<Bitmap> attack2 = new List<Bitmap>();
        public List<Bitmap> attack3 = new List<Bitmap>();
        public List<Bitmap> hurt = new List<Bitmap>();
        public List<Bitmap> dead = new List<Bitmap>();
        public List<Bitmap> charge = new List<Bitmap>();
    }
    public partial class Form1 : Form
    {
        int count = 0;
        bool laseron = false;
        bool isattack = false;
        Bitmap off;
        Timer tt = new Timer();
        actor hero = new actor();
        actor fa7l = new actor();
        actor skearch = new actor();
        actor skespe = new actor();
        actor skewar = new actor();
        actor demon = new actor();
        List<ball> fireball = new List<ball>();
        public Form1()
        {
            WindowState = FormWindowState.Maximized;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            MouseDown += Form1_MouseDown;
            MouseUp += Form1_MouseUp;
            Paint += Form1_Paint;
            Load += Form1_Load1;
            BackColor = Color.LightBlue;
            tt.Tick += Tt_Tick; ;
            tt.Interval = 100;
            tt.Start();
        }


        private void Tt_Tick(object sender, EventArgs e)
        {
            count++;
            manageanimation(hero);
            managechargeanimation();
            manageanimation(fa7l);
            drawdubb(this.CreateGraphics());
            enemyhitcheck();
        }

        private void Form1_Load1(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

            for (int i = 1; i <= 14; i++)
            {
                if (i <= 7) { hero.idle.Add(new Bitmap("hero\\idle\\heroidle" + i + ".png")); }
                if (i <= 8) { hero.walk.Add(new Bitmap("hero\\walk\\herorun" + i + ".png")); }
                if (i <= 4) { hero.attack1.Add(new Bitmap("hero\\attack1\\heromeleeattack" + i + ".png")); }
                if (i <= 8) { hero.attack2.Add(new Bitmap("hero\\attack2\\heroattackfireball" + i + ".png")); }
                { hero.attack3.Add(new Bitmap("hero\\attack3\\heroflameattack" + i + ".png")); }
                if (i <= 9) { hero.jump.Add(new Bitmap("hero\\jump\\herojump" + i + ".png")); }
                if (i <= 6) { hero.dead.Add(new Bitmap("hero\\dead\\herodead" + i + ".png")); }
                if (i <= 3) { hero.hurt.Add(new Bitmap("hero\\hurt\\herohurt" + i + ".png")); }
            }
            hero.x = 750;
            hero.y = 723 - hero.walk[0].Height;
            hero.state = 0;

            for (int i = 1; i <= 12; i++)
            {
                if (i <= 5) { fa7l.attack1.Add(new Bitmap("fa7l\\attack\\attackfa7l" + i + ".png")); }
                if (i <= 5) { fa7l.dead.Add(new Bitmap("fa7l\\dead\\deadfa7l" + i + ".png")); }
                if (i <= 3) { fa7l.hurt.Add(new Bitmap("fa7l\\hurt\\hurtfa7l" + i + ".png")); }
                if (i <= 10) { fa7l.idle.Add(new Bitmap("fa7l\\idle\\idlefa7l" + i + ".png")); }
                fa7l.walk.Add(new Bitmap("fa7l\\walk\\walkfa7l" + i + ".png"));
            }
            fa7l.x = 1150;
            fa7l.y = 723 - fa7l.walk[0].Height;
            fa7l.state = 0;
            fa7l.health = 11;

            for (int i = 1; i <= 15; i++)
            {
                skearch.attack1.Add(new Bitmap("skearch\\attack\\skearchattack" + i + ".png"));
                if (i <= 5) { skearch.dead.Add(new Bitmap("skearch\\dead\\skearchdead" + i + ".png")); }
                if (i <= 2) { skearch.hurt.Add(new Bitmap("skearch\\hurt\\skearchhurt" + i + ".png")); }
                if (i <= 7) { skearch.idle.Add(new Bitmap("skearch\\idle\\skearchidle" + i + ".png")); }
                if (i <= 8) { skearch.walk.Add(new Bitmap("skearch\\walk\\skearchwalk" + i + ".png")); }
            }

            for (int i = 1; i <= 7; i++)
            {
                skespe.idle.Add(new Bitmap("skespe\\idle\\skespeidle" + i + ".png"));
                skespe.walk.Add(new Bitmap("skespe\\walk\\skespewalk" + i + ".png"));
                if (i <= 6) { skespe.walk.Add(new Bitmap("skespe\\run\\skesperun" + i + ".png")); }
                if (i <= 5)
                {
                    skespe.dead.Add(new Bitmap("skespe\\dead\\skespedead" + i + ".png"));
                    skespe.attack2.Add(new Bitmap("skespe\\attack+run\\skesperunattack" + i + ".png"));
                }
                if (i <= 4)
                {
                    skespe.attack1.Add(new Bitmap("skespe\\attack\\skespeattack" + i + ".png"));
                    skespe.charge.Add(new Bitmap("skespe\\Arrow.png"));
                }
                if (i <= 3) { skespe.hurt.Add(new Bitmap("skespe\\hurt\\skespehurt" + i + ".png")); }
            }

            for (int i = 1; i <= 7; i++)
            {
                skewar.idle.Add(new Bitmap("skewar\\idle\\skewaridle" + i + ".png"));
                skewar.walk.Add(new Bitmap("skewar\\walk\\skewarwalk" + i + ".png"));
                if (i <= 5) { skewar.attack1.Add(new Bitmap("skewar\\attack\\skewarattack" + i + ".png")); }
                if (i <= 4) { skewar.dead.Add(new Bitmap("skewar\\dead\\skewardead" + i + ".png")); }
                if (i <= 2) { skewar.hurt.Add(new Bitmap("skewar\\hurt\\skewarhurt" + i + ".png")); }
            }

            for (int i = 1; i <= 8; i++)
            {
                if (i <= 8) { demon.attack1.Add(new Bitmap("demon\\attack\\demonattack" + i + ".png")); }
                if (i <= 6) { demon.dead.Add(new Bitmap("demon\\dead\\demondead" + i + ".png")); }
                if (i <= 4) { demon.walk.Add(new Bitmap("demon\\fly\\demonfly" + i + ".png")); }
                if (i <= 4) { demon.hurt.Add(new Bitmap("demon\\hurt\\demonhurt" + i + ".png")); }
                if (i <= 4) { demon.idle.Add(new Bitmap("demon\\idle\\demonideal" + i + ".png")); }
            }

        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (hero.state == 1)
            {
                hero.state = 0;
                hero.cf = 0;
            }

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.D)
            {    
                if (hero.state != 1)
                {
                    hero.cf = 0;
                    hero.state = 1;
                }

                if (e.KeyCode == Keys.A) hero.x -= 10;
                if (e.KeyCode == Keys.D) hero.x += 10;
                hero.cf++;
                if (hero.cf >= 8)
                {
                    hero.cf = 0;
                }
            }
            if (e.KeyCode == Keys.E)
            {
                isattack = true;
                hero.state = 6;
            }

        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
           if (e.Button == MouseButtons.Right)
            {
                laseron = false;
            }
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isattack = true;
                hero.state = 5;
                hero.cf = 0;
            }
            if (e.Button == MouseButtons.Right)
            {
                isattack = true;
                laseron = true;
                hero.state = 7;
                hero.cf = 0;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            drawdubb(e.Graphics);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        void drawdubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            drawscene(g2);
            g.DrawImage(off, 0, 0);
        }

        void drawscene(Graphics g2)
        {
            g2.Clear(BackColor);
            SolidBrush br = new SolidBrush(Color.Black);
            g2.FillRectangle(br, 0, 750, ClientSize.Width, 800);
            if (fireball.Count > 0)
            {
                for(int i = 0; i < fireball.Count; i++)
                {
                    g2.DrawImage(fireball[i].charge[fireball[i].cf], fireball[i].x, fireball[i].y);
                }
            }
            if (hero.idle.Count > 0)
            {
                int frameToDraw = hero.cf;

                if (hero.state == 0 && hero.idle.Count > frameToDraw)
                {
                    g2.DrawImage(hero.idle[frameToDraw], hero.x, hero.y);
                }
                else if (hero.state == 1)
                {
                    g2.DrawImage(hero.walk[frameToDraw], hero.x, hero.y);
                }
                else if (hero.state == 5)
                {
                    g2.DrawImage(hero.attack1[frameToDraw], hero.x, hero.y);
                }
                else if (hero.state == 6)
                {
                    g2.DrawImage(hero.attack2[frameToDraw], hero.x, hero.y);
                }
                else if (hero.state == 7)
                {
                    g2.DrawImage(hero.attack3[frameToDraw], hero.x, hero.y);
                }
                if (fa7l.state == 0) { g2.DrawImage(fa7l.idle[fa7l.cf], fa7l.x, fa7l.y); }
                else if (fa7l.state == 1) { g2.DrawImage(fa7l.walk[fa7l.cf], fa7l.x, fa7l.y); }
                else if (fa7l.state == 3) { g2.DrawImage(fa7l.hurt[fa7l.cf], fa7l.x, fa7l.y); }
                else if (fa7l.state == 4) { g2.DrawImage(fa7l.dead[fa7l.cf], fa7l.x, fa7l.y); }
                else if (fa7l.state == 5) { g2.DrawImage(fa7l.attack1[fa7l.cf], fa7l.x, fa7l.y); }
                else { g2.DrawImage(fa7l.idle[0], fa7l.x, fa7l.y); }
            }
        }
        
        void manageanimation(actor actor)
        {
            if (actor.state == 0)
            {
                actor.cf++;
                if (actor.cf >= actor.idle.Count)
                {
                    actor.cf = 0;
                }
            }
            else if (actor.state == 1)
            {
                actor.cf++;
                if (actor.cf >= actor.walk.Count)
                {
                    actor.cf--;
                }
            }
            else if (actor.state == 3)
            {
                actor.cf++;
                if (actor.cf >= actor.hurt.Count)
                {
                    actor.cf = 0;
                    actor.state = 0;
                }
            }
            else if (actor.state == 4)
            {
                actor.cf++;
                if (actor.cf >= actor.dead.Count)
                {
                    actor.cf = actor.dead.Count - 1;
                }
            }
            else if (actor.state == 5)
            {
                actor.cf++;
                if (actor.cf >= actor.attack1.Count)
                {
                    actor.cf = 0;
                    actor.state = 0;
                    fa7l.gothit = false;
                }
            }
            else if (actor.state == 6)
            {
                actor.cf++;
                if (actor.cf >= actor.attack2.Count)
                {
                    ball pnn = new ball();
                    pnn.x = hero.x + hero.attack2[hero.attack2.Count-1].Width;
                    pnn.y = hero.y ;
                    for(int i = 1; i <= 12 ; i++)
                    {
                        pnn.charge.Add(new Bitmap("hero\\charge\\herocharge" + i + ".png"));
                    }
                    fireball.Add(pnn);
                    actor.cf = 0;
                    actor.state = 0;
                }
            }
            else if (actor.state == 7)
            {
                actor.cf++;
                if (actor.cf >= actor.attack3.Count-3)
                {
                    if (laseron)
                    {
                        actor.cf = 6;
                    }
                    else
                    {
                        actor.cf = 0;
                        actor.state = 0;
                    }

                }
            }
        }
        void managechargeanimation()
        {
            if(fireball.Count > 0)
            {
                for(int i = 0; i < fireball.Count; i++)
                {
                    ball ptrv = fireball[i];
                    ptrv.x += 10;
                    ptrv.cf++;
                    if ((ptrv.x + ptrv.charge[ptrv.cf].Width) > fa7l.x -30 && fa7l.x > ptrv.x)
                    {
                        ptrv.cf = 6;
                    }
                    else if(ptrv.cf >= ptrv.charge.Count-7)
                    {
                        ptrv.cf = 0;
                    }

                    if (ptrv.w == 20 || ptrv.cf == ptrv.charge.Count)
                    {
                        fireball.RemoveAt(i);
                        i--;
                    }
                    ptrv.w++;
                }
            }
        }
        
        void enemyhitcheck()
        {
            if (hero.state == 5 && (hero.x + hero.attack1[hero.cf].Width >= fa7l.x) && fa7l.x > hero.x && isattack)
            {
                if (!fa7l.gothit)
                {
                    fa7l.health -= 10;
                    fa7l.gothit = true;
                    deadcheck(fa7l);
                }
            }
            else if(hero.state == 7 && (hero.x + hero.attack3[hero.cf].Width >= fa7l.x-30) && fa7l.x > hero.x && isattack)
            {

                fa7l.health -= 1;
                fa7l.gothit = true;
                deadcheck(fa7l);
            }
            if (fireball.Count > 0)
            {
                for (int i = 0; i < fireball.Count; i++)
                {
                    if ((fireball[i].x + fireball[i].charge[fireball[i].cf].Width) > fa7l.x && fa7l.x > fireball[i].x)
                    {
                        fa7l.health -= 5;
                        fa7l.gothit = true;
                        deadcheck(fa7l);
                    }
                }
            }
            
        }
        void deadcheck(actor enemy)
        {
            if (enemy.health > 0)
            {
                enemy.state = 3;
            }
            else if (enemy.health < 0)
            {
                enemy.state = 4;
                enemy.cf = 0;
            }
        }
    }
}
