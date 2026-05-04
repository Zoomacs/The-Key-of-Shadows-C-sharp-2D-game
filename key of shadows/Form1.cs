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
    class actor
    {
        public int x, y;
        public int w, d;
        public int state;
        public int cf;
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
        bool animation = false;
        Bitmap off;
        Timer tt = new Timer();
        actor hero = new actor();
        actor fa7l = new actor();
        actor skearch = new actor();
        actor skespe = new actor();
        actor skewar = new actor();
        actor demon = new actor();
        List<actor> l = new List<actor>();
        public Form1()
        {
            WindowState = FormWindowState.Maximized;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
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
            idle();
            walking();
            attack();
            dead();
            hurt();
            drawdubb(this.CreateGraphics());
        }

        private void Form1_Load1(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

            for (int i = 1; i <= 14; i++)
            {
                if (i <= 8) { hero.walk.Add(new Bitmap("herorun" + i + ".png")); }
                if (i <= 7) { hero.idle.Add(new Bitmap("heroidle" + i + ".png")); }
                if (i <= 4) { hero.attack1.Add(new Bitmap("heromeleeattack" + i + ".png")); }
                if (i <= 8) { hero.attack2.Add(new Bitmap("heroattackfireball" + i + ".png")); }
                { hero.attack3.Add(new Bitmap("heroflameattack" + i + ".png")); }
                if (i <= 9) { hero.jump.Add(new Bitmap("herojump" + i + ".png")); }
                if (i <= 6) { hero.dead.Add(new Bitmap("herodead" + i + ".png")); }
                if (i <= 3) { hero.hurt.Add(new Bitmap("herohurt" + i + ".png")); }
                if (i <= 12) { hero.charge.Add(new Bitmap("herocharge" + i + ".png")); }
            }
            hero.x = 750;
            hero.y = 750 - hero.walk[0].Height;
            hero.state = 0;

            for (int i = 1; i <= 12; i++)
            {
                if (i <= 5) { fa7l.attack1.Add(new Bitmap("attackfa7" + i + ".png")); }
                if (i <= 5) { fa7l.dead.Add(new Bitmap("deadfa7" + i + ".png")); }
                if (i <= 3) { fa7l.hurt.Add(new Bitmap("hurtfa7" + i + ".png")); }
                if (i <= 10) { fa7l.idle.Add(new Bitmap("idlefa7" + i + ".png")); }
                fa7l.walk.Add(new Bitmap("walkfa7" + i + ".png"));
            }
            fa7l.x = 1150;
            fa7l.y = 700 - fa7l.idle[0].Height;
            fa7l.state = -1;

            for (int i = 1; i <= 15; i++)
            {
                skearch.attack1.Add(new Bitmap("skearchattack" + i + ".png"));
                if (i <= 5) { skearch.dead.Add(new Bitmap("skearchdead" + i + ".png")); }
                if (i <= 2) { skearch.hurt.Add(new Bitmap("skearchhurt" + i + ".png")); }
                if (i <= 7) { skearch.idle.Add(new Bitmap("skearchidle" + i + ".png")); }
                if (i <= 8) { skearch.walk.Add(new Bitmap("skearchwalk" + i + ".png")); }
            }

            for (int i = 1; i <= 7; i++)
            {
                skespe.idle.Add(new Bitmap("skespeidle" + i + ".png"));
                skespe.walk.Add(new Bitmap("skespewalk" + i + ".png"));
                if (i <= 6) { skespe.walk.Add(new Bitmap("skesperun" + i + ".png")); }
                if (i <= 5)
                {
                    skespe.dead.Add(new Bitmap("skespedead" + i + ".png"));
                    skespe.attack2.Add(new Bitmap("skesperunattack" + i + ".png"));
                }
                if (i <= 4)
                {
                    skespe.attack1.Add(new Bitmap("skespeattack" + i + ".png"));
                    skespe.charge.Add(new Bitmap("Arrow" + i + ".png"));
                }
                if (i <= 3) { skespe.hurt.Add(new Bitmap("skespehurt" + i + ".png")); }
            }

            for (int i = 1; i <= 7; i++)
            {
                skewar.idle.Add(new Bitmap("skewaridle" + i + ".png"));
                skewar.walk.Add(new Bitmap("skewarwalk" + i + ".png"));
                if (i <= 5) { skewar.attack1.Add(new Bitmap("skewarattack" + i + ".png")); }
                if (i <= 4) { skewar.dead.Add(new Bitmap("skewardead" + i + ".png")); }
                if (i <= 2) { skewar.hurt.Add(new Bitmap("skewarhurt" + i + ".png")); }
            }

            for (int i = 1; i <= 8; i++)
            {
                if (i <= 8) { demon.attack1.Add(new Bitmap("demonattack" + i + ".png")); }
                if (i <= 6) { demon.dead.Add(new Bitmap("demondead" + i + ".png")); }
                if (i <= 4) { demon.walk.Add(new Bitmap("demonfly" + i + ".png")); }
                if (i <= 4) { demon.hurt.Add(new Bitmap("demonhurt" + i + ".png")); }
                if (i <= 4) { demon.idle.Add(new Bitmap("demonideal" + i + ".png")); }
            }

        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            hero.state = 0;
            hero.cf = 0;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                if (hero.state != 1)
                {
                    hero.cf = 0;
                    hero.state = 1;
                }

                if (e.KeyCode == Keys.Left) hero.x -= 10;
                if (e.KeyCode == Keys.Right) hero.x += 10;
                hero.cf++;
                if (hero.cf >= 8)
                {
                    hero.cf = 0;
                }
            }
            if (e.KeyCode == Keys.Q)
            {
                animation = true;
                fa7l.state = 5;
                fa7l.cf = 0;
            }
            if (e.KeyCode == Keys.W)
            {
                animation = true;
                fa7l.state = 3;
                fa7l.cf = 0;
            }
            if (e.KeyCode == Keys.E)
            {
                animation = true;
                fa7l.state = 4;
                fa7l.cf = 0;
            }
            if (e.KeyCode == Keys.R)
            {
                animation = true;
                fa7l.state = 0;
                fa7l.cf = 0;
            }
            if (e.KeyCode == Keys.T)
            {

                animation = true;
                fa7l.state = 1;
                fa7l.cf = 0;
            }
            drawdubb(this.CreateGraphics());
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

            if (hero.idle.Count > 0)
            {
                int frameToDraw = hero.cf;

                if (hero.state == 0)
                {
                    g2.DrawImage(hero.idle[frameToDraw], hero.x, hero.y);
                }
                else if (hero.state == 1)
                {
                    g2.DrawImage(hero.walk[frameToDraw], hero.x, hero.y);
                }
                if (fa7l.state == 0) { g2.DrawImage(fa7l.idle[fa7l.cf], fa7l.x, fa7l.y); }
                else if (fa7l.state == 5) { g2.DrawImage(fa7l.attack1[fa7l.cf], fa7l.x, fa7l.y); }
                else if (fa7l.state == 3) { g2.DrawImage(fa7l.hurt[fa7l.cf], fa7l.x, fa7l.y); }
                else if (fa7l.state == 4) { g2.DrawImage(fa7l.dead[fa7l.cf], fa7l.x, fa7l.y); }
                else if (fa7l.state == 1) { g2.DrawImage(fa7l.walk[fa7l.cf], fa7l.x, fa7l.y); }
                else { g2.DrawImage(fa7l.idle[0], fa7l.x, fa7l.y); }
            }
        }
        void idle()
        {
            if (hero.state == 0)
            {
                hero.cf++;
                if (hero.cf >= 8)
                {
                    hero.cf = 0;
                }
            }

            if (fa7l.state == 0 && animation)
            {
                fa7l.cf++;
                if (fa7l.cf >= 10)
                {
                    fa7l.cf--;
                    animation = false;
                }
            }


        }
        void attack()
        {
            if (fa7l.state == 5 && animation)
            {
                fa7l.cf++;
                if (fa7l.cf >= 5)
                {
                    fa7l.cf--;
                    animation = false;
                }
            }
        }
        void walking()
        {
            if (fa7l.state == 1 && animation)
            {
                fa7l.cf++;
                if (fa7l.cf >= 12)
                {
                    fa7l.cf--;
                    animation = false;
                }
            }
        }
        void hurt()
        {
            if (fa7l.state == 3 && animation)
            {
                fa7l.cf++;
                if (fa7l.cf >= 3)
                {
                    fa7l.cf--;
                    animation = false;
                }
            }
        }
        void dead()
        {
            if (fa7l.state == 4 && animation)
            {
                fa7l.cf++;
                if (fa7l.cf >= 5)
                {
                    fa7l.cf--;
                    animation = false;
                }
            }
        }
    }
}
