using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace khoangCach
{
    public partial class Form1 : Form
    {
        class MousePoint
        {
            private int x;
            private int y;
            public MousePoint(int x, int y)
            {
                setX(x);
                setY(y);
            }
            public int getX()
            {
                return x;
            }
            public int getY()
            {
                return y;
            }
            public void setX(int x)
            {
                this.x = x;
            }
            public void setY(int y)
            {
                this.y = y;
            }
            //
            public double distanceFrom(MousePoint from)
            {
                double tmp = (double)(x - from.getX()) * (double)(x - from.getX()) + (double)(y - from.getY()) * (double)(y - from.getY());
                return Math.Sqrt(tmp);
            }
            public MousePoint cloneNew()
            {
                MousePoint tmp = new MousePoint(this.getX(), this.getY());
                return tmp;
            }
        }
        private MousePoint current = null;
        private ArrayList list = null;
        public Form1()
        {
            InitializeComponent();
            //init
            current = new MousePoint(0, 0);
            list = new ArrayList();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            current.setX(e.Location.X);
            current.setY(e.Location.Y);
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //add current point to list
            list.Add(this.current.cloneNew());
            richTextBox_log.Text += "Point "+list.Count+" ("+current.getX()+", "+current.getY()+")"+"\n";
            
            


            /*
            Bitmap bmp = (Bitmap)pictureBox1.Image;
            // bmp.GetPixel(current.getX(), current.getY());
            int x = current.getX();
            int y = current.getY();
            int max_y = 1, min_y = -1;
            bool fmax_y = false, fmin_y = false;

            bmp.SetPixel(x,y,Color.Red);
            pictureBox1.Image = bmp;
            return;
            while (!(fmax_y && fmin_y))
            {
                Color min_c = bmp.GetPixel(x, y + min_y);
                if (min_c.R > 200 && min_c.G > 200 && min_c.B > 200) min_y--;
                else fmin_y = true;

                Console.WriteLine("Y -- MinColor({0}, {1}, {2}) - min_y: {3}", min_c.R, min_c.G, min_c.B, min_y);
                Color max_c = bmp.GetPixel(x, y + max_y);
                if (max_c.R > 200 && max_c.G > 200 && max_c.B > 200) max_y++;
                else fmax_y = true;

                Console.WriteLine("Y -- MaxColor({0}, {1}, {2}) - max_y: {3}", max_c.R, max_c.G, max_c.B, max_y);

                bmp.SetPixel(x, y + min_y + 1, Color.Black);
                bmp.SetPixel(x,  y + max_y - 1, Color.Black);
            }


            ////////
            int max_x = 1, min_x = -1;
            bool fmax_x = false, fmin_x = false;

            while (!(fmax_x && fmin_x))
            {
                Color min_c = bmp.GetPixel(x + min_x, y);
                if (min_c.R > 200 && min_c.G > 200 && min_c.B > 200) min_x--;
                else fmin_x = true;

                Color max_c = bmp.GetPixel(x + max_x, y);
                if (max_c.R > 200 && max_c.G > 200 && max_c.B > 200) max_x++;
                else fmax_x = true;
            }

            int fx = (min_x + max_x) / 2;
            int fy = (min_y + max_y) / 2;

            bmp.SetPixel(fx, fy, Color.Red);
            */
        }

        private void button_calculate_Click(object sender, EventArgs e)
        {
            //check
            int total = 0;
            MousePoint tmp_cur;
            MousePoint tmp_prev;
            if(list.Count>=2)
            {
                for(int i=1;i<list.Count;i++)
                {
                    tmp_prev = (MousePoint)list[i-1];
                    tmp_cur = (MousePoint) list[i];
                    total += (int)tmp_cur.distanceFrom(tmp_prev);
                }
            }
            //calculate relative image size
            int wraperH = pictureBox1.Height;
            int wraperW = pictureBox1.Width;

            Bitmap bmp = (Bitmap)pictureBox1.Image;
            int realImgH = bmp.Height;
            int realImgW = bmp.Width;

            MousePoint relative = calculate(wraperH, wraperW, realImgH, realImgW);
            //
            float tile = (float)realImgH / relative.getY();
            total = (int)(total * tile);//realIMG => distance
            MousePoint relative2 = calculate(2400, 3000, realImgH, realImgW);
            float tile2 = (float)realImgH / relative2.getY();
            total = (int)(total / tile2);//relative => distance
            //display on view
            label_totalDistance.Text = total.ToString();
        }
        private MousePoint calculate(int wrapperH, int wrapperW, int realImgH, int realImgW)
        {
            MousePoint re = new MousePoint(0,0);

            int relativeImgH = 0;
            int relativeImgW = 0;
            float wraperTL = (float)wrapperH / wrapperW;
            float realImgTL = (float)realImgH / realImgW;
            if (wraperTL >= realImgTL)
            {
                relativeImgW = wrapperW;
                relativeImgH = (int)(relativeImgW * realImgTL);
            }
            else
            {
                relativeImgH = wrapperH;
                relativeImgW = (int)(relativeImgH / realImgTL);
            }
            re.setX(relativeImgW);
            re.setY(relativeImgH);
            return re;
        }

        private void button_clearLog_Click(object sender, EventArgs e)
        {
            list.Clear();
            label_totalDistance.Text = "[not set]";
            richTextBox_log.Clear();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }
        private MousePoint drag_begin = new MousePoint(0,0);
        private MousePoint drag_end = new MousePoint(0, 0);
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            drag_begin.setX(e.Location.X);
            drag_begin.setY(e.Location.Y);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drag_end.setX(e.Location.X);
            drag_end.setY(e.Location.Y);
            //calculate drag
            /*
            int XMove = drag_end.getX()-drag_begin.getX();
            int YMove = drag_end.getY() - drag_begin.getY();
            int maxH = panel1.HorizontalScroll.Maximum;
            int minH = panel1.HorizontalScroll.Minimum;
            int maxV = panel1.VerticalScroll.Maximum;
            int minV = panel1.VerticalScroll.Minimum;
            //to Right
            if(XMove>0)
            {
                if(YMove>0)
                {
                    //to Down
                    if(panel1.HorizontalScroll.Value-XMove>=minH)
                    {
                        panel1.HorizontalScroll.Value -= XMove;
                    }
                    if(panel1.VerticalScroll.Value-YMove>=minV)
                    {
                        panel1.VerticalScroll.Value -= YMove;
                    }
                }
                else
                {
                    YMove = -YMove;
                    //to Up
                    if (panel1.HorizontalScroll.Value - XMove >= minH)
                    {
                        panel1.HorizontalScroll.Value -= XMove;
                    }
                    if (panel1.VerticalScroll.Value + YMove <= minV)
                    {
                        panel1.VerticalScroll.Value += YMove;
                    }
                }
            }
            //to Left
            else
            {
                XMove = -XMove;
                if (YMove > 0)
                {
                    //to Down
                    if (panel1.HorizontalScroll.Value + XMove <= minH)
                    {
                        panel1.HorizontalScroll.Value += XMove;
                    }
                    if (panel1.VerticalScroll.Value - YMove >= minV)
                    {
                        panel1.VerticalScroll.Value -= YMove;
                    }
                }
                else
                {
                    YMove = -YMove;
                    //to Up
                    if (panel1.HorizontalScroll.Value + XMove <= minH)
                    {
                        panel1.HorizontalScroll.Value += XMove;
                    }
                    if (panel1.VerticalScroll.Value + YMove <= minV)
                    {
                        panel1.VerticalScroll.Value += YMove;
                    }
                }
            }
             * */

        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            //resize picturebox too
            pictureBox1.Width = panel1.Width;
            pictureBox1.Height = panel1.Height;
            Console.WriteLine(panel1.Width + "-"+panel1.Height);
        }
    }
}

