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
            this.CardName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CardExpansion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_CustomerName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_PriceSubtotal = new System.Windows.Forms.TextBox();
            this.textBox_PriceTaxes = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_PriceTotal = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_CustomerPhone = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_CustomerEmail = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Items)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(495, 319);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(98, 77);
            this.button_Cancel.TabIndex = 2;
            this.button_Cancel.Text = "Cancel Sale";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(599, 319);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(98, 77);
            this.button_Save.TabIndex = 3;
            this.button_Save.Text = "Save Cart";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_CompleteSale
            // 
            this.button_CompleteSale.Location = new System.Drawing.Point(703, 319);
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
            this.dataGridView_Items.Location = new System.Drawing.Point(12, 28);
            this.dataGridView_Items.Name = "dataGridView_Items";
            this.dataGridView_Items.Size = new System.Drawing.Size(477, 368);
            this.dataGridView_Items.TabIndex = 5;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Items";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Name";
            // 
            // textBox_CustomerName
            // 
            this.textBox_CustomerName.Location = new System.Drawing.Point(6, 32);
            this.textBox_CustomerName.Name = "textBox_CustomerName";
            this.textBox_CustomerName.Size = new System.Drawing.Size(294, 20);
            this.textBox_CustomerName.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Subtotal";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_PriceTotal);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox_PriceTaxes);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_PriceSubtotal);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(495, 159);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 154);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Price";
            // 
            // textBox_PriceSubtotal
            // 
            this.textBox_PriceSubtotal.Location = new System.Drawing.Point(7, 33);
            this.textBox_PriceSubtotal.Name = "textBox_PriceSubtotal";
            this.textBox_PriceSubtotal.Size = new System.Drawing.Size(291, 20);
            this.textBox_PriceSubtotal.TabIndex = 12;
            // 
            // textBox_PriceTaxes
            // 
            this.textBox_PriceTaxes.Location = new System.Drawing.Point(7, 73);
            this.textBox_PriceTaxes.Name = "textBox_PriceTaxes";
            this.textBox_PriceTaxes.Size = new System.Drawing.Size(291, 20);
            this.textBox_PriceTaxes.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Taxes";
            // 
            // textBox_PriceTotal
            // 
            this.textBox_PriceTotal.Location = new System.Drawing.Point(7, 113);
            this.textBox_PriceTotal.Name = "textBox_PriceTotal";
            this.textBox_PriceTotal.Size = new System.Drawing.Size(291, 20);
            this.textBox_PriceTotal.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Total";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.textBox_CustomerEmail);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBox_CustomerPhone);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBox_CustomerName);
            this.groupBox2.Location = new System.Drawing.Point(495, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(306, 141);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Customer";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Phone Number";
            // 
            // textBox_CustomerPhone
            // 
            this.textBox_CustomerPhone.Location = new System.Drawing.Point(6, 71);
            this.textBox_CustomerPhone.Name = "textBox_CustomerPhone";
            this.textBox_CustomerPhone.Size = new System.Drawing.Size(294, 20);
            this.textBox_CustomerPhone.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 94);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Email";
            // 
            // textBox_CustomerEmail
            // 
            this.textBox_CustomerEmail.Location = new System.Drawing.Point(7, 110);
            this.textBox_CustomerEmail.Name = "textBox_CustomerEmail";
            this.textBox_CustomerEmail.Size = new System.Drawing.Size(294, 20);
            this.textBox_CustomerEmail.TabIndex = 14;
            // 
            // Cart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 408);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView_Items);
            this.Controls.Add(this.button_CompleteSale);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.button_Cancel);
            this.Name = "Cart";
            this.Text = "Cart";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Items)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_CustomerName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_PriceTotal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_PriceTaxes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_PriceSubtotal;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_CustomerEmail;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_CustomerPhone;
    }
}