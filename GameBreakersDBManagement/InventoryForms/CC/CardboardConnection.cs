using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Scripting.Hosting;
using System.Threading;
using System.Globalization;

namespace GameBreakersDBManagement
{
    public partial class CardboardConnection : Form
    {
        //TODO: Update database to use one table, cards?
        //TODO: Update files to call a function in DBMan (CreateQuery) then call RunQuery
        //TODO: Save to database in ccscraper

        delegate void AddCardToRowDelegate(Dictionary<string, object> cardData);
        //TODO: Stop the service that keeps starting, I dont want it 
        public CardboardConnection()
        {
            InitializeComponent();

            LoadCarts();
        }

        //BUTTONS
        private void button_Name_Click(object sender, EventArgs e)
        {
            var query = "SELECT * FROM Non_Mtg WHERE NAME LIKE \'%" + textBox_Name.Text + "%\'";
            var matches = DatabaseManager.RunQuery(query);

            AddTableToRow(matches);
        }
        private void button_Team_Click(object sender, EventArgs e)
        {
            var query = "SELECT * FROM Non_Mtg WHERE TEAM LIKE \'%" + textBox_Team.Text + "%\'";
            var matches = DatabaseManager.RunQuery(query);

            AddTableToRow(matches);
        }
        private void button_Number_Click(object sender, EventArgs e)
        {
            var query = "SELECT * FROM Non_Mtg WHERE NUMBER LIKE \'" + textBox_Number.Text + "\'";
            var matches = DatabaseManager.RunQuery(query);

            AddTableToRow(matches);
        }
        private void button_Set_Click(object sender, EventArgs e)
        {
            var query = "SELECT * FROM Non_Mtg WHERE CATEGORY LIKE \'%" + textBox_Set.Text + "%\'";
            var matches = DatabaseManager.RunQuery(query);

            AddTableToRow(matches);
        }
        private void button_SelectSet_Click(object sender, EventArgs e)
        {
            CCScraper ccScraper = new CCScraper();
            ccScraper.Show();
        }
        private void button_RemoveSingle_Click(object sender, EventArgs e)  //TODO: Condense these
        {
            var index = dataGridView_CardData.CurrentCell.RowIndex;

            var name = dataGridView_CardData.Rows[index].Cells[2].Value.ToString();
            var category = dataGridView_CardData.Rows[index].Cells[0].Value.ToString();

            RemoveOneFromInventory(name, category);

            Logger.LogActivity("Removed one of card: " + name + " of set " + category + " from inventory");

            var value = Int32.Parse(dataGridView_CardData.Rows[index].Cells[6].Value.ToString()) - 1;
            if (value < 0) value = 0;
            dataGridView_CardData.Rows[index].Cells[6].Value = value;
        }
        private void button_AddSingle_Click(object sender, EventArgs e)
        {
            var index = dataGridView_CardData.CurrentCell.RowIndex;

            var name = dataGridView_CardData.Rows[index].Cells[2].Value.ToString();
            var category = dataGridView_CardData.Rows[index].Cells[0].Value.ToString();

            AddOneToInventory(name, category);

            Logger.LogActivity("Added one of card: " + name + " of set " + category + " from inventory");

            var value = Int32.Parse(dataGridView_CardData.Rows[index].Cells[6].Value.ToString()) + 1;
            dataGridView_CardData.Rows[index].Cells[6].Value = value;
        }
        private void button_EditSet_Click(object sender, EventArgs e)
        {
            //TODO: Setup a CC Set Editor?
            SetEditorForm setEditor = new SetEditorForm();
            setEditor.Show();
        }

