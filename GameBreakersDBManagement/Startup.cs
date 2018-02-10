using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            MtGForm = new MtG();
            CCForm = new CardboardConnection();
            DatabaseManager.UnlockAllSets();

            StartupInstance = this;
        }
        public static Startup GetInstance()
        {
            return StartupInstance;
        }

        public void UpdateCarts()
        {
            MtGForm.LoadCarts();
            CCForm.LoadCarts();
        }

        private void button_MtG_Click(object sender, EventArgs e)
        {
            if (!MtGForm.Enabled)
                MtGForm = new MtG();
            MtGForm.Show();
        }

        private void button_CardboardConnection_Click(object sender, EventArgs e)
        {
            CCForm = new CardboardConnection();
            CCForm.Show();
        }
    }
}
