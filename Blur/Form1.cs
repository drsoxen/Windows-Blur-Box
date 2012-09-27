using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ZUtilities;
namespace Blur
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);

        int blurAmount;

        public Form1()
        {
            InitializeComponent();
            this.TopMost = true;

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;

            this.pictureBox1.Location = new Point(0, 0);
            this.pictureBox1.Width = this.Width;
            this.pictureBox1.Height = this.Height;

            this.button1.Visible = true;
            this.button2.Visible = true;

            blurAmount = 10;

            takeScreenShot();

            //SetWindowLong(this.Handle, -20, 0x80000 | 0x20);
            //SetLayeredWindowAttributes(this.Handle, 0, 100, 0x2);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
            {
                this.FormBorderStyle = this.FormBorderStyle == System.Windows.Forms.FormBorderStyle.Sizable ? System.Windows.Forms.FormBorderStyle.None : System.Windows.Forms.FormBorderStyle.Sizable;
                this.button1.Visible = this.button1.Visible ? false : true;
                this.button2.Visible = this.button2.Visible ? false : true;
            }
            
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        Bitmap GetScreenBitmap(int GrabWidth, int GrabHeight, int X, int Y)
        {
            Bitmap bmpScreenShot = new Bitmap(GrabWidth, GrabHeight);
            Graphics gfx = Graphics.FromImage((Image)bmpScreenShot);
            gfx.CopyFromScreen(X, Y, 0, 0, new Size(GrabWidth, GrabHeight));

            return bmpScreenShot;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            takeScreenShot();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            this.pictureBox1.Width = this.Width;
            this.pictureBox1.Height = this.Height;
            takeScreenShot();            
        }

        void takeScreenShot()
        {
            this.Opacity = 0;
            this.Refresh();
            Bitmap temp = GetScreenBitmap(this.Width, this.Height, this.Location.X, this.Location.Y);
            this.Opacity = 100;
            this.Refresh();
            for (int i = 0; i < blurAmount; i++)
                BitmapFilter.GaussianBlur(temp, 4);
            this.pictureBox1.Image = temp;
            this.pictureBox1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            blurAmount+=4;
            takeScreenShot();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (blurAmount > 0)
            {
                blurAmount -= 4;
                takeScreenShot();
            }
        }

        private void button2_KeyDown(object sender, KeyEventArgs e)
        {
            Form1_KeyDown(sender, e);
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            Form1_KeyDown(sender, e);

        }


    }
}
