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
    class Tile
    {
        public int X, Y, W, H;
        public Bitmap img;
    }

    class Ball
    {
        public int X, Y;
        public int W, D;
        public int IF;
        public int Floor;
        public List<Bitmap> Charge = new List<Bitmap>();
    }
    class CActor
    {
        public int X, Y;
        public int D;
        public int Startpoint, endpoint;
        public int State;
        public int Speed = 20;
        public bool Direction;
        public bool JState;
        public int IF;
        public int IdleTimer;
        public int StunTimer;
        public int DetectRange;
        public int AttackRange;
        public int RecoveryDuration;
        public int HurtStunDuration;
        public bool IsStunned;
        public int Health;
        public bool GotHit = false;
        public bool isDead = false;
        public Bitmap Img;
        public HealthBar H = new HealthBar();
        public List<Bitmap> Imgs = new List<Bitmap>();

        public List<Bitmap> IdleR = new List<Bitmap>();
        public List<Bitmap> IdleL = new List<Bitmap>();

        public List<Bitmap> WalkR = new List<Bitmap>();
        public List<Bitmap> WalkL = new List<Bitmap>();

        public List<Bitmap> JumpR = new List<Bitmap>();
        public List<Bitmap> JumpL = new List<Bitmap>();

        public List<Bitmap> ClimbR = new List<Bitmap>();
        public List<Bitmap> ClimbL = new List<Bitmap>();

        public List<Bitmap> Attack1R = new List<Bitmap>();
        public List<Bitmap> Attack1L = new List<Bitmap>();

        public List<Bitmap> Attack2R = new List<Bitmap>();
        public List<Bitmap> Attack2L = new List<Bitmap>();

        public List<Bitmap> Attack3R = new List<Bitmap>();
        public List<Bitmap> Attack3L = new List<Bitmap>();

        public List<Bitmap> HurtR = new List<Bitmap>();
        public List<Bitmap> HurtL = new List<Bitmap>();

        public List<Bitmap> DeadR = new List<Bitmap>();
        public List<Bitmap> DeadL = new List<Bitmap>();

        public List<Bitmap> ChargeR = new List<Bitmap>();
        public List<Bitmap> ChargeL = new List<Bitmap>();
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
        Elevator Elevator = new Elevator();
        List<CActor> Ladder = new List<CActor>();
        List<Ball> FireBall = new List<Ball>();
        List<Tile> Floors = new List<Tile>();
        List<Tile> Walls = new List<Tile>();
        CActor Background = new CActor();
        List<Bitmap> idle;
        List<Bitmap> walk;
        List<Bitmap> jump;
        List<Bitmap> attack1;
        List<Bitmap> attack2;
        List<Bitmap> attack3;
        List<Bitmap> hurt;
        List<Bitmap> dead;
        List<Bitmap> climb;
        int BStartX, BStartY;
        bool keyW;
        bool keyW2;
        bool keyA;
        bool keyS;
        bool keyD;
        bool isClimbing;
        int ctJump, JumpLimit = 4;
        int Speed = 50;
        int damage = 12;
        int skespestate;
        bool skespestatechange = false;
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
            BackColor = Color.Black;
            tt.Tick += Tt_Tick; ;
            tt.Interval = 100;
            tt.Start();
        }


        private void Tt_Tick(object sender, EventArgs e)
        {
            CheckHeroHealth();
            ManageHeroMove();

            ManageActorAnimation(Hero);
            ManageActorAnimation(Fa7l);
            ManageActorAnimation(Skearch);
            ManageActorAnimation(Skespe);
            ManageActorAnimation(Skewar);
            ManageActorAnimation(Demon);

            CheckEnemyHit(Skespe);
            CheckEnemyHit(Skewar);
            CheckEnemyHit(Fa7l);

            CheckHeroHit(Skespe);
            ManageSkespeBehavior();
            Manageenemybehavior(Fa7l);
            CheckHeroHit(Fa7l);
            CheckHeroHit(Hero);

            ManageChargeAnimation();

            Jump();
            Text = Hero.State.ToString() + "  " + Hero.IF.ToString();
            CheckGravity();
            MoveElevator();

            DrawDubb(CreateGraphics());
            RunStunTimer(Hero);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            CreateFloors();
            CreateGround();
            CreateHero();
            CreateFa7l();
            CreateSkearch();
            CreateSkespe();
            CreateSkewar();
            CreateDemon();
            CreateLadder();
            CreateWall();
            CreateElevator();
            CreateHealthBar();
            BStartY = Background.Img.Height - ClientSize.Height;
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
                if (Hero.State != 9)
                {
                    Hero.IF = 0;
                    Hero.State = 9;
                }
            }
            else if (e.KeyCode == Keys.S && CheckLadder("d"))
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
                        Hero.Direction = true;
                    }
                    if (e.KeyCode == Keys.D)
                    {
                        keyD = true;
                        Hero.Direction = false;
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

                if (e.KeyCode == Keys.R)
                {
                    isAttack = true;
                    Hero.State = 6;
                }
                if (e.KeyCode == Keys.H)
                {
                    Hero.H.Amount -= damage;
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
            if (e.KeyCode == Keys.W) { keyW2 = false; }
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
            List<Bitmap> heroIdle;
            if (Hero.Direction) { heroIdle = Hero.IdleL; } else { heroIdle = Hero.IdleR; }

            if (keyA && CheckWalls("a"))
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
            else if (keyD && CheckWalls("d"))
            {
                if (Hero.State != 2)
                {
                    Hero.State = 1;
                }
                if (Hero.X + heroIdle[0].Width + 50 < Background.Img.Width)
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

            int heroWorldX = Hero.X + BStartX;
            int distance = heroWorldX - Skespe.X;
            if (distance < 0)
            {
                distance = distance * -1;
            }

            if (Hero.Floor != Skespe.Floor)
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
                    Skespe.X += Skespe.Speed * 2;
                }
                else
                {
                    Skespe.X -= Skespe.Speed * 2;
                }
            }
            else
            {
                Skespe.State = 1;
                if (heroWorldX > Skespe.X)
                {
                    Skespe.X += Skespe.Speed;
                }
                else
                {
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
            int heroWorldX = Hero.X + BStartX;
            int distance = heroWorldX - enemy.X;
            if (distance < 0)
            {
                distance = distance * -1;
            }
            if (distance <= enemy.DetectRange && Hero.Floor == enemy.Floor)
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
                    enemy.X += enemy.Speed;
                }
                else
                {
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

            if (Actor.Direction)
            {
                idle = Actor.IdleL;
                walk = Actor.WalkL;
                hurt = Actor.HurtL;
                dead = Actor.DeadL;
                attack1 = Actor.Attack1L;
                attack2 = Actor.Attack2L;
                attack3 = Actor.Attack3L;
                climb = Actor.ClimbL;
            }
            else
            {
                idle = Actor.IdleR;
                walk = Actor.WalkR;
                hurt = Actor.HurtR;
                dead = Actor.DeadR;
                attack1 = Actor.Attack1R;
                attack2 = Actor.Attack2R;
                attack3 = Actor.Attack3R;
                climb = Actor.ClimbR;
            }

            if (Actor.State == 0)
            {
                Actor.IF++;
                if (Actor.IF >= idle.Count)
                {
                    Actor.IF = 0;
                }
            }
            else if (Actor.State == 1)
            {
                Actor.IF++;
                if (Actor.IF >= walk.Count)
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
                if (Actor.IF >= hurt.Count)
                {
                    Actor.IF = 0;
                    Actor.State = 0;
                }
            }
            else if (Actor.State == 4)
            {
                Actor.IF++;
                if (Actor.IF >= dead.Count)
                {
                    Actor.IF = dead.Count - 1;
                }
            }
            else if (Actor.State == 5)
            {
                Actor.IF++;
                if (Actor.IF >= attack1.Count)
                {
                    Actor.IF = 0;
                    Actor.State = 0;
                    Fa7l.GotHit = false;
                }
            }
            else if (Actor.State == 6)
            {
                Actor.IF++;
                if (Actor.IF >= attack2.Count)
                {
                    if (Actor == Hero)
                    {
                        Ball pnn = new Ball();
                        pnn.X = Hero.X + BStartX + attack2[attack2.Count - 1].Width;
                        pnn.Y = Hero.Y;
                        pnn.W = 1;
                        pnn.Floor = Actor.Floor;
                        for (int i = 1; i <= 12; i++)
                        {
                            pnn.Charge.Add(new Bitmap("hero\\charge\\herocharge" + i + ".png"));
                        }
                        FireBall.Add(pnn);
                    }
                    Actor.IF = 0;
                    Actor.State = 0;
                    Actor.GotHit = false;
                }
            }
            else if (Actor.State == 7)
            {
                Actor.IF++;
                if (Actor.IF >= attack3.Count - 3)
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
                if (Actor.IF >= climb.Count)
                {
                    Actor.IF = 0;
                }
            }
        }

        void ManageChargeAnimation()
        {
            if (FireBall.Count > 0)
            {
                for (int i = 0; i < FireBall.Count; i++)
                {
                    Ball ptrv = FireBall[i];
                    if (ptrv.W == 1)
                    {
                        ptrv.X += 10;
                        ptrv.IF++;
                        if (ptrv.IF >= 5)
                        {
                            ptrv.IF = 0;
                        }
                        bool hit = false;

                        if (!Fa7l.isDead && ptrv.X + ptrv.Charge[ptrv.IF].Width >= Fa7l.X - BStartX && ptrv.X < Fa7l.X - BStartX + 50 && ptrv.Floor == Fa7l.Floor)
                        {
                            Fa7l.Health -= 5;
                            Fa7l.GotHit = true;
                            CheckDead(Fa7l);
                            hit = true;
                        }
                        if (!Skewar.isDead && ptrv.X + ptrv.Charge[ptrv.IF].Width >= Skewar.X - BStartX && ptrv.X < Skewar.X - BStartX + 50 && ptrv.Floor == Skewar.Floor)
                        {
                            Skewar.Health -= 5;
                            Skewar.GotHit = true;
                            CheckDead(Skewar);
                            hit = true;
                        }
                        if (!Skespe.isDead && ptrv.X + ptrv.Charge[ptrv.IF].Width >= Skespe.X - BStartX && ptrv.X < Skespe.X - BStartX + 50 && ptrv.Floor == Skespe.Floor)
                        {
                            Skespe.Health -= 5;
                            Skespe.GotHit = true;
                            CheckDead(Skespe);
                            hit = true;
                        }
                        if (ptrv.X > ClientSize.Width)
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
                            FireBall.RemoveAt(i);
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
            int enemyScreenX = enemy.X - BStartX;
            if (enemy.State != 5 || enemy.isDead)
            {
                Hero.GotHit = false;
                return;
            }

            if (!Hero.GotHit)
            {
                if (Hero.X > enemyScreenX && enemyScreenX + attack1[enemy.IF].Width >= Hero.X - 40)
                {
                    Hero.Health -= 10;
                    Hero.GotHit = true;
                    CheckDead(Hero);
                }
                else if (Hero.X < enemyScreenX && Hero.X + idle[0].Width >= enemyScreenX - 30)
                {
                    Hero.Health -= 10;
                    Hero.GotHit = true;
                    CheckDead(Hero);
                }
            }
        }
        void CheckEnemyHit(CActor enemy)
        {
            List<Bitmap> attack1Hero;
            List<Bitmap> attack3Hero;
            if (Hero.Direction)
            {
                attack1Hero = Hero.Attack1L;
                attack3Hero = Hero.Attack3L;
            }
            else
            {
                attack1Hero = Hero.Attack1R;
                attack3Hero = Hero.Attack3R;
            }
            if (enemy.Direction)
            {
                attack1 = enemy.Attack1L;
            }
            else
            {
                attack1 = enemy.Attack1R;
            }
            if (Hero.Direction)
            {
                idle = Hero.IdleL;
            }
            else
            {
                idle = Hero.IdleR;
            }
            if (enemy.isDead) { return; }
            if (Hero.State == 5 && (Hero.X + attack1Hero[Hero.IF].Width >= enemy.X - BStartX - 30) && enemy.X - BStartX > Hero.X && isAttack)
            {
                if (!enemy.GotHit)
                {
                    enemy.Health -= 10;
                    enemy.IF = 0;
                    enemy.GotHit = true;
                    CheckDead(enemy);
                }
            }
            else if (Hero.State == 7 && (Hero.X + attack3Hero[Hero.IF].Width >= enemy.X - BStartX - 30) && enemy.X - BStartX > Hero.X && isAttack)
            {
                enemy.Health -= 1;
                enemy.IF = 0;
                enemy.GotHit = true;
                CheckDead(enemy);
            }
            if (FireBall.Count > 0)
            {
                for (int i = 0; i < FireBall.Count; i++)
                {
                    Ball ptrv = FireBall[i];
                    if (ptrv.W == 1 && ptrv.Floor == enemy.Floor)
                    {
                        int FireBallWorldX = ptrv.X + BStartX;
                        int enemyWidth = 50;
                        if (FireBallWorldX + ptrv.Charge[ptrv.IF].Width >= enemy.X && FireBallWorldX < enemy.X + enemyWidth)
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
                if (Hero.X > Ladder[i].X && Hero.X + Hero.IdleR[0].Width < Ladder[i].X + Ladder[i].Img.Width && Hero.Y + Hero.IdleR[0].Height > Ladder[i].Y - 30 && Hero.Y + Hero.IdleR[0].Height < Ladder[i].Y + Ladder[i].Img.Height && Direction == "u" && Hero.State != 1 && Hero.State != 2)
                {
                    Hero.X = Ladder[i].X + Ladder[i].Img.Width / 2 - Hero.IdleR[0].Width / 2;
                    if (BStartY + ClientSize.Height + Speed < Background.Img.Height)
                    {
                        if (!isClimbing)
                        {
                            Hero.Floor++;
                            isClimbing = true;
                        }
                        Hero.Y -= Speed;
                    }
                    else
                    {
                        Hero.Y -= Speed;
                    }
                    if (Hero.Y < Ladder[i].Y)
                    {
                        Hero.Y = Floors[Ladder[i].Floor].Y - Floors[Ladder[i].Floor].H - Hero.IdleR[0].Height;
                        Hero.State = 0;
                        if (isClimbing)
                        {
                            Hero.Floor--;
                            isClimbing = false;
                        }
                        return false;
                    }
                    return true;
                }
                else if (Hero.X > Ladder[i].X && Hero.X + Hero.IdleR[0].Width < Ladder[i].X + Ladder[i].Img.Width && Hero.Y + Hero.IdleR[0].Height > Ladder[i].Y - 100 && Hero.Y + Hero.IdleR[0].Height < Ladder[i].Y + Ladder[i].Img.Height && Direction == "d" && Hero.State != 1 && Hero.State != 2)
                {
                    Hero.X = Ladder[i].X + Ladder[i].Img.Width / 2 - Hero.IdleR[0].Width / 2;
                    if (Speed < Background.Img.Height - ClientSize.Height)
                    {
                        Hero.Y += Speed;
                    }
                    else
                    {
                        Hero.Y += Speed;
                    }
                    if (Hero.Y + Hero.IdleR[0].Height > Ladder[i].Y + Ladder[i].Img.Height - 70)
                    {
                        Hero.Y = Floors[Ladder[i].Floor + 1].Y - Floors[Ladder[i].Floor + 1].H - Hero.IdleR[0].Height;
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
            g2.Clear(BackColor);
            CheckBorders();
            DrawBackground(g2);
            DrawLadder(g2);
            DrawFireBall(g2);
            DrawHero(g2);
            DrawFa7l(g2);
            DrawSkearch(g2);
            DrawSkewar(g2);
            DrawSkespe(g2);
            DrawDemon(g2);
            DrawFloors(g2);
            DrawWalls(g2);
            DrawElevator(g2);
            DrawHealthBar(g2);
        }
        void DrawBackground(Graphics g)
        {
            Rectangle rcDst = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            Rectangle rcSrc = new Rectangle(BStartX, BStartY, ClientSize.Width, ClientSize.Height);
            g.DrawImage(Background.Img, rcDst, rcSrc, GraphicsUnit.Pixel);
        }

        void DrawFloors(Graphics g)
        {
            Pen pn = new Pen(Color.Yellow, 2);
            for (int i = 0; i < 5; i++)
            {
                Tile ptrv = Floors[i];
                // g.DrawRectangle(pn, ptrv.X - BStartX, ptrv.Y - BStartY, ptrv.W, ptrv.H);
            }
        }
        void DrawWalls(Graphics g)
        {
            Pen pn = new Pen(Color.Yellow, 2);
            for (int i = 0; i < 5; i++)
            {
                Tile ptrv = Walls[i];
                // g.DrawRectangle(pn, ptrv.X - BStartX, ptrv.Y - BStartY, ptrv.img.Width, ptrv.img.Height);
                g.DrawImage(ptrv.img, ptrv.X - BStartX, ptrv.Y - BStartY);
            }
        }
        void DrawLadder(Graphics g)
        {
            for (int i = 0; i < Ladder.Count; i++)
            {
                g.DrawImage(Ladder[i].Img, Ladder[i].X - BStartX, Ladder[i].Y - BStartY);
            }
        }
        void DrawHero(Graphics g)
        {
            int frameToDraw = Hero.IF;



            if (Hero.Direction)
            {
                idle = Hero.IdleL;
                walk = Hero.WalkL;
                jump = Hero.JumpL;
                attack1 = Hero.Attack1L;
                attack2 = Hero.Attack2L;
                attack3 = Hero.Attack3L;
                climb = Hero.ClimbL;
            }
            else
            {
                idle = Hero.IdleR;
                walk = Hero.WalkR;
                jump = Hero.JumpR;
                attack1 = Hero.Attack1R;
                attack2 = Hero.Attack2R;
                attack3 = Hero.Attack3R;
                climb = Hero.ClimbR;
            }

            if (Hero.State == 0 && idle.Count > frameToDraw)
            {
                g.DrawImage(idle[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
            }
            else if (Hero.State == 1)
            {
                g.DrawImage(walk[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
            }
            else if (Hero.State == 2)
            {
                g.DrawImage(jump[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
            }
            else if (Hero.State == 5)
            {
                g.DrawImage(attack1[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
            }
            else if (Hero.State == 6)
            {
                g.DrawImage(attack2[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
            }
            else if (Hero.State == 7)
            {
                g.DrawImage(attack3[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
            }
            else if (Hero.State == 9)
            {
                g.DrawImage(climb[frameToDraw], Hero.X - BStartX, Hero.Y - BStartY);
            }
        }
        void DrawFa7l(Graphics g)
        {


            if (Fa7l.Direction)
            {
                idle = Fa7l.IdleL;
                walk = Fa7l.WalkL;
                hurt = Fa7l.HurtL;
                dead = Fa7l.DeadL;
                attack1 = Fa7l.Attack1L;
            }
            else
            {
                idle = Fa7l.IdleR;
                walk = Fa7l.WalkR;
                hurt = Fa7l.HurtR;
                dead = Fa7l.DeadR;
                attack1 = Fa7l.Attack1R;
            }

            int fa7lDrawX = Fa7l.X - BStartX;
            if (Fa7l.IF < Fa7l.Xadd.Length)
            {
                fa7lDrawX += Fa7l.Xadd[Fa7l.IF];
            }
            if (Fa7l.State == 0) { g.DrawImage(idle[Fa7l.IF], fa7lDrawX, Fa7l.Y - BStartY); }
            else if (Fa7l.State == 1) { g.DrawImage(walk[Fa7l.IF], Fa7l.X - BStartX, Fa7l.Y - BStartY); }
            else if (Fa7l.State == 3) { g.DrawImage(hurt[Fa7l.IF], Fa7l.X - BStartX, Fa7l.Y - BStartY); }
            else if (Fa7l.State == 4) { g.DrawImage(dead[Fa7l.IF], Fa7l.X - BStartX, Fa7l.Y - BStartY); }
            else if (Fa7l.State == 5) { g.DrawImage(attack1[Fa7l.IF], Fa7l.X - BStartX, Fa7l.Y - BStartY); }
            else { g.DrawImage(idle[0], Fa7l.X, Fa7l.Y); }
        }

        void DrawSkearch(Graphics g)
        {


            if (Skearch.Direction)
            {
                idle = Skearch.IdleL;
                walk = Skearch.WalkL;
                hurt = Skearch.HurtL;
                dead = Skearch.DeadL;
                attack1 = Skearch.Attack1L;
            }
            else
            {
                idle = Skearch.IdleR;
                walk = Skearch.WalkR;
                hurt = Skearch.HurtR;
                dead = Skearch.DeadR;
                attack1 = Skearch.Attack1R;
            }

            if (Skearch.State == 0) { g.DrawImage(idle[Skearch.IF], Skearch.X - BStartX, Skearch.Y - BStartY); }
            else if (Skearch.State == 1) { g.DrawImage(walk[Skearch.IF], Skearch.X - BStartX, Skearch.Y - BStartY); }
            else if (Skearch.State == 3) { g.DrawImage(hurt[Skearch.IF], Skearch.X - BStartX, Skearch.Y - BStartY); }
            else if (Skearch.State == 4) { g.DrawImage(dead[Skearch.IF], Skearch.X - BStartX, Skearch.Y - BStartY); }
            else if (Skearch.State == 5) { g.DrawImage(attack1[Skearch.IF], Skearch.X - BStartX, Skearch.Y - BStartY); }
            else { g.DrawImage(idle[0], Skearch.X, Skearch.Y); }
        }
        void DrawSkewar(Graphics g)
        {
            if (Skewar.Direction)
            {
                idle = Skewar.IdleL;
                walk = Skewar.WalkL;
                hurt = Skewar.HurtL;
                dead = Skewar.DeadL;
                attack1 = Skewar.Attack1L;
            }
            else
            {
                idle = Skewar.IdleR;
                walk = Skewar.WalkR;
                hurt = Skewar.HurtR;
                dead = Skewar.DeadR;
                attack1 = Skewar.Attack1R;
            }

            if (Skewar.State == 0) { g.DrawImage(idle[Skewar.IF], Skewar.X - BStartX, Skewar.Y - BStartY); }
            else if (Skewar.State == 1) { g.DrawImage(walk[Skewar.IF], Skewar.X - BStartX, Skewar.Y - BStartY); }
            else if (Skewar.State == 3) { g.DrawImage(hurt[Skewar.IF], Skewar.X - BStartX, Skewar.Y - BStartY); }
            else if (Skewar.State == 4) { g.DrawImage(dead[Skewar.IF], Skewar.X - BStartX, Skewar.Y - BStartY); }
            else if (Skewar.State == 5) { g.DrawImage(attack1[Skewar.IF], Skewar.X - BStartX, Skewar.Y - BStartY); }
            else { g.DrawImage(idle[0], Skewar.X, Skewar.Y); }
        }
        void DrawSkespe(Graphics g)
        {

            if (Skespe.Direction)
            {
                idle = Skespe.IdleL;
                walk = Skespe.WalkL;
                hurt = Skespe.HurtL;
                dead = Skespe.DeadL;
                attack1 = Skespe.Attack1L;
            }
            else
            {
                idle = Skespe.IdleR;
                walk = Skespe.WalkR;
                hurt = Skespe.HurtR;
                dead = Skespe.DeadR;
                attack1 = Skespe.Attack1R;
            }

            if (Skespe.State == 0) { g.DrawImage(idle[Skespe.IF], Skespe.X - BStartX, Skespe.Y - BStartY); }
            else if (Skespe.State == 1) { g.DrawImage(walk[Skespe.IF], Skespe.X - BStartX, Skespe.Y - BStartY); }
            else if (Skespe.State == 3) { g.DrawImage(hurt[Skespe.IF], Skespe.X - BStartX, Skespe.Y - BStartY); }
            else if (Skespe.State == 4) { g.DrawImage(dead[Skespe.IF], Skespe.X - BStartX, Skespe.Y - BStartY); }
            else if (Skespe.State == 5) { g.DrawImage(attack1[Skespe.IF], Skespe.X - BStartX, Skespe.Y - BStartY); }
            else { g.DrawImage(idle[0], Skespe.X, Skespe.Y); }
        }
        void DrawDemon(Graphics g)
        {
            if (Demon.Direction)
            {
                idle = Demon.IdleL;
                walk = Demon.WalkL;
                hurt = Demon.HurtL;
                dead = Demon.DeadL;
                attack1 = Demon.Attack1L;
            }
            else
            {
                idle = Demon.IdleR;
                walk = Demon.WalkR;
                hurt = Demon.HurtR;
                dead = Demon.DeadR;
                attack1 = Demon.Attack1R;
            }

            if (Demon.State == 0) { g.DrawImage(idle[Demon.IF], Demon.X - BStartX, Demon.Y - BStartY); }
            else if (Demon.State == 1) { g.DrawImage(walk[Demon.IF], Demon.X - BStartX, Demon.Y - BStartY); }
            else if (Demon.State == 3) { g.DrawImage(hurt[Demon.IF], Demon.X - BStartX, Demon.Y - BStartY); }
            else if (Demon.State == 4) { g.DrawImage(dead[Demon.IF], Demon.X - BStartX, Demon.Y - BStartY); }
            else if (Demon.State == 5) { g.DrawImage(attack1[Demon.IF], Demon.X - BStartX, Demon.Y - BStartY); }
            else { g.DrawImage(idle[0], Demon.X, Demon.Y); }
        }

        void DrawFireBall(Graphics g)
        {
            for (int i = 0; i < FireBall.Count; i++)
            {
                g.DrawImage(FireBall[i].Charge[FireBall[i].IF], FireBall[i].X - BStartX, FireBall[i].Y - BStartY);
            }
        }
        void CreateGround()
        {
            Background.Img = new Bitmap("map.png");
            Background.X = 0;
            Background.Y = 0;
        }
        void CreateFloors()
        {
            int Floorsy = 450, yadd = 0;
            for (int i = 0; i < 5; i++)
            {
                Tile pnn = new Tile();
                pnn.X = 0;
                pnn.Y = Floorsy + yadd;
                yadd += 436 + 21;
                pnn.W = 4245;
                pnn.H = 26;
                Floors.Add(pnn);
            }
        }
        void CreateWall()
        {
            Tile pnn = new Tile();
            pnn.X = 2280;
            pnn.Y = 1857;
            pnn.img = new Bitmap("pillars\\p51.png");
            Walls.Add(pnn);
            pnn = new Tile();
            pnn.X = 1560;
            pnn.Y = 1390;
            pnn.img = new Bitmap("pillars\\p411.png");
            Walls.Add(pnn);
            pnn = new Tile();
            pnn.X = 840;
            pnn.Y = 935;
            pnn.img = new Bitmap("pillars\\p3.png");
            Walls.Add(pnn);
            pnn = new Tile();
            pnn.X = 2630;
            pnn.Y = 938;
            pnn.img = new Bitmap("pillars\\p31.png");
            Walls.Add(pnn);
            pnn = new Tile();
            pnn.X = 1830;
            pnn.Y = 478;
            pnn.img = new Bitmap("pillars\\p21.png");
            Walls.Add(pnn);
        }
        void CreateLadder()
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

        void CreateHero()
        {
            for (int i = 1; i <= 14; i++)
            {
                if (i <= 7)
                {
                    Hero.IdleR.Add(new Bitmap("hero\\heroR\\idle\\heroidle" + i + ".png"));
                    Hero.IdleL.Add(new Bitmap("hero\\heroL\\idle\\heroidle" + i + ".png"));
                }

                if (i <= 8)
                {
                    Hero.WalkR.Add(new Bitmap("hero\\heroR\\walk\\herorun" + i + ".png"));
                    Hero.WalkL.Add(new Bitmap("hero\\heroL\\walk\\herorun" + i + ".png"));
                }

                if (i <= 4)
                {
                    Hero.Attack1R.Add(new Bitmap("hero\\heroR\\attack1\\heromeleeattack" + i + ".png"));
                    Hero.Attack1L.Add(new Bitmap("hero\\heroL\\attack1\\heromeleeattack" + i + ".png"));
                }

                if (i <= 8)
                {
                    Hero.Attack2R.Add(new Bitmap("hero\\heroR\\attack2\\heroattackFireBall" + i + ".png"));
                    Hero.Attack2L.Add(new Bitmap("hero\\heroL\\attack2\\heroattackFireBall" + i + ".png"));
                }

                Hero.Attack3R.Add(new Bitmap("hero\\heroR\\attack3\\heroflameattack" + i + ".png"));
                Hero.Attack3L.Add(new Bitmap("hero\\heroL\\attack3\\heroflameattack" + i + ".png"));

                if (i <= 9)
                {
                    Hero.JumpR.Add(new Bitmap("hero\\heroR\\jump\\herojump" + i + ".png"));
                    Hero.JumpL.Add(new Bitmap("hero\\heroL\\jump\\herojump" + i + ".png"));
                }

                if (i <= 6)
                {
                    Hero.DeadR.Add(new Bitmap("hero\\heroR\\dead\\herodead" + i + ".png"));
                    Hero.DeadL.Add(new Bitmap("hero\\heroL\\dead\\herodead" + i + ".png"));
                }

                if (i <= 3)
                {
                    Hero.HurtR.Add(new Bitmap("hero\\heroR\\hurt\\herohurt" + i + ".png"));
                    Hero.HurtL.Add(new Bitmap("hero\\heroL\\hurt\\herohurt" + i + ".png"));
                }

                if (i <= 2)
                {
                    Hero.ClimbR.Add(new Bitmap("hero\\heroR\\climb\\c" + i + ".png"));
                    Hero.ClimbL.Add(new Bitmap("hero\\heroL\\climb\\c" + i + ".png"));
                }
            }

            Hero.X = ClientSize.Width / 2 - Hero.IdleR[0].Width / 2;
            Hero.Y = Floors[Floors.Count - 1].Y - Floors[Floors.Count - 1].H - Hero.IdleR[0].Height;
            Hero.State = 0;
            Hero.Health = 100;
            Hero.HurtStunDuration = 8;
            Hero.Floor = 0;
        }

        void CreateFa7l()
        {
            for (int i = 1; i <= 12; i++)
            {
                if (i <= 5)
                {
                    Fa7l.Attack1R.Add(new Bitmap("fa7l\\fa7lR\\attack\\attackfa7l" + i + ".png"));
                    Fa7l.Attack1L.Add(new Bitmap("fa7l\\fa7lL\\attack\\attackfa7l" + i + ".png"));

                    Fa7l.DeadR.Add(new Bitmap("fa7l\\fa7lR\\dead\\deadfa7l" + i + ".png"));
                    Fa7l.DeadL.Add(new Bitmap("fa7l\\fa7lL\\dead\\deadfa7l" + i + ".png"));
                }

                if (i <= 3)
                {
                    Fa7l.HurtR.Add(new Bitmap("fa7l\\fa7lR\\hurt\\hurtfa7l" + i + ".png"));
                    Fa7l.HurtL.Add(new Bitmap("fa7l\\fa7lL\\hurt\\hurtfa7l" + i + ".png"));
                }

                if (i <= 10)
                {
                    Fa7l.IdleR.Add(new Bitmap("fa7l\\fa7lR\\idle\\idlefa7l" + i + ".png"));
                    Fa7l.IdleL.Add(new Bitmap("fa7l\\fa7lL\\idle\\idlefa7l" + i + ".png"));
                }

                Fa7l.WalkR.Add(new Bitmap("fa7l\\fa7lR\\walk\\walkfa7l" + i + ".png"));
                Fa7l.WalkL.Add(new Bitmap("fa7l\\fa7lL\\walk\\walkfa7l" + i + ".png"));
            }

            Fa7l.Xadd = new int[] { 0, 0, 0, -8, -21, -27, 0, 0, 0, 0 };
            Fa7l.X = 1150;
            Fa7l.Y = Floors[4].Y - Floors[4].H - Fa7l.Attack1R[2].Height;
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
            Fa7l.Floor = 0;
        }
        void CreateSkearch()
        {
            for (int i = 1; i <= 15; i++)
            {
                Skearch.Attack1R.Add(new Bitmap("skearch\\skearchR\\attack\\skearchattack" + i + ".png"));
                Skearch.Attack1L.Add(new Bitmap("skearch\\skearchL\\attack\\skearchattack" + i + ".png"));

                if (i <= 5)
                {
                    Skearch.DeadR.Add(new Bitmap("skearch\\skearchR\\dead\\skearchdead" + i + ".png"));
                    Skearch.DeadL.Add(new Bitmap("skearch\\skearchL\\dead\\skearchdead" + i + ".png"));
                }

                if (i <= 2)
                {
                    Skearch.HurtR.Add(new Bitmap("skearch\\skearchR\\hurt\\skearchhurt" + i + ".png"));
                    Skearch.HurtL.Add(new Bitmap("skearch\\skearchL\\hurt\\skearchhurt" + i + ".png"));
                }

                if (i <= 7)
                {
                    Skearch.IdleR.Add(new Bitmap("skearch\\skearchR\\idle\\skearchidle" + i + ".png"));
                    Skearch.IdleL.Add(new Bitmap("skearch\\skearchL\\idle\\skearchidle" + i + ".png"));
                }

                if (i <= 8)
                {
                    Skearch.WalkR.Add(new Bitmap("skearch\\skearchR\\walk\\skearchwalk" + i + ".png"));
                    Skearch.WalkL.Add(new Bitmap("skearch\\skearchL\\walk\\skearchwalk" + i + ".png"));
                }
            }

            Skearch.X = 1050;
            Skearch.Y = Floors[4].Y - Floors[4].H - Skearch.Attack1R[2].Height;
        }

        void CreateSkewar()
        {
            for (int i = 1; i <= 7; i++)
            {
                Skewar.IdleR.Add(new Bitmap("skewar\\skewarR\\idle\\skewaridle" + i + ".png"));
                Skewar.IdleL.Add(new Bitmap("skewar\\skewarL\\idle\\skewaridle" + i + ".png"));

                Skewar.WalkR.Add(new Bitmap("skewar\\skewarR\\walk\\skewarwalk" + i + ".png"));
                Skewar.WalkL.Add(new Bitmap("skewar\\skewarL\\walk\\skewarwalk" + i + ".png"));

                if (i <= 5)
                {
                    Skewar.Attack1R.Add(new Bitmap("skewar\\skewarR\\attack\\skewarattack" + i + ".png"));
                    Skewar.Attack1L.Add(new Bitmap("skewar\\skewarL\\attack\\skewarattack" + i + ".png"));
                }

                if (i <= 4)
                {
                    Skewar.DeadR.Add(new Bitmap("skewar\\skewarR\\dead\\skewardead" + i + ".png"));
                    Skewar.DeadL.Add(new Bitmap("skewar\\skewarL\\dead\\skewardead" + i + ".png"));
                }

                if (i <= 2)
                {
                    Skewar.HurtR.Add(new Bitmap("skewar\\skewarR\\hurt\\skewarhurt" + i + ".png"));
                    Skewar.HurtL.Add(new Bitmap("skewar\\skewarL\\hurt\\skewarhurt" + i + ".png"));
                }
            }

            Skewar.X = 1300;
            Skewar.Y = Floors[2].Y - Skewar.IdleR[0].Height - 30;
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
            Skewar.Floor = 2;
        }
        void CreateSkespe()
        {
            for (int i = 1; i <= 7; i++)
            {
                Skespe.IdleR.Add(new Bitmap("skespe\\skespeR\\idle\\skespeidle" + i + ".png"));
                Skespe.IdleL.Add(new Bitmap("skespe\\skespeL\\idle\\skespeidle" + i + ".png"));

                Skespe.WalkR.Add(new Bitmap("skespe\\skespeR\\walk\\skespewalk" + i + ".png"));
                Skespe.WalkL.Add(new Bitmap("skespe\\skespeL\\walk\\skespewalk" + i + ".png"));

                if (i <= 6)
                {
                    Skespe.WalkR.Add(new Bitmap("skespe\\skespeR\\run\\skesperun" + i + ".png"));
                    Skespe.WalkL.Add(new Bitmap("skespe\\skespeL\\run\\skesperun" + i + ".png"));
                }

                if (i <= 5)
                {
                    Skespe.DeadR.Add(new Bitmap("skespe\\skespeR\\dead\\skespedead" + i + ".png"));
                    Skespe.DeadL.Add(new Bitmap("skespe\\skespeL\\dead\\skespedead" + i + ".png"));

                    Skespe.Attack2R.Add(new Bitmap("skespe\\skespeR\\attack+run\\skesperunattack" + i + ".png"));
                    Skespe.Attack2L.Add(new Bitmap("skespe\\skespeL\\attack+run\\skesperunattack" + i + ".png"));
                }

                if (i <= 4)
                {
                    Skespe.Attack1R.Add(new Bitmap("skespe\\skespeR\\attack\\skespeattack" + i + ".png"));
                    Skespe.Attack1L.Add(new Bitmap("skespe\\skespeL\\attack\\skespeattack" + i + ".png"));

                    Skespe.ChargeR.Add(new Bitmap("skespe\\Arrow.png"));
                    Skespe.ChargeL.Add(new Bitmap("skespe\\Arrow.png"));
                }

                if (i <= 3)
                {
                    Skespe.HurtR.Add(new Bitmap("skespe\\skespeR\\hurt\\skespehurt" + i + ".png"));
                    Skespe.HurtL.Add(new Bitmap("skespe\\skespeL\\hurt\\skespehurt" + i + ".png"));
                }
            }

            Skespe.X = 2000;
            Skespe.Y = Floors[3].Y - Skespe.IdleR[0].Height - 30;
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
            Skespe.Floor = 1;
        }

        void CreateDemon()
        {
            for (int i = 1; i <= 8; i++)
            {
                if (i <= 8)
                {
                    Demon.Attack1R.Add(new Bitmap("demon\\demonR\\attack\\demonattack" + i + ".png"));
                    Demon.Attack1L.Add(new Bitmap("demon\\demonL\\attack\\demonattack" + i + ".png"));
                }

                if (i <= 6)
                {
                    Demon.DeadR.Add(new Bitmap("demon\\demonR\\dead\\demondead" + i + ".png"));
                    Demon.DeadL.Add(new Bitmap("demon\\demonL\\dead\\demondead" + i + ".png"));
                }

                if (i <= 4)
                {
                    Demon.WalkR.Add(new Bitmap("demon\\demonR\\fly\\demonfly" + i + ".png"));
                    Demon.WalkL.Add(new Bitmap("demon\\demonL\\fly\\demonfly" + i + ".png"));
                }

                if (i <= 4)
                {
                    Demon.HurtR.Add(new Bitmap("demon\\demonR\\hurt\\demonhurt" + i + ".png"));
                    Demon.HurtL.Add(new Bitmap("demon\\demonL\\hurt\\demonhurt" + i + ".png"));
                }

                if (i <= 4)
                {
                    Demon.IdleR.Add(new Bitmap("demon\\demonR\\idle\\demonideal" + i + ".png"));
                    Demon.IdleL.Add(new Bitmap("demon\\demonL\\idle\\demonideal" + i + ".png"));
                }
            }

            Demon.X = 2930;
            Demon.Y = Floors[4].Y - Floors[4].H - Demon.Attack1R[2].Height;
            Demon.Startpoint = 2930;
            Demon.endpoint = 3660;
        }
        bool CheckWalls(string Direction)
        {
            List<Bitmap> heroIdle;
            if (Hero.Direction) { heroIdle = Hero.IdleL; } else { heroIdle = Hero.IdleR; }

            for (int i = 0; i < Walls.Count; i++)
            {
                if (Hero.X + heroIdle[0].Width + 45 > Walls[i].X && Hero.X < Walls[i].X && Hero.Y >= Walls[i].Y && Hero.Y < Walls[i].Y + Walls[i].img.Height && Direction == "d")
                {
                    return false;
                }
                else if (Hero.X - 130 < Walls[i].X && Hero.X > Walls[i].X && Hero.Y >= Walls[i].Y && Hero.Y < Walls[i].Y + Walls[i].img.Height && Direction == "a")
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
        bool CheckGravity()
        {
            List<Bitmap> heroIdle;
            if (Hero.Direction) { heroIdle = Hero.IdleL; } else { heroIdle = Hero.IdleR; }

            if (Hero.State != 9 && (ctJump > JumpLimit || ctJump == 0))
            {
                if (Hero.X > Elevator.X && Hero.X + heroIdle[0].Width < Elevator.X + Elevator.Img.Width && Hero.Y + heroIdle[0].Height + 50 > Elevator.Y && Hero.Y < Elevator.Y)
                {
                    Elevator.State = true;
                    return false;
                }
                else
                {
                    Elevator.State = false;
                }
                for (int i = 0; i < Floors.Count; i++)
                {
                    if (Hero.Y + heroIdle[0].Height + 10 > Floors[i].Y - Floors[i].H && Hero.Y + heroIdle[0].Height < Floors[i].Y)
                    {
                        return false;
                    }
                }
                Hero.Y += 25;
            }
            return true;
        }
        void CreateElevator()
        {
            Elevator.Img = new Bitmap("elevator1.png");
            Elevator.X = 100;
            Elevator.Y = Floors[4].Y - Floors[4].H;
            Elevator.Start = Floors[4].Y - Floors[4].H;
            Elevator.End = Floors[3].Y - 50;
        }
        void DrawElevator(Graphics g)
        {
            g.DrawImage(Elevator.Img, Elevator.X - BStartX, Elevator.Y - BStartY);
        }
        void MoveElevator()
        {
            if (Hero.Direction) { idle = Hero.IdleL; }
            else { idle = Hero.IdleR; }

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
                Hero.Y = Elevator.Y - idle[0].Height - 25;
            }
        }

        void CreateHealthBar()
        {
            for (int i = 0; i < 8; i++)
            {
                Hero.H.Imgs.Add(new Bitmap("healthbar\\h" + i + ".png"));
            }
        }
        void DrawHealthBar(Graphics g)
        {
            g.DrawImage(Hero.H.Imgs[Hero.H.IF], Hero.H.X, Hero.H.Y);
        }
        void CheckHeroHealth()
        {
            Hero.H.IF = Hero.H.Amount / 12 - 1;
            if (Hero.H.IF < 0) Hero.H.IF = 0;
        }
    }
}