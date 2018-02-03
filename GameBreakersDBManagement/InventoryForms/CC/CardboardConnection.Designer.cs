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
            this.button_AddSingle = new System.Windows.Forms.Button();
            this.button_RemoveSingle = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_SelectSet = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox_Name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_Name = new System.Windows.Forms.Button();
            this.textBox_Team = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_Team = new System.Windows.Forms.Button();
            this.button_Number = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Number = new System.Windows.Forms.TextBox();
            this.button_Set = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_Set = new System.Windows.Forms.TextBox();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlayerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Team = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrintRun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Odds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Inventory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CardData)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView_CardData);
            this.groupBox2.Controls.Add(this.button_AddSingle);
            this.groupBox2.Controls.Add(this.button_RemoveSingle);
            this.groupBox2.Location = new System.Drawing.Point(12, 226);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1044, 257);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Results";
            // 
            // dataGridView_CardData
            // 
            this.dataGridView_CardData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_CardData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Category,
            this.Number,
            this.PlayerName,
            this.Team,
            this.PrintRun,
            this.Odds,
            this.Inventory});
            this.dataGridView_CardData.Location = new System.Drawing.Point(6, 20);
            this.dataGridView_CardData.Name = "dataGridView_CardData";
            this.dataGridView_CardData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_CardData.Size = new System.Drawing.Size(798, 231);
            this.dataGridView_CardData.TabIndex = 12;
            // 
            // button_AddSingle
            // 
            this.button_AddSingle.Location = new System.Drawing.Point(828, 19);
            this.button_AddSingle.Name = "button_AddSingle";
            this.button_AddSingle.Size = new System.Drawing.Size(210, 100);
            this.button_AddSingle.TabIndex = 11;
            this.button_AddSingle.Text = "Add One To Inventory";
            this.button_AddSingle.UseVisualStyleBackColor = true;
            this.button_AddSingle.Click += new System.EventHandler(this.button_AddSingle_Click);
            // 
            // button_RemoveSingle
            // 
            this.button_RemoveSingle.Location = new System.Drawing.Point(828, 151);
            this.button_RemoveSingle.Name = "button_RemoveSingle";
            this.button_RemoveSingle.Size = new System.Drawing.Size(210, 100);
            this.button_RemoveSingle.TabIndex = 10;
            this.button_RemoveSingle.Text = "Remove One From Inventory";
            this.button_RemoveSingle.UseVisualStyleBackColor = true;
            this.button_RemoveSingle.Click += new System.EventHandler(this.button_RemoveSingle_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_SelectSet);
            this.groupBox3.Location = new System.Drawing.Point(834, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(222, 116);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sets";
            // 
            // button_SelectSet
            // 
            this.button_SelectSet.Location = new System.Drawing.Point(6, 16);
            this.button_SelectSet.Name = "button_SelectSet";
            this.button_SelectSet.Size = new System.Drawing.Size(210, 41);
            this.button_SelectSet.TabIndex = 15;
            this.button_SelectSet.Text = "Add Set";
            this.button_SelectSet.UseVisualStyleBackColor = true;
            this.button_SelectSet.Click += new System.EventHandler(this.button_SelectSet_Click);
            // 
            // groupBox4
            // 
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
            this.groupBox4.Size = new System.Drawing.Size(815, 208);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Search";
            // 
            // textBox_Name
            // 
            this.textBox_Name.Location = new System.Drawing.Point(6, 32);
            this.textBox_Name.Name = "textBox_Name";
            this.textBox_Name.Size = new System.Drawing.Size(571, 20);
            this.textBox_Name.TabIndex = 0;
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
            // button_Name
            // 
            this.button_Name.Location = new System.Drawing.Point(583, 16);
            this.button_Name.Name = "button_Name";
            this.button_Name.Size = new System.Drawing.Size(226, 41);
            this.button_Name.TabIndex = 2;
            this.button_Name.Text = "Search Name";
            this.button_Name.UseVisualStyleBackColor = true;
            this.button_Name.Click += new System.EventHandler(this.button_Name_Click);
            // 
            // textBox_Team
            // 
            this.textBox_Team.Location = new System.Drawing.Point(6, 79);
            this.textBox_Team.Name = "textBox_Team";
            this.textBox_Team.Size = new System.Drawing.Size(571, 20);
            this.textBox_Team.TabIndex = 3;
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
            // button_Team
            // 
            this.button_Team.Location = new System.Drawing.Point(583, 63);
            this.button_Team.Name = "button_Team";
            this.button_Team.Size = new System.Drawing.Size(226, 41);
            this.button_Team.TabIndex = 5;
            this.button_Team.Text = "Search Team";
            this.button_Team.UseVisualStyleBackColor = true;
            this.button_Team.Click += new System.EventHandler(this.button_Team_Click);
            // 
            // button_Number
            // 
            this.button_Number.Location = new System.Drawing.Point(583, 110);
            this.button_Number.Name = "button_Number";
            this.button_Number.Size = new System.Drawing.Size(226, 41);
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
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "By Number:";
            // 
            // textBox_Number
            // 
            this.textBox_Number.Location = new System.Drawing.Point(6, 126);
            this.textBox_Number.Name = "textBox_Number";
            this.textBox_Number.Size = new System.Drawing.Size(571, 20);
            this.textBox_Number.TabIndex = 6;
            // 
            // button_Set
            // 
            this.button_Set.Location = new System.Drawing.Point(583, 157);
            this.button_Set.Name = "button_Set";
            this.button_Set.Size = new System.Drawing.Size(226, 41);
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
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "By Set:";
            // 
            // textBox_Set
            // 
            this.textBox_Set.Location = new System.Drawing.Point(6, 173);
            this.textBox_Set.Name = "textBox_Set";
            this.textBox_Set.Size = new System.Drawing.Size(571, 20);
            this.textBox_Set.TabIndex = 9;
            // 
            // Category
            // 
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            this.Category.ReadOnly = true;
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
            this.Inventory.ReadOnly = true;
            // 
            // CardboardConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 493);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Name = "CardboardConnection";
            this.Text = "GameBreakers M:tG Inventory";
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CardData)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_AddSingle;
        private System.Windows.Forms.Button button_RemoveSingle;
        private System.Windows.Forms.DataGridView dataGridView_CardData;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_SelectSet;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlayerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Team;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrintRun;
        private System.Windows.Forms.DataGridViewTextBoxColumn Odds;
        private System.Windows.Forms.DataGridViewTextBoxColumn Inventory;
    }
}

