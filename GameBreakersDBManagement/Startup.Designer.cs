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
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.magicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cardboardConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.magicToolStripMenuItem,
            this.cardboardConnectionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1187, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // magicToolStripMenuItem
            // 
            this.magicToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inventoryToolStripMenuItem,
            this.setEditorToolStripMenuItem});
            this.magicToolStripMenuItem.Name = "magicToolStripMenuItem";
            this.magicToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.magicToolStripMenuItem.Text = "Magic";
            // 
            // cardboardConnectionToolStripMenuItem
            // 
            this.cardboardConnectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inventoryToolStripMenuItem1,
            this.addSetToolStripMenuItem,
            this.addCardToolStripMenuItem});
            this.cardboardConnectionToolStripMenuItem.Name = "cardboardConnectionToolStripMenuItem";
            this.cardboardConnectionToolStripMenuItem.Size = new System.Drawing.Size(140, 20);
            this.cardboardConnectionToolStripMenuItem.Text = "Cardboard Connection";
            // 
            // inventoryToolStripMenuItem
            // 
            this.inventoryToolStripMenuItem.Name = "inventoryToolStripMenuItem";
            this.inventoryToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.inventoryToolStripMenuItem.Text = "Inventory";
            this.inventoryToolStripMenuItem.Click += new System.EventHandler(this.inventoryToolStripMenuItem_Click);
            // 
            // setEditorToolStripMenuItem
            // 
            this.setEditorToolStripMenuItem.Name = "setEditorToolStripMenuItem";
            this.setEditorToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.setEditorToolStripMenuItem.Text = "Set Editor";
            this.setEditorToolStripMenuItem.Click += new System.EventHandler(this.setEditorToolStripMenuItem_Click);
            // 
            // inventoryToolStripMenuItem1
            // 
            this.inventoryToolStripMenuItem1.Name = "inventoryToolStripMenuItem1";
            this.inventoryToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.inventoryToolStripMenuItem1.Text = "Inventory";
            this.inventoryToolStripMenuItem1.Click += new System.EventHandler(this.inventoryToolStripMenuItem1_Click);
            // 
            // addSetToolStripMenuItem
            // 
            this.addSetToolStripMenuItem.Name = "addSetToolStripMenuItem";
            this.addSetToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addSetToolStripMenuItem.Text = "Add Set";
            this.addSetToolStripMenuItem.Click += new System.EventHandler(this.addSetToolStripMenuItem_Click);
            // 
            // addCardToolStripMenuItem
            // 
            this.addCardToolStripMenuItem.Name = "addCardToolStripMenuItem";
            this.addCardToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addCardToolStripMenuItem.Text = "Add Card";
            this.addCardToolStripMenuItem.Click += new System.EventHandler(this.addCardToolStripMenuItem_Click);
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
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventoryToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem addSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addCardToolStripMenuItem;
    }
}