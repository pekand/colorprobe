using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColorProbe
{
    
    public partial class ColorProbeForm : Form
    {
        public ColorProbeForm()
        {
            InitializeComponent();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ColorProbeForm_Load(object sender, EventArgs e)
        {
            this.Left = Properties.Settings.Default.PosLeft;
            this.Top = Properties.Settings.Default.PosTop;
            Color color = System.Drawing.ColorTranslator.FromHtml(Properties.Settings.Default.Selector);
            this.selector = color;
            this.TopMost = Properties.Settings.Default.TopMost;

            this.ShowInTaskbar = false;
            this.ShowInTaskbar = true;

            this.Height = 100;
            this.Width = 100;

            TransparencyKey = BackColor = Color.LavenderBlush;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void ColorProbeForm_MouseDown(object sender, MouseEventArgs e)
        {
            this.cicle = 0;

            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }

            if (e.Button == MouseButtons.Right)
            {
                Point pt = this.PointToScreen(e.Location);
                contextMenuStrip1.Show(pt);
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowDC(IntPtr window);
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern uint GetPixel(IntPtr dc, int x, int y);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ReleaseDC(IntPtr window, IntPtr dc);

        public Color GetColorAt(int x, int y)
        {
            IntPtr desk = GetDesktopWindow();
            IntPtr dc = GetWindowDC(desk);
            int a = (int)GetPixel(dc, x, y);
            ReleaseDC(desk, dc);
            return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
        }

        private void ColorProbeForm_Move(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        Color color;
        Color selector;

        private void ColorProbeForm_Paint(object sender, PaintEventArgs e)
        {
            int t = this.Top;
            int l = this.Left;
            int w = this.Width;

            Graphics g = e.Graphics;

            // background
            SolidBrush myBrushTop = new SolidBrush(
                this.TransparencyKey
            );
            g.FillRectangle(myBrushTop, new Rectangle(0, 0, this.Width, this.Height));

            //botom space
            this.color = GetColorAt(l+50, t-1);
            SolidBrush myBrush = new SolidBrush(
                 this.color
            );
            g.FillRectangle(myBrush, new Rectangle(0, 50, 100, 30));


            Pen myPen = new Pen(this.selector);
            g.DrawLine(myPen, 50, 0, 50, 50);//picker
            g.DrawLine(myPen, 0, 50, 100, 50); //long
            g.DrawLine(myPen, 0, 50, 0, 79);// most left
            g.DrawLine(myPen, 0, 79, 30, 79);// short bottom

            

            if (cicle > 0) {
                int invers = (this.cicleMax - cicle);

                SolidBrush animationBrush = new SolidBrush(
                    this.selector
                );

                double angle = 2 * Math.PI * (double)((100.0 - cicle)/100);
                int x = (int)(50.0 + 40.0 * Math.Cos(angle));
                int y = (int)(50.0 + 40.0 * Math.Sin(angle));

                Pen myPen2 = new Pen(this.selector,1);
                g.FillEllipse(animationBrush, x, y, 5, 5);


                angle = 2 * Math.PI * (double)((100.0 - cicle) / 100) + Math.PI * 0.5;
                x = (int)(50.0 + 30.0 * Math.Cos(angle));
                y = (int)(50.0 + 30.0 * Math.Sin(angle));

                g.FillEllipse(animationBrush, x, y, 7, 7);

                angle = 2 * Math.PI * (double)((100.0 - cicle) / 100) + Math.PI;
                x = (int)(50.0 + 25.0 * Math.Cos(angle));
                y = (int)(50.0 + 25.0 * Math.Sin(angle));

                g.FillEllipse(animationBrush, x, y, 5, 5);

            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(
                System.Drawing.ColorTranslator.ToHtml(this.color)
            );
        }

        private void ColorProbeForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10 || e.KeyCode == Keys.Escape)
            {
                this.Close();
            }


            int add = 1;
            if (e.Alt)
                add = 20;

            if (e.Control || (e.Control && e.Alt))
            {
                if (e.KeyCode == Keys.Left)
                {
                    this.Width = this.Width - add;
                }
                if (e.KeyCode == Keys.Right)
                {
                    this.Width = this.Width + add;
                }
                if (e.KeyCode == Keys.Up)
                {
                    this.Height = this.Height - add;
                }
                if (e.KeyCode == Keys.Down)
                {
                    this.Height = this.Height + add;
                }
            }
            else
            {
                if (e.KeyCode == Keys.Left)
                {
                    this.Left = this.Left - add;
                }
                if (e.KeyCode == Keys.Right)
                {
                    this.Left = this.Left + add;
                }
                if (e.KeyCode == Keys.Up)
                {
                    this.Top = this.Top - add;
                }
                if (e.KeyCode == Keys.Down)
                {
                    this.Top = this.Top + add;
                }
            }
        }

        private void pickerColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.selector = colorDialog1.Color;

                if (this.TransparencyKey == this.selector) {
                    this.TransparencyKey = Color.FromArgb(this.selector.ToArgb() ^ 0xffffff);
                }

                this.Invalidate();
            }
        }

        private void ColorProbeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.PosLeft = this.Left;
            Properties.Settings.Default.PosTop = this.Top;
            string color = System.Drawing.ColorTranslator.ToHtml(this.selector);
            Properties.Settings.Default.Selector = color;
            Properties.Settings.Default.TopMost = this.TopMost;
            Properties.Settings.Default.Save();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            this.copyToolStripMenuItem.Text = System.Drawing.ColorTranslator.ToHtml(this.color);
            topToolStripMenuItem.Checked = this.TopMost;
        }

        private void topToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
            topToolStripMenuItem.Checked = this.TopMost;
        }

        private void ColorProbeForm_Activated(object sender, EventArgs e)
        {
            if (cicle == 0)
            {
                cicle = cicleMax; // start animation
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.color != GetColorAt(this.Left + 50, this.Top - 1)) {
                this.Invalidate();
            }
        }

        int cicle = 0;
        int cicleMax = 100;

        private void timerAnimation_Tick(object sender, EventArgs e)
        {
            if (cicle > 0) {
                --cicle;
                this.Invalidate();
            }
        }
    }
}
