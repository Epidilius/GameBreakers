using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameBreakersDBManagement
{
    public partial class Startup : Form
    {
        MtG MtGForm;
        CardboardConnection CCForm;
        static Startup StartupInstance;
        
        public Startup()
        {
            InitializeComponent();

            DatabaseManager.UnlockAllSets();

            SetupForms();

            StartupInstance = this;
        }
        public static Startup GetInstance()
        {
            return StartupInstance;
        }

        void SetupForms()
        {
            MtGForm           = new MtG();
            CCForm            = new CardboardConnection();
            MtGForm.MdiParent = this;
            MtGForm.Dock      = DockStyle.Fill;
            CCForm.MdiParent  = this;
            CCForm.Dock       = DockStyle.Fill;
        }
        
        public void UpdateCarts()
        {
            MtGForm.LoadCarts();
            CCForm.LoadCarts();
        }

        //FILE MENU ITEMS
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("For help, send an email to joel.cright@gmail.com", "Help", MessageBoxButtons.OK);
        }

        //MTG MENU ITEMS
        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CCForm.Hide();
            MtGForm.Show();
        }
        private void setEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetEditorForm setEditor = new SetEditorForm();
            setEditor.Show();
        }

        //CC MENU ITEMS
        private void inventoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MtGForm.Hide();
            CCForm.Show();
        }
        private void addSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CCScraper ccScraper = new CCScraper();
            ccScraper.Show();
        }
        private void addCardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManualEntry manualEntry = new ManualEntry();
            manualEntry.Show();
        }
    }
}
