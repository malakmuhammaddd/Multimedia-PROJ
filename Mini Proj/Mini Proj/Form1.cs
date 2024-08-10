using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mini_Proj
{
    public class CActor
    {
        public int X, Y;
        public List<Bitmap> imgs = new List<Bitmap>();
        public int icurr;
        public int icurr2;
        public int dy=-1;
    }
    public class ground
    {
        public Bitmap img;
        public Rectangle rcDsc;
        public Rectangle rcSrc;
    }
    public class Snail
    {
        public int X, Y;
        public List<Bitmap> imgs = new List<Bitmap>();
        public int icurr;
        public int dy;
    }
    public class Dbana
    {
        public int X, Y;
        public List<Bitmap> imgs = new List<Bitmap>();
        public int icurr;
        public int dy;
    }
    public partial class Form1 : Form
    {
        Bitmap off; int x; int ct1 = 0; int p = 0;
        int o = 0; int ct = 0; int ct2 = 0;
        Bitmap BK = new Bitmap("Sky.png");
        List<ground> G = new List<ground>();
        List<CActor> C = new List<CActor>();
        List<Snail> S = new List<Snail>();
        List<Dbana> D = new List<Dbana>();
        Timer t = new Timer();
        int tclick = 0;
        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            t.Start();
            t.Tick += T_Tick;
        }
        private void T_Tick(object sender, EventArgs e)
        {
            tclick++;
            if (tclick % 25 == 0)
            {
                CreateSnail();
            }
            if (tclick % 25 == 0)
            {
                CreateDbana();
            }
            MoveHero();
            MoveSnail();
            if (o == 0)
            {
                MoveDbana();
            }
            jumping();
            DBuffer(this.CreateGraphics());
            if(o==1)
            {
                t.Stop();
                o = 2;
                MessageBox.Show("GAME OVER");
            }
        }
        void jumping()
        {
            if (p == 1)
            {
                for (int i = 0; i < C.Count; i++)
                {
                    C[i].icurr = 2;
                    if (ct1<10)
                    {
                        C[i].Y += C[i].dy * 30;
                        ct1++;
                    }
                    else if (ct2 < 10)
                    {
                        C[i].dy = 1;
                        C[i].Y += C[i].dy * 30;
                        ct2++;
                    }
                    else
                    {
                        C[i].dy = -1;
                        ct1 = 0;
                        ct2 = 0;
                        p = 0;
                    }
                }
            }
        }

        void CreateDbana()
        {
            Dbana pnn = new Dbana();
            if (ct == 0)
            {
                pnn.X = this.Width;
                ct++;
            }
            if (ct == 1)
            {
                pnn.X = this.Width + x;
            }
            pnn.dy = -1;
            pnn.Y = this.Height - 400;
            for (int k = 0; k < 2; k++)
            {
                Bitmap img = new Bitmap("Fly" + (k + 1) + ".png");
                pnn.imgs.Add(img);
            }
            x += 500;
            D.Add(pnn);
        }
        void MoveDbana()
        {
            for (int i = 0; i < D.Count; i++)
            {
                D[i].X += D[i].dy * 15;
                if (tclick % 5 == 0)
                {
                    D[i].icurr++;
                    if (D[i].icurr >= 2)
                    {
                        D[i].icurr = 0;
                    }
                }
                if (D[i].X <= C[0].X + C[0].imgs[C[0].icurr].Width &&
                    D[i].X + D[i].imgs[D[i].icurr].Width >= C[0].X &&
                    D[i].Y <= C[0].Y + C[0].imgs[C[0].icurr].Height &&
                    D[i].Y + D[i].imgs[D[i].icurr].Height >= C[0].Y)
                {
                    o = 1;
                }

            }
        }
        void MoveHero()
        {
            for (int i = 0; i < C.Count; i++)
            {
                if (tclick % 2 == 0)
                {
                    C[i].icurr++;
                    if (C[i].icurr >= 2)
                    {
                        C[i].icurr = 0;
                    }

                }
                
            }

        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Space && p==0)
            {
                p = 1;
            }
            if (e.KeyCode == Keys.Right)
            {
                for (int i = 0; i < C.Count; i++)
                {
                    C[i].X += 10;
                    if (tclick % 2 == 0)
                    {
                        C[i].icurr++;
                        if (C[i].icurr >= 2)
                        {
                            C[i].icurr = 0;
                        }

                    }

                }
            }
            if (e.KeyCode == Keys.Left)
            {
                for (int i = 0; i < C.Count; i++)
                {
                    C[i].X -= 10;
                    if (tclick % 2 == 0)
                    {
                        C[i].icurr++;
                        if (C[i].icurr >= 2)
                        {
                            C[i].icurr = 0;
                        }

                    }

                }
            }



        }
        void CreateGround()
        {
            ground pnn = new ground();
            pnn.img = new Bitmap("ground.png");
            pnn.rcDsc = new Rectangle(0, this.Height - pnn.img.Height, this.Width, this.Height );
            pnn.rcSrc = new Rectangle(0, 0, pnn.img.Width, this.Height );
            G.Add(pnn);
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DBuffer(e.Graphics);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.Width, this.Height);
            CreateGround();
            CreateActor();
            
        }
        void CreateSnail()
       {
            Snail pnn = new Snail();
            if (ct == 0)
            {
                pnn.X = this.Width;
                ct++;
            }
            if (ct == 1)
            {
                pnn.X = this.Width + x;
            }
            pnn.dy = -1;
            pnn.Y = this.Height - G[0].img.Height - 35;
            for (int k = 0; k < 2; k++)
            {
                Bitmap img = new Bitmap("snail"+ (k+1)+ ".png");
                Color clr = img.GetPixel(0, 0);
                img.MakeTransparent(clr);
                pnn.imgs.Add(img);
            }
            x += 500;
            S.Add(pnn);
       }
        void MoveSnail()
        {
            for (int i = 0; i < S.Count; i++)
            {
                S[i].X += S[i].dy * 15;
                if (tclick % 2 == 0)
                {
                    S[i].icurr++;
                    if (S[i].icurr >= 2)
                    {
                        S[i].icurr = 0;
                    }

                }
                if (S[i].X + S[i].imgs[0].Width >= C[0].X &&
                    S[i].X <= C[0].X + C[0].imgs[C[0].icurr].Width &&
                    S[i].Y + S[i].imgs[0].Height >= C[0].Y &&
                    S[i].Y <= C[0].Y + C[0].imgs[C[0].icurr].Height)
                {
                    o = 1;
                }


            }
        }
        void CreateActor()
        {
            for (int i = 0; i < 1; i++)
            {
                CActor pnn = new CActor();
                pnn.X = 500;
                pnn.Y = this.Height - G[0].img.Height -85;
                for (int k = 0; k < 3; k++)
                {
                    Bitmap img = new Bitmap("Walk" + (k + 1) + ".png");
                    Color clr = img.GetPixel(0, 0);
                    img.MakeTransparent(clr);
                    pnn.imgs.Add(img);
                }
                C.Add(pnn);
            }
        }
        void DBuffer(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
        void DrawScene(Graphics g)
        {
            g.Clear(Color.Black);
            g.DrawImage(BK, 0, 0, this.Width, this.Height);
            g.DrawImage(G[0].img, G[0].rcDsc, G[0].rcSrc, GraphicsUnit.Pixel);
            for (int i = 0; i < C.Count; i++)
            {
                g.DrawImage(C[i].imgs[C[i].icurr], C[i].X, C[i].Y);
            }
            for (int i = 0; i < S.Count; i++)
            {
                g.DrawImage(S[i].imgs[S[i].icurr], S[i].X, S[i].Y);
            }
            for (int i = 0; i < D.Count; i++)
            {
                g.DrawImage(D[i].imgs[D[i].icurr], D[i].X, D[i].Y);
            }
        }
    }
}
