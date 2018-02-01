namespace GameBreakersDBManagement
{
    partial class Startup
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
            this.button_CardboardConnection = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.button_MtG = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 255);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Magic: the Gathering";
            // 
            // button_CardboardConnection
            // 
            this.button_CardboardConnection.BackgroundImage = global::GameBreakersDBManagement.Properties.Resources.The_Cardboard_Connection;
            this.button_CardboardConnection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_CardboardConnection.Cursor = System.Windows.Forms.Cursors.Default;
            this.button_CardboardConnection.Location = new System.Drawing.Point(254, 18);
            this.button_CardboardConnection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_CardboardConnection.Name = "button_CardboardConnection";
            this.button_CardboardConnection.Size = new System.Drawing.Size(225, 231);
            this.button_CardboardConnection.TabIndex = 7;
            this.button_CardboardConnection.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_CardboardConnection.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_CardboardConnection.UseVisualStyleBackColor = true;
            this.button_CardboardConnection.Click += new System.EventHandler(this.button_CardboardConnection_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(277, 255);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(169, 20);
            this.label6.TabIndex = 6;
            this.label6.Text = "Cardboard Connection";
            // 
            // button_MtG
            // 
            this.button_MtG.BackgroundImage = global::GameBreakersDBManagement.Properties.Resources.MTG_Logo;
            this.button_MtG.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_MtG.Cursor = System.Windows.Forms.Cursors.Default;
            this.button_MtG.Location = new System.Drawing.Point(20, 20);
            this.button_MtG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_MtG.Name = "button_MtG";
            this.button_MtG.Size = new System.Drawing.Size(225, 231);
            this.button_MtG.TabIndex = 1;
            this.button_MtG.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_MtG.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_MtG.UseVisualStyleBackColor = true;
            this.button_MtG.Click += new System.EventHandler(this.button_MtG_Click);
            // 
            // Startup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 308);
            this.Controls.Add(this.button_CardboardConnection);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button_MtG);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Startup";
            this.Text = "Startup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_MtG;
        private System.Windows.Forms.Button button_CardboardConnection;
        private System.Windows.Forms.Label label6;
    }
}