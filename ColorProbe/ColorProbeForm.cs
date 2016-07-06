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

        // constructor inicialize components
        public ColorProbeForm()
        {
            InitializeComponent();
        }

        // event load form
        private void ColorProbeForm_Load(object sender, EventArgs e)
        {
            
            // restore previousposition
            this.Left = Properties.Settings.Default.PosLeft;
            this.Top = Properties.Settings.Default.PosTop;
            this.TopMost = Properties.Settings.Default.TopMost;
            this.orientation = Properties.Settings.Default.Oreintation;

            if (!this.IsOnScreen(this)) {
                this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
                this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
                this.WindowState = FormWindowState.Normal;
                animationState = 100;
            }

            // restore previou color for selector
            Color color = System.Drawing.ColorTranslator.FromHtml(Properties.Settings.Default.Selector);
            this.selector = color;
            
            // default for size
            this.Height = 130;
            this.Width = 200;

            // set color for transparent background
            TransparencyKey = BackColor = Color.LavenderBlush;

            // hack for show in taskbar (default is invisible)
            this.ShowInTaskbar = false;
            this.ShowInTaskbar = true;

            // init draw constants;
            calculatePositions();

            // init pens and brusies
            initDrawingTools();
        }

        // event select form
        private void ColorProbeForm_Activated(object sender, EventArgs e)
        {
            if (animationState == 0)
            {
                animationState = animationStateMax; // start animation if not running
                timerAnimation.Enabled = true;
            }
        }

        private void ColorProbeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // save actual state of application
            Properties.Settings.Default.PosLeft = this.Left;
            Properties.Settings.Default.PosTop = this.Top;
            Properties.Settings.Default.TopMost = this.TopMost;
            Properties.Settings.Default.Oreintation = this.orientation;

            string color = System.Drawing.ColorTranslator.ToHtml(this.selector);
            Properties.Settings.Default.Selector = color;
            
            Properties.Settings.Default.Save();
        }

        // event keyboard shortcuts
        private void ColorProbeForm_KeyDown(object sender, KeyEventArgs e)
        {
            // close picker
            if (e.KeyCode == Keys.F10 || e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

            int add = 1;
            if (e.Alt) // spped up with alt
                add = 20;

            if (e.KeyCode == Keys.Left)
            {
                this.Left = this.Left - add; // move left
            }
            if (e.KeyCode == Keys.Right)
            {
                this.Left = this.Left + add; // move right
            }
            if (e.KeyCode == Keys.Up)
            {
                this.Top = this.Top - add; // move up
            }
            if (e.KeyCode == Keys.Down)
            {
                this.Top = this.Top + add; // move down
            }

            // reset position with f5
            if (e.KeyCode == Keys.F5)
            {
                this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
                this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
                this.WindowState = FormWindowState.Normal;
                animationState = 100;
            }
        }

        // event move form
        private void ColorProbeForm_Move(object sender, EventArgs e)
        {
            stopAnimation();
        }

        private void ColorProbeForm_MouseDown(object sender, MouseEventArgs e)
        {
            // stop animation
            stopAnimation();

            if (e.Button == MouseButtons.Left)
            {
                Manager.dragWindow(Handle); // drag wiht mouse anywhere
            }

            if (e.Button == MouseButtons.Right) //show popup
            {
                Point pt = this.PointToScreen(e.Location);
                contextMenuStrip1.Show(pt);
            }
        }

        int orientation = 0; // picker orientation 0 - top 1 - right 2 - bottom 3 - left

        private void ColorProbeForm_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Update the drawing based upon the mouse wheel scrolling.

            if (e.Delta < 0)
            {
                orientation += 1;
            }
            else
            {
                orientation -= 1;
            }

            if (orientation > 3) {
                orientation = 0;
            }

            if (orientation < 0)
            {
                orientation = 3;
            }

            calculatePositions();
            this.Invalidate();
        }

        Color color; // actual selected color with selector
        Color selector; // color of selector line

        int pickerLeft = 50;
        int pickerTop = 50;
        int pickerWidth = 100;
        int pickerHeight = 30;
        int selsectorLength = 50;

        int pickerMiddleX = 0;
        int pickerMiddleY = 0;

        int selectorStartX = 0;
        int selectorStartY = 0;
        int selectorEndX = 0;
        int selectorEndY = 0;

        double animation1radius = 40;
        double animation2radius = 30.0;
        double animation3radius = 25.0;

        int pickerCorrectionX = -1;
        int pickerCorrectionY = 0;

        public void calculatePositions()
        {
            pickerMiddleX = pickerLeft + pickerWidth / 2;
            pickerMiddleY = pickerTop + pickerHeight / 2;

            // top picker
            if (orientation == 0) {
                selectorStartX = pickerMiddleX;
                selectorStartY = pickerTop - selsectorLength;
                pickerCorrectionX = 0;
                pickerCorrectionY = -1;
            }

            // right picker
            if (orientation == 1)
            {
                selectorStartX = pickerLeft + pickerWidth + selsectorLength;
                selectorStartY = pickerMiddleY;
                pickerCorrectionX = 1;
                pickerCorrectionY = 0;
            }

            // bottom picker
            if (orientation == 2)
            {
                selectorStartX = pickerMiddleX;
                selectorStartY = pickerTop + pickerHeight + selsectorLength;
                pickerCorrectionX = 0;
                pickerCorrectionY = 1;
            }

            // left picker
            if (orientation == 3)
            {
                selectorStartX = pickerLeft - selsectorLength;
                selectorStartY = pickerMiddleY;
                pickerCorrectionX = -1;
                pickerCorrectionY = 0;
            }

            selectorEndX = pickerMiddleX;
            selectorEndY = pickerMiddleY;
        }

        SolidBrush animationBrush = null;
        Pen selectorPen = null;
        SolidBrush transparentBrush = null;
        SolidBrush selectedColorBrush = null;

        public void initDrawingTools()
        {
            transparentBrush = new SolidBrush(
                this.TransparencyKey
            );

            animationBrush = new SolidBrush(
                   this.selector
            );

            selectorPen = new Pen(this.selector);

            selectedColorBrush = new SolidBrush(
                 this.color
            );
        }

        // event paint
        private void ColorProbeForm_Paint(object sender, PaintEventArgs e)
        {
            int t = this.Top;
            int l = this.Left;
            int w = this.Width;

            Graphics g = e.Graphics;

            // background
            g.FillRectangle(transparentBrush, new Rectangle(0, 0, this.Width, this.Height));

            g.DrawLine(selectorPen, selectorStartX, selectorStartY, selectorEndX, selectorEndY); // picker line
            g.FillRectangle(selectedColorBrush, new Rectangle(pickerLeft, pickerTop, pickerWidth, pickerHeight)); // selected color

            // decoration

            if (orientation == 2) {
                g.DrawLine(selectorPen, pickerLeft, pickerTop + pickerHeight, pickerLeft + pickerWidth, pickerTop + pickerHeight); //long
            } else {
                g.DrawLine(selectorPen, pickerLeft, pickerTop, pickerLeft + pickerWidth, pickerTop); //long
            }

            if (orientation == 1) {
                g.DrawLine(selectorPen, pickerLeft + pickerWidth, pickerTop, pickerLeft + pickerWidth, pickerTop + pickerHeight - 1);// most left
            } else {
                g.DrawLine(selectorPen, pickerLeft, pickerTop, pickerLeft, pickerTop + pickerHeight - 1);// most left
            }

            if (orientation == 1) {
                g.DrawLine(selectorPen, pickerLeft + pickerWidth - 30, pickerTop + pickerHeight - 1, pickerLeft + pickerWidth, pickerTop + pickerHeight - 1);// short bottom
            } else if (orientation == 2) {
                g.DrawLine(selectorPen, pickerLeft, pickerTop, pickerLeft + 30, pickerTop);// short bottom
            } else {
                g.DrawLine(selectorPen, pickerLeft, pickerTop + pickerHeight - 1, pickerLeft + 30, pickerTop + pickerHeight - 1);// short bottom
            }

            // animation
            if (animationState > 0)
            {
                int invers = (this.animationStateMax - animationState);

                double animationRelative = (double)(animationStateMax - animationState) / animationStateMax;

                double angle = 2 * Math.PI * animationRelative;
                int x = (int)(pickerMiddleX + animation1radius * Math.Cos(angle));
                int y = (int)(pickerMiddleY + animation1radius * Math.Sin(angle));

                Pen myPen2 = new Pen(this.selector, 1);
                g.FillEllipse(animationBrush, x, y, 5, 5);

                angle = 2 * Math.PI * animationRelative + Math.PI * 0.5;
                x = (int)(pickerMiddleX + animation2radius * Math.Cos(angle));
                y = (int)(pickerMiddleY + animation2radius * Math.Sin(angle));

                g.FillEllipse(animationBrush, x, y, 7, 7);

                angle = 2 * Math.PI * animationRelative + Math.PI;
                x = (int)(pickerMiddleX + animation3radius * Math.Cos(angle));
                y = (int)(pickerMiddleY + animation3radius * Math.Sin(angle));

                g.FillEllipse(animationBrush, x, y, 5, 5);

            }
            else
            {
                stopAnimation();
            }
        }

        // timer change position check Redraw picker if pixel is changed
        private void timer1_Tick(object sender, EventArgs e)
        {
            // get selected color
            Color currentColor = Manager.GetColorAt(
                this.Left + selectorStartX + pickerCorrectionX, 
                this.Top + selectorStartY + pickerCorrectionY
            );

            if (this.color != currentColor)
            {
                this.color = currentColor;
                this.initDrawingTools();
                this.Invalidate();
            }
        }

        int animationState = 0; // animation progress from 100 to 0
        int animationStateMax = 100; //animation init state decreas from 100 to 0

        // timer animation 
        private void timerAnimation_Tick(object sender, EventArgs e)
        {
            if (animationState > 0)
            {
                --animationState; // tick
                this.Invalidate();
            }
        }

        // stop animation
        private void stopAnimation()
        {
            if (this.animationState > 0)
            {
                this.animationState = 0;
                this.timerAnimation.Enabled = false;
                this.Invalidate();
            }
        }

        private string getHtmlColor(Color color)
        {
            return System.Drawing.ColorTranslator.ToHtml(color);
        }

        private string getRGBlColor(Color color)
        {
            return "RGB(" +
                color.R.ToString() + "," +
                color.G.ToString() + "," +
                color.B.ToString() +
                ")";
        }

        private string getHSLColor(Color color)
        {
            return "HSL(" +
                color.GetHue().ToString() + "," +
                color.GetSaturation().ToString() + "," +
                color.GetBrightness().ToString() +
                ")";
        }

        // popup open popup event
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            this.copyToolStripMenuItem.Text = getHtmlColor(this.color);
            this.copyRGBToolStripMenuItem.Text = getRGBlColor(this.color);
            this.copyHSLToolStripMenuItem.Text = getHSLColor(this.color);
            topToolStripMenuItem.Checked = this.TopMost;
        }

        // popup copy hex color to clipboard
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(
                getHtmlColor(this.color)
            );
        }

        private void copyRGBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(getRGBlColor(this.color));
        }

        private void copyHSLToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Clipboard.SetText(getHSLColor(this.color));
        }

        // popup select color for selector
        private void pickerColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.selector = colorDialog.Color;

                if (this.TransparencyKey == this.selector)
                {
                    this.TransparencyKey = Color.FromArgb(this.selector.ToArgb() ^ 0xffffff);
                }

                this.initDrawingTools();

                this.Invalidate();
            }
        }

        // popup stay on top
        private void topToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
            topToolStripMenuItem.Checked = this.TopMost;
        }

        // popup close button action
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // check if form is on screen
        public bool IsOnScreen(Form form)
        {
            Screen[] screens = Screen.AllScreens;
            foreach (Screen screen in screens)
            {
                Point formTopLeft = new Point(form.Left, form.Top);

                if (screen.WorkingArea.Contains(formTopLeft))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
