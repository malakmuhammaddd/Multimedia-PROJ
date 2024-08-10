using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Large_Game
{
    public class Background
    {
        public Bitmap img;
        public Rectangle rcDsc;
        public Rectangle rcSrc;
    }
    public class ground
    {
        public Bitmap img;
        public int X,Y;
        public int dx = -1;
    }
    public class Elevator
    {
        public Bitmap img;
        public int X, Y;
        public int dy = -1;
        public int dx = 1;
    }
    public class gate
    {
        public Bitmap img;
        public Rectangle rcDes;
        public Rectangle rcSrc;
    }

    public class adv_bullet_img
    {
        public List<Bitmap> Limgs = new List<Bitmap>();
        public Rectangle Rct_Rrc, Rct_Dst;
        public int W, H;
        public int icurr;
        public int dir; // one will be right 
        public bool end;
    }

    public class enemy1
    {
        public Rectangle Rct_src, Rct_dst;
        public int w, h;
        public bool live = true;
        public Bitmap img;
        public List<adv_bullet_img> LEBullets = new List<adv_bullet_img>(); 
    }
    public class Hero
    {
        public List<Bitmap> Limgs = new List<Bitmap>();
        public List<Bitmap> Rimgs = new List<Bitmap>();
        public List<Bitmap> Jimgs = new List<Bitmap>();
        public Rectangle rcDes;
        public Rectangle rcSrc;
        public int icurr;
        public int flag_state;
        public int dx = 1;
        public int dy = -1;
        public bool live = true;
        public List<adv_bullet_img> Lbullets = new List<adv_bullet_img>();
        
        // flag state 1 will be right run
        // flag state 2 will be left run
        // flag state 3 will be jump right
        // flag state 4 will be jump left


    }
    public class Cline
    {
        public int x1,x2,y1,y2;
    }
    public partial class Form1 : Form
    {
        Bitmap off; int p = 0; int c = 0;
        Bitmap fimg;
        Background BK = new Background();
        List<ground> G = new List<ground>();
        Elevator E = new Elevator();
        gate Sgate = new gate();
        gate Egate = new gate();
        gate spike = new gate();
        List <Hero> H = new List <Hero>();
        List<gate> Ladderthabet = new List<gate>();
        List<Cline> L = new List<Cline>();
        List<Cline> L2 = new List<Cline>();
        List<enemy1> enemy = new List <enemy1> ();
        Timer t = new Timer();
        int tclick = 0; int ctgravity=0; int u = 0; int pp = 0;
        int xx1 = 200; int xx2 = 200; int yy1 = 300; int yy2 = 400;
        int ctt1 = 0; int ctt2 = 0; int T = 0; int oo = 0;
        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            t.Tick += T_Tick;
            t.Start();
            t.Interval = 50;
        }

        
        private void T_Tick(object sender, EventArgs e)
        {
            tclick++;
            
            ///////Movement
            //Movenemies();
            //EnemiesIsdeadWhen();
            MoveElevator();
            MoveLadderMota7arek();
            isEnemy_touched_by_bullet();
            //SingleBullets();
            //MultipleBullets();
            if (tclick % 10 == 0)
            {
                if (c == 0)
                {
                    CreateLaser();
                }
                else if (c == 1)
                {
                    RemoveLaser();
                }
                CreateLaser2();
            }
            if (tclick % 50 == 0)
            {
                L2.Clear();
            }
            if (H[0].rcDes.X >= G[3].X && u==0)
            {
                ctgravity++;
                if (ctgravity > 10)
                {
                    H[0].rcDes.Y -= H[0].dy *10;
                }
                if(H[0].rcDes.Y>=705)
                {
                    u = 1;
                }
            }
            
            if (T == 1)
            {
                if (ctt1 < 10)
                {
                    H[0].rcDes.Y += H[0].dy * 15;
                    H[0].rcDes.X += H[0].dx * 15;
                    ctt1++;
                }
                else if (ctt2 < 10)
                {
                    H[0].rcDes.Y -= H[0].dy * 15;
                    H[0].rcDes.X += H[0].dx * 15;


                    ctt2++;
                }
                else
                {
                    ctt1 = 0;
                    ctt2 = 0;
                    T = 0;
                }

                if (H[0].rcDes.Y >= 705)
                {
                    H[0].rcDes.Y -= H[0].dy * 10;
                }
            }
            if(oo==1)
            {
                t.Stop();
                oo = 2;
                MessageBox.Show("GAME OVER");
            }
            if(pp==1)
            {
                t.Stop();
                pp = 2;
                MessageBox.Show("GAME OVER");
            }
            //JumpingDiagonalright();
            //JumpingDiagonalleft();
            //DoubleJumping();
            HeroIsdeadWhen();
            //HeroWin();

            for(int i= 0;i < H[0].Lbullets.Count;i++)
            {
                if(!H[0].Lbullets[i].end)
                {
                    H[0].Lbullets[i].icurr++;
                    H[0].Lbullets[i].icurr %= 7;
                }
                if (H[0].Lbullets[i].icurr == 6) H[0].Lbullets[i].end = true;
                H[0].Lbullets[i].Rct_Dst.X += 20 * H[0].Lbullets[i].dir;
            }

            if(tclick%20==0)
            {
                create_enemy_bullet();
            }
            for (int i = 0; i < enemy[0].LEBullets.Count; i++)
            {
                
                enemy[0].LEBullets[i].Rct_Dst.X += 20 * enemy[0].LEBullets[i].dir;
            }
            isEnemy_touched_Hero_by_bullet();
            DrawDubb(this.CreateGraphics());
        }
        void HeroIsdeadWhen()
        {
            for (int i = 0; i < L.Count; i++)
            {
                if (H[0].rcDes.Right >= L[i].x1 && H[0].rcDes.Left <= L[i].x2 && H[0].rcDes.Bottom >= L[i].y1 && H[0].rcDes.Top <= L[i].y2)
                {
                    oo = 1;
                }
            }
            //if(H[0].rcDes.X>= spike.rcDes.X + spike.rcDes.Width && H[0].rcDes.Y>= 900)
            //{
            //    H[0].rcDes.Y -= H[0].dy * 15;
            //
            //    //oo = 1;
            //}
            
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                H[0].flag_state = 1;
                p = 1;
                if (BK.rcSrc.X + BK.rcSrc.Width <= BK.img.Width)
                {
                    BK.rcSrc.X += 20;
                }
                for (int i = 0; i <G.Count; i++)
                {
                    G[i].X-=20;
                }
                for (int i = 0; i < Ladderthabet.Count; i++)
                {
                    Ladderthabet[i].rcDes.X -= 20;
                }
                // the movement of the hero 
                H[0].icurr++;
                H[0].icurr = H[0].icurr % 10;

                spike.rcDes.X -= 20;
                Egate.rcDes.X -= 20;
                Sgate.rcDes.X -= 20;
                E.X -= 20;
                for (int i = 0; i < L.Count; i++)
                {
                    L[i].x1 -= 20;
                    L[i].x2 -= 20;
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                H[0].flag_state = 2;
                BK.rcSrc.X -= 20;
                if (BK.rcSrc.X < 0)
                {
                    BK.rcSrc.X = 0;
                }
                for (int i = 0; i < G.Count; i++)
                {
                    G[i].X += 20;
                }
                for (int i = 0; i < Ladderthabet.Count; i++)
                {
                    Ladderthabet[i].rcDes.X += 20;
                }

                H[0].icurr++;
                H[0].icurr = H[0].icurr % 10;

                E.X += 20;
                spike.rcDes.X += 20;
                Egate.rcDes.X += 20;
                Sgate.rcDes.X += 20;
            }
            if (e.KeyCode == Keys.Down)
            {
                if (BK.rcSrc.Y + BK.rcSrc.Height <= BK.img.Height)
                {
                    BK.rcSrc.Y += 20;
                }
                for (int i = 0; i < G.Count; i++)
                {
                    G[i].Y -= 22;   
                }
                for (int i = 0; i < Ladderthabet.Count; i++)
                {
                    Ladderthabet[i].rcDes.Y -= 22;
                }
                spike.rcDes.Y += 20;
                Egate.rcDes.Y -= 20;
                Sgate.rcDes.Y -= 20;
                E.Y -= 20;
            }
            if (e.KeyCode == Keys.Up)
            {
                BK.rcSrc.Y -= 20;
                if (BK.rcSrc.Y < 0)
                {
                    BK.rcSrc.Y = 0;
                }
                for (int i = 0; i < G.Count; i++)
                {
                    G[i].Y += 22;
                }
                for(int i =0;i < Ladderthabet.Count;i++)
                {
                    Ladderthabet[i].rcDes.Y += 22;
                }
                
                if (H[0].rcDes.X >= Ladderthabet[0].rcDes.X && H[0].rcDes.X <= Ladderthabet[0].rcDes.X + Ladderthabet[0].rcDes.Width)
                {
                    H[0].rcDes.Y -= 5;
                }
                
                
                E.Y += 20;
                spike.rcDes.Y += 20;
                Egate.rcDes.Y += 20;
                Sgate.rcDes.Y += 20;
            }
            if(e.KeyCode == Keys.J)
            {
                T = 1;
            }
            if(e.KeyCode == Keys.B)
            {
                CreateBullets();
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.Width, this.Height);
            CreateBK();
            CreateGround1(); //top
            CreateGround2(); //after
            CreateGround3(); //after spike
            CreateGround4(); //after
            CreateGround5();//enemies
            CreateGround6(); //down1
            CreateGround7(); //down2
            CreateSpike();
            CreateSgate();
            CreateLadderMota7arek();
            CreateGround8(); //down3
            CreateLadderThabet();
            CreateGround9(); 
            CreateGround10(); 
            CreateElevator(); 
            CreateGround11(); 
            CreateEndgate();
            CreateHero();
            Create_Enemy();
        }
        void CreateBullets()
        {
            adv_bullet_img pnn = new adv_bullet_img();
            pnn.icurr = 0; 
            if (H[0].flag_state == 1) // will do the bullets of the right 
            {
                pnn.dir = 1; // direction of the bullet 
                for(int i =1; i <=7;i ++)
                {
                    fimg = new Bitmap("BR(" + i + ").png");
                    Color clr = fimg.GetPixel(0, 0);
                    fimg.MakeTransparent(clr);
                    pnn.Limgs.Add(fimg);
                }
                pnn.Rct_Dst = new Rectangle(H[0].rcDes.X + 50, H[0].rcDes.Y + 20, 30, 50);
                pnn.Rct_Rrc = new Rectangle(0, 0, fimg.Width, fimg.Height);
                H[0].Lbullets.Add(pnn);
            }
            else if (H[0].flag_state == 2) // left bullets will be done
            {
                pnn.dir = -1;
                for (int i = 1; i <= 7; i++)
                {
                    fimg = new Bitmap("BL(" + i + ").png");
                    Color clr = fimg.GetPixel(0, 0);
                    fimg.MakeTransparent(clr);
                    pnn.Limgs.Add(fimg);
                }
                pnn.Rct_Dst = new Rectangle(H[0].rcDes.X + 50, H[0].rcDes.Y + 20, 30, 50);
                pnn.Rct_Rrc = new Rectangle(0, 0, fimg.Width, fimg.Height);
                H[0].Lbullets.Add(pnn);
            }
        }
        void RemoveLaser()
        {

            if (L.Count > 0)
            {
                L.RemoveAt(L.Count - 1);

                if (L.Count == 0)
                {
                    c = 0;
                    
                }
            }


        }
       
        void create_enemy_bullet()
        {

            adv_bullet_img pnn = new adv_bullet_img();
            pnn.icurr = 0; 

            pnn.dir = -1; // direction of the bullet
            fimg = new Bitmap("Benemy.png");
            Color clr = fimg.GetPixel(0, 0);
            fimg.MakeTransparent(clr);
            pnn.Limgs.Add(fimg);
            
            
            pnn.Rct_Dst = new Rectangle(enemy[0].Rct_dst.X + 50, enemy[0].Rct_dst.Y + 20, 30, 50);
            pnn.Rct_Rrc = new Rectangle(0, 0, fimg.Width, fimg.Height);
            enemy[0].LEBullets.Add(pnn);

        }
        void Create_Enemy()
        {
            enemy1 pnn = new enemy1();
            enemy.Add(pnn);
            for (int i = 0; i < enemy.Count; i++)
            {
                Bitmap img = new Bitmap("Enemy.png");
                Color clr = img.GetPixel(0, 0);
                img.MakeTransparent(clr);
                enemy[i].img = img;
                enemy[i].Rct_dst = new Rectangle(600, 540, enemy[i].img.Width + 60, enemy[i].img.Height + 60);
                enemy[i].Rct_src = new Rectangle(0, 0, enemy[i].img.Width, enemy[i].img.Height);
            }
        }
        void CreateHero()
        {
            Hero pnn = new Hero();
            H.Add(pnn);
            for (int i = 0; i < H.Count; i++)
            {
                
                H[i].flag_state = 1; H[i].icurr = 0; 
                for (int k = 1; k <= 10; k++)
                {
                    Bitmap img = new Bitmap("HL(" + k + ").png");
                    Color clr = img.GetPixel(0, 0);
                    img.MakeTransparent(clr);
                    H[i].Limgs.Add(img);
                }

                for (int k = 1; k <= 10; k++)
                {
                    Bitmap img = new Bitmap("HR(" + k + ").png");
                    Color clr = img.GetPixel(0, 0);
                    img.MakeTransparent(clr);
                    H[i].Rimgs.Add(img);

                }

                H[i].rcDes = new Rectangle(50, 540, H[i].Rimgs[H[i].icurr].Width + 60, H[i].Rimgs[H[i].icurr].Height + 60);
                H[i].rcSrc = new Rectangle(0, 0, H[i].Rimgs[H[i].icurr].Width, H[i].Rimgs[H[i].icurr].Height);
            }
           
        }

        void isEnemy_touched_by_bullet()
        {
            for(int i =0; i < H[0].Lbullets.Count;i++)
            {
                if(enemy.Count!=0)
                {
                    if (H[0].Lbullets[i].Rct_Dst.X >= enemy[0].Rct_dst.X && H[0].Lbullets[i].Rct_Dst.X < enemy[0].Rct_dst.X + 50)
                    {
                        enemy[0].live = false;
                        enemy.Clear();
                        H[0].Lbullets.RemoveAt(i);
                    }
                }    
            }
        }
        void isEnemy_touched_Hero_by_bullet()
        {
            for (int i = 0; i < enemy[0].LEBullets.Count; i++)
            {
                if (H.Count != 0)
                {
                    if (enemy[0].LEBullets[i].Rct_Dst.X <= H[0].rcDes.X +50 )
                    {
                        H[0].live = false;
                        enemy[0].LEBullets.RemoveAt(i);
                        pp = 1;
                    }
                }
            }
        }
        void CreateLaser()
        {
            Cline pnn2 = new Cline();
            pnn2.x1 = xx1;
            pnn2.x2 = xx2;
            pnn2.y1 = yy1;
            pnn2.y2 = yy2;
            yy1 += 100;
            yy2 += 100;
            if(yy1>=1000)
            {
                c = 1;
                xx1 = 200;
                xx2 = 200;
                yy1 = 300;
                yy2 = 400;
            }
            
            L.Add(pnn2);
        }
        void CreateLaser2()
        {
            Cline pnn2 = new Cline();
            pnn2.x1 = 400;
            pnn2.x2 = 610;
            pnn2.y1 = 600;
            pnn2.y2 = 600;
            L2.Add(pnn2);
        }
        void MoveLadderMota7arek()
        {
            G[21].X += G[21].dx * 3;
            if (G[21].X<= G[22].X+410)
            {
                G[21].dx *= -1;
            }
            if (G[21].X >= G[18].X  )
            {
                G[21].dx *= -1;
            }
        }
        void MoveElevator()
        {
            E.X += E.dx * 4;
            if (p == 0)
            {
                if (E.X >= G[4].X + 135 && E.Y == 450)
                {
                    E.dx *= -1;
                    p = 1;
                }
            }
           if (E.X <= G[3].X + 200 && E.X <= G[4].X + 135&&p==1)
           {
                if (E.Y <= 960)
                {
                    E.dy = 1;
                    E.Y += E.dy * 4;
                    E.X -= E.dx * 4;
                }
                if (E.Y >= 960)
                {
                    p = 2;
                }
           }
           if(p==2)
           {
                E.dx *= 1;
                if(E.X <= G[3].X +28 )
                {
                    E.dx *= -1;
                    p = 3;
                }
           }
           if (p == 3)
           {
                if (E.X >= G[4].X)
                {
                    E.dy = -1;
                    E.Y += E.dy * 4;
                    E.X -= E.dx * 4;
                    if (E.Y <= 450)
                    {
                        p = 0;
                    }
                }
           }
        }
        void CreateEndgate()
        {
            Egate.img = new Bitmap("Endgate.png");
            Egate.rcDes = new Rectangle(0, 715, Egate.img.Width /2 +100, Egate.img.Height - 100);
            Egate.rcSrc = new Rectangle(0, 0, Egate.img.Width, Egate.img.Height);
        }
        void CreateGround11()
        {
            int v = -3;
            for (int i = 0; i < 3; i++)
            {
                ground pnn = new ground();
                pnn.img = new Bitmap("ground.png");
                pnn.X = v;
                v = pnn.X + pnn.img.Width - 400;
                pnn.Y = 960;
                G.Add(pnn);
            }
        }
        void CreateElevator()
        {
            E.img = new Bitmap("ground.png");
            E.X = G[4].X ;
            E.Y = 450;
        }
        void CreateGround10()
        {
            ground pnn = new ground();
            pnn.img = new Bitmap("ground.png");
            pnn.X = G[4].X + 340;
            pnn.Y = 450;
            G.Add(pnn);
        }
        void CreateGround9()
        {
            ground pnn = new ground();
            pnn.img = new Bitmap("ground.png");
            pnn.X = G[5].X + 410;
            pnn.Y = 570;
            G.Add(pnn);
        }
        void CreateLadderThabet()
        {
            int r = 510;
            for (int i = 0; i < 3; i++)
            {
                gate pnn = new gate();
                pnn.img = new Bitmap("Ladder.png");
                pnn.rcDes = new Rectangle(G[3].X + G[3].img.Width-200, r, pnn.img.Width, pnn.img.Height - 10);
                pnn.rcSrc = new Rectangle(0, 0, pnn.img.Width, pnn.img.Height);
                Ladderthabet.Add(pnn);
                r += 85;
            }
        }
        void CreateLadderMota7arek()
        {
            ground pnn = new ground();
            pnn.img = new Bitmap("ground.png");
            pnn.X = G[7].X +300 ;
            pnn.Y = 300;
            G.Add(pnn);
        }
        void CreateGround8()
        {
            ground pnn = new ground();
            pnn.img = new Bitmap("ground.png");
            pnn.X = G[6].X ;
            pnn.Y = 300;
            G.Add(pnn);
        }
        void CreateGround7()
        {
            ground pnn = new ground();
            pnn.img = new Bitmap("ground.png");
            pnn.X = G[6].X +200;
            pnn.Y = 300;
            G.Add(pnn);
        }
        void CreateGround6()
        {
            int e = G[9].X + 210;
            for (int i = 0; i < 2; i++)
            {
                ground pnn = new ground();
                pnn.img = new Bitmap("ground.png");
                pnn.X = e;
                pnn.Y = 300;
                e = pnn.X + pnn.img.Width - 380;
                G.Add(pnn);
            }
        }
        void CreateGround5()
        {
            int w = G[9].X +626;
            for (int i = 0; i < 8; i++)
            {
                ground pnn = new ground();
                pnn.img = new Bitmap("ground.png");
                pnn.X = w;
                pnn.Y = 780 + 160; ;
                w = pnn.X + pnn.img.Width - 380;
                G.Add(pnn);
            }
        }
        void CreateGround4()
        {
            int w = G[7].X +210 ;
            for (int i = 0; i < 2; i++)
            {
                ground pnn = new ground();
                pnn.img = new Bitmap("ground.png");
                pnn.X = w;
                pnn.Y = 980;
                w = pnn.X + pnn.img.Width - 380;
                G.Add(pnn);
            }
        }
        void CreateGround3()
        {
            int q = G[5].X + G[5].img.Width + 25; ;
            for (int i = 0; i < 2; i++)
            {
                ground pnn = new ground();
                pnn.img = new Bitmap("ground.png");
                pnn.X = q;
                pnn.Y = 780 + 160;
                q = pnn.X + pnn.img.Width - 380;
                G.Add(pnn);
            }
        }
        void CreateSpike()
        {
            spike.img = new Bitmap("spike.png");
            spike.rcDes = new Rectangle(G[5].X + G[5].img.Width - 373, 972, spike.img.Width, spike.img.Height -10);
            spike.rcSrc = new Rectangle(0, 0, spike.img.Width, spike.img.Height);
        }
        void CreateSgate()
        {
            Sgate.img = new Bitmap("startgate.png");
            Sgate.rcDes= new Rectangle(50, 510, Sgate.img.Width -100, Sgate.img.Height -50);
            Sgate.rcSrc = new Rectangle(0, 0, Sgate.img.Width, Sgate.img.Height);
        }
        void CreateGround1()
        {
            int x = -3;
            for (int i = 0; i < 4; i++)
            {
                ground pnn = new ground();
                pnn.img = new Bitmap("ground.png");
                pnn.X = x;
                x = pnn.X + pnn.img.Width -400;
                pnn.Y = 610;
                G.Add(pnn);
            }
        }
        void CreateGround2()
        {
            int z = G[3].X + G[3].img.Width - 375;
            for (int i = 0; i < 2; i++)
            {
                ground pnn = new ground();
                pnn.img = new Bitmap("ground.png");
                pnn.X = z;
                pnn.Y = 780;
                z = pnn.X + pnn.img.Width - 414;
                G.Add(pnn);
            }
        }
        void CreateBK()
        {
            BK.img = new Bitmap("JOJO JAMBO.png");
            BK.rcDsc = new Rectangle(0, 0, this.Width, this.Height +180);
            BK.rcSrc = new Rectangle(0, 0, this.Width, this.Height );
        }
        void DrawScene(Graphics g)
        {
            g.Clear(Color.Black);
            g.DrawImage(BK.img, BK.rcDsc, BK.rcSrc, GraphicsUnit.Pixel);
            for (int i = 0; i < G.Count; i++)
            {
                g.DrawImage(G[i].img, G[i].X, G[i].Y);
            }
            g.DrawImage(Sgate.img, Sgate.rcDes, Sgate.rcSrc, GraphicsUnit.Pixel);
            g.DrawImage(spike.img, spike.rcDes, spike.rcSrc, GraphicsUnit.Pixel);
            for (int i = 0; i < Ladderthabet.Count; i++)
            {
                g.DrawImage(Ladderthabet[i].img, Ladderthabet[i].rcDes, Ladderthabet[i].rcSrc, GraphicsUnit.Pixel);
            }
            g.DrawImage(Egate.img, Egate.rcDes, Egate.rcSrc, GraphicsUnit.Pixel);
            g.DrawImage(E.img, E.X, E.Y);
            if (L.Count >= 0)
            {
                for (int i = 0; i < L.Count; i++)
                {
                    Pen p = new Pen(Color.Red, 10);
                    g.DrawLine(p, L[i].x1, L[i].y1, L[i].x2, L[i].y2);
                }
            }
            for (int i = 0; i < L2.Count; i++)
            {
                Pen p = new Pen(Color.Yellow, 10);
                g.DrawLine(p, L2[i].x1, L2[i].y1, L2[i].x2, L2[i].y2);
            }
            for (int i = 0; i < H.Count; i++)
            {
                if (H[i].flag_state == 1)
                    g.DrawImage(H[i].Rimgs[H[i].icurr], H[i].rcDes, H[i].rcSrc, GraphicsUnit.Pixel);
                else if (H[i].flag_state == 2)
                    g.DrawImage(H[i].Limgs[H[i].icurr], H[i].rcDes, H[i].rcSrc, GraphicsUnit.Pixel);
                if (H[i].live)
                    g.DrawImage(H[i].Limgs[H[i].icurr], H[i].rcDes, H[i].rcSrc, GraphicsUnit.Pixel);
            }
            for (int i = 0; i < H[0].Lbullets.Count; i++)
            {
                g.DrawImage(H[0].Lbullets[i].Limgs[H[0].Lbullets[i].icurr], H[0].Lbullets[i].Rct_Dst, H[0].Lbullets[i].Rct_Rrc, GraphicsUnit.Pixel);
            }
            for (int i = 0; i < enemy.Count; i++)
            {
                if(enemy[i].live)
                     g.DrawImage(enemy[i].img, enemy[i].Rct_dst, enemy[i].Rct_src, GraphicsUnit.Pixel);
            }
            for (int i = 0; i < enemy[0].LEBullets.Count; i++)
            {
                g.DrawImage(enemy[0].LEBullets[i].Limgs[0], enemy[0].LEBullets[i].Rct_Dst, enemy[0].LEBullets[i].Rct_Rrc, GraphicsUnit.Pixel);
            }
        }
        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);

        }
    }
}
