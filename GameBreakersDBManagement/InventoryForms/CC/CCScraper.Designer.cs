﻿namespace GameBreakersDBManagement
{
    partial class CCScraper
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
            this.textBox_URL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_Scrape = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.dataGridView_CardList = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CardList)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox_URL
            // 
            this.textBox_URL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_URL.Location = new System.Drawing.Point(12, 29);
            this.textBox_URL.Name = "textBox_URL";
            this.textBox_URL.Size = new System.Drawing.Size(1005, 20);
            this.textBox_URL.TabIndex = 0;
            this.textBox_URL.Text = "https://www.cardboardconnection.com/2013-14-panini-national-treasures-hockey-card" +
    "s";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "URL";
            // 
            // button_Scrape
            // 
            this.button_Scrape.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Scrape.Location = new System.Drawing.Point(1023, 12);
            this.button_Scrape.Name = "button_Scrape";
            this.button_Scrape.Size = new System.Drawing.Size(159, 83);
            this.button_Scrape.TabIndex = 2;
            this.button_Scrape.Text = "Collect Data";
            this.button_Scrape.UseVisualStyleBackColor = true;
            this.button_Scrape.Click += new System.EventHandler(this.button_Scrape_Click);
            // 
            // button_Save
            // 
            this.button_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Save.Location = new System.Drawing.Point(1023, 101);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(159, 83);
            this.button_Save.TabIndex = 3;
            this.button_Save.Text = "Save Data";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // dataGridView_CardList
            // 
            this.dataGridView_CardList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_CardList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_CardList.Location = new System.Drawing.Point(13, 56);
            this.dataGridView_CardList.Name = "dataGridView_CardList";
            this.dataGridView_CardList.Size = new System.Drawing.Size(1004, 399);
            this.dataGridView_CardList.TabIndex = 4;
            // 
            // CCScraper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1194, 470);
            this.Controls.Add(this.dataGridView_CardList);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.button_Scrape);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_URL);
            this.Name = "CCScraper";
            this.Text = "Cardboard Connections Data Scraper";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CardList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_URL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_Scrape;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.DataGridView dataGridView_CardList;
    }
}

