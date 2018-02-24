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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dataGridView_SearchParams = new System.Windows.Forms.DataGridView();
            this.comboBox_InclusionType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox_ParamType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button_AddParam = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_SearchParameter = new System.Windows.Forms.TextBox();
            this.button_NewCart = new System.Windows.Forms.Button();
            this.button_OpenCart = new System.Windows.Forms.Button();
            this.dataGridView_Carts = new System.Windows.Forms.DataGridView();
            this.button_AddToCart = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Search = new System.Windows.Forms.Button();
            this.comboBox_SearchType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Parameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Inclusion = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.SearchType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Year = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Brand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sport = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlayerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Team = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrintRun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Odds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Inventory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CardID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CartID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Items = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_Remove = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CardData)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_SearchParams)).BeginInit();
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
            this.Subset,
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
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.button_Remove);
            this.groupBox4.Controls.Add(this.comboBox_SearchType);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.button_Search);
            this.groupBox4.Controls.Add(this.dataGridView_SearchParams);
            this.groupBox4.Controls.Add(this.comboBox_InclusionType);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.comboBox_ParamType);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.button_AddParam);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.textBox_SearchParameter);
            this.groupBox4.Location = new System.Drawing.Point(13, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1043, 208);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Search";
            // 
            // dataGridView_SearchParams
            // 
            this.dataGridView_SearchParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_SearchParams.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Parameter,
            this.Type,
            this.Inclusion,
            this.SearchType});
            this.dataGridView_SearchParams.Location = new System.Drawing.Point(6, 59);
            this.dataGridView_SearchParams.Name = "dataGridView_SearchParams";
            this.dataGridView_SearchParams.Size = new System.Drawing.Size(456, 143);
            this.dataGridView_SearchParams.TabIndex = 17;
            // 
            // comboBox_InclusionType
            // 
            this.comboBox_InclusionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_InclusionType.FormattingEnabled = true;
            this.comboBox_InclusionType.Items.AddRange(new object[] {
            "And",
            "Not"});
            this.comboBox_InclusionType.Location = new System.Drawing.Point(341, 32);
            this.comboBox_InclusionType.Name = "comboBox_InclusionType";
            this.comboBox_InclusionType.Size = new System.Drawing.Size(121, 21);
            this.comboBox_InclusionType.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(338, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Inclusion Type";
            // 
            // comboBox_ParamType
            // 
            this.comboBox_ParamType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ParamType.FormattingEnabled = true;
            this.comboBox_ParamType.Items.AddRange(new object[] {
            "Expansion",
            "Subset",
            "Year",
            "Sport",
            "Brand",
            "Card Number",
            "Player Name",
            "Team",
            "Print Run",
            "Odds",
            "Other"});
            this.comboBox_ParamType.Location = new System.Drawing.Point(214, 32);
            this.comboBox_ParamType.Name = "comboBox_ParamType";
            this.comboBox_ParamType.Size = new System.Drawing.Size(121, 21);
            this.comboBox_ParamType.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(211, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Parameter Type";
            // 
            // button_AddParam
            // 
            this.button_AddParam.Location = new System.Drawing.Point(468, 59);
            this.button_AddParam.Name = "button_AddParam";
            this.button_AddParam.Size = new System.Drawing.Size(121, 23);
            this.button_AddParam.TabIndex = 2;
            this.button_AddParam.Text = "Add";
            this.button_AddParam.UseVisualStyleBackColor = true;
            this.button_AddParam.Click += new System.EventHandler(this.button_AddParam_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search Parameter";
            // 
            // textBox_SearchParameter
            // 
            this.textBox_SearchParameter.Location = new System.Drawing.Point(6, 33);
            this.textBox_SearchParameter.Name = "textBox_SearchParameter";
            this.textBox_SearchParameter.Size = new System.Drawing.Size(202, 20);
            this.textBox_SearchParameter.TabIndex = 0;
            this.textBox_SearchParameter.Enter += new System.EventHandler(this.textBox_SearchParameter_Enter);
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
            this.CustomerName,
            this.Items});
            this.dataGridView_Carts.Location = new System.Drawing.Point(6, 19);
            this.dataGridView_Carts.MultiSelect = false;
            this.dataGridView_Carts.Name = "dataGridView_Carts";
            this.dataGridView_Carts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_Carts.Size = new System.Drawing.Size(342, 365);
            this.dataGridView_Carts.TabIndex = 26;
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
            // button_Search
            // 
            this.button_Search.Location = new System.Drawing.Point(468, 179);
            this.button_Search.Name = "button_Search";
            this.button_Search.Size = new System.Drawing.Size(121, 23);
            this.button_Search.TabIndex = 18;
            this.button_Search.Text = "Search";
            this.button_Search.UseVisualStyleBackColor = true;
            this.button_Search.Click += new System.EventHandler(this.button_Search_Click);
            // 
            // comboBox_SearchType
            // 
            this.comboBox_SearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_SearchType.FormattingEnabled = true;
            this.comboBox_SearchType.Items.AddRange(new object[] {
            "Exact",
            "Partial"});
            this.comboBox_SearchType.Location = new System.Drawing.Point(468, 32);
            this.comboBox_SearchType.Name = "comboBox_SearchType";
            this.comboBox_SearchType.Size = new System.Drawing.Size(121, 21);
            this.comboBox_SearchType.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(465, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Search Type";
            // 
            // Parameter
            // 
            this.Parameter.HeaderText = "Parameter";
            this.Parameter.Name = "Parameter";
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Items.AddRange(new object[] {
            "Expansion",
            "Subset",
            "Year",
            "Sport",
            "Brand",
            "Card Number",
            "Player Name",
            "Team",
            "Print Run",
            "Odds",
            "Other"});
            this.Type.Name = "Type";
            // 
            // Inclusion
            // 
            this.Inclusion.HeaderText = "Inclusion";
            this.Inclusion.Items.AddRange(new object[] {
            "And",
            "Not"});
            this.Inclusion.Name = "Inclusion";
            // 
            // SearchType
            // 
            this.SearchType.HeaderText = "Search Type";
            this.SearchType.Items.AddRange(new object[] {
            "Exact",
            "Partial"});
            this.SearchType.Name = "SearchType";
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
            // Subset
            // 
            this.Subset.HeaderText = "Subset";
            this.Subset.Name = "Subset";
            this.Subset.ReadOnly = true;
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
            this.ExtraData.ReadOnly = true;
            // 
            // CardID
            // 
            this.CardID.HeaderText = "CardID";
            this.CardID.Name = "CardID";
            this.CardID.ReadOnly = true;
            this.CardID.Visible = false;
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
            // button_Remove
            // 
            this.button_Remove.Location = new System.Drawing.Point(468, 88);
            this.button_Remove.Name = "button_Remove";
            this.button_Remove.Size = new System.Drawing.Size(121, 23);
            this.button_Remove.TabIndex = 21;
            this.button_Remove.Text = "Remove";
            this.button_Remove.UseVisualStyleBackColor = true;
            this.button_Remove.Click += new System.EventHandler(this.button_Remove_Click);
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_SearchParams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Carts)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridView_CardData;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button_AddParam;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_SearchParameter;
        private System.Windows.Forms.Button button_NewCart;
        private System.Windows.Forms.Button button_OpenCart;
        private System.Windows.Forms.DataGridView dataGridView_Carts;
        private System.Windows.Forms.Button button_AddToCart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox_InclusionType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox_ParamType;
        private System.Windows.Forms.DataGridView dataGridView_SearchParams;
        private System.Windows.Forms.Button button_Search;
        private System.Windows.Forms.ComboBox comboBox_SearchType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Parameter;
        private System.Windows.Forms.DataGridViewComboBoxColumn Type;
        private System.Windows.Forms.DataGridViewComboBoxColumn Inclusion;
        private System.Windows.Forms.DataGridViewComboBoxColumn SearchType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Year;
        private System.Windows.Forms.DataGridViewTextBoxColumn Brand;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sport;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subset;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlayerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Team;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrintRun;
        private System.Windows.Forms.DataGridViewTextBoxColumn Odds;
        private System.Windows.Forms.DataGridViewTextBoxColumn Inventory;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraData;
        private System.Windows.Forms.DataGridViewTextBoxColumn CardID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CartID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Items;
        private System.Windows.Forms.Button button_Remove;
    }
}

