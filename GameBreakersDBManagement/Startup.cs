using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameBreakersDatabaseManagement
{
    public partial class Startup : Form
    {
        public Startup()
        {
            InitializeComponent();
            Logger.Prep();
        }

        private void button_MtG_Click(object sender, EventArgs e)
        {
            MtG mtgForm = new MtG();
            mtgForm.Show();
        }

        private void button_CardboardConnection_Click(object sender, EventArgs e)
        {
            CardboardConnection ccForm = new CardboardConnection();
            ccForm.Show();
        }
    }
}
