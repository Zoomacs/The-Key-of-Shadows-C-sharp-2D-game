using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace key_of_shadows
{
    class tiles
    {
        public int X, Y, W, H;
        public Bitmap img;
    }

    class ball
    {
        public int X, Y;
        public int W, D;
        public int IF;
        public List<Bitmap> Charge = new List<Bitmap>();
    }
    class CActor
    {
        public int X, Y;
        public int W, D;
        public int State;
        public int IF;
        public int Health;
        public bool GotHit = false;
        public bool isDead = false;
        public Bitmap Img;
        public List<Bitmap> Imgs = new List<Bitmap>();
        public List<Bitmap> Idle = new List<Bitmap>();
        public List<Bitmap> Walk = new List<Bitmap>();
        public List<Bitmap> Jump = new List<Bitmap>();
        public List<Bitmap> Climb = new List<Bitmap>();
        public List<Bitmap> Attack1 = new List<Bitmap>();
        public List<Bitmap> Attack2 = new List<Bitmap>();
        public List<Bitmap> Attack3 = new List<Bitmap>();
        public List<Bitmap> Hurt = new List<Bitmap>();
        public List<Bitmap> Dead = new List<Bitmap>();
        public List<Bitmap> Charge = new List<Bitmap>();
        public int[] Xadd;
        public int Floor;
    }
    public partial class Form1 : Form
    {

        bool LaserOn = false;
        bool isAttack = false;
        Bitmap off;
        Timer tt = new Timer();
        CActor Hero = new CActor();
        CActor Fa7l = new CActor();
        CActor Skearch = new CActor();
        CActor Skespe = new CActor();
        CActor Skewar = new CActor();
        CActor Demon = new CActor();
        List<CActor> Ladder = new List<CActor>();
        List<ball> Fireball = new List<ball>();
        List<tiles> Floor = new List<tiles>();
        List<tiles> Walls = new List<tiles>();
        CActor Background = new CActor();
        int BStartX, BStartY;     //1300 1510
        bool keyW = false;
        bool keyA = false;
        bool keyS = false;
        bool keyD = false;
        int ctJump, JumpLimit = 8;
        int Speed = 50;
        public Form1()
        {
            WindowState = FormWindowState.Maximized;
            // FormBorderStyle = FormBorderStyle.None;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            MouseDown += Form1_MouseDown;
            MouseUp += Form1_MouseUp;
            Paint += Form1_Paint;
            Load += Form1_Load;
            BackColor = Color.Orange;
            tt.Tick += Tt_Tick; ;
            tt.Interval = 100;
            tt.Start();
        }


        private void Tt_Tick(object sender, EventArgs e)
        {
            ManageHeroMove();

            ManageActorAnimation(Hero);
            ManageActorAnimation(Fa7l);
            ManageChargeAnimation();

            Jump();

            CheckEnemyHit();
            DrawDubb(CreateGraphics());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            CreateFloor();
            Createground();
            Createhero();
            Createfa7l();
            Createskearch();
            createskespe();
            createskewar();
            createdemon();
            createladder();
            Createwall();
            BStartY = Background.Img.Height - ClientSize.Height;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W && CheckLadder("u") && Hero.State != 2)
            {
                keyW = true;
                if (Hero.State != 9)
                {
                    Hero.IF = 0;
                    Hero.State = 9;
                }
            }
            else if (e.KeyCode == Keys.S && CheckLadder("d") && Hero.State != 2)
            {
                keyS = true;
                if (Hero.State != 9)
                {
                    Hero.IF = 0;
                    Hero.State = 9;
                }
            }
            else if (Hero.State == 0 || Hero.State == 1 || Hero.State == 2)
            {
                if (e.KeyCode == Keys.A || e.KeyCode == Keys.D)
                {
                    if (Hero.State != 1 && Hero.State != 2)
                    {
                        Hero.IF = 0;
                        Hero.State = 1;
                    }

                    if (e.KeyCode == Keys.A)
                    {
                        keyA = true;
                    }
                    if (e.KeyCode == Keys.D)
                    {
                        keyD = true;
                    }
                    if (Hero.State == 0 || Hero.State == 1)
                    {
                        Hero.IF++;
                        if (Hero.IF >= 8)
                        {
                            Hero.IF = 0;
                        }
                    }
                }
                else if (e.KeyCode == Keys.W)
                {
                    if (Hero.State != 2)
                    {
                        Hero.IF = 0;
                        Hero.State = 2;
                    }

                }

                if (e.KeyCode == Keys.R)
                {
                    isAttack = true;
                    Hero.State = 6;
                }
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (Hero.State == 1)
            {
                Hero.State = 0;
                Hero.IF = 0;
            }
            if (e.KeyCode == Keys.W) { keyW = false; }
            if (e.KeyCode == Keys.A) { keyA = false; }
            if (e.KeyCode == Keys.S) { keyS = false; }
            if (e.KeyCode == Keys.D) { keyD = false; }

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isAttack = true;
                Hero.State = 5;
                Hero.IF = 0;
            }
            if (e.Button == MouseButtons.Right)
            {
                isAttack = true;
                LaserOn = true;
                Hero.State = 7;
                Hero.IF = 0;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                LaserOn = false;
            }
        }
        void ManageHeroMove()
        {
            if (keyA == true && CheckWalls("a"))
            {
                if (Hero.X - Speed > 0)
                {
                    Hero.X -= Speed;
                }
            }
            else if (keyD == true && CheckWalls("d"))
            {
                if (Hero.X + Hero.Idle[0].Width < Background.Img.Width)
                {
                    Hero.X += Speed;
                }
            }
            CheckBorders();
        }
        void ManageActorAnimation(CActor Actor)
        {

            if (Actor.State == 0)
            {
                Actor.IF++;
                if (Actor.IF >= Actor.Idle.Count)
                {
                    Actor.IF = 0;
                }
            }
            else if (Actor.State == 1)
            {
                Actor.IF++;
                if (Actor.IF >= Actor.Walk.Count)
                {
                    Actor.IF--;
                }
            }
            else if (Actor.State == 2)
            {
                Actor.IF++;
                if (Actor.IF == 5 && ctJump < JumpLimit / 2)
                {
                    Actor.IF--;
                }
                else if (Actor.IF == 6 && ctJump > JumpLimit / 2 && ctJump < JumpLimit - 4)
                {
                    Actor.IF--;
                }
            }
            else if (Actor.State == 3)
            {
                Actor.IF++;
                if (Actor.IF >= Actor.Hurt.Count)
                {
                    Actor.IF = 0;
                    Actor.State = 0;
                }
            }
            else if (Actor.State == 4)
            {
                Actor.IF++;
                if (Actor.IF >= Actor.Dead.Count)
                {
                    Actor.IF = Actor.Dead.Count - 1;
                }
            }
            else if (Actor.State == 5)
            {
                Actor.IF++;
                if (Actor.IF >= Actor.Attack1.Count)
                {
                    Actor.IF = 0;
                    Actor.State = 0;
                    Fa7l.GotHit = false;
                }
            }
            else if (Actor.State == 6)
            {
                Actor.IF++;
                if (Actor.IF >= Actor.Attack2.Count)
                {
                    ball pnn = new ball();
                    pnn.X = Hero.X + Hero.Attack2[Hero.Attack2.Count - 1].Width;
                    pnn.Y = Hero.Y;
                    for (int i = 1; i <= 12; i++)
                    {
                        pnn.Charge.Add(new Bitmap("hero\\charge\\herocharge" + i + ".png"));
                    }
                    Fireball.Add(pnn);
                    Actor.IF = 0;
                    Actor.State = 0;
                }
            }
            else if (Actor.State == 7)
            {
                Actor.IF++;
                if (Actor.IF >= Actor.Attack3.Count - 3)
                {
                    if (LaserOn)
                    {
                        Actor.IF = 6;
                    }
                    else
                    {
                        Actor.IF = 0;
                        Actor.State = 0;
                    }
                }
            }
            else if (Actor.State == 9)
            {
                if (Actor.IF >= Actor.Climb.Count)
                {
                    Actor.IF = 0;
                }
            }
        }

        void ManageChargeAnimation()
        {
            if (Fireball.Count > 0)
            {
                for (int i = 0; i < Fireball.Count; i++)
                {
                    ball ptrv = Fireball[i];
                    ptrv.X += 10;
                    ptrv.IF++;
                    if ((ptrv.X + ptrv.Charge[ptrv.IF].Width) > Fa7l.X - 30 && Fa7l.X > ptrv.X)
                    {
                        ptrv.IF = 6;
                    }
                    else if (ptrv.IF >= ptrv.Charge.Count - 7)
                    {
                        ptrv.IF = 0;
                    }

                    if (ptrv.W == 20 || ptrv.IF == ptrv.Charge.Count)
                    {
                        Fireball.RemoveAt(i);
                        i--;
                    }
                    ptrv.W++;
                }
            }
        }

        void CheckEnemyHit()
        {
            if (Hero.State == 5 && (Hero.X + Hero.Attack1[Hero.IF].Width >= Fa7l.X - BStartX - 30) && Fa7l.X - BStartX > Hero.X && isAttack && !Fa7l.isDead)
            {
                if (!Fa7l.GotHit)
                {
                    Fa7l.Health -= 10;
                    Fa7l.IF = 0;
                    Fa7l.GotHit = true;
                    CheckDead(Fa7l);
                }
            }
            else if (Hero.State == 7 && (Hero.X + Hero.Attack3[Hero.IF].Width >= Fa7l.X - BStartX - 30) && Fa7l.X - BStartX > Hero.X && isAttack && !Fa7l.isDead)
            {

                Fa7l.Health -= 1;
                Fa7l.IF = 0;
                Fa7l.GotHit = true;
                CheckDead(Fa7l);
            }
            if (Fireball.Count > 0)
            {
                for (int i = 0; i < Fireball.Count; i++)
                {
                    if ((Fireball[i].X + Fireball[i].Charge[Fireball[i].IF].Width) > Fa7l.X - BStartX && Fa7l.X - BStartX - 30 > Fireball[i].X && !Fa7l.isDead)
                    {
                        Fa7l.Health -= 5;
                        Fa7l.IF = 0;
                        Fa7l.GotHit = true;
                        CheckDead(Fa7l);
                    }
                }
            }

        }
        void CheckDead(CActor enemy)
        {
            if (enemy.Health > 0)
            {
                enemy.State = 3;
            }
            else if (enemy.Health < 0)
            {
                enemy.State = 4;
                enemy.IF = 0;
                enemy.isDead = true;
            }

        }
        void Jump()
        {
            if (Hero.State == 2)
            {
                if (ctJump < JumpLimit / 2)
                {
                    Hero.Y -= 25;
                }
                else
                {
                    Hero.Y += 25;
                }

                ctJump++;
                if (ctJump == JumpLimit)
                {
                    Hero.State = 0;
                    Hero.IF = 0;
                    ctJump = 0;
                }
            }
        }

        bool CheckLadder(string Direction)
        {
            for (int i = 0; i < Ladder.Count; i++)
            {
                if (Hero.X > Ladder[i].X && Hero.X + Hero.Idle[0].Width < Ladder[i].X + Ladder[i].Img.Width && Hero.Y + Hero.Idle[0].Height > Ladder[i].Y - 30 && Hero.Y + Hero.Idle[0].Height < Ladder[i].Y + Ladder[i].Img.Height && Direction == "u")
                {
                    Hero.X = Ladder[i].X + Ladder[i].Img.Width / 2 - Hero.Idle[0].Width / 2;
                    Hero.IF++;
                    if (BStartY + ClientSize.Height + Speed < Background.Img.Height)
                    {
                        Hero.Y += Speed;
                    }
                    else
                    {
                        Hero.Y -= Speed;
                    }
                    if (Hero.Y < Ladder[i].Y)
                    {
                        Hero.Y = Floor[Ladder[i].Floor].Y - Floor[Ladder[i].Floor].H - Hero.Idle[0].Height;
                        Hero.State = 0;
                        return false;
                    }
                    return true;
                }
                else if (Hero.X > Ladder[i].X && Hero.X + Hero.Idle[0].Width < Ladder[i].X + Ladder[i].Img.Width && Hero.Y + Hero.Idle[0].Height > Ladder[i].Y - 100 && Hero.Y + Hero.Idle[0].Height < Ladder[i].Y + Ladder[i].Img.Height && Direction == "d")
                {
                    Hero.X = Ladder[i].X + Ladder[i].Img.Width / 2 - Hero.Idle[0].Width / 2;
                    Hero.IF++;
                    if (Speed < Background.Img.Height - ClientSize.Height)
                    {
                        Hero.Y += Speed;
                    }
                    else
                    {
                        Hero.Y += Speed;
                    }
                    if (Hero.Y + Hero.Idle[0].Height > Ladder[i].Y + Ladder[i].Img.Height - 70)
                    {
                        Hero.Y = Floor[Ladder[i].Floor + 1].Y - Floor[Ladder[i].Floor + 1].H - Hero.Idle[0].Height;
                        Hero.State = 0;
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }



        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
        void DrawScene(Graphics g2)
        {
            BStartX = Hero.X - ClientSize.Width / 2;
            BStartY = Hero.Y - ClientSize.Height / 2;
            g2.Clear(BackColor);
            CheckBorders();
            Drawbackground(g2);
            Drawladder(g2);
            drawfireball(g2);
            drawfa7l(g2);
            drawhero(g2);
            DrawFloor(g2);
            DrawWalls(g2);
        }
        void Drawbackground(Graphics g)
        {
            Rectangle rcDst = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            Rectangle rcSrc = new Rectangle(BStartX, BStartY, ClientSize.Width, ClientSize.Height);
            g.DrawImage(Background.Img, rcDst, rcSrc, GraphicsUnit.Pixel);
        }

        void DrawFloor(Graphics g)
        {
            Pen pn = new Pen(Color.Yellow, 2);
            for (int i = 0; i < 5; i++)
            {
                tiles ptrv = Floor[i];
                g.DrawRectangle(pn, ptrv.X - BStartX, ptrv.Y - BStartY, ptrv.W, ptrv.H);
            }
        }
        void DrawWalls(Graphics g)
        {
            Pen pn = new Pen(Color.Yellow, 2);
            for (int i = 0; i < 5; i++)
            {
                tiles ptrv = Walls[i];
                g.DrawRectangle(pn, ptrv.X - BStartX, ptrv.Y - BStartY, ptrv.img.Width, ptrv.img.Height);
                g.DrawImage(ptrv.img, ptrv.X - BStartX, ptrv.Y - BStartY);
            }
        }
        void Drawladder(Graphics g)
        {
            for (int i = 0; i < Ladder.Count; i++)
            {
                g.DrawImage(Ladder[i].Img, Ladder[i].X - BStartX, Ladder[i].Y - BStartY);
            }
        }
        void drawfa7l(Graphics g)
        {
            int fa7lDrawX = Fa7l.X - BStartX;
            if (Fa7l.IF < Fa7l.Xadd.Length)
            {
                fa7lDrawX += Fa7l.Xadd[Fa7l.IF];
            }
            if (Fa7l.State == 0) { g.DrawImage(Fa7l.Idle[Fa7l.IF], fa7lDrawX, Fa7l.Y - BStartY); }
            else if (Fa7l.State == 1) { g.DrawImage(Fa7l.Walk[Fa7l.IF], Fa7l.X - BStartX, Fa7l.Y - BStartY); }
            else if (Fa7l.State == 3) { g.DrawImage(Fa7l.Hurt[Fa7l.IF], Fa7l.X - BStartX, Fa7l.Y - BStartY); }
            else if (Fa7l.State == 4) { g.DrawImage(Fa7l.Dead[Fa7l.IF], Fa7l.X - BStartX, Fa7l.Y - BStartY); }
            else if (Fa7l.State == 5) { g.DrawImage(Fa7l.Attack1[Fa7l.IF], Fa7l.X - BStartX, Fa7l.Y - BStartY); }
            else { g.DrawImage(Fa7l.Idle[0], Fa7l.X, Fa7l.Y); }
        }
        void drawhero(Graphics g)
        {
            int frameToDraw = Hero.IF;

            if (Hero.State == 0 && Hero.Idle.Count > frameToDraw)
            {
                g.DrawImage(Hero.Idle[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
            }
            else if (Hero.State == 1)
            {
                g.DrawImage(Hero.Walk[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
            }
            else if (Hero.State == 2)
            {
                g.DrawImage(Hero.Jump[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
            }
            else if (Hero.State == 5)
            {
                g.DrawImage(Hero.Attack1[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
            }
            else if (Hero.State == 6)
            {
                g.DrawImage(Hero.Attack2[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
            }
            else if (Hero.State == 7)
            {
                g.DrawImage(Hero.Attack3[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
            }
            else if (Hero.State == 9)
            {
                g.DrawImage(Hero.Climb[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
            }
        }
        void drawfireball(Graphics g)
        {
            for (int i = 0; i < Fireball.Count; i++)
            {
                g.DrawImage(Fireball[i].Charge[Fireball[i].IF], Fireball[i].X, Fireball[i].Y);
            }
        }
        void Createground()
        {
            Background.Img = new Bitmap("map.png");
            Background.X = 0;
            Background.Y = 0;
        }
        void CreateFloor()
        {
            int Floory = 450, yadd = 0;
            for (int i = 0; i < 5; i++)
            {
                tiles pnn = new tiles();
                pnn.X = 0;
                pnn.Y = Floory + yadd;
                yadd += 436 + 21;
                pnn.W = 4245;
                pnn.H = 26;
                Floor.Add(pnn);
            }
        }
        void Createwall()
        {
            tiles pnn = new tiles();
            pnn.X = 2280;
            pnn.Y = 1857;
            pnn.img = new Bitmap("pillars\\p51.png");
            Walls.Add(pnn);
            pnn = new tiles();
            pnn.X = 1560;
            pnn.Y = 1390;
            pnn.img = new Bitmap("pillars\\p411.png");
            Walls.Add(pnn);
            pnn = new tiles();
            pnn.X = 840;
            pnn.Y = 935;
            pnn.img = new Bitmap("pillars\\p3.png");
            Walls.Add(pnn);
            pnn = new tiles();
            pnn.X = 2630;
            pnn.Y = 938;
            pnn.img = new Bitmap("pillars\\p31.png");
            Walls.Add(pnn);
            pnn = new tiles();
            pnn.X = 1830;
            pnn.Y = 478;
            pnn.img = new Bitmap("pillars\\p21.png");
            Walls.Add(pnn);
        }
        void createladder()
        {
            CActor pnn = new CActor();
            pnn.Img = new Bitmap("ladder.png");
            pnn.X = 3780;
            pnn.Y = 470;
            pnn.Floor = 0;
            Ladder.Add(pnn);

            pnn = new CActor();
            pnn.Img = new Bitmap("ladder.png");
            pnn.X = 1300;
            pnn.Y = 920;
            pnn.Floor = 1;
            Ladder.Add(pnn);

            pnn = new CActor();
            pnn.Img = new Bitmap("ladder.png");
            pnn.X = 2800;
            pnn.Y = 920;
            pnn.Floor = 1;
            Ladder.Add(pnn);

            pnn = new CActor();
            pnn.Img = new Bitmap("ladder.png");
            pnn.X = 4015;
            pnn.Y = 1370;
            pnn.Floor = 2;
            Ladder.Add(pnn);

            pnn = new CActor();
            pnn.Img = new Bitmap("ladder.png");
            pnn.X = 1345;
            pnn.Y = 1830;
            pnn.Floor = 3;
            Ladder.Add(pnn);

            pnn = new CActor();
            pnn.Img = new Bitmap("ladder.png");
            pnn.X = 1860;
            pnn.Y = 1830;
            pnn.Floor = 3;
            Ladder.Add(pnn);

            pnn = new CActor();
            pnn.Img = new Bitmap("ladder.png");
            pnn.X = 2660;
            pnn.Y = 1830;
            pnn.Floor = 3;
            Ladder.Add(pnn);

        }

        void Createhero()
        {
            for (int i = 1; i <= 14; i++)
            {
                if (i <= 7) { Hero.Idle.Add(new Bitmap("hero\\idle\\heroidle" + i + ".png")); }
                if (i <= 8) { Hero.Walk.Add(new Bitmap("hero\\walk\\herorun" + i + ".png")); }
                if (i <= 4) { Hero.Attack1.Add(new Bitmap("hero\\attack1\\heromeleeattack" + i + ".png")); }
                if (i <= 8) { Hero.Attack2.Add(new Bitmap("hero\\attack2\\heroattackFireball" + i + ".png")); }
                { Hero.Attack3.Add(new Bitmap("hero\\attack3\\heroflameattack" + i + ".png")); }
                if (i <= 9) { Hero.Jump.Add(new Bitmap("hero\\jump\\herojump" + i + ".png")); }
                if (i <= 6) { Hero.Dead.Add(new Bitmap("hero\\dead\\herodead" + i + ".png")); }
                if (i <= 3) { Hero.Hurt.Add(new Bitmap("hero\\hurt\\herohurt" + i + ".png")); }
                if (i <= 2) { Hero.Climb.Add(new Bitmap("hero\\climb\\c" + i + ".png")); }
            }
            Hero.X = ClientSize.Width / 2 - Hero.Idle[0].Width / 2;
            Hero.Y = Floor[Floor.Count - 1].Y - Floor[Floor.Count - 1].H - Hero.Idle[0].Height;
            Hero.State = 0;
        }
        void Createfa7l()
        {
            for (int i = 1; i <= 12; i++)
            {
                if (i <= 5) { Fa7l.Attack1.Add(new Bitmap("fa7l\\attack\\attackfa7l" + i + ".png")); }
                if (i <= 5) { Fa7l.Dead.Add(new Bitmap("fa7l\\dead\\deadfa7l" + i + ".png")); }
                if (i <= 3) { Fa7l.Hurt.Add(new Bitmap("fa7l\\hurt\\hurtfa7l" + i + ".png")); }
                if (i <= 10) { Fa7l.Idle.Add(new Bitmap("fa7l\\idle\\idlefa7l" + i + ".png")); }
                Fa7l.Walk.Add(new Bitmap("fa7l\\walk\\walkfa7l" + i + ".png"));
            }
            Fa7l.Xadd = new int[] { 0, 0, 0, -8, -21, -27, 0, 0, 0, 0 };
            Fa7l.X = 1150;
            Fa7l.Y = Floor[4].Y - Fa7l.Attack1[2].Height;
            Fa7l.State = 0;
            Fa7l.Health = 11;
        }
        void Createskearch()
        {
            for (int i = 1; i <= 15; i++)
            {
                Skearch.Attack1.Add(new Bitmap("skearch\\attack\\skearchattack" + i + ".png"));
                if (i <= 5) { Skearch.Dead.Add(new Bitmap("skearch\\dead\\skearchdead" + i + ".png")); }
                if (i <= 2) { Skearch.Hurt.Add(new Bitmap("skearch\\hurt\\skearchhurt" + i + ".png")); }
                if (i <= 7) { Skearch.Idle.Add(new Bitmap("skearch\\idle\\skearchidle" + i + ".png")); }
                if (i <= 8) { Skearch.Walk.Add(new Bitmap("skearch\\walk\\skearchwalk" + i + ".png")); }
            }
        }
        void createskewar()
        {
            for (int i = 1; i <= 7; i++)
            {
                Skewar.Idle.Add(new Bitmap("skewar\\idle\\skewaridle" + i + ".png"));
                Skewar.Walk.Add(new Bitmap("skewar\\walk\\skewarwalk" + i + ".png"));
                if (i <= 5) { Skewar.Attack1.Add(new Bitmap("skewar\\attack\\skewarattack" + i + ".png")); }
                if (i <= 4) { Skewar.Dead.Add(new Bitmap("skewar\\dead\\skewardead" + i + ".png")); }
                if (i <= 2) { Skewar.Hurt.Add(new Bitmap("skewar\\hurt\\skewarhurt" + i + ".png")); }
            }
        }
        void createskespe()
        {
            for (int i = 1; i <= 7; i++)
            {
                Skespe.Idle.Add(new Bitmap("skespe\\idle\\skespeidle" + i + ".png"));
                Skespe.Walk.Add(new Bitmap("skespe\\walk\\skespewalk" + i + ".png"));
                if (i <= 6) { Skespe.Walk.Add(new Bitmap("skespe\\run\\skesperun" + i + ".png")); }
                if (i <= 5)
                {
                    Skespe.Dead.Add(new Bitmap("skespe\\dead\\skespedead" + i + ".png"));
                    Skespe.Attack2.Add(new Bitmap("skespe\\attack+run\\skesperunattack" + i + ".png"));
                }
                if (i <= 4)
                {
                    Skespe.Attack1.Add(new Bitmap("skespe\\attack\\skespeattack" + i + ".png"));
                    Skespe.Charge.Add(new Bitmap("skespe\\Arrow.png"));
                }
                if (i <= 3) { Skespe.Hurt.Add(new Bitmap("skespe\\hurt\\skespehurt" + i + ".png")); }
            }
        }
        void createdemon()
        {
            for (int i = 1; i <= 8; i++)
            {
                if (i <= 8) { Demon.Attack1.Add(new Bitmap("demon\\attack\\demonattack" + i + ".png")); }
                if (i <= 6) { Demon.Dead.Add(new Bitmap("demon\\dead\\demondead" + i + ".png")); }
                if (i <= 4) { Demon.Walk.Add(new Bitmap("demon\\fly\\demonfly" + i + ".png")); }
                if (i <= 4) { Demon.Hurt.Add(new Bitmap("demon\\hurt\\demonhurt" + i + ".png")); }
                if (i <= 4) { Demon.Idle.Add(new Bitmap("demon\\idle\\demonideal" + i + ".png")); }
            }
        }
        bool CheckWalls(string Direction)
        {
            for (int i = 0; i < Walls.Count; i++)
            {
                if (Hero.X + Hero.Idle[0].Width + 45 > Walls[i].X  && Hero.X < Walls[i].X  && Hero.Y >= Walls[i].Y && Hero.Y < Walls[i].Y + Walls[i].img.Height  && Direction == "d")
                {
                    return false;
                }
                else if (Hero.X - 130 < Walls[i].X  && Hero.X > Walls[i].X  && Hero.Y >= Walls[i].Y  && Hero.Y < Walls[i].Y + Walls[i].img.Height && Direction == "a")
                {
                    return false;
                }
            }
            return true;
        }

        void CheckBorders()
        {
            BStartX = Hero.X - ClientSize.Width / 2;
            BStartY = Hero.Y - ClientSize.Height / 2;

            if (BStartX < 0) BStartX = 0;
            if (BStartY < 0) BStartY = 0;
            if (BStartX > Background.Img.Width - ClientSize.Width) BStartX = Background.Img.Width - ClientSize.Width;
            if (BStartY > Background.Img.Height - ClientSize.Height) BStartY = Background.Img.Height - ClientSize.Height;

        }
    }
}