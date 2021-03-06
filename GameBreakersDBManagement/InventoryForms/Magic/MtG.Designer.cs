﻿namespace GameBreakersDBManagement
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
            this.dataGridView_CardData = new System.Windows.Forms.DataGridView();
            this.CardName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Expansion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Rarity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Colour = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Inventory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FoilInventory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FoilPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeLastUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pictureBox_Card = new System.Windows.Forms.PictureBox();
            this.button_AddFoilToCart = new System.Windows.Forms.Button();
            this.button_NewCart = new System.Windows.Forms.Button();
            this.button_OpenCart = new System.Windows.Forms.Button();
            this.dataGridView_Carts = new System.Windows.Forms.DataGridView();
            this.CartID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Items = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_AddToCart = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CardData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Card)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Carts)).BeginInit();
            this.groupBox4.SuspendLayout();
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
            this.textBox_Name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Name.Location = new System.Drawing.Point(6, 32);
            this.textBox_Name.Name = "textBox_Name";
            this.textBox_Name.Size = new System.Drawing.Size(912, 20);
            this.textBox_Name.TabIndex = 0;
            this.textBox_Name.GotFocus += new System.EventHandler(this.textBox_Name_GotFocus);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button_SearchSet);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_Set);
            this.groupBox1.Controls.Add(this.button_Search);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_Name);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1050, 116);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search";
            // 
            // button_SearchSet
            // 
            this.button_SearchSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SearchSet.Location = new System.Drawing.Point(924, 66);
            this.button_SearchSet.Name = "button_SearchSet";
            this.button_SearchSet.Size = new System.Drawing.Size(120, 41);
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
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "By Expansion:";
            // 
            // textBox_Set
            // 
            this.textBox_Set.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Set.Location = new System.Drawing.Point(6, 79);
            this.textBox_Set.Name = "textBox_Set";
            this.textBox_Set.Size = new System.Drawing.Size(912, 20);
            this.textBox_Set.TabIndex = 3;
            this.textBox_Set.GotFocus += new System.EventHandler(this.textBox_Set_GotFocus);
            // 
            // button_Search
            // 
            this.button_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Search.Location = new System.Drawing.Point(924, 19);
            this.button_Search.Name = "button_Search";
            this.button_Search.Size = new System.Drawing.Size(120, 41);
            this.button_Search.TabIndex = 2;
            this.button_Search.Text = "Search Name";
            this.button_Search.UseVisualStyleBackColor = true;
            this.button_Search.Click += new System.EventHandler(this.button_Search_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dataGridView_CardData);
            this.groupBox2.Controls.Add(this.pictureBox_Card);
            this.groupBox2.Location = new System.Drawing.Point(12, 134);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1050, 596);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Results";
            // 
            // dataGridView_CardData
            // 
            this.dataGridView_CardData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_CardData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_CardData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CardName,
            this.Expansion,
            this.Rarity,
            this.Colour,
            this.Inventory,
            this.FoilInventory,
            this.Price,
            this.FoilPrice,
            this.TimeLastUpdated});
            this.dataGridView_CardData.Location = new System.Drawing.Point(6, 20);
            this.dataGridView_CardData.MultiSelect = false;
            this.dataGridView_CardData.Name = "dataGridView_CardData";
            this.dataGridView_CardData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_CardData.Size = new System.Drawing.Size(809, 570);
            this.dataGridView_CardData.TabIndex = 12;
            this.dataGridView_CardData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CardData_CellClick);
            this.dataGridView_CardData.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.InventoryChanged);
            this.dataGridView_CardData.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CardData_CellEnter);
            this.dataGridView_CardData.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OnDoubleClick);
            // 
            // CardName
            // 
            this.CardName.HeaderText = "Name";
            this.CardName.Name = "CardName";
            this.CardName.ReadOnly = true;
            // 
            // Expansion
            // 
            this.Expansion.HeaderText = "Expansion";
            this.Expansion.Name = "Expansion";
            this.Expansion.ReadOnly = true;
            // 
            // Rarity
            // 
            this.Rarity.HeaderText = "Rarity";
            this.Rarity.Name = "Rarity";
            this.Rarity.ReadOnly = true;
            // 
            // Colour
            // 
            this.Colour.HeaderText = "Colour";
            this.Colour.Name = "Colour";
            this.Colour.ReadOnly = true;
            // 
            // Inventory
            // 
            this.Inventory.HeaderText = "Inventory";
            this.Inventory.Name = "Inventory";
            // 
            // FoilInventory
            // 
            this.FoilInventory.HeaderText = "Foil Inventory";
            this.FoilInventory.Name = "FoilInventory";
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
            // TimeLastUpdated
            // 
            this.TimeLastUpdated.HeaderText = "TimeLastUpdated";
            this.TimeLastUpdated.Name = "TimeLastUpdated";
            this.TimeLastUpdated.ReadOnly = true;
            this.TimeLastUpdated.Visible = false;
            // 
            // pictureBox_Card
            // 
            this.pictureBox_Card.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_Card.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Card.Image")));
            this.pictureBox_Card.Location = new System.Drawing.Point(821, 20);
            this.pictureBox_Card.Name = "pictureBox_Card";
            this.pictureBox_Card.Size = new System.Drawing.Size(223, 311);
            this.pictureBox_Card.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Card.TabIndex = 2;
            this.pictureBox_Card.TabStop = false;
            // 
            // button_AddFoilToCart
            // 
            this.button_AddFoilToCart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_AddFoilToCart.Location = new System.Drawing.Point(92, 632);
            this.button_AddFoilToCart.Name = "button_AddFoilToCart";
            this.button_AddFoilToCart.Size = new System.Drawing.Size(80, 80);
            this.button_AddFoilToCart.TabIndex = 19;
            this.button_AddFoilToCart.Text = "Add Foil To Cart";
            this.button_AddFoilToCart.UseVisualStyleBackColor = true;
            this.button_AddFoilToCart.Click += new System.EventHandler(this.button_AddFoilToCart_Click);
            // 
            // button_NewCart
            // 
            this.button_NewCart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_NewCart.Location = new System.Drawing.Point(178, 633);
            this.button_NewCart.Name = "button_NewCart";
            this.button_NewCart.Size = new System.Drawing.Size(80, 79);
            this.button_NewCart.TabIndex = 18;
            this.button_NewCart.Text = "New Cart";
            this.button_NewCart.UseVisualStyleBackColor = true;
            this.button_NewCart.Click += new System.EventHandler(this.button_NewCart_Click);
            // 
            // button_OpenCart
            // 
            this.button_OpenCart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_OpenCart.Location = new System.Drawing.Point(264, 633);
            this.button_OpenCart.Name = "button_OpenCart";
            this.button_OpenCart.Size = new System.Drawing.Size(80, 79);
            this.button_OpenCart.TabIndex = 16;
            this.button_OpenCart.Text = "View Cart";
            this.button_OpenCart.UseVisualStyleBackColor = true;
            this.button_OpenCart.Click += new System.EventHandler(this.button_OpenCart_Click);
            // 
            // dataGridView_Carts
            // 
            this.dataGridView_Carts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_Carts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Carts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CartID,
            this.CustomerName,
            this.Items});
            this.dataGridView_Carts.Location = new System.Drawing.Point(6, 16);
            this.dataGridView_Carts.MultiSelect = false;
            this.dataGridView_Carts.Name = "dataGridView_Carts";
            this.dataGridView_Carts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_Carts.Size = new System.Drawing.Size(337, 611);
            this.dataGridView_Carts.TabIndex = 15;
            // 
            // CartID
            // 
            this.CartID.HeaderText = "Cart ID";
            this.CartID.Name = "CartID";
            this.CartID.ReadOnly = true;
            // 
            // CustomerName
            // 
            this.CustomerName.HeaderText = "Customer Name";
            this.CustomerName.Name = "CustomerName";
            this.CustomerName.ReadOnly = true;
            // 
            // Items
            // 
            this.Items.HeaderText = "Item Amount";
            this.Items.Name = "Items";
            this.Items.ReadOnly = true;
            // 
            // button_AddToCart
            // 
            this.button_AddToCart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_AddToCart.Location = new System.Drawing.Point(6, 632);
            this.button_AddToCart.Name = "button_AddToCart";
            this.button_AddToCart.Size = new System.Drawing.Size(80, 80);
            this.button_AddToCart.TabIndex = 14;
            this.button_AddToCart.Text = "Add To Cart";
            this.button_AddToCart.UseVisualStyleBackColor = true;
            this.button_AddToCart.Click += new System.EventHandler(this.button_AddToCart_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.button_AddFoilToCart);
            this.groupBox4.Controls.Add(this.dataGridView_Carts);
            this.groupBox4.Controls.Add(this.button_AddToCart);
            this.groupBox4.Controls.Add(this.button_NewCart);
            this.groupBox4.Controls.Add(this.button_OpenCart);
            this.groupBox4.Location = new System.Drawing.Point(1068, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(353, 718);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Carts";
            // 
            // MtG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1433, 742);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MtG";
            this.Text = "GameBreakers M:tG Inventory";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CardData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Card)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Carts)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox_Card;
        private System.Windows.Forms.TextBox textBox_Name;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_Search;
        private System.Windows.Forms.DataGridView dataGridView_CardData;
        private System.Windows.Forms.Button button_SearchSet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Set;
        private System.Windows.Forms.Button button_AddToCart;
        private System.Windows.Forms.DataGridView dataGridView_Carts;
        private System.Windows.Forms.Button button_OpenCart;
        private System.Windows.Forms.Button button_NewCart;
        private System.Windows.Forms.Button button_AddFoilToCart;
        private System.Windows.Forms.DataGridViewTextBoxColumn CardName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Expansion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rarity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Colour;
        private System.Windows.Forms.DataGridViewTextBoxColumn Inventory;
        private System.Windows.Forms.DataGridViewTextBoxColumn FoilInventory;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn FoilPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeLastUpdated;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridViewTextBoxColumn CartID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Items;
    }
}

