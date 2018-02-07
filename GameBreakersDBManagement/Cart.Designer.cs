namespace GameBreakersDBManagement
{
    partial class Cart
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
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_CompleteSale = new System.Windows.Forms.Button();
            this.dataGridView_Items = new System.Windows.Forms.DataGridView();
            this.dataGridView_Price = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.CardName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CardExpansion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Items)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Price)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(499, 381);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(98, 77);
            this.button_Cancel.TabIndex = 2;
            this.button_Cancel.Text = "Cancel Sale";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(603, 381);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(98, 77);
            this.button_Save.TabIndex = 3;
            this.button_Save.Text = "Save Cart";
            this.button_Save.UseVisualStyleBackColor = true;
            // 
            // button_CompleteSale
            // 
            this.button_CompleteSale.Location = new System.Drawing.Point(707, 381);
            this.button_CompleteSale.Name = "button_CompleteSale";
            this.button_CompleteSale.Size = new System.Drawing.Size(98, 77);
            this.button_CompleteSale.TabIndex = 4;
            this.button_CompleteSale.Text = "Complete Sale";
            this.button_CompleteSale.UseVisualStyleBackColor = true;
            this.button_CompleteSale.Click += new System.EventHandler(this.button_CompleteSale_Click);
            // 
            // dataGridView_Items
            // 
            this.dataGridView_Items.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Items.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CardName,
            this.CardExpansion,
            this.Price,
            this.Amount});
            this.dataGridView_Items.Location = new System.Drawing.Point(13, 90);
            this.dataGridView_Items.Name = "dataGridView_Items";
            this.dataGridView_Items.Size = new System.Drawing.Size(477, 368);
            this.dataGridView_Items.TabIndex = 5;
            // 
            // dataGridView_Price
            // 
            this.dataGridView_Price.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Price.Location = new System.Drawing.Point(496, 90);
            this.dataGridView_Price.Name = "dataGridView_Price";
            this.dataGridView_Price.Size = new System.Drawing.Size(309, 285);
            this.dataGridView_Price.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Items";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(496, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Price";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Customer";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(478, 20);
            this.textBox1.TabIndex = 10;
            // 
            // CardName
            // 
            this.CardName.HeaderText = "Name";
            this.CardName.Name = "CardName";
            this.CardName.ReadOnly = true;
            // 
            // CardExpansion
            // 
            this.CardExpansion.HeaderText = "Expansion";
            this.CardExpansion.Name = "CardExpansion";
            this.CardExpansion.ReadOnly = true;
            // 
            // Price
            // 
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            // 
            // Amount
            // 
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            // 
            // Cart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 470);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView_Price);
            this.Controls.Add(this.dataGridView_Items);
            this.Controls.Add(this.button_CompleteSale);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.button_Cancel);
            this.Name = "Cart";
            this.Text = "Cart";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Items)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Price)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_CompleteSale;
        private System.Windows.Forms.DataGridView dataGridView_Items;
        private System.Windows.Forms.DataGridViewTextBoxColumn CardName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CardExpansion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridView dataGridView_Price;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
    }
}