namespace GameBreakersDBManagement
{
    partial class CardboardConnection
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView_CardData = new System.Windows.Forms.DataGridView();
            this.Year = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Brand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sport = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlayerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Team = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrintRun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Odds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Inventory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CardID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button_Set = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_Set = new System.Windows.Forms.TextBox();
            this.button_Number = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Number = new System.Windows.Forms.TextBox();
            this.button_Team = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Team = new System.Windows.Forms.TextBox();
            this.button_Name = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Name = new System.Windows.Forms.TextBox();
            this.button_NewCart = new System.Windows.Forms.Button();
            this.button_OpenCart = new System.Windows.Forms.Button();
            this.dataGridView_Carts = new System.Windows.Forms.DataGridView();
            this.CartID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_AddToCart = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CardData)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Carts)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dataGridView_CardData);
            this.groupBox2.Location = new System.Drawing.Point(12, 226);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1044, 261);
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
            this.Year,
            this.Brand,
            this.Sport,
            this.Category,
            this.Number,
            this.PlayerName,
            this.Team,
            this.PrintRun,
            this.Odds,
            this.Inventory,
            this.ExtraData,
            this.CardID});
            this.dataGridView_CardData.Location = new System.Drawing.Point(6, 20);
            this.dataGridView_CardData.Name = "dataGridView_CardData";
            this.dataGridView_CardData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_CardData.Size = new System.Drawing.Size(1032, 231);
            this.dataGridView_CardData.TabIndex = 12;
            this.dataGridView_CardData.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.InventoryChanged);
            // 
            // Year
            // 
            this.Year.HeaderText = "Year";
            this.Year.Name = "Year";
            this.Year.ReadOnly = true;
            // 
            // Brand
            // 
            this.Brand.HeaderText = "Brand";
            this.Brand.Name = "Brand";
            this.Brand.ReadOnly = true;
            // 
            // Sport
            // 
            this.Sport.HeaderText = "Sport";
            this.Sport.Name = "Sport";
            this.Sport.ReadOnly = true;
            // 
            // Category
            // 
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            // 
            // Number
            // 
            this.Number.HeaderText = "Number";
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            // 
            // PlayerName
            // 
            this.PlayerName.HeaderText = "Name";
            this.PlayerName.Name = "PlayerName";
            this.PlayerName.ReadOnly = true;
            // 
            // Team
            // 
            this.Team.HeaderText = "Team";
            this.Team.Name = "Team";
            this.Team.ReadOnly = true;
            // 
            // PrintRun
            // 
            this.PrintRun.HeaderText = "Print Run";
            this.PrintRun.Name = "PrintRun";
            this.PrintRun.ReadOnly = true;
            // 
            // Odds
            // 
            this.Odds.HeaderText = "Odds";
            this.Odds.Name = "Odds";
            this.Odds.ReadOnly = true;
            // 
            // Inventory
            // 
            this.Inventory.HeaderText = "Inventory";
            this.Inventory.Name = "Inventory";
            // 
            // ExtraData
            // 
            this.ExtraData.HeaderText = "Extra Data";
            this.ExtraData.Name = "ExtraData";
            // 
            // CardID
            // 
            this.CardID.HeaderText = "CardID";
            this.CardID.Name = "CardID";
            this.CardID.Visible = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.button_Set);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.textBox_Set);
            this.groupBox4.Controls.Add(this.button_Number);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.textBox_Number);
            this.groupBox4.Controls.Add(this.button_Team);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.textBox_Team);
            this.groupBox4.Controls.Add(this.button_Name);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.textBox_Name);
            this.groupBox4.Location = new System.Drawing.Point(13, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1043, 208);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Search";
            // 
            // button_Set
            // 
            this.button_Set.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Set.Location = new System.Drawing.Point(917, 157);
            this.button_Set.Name = "button_Set";
            this.button_Set.Size = new System.Drawing.Size(120, 41);
            this.button_Set.TabIndex = 11;
            this.button_Set.Text = "Search Set";
            this.button_Set.UseVisualStyleBackColor = true;
            this.button_Set.Click += new System.EventHandler(this.button_Set_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "By Expansion:";
            // 
            // textBox_Set
            // 
            this.textBox_Set.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Set.Location = new System.Drawing.Point(6, 173);
            this.textBox_Set.Name = "textBox_Set";
            this.textBox_Set.Size = new System.Drawing.Size(905, 20);
            this.textBox_Set.TabIndex = 9;
            this.textBox_Set.Enter += new System.EventHandler(this.textBox_Set_Enter);
            // 
            // button_Number
            // 
            this.button_Number.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Number.Location = new System.Drawing.Point(917, 110);
            this.button_Number.Name = "button_Number";
            this.button_Number.Size = new System.Drawing.Size(120, 41);
            this.button_Number.TabIndex = 8;
            this.button_Number.Text = "Search Number";
            this.button_Number.UseVisualStyleBackColor = true;
            this.button_Number.Click += new System.EventHandler(this.button_Number_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "By Card Number:";
            // 
            // textBox_Number
            // 
            this.textBox_Number.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Number.Location = new System.Drawing.Point(6, 126);
            this.textBox_Number.Name = "textBox_Number";
            this.textBox_Number.Size = new System.Drawing.Size(905, 20);
            this.textBox_Number.TabIndex = 6;
            this.textBox_Number.Enter += new System.EventHandler(this.textBox_Number_Enter);
            // 
            // button_Team
            // 
            this.button_Team.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Team.Location = new System.Drawing.Point(917, 63);
            this.button_Team.Name = "button_Team";
            this.button_Team.Size = new System.Drawing.Size(120, 41);
            this.button_Team.TabIndex = 5;
            this.button_Team.Text = "Search Team";
            this.button_Team.UseVisualStyleBackColor = true;
            this.button_Team.Click += new System.EventHandler(this.button_Team_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "By Team:";
            // 
            // textBox_Team
            // 
            this.textBox_Team.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Team.Location = new System.Drawing.Point(6, 79);
            this.textBox_Team.Name = "textBox_Team";
            this.textBox_Team.Size = new System.Drawing.Size(905, 20);
            this.textBox_Team.TabIndex = 3;
            this.textBox_Team.Enter += new System.EventHandler(this.textBox_Team_Enter);
            // 
            // button_Name
            // 
            this.button_Name.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Name.Location = new System.Drawing.Point(917, 16);
            this.button_Name.Name = "button_Name";
            this.button_Name.Size = new System.Drawing.Size(120, 41);
            this.button_Name.TabIndex = 2;
            this.button_Name.Text = "Search Name";
            this.button_Name.UseVisualStyleBackColor = true;
            this.button_Name.Click += new System.EventHandler(this.button_Name_Click);
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
            this.textBox_Name.Size = new System.Drawing.Size(905, 20);
            this.textBox_Name.TabIndex = 0;
            this.textBox_Name.Enter += new System.EventHandler(this.textBox_Name_Enter);
            // 
            // button_NewCart
            // 
            this.button_NewCart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_NewCart.Location = new System.Drawing.Point(268, 389);
            this.button_NewCart.Name = "button_NewCart";
            this.button_NewCart.Size = new System.Drawing.Size(80, 79);
            this.button_NewCart.TabIndex = 29;
            this.button_NewCart.Text = "New Cart";
            this.button_NewCart.UseVisualStyleBackColor = true;
            this.button_NewCart.Click += new System.EventHandler(this.button_NewCart_Click);
            // 
            // button_OpenCart
            // 
            this.button_OpenCart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_OpenCart.Location = new System.Drawing.Point(182, 390);
            this.button_OpenCart.Name = "button_OpenCart";
            this.button_OpenCart.Size = new System.Drawing.Size(80, 79);
            this.button_OpenCart.TabIndex = 27;
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
            this.CustomerName});
            this.dataGridView_Carts.Location = new System.Drawing.Point(6, 19);
            this.dataGridView_Carts.MultiSelect = false;
            this.dataGridView_Carts.Name = "dataGridView_Carts";
            this.dataGridView_Carts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_Carts.Size = new System.Drawing.Size(342, 365);
            this.dataGridView_Carts.TabIndex = 26;
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
            // button_AddToCart
            // 
            this.button_AddToCart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_AddToCart.Location = new System.Drawing.Point(6, 389);
            this.button_AddToCart.Name = "button_AddToCart";
            this.button_AddToCart.Size = new System.Drawing.Size(80, 80);
            this.button_AddToCart.TabIndex = 25;
            this.button_AddToCart.Text = "Add To Cart";
            this.button_AddToCart.UseVisualStyleBackColor = true;
            this.button_AddToCart.Click += new System.EventHandler(this.button_AddToCart_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dataGridView_Carts);
            this.groupBox1.Controls.Add(this.button_AddToCart);
            this.groupBox1.Controls.Add(this.button_OpenCart);
            this.groupBox1.Controls.Add(this.button_NewCart);
            this.groupBox1.Location = new System.Drawing.Point(1062, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(351, 475);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Carts";
            // 
            // CardboardConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1425, 492);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Name = "CardboardConnection";
            this.Text = "GameBreakers CC Inventory";
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CardData)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Carts)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridView_CardData;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button_Set;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_Set;
        private System.Windows.Forms.Button button_Number;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_Number;
        private System.Windows.Forms.Button button_Team;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Team;
        private System.Windows.Forms.Button button_Name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Name;
        private System.Windows.Forms.Button button_NewCart;
        private System.Windows.Forms.Button button_OpenCart;
        private System.Windows.Forms.DataGridView dataGridView_Carts;
        private System.Windows.Forms.DataGridViewTextBoxColumn CartID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerName;
        private System.Windows.Forms.Button button_AddToCart;
        private System.Windows.Forms.DataGridViewTextBoxColumn Year;
        private System.Windows.Forms.DataGridViewTextBoxColumn Brand;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sport;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlayerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Team;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrintRun;
        private System.Windows.Forms.DataGridViewTextBoxColumn Odds;
        private System.Windows.Forms.DataGridViewTextBoxColumn Inventory;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraData;
        private System.Windows.Forms.DataGridViewTextBoxColumn CardID;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

