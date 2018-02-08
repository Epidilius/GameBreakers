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
    public partial class Cart : Form
    {
        static string ITEM_DELIMITER = "|";
        int ID;

        public Cart()
        {
            InitializeComponent();
        }

        public Cart(int id)
        {
            InitializeComponent();

            ID = id;
            CartUpdated();
        }

        public void CartUpdated()
        {
            //TODO: Update cart from DB
            var query = "SELECT * FROM ActiveCarts WHERE ID = " + ID;
            var cartData = DatabaseManager.RunQuery(query);
        }
        void ParseCartData(DataTable cartData)
        {
            
        }


        private void button_CompleteSale_Click(object sender, EventArgs e)
        {

        }
        private void button_Cancel_Click(object sender, EventArgs e)
        {

        }
        private void button_Save_Click(object sender, EventArgs e)
        {

        }
    }
}
