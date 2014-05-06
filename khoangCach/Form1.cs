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
            //display on view
            label_totalDistance.Text = total.ToString();
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
    }
}
