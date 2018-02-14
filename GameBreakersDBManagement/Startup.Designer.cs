namespace GameBreakersDBManagement
{
    partial class Startup
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.magicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cardboardConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.magicToolStripMenuItem,
            this.cardboardConnectionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1187, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // magicToolStripMenuItem
            // 
            this.magicToolStripMenuItem.Name = "magicToolStripMenuItem";
            this.magicToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.magicToolStripMenuItem.Text = "Magic";
            this.magicToolStripMenuItem.Click += new System.EventHandler(this.magicToolStripMenuItem_Click);
            // 
            // cardboardConnectionToolStripMenuItem
            // 
            this.cardboardConnectionToolStripMenuItem.Name = "cardboardConnectionToolStripMenuItem";
            this.cardboardConnectionToolStripMenuItem.Size = new System.Drawing.Size(140, 20);
            this.cardboardConnectionToolStripMenuItem.Text = "Cardboard Connection";
            this.cardboardConnectionToolStripMenuItem.Click += new System.EventHandler(this.cardboardConnectionToolStripMenuItem_Click_1);
            // 
            // Startup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1187, 628);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.Name = "Startup";
            this.Text = "Startup";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem magicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cardboardConnectionToolStripMenuItem;
    }
}