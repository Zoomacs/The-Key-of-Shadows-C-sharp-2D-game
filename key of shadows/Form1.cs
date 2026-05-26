using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace key_of_shadows
{
    public class HealthBar
    {
        public int X, Y;
        public int IF;
        public int Amount = 100;
        public List<Bitmap> Imgs = new List<Bitmap>();
    }
    class Elevator
    {
        public int X, Y;
        public int Start, End;
        public int Speed = 10;
        public bool Direction;
        public bool State;
        public Bitmap Img;
    }
    class tiles
    {
        public int x, y, w, h;
        public int cf = 0;
        public Bitmap img;
        public List<Bitmap> imgs;
    }

    class ball
    {
        public int X, Y;
        public int W, D;
        public int IF;
        public int floor;
        public List<Bitmap> Charge = new List<Bitmap>();
    }
    class CActor
    {
        public int X, Y;
        public int D;
        public int Startpoint, endpoint;
        public int State;
        public int IF;
        public int IdleTimer;
        public int StunTimer;
        public int DetectRange;
        public int Speed;
        public bool JState;
        public int AttackRange;
        public int RecoveryDuration;
        public int HurtStunDuration;
        public bool IsStunned;
        public int Health;
        public bool GotHit = false;
        public bool isDead = false;
        public Bitmap Img;
        public List<Bitmap> Idlel = new List<Bitmap>();
        public List<Bitmap> Idler = new List<Bitmap>();
        public List<Bitmap> Walkl = new List<Bitmap>();
        public List<Bitmap> Walkr = new List<Bitmap>();
        public List<Bitmap> Jumpl = new List<Bitmap>();
        public List<Bitmap> Jumpr = new List<Bitmap>();
        public List<Bitmap> Climbl = new List<Bitmap>();
        public List<Bitmap> Climbr = new List<Bitmap>();
        public List<Bitmap> Attack1l = new List<Bitmap>();
        public List<Bitmap> Attack1r = new List<Bitmap>();
        public List<Bitmap> Attack2l = new List<Bitmap>();
        public List<Bitmap> Attack2r = new List<Bitmap>();
        public List<Bitmap> Attack3l = new List<Bitmap>();
        public List<Bitmap> Attack3r = new List<Bitmap>();
        public List<Bitmap> Hurtl = new List<Bitmap>();
        public List<Bitmap> Hurtr = new List<Bitmap>();
        public List<Bitmap> Deadl = new List<Bitmap>();
        public List<Bitmap> Deadr = new List<Bitmap>();
        public List<Bitmap> Chargel = new List<Bitmap>();
        public List<Bitmap> Charger = new List<Bitmap>();
        public int[] xadd;
        public int floor;
    }
    public partial class Form1 : Form
    {
        bool iscilimbing = false;
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
        ball singlefireball;
        List<ball> Fireball = new List<ball>();
        List<tiles> floor = new List<tiles>();
        List<tiles> walls = new List<tiles>();
        List<tiles> portals = new List<tiles>();
        HealthBar H = new HealthBar();
        Elevator Elevator = new Elevator();
        CActor Background = new CActor();
        int BStartX, BStartY;
        bool keyW = false;
        bool keyW2 = false;
        bool keyA = false;
        bool keyS = false;
        bool keyD = false;
        bool keyE = false;
        bool sinbullet = false;
        int ctJump, JumpLimit = 4;
        int Speed = 50;
        int skespestate;
        bool skespestatechange = false;
        int DemonflyCounter = 0;
        bool DemonflyDir = false;
        public Form1()
        {
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            MouseDown += Form1_MouseDown;
            MouseUp += Form1_MouseUp;
            Paint += Form1_Paint;
            Load += Form1_Load;
            BackColor = Color.Black;
            tt.Tick += Tt_Tick; ;
            tt.Interval = 100;
            tt.Start();
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            ManageHeroMove();
            ManageActorAnimation(Hero);
            RunStunTimer(Hero);
            CheckHeroHit(Hero);
            ManageActorAnimation(Skewar);
            Manageenemybehavior(Skewar);
            CheckEnemyHit(Skewar);
            CheckHeroHit(Skewar);
            ManageActorAnimation(Demon);
            Manageenemybehavior(Demon);
            CheckEnemyHit(Demon);
            ManageActorAnimation(Fa7l);
            Manageenemybehavior(Fa7l);
            CheckEnemyHit(Fa7l);
            CheckHeroHit(Fa7l);
            ManageActorAnimation(Skespe);
            CheckEnemyHit(Skespe);
            CheckHeroHit(Skespe);
            CheckGravity();
            ManageSkespeBehavior();
            ManageChargeAnimation();
            managefirballmove();
            Jump();
            manageportalanimation();
            DrawDubb(CreateGraphics());
            CheckHeroHealth();
            MoveElevator();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            Createground();
            Createfloor();
            createportals();
            Createwall();
            Createhero();
            Createfa7l();
            Createskearch();
            createskespe();
            createskewar();
            createdemon();
            createladder();
            CreateElevator(); 
            CreateHealthBar();
           // BStartY = Background.Img.Height - ClientSize.Height;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W && CheckLadder("u"))
            {
                keyW = true;
                Hero.State = 9;
            }
            else if (e.KeyCode == Keys.S && CheckLadder("d"))
            {
                keyS = true;
                Hero.State = 9;

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
                        Hero.D = -1;
                    }

                    if (e.KeyCode == Keys.D)
                    {
                        keyD = true;
                        Hero.D = 1;
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
                    else if (!Hero.JState)
                    {
                        keyW2 = true;
                        Hero.JState = true;
                    }

                }
                if (e.KeyCode == Keys.E)
                {
                    keyE = true;
                    TryTeleport();
                }

                if (e.KeyCode == Keys.R)
                {
                    isAttack = true;
                    Hero.State = 6;
                }

                if (e.KeyCode == Keys.F)
                {
                    createsinglefireball();
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
            if (e.KeyCode == Keys.E) { keyE = false; }

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
        void managefirballmove()
        {
            if (sinbullet)
            {
                if (singlefireball.W == 1)
                {
                    if (singlefireball.D == 1)
                    {
                        singlefireball.X += 10;
                    }
                    else
                    {
                        singlefireball.X -= 10;
                    }
                    singlefireball.IF++;
                    if (singlefireball.IF >= 5)
                    {
                        singlefireball.IF = 0;
                    }
                    bool hit = false;

                    if (!Fa7l.isDead && singlefireball.X + singlefireball.Charge[singlefireball.IF].Width >= Fa7l.X && singlefireball.X < Fa7l.X + 50 && singlefireball.floor == Fa7l.floor)
                    {
                        Fa7l.Health -= 5;
                        Fa7l.GotHit = true;
                        CheckDead(Fa7l);
                        hit = true;
                    }
                    if (!Skewar.isDead && singlefireball.X + singlefireball.Charge[singlefireball.IF].Width >= Skewar.X && singlefireball.X < Skewar.X + 50 && singlefireball.floor == Skewar.floor)
                    {
                        Skewar.Health -= 5;
                        Skewar.GotHit = true;
                        CheckDead(Skewar);
                        hit = true;
                    }
                    if (!Skespe.isDead && singlefireball.X + singlefireball.Charge[singlefireball.IF].Width >= Skespe.X && singlefireball.X < Skespe.X + 50 && singlefireball.floor == Skespe.floor)
                    {
                        Skespe.Health -= 5;
                        Skespe.GotHit = true;
                        CheckDead(Skespe);
                        hit = true;
                    }
                    if (singlefireball.X > Background.Img.Width)
                    {
                        hit = true;
                    }
                    if (hit)
                    {
                        singlefireball.W = 2;
                        singlefireball.IF = 5;
                    }
                }
                else if (singlefireball.W == 2)
                {
                    singlefireball.IF++;
                    if (singlefireball.IF >= singlefireball.Charge.Count)
                    {
                        singlefireball = null;
                        sinbullet = false;
                    }
                }
            }
        }
        void createsinglefireball()
        {
            if (!sinbullet)
            {
                singlefireball = new ball();
                singlefireball.Y = Hero.Y;
                singlefireball.W = 1;
                singlefireball.D = Hero.D;
                singlefireball.floor = Hero.floor;
                if (Hero.D == 1)
                {
                    singlefireball.X = Hero.X + Hero.Attack2r[Hero.Attack2r.Count - 1].Width;
                }
                else
                {
                    singlefireball.X = Hero.X - Hero.Attack2l[Hero.Attack2l.Count - 1].Width;
                }
                for (int i = 1; i <= 12; i++)
                {
                    singlefireball.Charge.Add(new Bitmap("hero\\charge\\herocharge" + i + ".png"));
                }
                sinbullet = true;

            }
          
        }
        void drawsinglefirball(Graphics g)
        {
            if (sinbullet)
            {
                g.DrawImage(singlefireball.Charge[singlefireball.IF], singlefireball.X - BStartX, singlefireball.Y - BStartY);
            }
        }
        void ManageHeroMove()
        {
            if (keyA == true && CheckWalls("a"))
            {
                if (Hero.State != 2)
                {
                    Hero.State = 1;
                }
                if (Hero.X - Speed > 0)
                {
                    Hero.X -= Speed;
                }
            }
            else if (keyD == true && CheckWalls("d"))
            {
                if (Hero.State != 2)
                {
                    Hero.State = 1;
                }
                if (Hero.X + Hero.Idler[0].Width + 50 < Background.Img.Width)
                {
                    Hero.X += Speed;
                }
            }
            else if (keyW && CheckLadder("u"))
            {
                Hero.IF++;
            }
            else if (keyS && CheckLadder("d"))
            {
                Hero.IF++;
            }

            CheckBorders();
        }
        void ManageSkespeBehavior()
        {
            if (Skespe.isDead) { return; }
            if (Skespe.IsStunned)
            {
                RunStunTimer(Skespe);
                return;
            }

            int heroWorldX = Hero.X;
            int distance = heroWorldX - Skespe.X;
            if (distance < 0)
            {
                distance = distance * -1;
            }

            if (Hero.floor != Skespe.floor)
            {
                RunMoveBehavior(Skespe);
                return;
            }
            if (distance <= Skespe.DetectRange && distance >= Skespe.DetectRange / 2)
            {
                if (!skespestatechange)
                {
                    skespestate = 0;
                    skespestatechange = true;
                }
            }
            else
            {
                if (!skespestatechange)
                {
                    skespestate = 1;
                    skespestatechange = true;
                }
            }
            if (distance <= Skespe.AttackRange)
            {
                skespestatechange = false;
                if (skespestate == 0)
                {
                    TriggerSkeSpeAttack2();
                }
                else
                {
                    TriggerAttack(Skespe);
                }
            }
            else if (skespestate == 0)
            {
                Skespe.State = 9;
                if (heroWorldX > Skespe.X)
                {
                    Skespe.D = 1;
                    Skespe.X += Skespe.Speed * 2;
                }
                else
                {
                    Skespe.D = -1;
                    Skespe.X -= Skespe.Speed * 2;
                }
            }
            else
            {
                Skespe.State = 1;
                if (heroWorldX > Skespe.X)
                {
                    Skespe.D = 1;
                    Skespe.X += Skespe.Speed;
                }
                else
                {
                    Skespe.D = -1;
                    Skespe.X -= Skespe.Speed;
                }
            }

        }
        void Manageenemybehavior(CActor enemy)
        {
            if (enemy.isDead) { return; };
            if (enemy.IsStunned)
            {
                RunStunTimer(enemy);
                return;
            }
            int heroWorldX = Hero.X;
            int distance = heroWorldX - enemy.X;
            if (distance < 0)
            {
                distance = distance * -1;
            }
            if (distance <= enemy.DetectRange && Hero.floor == enemy.floor)
            {
                RunDetectBehavior(enemy, heroWorldX);
            }
            else
            {
                RunMoveBehavior(enemy);
            }
        }
        void RunStunTimer(CActor enemy)
        {
            if (enemy.IsStunned)
            {
                enemy.StunTimer--;

                if (enemy.StunTimer <= 0)
                {
                    enemy.IsStunned = false;
                    enemy.StunTimer = 0;

                    if (enemy.State == 3 || enemy.State == 5)
                    {
                        enemy.State = 0;
                        enemy.IF = 0;
                        enemy.GotHit = false;

                        if (enemy == Demon)
                        {
                            Demon.Y -= 40;
                        }
                    }
                }
            }

        }
        void RunMoveBehavior(CActor enemy)
        {
            if (enemy.State == 4) return;

            if (enemy.State == 0)
            {
                enemy.IdleTimer--;
                if (enemy.IdleTimer <= 0)
                {
                    enemy.D *= -1;
                    enemy.IdleTimer = 5;
                    enemy.State = 1;
                    enemy.IF = 0;
                }
                return;
            }

            enemy.X += enemy.Speed * enemy.D;

            if (enemy.D == 1 && enemy.X >= enemy.endpoint)
            {
                enemy.X -= enemy.Speed;
                enemy.State = 0;
                enemy.IF = 0;
            }
            else if (enemy.D == -1 && enemy.X <= enemy.Startpoint)
            {
                enemy.X += enemy.Speed;
                enemy.State = 0;
                enemy.IF = 0;
            }
            if (enemy == Demon && !enemy.isDead)
            {
                
            }
        }
        void Demonfly(CActor enemy)
        {
            if (enemy == Demon && !enemy.isDead)
            {
                if (!DemonflyDir)
                {
                    Demon.Y++;
                    DemonflyCounter++;
                    if (DemonflyCounter >= 15)
                    {
                        DemonflyDir = true;
                    }
                }
                else
                {
                    Demon.Y--;
                    DemonflyCounter--;
                    if (DemonflyCounter <= 0)
                    {
                        DemonflyDir = false;
                    }
                }
            }
        }
        void RunDetectBehavior(CActor enemy, int heroWorldX)
        {
            int distance = heroWorldX - enemy.X;
            if (distance < 0)
            {
                distance = distance * -1;
            }

            if (distance > enemy.AttackRange)
            {
                enemy.State = 1;
                if (heroWorldX > enemy.X)
                {
                    enemy.D = 1;
                    enemy.X += enemy.Speed;
                }
                else
                {
                    enemy.D = -1;
                    enemy.X -= enemy.Speed;
                }
            }
            else
            {
                TriggerAttack(enemy);
            }
        }
        void TriggerSkeSpeAttack2()
        {
            if (Skespe.IsStunned) { return; }
            if (Hero.isDead) { return; }
            Skespe.State = 6;
            Skespe.IF = 0;
            Skespe.IsStunned = true;
            Skespe.StunTimer = Skespe.RecoveryDuration;

        }
        void TriggerAttack(CActor enemy)
        {
            if (enemy.IsStunned) { return; }
            if (Hero.isDead) { return; }
            enemy.State = 5;
            enemy.IF = 0;
            enemy.IsStunned = true;
            enemy.StunTimer = enemy.RecoveryDuration;
            if (enemy == Demon)
            {
                Demon.Y += 40;
            }

        }
        void TriggerHurt(CActor enemy)
        {
            if (enemy.isDead || enemy.IsStunned) { return; }

            enemy.State = 3;
            enemy.IF = 0;
            enemy.IsStunned = true;
            enemy.StunTimer = enemy.HurtStunDuration;
        }
        void ManageActorAnimation(CActor Actor)
        {

            if (Actor.State == 0)
            {
                Actor.IF++;
                if (Actor.IF >= Actor.Idler.Count)
                {
                    Actor.IF = 0;
                }
            }
            else if (Actor.State == 1)
            {
                Actor.IF++;
                if (Actor.IF >= Actor.Walkr.Count)
                {
                    Actor.IF = 0;
                }
            }
            else if (Actor.State == 2)
            {
                Actor.IF++;
                if (Actor.IF == 5 && ctJump < JumpLimit)
                {
                    Actor.IF--;
                }
                else if (Actor.IF == 6 && ctJump > JumpLimit && CheckGravity())
                {
                    Actor.IF--;
                }
            }
            else if (Actor.State == 3)
            {
                Actor.IF++;
                if (Actor.IF >= Actor.Hurtr.Count)
                {
                    Actor.IF = 0;
                    Actor.State = 0;
                }
            }
            else if (Actor.State == 4)
            {
                Actor.IF++;
                if (Actor.IF >= Actor.Deadr.Count)
                {
                    Actor.IF = Actor.Deadr.Count - 1;
                }
            }
            else if (Actor.State == 5)
            {
                Actor.IF++;
                if (Actor.IF >= Actor.Attack1r.Count)
                {
                    Actor.IF = 0;
                    Actor.State = 0;
                    Actor.GotHit = false;
                }
            }
            else if (Actor.State == 6)
            {
                Actor.IF++;
                if (Actor.IF >= Actor.Attack2r.Count)
                {
                    if (Actor == Hero)
                    {
                        ball pnn = new ball();
                        pnn.X = (Hero.X) + Hero.Attack2r[Hero.Attack2r.Count - 1].Width;
                        pnn.Y = Hero.Y;
                        pnn.W = 1;
                        pnn.floor = Actor.floor;
                        pnn.D = Hero.D;
                        if (Hero.D == 1)
                        {
                            pnn.X = Hero.X + Hero.Attack2r[Hero.Attack2r.Count - 1].Width;
                        }
                        else
                        {
                            pnn.X = Hero.X - Hero.Attack2l[Hero.Attack2l.Count - 1].Width;
                        }
                        for (int i = 1; i <= 12; i++)
                        {
                            if(pnn.D == 1)
                            {
                                pnn.Charge.Add(new Bitmap("hero\\heroR\\charge\\herocharge" + i + ".png"));
                            }
                            else
                            {
                                pnn.Charge.Add(new Bitmap("hero\\heroL\\charge\\herocharge" + i + "_result.png"));
                            }
                           
                        }
                        Fireball.Add(pnn);

                    }
                    Actor.IF = 0;
                    Actor.State = 0;
                    Actor.GotHit = false;

                }
            }
            else if (Actor.State == 7)
            {
                Actor.IF++;
                if (Actor.IF >= Actor.Attack3r.Count - 3)
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
                if (Actor.IF >= Actor.Climbr.Count)
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

                    if (ptrv.W == 1)
                    {
                        if (ptrv.D == 1)
                        {
                            ptrv.X += 10;
                        }
                        else
                        {
                            ptrv.X -= 10;
                        }
                        ptrv.IF++;
                        if (ptrv.IF >= 5)
                        {
                            ptrv.IF = 0;
                        }
                        bool hit = false;

                        if (!Fa7l.isDead && ptrv.X + ptrv.Charge[ptrv.IF].Width >= Fa7l.X && ptrv.X < Fa7l.X + 50 && ptrv.floor == Fa7l.floor)
                        {
                            Fa7l.Health -= 5;
                            Fa7l.GotHit = true;
                            CheckDead(Fa7l);
                            hit = true;
                        }
                        if (!Skewar.isDead && ptrv.X + ptrv.Charge[ptrv.IF].Width >= Skewar.X && ptrv.X < Skewar.X + 50 && ptrv.floor == Skewar.floor)
                        {
                            Skewar.Health -= 5;
                            Skewar.GotHit = true;
                            CheckDead(Skewar);
                            hit = true;
                        }
                        if (!Skespe.isDead && ptrv.X + ptrv.Charge[ptrv.IF].Width >= Skespe.X && ptrv.X < Skespe.X + 50 && ptrv.floor == Skespe.floor)
                        {
                            Skespe.Health -= 5;
                            Skespe.GotHit = true;
                            CheckDead(Skespe);
                            hit = true;
                        }
                        if (ptrv.X > Background.Img.Width)
                        {
                            hit = true;
                        }
                        if (hit)
                        {
                            ptrv.W = 2;
                            ptrv.IF = 5;
                        }
                    }
                    else if (ptrv.W == 2)
                    {
                        ptrv.IF++;
                        if (ptrv.IF >= ptrv.Charge.Count)
                        {
                            Fireball.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }
        void CheckHeroHit(CActor enemy)
        {

            if (enemy.State != 5 || enemy.isDead)
            {
                Hero.GotHit = false;
                return;
            }

            if (!Hero.GotHit)
            {
                if (Hero.X > enemy.X && enemy.X + enemy.Attack1r[enemy.IF].Width >= Hero.X - 40)
                {
                    Hero.Health -= 10;
                    Hero.GotHit = true;
                    CheckDead(Hero);
                }
                else if (Hero.X < enemy.X && Hero.X + Hero.Idler[0].Width >= enemy.X - 30)
                {
                    Hero.Health -= 10;
                    Hero.GotHit = true;
                    CheckDead(Hero);
                }
            }
        }

        void CheckEnemyHit(CActor enemy)
        {
            bool heroHitsRight = false;
            bool heroHitsLeft = false;
            if (enemy.isDead) { return; }
            if (Hero.State == 5 && isAttack && !enemy.GotHit)
            {
                if(Hero.D == 1 && enemy.X > Hero.X && Hero.X + Hero.Attack1r[Hero.IF].Width >= enemy.X - 30)
                {
                    heroHitsRight = true;
                }
                if (Hero.D == -1 && enemy.X < Hero.X && Hero.X - Hero.Attack1l[Hero.IF].Width <= enemy.X + enemy.Idler[0].Width)
                {
                    heroHitsLeft = true;
                }
                if (heroHitsRight || heroHitsLeft)
                {
                    enemy.Health -= 10;
                    enemy.IF = 0;
                    enemy.GotHit = true;
                    CheckDead(enemy);
                }
            }
            else if (Hero.State == 7 && isAttack)
            {
                if (Hero.D == 1 && enemy.X > Hero.X && Hero.X + Hero.Attack3r[Hero.IF].Width >= enemy.X - 30)
                {
                    heroHitsRight = true;
                }
                if (Hero.D == -1 && enemy.X < Hero.X && Hero.X - Hero.Attack3l[Hero.IF].Width <= enemy.X + enemy.Idler[0].Width)
                {
                    heroHitsLeft = true;
                }
                if (heroHitsRight || heroHitsLeft)
                {
                    enemy.Health -= 1;
                    enemy.IF = 0;
                    enemy.GotHit = true;
                    CheckDead(enemy);
                }
            }
            if (Fireball.Count > 0)
            {
                for (int i = 0; i < Fireball.Count; i++)
                {
                    ball ptrv = Fireball[i];
                    if (ptrv.W == 1 && ptrv.floor == enemy.floor)
                    {
                        int fireballWorldX = ptrv.X;
                        int enemyWidth = 50;
                        if (fireballWorldX + ptrv.Charge[ptrv.IF].Width >= enemy.X && fireballWorldX < enemy.X + enemyWidth)
                        {
                            enemy.Health -= 5;
                            enemy.IF = 0;
                            enemy.GotHit = true;
                            CheckDead(enemy);
                            ptrv.W = 2;
                            ptrv.IF = 5;
                        }
                    }
                }
            }

        }
        void CheckDead(CActor enemy)
        {
            if (enemy.Health <= 0)
            {
                enemy.State = 4;
                enemy.IF = 0;
                enemy.isDead = true;
                enemy.IsStunned = false;
            }
            else
            {
                TriggerHurt(enemy);
            }
        }
        void Jump()
        {
            if (Hero.State == 2)
            {
                if (ctJump < JumpLimit)
                {
                    Hero.Y -= 25;
                }

                if (keyW2 && Hero.JState)
                {
                    Hero.IF = 0;
                    ctJump = 0;
                    keyW2 = false;
                }

                ctJump++;
                if (Hero.IF == 9)
                {
                    Hero.JState = false;
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
                if (Hero.X > Ladder[i].X && Hero.X + Hero.Idler[0].Width < Ladder[i].X + Ladder[i].Img.Width && Hero.Y + Hero.Idler[0].Height > Ladder[i].Y - 30 && Hero.Y + Hero.Idler[0].Height < Ladder[i].Y + Ladder[i].Img.Height && Direction == "u" && Hero.State != 1 && Hero.State != 2)
                {
                    Hero.X = Ladder[i].X + Ladder[i].Img.Width / 2 - Hero.Idler[0].Width / 2;
                        Hero.Y -= Speed;
                                            if (!iscilimbing)
                        {
                            Hero.floor++;
                            iscilimbing = true;
                        }
                    if (Hero.Y < Ladder[i].Y)
                    {
                        Hero.Y = floor[Ladder[i].floor].y - floor[Ladder[i].floor].h - Hero.Idler[0].Height;
                        Hero.State = 0;
                        iscilimbing = false;
                        return false;
                    }
                    return true;
                }
                else if (Hero.X > Ladder[i].X && Hero.X + Hero.Idler[0].Width < Ladder[i].X + Ladder[i].Img.Width && Hero.Y + Hero.Idler[0].Height > Ladder[i].Y - 100 && Hero.Y + Hero.Idler[0].Height < Ladder[i].Y + Ladder[i].Img.Height && Direction == "d" && Hero.State != 1 && Hero.State != 2)
                {
                    Hero.X = Ladder[i].X + Ladder[i].Img.Width / 2 - Hero.Idler[0].Width / 2;
                        Hero.Y += Speed;
                    
                    if (Hero.Y + Hero.Idler[0].Height > Ladder[i].Y + Ladder[i].Img.Height - 70)
                    {
                        if (iscilimbing)
                        {
                            Hero.floor--;
                            iscilimbing = false;
                        }
                        Hero.Y = floor[Ladder[i].floor + 1].y - floor[Ladder[i].floor + 1].h - Hero.Idler[0].Height;
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
            drawportals(g2);
            drawfireball(g2);
            drawhero(g2);
            drawfa7l(g2);
            DrawDemon(g2);
            drawSkewar(g2);
            drawSkespe(g2);
            Drawfloor(g2);
            Drawwalls(g2);
            DrawElevator(g2);
            DrawHealthBar(g2);

        }
        void Drawbackground(Graphics g)
        {
            Rectangle rcDst = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            Rectangle rcSrc = new Rectangle(BStartX, BStartY, ClientSize.Width, ClientSize.Height);
            g.DrawImage(Background.Img, rcDst, rcSrc, GraphicsUnit.Pixel);
        }

        void Drawfloor(Graphics g)
        {
            Pen pn = new Pen(Color.Yellow, 2);
            for (int i = 0; i < 5; i++)
            {
                tiles ptrv = floor[i];
                g.DrawRectangle(pn, ptrv.x - BStartX, ptrv.y - BStartY, ptrv.w, ptrv.h);
            }
        }
        void Drawwalls(Graphics g)
        {
            Pen pn = new Pen(Color.Yellow, 2);
            for (int i = 0; i < 5; i++)
            {
                tiles ptrv = walls[i];
                g.DrawRectangle(pn, ptrv.x - BStartX, ptrv.y - BStartY, ptrv.img.Width, ptrv.img.Height);
                g.DrawImage(ptrv.img, ptrv.x - BStartX, ptrv.y - BStartY);
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
            int frameToDraw = Fa7l.IF;
            int drawX = Fa7l.X - BStartX;
            int drawY = Fa7l.Y - BStartY;

            if (Fa7l.D == 1)
            {
                int fa7lDrawX = drawX;
                if (frameToDraw < Fa7l.xadd.Length)
                    fa7lDrawX += Fa7l.xadd[frameToDraw];

                if (Fa7l.State == 0 && frameToDraw < Fa7l.Idler.Count)
                    g.DrawImage(Fa7l.Idler[frameToDraw], fa7lDrawX, drawY);
                else if (Fa7l.State == 1 && frameToDraw < Fa7l.Walkr.Count)
                    g.DrawImage(Fa7l.Walkr[frameToDraw], drawX, drawY);
                else if (Fa7l.State == 3 && frameToDraw < Fa7l.Hurtr.Count)
                    g.DrawImage(Fa7l.Hurtr[frameToDraw], drawX, drawY);
                else if (Fa7l.State == 4 && frameToDraw < Fa7l.Deadr.Count)
                    g.DrawImage(Fa7l.Deadr[frameToDraw], drawX, drawY);
                else if (Fa7l.State == 5 && frameToDraw < Fa7l.Attack1r.Count)
                    g.DrawImage(Fa7l.Attack1r[frameToDraw], drawX, drawY);
                else
                    g.DrawImage(Fa7l.Idler[0], drawX, drawY);
            }
            else
            {
                int fa7lDrawX = drawX;
                if (frameToDraw < Fa7l.xadd.Length)
                    fa7lDrawX -= Fa7l.xadd[frameToDraw];

                if (Fa7l.State == 0 && frameToDraw < Fa7l.Idlel.Count)
                    g.DrawImage(Fa7l.Idlel[frameToDraw], fa7lDrawX - Fa7l.Idlel[frameToDraw].Width, drawY);
                else if (Fa7l.State == 1 && frameToDraw < Fa7l.Walkl.Count)
                    g.DrawImage(Fa7l.Walkl[frameToDraw], drawX - Fa7l.Walkl[frameToDraw].Width, drawY);
                else if (Fa7l.State == 3 && frameToDraw < Fa7l.Hurtl.Count)
                    g.DrawImage(Fa7l.Hurtl[frameToDraw], drawX - Fa7l.Hurtl[frameToDraw].Width, drawY);
                else if (Fa7l.State == 4 && frameToDraw < Fa7l.Deadl.Count)
                    g.DrawImage(Fa7l.Deadl[frameToDraw], drawX - Fa7l.Deadl[frameToDraw].Width, drawY);
                else if (Fa7l.State == 5 && frameToDraw < Fa7l.Attack1l.Count)
                    g.DrawImage(Fa7l.Attack1l[frameToDraw], drawX - Fa7l.Attack1l[frameToDraw].Width, drawY);
                else
                    g.DrawImage(Fa7l.Idlel[0], drawX - Fa7l.Idlel[0].Width, drawY);
            }
        }
        void drawSkespe(Graphics g)
        {
            int frameToDraw = Skespe.IF;
            int drawX = Skespe.X - BStartX;
            int drawY = Skespe.Y - BStartY;

            if (Skespe.D == 1)
            {
                if (Skespe.State == 0 && frameToDraw < Skespe.Idler.Count)
                    g.DrawImage(Skespe.Idler[frameToDraw], drawX, drawY);
                else if (Skespe.State == 1 && frameToDraw < Skespe.Walkr.Count)
                    g.DrawImage(Skespe.Walkr[frameToDraw], drawX, drawY);
                else if (Skespe.State == 9 && frameToDraw < Skespe.Climbr.Count)
                    g.DrawImage(Skespe.Climbr[frameToDraw], drawX, drawY);
                else if (Skespe.State == 3 && frameToDraw < Skespe.Hurtr.Count)
                    g.DrawImage(Skespe.Hurtr[frameToDraw], drawX, drawY);
                else if (Skespe.State == 4 && frameToDraw < Skespe.Deadr.Count)
                    g.DrawImage(Skespe.Deadr[frameToDraw], drawX, drawY);
                else if (Skespe.State == 5 && frameToDraw < Skespe.Attack1r.Count)
                    g.DrawImage(Skespe.Attack1r[frameToDraw], drawX, drawY);
                else if (Skespe.State == 6 && frameToDraw < Skespe.Attack2r.Count)
                    g.DrawImage(Skespe.Attack2r[frameToDraw], drawX, drawY);
            }
            else
            {
                if (Skespe.State == 0 && frameToDraw < Skespe.Idlel.Count)
                    g.DrawImage(Skespe.Idlel[frameToDraw], drawX - Skespe.Idlel[frameToDraw].Width, drawY);
                else if (Skespe.State == 1 && frameToDraw < Skespe.Walkl.Count)
                    g.DrawImage(Skespe.Walkl[frameToDraw], drawX - Skespe.Walkl[frameToDraw].Width, drawY);
                else if (Skespe.State == 9 && frameToDraw < Skespe.Climbl.Count)
                    g.DrawImage(Skespe.Climbl[frameToDraw], drawX - Skespe.Climbl[frameToDraw].Width, drawY);
                else if (Skespe.State == 3 && frameToDraw < Skespe.Hurtl.Count)
                    g.DrawImage(Skespe.Hurtl[frameToDraw], drawX - Skespe.Hurtl[frameToDraw].Width, drawY);
                else if (Skespe.State == 4 && frameToDraw < Skespe.Deadl.Count)
                    g.DrawImage(Skespe.Deadl[frameToDraw], drawX - Skespe.Deadl[frameToDraw].Width, drawY);
                else if (Skespe.State == 5 && frameToDraw < Skespe.Attack1l.Count)
                    g.DrawImage(Skespe.Attack1l[frameToDraw], drawX - Skespe.Attack1l[frameToDraw].Width, drawY);
                else if (Skespe.State == 6 && frameToDraw < Skespe.Attack2l.Count)
                    g.DrawImage(Skespe.Attack2l[frameToDraw], drawX - Skespe.Attack2l[frameToDraw].Width, drawY);
            }
        }
        
        void DrawDemon(Graphics g)
        {
            int frameToDraw = Demon.IF;
            int drawX = Demon.X - BStartX;
            int drawY = Demon.Y - BStartY;
            if (Demon.D == 1)
            {
                if (Demon.State == 0 && frameToDraw < Demon.Idler.Count)
                    g.DrawImage(Demon.Idler[frameToDraw], drawX, drawY);
                else if (Demon.State == 1 && frameToDraw < Demon.Walkr.Count)
                    g.DrawImage(Demon.Walkr[frameToDraw], drawX, drawY);
                else if (Demon.State == 3 && frameToDraw < Demon.Hurtr.Count)
                    g.DrawImage(Demon.Hurtr[frameToDraw], drawX, drawY);
                else if (Demon.State == 4 && frameToDraw < Demon.Deadr.Count)
                    g.DrawImage(Demon.Deadr[frameToDraw], drawX, drawY);
                else if (Demon.State == 5 && frameToDraw < Demon.Attack1r.Count)
                    g.DrawImage(Demon.Attack1r[frameToDraw], drawX, drawY);
            }
            else
            {
                if (Demon.State == 0 && frameToDraw < Demon.Idlel.Count)
                    g.DrawImage(Demon.Idlel[frameToDraw], drawX - Demon.Idlel[frameToDraw].Width, drawY);
                else if (Demon.State == 1 && frameToDraw < Demon.Walkl.Count)
                    g.DrawImage(Demon.Walkl[frameToDraw], drawX - Demon.Walkl[frameToDraw].Width, drawY);
                else if (Demon.State == 3 && frameToDraw < Demon.Hurtl.Count)
                    g.DrawImage(Demon.Hurtl[frameToDraw], drawX - Demon.Hurtl[frameToDraw].Width, drawY);
                else if (Demon.State == 4 && frameToDraw < Demon.Deadl.Count)
                    g.DrawImage(Demon.Deadl[frameToDraw], drawX - Demon.Deadl[frameToDraw].Width, drawY);
                else if (Demon.State == 5 && frameToDraw < Demon.Attack1l.Count)
                    g.DrawImage(Demon.Attack1l[frameToDraw], drawX - Demon.Attack1l[frameToDraw].Width, drawY);
            }

        }
        void drawSkewar(Graphics g)
        {
            int frameToDraw = Skewar.IF;
            int drawX = Skewar.X - BStartX;
            int drawY = Skewar.Y - BStartY;

            if (Skewar.D == 1)
            {
                if (Skewar.State == 0 && frameToDraw < Skewar.Idler.Count)
                    g.DrawImage(Skewar.Idler[frameToDraw], drawX, drawY);
                else if (Skewar.State == 1 && frameToDraw < Skewar.Walkr.Count)
                    g.DrawImage(Skewar.Walkr[frameToDraw], drawX, drawY);
                else if (Skewar.State == 3 && frameToDraw < Skewar.Hurtr.Count)
                    g.DrawImage(Skewar.Hurtr[frameToDraw], drawX, drawY);
                else if (Skewar.State == 4 && frameToDraw < Skewar.Deadr.Count)
                    g.DrawImage(Skewar.Deadr[frameToDraw], drawX, drawY);
                else if (Skewar.State == 5 && frameToDraw < Skewar.Attack1r.Count)
                    g.DrawImage(Skewar.Attack1r[frameToDraw], drawX, drawY);
            }
            else
            {
                if (Skewar.State == 0 && frameToDraw < Skewar.Idlel.Count)
                    g.DrawImage(Skewar.Idlel[frameToDraw], drawX - Skewar.Idlel[frameToDraw].Width, drawY);
                else if (Skewar.State == 1 && frameToDraw < Skewar.Walkl.Count)
                    g.DrawImage(Skewar.Walkl[frameToDraw], drawX - Skewar.Walkl[frameToDraw].Width, drawY);
                else if (Skewar.State == 3 && frameToDraw < Skewar.Hurtl.Count)
                    g.DrawImage(Skewar.Hurtl[frameToDraw], drawX - Skewar.Hurtl[frameToDraw].Width, drawY);
                else if (Skewar.State == 4 && frameToDraw < Skewar.Deadl.Count)
                    g.DrawImage(Skewar.Deadl[frameToDraw], drawX - Skewar.Deadl[frameToDraw].Width, drawY);
                else if (Skewar.State == 5 && frameToDraw < Skewar.Attack1l.Count)
                    g.DrawImage(Skewar.Attack1l[frameToDraw], drawX - Skewar.Attack1l[frameToDraw].Width, drawY);
            }
        }
        void drawhero(Graphics g)
        {
            int frameToDraw = Hero.IF;
            if (Hero.D == 1)
            {
                if (Hero.State == 0 && Hero.Idler.Count > frameToDraw)
                {
                    g.DrawImage(Hero.Idler[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
                }
                else if (Hero.State == 1)
                {
                    g.DrawImage(Hero.Walkr[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
                }
                else if (Hero.State == 2)
                {
                    g.DrawImage(Hero.Jumpr[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
                }
                else if (Hero.State == 3)
                {
                    g.DrawImage(Hero.Hurtr[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
                }
                else if (Hero.State == 4)
                {
                    g.DrawImage(Hero.Deadr[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
                }
                else if (Hero.State == 5)
                {
                    g.DrawImage(Hero.Attack1r[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
                }
                else if (Hero.State == 6)
                {
                    g.DrawImage(Hero.Attack2r[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
                }
                else if (Hero.State == 7)
                {
                    g.DrawImage(Hero.Attack3r[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
                }
                else if (Hero.State == 9)
                {
                    g.DrawImage(Hero.Climbr[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
                }
            }
            else
            {
                if (Hero.State == 0 && Hero.Idler.Count > frameToDraw)
                {
                    g.DrawImage(Hero.Idlel[frameToDraw], Hero.X - BStartX - Hero.Idlel[frameToDraw].Width, Hero.Y - BStartY);
                }
                else if (Hero.State == 1)
                {
                    g.DrawImage(Hero.Walkl[frameToDraw], Hero.X - BStartX - Hero.Walkl[frameToDraw].Width, Hero.Y - BStartY);
                }
                else if (Hero.State == 2)
                {
                    g.DrawImage(Hero.Jumpl[frameToDraw], Hero.X - BStartX - Hero.Jumpl[frameToDraw].Width, Hero.Y - BStartY);
                }
                else if (Hero.State == 3)
                {
                    g.DrawImage(Hero.Hurtl[frameToDraw], Hero.X - BStartX - Hero.Hurtl[frameToDraw].Width, Hero.Y - BStartY);
                }
                else if (Hero.State == 4)
                {
                    g.DrawImage(Hero.Deadl[frameToDraw], Hero.X - BStartX - Hero.Deadl[frameToDraw].Width, Hero.Y - BStartY);
                }
                else if (Hero.State == 5)
                {
                    g.DrawImage(Hero.Attack1l[frameToDraw], Hero.X - BStartX - Hero.Attack1l[frameToDraw].Width, Hero.Y - BStartY);
                }
                else if (Hero.State == 6)
                {
                    g.DrawImage(Hero.Attack2l[frameToDraw], Hero.X - BStartX - Hero.Attack2l[frameToDraw].Width, Hero.Y - BStartY);
                }
                else if (Hero.State == 7)
                {
                    g.DrawImage(Hero.Attack3l[frameToDraw], Hero.X - BStartX - Hero.Attack3l[frameToDraw].Width, Hero.Y - BStartY);
                }
                else if (Hero.State == 9)
                {
                    g.DrawImage(Hero.Climbl[frameToDraw], Hero.X - BStartX - Hero.Climbl[frameToDraw].Width, Hero.Y - BStartY);
                }
            }
            
        }

        void drawfireball(Graphics g)
        {
            for (int i = 0; i < Fireball.Count; i++)
            {
                g.DrawImage(Fireball[i].Charge[Fireball[i].IF], Fireball[i].X - BStartX, Fireball[i].Y - BStartY);
            }
        }
        void Createground()
        {
            Background.Img = new Bitmap("map.png");
            Background.X = 0;
            Background.Y = 0;
        }
        void Createfloor()
        {
            int floory = 450, yadd = 0;
            for (int i = 0; i < 5; i++)
            {
                tiles pnn = new tiles();
                pnn.x = 0;
                pnn.y = floory + yadd;
                yadd += 436 + 21;
                pnn.w = 4245;
                pnn.h = 26;
                floor.Add(pnn);
            }
        }
        void Createwall()
        {
            tiles pnn = new tiles();
            pnn.x = 2280;
            pnn.y = 1857;
            pnn.img = new Bitmap("pillars\\p51.png");
            walls.Add(pnn);
            pnn = new tiles();
            pnn.x = 1560;
            pnn.y = 1390;
            pnn.img = new Bitmap("pillars\\p411.png");
            walls.Add(pnn);
            pnn = new tiles();
            pnn.x = 840;
            pnn.y = 935;
            pnn.img = new Bitmap("pillars\\p3.png");
            walls.Add(pnn);
            pnn = new tiles();
            pnn.x = 2630;
            pnn.y = 938;
            pnn.img = new Bitmap("pillars\\p31.png");
            walls.Add(pnn);
            pnn = new tiles();
            pnn.x = 1830;
            pnn.y = 478;
            pnn.img = new Bitmap("pillars\\p21.png");
            walls.Add(pnn);
        }
        void createladder()
        {
            CActor pnn = new CActor();
            pnn.Img = new Bitmap("ladder.png");
            pnn.X = 1300;
            pnn.Y = 920;
            pnn.floor = 1;
            Ladder.Add(pnn);
            pnn = new CActor();
            pnn.Img = new Bitmap("ladder.png");
            pnn.X = 2800;
            pnn.Y = 920;
            pnn.floor = 1;
            Ladder.Add(pnn);
            pnn = new CActor();
            pnn.Img = new Bitmap("ladder.png");
            pnn.X = 3780;
            pnn.Y = 470;
            pnn.floor = 0;
            Ladder.Add(pnn);
            pnn = new CActor();
            pnn.Img = new Bitmap("ladder.png");
            pnn.X = 4015;
            pnn.Y = 1370;
            pnn.floor = 2;
            Ladder.Add(pnn);
            pnn = new CActor();
            pnn.Img = new Bitmap("ladder.png");
            pnn.X = 1345;
            pnn.Y = 1830;
            pnn.floor = 3;
            Ladder.Add(pnn);
            pnn = new CActor();
            pnn.Img = new Bitmap("ladder.png");
            pnn.X = 1860;
            pnn.Y = 1830;
            pnn.floor = 3;
            Ladder.Add(pnn);
            pnn = new CActor();
            pnn.Img = new Bitmap("ladder.png");
            pnn.X = 2660;
            pnn.Y = 1830;
            pnn.floor = 3;
            Ladder.Add(pnn);

        }
        bool CheckPortalRegion(tiles portal)
        {
            int heroLeft = Hero.X;
            int heroRight = Hero.X + Hero.Idler[0].Width;
            int heroTop = Hero.Y;
            int heroBot = Hero.Y + Hero.Idler[0].Height;
            int portalLeft = portal.x;
            int portalRight = portal.x + portal.imgs[0].Width;
            int portalTop = portal.y;
            int portalBottom = portal.y + portal.imgs[0].Height;

            return heroRight > portalLeft && heroLeft < portalRight && heroBot > portalTop && heroTop < portalBottom;
        }
        void manageportalanimation()
        {
            for (int i = 0; i < portals.Count; i++)
            {
                tiles p = portals[i];
                bool heroInside = CheckPortalRegion(p);

                if (heroInside)
                {
                    if (p.cf < p.imgs.Count - 1)
                        p.cf++;
                }
                else
                {
                    if (p.cf > 0)
                        p.cf--;
                }
            }
        }
        void TryTeleport()
        {
            for (int i = 0; i < portals.Count; i++)
            {
                tiles p = portals[i];
                if (p.cf == p.imgs.Count - 1 && CheckPortalRegion(p))
                {
                    int dest;

                    if (i == 0)
                    {
                        dest = 1;
                    }
                    else
                    {
                        dest = 0;
                    }
                    tiles target = portals[dest];

                    Hero.X = target.x + target.imgs[0].Width / 2 - Hero.Idler[0].Width / 2;
                    Hero.Y = target.y + target.imgs[0].Height - Hero.Idler[0].Height;
                    Hero.floor = (dest == 0) ? 3 : 2;
                    return;
                }
            }
        }
        void drawportals(Graphics g)
        {
            int i = portals[0].cf;
            g.DrawImage(portals[0].imgs[i], portals[0].x - BStartX, portals[0].y - BStartY);
            i = portals[1].cf;
            g.DrawImage(portals[1].imgs[i], portals[1].x - BStartX, portals[1].y - BStartY);
        }
        void createportals()
        {
            tiles pnn = new tiles();
            pnn.imgs = new List<Bitmap>();
            for (int i = 1; i < 6; i++)
            {
                pnn.imgs.Add(new Bitmap("portals\\redp" + i + ".png"));
            }
            pnn.x = Background.Img.Width - pnn.imgs[0].Width - 50;
            pnn.y = floor[4].y - floor[3].h - pnn.imgs[0].Height + 25;
            pnn.cf = 0;
            portals.Add(pnn);
            pnn = new tiles();
            pnn.imgs = new List<Bitmap>();
            for (int i = 1; i < 6; i++)
            {
                pnn.imgs.Add(new Bitmap("portals\\greenp" + i + ".png"));
            }
            pnn.x = 2630 - pnn.imgs[0].Width;
            pnn.y = floor[2].y - floor[2].h - pnn.imgs[0].Height + 25;
            pnn.cf = 0;
            portals.Add(pnn);
        }
        void Createhero()
        {
            for (int i = 1; i <= 14; i++)
            {
                if (i <= 7) { Hero.Idler.Add(new Bitmap("hero\\heroR\\idle\\heroidle" + i + ".png")); }
                if (i <= 7) { Hero.Idlel.Add(new Bitmap("hero\\heroL\\idle\\heroidle" + i + "_result.png")); }
                if (i <= 8) { Hero.Walkr.Add(new Bitmap("hero\\heroR\\walk\\herorun" + i + ".png")); }
                if (i <= 8) { Hero.Walkl.Add(new Bitmap("hero\\heroL\\walk\\herorun" + i + "_result.png")); }
                if (i <= 4) { Hero.Attack1r.Add(new Bitmap("hero\\heroR\\attack1\\heromeleeattack" + i + ".png")); }
                if (i <= 4) { Hero.Attack1l.Add(new Bitmap("hero\\heroL\\attack1\\heromeleeattack" + i + "_result.png")); }
                if (i <= 8) { Hero.Attack2r.Add(new Bitmap("hero\\heroR\\attack2\\heroattackFireball" + i + ".png")); }
                if (i <= 8) { Hero.Attack2l.Add(new Bitmap("hero\\heroL\\attack2\\heroattackFireball" + i + "_result.png")); }
                { Hero.Attack3r.Add(new Bitmap("hero\\heroR\\attack3\\heroflameattack" + i + ".png")); }
                { Hero.Attack3l.Add(new Bitmap("hero\\heroL\\attack3\\heroflameattack" + i + "_result.png")); }
                if (i <= 9) { Hero.Jumpr.Add(new Bitmap("hero\\heroR\\jump\\herojump" + i + ".png")); }
                if (i <= 9) { Hero.Jumpl.Add(new Bitmap("hero\\heroL\\jump\\herojump" + i + "_result.png")); }
                if (i <= 6) { Hero.Deadr.Add(new Bitmap("hero\\heroR\\dead\\herodead" + i + ".png")); }
                if (i <= 6) { Hero.Deadl.Add(new Bitmap("hero\\heroL\\dead\\herodead" + i + "_result.png")); }
                if (i <= 3) { Hero.Hurtr.Add(new Bitmap("hero\\heroR\\hurt\\herohurt" + i + ".png")); }
                if (i <= 3) { Hero.Hurtl.Add(new Bitmap("hero\\heroL\\hurt\\herohurt" + i + "_result.png")); }
                if (i <= 2) { Hero.Climbr.Add(new Bitmap("hero\\heroR\\climb\\c" + i + ".png")); }
                if (i <= 2) { Hero.Climbl.Add(new Bitmap("hero\\heroL\\climb\\c" + i + "_result.png")); }
            }
            Hero.X = ClientSize.Width / 2 - Hero.Idler[0].Width / 2;
            Hero.Y = floor[floor.Count - 1].y - floor[floor.Count - 1].h - Hero.Idler[0].Height;
            Hero.State = 0;
            Hero.Health = 100;
            Hero.HurtStunDuration = 8;
            Hero.floor = 0;
            Hero.D = 1;
        }
        void Createfa7l()
        {
            for (int i = 1; i <= 12; i++)
            {
                if (i <= 5) { Fa7l.Attack1r.Add(new Bitmap("fa7l\\fa7lR\\attack\\attackfa7l" + i + ".png")); }
                if (i <= 5) { Fa7l.Attack1l.Add(new Bitmap("fa7l\\fa7lL\\attack\\attackfa7l" + i + "_result.png")); }
                if (i <= 5) { Fa7l.Deadr.Add(new Bitmap("fa7l\\fa7lR\\dead\\deadfa7l" + i + ".png")); }
                if (i <= 5) { Fa7l.Deadl.Add(new Bitmap("fa7l\\fa7lL\\dead\\deadfa7l" + i + "_result.png")); }
                if (i <= 3) { Fa7l.Hurtr.Add(new Bitmap("fa7l\\fa7lR\\hurt\\hurtfa7l" + i + ".png")); }
                if (i <= 3) { Fa7l.Hurtl.Add(new Bitmap("fa7l\\fa7lL\\hurt\\hurtfa7l" + i + "_result.png")); }
                if (i <= 10) { Fa7l.Idler.Add(new Bitmap("fa7l\\fa7lR\\idle\\idlefa7l" + i + ".png")); }
                if (i <= 10) { Fa7l.Idlel.Add(new Bitmap("fa7l\\fa7lL\\idle\\idlefa7l" + i + "_result.png")); }
                Fa7l.Walkr.Add(new Bitmap("fa7l\\fa7lR\\walk\\walkfa7l" + i + ".png"));
                Fa7l.Walkl.Add(new Bitmap("fa7l\\fa7lL\\walk\\walkfa7l" + i + "_result.png"));
            }
            Fa7l.xadd = new int[] { 0, 0, 0, -8, -21, -27, 0, 0, 0, 0 };
            Fa7l.X = 1150;
            Fa7l.Y = floor[4].y - Fa7l.Idler[2].Height - 40;
            Fa7l.State = 0;
            Fa7l.Health = 11;
            Fa7l.Startpoint = 900;
            Fa7l.endpoint = 1400;
            Fa7l.D = 1;
            Fa7l.IdleTimer = 20;
            Fa7l.DetectRange = 400;
            Fa7l.AttackRange = 80;
            Fa7l.Speed = 10;
            Fa7l.RecoveryDuration = 12;
            Fa7l.HurtStunDuration = 6;
            Fa7l.floor = 0;
        }
        void Createskearch()
        {
            for (int i = 1; i <= 15; i++)
            {
                Skearch.Attack1r.Add(new Bitmap("skearch\\attack\\skearchattack" + i + ".png"));
                if (i <= 5) { Skearch.Deadr.Add(new Bitmap("skearch\\dead\\skearchdead" + i + ".png")); }
                if (i <= 2) { Skearch.Hurtr.Add(new Bitmap("skearch\\hurt\\skearchhurt" + i + ".png")); }
                if (i <= 7) { Skearch.Idler.Add(new Bitmap("skearch\\idle\\skearchidle" + i + ".png")); }
                if (i <= 8) { Skearch.Walkr.Add(new Bitmap("skearch\\walk\\skearchwalk" + i + ".png")); }
            }
        }

        void createskewar()
        {
            for (int i = 1; i <= 7; i++)
            {
                Skewar.Idler.Add(new Bitmap("skewar\\skewarR\\idle\\skewaridle" + i + ".png"));
                Skewar.Idlel.Add(new Bitmap("skewar\\skewarL\\idle\\skewaridle" + i + "_result.png"));
                Skewar.Walkr.Add(new Bitmap("skewar\\skewarR\\walk\\skewarwalk" + i + ".png"));
                Skewar.Walkl.Add(new Bitmap("skewar\\skewarL\\walk\\skewarwalk" + i + "_result.png"));
                if (i <= 5) { Skewar.Attack1r.Add(new Bitmap("skewar\\skewarR\\attack\\skewarattack" + i + ".png")); }
                if (i <= 5) { Skewar.Attack1l.Add(new Bitmap("skewar\\skewarL\\attack\\skewarattack" + i + "_result.png")); }
                if (i <= 4) { Skewar.Deadr.Add(new Bitmap("skewar\\skewarR\\dead\\skewardead" + i + ".png")); }
                if (i <= 4) { Skewar.Deadl.Add(new Bitmap("skewar\\skewarL\\dead\\skewardead" + i + "_result.png")); }
                if (i <= 2) { Skewar.Hurtr.Add(new Bitmap("skewar\\skewarR\\hurt\\skewarhurt" + i + ".png")); }
                if (i <= 2) { Skewar.Hurtl.Add(new Bitmap("skewar\\skewarL\\hurt\\skewarhurt" + i + "_result.png")); }
            }
            Skewar.X = 1300;
            Skewar.Y = floor[2].y - Skewar.Idler[0].Height - 30;
            Skewar.State = 0;
            Skewar.Health = 30;
            Skewar.Startpoint = 1900;
            Skewar.endpoint = 2530;
            Skewar.D = 1;
            Skewar.IdleTimer = 15;
            Skewar.DetectRange = 350;
            Skewar.AttackRange = 70;
            Skewar.Speed = 12;
            Skewar.RecoveryDuration = 10;
            Skewar.HurtStunDuration = 5;
            Skewar.floor = 2;
        }
        void createskespe()
        {
            for (int i = 1; i <= 7; i++)
            {
                Skespe.Idler.Add(new Bitmap("skespe\\skespeR\\idle\\skespeidle" + i + ".png"));
                Skespe.Idlel.Add(new Bitmap("skespe\\skespeL\\idle\\skespeidle" + i + "_result.png"));
                Skespe.Walkr.Add(new Bitmap("skespe\\skespeR\\walk\\skespewalk" + i + ".png"));
                Skespe.Walkl.Add(new Bitmap("skespe\\skespeL\\walk\\skespewalk" + i + "_result.png"));
                if (i <= 6) { Skespe.Climbr.Add(new Bitmap("skespe\\skespeR\\run\\skesperun" + i + ".png")); }
                if (i <= 6) { Skespe.Climbl.Add(new Bitmap("skespe\\skespeL\\run\\skesperun" + i + "_result.png")); }
                if (i <= 5)
                {
                    Skespe.Deadr.Add(new Bitmap("skespe\\skespeR\\dead\\skespedead" + i + ".png"));
                    Skespe.Deadl.Add(new Bitmap("skespe\\skespeL\\dead\\skespedead" + i + "_result.png"));
                    Skespe.Attack2r.Add(new Bitmap("skespe\\skespeR\\attack+run\\skesperunattack" + i + ".png"));
                    Skespe.Attack2l.Add(new Bitmap("skespe\\skespeL\\attack+run\\skesperunattack" + i + "_result.png"));
                }
                if (i <= 4)
                {
                    Skespe.Attack1r.Add(new Bitmap("skespe\\skespeR\\attack\\skespeattack" + i + ".png"));
                    Skespe.Attack1l.Add(new Bitmap("skespe\\skespeL\\attack\\skespeattack" + i + "_result.png"));
                }
                if (i <= 3)
                {
                    Skespe.Hurtr.Add(new Bitmap("skespe\\skespeR\\hurt\\skespehurt" + i + ".png"));
                    Skespe.Hurtl.Add(new Bitmap("skespe\\skespeL\\hurt\\skespehurt" + i + "_result.png"));
                }
            }
            Skespe.X = 2000;
            Skespe.Y = floor[3].y - Skespe.Idler[0].Height - 30;
            Skespe.State = 0;
            Skespe.Health = 25;
            Skespe.Startpoint = 1800;
            Skespe.endpoint = 4000;
            Skespe.D = 1;
            Skespe.IdleTimer = 15;
            Skespe.DetectRange = 500;
            Skespe.AttackRange = 70;
            Skespe.Speed = 15;
            Skespe.RecoveryDuration = 10;
            Skespe.HurtStunDuration = 5;
            Skespe.floor = 1;
        }
        void createdemon()
        {
            for (int i = 1; i <= 8; i++)
            {
                if (i <= 8) { Demon.Attack1r.Add(new Bitmap("demon\\demonR\\attack\\demonattack" + i + "_result.png")); }
                if (i <= 8) { Demon.Attack1l.Add(new Bitmap("demon\\demonL\\attack\\demonattack" + i + ".png")); }
                if (i <= 6) { Demon.Deadr.Add(new Bitmap("demon\\demonR\\dead\\demondead" + i + "_result.png")); }
                if (i <= 6) { Demon.Deadl.Add(new Bitmap("demon\\demonL\\dead\\demondead" + i + ".png")); }
                if (i <= 4) { Demon.Walkr.Add(new Bitmap("demon\\demonR\\fly\\demonfly" + i + "_result.png")); }
                if (i <= 4) { Demon.Walkl.Add(new Bitmap("demon\\demonL\\fly\\demonfly" + i + ".png")); }
                if (i <= 4) { Demon.Hurtr.Add(new Bitmap("demon\\demonR\\hurt\\demonhurt" + i + "_result.png")); }
                if (i <= 4) { Demon.Hurtl.Add(new Bitmap("demon\\demonL\\hurt\\demonhurt" + i + ".png")); }
                if (i <= 4) { Demon.Idler.Add(new Bitmap("demon\\demonR\\idle\\demonideal" + i + "_result.png")); }
                if (i <= 4) { Demon.Idlel.Add(new Bitmap("demon\\demonL\\idle\\demonideal" + i + ".png")); }
            }
            Demon.X = walls[0].x + walls[0].img.Width + 100;
            Demon.Y = floor[4].y - Demon.Idler[0].Height - 100;
            Demon.State = 0;
            Demon.Health = 40;
            Demon.Startpoint = walls[0].x + walls[0].img.Width + 100;
            Demon.endpoint = portals[0].x - 100;
            Demon.D = 1;
            Demon.IdleTimer = 15;
            Demon.DetectRange = 300;
            Demon.AttackRange = 80;
            Demon.Speed = 14;
            Demon.RecoveryDuration = 15;
            Demon.HurtStunDuration = 4;
            Demon.floor = 0;
        }
        bool CheckWalls(string Direction)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                if (Hero.X + Hero.Idler[0].Width + 45 > walls[i].x && Hero.X < walls[i].x && Hero.Y >= walls[i].y && Hero.Y < walls[i].y + walls[i].img.Height  && Direction == "d")
                {
                    return false;
                }
                else if (Hero.X - 130 < walls[i].x && Hero.X > walls[i].x  && Hero.Y >= walls[i].y  && Hero.Y < walls[i].y + walls[i].img.Height && Direction == "a")
                {
                    return false;
                }
            }
            return true;
        }
        void CreateElevator()
        {
            Elevator.Img = new Bitmap("elevator1.png");
            Elevator.X = 100;
            Elevator.Y = floor[2].y - floor[2].h;
            Elevator.Start = floor[2].y - floor[2].h;
            Elevator.End = floor[1].y - 50;
        }
        void DrawElevator(Graphics g)
        {
            g.DrawImage(Elevator.Img, Elevator.X - BStartX, Elevator.Y - BStartY);
        }
        void MoveElevator()
        {
            if (!Elevator.Direction)
            {
                Elevator.Y -= Elevator.Speed;
                if (Elevator.Y < Elevator.End)
                {
                    Elevator.Direction = true;
                }
            }
            else
            {
                Elevator.Y += Elevator.Speed;
                if (Elevator.Y > Elevator.Start)
                {
                    Elevator.Direction = false;
                }
            }
            if (Elevator.State)
            {
                Hero.Y = Elevator.Y - Hero.Idler[0].Height - 25;
            }
        }
        void CreateHealthBar()
        {
            for (int i = 0; i < 8; i++)
            {
                H.Imgs.Add(new Bitmap("healthbar\\h" + i + ".png"));
            }
        }
        void DrawHealthBar(Graphics g)
        {
            g.DrawImage(H.Imgs[H.IF], H.X, H.Y);
        }
        void CheckHeroHealth()
        {
            H.IF = Hero.Health / 12 - 1;
            if (H.IF < 0) H.IF = 0;
        }
        bool CheckGravity()
        {
            if (Hero.State != 9 && (ctJump > JumpLimit || ctJump == 0))
            {
                if (Hero.X > Elevator.X && Hero.X + Hero.Idler[0].Width < Elevator.X + Elevator.Img.Width && Hero.Y + Hero.Idler[0].Height + 50 > Elevator.Y && Hero.Y < Elevator.Y)
                {
                    Elevator.State = true;
                    return false;
                }
                else
                {
                    Elevator.State = false;
                }
                for (int i = 0; i < floor.Count; i++)
                {
                    if (Hero.Y + Hero.Idler[0].Height + 10 > floor[i].y - floor[i].h && Hero.Y + Hero.Idler[0].Height < floor[i].y)
                    { 
                        return false;
                    }
                }
                Hero.Y += 25;
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
        void CreateEntitiesandStructures()
        {

        }
    }
    
}