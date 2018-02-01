namespace GameBreakersDBManagement
{
    partial class SetEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetEditorForm));
            this.dataGridView_CardData = new System.Windows.Forms.DataGridView();
            this.CardName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Set = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Rarity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Inventory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FoilInventory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_Close = new System.Windows.Forms.Button();
            this.comboBox_Sets = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox_Card = new System.Windows.Forms.PictureBox();
            this.button_LoadSet = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CardData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Card)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_CardData
            // 
            this.dataGridView_CardData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_CardData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CardName,
            this.Set,
            this.Rarity,
            this.Inventory,
            this.FoilInventory});
            this.dataGridView_CardData.Location = new System.Drawing.Point(12, 12);
            this.dataGridView_CardData.Name = "dataGridView_CardData";
            this.dataGridView_CardData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_CardData.Size = new System.Drawing.Size(798, 608);
            this.dataGridView_CardData.TabIndex = 13;
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
            // 
            // FoilInventory
            // 
            this.FoilInventory.HeaderText = "Foil Inventory";
            this.FoilInventory.Name = "FoilInventory";
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(816, 515);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(105, 105);
            this.button_Save.TabIndex = 14;
            this.button_Save.Text = "Save Changes";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_Close
            // 
            this.button_Close.Location = new System.Drawing.Point(934, 515);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(105, 105);
            this.button_Close.TabIndex = 15;
            this.button_Close.Text = "Close";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // comboBox_Sets
            // 
            this.comboBox_Sets.FormattingEnabled = true;
            this.comboBox_Sets.Location = new System.Drawing.Point(816, 29);
            this.comboBox_Sets.Name = "comboBox_Sets";
            this.comboBox_Sets.Size = new System.Drawing.Size(223, 21);
            this.comboBox_Sets.TabIndex = 16;
            this.comboBox_Sets.SelectedIndexChanged += new System.EventHandler(this.comboBox_Sets_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(817, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Select Set:";
            // 
            // pictureBox_Card
            // 
            this.pictureBox_Card.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Card.Image")));
            this.pictureBox_Card.Location = new System.Drawing.Point(816, 198);
            this.pictureBox_Card.Name = "pictureBox_Card";
            this.pictureBox_Card.Size = new System.Drawing.Size(223, 311);
            this.pictureBox_Card.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Card.TabIndex = 18;
            this.pictureBox_Card.TabStop = false;
            // 
            // button_LoadSet
            // 
            this.button_LoadSet.Location = new System.Drawing.Point(816, 56);
            this.button_LoadSet.Name = "button_LoadSet";
            this.button_LoadSet.Size = new System.Drawing.Size(223, 23);
            this.button_LoadSet.TabIndex = 19;
            this.button_LoadSet.Text = "Load Set";
            this.button_LoadSet.UseVisualStyleBackColor = true;
            this.button_LoadSet.Click += new System.EventHandler(this.button_LoadSet_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 632);
            this.Controls.Add(this.button_LoadSet);
            this.Controls.Add(this.pictureBox_Card);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_Sets);
            this.Controls.Add(this.button_Close);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.dataGridView_CardData);
            this.Name = "Form2";
            this.Text = "Set Editor";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CardData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Card)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_CardData;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.ComboBox comboBox_Sets;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox_Card;
        private System.Windows.Forms.Button button_LoadSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn CardName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Set;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rarity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Inventory;
        private System.Windows.Forms.DataGridViewTextBoxColumn FoilInventory;
    }
}