        //UTIL
        void AddTableToRow(DataTable table)
        {
            dataGridView_CardData.Rows.Clear();
            if (table.Rows.Count > 1)
            {
                foreach (DataRow card in table.Rows)
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();

                    var id        = card["MD5Hash"];
                    var expansion = card["Expansion"];
                    var category  = card["Category"];
                    var number    = card["Number"];
                    var name      = card["Name"];
                    var team      = card["Team"];
                    var printRun  = card["PrintRun"];
                    var odds      = card["Odds"];
                    var extraData = card["ExtraData"];
                    var inventory = card["Inventory"];

                    values.Add("id", id);
                    values.Add("category", expansion);
                    values.Add("subCategory", category);
                    values.Add("number", number);
                    values.Add("name", name);
                    values.Add("team", team);
                    values.Add("printRun", printRun);
                    values.Add("odds", odds);
                    values.Add("inventory", inventory);
                    values.Add("extraData", extraData);

                    AddCardToRow(values);
                }
            }
        }
        void AddCardToRow(Dictionary<string, object> cardData)
        {
            if (dataGridView_CardData.InvokeRequired)
            {
                AddCardToRowDelegate addCardToRowDelegate = new AddCardToRowDelegate(AddCardToRow);
                Invoke(addCardToRowDelegate, new object[] { cardData });
                return;
            }

            var id          = cardData["id"];
            var category    = cardData["category"];
            var subCategory = cardData["subCategory"];
            var number      = cardData["number"];
            var name        = cardData["name"];
            var team        = cardData["team"];
            var printRun    = cardData["printRun"];
            var odds        = cardData["odds"];
            var inventory   = cardData["inventory"];
            var extraData   = cardData["extraData"];

            dataGridView_CardData.Rows.Add(category, subCategory, number, name, team, printRun, odds, inventory, extraData, id);
        }

        //INVENTORY
        void AddOneToInventory(string name, string category)
        {
            var query = "UPDATE Non_Mtg SET INVENTORY = INVENTORY + 1 WHERE NAME = \'" + name + "\' AND CATEGORY = \'" + category + "\'";
            DatabaseManager.RunQuery(query);
        }
        void RemoveOneFromInventory(string name, string category)
        {
            try
            {
                var query = "UPDATE Non_Mtg SET INVENTORY = INVENTORY - 1 WHERE NAME = \'" + name + "\' AND CATEGORY = \'" + category + "\'";
                DatabaseManager.RunQuery(query);
            }
            catch (Exception ex)
            {

            }
        }
        int GetInventory(string name, string category)
        {
            var query = "SELECT INVENTORY FROM Non_Mtg WHERE NAME = \'" + name + "\' AND CATEGORY = \'" + category + "\'";
            var dataTable = DatabaseManager.RunQuery(query);
            return (int)dataTable.Rows[0]["inventory"];
        }
        void AddNewSet(string file)
        {
            try
            {
                string setData = File.ReadAllText(file);
                JObject setJSON = JObject.Parse(setData);
                //TODO: ADD SET ID
                AddExpansionToDatabase(setJSON["cards"], setJSON["name"].ToString(), setJSON["code"].ToString());
            }
            catch (Exception ex)
            {
                Logger.LogError("Adding MtG set to database", ex.ToString(), file);
            }
        }
        void AddExpansionToDatabase(JToken cardList, string expansion, string abbreviation)
        {
            if (!DatabaseManager.CheckIfSetExists(expansion))
            {
                DatabaseManager.AddNewSet(expansion, abbreviation, null, "non_mtg");
                Logger.LogActivity("Adding new set: " + expansion + " to databse");
            }
            foreach (var card in cardList)
            {
                try
                {
                    string multiverseID = PrepareString(card, "multiverseid");
                    if (DatabaseManager.CheckIfCardExists(multiverseID))
                        continue;

                    Dictionary<string, object> values = new Dictionary<string, object>();

                    values.Add("category", PrepareString(card, "category"));
                    values.Add("number", PrepareString(card, "number"));
                    values.Add("name", PrepareString(card, "name"));
                    values.Add("team", PrepareString(card, "team"));
                    values.Add("printRun", PrepareString(card, "printRun"));
                    values.Add("odds", PrepareString(card, "odds"));
                    values.Add("other", PrepareString(card, "other"));
                    values.Add("inventory", 0);

                    try
                    {
                        DatabaseManager.AddNewCard("non_mtg", values);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Adding MtG card to database", ex.ToString(), card.ToString());
                    }
                }
                catch (Exception ex)
                {
                    //TODO: THROW MY OWN EXCEPTIONS
                    Logger.LogError("Preparing MtG card data to be added to database", ex.ToString(), card.ToString());
                }
            }
        }
        string PrepareString(JToken card, string index)
        {
            var data = "";

            try
            {
                data = card[index].ToString();
            }
            catch (Exception ex)
            {
                //Logger.LogError("Failed to parse data: " + index + " for card: " + card["name"].ToString());
            }

            return data;
        }

