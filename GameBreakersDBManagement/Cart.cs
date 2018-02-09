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
        static char ITEM_DELIMITER = '|';
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
            var query = "SELECT * FROM ActiveCarts WHERE ID = " + ID;
            var cartData = DatabaseManager.RunQuery(query);
            ParseCartData(cartData);
            UpdatePrice();
        }
        void ParseCartData(DataTable cartData)
        {
            var row           = cartData.Rows[0];

            var cardNames     = row["CardNames"].ToString().Split(ITEM_DELIMITER);
            var cardSets      = row["CardExpansions"].ToString().Split(ITEM_DELIMITER);
            var cardAmounts   = row["CardAmounts"].ToString().Split(ITEM_DELIMITER);

            var customerName  = row["CustomerName"].ToString();
            var customerPhone = row["CustomerNumber"].ToString();
            var customerEmail = row["CustomerEmail"].ToString();

            textBox_CustomerName.Text  = customerName;
            textBox_CustomerPhone.Text = customerPhone;
            textBox_CustomerEmail.Text = customerEmail;

            for(int i = 0; i < cardNames.Count(); i++)
            {
                var cardName   = cardNames[i];
                var cardSet    = cardSets[i];
                var cardAmount = cardAmounts[i];

                var query = "";

                if(cardName.Contains("*F*"))
                {
                    query = "SELECT foilPrice FROM MtG WHERE (name = '" + cardName.Replace("'", "''").Replace(" *F*", "") + "' and expansion = '" + cardSet.Replace("'", "''") + "')";
                }
                else
                {
                    query = "SELECT price FROM MtG WHERE (name = '" + cardName.Replace("'", "''") + "' and expansion = '" + cardSet.Replace("'", "''") + "')";
                }
                
                var results = DatabaseManager.RunQuery(query).Rows;

                var price = results[0][0].ToString();

                dataGridView_Items.Rows.Add(cardNames[i], cardSets[i], price, cardAmounts[i]);
            }

            var temp = -1;
        }

        void UpdatePrice()
        {
            var subtotal = 0f;

            foreach(DataGridViewRow row in dataGridView_Items.Rows)
            {
                try
                {
                    var price  = float.Parse(row.Cells[2].Value.ToString());
                    var amount = float.Parse(row.Cells[3].Value.ToString());

                    subtotal += (price * amount);
                }
                catch (Exception ex)
                {
                    //TODO: Anything?
                }
            }
            
            var taxes = subtotal * 0.13f;
            var total = subtotal + taxes;

            textBox_PriceSubtotal.Text = subtotal.ToString();
            textBox_PriceTaxes.Text    = taxes.ToString();
            textBox_PriceTotal.Text    = total.ToString();
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

        private void CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdatePrice();
        }
    }
}
