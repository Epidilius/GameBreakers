namespace GameBreakersDBManagement
{
    partial class MtG
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MtG));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Name = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_SearchSet = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Set = new System.Windows.Forms.TextBox();
            this.button_Search = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_AddFoil = new System.Windows.Forms.Button();
            this.button_RemoveFoil = new System.Windows.Forms.Button();
            this.dataGridView_CardData = new System.Windows.Forms.DataGridView();
            this.CardName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Set = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Rarity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Inventory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FoilInventory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FoilPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_AddSingle = new System.Windows.Forms.Button();
            this.button_RemoveSingle = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_EditSet = new System.Windows.Forms.Button();
            this.pictureBox_Card = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CardData)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Card)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "By Name:";
            // 
            // textBox_Name
            // 
            this.textBox_Name.Location = new System.Drawing.Point(6, 32);
            this.textBox_Name.Name = "textBox_Name";
            this.textBox_Name.Size = new System.Drawing.Size(571, 20);
            this.textBox_Name.TabIndex = 0;
            this.textBox_Name.GotFocus += new System.EventHandler(this.textBox_Name_GotFocus);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_SearchSet);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_Set);
            this.groupBox1.Controls.Add(this.button_Search);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_Name);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(815, 116);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search";
            // 
            // button_SearchSet
            // 
            this.button_SearchSet.Location = new System.Drawing.Point(583, 63);
            this.button_SearchSet.Name = "button_SearchSet";
            this.button_SearchSet.Size = new System.Drawing.Size(226, 41);
            this.button_SearchSet.TabIndex = 5;
            this.button_SearchSet.Text = "Search Set";
            this.button_SearchSet.UseVisualStyleBackColor = true;
            this.button_SearchSet.Click += new System.EventHandler(this.button_SearchSet_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "By Set:";
            // 
            // textBox_Set
            // 
            this.textBox_Set.Location = new System.Drawing.Point(6, 79);
            this.textBox_Set.Name = "textBox_Set";
            this.textBox_Set.Size = new System.Drawing.Size(571, 20);
            this.textBox_Set.TabIndex = 3;
            this.textBox_Set.GotFocus += new System.EventHandler(this.textBox_Set_GotFocus);
            // 
            // button_Search
            // 
            this.button_Search.Location = new System.Drawing.Point(583, 16);
            this.button_Search.Name = "button_Search";
            this.button_Search.Size = new System.Drawing.Size(226, 41);
            this.button_Search.TabIndex = 2;
            this.button_Search.Text = "Search Name";
            this.button_Search.UseVisualStyleBackColor = true;
            this.button_Search.Click += new System.EventHandler(this.button_Search_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_AddFoil);
            this.groupBox2.Controls.Add(this.button_RemoveFoil);
            this.groupBox2.Controls.Add(this.dataGridView_CardData);
            this.groupBox2.Controls.Add(this.button_AddSingle);
            this.groupBox2.Controls.Add(this.button_RemoveSingle);
            this.groupBox2.Location = new System.Drawing.Point(12, 134);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(815, 349);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Results";
            // 
            // button_AddFoil
            // 
            this.button_AddFoil.Location = new System.Drawing.Point(408, 243);
            this.button_AddFoil.Name = "button_AddFoil";
            this.button_AddFoil.Size = new System.Drawing.Size(195, 100);
            this.button_AddFoil.TabIndex = 14;
            this.button_AddFoil.Text = "Add One Foil To Inventory";
            this.button_AddFoil.UseVisualStyleBackColor = true;
            this.button_AddFoil.Click += new System.EventHandler(this.button_AddFoil_Click);
            // 
            // button_RemoveFoil
            // 
            this.button_RemoveFoil.Location = new System.Drawing.Point(207, 243);
            this.button_RemoveFoil.Name = "button_RemoveFoil";
            this.button_RemoveFoil.Size = new System.Drawing.Size(195, 100);
            this.button_RemoveFoil.TabIndex = 13;
            this.button_RemoveFoil.Text = "Remove One Foil From Inventory";
            this.button_RemoveFoil.UseVisualStyleBackColor = true;
            this.button_RemoveFoil.Click += new System.EventHandler(this.button_RemoveFoil_Click);
            // 
            // dataGridView_CardData
            // 
            this.dataGridView_CardData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_CardData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CardName,
            this.Set,
            this.Rarity,
            this.Inventory,
            this.FoilInventory,
            this.Price,
            this.FoilPrice});
            this.dataGridView_CardData.Location = new System.Drawing.Point(6, 20);
            this.dataGridView_CardData.Name = "dataGridView_CardData";
            this.dataGridView_CardData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_CardData.Size = new System.Drawing.Size(798, 217);
            this.dataGridView_CardData.TabIndex = 12;
            this.dataGridView_CardData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CardData_CellClick);
            // 
            // CardName
            // 
            this.CardName.HeaderText = "Name";
            this.CardName.Name = "CardName";
            this.CardName.ReadOnly = true;
            // 
            // Set
            // 
            this.Set.HeaderText = "Set";
            this.Set.Name = "Set";
            this.Set.ReadOnly = true;
            // 
            // Rarity
            // 
            this.Rarity.HeaderText = "Rarity";
            this.Rarity.Name = "Rarity";
            this.Rarity.ReadOnly = true;
            // 
            // Inventory
            // 
            this.Inventory.HeaderText = "Inventory";
            this.Inventory.Name = "Inventory";
            this.Inventory.ReadOnly = true;
            // 
            // FoilInventory
            // 
            this.FoilInventory.HeaderText = "Foil Inventory";
            this.FoilInventory.Name = "FoilInventory";
            this.FoilInventory.ReadOnly = true;
            // 
            // Price
            // 
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            // 
            // FoilPrice
            // 
            this.FoilPrice.HeaderText = "Foil Price";
            this.FoilPrice.Name = "FoilPrice";
            this.FoilPrice.ReadOnly = true;
            // 
            // button_AddSingle
            // 
            this.button_AddSingle.Location = new System.Drawing.Point(609, 243);
            this.button_AddSingle.Name = "button_AddSingle";
            this.button_AddSingle.Size = new System.Drawing.Size(195, 100);
            this.button_AddSingle.TabIndex = 11;
            this.button_AddSingle.Text = "Add One To Inventory";
            this.button_AddSingle.UseVisualStyleBackColor = true;
            this.button_AddSingle.Click += new System.EventHandler(this.button_AddSingle_Click);
            // 
            // button_RemoveSingle
            // 
            this.button_RemoveSingle.Location = new System.Drawing.Point(6, 243);
            this.button_RemoveSingle.Name = "button_RemoveSingle";
            this.button_RemoveSingle.Size = new System.Drawing.Size(195, 100);
            this.button_RemoveSingle.TabIndex = 10;
            this.button_RemoveSingle.Text = "Remove One From Inventory";
            this.button_RemoveSingle.UseVisualStyleBackColor = true;
            this.button_RemoveSingle.Click += new System.EventHandler(this.button_RemoveSingle_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_EditSet);
            this.groupBox3.Location = new System.Drawing.Point(834, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(222, 154);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sets";
            // 
            // button_EditSet
            // 
            this.button_EditSet.Location = new System.Drawing.Point(6, 83);
            this.button_EditSet.Name = "button_EditSet";
            this.button_EditSet.Size = new System.Drawing.Size(210, 61);
            this.button_EditSet.TabIndex = 16;
            this.button_EditSet.Text = "Edit Set";
            this.button_EditSet.UseVisualStyleBackColor = true;
            this.button_EditSet.Click += new System.EventHandler(this.button_EditSet_Click);
            // 
            // pictureBox_Card
            // 
            this.pictureBox_Card.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Card.Image")));
            this.pictureBox_Card.Location = new System.Drawing.Point(834, 172);
            this.pictureBox_Card.Name = "pictureBox_Card";
            this.pictureBox_Card.Size = new System.Drawing.Size(223, 311);
            this.pictureBox_Card.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Card.TabIndex = 2;
            this.pictureBox_Card.TabStop = false;
            // 
            // MtG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 493);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox_Card);
            this.Name = "MtG";
            this.Text = "GameBreakers M:tG Inventory";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CardData)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Card)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox_Card;
        private System.Windows.Forms.TextBox textBox_Name;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_Search;
        private System.Windows.Forms.Button button_AddSingle;
        private System.Windows.Forms.Button button_RemoveSingle;
        private System.Windows.Forms.DataGridView dataGridView_CardData;
        private System.Windows.Forms.Button button_AddFoil;
        private System.Windows.Forms.Button button_RemoveFoil;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_EditSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn CardName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Set;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rarity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Inventory;
        private System.Windows.Forms.DataGridViewTextBoxColumn FoilInventory;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn FoilPrice;
        private System.Windows.Forms.Button button_SearchSet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Set;
    }
}

