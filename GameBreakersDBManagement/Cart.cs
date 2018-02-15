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

        public int GetID()
        {
            return ID;
        }

        public void CartUpdated()
        {
            dataGridView_Items.Rows.Clear();

            var query = "SELECT * FROM Carts WHERE (ID = " + ID + " AND Status = 'Active')";
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

                if(String.IsNullOrWhiteSpace(cardName) && String.IsNullOrWhiteSpace(cardSet) && String.IsNullOrWhiteSpace(cardAmount))
                {
                    continue;
                }

                var query = "";

                if(cardName.Contains("*F*"))
                {
                    query = "SELECT foilPrice FROM MtG WHERE (name = '" + cardName.Replace("'", "''").Replace(" *F*", "") + "' and expansion = '" + cardSet.Replace("'", "''") + "')";
                }
                else
                {
                    query = "SELECT price FROM MtG WHERE (name = '" + cardName.Replace("'", "''") + "' and expansion = '" + cardSet.Replace("'", "''") + "')";
                }

                try
                {
                    var results = DatabaseManager.RunQuery(query).Rows;
                    var price = results[0][0].ToString();

                    dataGridView_Items.Rows.Add(cardNames[i], cardSets[i], price, cardAmounts[i]);
                }
                catch(Exception ex)
                {
                    //Not Magic, currently don't have price data
                    dataGridView_Items.Rows.Add(cardNames[i], cardSets[i], "0", cardAmounts[i]);
                }
            }
        }

        void UpdatePrice()
        {
            var subtotal = 0f;

            foreach(DataGridViewRow row in dataGridView_Items.Rows)
            {
                try
                {
                    var price  = float.Parse(row.Cells["Price"].Value.ToString());
                    var amount = float.Parse(row.Cells["Amount"].Value.ToString());

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

        void SaveCustomerData()
        {
            var query = "UPDATE Carts SET " +
                "CustomerName = '" + textBox_CustomerName.Text + "', " +
                "CustomerEmail = '" + textBox_CustomerEmail.Text + "', " +
                "CustomerNumber = '" + textBox_CustomerEmail.Text + "', " + 
                "LastUpdated = '" + DateTime.Now + "' " +
                "WHERE ID = '" + ID + "'";

            DatabaseManager.RunQuery(query);
            Startup.GetInstance().UpdateCarts();
        }

        private void button_CompleteSale_Click(object sender, EventArgs e)
        {
            var idQuery   = "SELECT CardIDs FROM Carts WHERE ID = '" + ID + "'";
            var idResults = DatabaseManager.RunQuery(idQuery).Rows;
            var idArray   = idResults[0][0].ToString().Split('|');

            var cardPrices = "";
            for(int i = 0; i < dataGridView_Items.Rows.Count - 1; i++)
            {
                var row = dataGridView_Items.Rows[i];

                if (i != 0)
                    cardPrices += "|";
                cardPrices += row.Cells["Price"].Value.ToString();

                var id = idArray[i + 1];
                if (String.IsNullOrWhiteSpace(id))
                    continue;

                int multiverseID;
                var updateQuery = "";
                if (int.TryParse(id, out multiverseID))
                {
                    if (row.Cells["CardName"].Value.ToString().Contains("*F*"))
                    {
                        updateQuery = "UPDATE Mtg SET foilInventory = (foilInventory - " + row.Cells["Amount"].Value.ToString() + ") WHERE multiverseID = '" + multiverseID + "'";
                    }
                    else
                    {
                        updateQuery = "UPDATE Mtg SET inventory = (inventory - " + row.Cells["Amount"].Value.ToString() + ") WHERE multiverseID = '" + multiverseID + "'";
                    }
                }
                else
                {
                    updateQuery = "UPDATE Non_Mtg SET inventory = (inventory - " + row.Cells["Amount"].Value.ToString() + ") WHERE MD5Hash = '" + id + "'";
                }

                DatabaseManager.RunQuery(updateQuery);
            }

            var query = "UPDATE Carts SET " + 
                "CardPrices = '" + cardPrices + "', " +
                "Subtotal = '" + textBox_PriceSubtotal.Text + "', " +
                "Taxes = '" + textBox_PriceTaxes.Text + "', " +
                "Total = '" + textBox_PriceTotal.Text + "', " +
                "Status = 'Sale Complete' " + 
                "WHERE ID = '" + ID + "'";

            DatabaseManager.RunQuery(query);       

            SaveCustomerData();
            CartManager.StatusChanged();
            Close();
        }
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmResult = MessageBox.Show("Are you sure you want to delete the selected cart?",
                                         "Delete cart: " + ID,
                                         MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    CartManager.DeleteCart(ID);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Attempted to delete cart", ex.ToString(), "");
            }
            Close();
        }
        private void button_Save_Click(object sender, EventArgs e)
        {
            SaveCustomerData();
            Startup.GetInstance().UpdateCarts();
        }

        private void CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdatePrice();
        }

        private void OnClose(object sender, FormClosedEventArgs e)
        {
            CartManager.CartClosed(ID);
        }
    }
}