        private void textBox_Name_Enter(object sender, EventArgs e)
        {
            AcceptButton = button_Name;
        }
        private void textBox_Team_Enter(object sender, EventArgs e)
        {
            AcceptButton = button_Team;
        }
        private void textBox_Number_Enter(object sender, EventArgs e)
        {
            AcceptButton = button_Number;
        }
        private void textBox_Set_Enter(object sender, EventArgs e)
        {
            AcceptButton = button_Set;
        }

        //CART
        public void LoadCarts()
        {
            dataGridView_Carts.Rows.Clear();

            var query = "SELECT ID, CustomerName FROM Carts WHERE Status = 'Active' ORDER BY ID";
            var cartData = DatabaseManager.RunQuery(query);

            foreach (DataRow cart in cartData.Rows)
            {
                var cartID = cart[0].ToString();
                var cartCustomer = cart[1].ToString();

                dataGridView_Carts.Rows.Add(cartID, cartCustomer);
            }
        }
        private void button_AddToCart_Click(object sender, EventArgs e)
        {
            try
            {
                var cardIndex = dataGridView_CardData.CurrentCell.RowIndex;
                var cartIndex = dataGridView_Carts.CurrentCell.RowIndex;

                var cardExpansion = dataGridView_CardData.Rows[cardIndex].Cells[0].Value.ToString();
                var cardCategory = dataGridView_CardData.Rows[cardIndex].Cells[1].Value.ToString();
                var cardNumber = dataGridView_CardData.Rows[cardIndex].Cells[2].Value.ToString();
                var cardName = dataGridView_CardData.Rows[cardIndex].Cells[3].Value.ToString();
                var cartID = Convert.ToInt32(dataGridView_Carts.Rows[cartIndex].Cells[0].Value);

                var name = cardName + " - " + cardNumber;
                var set = cardExpansion + ": " + cardCategory;

                CartManager.AddItemToCart(cartID, name, set, 1);
            }
            catch (Exception ex)
            {
                //TODO: Message? Issue is most likely having not selected a cart or card
            }
        }
        private void button_OpenCart_Click(object sender, EventArgs e)
        {
            try
            {
                var cartIndex = dataGridView_Carts.CurrentCell.RowIndex;
                var cartID = Convert.ToInt32(dataGridView_Carts.Rows[cartIndex].Cells[0].Value);

                CartManager.DisplayCart(cartID);
            }
            catch (Exception ex)
            {
                //TODO: Message? Issue is most likely having not selected a cart or card
            }
        }
        private void button_DeleteCart_Click(object sender, EventArgs e)
        {
            try
            {
                var cartIndex = dataGridView_Carts.CurrentCell.RowIndex;
                var cartID = Convert.ToInt32(dataGridView_Carts.Rows[cartIndex].Cells[0].Value);

                var confirmResult = MessageBox.Show("Are you sure you want to delete the selected cart?", "Delete cart: " + cartID, MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    CartManager.DeleteCart(cartID);
                    LoadCarts();
                }
            }
            catch (Exception ex)
            {
                //TODO: Log it
            }
        }
        private void button_NewCart_Click(object sender, EventArgs e)
        {
            CartManager.CreateCart();
            LoadCarts();
        }

        void UpdateInventory(string md5, int amount)
        {
            var query = "UPDATE Non_Mtg SET " +
                "Inventory = '" + amount + "' " +
                "WHERE MD5Hash = '" + md5 + "'";

            DatabaseManager.RunQuery(query);
        }

        private void InventoryChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_CardData.Columns[e.ColumnIndex].Name == "Inventory")
            {
                var md5 = dataGridView_CardData.Rows[e.RowIndex].Cells[9].Value.ToString();
                var amount = Convert.ToInt32(dataGridView_CardData.Rows[e.RowIndex].Cells[7].Value);

                UpdateInventory(md5, amount);
            }
        }
    }
}
