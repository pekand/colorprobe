namespace ColorProbe
{
    partial class ColorProbeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorProbeForm));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyRGBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyHSLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pickerColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.timerLive = new System.Windows.Forms.Timer(this.components);
            this.timerAnimation = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.copyRGBToolStripMenuItem,
            this.copyHSLToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 136);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // copyRGBToolStripMenuItem
            // 
            this.copyRGBToolStripMenuItem.Name = "copyRGBToolStripMenuItem";
            this.copyRGBToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyRGBToolStripMenuItem.Text = "Copy RGB";
            this.copyRGBToolStripMenuItem.Click += new System.EventHandler(this.copyRGBToolStripMenuItem_Click);
            // 
            // copyHSLToolStripMenuItem
            // 
            this.copyHSLToolStripMenuItem.Name = "copyHSLToolStripMenuItem";
            this.copyHSLToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyHSLToolStripMenuItem.Text = "Copy HSL";
            this.copyHSLToolStripMenuItem.Click += new System.EventHandler(this.copyHSLToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pickerColorToolStripMenuItem,
            this.topToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // pickerColorToolStripMenuItem
            // 
            this.pickerColorToolStripMenuItem.Name = "pickerColorToolStripMenuItem";
            this.pickerColorToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.pickerColorToolStripMenuItem.Text = "Picker color";
            this.pickerColorToolStripMenuItem.Click += new System.EventHandler(this.pickerColorToolStripMenuItem_Click);
            // 
            // topToolStripMenuItem
            // 
            this.topToolStripMenuItem.Name = "topToolStripMenuItem";
            this.topToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.topToolStripMenuItem.Text = "Top";
            this.topToolStripMenuItem.Click += new System.EventHandler(this.topToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // timerLive
            // 
            this.timerLive.Enabled = true;
            this.timerLive.Interval = 50;
            this.timerLive.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timerAnimation
            // 
            this.timerAnimation.Enabled = true;
            this.timerAnimation.Interval = 5;
            this.timerAnimation.Tick += new System.EventHandler(this.timerAnimation_Tick);
            // 
            // ColorProbeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(100, 100);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorProbeForm";
            this.Text = "ColorProbe";
            this.Activated += new System.EventHandler(this.ColorProbeForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ColorProbeForm_FormClosing);
            this.Load += new System.EventHandler(this.ColorProbeForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ColorProbeForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ColorProbeForm_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ColorProbeForm_MouseDown);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ColorProbeForm_MouseWheel);
            this.Move += new System.EventHandler(this.ColorProbeForm_Move);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pickerColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem topToolStripMenuItem;
        private System.Windows.Forms.Timer timerLive;
        private System.Windows.Forms.Timer timerAnimation;
        private System.Windows.Forms.ToolStripMenuItem copyRGBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyHSLToolStripMenuItem;
    }
}

