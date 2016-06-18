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

namespace Colorpicker
{
    
    public partial class ColorPickerForm : Form
    {
        public ColorPickerForm()
        {
            InitializeComponent();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ColorPickerForm_Load(object sender, EventArgs e)
        {
            this.Left = Properties.Settings.Default.PosLeft;
            this.Top = Properties.Settings.Default.PosTop;
            Color color = System.Drawing.ColorTranslator.FromHtml(Properties.Settings.Default.Selector);
            this.selector = color;
            this.TopMost = Properties.Settings.Default.TopMost;

            this.ShowInTaskbar = false;
            this.ShowInTaskbar = true;

            this.Height = 80;
            this.Width = 100;

            this.TransparencyKey = Color.LimeGreen;           
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void ColorPickerForm_MouseDown(object sender, MouseEventArgs e)
        {
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

        private void ColorPickerForm_Move(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        Color color;
        Color selector;

        private void ColorPickerForm_Paint(object sender, PaintEventArgs e)
        {
            int t = this.Top;
            int l = this.Left;
            int w = this.Width;


            // top space
            System.Drawing.SolidBrush myBrushTop = new System.Drawing.SolidBrush(
                this.TransparencyKey
            );
            e.Graphics.FillRectangle(myBrushTop, new Rectangle(0, 0, 100, 50));

            //botom space
            this.color = GetColorAt(l+50, t-1);
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(
                 this.color
            );
            e.Graphics.FillRectangle(myBrush, new Rectangle(0, 50, 100, 30));


            System.Drawing.Pen myPen = new System.Drawing.Pen(this.selector);
            e.Graphics.DrawLine(myPen, 50, 0, 50, 50);
            e.Graphics.DrawLine(myPen, 0, 50, 100, 50);
            e.Graphics.DrawLine(myPen, 0, 50, 0, 80);
            e.Graphics.DrawLine(myPen, 0, 79, 30, 79);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(
                System.Drawing.ColorTranslator.ToHtml(this.color)
            );
        }

        private void ColorPickerForm_KeyDown(object sender, KeyEventArgs e)
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

        private void ColorPickerForm_FormClosing(object sender, FormClosingEventArgs e)
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

        private void ColorPickerForm_Activated(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.color != GetColorAt(this.Left + 50, this.Top - 1)) {
                this.Invalidate();
            }
        }
    }
}
