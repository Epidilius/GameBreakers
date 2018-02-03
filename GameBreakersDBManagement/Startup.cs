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
        public Startup()
        {
            InitializeComponent();
            Logger.Prep();
            MtGForm = new MtG();
            CCForm = new CardboardConnection();
            DatabaseManager.UnlockAllSets();

            //TODO: Query MtG Json for data. Do this here or in MtG form?
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
