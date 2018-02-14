namespace GameBreakersDBManagement
{
    partial class Settings
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_DaysBetweenUpdate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_Currency = new System.Windows.Forms.ComboBox();
            this.button_UpdatePrice = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_ImportExcelData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Days Between Price Updates";
            // 
            // textBox_DaysBetweenUpdate
            // 
            this.textBox_DaysBetweenUpdate.Location = new System.Drawing.Point(12, 29);
            this.textBox_DaysBetweenUpdate.Name = "textBox_DaysBetweenUpdate";
            this.textBox_DaysBetweenUpdate.Size = new System.Drawing.Size(156, 20);
            this.textBox_DaysBetweenUpdate.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(175, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Currency";
            // 
            // comboBox_Currency
            // 
            this.comboBox_Currency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Currency.FormattingEnabled = true;
            this.comboBox_Currency.Items.AddRange(new object[] {
            "USD",
            "CAD"});
            this.comboBox_Currency.Location = new System.Drawing.Point(174, 28);
            this.comboBox_Currency.Name = "comboBox_Currency";
            this.comboBox_Currency.Size = new System.Drawing.Size(156, 21);
            this.comboBox_Currency.TabIndex = 3;
            // 
            // button_UpdatePrice
            // 
            this.button_UpdatePrice.Location = new System.Drawing.Point(12, 55);
            this.button_UpdatePrice.Name = "button_UpdatePrice";
            this.button_UpdatePrice.Size = new System.Drawing.Size(156, 82);
            this.button_UpdatePrice.TabIndex = 4;
            this.button_UpdatePrice.Text = "Update Price Now";
            this.button_UpdatePrice.UseVisualStyleBackColor = true;
            this.button_UpdatePrice.Click += new System.EventHandler(this.button_UpdatePrice_Click);
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(255, 143);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(75, 75);
            this.button_Save.TabIndex = 5;
            this.button_Save.Text = "Save";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(12, 143);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 75);
            this.button_Cancel.TabIndex = 6;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_ImportExcelData
            // 
            this.button_ImportExcelData.Location = new System.Drawing.Point(174, 55);
            this.button_ImportExcelData.Name = "button_ImportExcelData";
            this.button_ImportExcelData.Size = new System.Drawing.Size(156, 82);
            this.button_ImportExcelData.TabIndex = 7;
            this.button_ImportExcelData.Text = "Import Excel Data";
            this.button_ImportExcelData.UseVisualStyleBackColor = true;
            this.button_ImportExcelData.Click += new System.EventHandler(this.button_ImportExcelData_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 224);
            this.Controls.Add(this.button_ImportExcelData);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.button_UpdatePrice);
            this.Controls.Add(this.comboBox_Currency);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_DaysBetweenUpdate);
            this.Controls.Add(this.label1);
            this.Name = "Settings";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_DaysBetweenUpdate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_Currency;
        private System.Windows.Forms.Button button_UpdatePrice;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_ImportExcelData;
    }
}