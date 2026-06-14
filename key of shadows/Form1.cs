using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace key_of_shadows
{
    public class laser
    {
        public int X, Y, W, H, D;
        public bool held = true;
        public int strmax = 800;
    }
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
        public int x, y, w, h;
        public int cf = 0;
        public Bitmap img;
        public List<Bitmap> imgs = new List<Bitmap>();
    }

    class ball
    {
        public int type;
        public int from;
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
        public int AttackRange;
        public int RecoveryDuration;
        public int HurtStunDuration;
        public int Health;
        public bool JState;
        public bool IsStunned;
        public bool GotHit = false;
        public bool isDead = false;
        public bool sinbullet = false;
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

    class MiniGame
    {
        public Bitmap Background;
        public int yStart;
        public int yStart2;
        public int State;
        public Tile Rocket = new Tile();
        public int RocketHealth = 5;
        public List<Tile> RocketBullets = new List<Tile>();
        public Tile Boss = new Tile();
        public int[] BossMovement = { 35, 35, 35, 35, 35, 35, 35, 35, 15, 15, 15, 15, 55, 55, 55, 55, 55, -35, -35, -35, -35, 55, 55, 55, 55, 55, 55, -15, -15, -15, -15, -55, -55, -55, -55, -55, -55, -55, -55, -35, -35, -35, -35, -35, -35, -35, -35 };
        public int IM;
        public int BossHealth = 5;
        public List<Tile> BossBullets = new List<Tile>();
        public Bitmap Heart;
        public List<Tile> Chickens = new List<Tile>();
        public List<Tile> Eggs = new List<Tile>();

    }
    public partial class Form1 : Form
    {
        bool LaserOn = false;
        bool isAttack = false;
        Bitmap off;
        Tile Door = new Tile();
        Timer tt = new Timer();
        CActor Hero = new CActor();
        CActor tor = new CActor();
        CActor Skearch = new CActor();
        CActor Skespe = new CActor();
        CActor Skewar = new CActor();
       
        CActor Sat = new CActor();
        List<CActor> Ladder = new List<CActor>();
        List<ball> Fireball = new List<ball>();
        List<Tile> floor = new List<Tile>();
        List<Tile> walls = new List<Tile>();
        List<Tile> portals = new List<Tile>();
        List<Tile> tables = new List<Tile>();
        List<Tile> Key = new List<Tile>();
        List<Tile> Healers = new List<Tile>();
        List<Tile> Ability = new List<Tile>();
        List<Tile> Start = new List<Tile>();
        List<CActor> Background = new List<CActor>();
        HealthBar H = new HealthBar();
        Elevator Elevator = new Elevator();
        MiniGame Game = new MiniGame();
        List<laser> lasers = new List<laser>();
        int BStartX, BStartY;
        int Fstate = 0;
        bool keyW;
        bool keyW2;
        bool keyA;
        bool keyS;
        bool keyD;
        bool keyE;
        int ctJump, JumpLimit = 4;
        int Speed = 30;
        int ctEgg, ctBoss;
        int skespestate;
        bool skespestatechange = false;

        CActor Demon = new CActor();
        int DemonflyCounter = 0;
        bool DemonflyDir = false;
        int DemonBaseY = 0;
        int DemonState = 0;
        bool DemonAttacked = false;

        CActor Demon2 = new CActor();
        int DemonflyCounter2 = 0;
        bool DemonflyDir2 = false;
        int DemonBaseY2 = 0;
        int DemonState2 = 0;
        bool DemonAttacked2 = false;

        CActor Demon3 = new CActor();
        int DemonflyCounter3 = 0;
        bool DemonflyDir3 = false;
        int DemonBaseY3 = 0;
        int DemonState3 = 0;
        bool DemonAttacked3 = false;

        bool createlas = false;
        public Form1()
        {
            WindowState = FormWindowState.Maximized;
            // FormBorderStyle = FormBorderStyle.None;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            MouseDown += Form1_MouseDown;
            MouseMove += Form1_MouseMove;
            MouseUp += Form1_MouseUp;
            Paint += Form1_Paint;
            Load += Form1_Load;
            BackColor = Color.Black;
            tt.Tick += Tt_Tick;
            tt.Interval = 100;
            tt.Start();
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            ManageHeroMove();
            ManageActorAnimation(Hero);
            RunStunTimer(Hero);
            CheckHeroHit(Hero);

            if (Fstate == 1)
            {
                ManageActorAnimation(Skewar);
                Manageenemybehavior(Skewar);
                CheckEnemyHit(Skewar);
                CheckHeroHit(Skewar);

                ManageActorAnimation(Demon);
                ManageDemonBehavior();
                CheckEnemyHit(Demon);
                CheckHeroHit(Demon);

                ManageActorAnimation(tor);
                Manageenemybehavior(tor);
                CheckEnemyHit(tor);
                CheckHeroHit(tor);

                ManageActorAnimation(Skespe);

                ManageActorAnimation(Sat);
                CheckEnemyHit(Sat);
                Manageenemybehavior(Sat);

                CheckEnemyHit(Skespe);
                CheckHeroHit(Skespe);
            }

            if (Fstate == 3)
            {
                Hero.floor = -1;
                ManageActorAnimation(Demon2);
                CheckEnemyHit(Demon2);
                CheckHeroHit(Demon2);
                ManageDemonBehavior2();

                ManageActorAnimation(Demon3);
                CheckEnemyHit(Demon3);
                CheckHeroHit(Demon3);
                ManageDemonBehavior3();

                if (Demon2.isDead && Demon3.isDead)
                {
                    Fstate = 4;
                }
            }

            CheckGravity();

            ManageSkespeBehavior();
            ManageChargeAnimation();

            Jump();

            manageportalanimation();

            DrawDubb(CreateGraphics());
            CheckHeroHealth();
            MoveElevator();
            managelaser();
            if (Fstate == 2)
            {
                MoveRocket();
                MoveChickens();
                MoveEggs();
                if (Game.State == 2)
                {
                    MoveBoss();
                    MoveBossBullets();
                }
                if (ctBoss == 10)
                {
                    CreateBossBullet();
                    ctBoss = 0;
                }
                if (ctEgg == 40)
                {
                    CreateEgg();
                    ctEgg = 0;
                }
                ctEgg++;
                ctBoss++;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            CreateBackground();
            Createfloor();
            createportals();
            Createwall();
            Createhero();
            Createtor();
            Createskearch();
            createskespe();
            createskewar();
            CreateTables();
            createdemon();
            CreateSat();
            createladder();
            CreateElevator();
            CreateHealthBar();
            CreateKey();
            CreateHealers();
            CreateStartMenu();
            createdoor();
            CreateMiniGame();
            CreateAbility();
            //Fstate = 3;
            //Background[0].IF = 1;
            //Hero.Y = ClientSize.Height - Hero.Idler[0].Height - 90;
            //BStartX = 0;
            //BStartY = 0;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Fstate == 1 || Fstate == 3)
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
                    if (e.KeyCode == Keys.E && Fstate == 1 && Game.RocketHealth > 0 && CheckKey())
                    {
                        Fstate = 2;
                    }
                    else if (e.KeyCode == Keys.E && CheckAbility())
                    {
                        Ability[0].cf = 1;
                        Ability[1].cf = 0;
                    }
                    else if (e.KeyCode == Keys.E && CheckHealers())
                    {
                        Hero.Health += 12;
                        if (Hero.Health > 100)
                        {
                            Hero.Health = 100;
                        }
                    }
                    else if (e.KeyCode == Keys.E && WinCondition())
                    {
                        Fstate = 3;
                        Background[0].IF = 1;
                        Hero.Y = ClientSize.Height - Hero.Idler[0].Height - 90;
                        BStartX = 0;
                        BStartY = 0;
                    }
                    else if (e.KeyCode == Keys.E)
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
                        createsinglefireball(Hero);
                    }
                    if (e.KeyCode == Keys.J && !createlas)
                    {
                        createlaser();
                        createlas = true;
                    }
                }
            }

            else if (Fstate == 2)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    Fstate = 1;
                }
                if (e.KeyCode == Keys.D)
                {
                    keyD = true;
                }
                if (e.KeyCode == Keys.A)
                {
                    keyA = true;
                }
            }
        }
        void drawlaser(Graphics g)
        {
            for (int i = 0; i < lasers.Count; i++)
            {
                laser ptrv = lasers[i];
                Brush br = new SolidBrush(Color.Red);
                Brush br2 = new SolidBrush(Color.DarkRed);
                int drawX = ptrv.X - BStartX;
                int drawY = ptrv.Y - BStartY;
                g.FillRectangle(br, drawX, drawY, ptrv.W, 20);
                g.FillRectangle(br2, drawX, drawY, ptrv.W, 7);
                g.FillRectangle(br2, drawX, drawY + 17, ptrv.W, 3);
            }

        }
        void createlaser()
        {
            laser pnn = new laser();
            pnn.Y = pnn.Y = Hero.Y + 40;
            pnn.D = Hero.D;
            if (Hero.D == 1)
            {
                pnn.X = Hero.X + Hero.Idlel[0].Width;
            }
            else
            {
                pnn.X = Hero.X - Hero.Idlel[0].Width;
            }
            lasers.Add(pnn);
        }
        void managelaser()
        {
            for (int i = lasers.Count - 1; i >= 0; i--)
            {
                laser ptrv = lasers[i];
                bool destroyLaser = false;

                if (ptrv.W < ptrv.strmax && ptrv.held)
                {
                    ptrv.W += 60;
                    if (ptrv.D == -1)
                    {
                        ptrv.X -= 60;
                    }
                }

                if (!ptrv.held)
                {
                    if (ptrv.D == 1)
                    {
                        ptrv.X += 60;
                    }
                    else
                    {
                        ptrv.X -= 60;
                    }
                }

                if (ptrv.X > Background[Background[0].IF].Img.Width || ptrv.X + ptrv.W < 0)
                {
                    destroyLaser = true;
                }

                if (!destroyLaser)
                {
                    CActor[] activeEnemies = { Demon, tor, Skespe, Skewar, Sat, Demon2, Demon3 };
                    for (int e = 0; e < activeEnemies.Length; e++)
                    {
                        CActor enemy = activeEnemies[e];

                        if (!enemy.isDead && enemy.floor == Hero.floor)
                        {
                            if (ptrv.X <= enemy.X + enemy.Idler[0].Width && ptrv.X + ptrv.W >= enemy.X &&
                                ptrv.Y <= enemy.Y + enemy.Idler[0].Height && ptrv.Y + 30 >= enemy.Y)
                            {
                                enemy.Health -= 10;
                                enemy.GotHit = true;
                                enemy.IF = 0;
                                CheckDead(enemy);

                                destroyLaser = true;
                                if (ptrv.held)
                                {
                                    createlas = false;
                                }
                                break;
                            }
                        }
                    }
                }

                if (!destroyLaser)
                {
                    for (int w = 0; w < walls.Count; w++)
                    {
                        Tile wall = walls[w];

                        if (ptrv.X <= wall.x + wall.img.Width && ptrv.X + ptrv.W >= wall.x &&
                            ptrv.Y <= wall.y + wall.img.Height && ptrv.Y + 30 >= wall.y)
                        {
                            destroyLaser = true;
                            if (ptrv.held)
                            {
                                createlas = false;
                            }
                            break;
                        }
                    }
                }
                if (destroyLaser)
                {
                    lasers.RemoveAt(i);
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
            if (e.KeyCode == Keys.J && lasers.Count > 0)
            {
                lasers[lasers.Count - 1].held = false;
                createlas = false;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Hero.isDead)
            {
                if (Fstate == 1 || Fstate == 3)
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
                else if (Fstate == 2)
                {
                    CreateRocketBullet();
                }
                if (Start[0].cf == 1 && Fstate == 0)
                {
                    Fstate = 1;
                }
            }
            
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Start != null && e.X > 600 && e.X < 1300 && e.Y > 730 && e.Y < 880)
            {
                Start[0].cf = 1;
            }
            else if (Start != null)
            {
                Start[0].cf = 0;
            }
            Text = e.X.ToString() + "   " + e.Y.ToString();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                LaserOn = false;
            }
        }
        void createsinglefireball(CActor Actor)
        {
            if (!Actor.sinbullet)
            {
                ball pnn = new ball();
                pnn.Y = Actor.Y;
                pnn.W = 1;
                pnn.D = Actor.D;
                pnn.floor = Actor.floor;
                if (Actor.D == 1)
                {
                    pnn.X = Actor.X + Actor.Attack2r[Actor.Attack2r.Count - 1].Width;
                }
                else
                {
                    pnn.X = Actor.X - Actor.Attack2l[Actor.Attack2l.Count - 1].Width;
                }
                for (int i = 1; i <= 12; i++)
                {
                    if (pnn.D == 1)
                    {
                        pnn.Charge.Add(new Bitmap("hero\\heroR\\charge\\herocharge" + i + ".png"));
                    }
                    else
                    {
                        pnn.Charge.Add(new Bitmap("hero\\heroL\\charge\\herocharge" + i + "_result.png"));
                    }
                }
                pnn.type = 2;
                if (Actor == Hero) { pnn.from = 0; }
                if (Actor == Sat) { pnn.from = 1; }
                if (Actor == Skearch) { pnn.from = 2; }
                Fireball.Add(pnn);
                Actor.sinbullet = true;
            }
        }

        void MoveRocket()
        {
            if (Fstate == 2)
            {
                if (keyD)
                {
                    Game.Rocket.x += 60;
                }
                else if (keyA)
                {
                    Game.Rocket.x -= 60;
                }
            }
        }
        void ManageHeroMove()
        {
            if (keyA == true && CheckWalls("a") && Checktable("a"))
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
            else if (keyD == true && CheckWalls("d") && Checktable("d"))
            {
                if (Hero.State != 2)
                {
                    Hero.State = 1;
                }
                if (Hero.X + Hero.Idler[0].Width + 50 < Background[Background[0].IF].Img.Width)
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

            if (Hero.floor != Skespe.floor || IsWallBetween(Skespe))
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
        bool IsWallBetween(CActor enemy)
        {
            int leftX;
            int rightX;

            if (enemy.X < Hero.X)
            {
                leftX = enemy.X;
                rightX = Hero.X;
            }
            else
            {
                leftX = Hero.X;
                rightX = enemy.X;
            }

            for (int i = 0; i < walls.Count; i++)
            {
                Tile wall = walls[i];

                if (wall.x + wall.img.Width > leftX && wall.x < rightX)
                {
                    if (enemy.Y + enemy.Idler[0].Height > wall.y && enemy.Y < wall.y + wall.img.Height)
                    {
                        return true;
                    }
                }
            }
            return false;
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
            if (distance <= enemy.DetectRange && Hero.floor == enemy.floor && !IsWallBetween(enemy))
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
                    if (enemy == Sat)
                    {
                        Sat.sinbullet = false;
                    }
                    if (enemy.State == 3 || enemy.State == 5 || enemy.State == 6)
                    {
                        enemy.State = 0;
                        enemy.IF = 0;
                        enemy.GotHit = false;
                        if (enemy == Demon)
                        {
                            DemonState = 2;
                            Hero.GotHit = false;
                        }
                        if (enemy == Demon2) { DemonState2 = 2; Hero.GotHit = false; }
                        if (enemy == Demon3) { DemonState3 = 2; Hero.GotHit = false; }
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
                if (enemy == Sat)
                {
                    enemy.State = 0;
                    if (heroWorldX > enemy.X)
                    {
                        enemy.D = 1;
                    }
                    else
                    {
                        enemy.D = -1;
                    }
                    if (!Sat.sinbullet)
                    {
                        createsinglefireball(Sat);
                        Sat.IsStunned = true;
                        Sat.StunTimer = Sat.RecoveryDuration;
                    }
                }
                else
                {
                    TriggerAttack(enemy);
                }
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
                DemonState = 1;
                DemonAttacked = true;
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
                    if (Actor == Demon)
                    {
                        Demon.X = 1000000;
                    }
                }
            }
            else if (Actor == Skespe && Actor.State == 9)
            {
                Actor.IF++;
                if (Actor.IF >= Actor.Climbr.Count)
                {
                    Actor.IF = 0;
                }
            }
            else if (Actor.State == 9)
            {
                if (Actor.IF >= Actor.Climbr.Count)
                {
                    Actor.IF = 0;
                }
            }
            if (!Hero.isDead)
            {
                if (Actor.State == 5)
                {
                    Actor.IF++;
                    if (Actor.IF >= Actor.Attack1r.Count)
                    {
                        Actor.IF = 0;
                        Actor.State = 0;
                        Actor.GotHit = false;
                        if (Actor == Demon)
                        {
                            DemonState = 2;
                        }
                        if (Actor == Demon2) { DemonState2 = 2; }
                        if (Actor == Demon3) { DemonState3 = 2; }
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
                                if (pnn.D == 1)
                                {
                                    pnn.Charge.Add(new Bitmap("hero\\heroR\\charge\\herocharge" + i + ".png"));
                                }
                                else
                                {
                                    pnn.Charge.Add(new Bitmap("hero\\heroL\\charge\\herocharge" + i + "_result.png"));
                                }

                            }
                            pnn.type = 1;
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
                            ptrv.X += 50;
                        }
                        else
                        {
                            ptrv.X -= 50;
                        }
                        ptrv.IF++;
                        if (ptrv.IF >= 5)
                        {
                            ptrv.IF = 0;
                        }
                        bool hit = false;

                        if (!tor.isDead && ptrv.X + ptrv.Charge[ptrv.IF].Width >= tor.X && ptrv.X < tor.X + 50 && ptrv.floor == tor.floor)
                        {
                            tor.Health -= 5;
                            tor.GotHit = true;
                            CheckDead(tor);
                            hit = true;
                        }
                        if (!Skewar.isDead && ptrv.X + ptrv.Charge[ptrv.IF].Width >= Skewar.X && ptrv.X < Skewar.X + 50 && ptrv.floor == Skewar.floor)
                        {
                            Skewar.Health -= 5;
                            Skewar.GotHit = true;
                            CheckDead(Skewar);
                            hit = true;
                        }
                        if (!Demon.isDead && DemonState == 1 && ptrv.X + ptrv.Charge[ptrv.IF].Width >= Demon.X && ptrv.X < Demon.X + Demon.Idler[0].Width && ptrv.Y + ptrv.Charge[ptrv.IF].Height >= Demon.Y && ptrv.Y < Demon.Y + Demon.Idler[0].Height)
                        {
                            Demon.Health -= 5;
                            Demon.GotHit = true;
                            CheckDead(Demon);
                            hit = true;
                        }
                        if (!Demon2.isDead && DemonState2 == 1 && ptrv.X + ptrv.Charge[ptrv.IF].Width >= Demon2.X && ptrv.X < Demon2.X + Demon2.Idler[0].Width && ptrv.Y + ptrv.Charge[ptrv.IF].Height >= Demon2.Y && ptrv.Y < Demon2.Y + Demon2.Idler[0].Height)
                        {
                            Demon.Health -= 5;
                            Demon.GotHit = true;
                            CheckDead(Demon);
                            hit = true;
                        }
                        if (!Demon3.isDead && DemonState3 == 1 && ptrv.X + ptrv.Charge[ptrv.IF].Width >= Demon3.X && ptrv.X < Demon3.X + Demon3.Idler[0].Width && ptrv.Y + ptrv.Charge[ptrv.IF].Height >= Demon3.Y && ptrv.Y < Demon3.Y + Demon3.Idler[0].Height)
                        {
                            Demon.Health -= 5;
                            Demon.GotHit = true;
                            CheckDead(Demon);
                            hit = true;
                        }
                        if (!Skespe.isDead && ptrv.X + ptrv.Charge[ptrv.IF].Width >= Skespe.X && ptrv.X < Skespe.X + 50 && ptrv.floor == Skespe.floor)
                        {
                            Skespe.Health -= 5;
                            Skespe.GotHit = true;
                            CheckDead(Skespe);
                            hit = true;
                        }
                        if (!Hero.isDead && ptrv.from != 0 && ptrv.X + ptrv.Charge[ptrv.IF].Width >= Hero.X && ptrv.X < Hero.X + Hero.Idler[0].Width && ptrv.floor == Hero.floor)
                        {
                            Hero.Health -= 10;
                            Hero.GotHit = true;
                            CheckDead(Hero);
                            hit = true;
                        }
                        if (ptrv.X > Background[Background[0].IF].Img.Width)
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
                            if (ptrv.type == 2)
                            {
                                if (ptrv.from == 0) { Hero.sinbullet = false; }
                                if (ptrv.from == 1) { Sat.sinbullet = false; }
                                if (ptrv.from == 2) { Skearch.sinbullet = false; }
                            }
                            Fireball.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }
        void ManageDemonBehavior()
        {
            if (Demon.isDead) return;
            if (Demon.IsStunned)
            {
                RunStunTimer(Demon);
                return;
            }

            int distToHeroX;
            if (Hero.X >= Demon.X)
            {
                distToHeroX = Hero.X - Demon.X;
            }
            else
            {
                distToHeroX = Demon.X - Hero.X;
            }

            bool heroInSight = distToHeroX <= Demon.DetectRange && Hero.floor == Demon.floor;

            if (DemonState == 2)
            {
                Demon.State = 1;

                if (Demon.Y > DemonBaseY)
                {
                    Demon.Y -= 15;
                }
                else
                {
                    Demon.Y = DemonBaseY;
                    DemonState = 0;
                    DemonAttacked = false;
                }
                return;
            }

            if (DemonState == 1)
            {
                Demon.State = 1;

                if (distToHeroX > Demon.Speed)
                {
                    if (Demon.X < Hero.X) 
                    {
                        Demon.X += Demon.Speed;
                        Demon.D = 1; 
                    }
                    else 
                    { 
                        Demon.X -= Demon.Speed; 
                        Demon.D = -1; 
                    }
                }

                int targetY = Hero.Y + (Hero.Idler[0].Height / 2) - (Demon.Idler[0].Height / 2);

                if (Demon.Y < targetY - 15) { Demon.Y += 15; }
                else if (Demon.Y > targetY + 15) { Demon.Y -= 15; }
                else Demon.Y = targetY;

                int diffY;
                if (Demon.Y >= targetY)
                {
                    diffY = Demon.Y - targetY;
                }
                else
                {
                    diffY = targetY - Demon.Y;
                }

                if (distToHeroX <= Demon.AttackRange && diffY <= 20 && !Hero.isDead)
                {
                    Demon.State = 5;
                    Demon.IF = 0;
                    Demon.IsStunned = true;
                    Demon.StunTimer = Demon.RecoveryDuration;
                }
                else if (!heroInSight)
                {
                    DemonState = 2;
                }

                return;
            }

            if (DemonState == 0)
            {
                int diffBaseY;
                if (Demon.Y >= DemonBaseY)
                {
                    diffBaseY = Demon.Y - DemonBaseY;
                }
                else
                {
                    diffBaseY = DemonBaseY - Demon.Y;
                }

                if (heroInSight && diffBaseY <= 20)
                {
                    DemonState = 1;
                    return;
                }

                if (!DemonflyDir) { Demon.Y++; DemonflyCounter++; if (DemonflyCounter >= 15) DemonflyDir = true; }
                else { Demon.Y--; DemonflyCounter--; if (DemonflyCounter <= 0) DemonflyDir = false; }

                if (Demon.State == 0)
                {
                    Demon.IdleTimer--;
                    if (Demon.IdleTimer <= 0)
                    {
                        Demon.State = 1;
                        Demon.D *= -1;
                    }
                    return;
                }

                Demon.State = 1;
                Demon.X += Demon.Speed * Demon.D;

                if (Demon.D == 1 && Demon.X >= Demon.endpoint)
                {
                    Demon.X = Demon.endpoint;
                    Demon.State = 0;
                    Demon.IdleTimer = 15;
                }
                else if (Demon.D == -1 && Demon.X <= Demon.Startpoint)
                {
                    Demon.X = Demon.Startpoint;
                    Demon.State = 0;
                    Demon.IdleTimer = 15;
                }
            }
        }

        void ManageDemonBehavior2()
        {
            if (Demon2.isDead) return;
            if (Demon2.IsStunned)
            {
                RunStunTimer(Demon2);
                return;
            }

            int distToHeroX;
            if (Hero.X >= Demon2.X)
            {
                distToHeroX = Hero.X - Demon2.X;
            }
            else
            {
                distToHeroX = Demon2.X - Hero.X;
            }

            bool heroInSight = distToHeroX <= Demon2.DetectRange && Hero.floor == Demon2.floor;

            if (DemonState2 == 2)
            {
                Demon2.State = 1;

                if (Demon2.Y > DemonBaseY2)
                {
                    Demon2.Y -= 15;
                }
                else
                {
                    Demon2.Y = DemonBaseY2;
                    DemonState2 = 0;
                    DemonAttacked2 = false;
                }
                return;
            }

            if (DemonState2 == 1)
            {
                Demon2.State = 1;

                if (distToHeroX > Demon2.Speed)
                {
                    if (Demon2.X < Hero.X) { Demon2.X += Demon2.Speed; Demon2.D = 1; }
                    else { Demon2.X -= Demon2.Speed; Demon2.D = -1; }
                }

                int targetY = Hero.Y + (Hero.Idler[0].Height / 2) - (Demon2.Idler[0].Height / 2);

                if (Demon2.Y < targetY - 15) Demon2.Y += 15;
                else if (Demon2.Y > targetY + 15) Demon2.Y -= 15;
                else Demon2.Y = targetY;

                int diffY;
                if (Demon2.Y >= targetY)
                {
                    diffY = Demon2.Y - targetY;
                }
                else
                {
                    diffY = targetY - Demon2.Y;
                }

                if (distToHeroX <= Demon2.AttackRange && diffY <= 20)
                {
                    Demon2.State = 5;
                    Demon2.IF = 0;
                    Demon2.IsStunned = true;
                    Demon2.StunTimer = Demon.RecoveryDuration;
                }
                else if (!heroInSight)
                {
                    DemonState2 = 2;
                }

                return;
            }

            if (DemonState2 == 0)
            {
                int diffBaseY;
                if (Demon2.Y >= DemonBaseY2)
                {
                    diffBaseY = Demon2.Y - DemonBaseY2;
                }
                else
                {
                    diffBaseY = DemonBaseY2 - Demon2.Y;
                }

                if (heroInSight && diffBaseY <= 20)
                {
                    DemonState2 = 1;
                    return;
                }

                if (!DemonflyDir2) { Demon2.Y++; DemonflyCounter2++; if (DemonflyCounter2 >= 15) DemonflyDir2 = true; }
                else { Demon2.Y--; DemonflyCounter2--; if (DemonflyCounter2 <= 0) DemonflyDir2 = false; }

                if (Demon2.State == 0)
                {
                    Demon2.IdleTimer--;
                    if (Demon2.IdleTimer <= 0)
                    {
                        Demon2.State = 1;
                        Demon2.D *= -1;
                    }
                    return;
                }

                Demon2.State = 1;
                Demon2.X += Demon2.Speed * Demon2.D;

                if (Demon2.D == 1 && Demon2.X >= Demon2.endpoint)
                {
                    Demon2.X = Demon2.endpoint;
                    Demon2.State = 0;
                    Demon2.IdleTimer = 15;
                }
                else if (Demon2.D == -1 && Demon2.X <= Demon2.Startpoint)
                {
                    Demon2.X = Demon2.Startpoint;
                    Demon2.State = 0;
                    Demon2.IdleTimer = 15;
                }
            }
        }

        void ManageDemonBehavior3()
        {
            if (Demon3.isDead) return;
            if (Demon3.IsStunned)
            {
                RunStunTimer(Demon3);
                return;
            }

            int distToHeroX;
            if (Hero.X >= Demon3.X)
                distToHeroX = Hero.X - Demon3.X;
            else
                distToHeroX = Demon3.X - Hero.X;

            bool heroInSight = distToHeroX <= Demon3.DetectRange && Hero.floor == Demon3.floor;

            if (DemonState3 == 2)
            {
                Demon3.State = 1;
                if (Demon3.Y > DemonBaseY3)
                    Demon3.Y -= 15;
                else
                {
                    Demon3.Y = DemonBaseY3;
                    DemonState3 = 0;
                    DemonAttacked3 = false;
                }
                return;
            }

            if (DemonState3 == 1)
            {
                Demon3.State = 1;
                if (distToHeroX > Demon3.Speed)
                {
                    if (Demon3.X < Hero.X) { Demon3.X += Demon3.Speed; Demon3.D = 1; }
                    else { Demon3.X -= Demon3.Speed; Demon3.D = -1; }
                }

                int targetY = Hero.Y + (Hero.Idler[0].Height / 2) - (Demon3.Idler[0].Height / 2);
                if (Demon3.Y < targetY - 15) Demon3.Y += 15;
                else if (Demon3.Y > targetY + 15) Demon3.Y -= 15;
                else Demon3.Y = targetY;

                int diffY;
                if (Demon3.Y >= targetY) diffY = Demon3.Y - targetY;
                else diffY = targetY - Demon3.Y;

                if (distToHeroX <= Demon3.AttackRange && diffY <= 20)
                {
                    Demon3.State = 5;
                    Demon3.IF = 0;
                    Demon3.IsStunned = true;
                    Demon3.StunTimer = Demon3.RecoveryDuration;
                    DemonAttacked3 = true;
                }
                else if (!heroInSight)
                {
                    DemonState3 = 2;
                }
                return;
            }

            if (DemonState3 == 0)
            {
                int diffBaseY;
                if (Demon3.Y >= DemonBaseY3) diffBaseY = Demon3.Y - DemonBaseY3;
                else diffBaseY = DemonBaseY3 - Demon3.Y;

                if (heroInSight && diffBaseY <= 20)
                {
                    DemonState3 = 1;
                    return;
                }

                if (!DemonflyDir3) { Demon3.Y++; DemonflyCounter3++; if (DemonflyCounter3 >= 15) DemonflyDir3 = true; }
                else { Demon3.Y--; DemonflyCounter3--; if (DemonflyCounter3 <= 0) DemonflyDir3 = false; }

                if (Demon3.State == 0)
                {
                    Demon3.IdleTimer--;
                    if (Demon3.IdleTimer <= 0)
                    {
                        Demon3.State = 1;
                        Demon3.D *= -1;
                    }
                    return;
                }

                Demon3.State = 1;
                Demon3.X += Demon3.Speed * Demon3.D;

                if (Demon3.D == 1 && Demon3.X >= Demon3.endpoint)
                {
                    Demon3.X = Demon3.endpoint;
                    Demon3.State = 0;
                    Demon3.IdleTimer = 15;
                }
                else if (Demon3.D == -1 && Demon3.X <= Demon3.Startpoint)
                {
                    Demon3.X = Demon3.Startpoint;
                    Demon3.State = 0;
                    Demon3.IdleTimer = 15;
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
            if (Hero.State == 5 && isAttack && !enemy.GotHit && Hero.floor == enemy.floor)
            {
                if (Hero.D == 1 && enemy.X > Hero.X && Hero.X + Hero.Attack1r[Hero.IF].Width >= enemy.X - 30)
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
            else if (Hero.State == 7 && isAttack && Hero.floor == enemy.floor)
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
                    if (ptrv.W == 1)
                    {
                        if (ptrv.X + ptrv.Charge[ptrv.IF].Width >= enemy.X && ptrv.X < enemy.X + enemy.Idler[0].Width && ptrv.Y + ptrv.Charge[ptrv.IF].Height >= enemy.Y && ptrv.Y < enemy.Y + enemy.Idler[0].Height)
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
                    if (Hero.Y < Ladder[i].Y)
                    {
                        Hero.Y = floor[Ladder[i].floor].y - floor[Ladder[i].floor].h - Hero.Idler[0].Height;
                        Hero.State = 0;
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
                        Hero.Y = floor[Ladder[i].floor + 1].y - floor[Ladder[i].floor + 1].h - Hero.Idler[0].Height;
                        Hero.State = 0;
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        bool CheckKey()
        {
            if (Hero.Y < Key[1].y && Hero.Y + Hero.Idler[0].Height > Key[1].y && Hero.X < Key[1].x && Hero.X > Key[1].x - 200)
            {
                return true;
            }
            return false;
        }

        bool CheckHealers()
        {
            for (int i = 0; i < Healers.Count; i++)
            {
                if (Hero.Y < Healers[i].y + Healers[i].img.Height && Hero.Y + Hero.Idler[0].Height > Healers[i].y && (Hero.X > Healers[i].x && Hero.X < Healers[i].x + 200 || Hero.X < Healers[i].x && Hero.X > Healers[i].x - 200))
                {
                    Healers.Remove(Healers[i]);
                    return true;
                }
            }
            return false;
        }
        bool WinCondition()
        {
            if (Hero.X > Door.x && Hero.X + Hero.Idler[0].Width < Door.x + Door.img.Width && Hero.Y + Hero.Idler[0].Height > Door.y && Hero.Y - 100 < Door.y + Door.img.Height /*&& Key[0].cf == 1*/)
            {
                return true;
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
            if (Fstate == 1)
            {
                BStartX = Hero.X - ClientSize.Width / 2;
                BStartY = Hero.Y - ClientSize.Height / 2;
            }
            g2.Clear(BackColor);
            CheckBorders();
            DrawBackground(g2);


            if (Fstate == 1)
            {
                DrawHealers(g2);
                drawportals(g2);
                Drawladder(g2);
                drawdoor(g2);
                drawtor(g2);
                DrawDemon(g2);
                drawSat(g2);
                drawSkewar(g2);
                drawSkespe(g2);
                Drawfloor(g2);
                Drawwalls(g2);
                drawtables(g2);
                DrawElevator(g2);
            }
            else if (Fstate == 2)
            {
                DrawMiniGame(g2);
                DrawChickens(g2);
                DrawEggs(g2);
                if (Game.State == 2)
                {
                    DrawBoss(g2);
                }
            }
            else if (Fstate == 3)
            {
                DrawDemonAny(g2, Demon2);
                DrawDemonAny(g2, Demon3);
               
            }
            else if (Fstate == 4)
            {
                g2.Clear(Color.Black);
                Font winFont = new Font("Impact", 72, FontStyle.Bold);
                SolidBrush textBrush = new SolidBrush(Color.Gold);
                string winText = "YOU WIN !";
                Size textSize = TextRenderer.MeasureText(winText, winFont);
                int centerX = (ClientSize.Width - textSize.Width) / 2;
                int centerY = (ClientSize.Height - textSize.Height) / 2;
                g2.DrawString(winText, winFont, textBrush, centerX, centerY);
            }
            if (Fstate != 2 && Fstate != 4)
            {
                drawfireball(g2);
                drawhero(g2);
                DrawHealthBar(g2);
                DrawStartMenu(g2);
                DrawKey(g2);
                DrawAbility(g2);
                drawlaser(g2);
            }

            if (Hero.isDead)
            {
                g2.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), 0, 0, ClientSize.Width, ClientSize.Height);
                Font deadFont = new Font("Impact", 72, FontStyle.Bold);
                Font subFont = new Font("Impact", 32, FontStyle.Bold);
                SolidBrush redBrush = new SolidBrush(Color.DarkRed);
                SolidBrush whiteBrush = new SolidBrush(Color.White);
                string deadText = "YOU DIED";
                Size deadSize = TextRenderer.MeasureText(deadText, deadFont);
                int cx = (ClientSize.Width - deadSize.Width) / 2;
                int cy = (ClientSize.Height - deadSize.Height) / 2 - 50;
                g2.DrawString(deadText, deadFont, redBrush, cx, cy);
            }
        }
        void DrawBackground(Graphics g)
        {
            Rectangle rcDst = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            Rectangle rcSrc = new Rectangle(BStartX, BStartY, ClientSize.Width, ClientSize.Height);

            if (Fstate == 3)
            {
                rcSrc = new Rectangle(0, 0, Background[1].Img.Width, Background[1].Img.Height);
            }
            g.DrawImage(Background[Background[0].IF].Img, rcDst, rcSrc, GraphicsUnit.Pixel);
        }

        void Drawfloor(Graphics g)
        {
            Pen pn = new Pen(Color.Yellow, 2);
            for (int i = 0; i < 5; i++)
            {
                Tile ptrv = floor[i];
                //g.DrawRectangle(pn, ptrv.x - BStartX, ptrv.y - BStartY, ptrv.w, ptrv.h);
            }
        }
        void Drawwalls(Graphics g)
        {
            Pen pn = new Pen(Color.Yellow, 2);
            for (int i = 0; i < 5; i++)
            {
                Tile ptrv = walls[i];
                // g.DrawRectangle(pn, ptrv.x - BStartX, ptrv.y - BStartY, ptrv.img.Width, ptrv.img.Height);
                g.DrawImage(ptrv.img, ptrv.x - BStartX, ptrv.y - BStartY);
            }
        }
        void drawtables(Graphics g)
        {
            Pen pn = new Pen(Color.Yellow, 2);
            for (int i = 0; i < tables.Count; i++)
            {
                Tile ptrv = tables[i];
                // g.DrawRectangle(pn, ptrv.x - BStartX, ptrv.y - BStartY, ptrv.img.Width, ptrv.img.Height);
                g.DrawImage(ptrv.img, ptrv.x - BStartX, ptrv.y - BStartY);
            }
        }
        void DrawDemonAny(Graphics g, CActor d)
        {
            int frame = d.IF;
            int drawX = d.X;
            int drawY = d.Y;

            if (d.D == 1)
            {
                if (d.State == 0 && frame < d.Idler.Count) g.DrawImage(d.Idler[frame], drawX, drawY);
                else if (d.State == 1 && frame < d.Walkr.Count) g.DrawImage(d.Walkr[frame], drawX, drawY);
                else if (d.State == 3 && frame < d.Hurtr.Count) g.DrawImage(d.Hurtr[frame], drawX, drawY);
                else if (d.State == 4 && frame < d.Deadr.Count) g.DrawImage(d.Deadr[frame], drawX, drawY);
                else if (d.State == 5 && frame < d.Attack1r.Count) g.DrawImage(d.Attack1r[frame], drawX, drawY);
            }
            else
            {
                if (d.State == 0 && frame < d.Idlel.Count) g.DrawImage(d.Idlel[frame], drawX - d.Idlel[frame].Width, drawY);
                else if (d.State == 1 && frame < d.Walkl.Count) g.DrawImage(d.Walkl[frame], drawX - d.Walkl[frame].Width, drawY);
                else if (d.State == 3 && frame < d.Hurtl.Count) g.DrawImage(d.Hurtl[frame], drawX - d.Hurtl[frame].Width, drawY);
                else if (d.State == 4 && frame < d.Deadl.Count) g.DrawImage(d.Deadl[frame], drawX - d.Deadl[frame].Width, drawY);
                else if (d.State == 5 && frame < d.Attack1l.Count) g.DrawImage(d.Attack1l[frame], drawX - d.Attack1l[frame].Width, drawY);
            }
        }
        void Drawladder(Graphics g)
        {
            for (int i = 0; i < Ladder.Count; i++)
            {
                g.DrawImage(Ladder[i].Img, Ladder[i].X - BStartX, Ladder[i].Y - BStartY);
            }
        }
        void drawtor(Graphics g)
        {
            int frameToDraw = tor.IF;
            int drawX = tor.X - BStartX;
            int drawY = tor.Y - BStartY;

            if (tor.D == 1)
            {
                int torDrawX = drawX;
                if (frameToDraw < tor.xadd.Length)
                    torDrawX += tor.xadd[frameToDraw];

                if (tor.State == 0 && frameToDraw < tor.Idler.Count)
                    g.DrawImage(tor.Idler[frameToDraw], torDrawX, drawY);
                else if (tor.State == 1 && frameToDraw < tor.Walkr.Count)
                    g.DrawImage(tor.Walkr[frameToDraw], drawX, drawY);
                else if (tor.State == 3 && frameToDraw < tor.Hurtr.Count)
                    g.DrawImage(tor.Hurtr[frameToDraw], drawX, drawY);
                else if (tor.State == 4 && frameToDraw < tor.Deadr.Count)
                    g.DrawImage(tor.Deadr[frameToDraw], drawX, drawY);
                else if (tor.State == 5 && frameToDraw < tor.Attack1r.Count)
                    g.DrawImage(tor.Attack1r[frameToDraw], drawX, drawY);
                else
                    g.DrawImage(tor.Idler[0], drawX, drawY);
            }
            else
            {
                int torDrawX = drawX;
                if (frameToDraw < tor.xadd.Length)
                    torDrawX -= tor.xadd[frameToDraw];

                if (tor.State == 0 && frameToDraw < tor.Idlel.Count)
                    g.DrawImage(tor.Idlel[frameToDraw], torDrawX - tor.Idlel[frameToDraw].Width, drawY);
                else if (tor.State == 1 && frameToDraw < tor.Walkl.Count)
                    g.DrawImage(tor.Walkl[frameToDraw], drawX - tor.Walkl[frameToDraw].Width, drawY);
                else if (tor.State == 3 && frameToDraw < tor.Hurtl.Count)
                    g.DrawImage(tor.Hurtl[frameToDraw], drawX - tor.Hurtl[frameToDraw].Width, drawY);
                else if (tor.State == 4 && frameToDraw < tor.Deadl.Count)
                    g.DrawImage(tor.Deadl[frameToDraw], drawX - tor.Deadl[frameToDraw].Width, drawY);
                else if (tor.State == 5 && frameToDraw < tor.Attack1l.Count)
                    g.DrawImage(tor.Attack1l[frameToDraw], drawX - tor.Attack1l[frameToDraw].Width, drawY);
                else
                    g.DrawImage(tor.Idlel[0], drawX - tor.Idlel[0].Width, drawY);
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
        void drawSat(Graphics g)
        {
            int frameToDraw = Sat.IF;
            if (Sat.D == 1)
            {
                if (Sat.State == 0 && Sat.Idler.Count > frameToDraw)
                {
                    g.DrawImage(Sat.Idler[frameToDraw], Sat.X - BStartX, Sat.Y - BStartY);
                }
                else if (Sat.State == 1)
                {
                    g.DrawImage(Sat.Walkr[frameToDraw], Sat.X - BStartX, Sat.Y - BStartY);
                }
                else if (Sat.State == 3)
                {
                    g.DrawImage(Sat.Hurtr[frameToDraw], Sat.X - BStartX, Sat.Y - BStartY);
                }
                else if (Sat.State == 4)
                {
                    g.DrawImage(Sat.Deadr[frameToDraw], Sat.X - BStartX, Sat.Y - BStartY - 50);
                }
                else if (Sat.State == 6)
                {
                    g.DrawImage(Sat.Attack2r[frameToDraw], Sat.X - BStartX, Sat.Y - BStartY);
                }
            }
            else
            {
                if (Sat.State == 0 && Sat.Idler.Count > frameToDraw)
                {
                    g.DrawImage(Sat.Idlel[frameToDraw], Sat.X - BStartX - Sat.Idlel[frameToDraw].Width, Sat.Y - BStartY);
                }
                else if (Sat.State == 1)
                {
                    g.DrawImage(Sat.Walkl[frameToDraw], Sat.X - BStartX - Sat.Walkl[frameToDraw].Width, Sat.Y - BStartY);
                }
                else if (Sat.State == 3)
                {
                    g.DrawImage(Sat.Hurtl[frameToDraw], Sat.X - BStartX - Sat.Hurtl[frameToDraw].Width, Sat.Y - BStartY);
                }
                else if (Sat.State == 4)
                {
                    g.DrawImage(Sat.Deadl[frameToDraw], Sat.X - BStartX - Sat.Deadl[frameToDraw].Width, Sat.Y - BStartY - 50);
                }
                else if (Sat.State == 6)
                {
                    g.DrawImage(Sat.Attack2l[frameToDraw], Sat.X - BStartX - Sat.Attack2l[frameToDraw].Width, Sat.Y - BStartY);
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
        void DrawKey(Graphics g)
        {
            for (int i = 0; i < Key.Count; i++)
            {
                if (Key[i].cf == 1)
                {
                    if (i == 0)
                    {
                        g.DrawImage(Key[i].img, Key[i].x, Key[i].y);
                    }
                    else
                    {
                        g.DrawImage(Key[i].img, Key[i].x - BStartX, Key[i].y - BStartY);
                    }
                }
            }
        }
        void DrawHealers(Graphics g)
        {
            for (int i = 0; i < Healers.Count; i++)
            {
                g.DrawImage(Healers[i].img, Healers[i].x - BStartX, Healers[i].y - BStartY);
            }
        }

        void DrawStartMenu(Graphics g)
        {
            if (Fstate == 0)
            {
                Rectangle rcDst = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
                Rectangle rcSrc = new Rectangle(0, 0, Start[Start[0].cf].img.Width, Start[Start[0].cf].img.Height);
                g.DrawImage(Start[Start[0].cf].img, rcDst, rcSrc, GraphicsUnit.Pixel);
            }
        }
        void DrawAbility(Graphics g)
        {
            for (int i = 0; i < Ability.Count; i++)
            {
                if (Ability[i].cf == 1)
                {
                    if (i == 0)
                    {
                        g.DrawImage(Ability[i].img, Ability[i].x, Ability[i].y);
                    }
                    else
                    {
                        g.DrawImage(Ability[i].img, Ability[i].x - BStartX, Ability[i].y - BStartY);
                    }
                }
            }
        }
        bool CheckAbility()
        {
            if (Hero.Y - 20 < Ability[1].y && Hero.Y + Hero.Idler[0].Height > Ability[1].y && Hero.X > Ability[1].x && Hero.X < Ability[1].x + 200)
            {
                return true;
            }
            return false;
        }

        void CreateAbility()
        {
            Tile pnn = new Tile();
            pnn.img = new Bitmap("mbullet2.png");
            pnn.x = ClientSize.Width - pnn.img.Width - 100;
            pnn.y = 20;
            Ability.Add(pnn);

            pnn = new Tile();
            pnn.img = new Bitmap("mbullet.png");
            pnn.x = tables[2].x + tables[2].img.Width - pnn.img.Width - 30;
            pnn.y = tables[2].y - 10;
            pnn.cf = 1;
            Ability.Add(pnn);
        }

        void DrawMiniGame(Graphics g)
        {
            if (Fstate == 2)
            {
                Rectangle rsrc = new Rectangle(0, 0, Game.Background.Width, Game.Background.Height);
                Rectangle rdist = new Rectangle(0, Game.yStart, ClientSize.Width, ClientSize.Height);
                Rectangle rdist2 = new Rectangle(0, Game.yStart2, ClientSize.Width, ClientSize.Height);

                g.DrawImage(Game.Background, rdist, rsrc, GraphicsUnit.Pixel);
                g.DrawImage(Game.Background, rdist2, rsrc, GraphicsUnit.Pixel);
                Game.yStart += 50;
                Game.yStart2 += 50;

                if (Game.yStart >= ClientSize.Height)
                {
                    Game.yStart = Game.yStart2 - ClientSize.Height;
                }
                if (Game.yStart2 >= ClientSize.Height)
                {
                    Game.yStart2 = Game.yStart - ClientSize.Height;
                }
                DrawRocketBullet(g);
                if (Game.State == 2)
                {
                    DrawBossBullets(g);
                }
                g.DrawImage(Game.Rocket.img, Game.Rocket.x, Game.Rocket.y);
                SolidBrush b = new SolidBrush(Color.White);
                Font f = new Font("Comic Sans MS", 18, FontStyle.Bold);
                g.DrawString(Game.RocketHealth.ToString(), f, b, 60, 20);
                g.DrawImage(Game.Heart, 20, 20);

            }
        }

        void DrawRocketBullet(Graphics g)
        {
            for (int i = 0; i < Game.RocketBullets.Count; i++)
            {
                g.DrawImage(Game.RocketBullets[i].imgs[Game.Rocket.cf], Game.RocketBullets[i].x, Game.RocketBullets[i].y);
                Game.RocketBullets[i].y -= 30;
            }
        }

        void DrawChickens(Graphics g)
        {
            for (int i = 0; i < Game.Chickens.Count; i++)
            {
                g.DrawImage(Game.Chickens[i].imgs[Game.Chickens[i].cf], Game.Chickens[i].x, Game.Chickens[i].y);
                Game.Chickens[i].cf = (Game.Chickens[i].cf + 1) % 2;
                if (Game.Chickens[i].cf == 0)
                {
                    Game.Chickens[i].y += 4;
                }
                else
                {
                    Game.Chickens[i].y -= 4;
                }
            }
        }

        void DrawEggs(Graphics g)
        {
            for (int i = 0; i < Game.Eggs.Count; i++)
            {
                Tile pt = Game.Eggs[i];
                g.DrawImage(pt.img, pt.x, pt.y);
            }
        }
        void MoveEggs()
        {
            for (int i = 0; i < Game.Eggs.Count; i++)
            {
                Tile pt = Game.Eggs[i];
                pt.y += 30;
            }
            isRocketHit(Game.Eggs);
        }
        void isRocketHit(List<Tile> pb)
        {
            for (int j = 0; j < pb.Count; j++)
            {
                Tile pt = pb[j];
                if (pt.y + Game.Rocket.img.Height > Game.Rocket.y && pt.y < Game.Rocket.y && pt.x > Game.Rocket.x && pt.x < Game.Rocket.x + Game.Rocket.img.Width)
                {
                    pb.Remove(pt);
                    Game.RocketHealth--;
                }
            }
            if (Game.RocketHealth <= 0)
            {
                Fstate = 1;
            }
        }

        void isBossHit()
        {
            Tile pBoss = Game.Boss;
            for (int j = 0; j < Game.RocketBullets.Count; j++)
            {
                Tile pt2 = Game.RocketBullets[j];
                if (pt2.y < pBoss.y + pBoss.img.Height && pt2.y + pt2.imgs[0].Height > pBoss.y + pBoss.img.Height && pt2.x > pBoss.x && pt2.x < pBoss.x + pBoss.img.Width)
                {
                    Game.BossHealth--;
                    Game.RocketBullets.Remove(pt2);
                }
            }
        }

        void CreateEgg()
        {
            Random r = new Random();
            for (int i = 0; i < Game.Chickens.Count; i++)
            {
                Tile pt = Game.Chickens[i];
                if (r.Next(0, 4) == 1)
                {
                    Tile pnn = new Tile();
                    pnn.img = new Bitmap("minigame\\egg.png");
                    pnn.x = pt.x + pt.imgs[0].Width / 2 - pnn.img.Width / 2;
                    pnn.y = pt.y + pt.imgs[0].Height;
                    Game.Eggs.Add(pnn);
                }
            }
        }
        void MoveChickens()
        {
            for (int i = 0; i < Game.Chickens.Count; i++)
            {
                Tile pt = Game.Chickens[i];
                if (pt.w == 0)
                {
                    if (pt.x < ClientSize.Width * 7 / 8 && i == 0)
                    {
                        pt.x += 80;
                    }
                    else if (i != 0)
                    {
                        if (pt.x < Game.Chickens[i - 1].x - 200)
                        {
                            pt.x += 80;
                        }
                        else if (pt.h != Game.Chickens[i - 1].h && pt.x < ClientSize.Width * 7 / 8)
                        {
                            pt.x += 80;
                        }
                        else
                        {
                            pt.w = 1;
                        }
                    }
                }
            }
            isChickenHit();
        }

        void isChickenHit()
        {
            for (int i = 0; i < Game.Chickens.Count; i++)
            {
                Tile pt = Game.Chickens[i];
                for (int j = 0; j < Game.RocketBullets.Count; j++)
                {
                    Tile pt2 = Game.RocketBullets[j];
                    if (pt2.y < pt.y + pt.imgs[0].Height && pt2.y + pt2.imgs[0].Height > pt.y + pt.imgs[0].Height && pt2.x > pt.x && pt2.x < pt.x + pt.imgs[0].Width)
                    {
                        Game.Chickens.Remove(pt);
                        Game.RocketBullets.Remove(pt2);
                        if (Game.Chickens.Count == 0)
                        {
                            Game.State = 2;
                        }
                    }
                }
            }
        }
        void MoveBossBullets()
        {
            for (int i = 0; i < Game.BossBullets.Count; i++)
            {
                Tile pt = Game.BossBullets[i];
                pt.y += 80;
            }
            isRocketHit(Game.BossBullets);
        }

        void DrawBossBullets(Graphics g)
        {
            for (int i = 0; i < Game.BossBullets.Count; i++)
            {
                Tile pt = Game.BossBullets[i];
                g.DrawImage(pt.img, pt.x, pt.y);
            }
        }

        void CreateBossBullet()
        {
            Random r = new Random();
            if (r.Next(0, 2) == 1)
            {
                Tile pnn = new Tile();
                pnn.img = new Bitmap("minigame\\bullet2.png");
                pnn.x = Game.Boss.x + Game.Boss.img.Width / 2 - pnn.img.Width / 2;
                pnn.y = Game.Boss.y + Game.Boss.img.Height;
                Game.BossBullets.Add(pnn);
            }
        }

        void MoveBoss()
        {
            Game.Boss.x += Game.BossMovement[Game.IM];
            Game.IM = (Game.IM + 1) % Game.BossMovement.Length;
            isBossHit();

            if (Game.Boss.x < 0)
            {
                Game.Boss.x = 0;
            }
            else if (Game.Boss.x > ClientSize.Width - Game.Boss.img.Width)
            {
                Game.Boss.x = ClientSize.Width - Game.Boss.img.Width;
            }
            if (Game.BossHealth <= 0)
            {
                Game.Boss = null;
                Key[0].cf = 1;
                Key[1].cf = 0;
                Key[1].x = Key[1].y = 0;
                Fstate = 1;
            }
        }

        void DrawBoss(Graphics g)
        {
            Pen p = new Pen(Color.DarkRed, 5);
            SolidBrush b = new SolidBrush(Color.DarkRed);
            int x = Game.Boss.x + 50;
            for (int i = 0; i < Game.BossHealth; i++)
            {
                g.FillRectangle(b, x, Game.Boss.y - 30, 30, 20);
                x += 30;
            }
            g.DrawImage(Game.Boss.img, Game.Boss.x, Game.Boss.y);
        }

        void CreateBoss()
        {
            Game.Boss.img = new Bitmap("minigame\\boss.png");
            Game.Boss.x = 200;
            Game.Boss.y = 100;
        }

        void CreateChickens()
        {
            int x = -150, y = 50;
            Random r = new Random();
            for (int i = 0; i < 27; i++)
            {
                Tile pnn = new Tile();
                pnn.imgs.Add(new Bitmap("minigame\\c1.png"));
                pnn.imgs.Add(new Bitmap("minigame\\c2.png"));
                pnn.x = x;
                pnn.y = y;
                pnn.cf = r.Next(0, 2);
                Game.Chickens.Add(pnn);
                x -= pnn.imgs[0].Width + 50;

                if (i > 8 && i < 18)
                {
                    pnn.h = 1;
                }
                else
                {
                    pnn.h = 2;
                }
                if (i == 8 || i == 17)
                {
                    y += 150;
                    x = -150;
                }
            }
        }

        void CreateRocketBullet()
        {
            Tile pnn = new Tile();
            pnn.imgs.Add(new Bitmap("minigame\\bullet1.png"));
            pnn.x = Game.Rocket.x + Game.Rocket.img.Width / 2 - pnn.imgs[0].Width / 2;
            pnn.y = Game.Rocket.y;
            Game.RocketBullets.Add(pnn);
        }

        void CreateMiniGame()
        {
            Game.Background = new Bitmap("minigame\\bg.png");
            Game.Rocket.img = new Bitmap("minigame\\rocket.png");
            Game.Rocket.y = ClientSize.Height - Game.Rocket.img.Height - 50;
            Game.Rocket.x = ClientSize.Width / 2 - Game.Rocket.img.Width / 2;

            Game.yStart2 = Game.yStart - ClientSize.Height;
            CreateChickens();
            Game.Heart = new Bitmap("minigame\\heart.png");
            CreateBoss();
        }
        void CreateStartMenu()
        {
            Tile pnn = new Tile();
            pnn.img = new Bitmap("11.png");
            Start.Add(pnn);

            pnn = new Tile();
            pnn.img = new Bitmap("22.png");
            Start.Add(pnn);
        }
        void CreateHealers()
        {
            Tile pnn = new Tile();
            pnn.img = new Bitmap("healer.png");
            pnn.x = tables[0].x + tables[0].img.Width / 2 - pnn.img.Width - 30;
            pnn.y = tables[0].y - 15;
            Healers.Add(pnn);

            pnn = new Tile();
            pnn.img = new Bitmap("healer.png");
            pnn.x = tables[1].x + tables[1].img.Width - pnn.img.Width - 30;
            pnn.y = tables[1].y - 15;
            Healers.Add(pnn);

            //pnn = new Tile();
            //pnn.img = new Bitmap("healer.png");
            //pnn.x = tables[2].x + tables[2].img.Width / 2 - pnn.img.Width - 30;
            //pnn.y = tables[2].y - 10;
            //Healers.Add(pnn);

            pnn = new Tile();
            pnn.img = new Bitmap("healer.png");
            pnn.x = tables[4].x + tables[4].img.Width - pnn.img.Width - 30;
            pnn.y = tables[4].y - 10;
            Healers.Add(pnn);
        }
        void CreateKey()
        {
            Tile pnn = new Tile();
            pnn.img = new Bitmap("4.png");
            pnn.x = ClientSize.Width - pnn.img.Width - 30;
            pnn.y = 20;
            Key.Add(pnn);

            pnn = new Tile();
            pnn.img = new Bitmap("5.png");
            pnn.x = tables[3].x + tables[3].img.Width / 2 - pnn.img.Width - 30;
            pnn.y = tables[3].y + 20;
            pnn.cf = 1;
            Key.Add(pnn);
        }
        void CreateBackground()
        {
            CActor pnn = new CActor();
            pnn.Img = new Bitmap("map.png");
            pnn.X = 0;
            pnn.Y = 0;
            Background.Add(pnn);

            pnn = new CActor();
            pnn.Img = new Bitmap("map2.png");
            pnn.X = 0;
            pnn.Y = 0;
            Background.Add(pnn);
        }
        void Createfloor()
        {
            int floory = 450, yadd = 0;
            for (int i = 0; i < 5; i++)
            {
                Tile pnn = new Tile();
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
            Tile pnn = new Tile();
            pnn.x = 2280;
            pnn.y = 1857;
            pnn.img = new Bitmap("pillars\\p51.png");
            walls.Add(pnn);
            pnn = new Tile();
            pnn.x = 1560;
            pnn.y = 1390;
            pnn.img = new Bitmap("pillars\\p411.png");
            walls.Add(pnn);
            pnn = new Tile();
            pnn.x = 840;
            pnn.y = 935;
            pnn.img = new Bitmap("pillars\\p3.png");
            walls.Add(pnn);
            pnn = new Tile();
            pnn.x = 2630;
            pnn.y = 938;
            pnn.img = new Bitmap("pillars\\p31.png");
            walls.Add(pnn);
            pnn = new Tile();
            pnn.x = 1830;
            pnn.y = 478;
            pnn.img = new Bitmap("pillars\\p21.png");
            walls.Add(pnn);
        }
        void CreateTables()
        {
            Tile pnn = new Tile();
            pnn.img = new Bitmap("Tables\\table-2.png");
            pnn.x = 1830 - pnn.img.Width - 30;
            pnn.y = floor[1].y - pnn.img.Height;
            tables.Add(pnn);
            pnn = new Tile();
            pnn.img = new Bitmap("Tables\\table-2.png");
            pnn.x = 1830 + walls[2].img.Width + 30;
            pnn.y = floor[1].y - pnn.img.Height;
            tables.Add(pnn);
            pnn = new Tile();
            pnn.img = new Bitmap("Tables\\table-5.png");
            pnn.x = 30;
            pnn.y = floor[3].y - pnn.img.Height;
            tables.Add(pnn);
            pnn = new Tile();
            pnn.img = new Bitmap("Tables\\table-3.png");
            pnn.x = 840 - pnn.img.Width - 30;
            pnn.y = floor[2].y - pnn.img.Height;
            tables.Add(pnn);
            pnn = new Tile();
            pnn.img = new Bitmap("Tables\\table-3.png");
            pnn.x = 840 + walls[2].img.Width + 30;
            pnn.y = floor[2].y - pnn.img.Height;
            tables.Add(pnn);


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
            pnn.X = 2900;
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
        bool CheckPortalRegion(Tile portal)
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
                Tile p = portals[i];
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
                Tile p = portals[i];
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

                    Tile target = portals[dest];

                    Hero.X = target.x + target.imgs[0].Width / 2 - Hero.Idler[0].Width / 2;

                    if (dest == 0)
                    {
                        Hero.floor = 3;
                        Hero.Y = floor[4].y - floor[4].h - Hero.Idler[0].Height;
                    }
                    else
                    {
                        Hero.floor = 2;
                        Hero.Y = floor[2].y - floor[2].h - Hero.Idler[0].Height;
                    }
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
            Tile pnn = new Tile();
            pnn.imgs = new List<Bitmap>();
            for (int i = 1; i < 6; i++)
            {
                pnn.imgs.Add(new Bitmap("portals\\redp" + i + ".png"));
            }
            pnn.x = Background[Background[0].IF].Img.Width - pnn.imgs[0].Width - 50;
            pnn.y = floor[4].y - floor[3].h - pnn.imgs[0].Height + 25;
            pnn.cf = 0;
            portals.Add(pnn);
            pnn = new Tile();
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
        void Createtor()
        {
            for (int i = 1; i <= 12; i++)
            {
                if (i <= 5) { tor.Attack1r.Add(new Bitmap("tor\\torR\\attack\\attacktor" + i + ".png")); }
                if (i <= 5) { tor.Attack1l.Add(new Bitmap("tor\\torL\\attack\\attacktor" + i + "_result.png")); }
                if (i <= 5) { tor.Deadr.Add(new Bitmap("tor\\torR\\dead\\deadtor" + i + ".png")); }
                if (i <= 5) { tor.Deadl.Add(new Bitmap("tor\\torL\\dead\\deadtor" + i + "_result.png")); }
                if (i <= 3) { tor.Hurtr.Add(new Bitmap("tor\\torR\\hurt\\hurttor" + i + ".png")); }
                if (i <= 3) { tor.Hurtl.Add(new Bitmap("tor\\torL\\hurt\\hurttor" + i + "_result.png")); }
                if (i <= 10) { tor.Idler.Add(new Bitmap("tor\\torR\\idle\\idletor" + i + ".png")); }
                if (i <= 10) { tor.Idlel.Add(new Bitmap("tor\\torL\\idle\\idletor" + i + "_result.png")); }
                tor.Walkr.Add(new Bitmap("tor\\torR\\walk\\walktor" + i + ".png"));
                tor.Walkl.Add(new Bitmap("tor\\torL\\walk\\walktor" + i + "_result.png"));
            }
            tor.xadd = new int[] { 0, 0, 0, -8, -21, -27, 0, 0, 0, 0 };
            tor.X = 1150;
            tor.Y = floor[0].y - tor.Idler[2].Height - 40;
            tor.State = 0;
            tor.Health = 100;
            tor.Startpoint = 900;
            tor.endpoint = 2400;
            tor.D = 1;
            tor.IdleTimer = 20;
            tor.DetectRange = 700;
            tor.AttackRange = 80;
            tor.Speed = 20;
            tor.RecoveryDuration = 12;
            tor.HurtStunDuration = 6;
            tor.floor = 0;
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
            Skewar.Health = 50;
            Skewar.Startpoint = 1900;
            Skewar.endpoint = 2530;
            Skewar.D = 1;
            Skewar.IdleTimer = 15;
            Skewar.DetectRange = 350;
            Skewar.AttackRange = 70;
            Skewar.Speed = 15;
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
            Skespe.Health = 50;
            Skespe.Startpoint = 1800;
            Skespe.endpoint = 4000;
            Skespe.D = 1;
            Skespe.IdleTimer = 15;
            Skespe.DetectRange = 500;
            Skespe.AttackRange = 70;
            Skespe.Speed = 15;
            Skespe.RecoveryDuration = 10;
            Skespe.HurtStunDuration = 5;
            Skespe.floor = 3;
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
            Demon.Y = floor[4].y - Demon.Idler[0].Height - 200;
            DemonBaseY = Demon.Y;
            Demon.State = 0;
            Demon.Health = 40;
            Demon.Startpoint = walls[0].x + walls[0].img.Width + 100;
            Demon.endpoint = portals[0].x - 100;
            Demon.D = 1;
            Demon.IdleTimer = 15;
            Demon.DetectRange = 400;
            Demon.AttackRange = 80;
            Demon.Speed = 14;
            Demon.RecoveryDuration = 15;
            Demon.HurtStunDuration = 4;
            Demon.floor = 4;

            for (int i = 0; i < Demon.Attack1r.Count; i++) Demon2.Attack1r.Add(Demon.Attack1r[i]);
            for (int i = 0; i < Demon.Attack1l.Count; i++) Demon2.Attack1l.Add(Demon.Attack1l[i]);
            for (int i = 0; i < Demon.Deadr.Count; i++) Demon2.Deadr.Add(Demon.Deadr[i]);
            for (int i = 0; i < Demon.Deadl.Count; i++) Demon2.Deadl.Add(Demon.Deadl[i]);
            for (int i = 0; i < Demon.Walkr.Count; i++) Demon2.Walkr.Add(Demon.Walkr[i]);
            for (int i = 0; i < Demon.Walkl.Count; i++) Demon2.Walkl.Add(Demon.Walkl[i]);
            for (int i = 0; i < Demon.Hurtr.Count; i++) Demon2.Hurtr.Add(Demon.Hurtr[i]);
            for (int i = 0; i < Demon.Hurtl.Count; i++) Demon2.Hurtl.Add(Demon.Hurtl[i]);
            for (int i = 0; i < Demon.Idler.Count; i++) Demon2.Idler.Add(Demon.Idler[i]);
            for (int i = 0; i < Demon.Idlel.Count; i++) Demon2.Idlel.Add(Demon.Idlel[i]);

            Demon2.X = 300;
            Demon2.Y = ClientSize.Height - Demon2.Idler[0].Height - 250;
            DemonBaseY2 = Demon2.Y;
            Demon2.State = 0;
            Demon2.Health = 40;
            Demon2.Startpoint = 50;
            Demon2.endpoint = ClientSize.Width / 2 - 100;
            Demon2.D = 1;
            Demon2.IdleTimer = 15;
            Demon2.DetectRange = 600;
            Demon2.AttackRange = 80;
            Demon2.Speed = 12;
            Demon2.RecoveryDuration = 15;
            Demon2.HurtStunDuration = 4;
            Demon2.floor = -1;

            
            for (int i = 0; i < Demon.Attack1r.Count; i++) Demon3.Attack1r.Add(Demon.Attack1r[i]);
            for (int i = 0; i < Demon.Attack1l.Count; i++) Demon3.Attack1l.Add(Demon.Attack1l[i]);
            for (int i = 0; i < Demon.Deadr.Count; i++) Demon3.Deadr.Add(Demon.Deadr[i]);
            for (int i = 0; i < Demon.Deadl.Count; i++) Demon3.Deadl.Add(Demon.Deadl[i]);
            for (int i = 0; i < Demon.Walkr.Count; i++) Demon3.Walkr.Add(Demon.Walkr[i]);
            for (int i = 0; i < Demon.Walkl.Count; i++) Demon3.Walkl.Add(Demon.Walkl[i]);
            for (int i = 0; i < Demon.Hurtr.Count; i++) Demon3.Hurtr.Add(Demon.Hurtr[i]);
            for (int i = 0; i < Demon.Hurtl.Count; i++) Demon3.Hurtl.Add(Demon.Hurtl[i]);
            for (int i = 0; i < Demon.Idler.Count; i++) Demon3.Idler.Add(Demon.Idler[i]);
            for (int i = 0; i < Demon.Idlel.Count; i++) Demon3.Idlel.Add(Demon.Idlel[i]);

            Demon3.X = 900;
            Demon3.Y = ClientSize.Height - Demon3.Idler[0].Height - 250;
            DemonBaseY3 = Demon3.Y;
            Demon3.State = 0;
            Demon3.Health = 40;
            Demon3.Startpoint = ClientSize.Width / 2 + 100;
            Demon3.endpoint = ClientSize.Width - 100;
            Demon3.D = -1;
            Demon3.IdleTimer = 15;
            Demon3.DetectRange = 600;
            Demon3.AttackRange = 80;
            Demon3.Speed = 15;
            Demon3.RecoveryDuration = 15;
            Demon3.HurtStunDuration = 4;
            Demon3.floor = -1;
        }
        void CreateSat()
        {
            for (int i = 1; i <= 12; i++)
            {
                if (i <= 12) { Sat.Walkr.Add(new Bitmap("sat\\satR\\walk\\satwalk" + i + ".png")); }
                if (i <= 12) { Sat.Walkl.Add(new Bitmap("sat\\satL\\walk\\satwalk" + i + "_result.png")); }
                if (i <= 8) { Sat.Attack2r.Add(new Bitmap("sat\\satR\\attack\\satattack" + i + ".png")); }
                if (i <= 8) { Sat.Attack2l.Add(new Bitmap("sat\\satL\\attack\\satattack" + i + "_result.png")); }
                if (i <= 7) { Sat.Idler.Add(new Bitmap("sat\\satR\\idle\\satidle" + i + ".png")); }
                if (i <= 7) { Sat.Idlel.Add(new Bitmap("sat\\satL\\idle\\satidle" + i + "_result.png")); }
                if (i <= 4) { Sat.Deadr.Add(new Bitmap("sat\\satR\\dead\\satdead" + i + ".png")); }
                if (i <= 4) { Sat.Deadl.Add(new Bitmap("sat\\satL\\dead\\satdead" + i + "_result.png")); }
                if (i <= 4) { Sat.Hurtr.Add(new Bitmap("sat\\satR\\hurt\\sathurt" + i + ".png")); }
                if (i <= 4) { Sat.Hurtl.Add(new Bitmap("sat\\satL\\hurt\\sathurt" + i + "_result.png")); }
            }
            Sat.X = 2800;
            Sat.Y = floor[1].y - Sat.Idler[0].Height - 30;
            Sat.State = 0;
            Sat.Health = 35;
            Sat.Startpoint = 2800;
            Sat.endpoint = 3780 - Sat.Idler[0].Width - 50;
            Sat.D = 1;
            Sat.IdleTimer = 15;
            Sat.DetectRange = 500;
            Sat.AttackRange = 400;
            Sat.Speed = 20;
            Sat.RecoveryDuration = 45;
            Sat.HurtStunDuration = 6;
            Sat.floor = 1;
        }
        bool CheckWalls(string Direction)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                if (Hero.X + Hero.Idler[0].Width + 45 > walls[i].x && Hero.X < walls[i].x && Hero.Y >= walls[i].y && Hero.Y < walls[i].y + walls[i].img.Height && Direction == "d")
                {
                    return false;
                }
                else if (Hero.X - 130 < walls[i].x + walls[i].img.Width && Hero.X > walls[i].x && Hero.Y >= walls[i].y && Hero.Y < walls[i].y + walls[i].img.Height && Direction == "a")
                {
                    return false;
                }
            }
            return true;
        }
        bool Checktable(string Direction)
        {
            for (int i = 0; i < tables.Count; i++)
            {
                if (Hero.X + Hero.Idler[0].Width + 50 > tables[i].x && Hero.X < tables[i].x && Hero.Y + 200 >= tables[i].y && Hero.Y < tables[i].y + tables[i].img.Height && Direction == "d")
                {
                    return false;
                }
                else if (Hero.X - 70 < tables[i].x + tables[i].img.Width && Hero.X > tables[i].x && Hero.Y + 200 >= tables[i].y && Hero.Y < tables[i].y + tables[i].img.Height && Direction == "a")
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
            if (Hero.State != 9 && (ctJump > JumpLimit || ctJump == 0) && Fstate == 1)
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
                        Hero.floor = i;
                        return false;
                    }
                }
                Hero.floor = -1;
                Hero.Y += 25;
            }
            else if (Hero.State != 9 && (ctJump > JumpLimit || ctJump == 0) && Fstate == 3)
            {
                if (Hero.Y + Hero.Idler[0].Height > ClientSize.Height - 100)
                {
                    Hero.Y = ClientSize.Height - Hero.Idler[0].Height - 80;
                    return false;
                }
                Hero.Y += 25;
            }
            else if (Hero.State == 9)
            {
                Hero.floor = -1;
            }

            return true;
        }
        void CheckBorders()
        {
            if (Fstate == 1)
            {
                BStartX = Hero.X - ClientSize.Width / 2;
                BStartY = Hero.Y - ClientSize.Height / 2;

                if (BStartX < 0) BStartX = 0;
                if (BStartY < 0) BStartY = 0;
                if (BStartX > Background[Background[0].IF].Img.Width - ClientSize.Width) BStartX = Background[Background[0].IF].Img.Width - ClientSize.Width;
                if (BStartY > Background[Background[0].IF].Img.Height - ClientSize.Height) BStartY = Background[Background[0].IF].Img.Height - ClientSize.Height;
            }
        }
        void createdoor()
        {
            Door.img = new Bitmap("door.png");
            Door.y = floor[0].y - Door.img.Height;
            Door.x = 50;
        }
        void drawdoor(Graphics g)
        {
            g.DrawImage(Door.img, Door.x - BStartX, Door.y - BStartY);
        }
    }

}