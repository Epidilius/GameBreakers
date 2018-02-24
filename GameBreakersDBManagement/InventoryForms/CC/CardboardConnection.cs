using System;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

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
            FormBorderStyle = FormBorderStyle.None;

            comboBox_ParamType.SelectedIndex     = 0;
            comboBox_InclusionType.SelectedIndex = 0;
            comboBox_SearchType.SelectedIndex    = 0;

            LoadCarts();
        }

        //SEARCH
        private void button_AddParam_Click(object sender, EventArgs e)
        {
            var param      = textBox_SearchParameter.Text;
            var type       = comboBox_ParamType.Text;
            var include    = comboBox_InclusionType.Text;
            var searchType = comboBox_SearchType.Text;

            if (String.IsNullOrWhiteSpace(param))
                return;

            AddParamToView(param, type, include, searchType);
        }
        private void button_Remove_Click(object sender, EventArgs e)
        {
            var index = dataGridView_SearchParams.CurrentCell.RowIndex;
            var row   = dataGridView_SearchParams.Rows[index];
            if(!row.IsNewRow)
            {
                dataGridView_SearchParams.Rows.Remove(row);
            }
        }
        void AddParamToView(string param, string type, string inclusion, string searchType)
        {
            dataGridView_SearchParams.Rows.Add(param, type, inclusion, searchType);
        }
        private void button_Search_Click(object sender, EventArgs e)
        {
            if (dataGridView_SearchParams.Rows.Count <= 1)
                return;

            var query = "SELECT * FROM Non_Mtg WHERE (";

            for(int i = 0; i < dataGridView_SearchParams.Rows.Count - 1; i++)
            {
                var param      = Convert.ToString(dataGridView_SearchParams.Rows[i].Cells["Parameter"].Value);
                var type       = Convert.ToString(dataGridView_SearchParams.Rows[i].Cells["Type"].Value);
                var include    = Convert.ToString(dataGridView_SearchParams.Rows[i].Cells["Inclusion"].Value);
                var searchType = Convert.ToString(dataGridView_SearchParams.Rows[i].Cells["SearchType"].Value);

                if(String.IsNullOrWhiteSpace(param) || String.IsNullOrWhiteSpace(type) || String.IsNullOrWhiteSpace(include) || String.IsNullOrWhiteSpace(searchType))
                {
                    continue;
                }

                query += PrepSearchData(param, type, include, searchType);

                if (i != dataGridView_SearchParams.Rows.Count - 2)
                {
                    query += " AND ";
                }
            }

            query += ")";

            if(query.Contains("()"))
                return;

            var cards = DatabaseManager.RunQuery(query);
            AddTableToRow(cards);
        }
        string PrepSearchData(string param, string type, string include, string searchType)
        {
            if (include == "And") include = " LIKE ";
            else include = " NOT LIKE ";

            if (type == "Card Number") type = "Number";
            else if (type == "Player Name") type = "Name";
            else if (type == "Print Run") type = "PrintRun";

            if (searchType == "Exact") param = "'" + param + "'";
            else param = "'%" + param + "%'";

            return type + include + param;
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

                    var id        = Convert.ToString(card["MD5Hash"]);
                    var year      = Convert.ToString(card["Year"]);
                    var sport     = Convert.ToString(card["Sport"]);
                    var brand     = Convert.ToString(card["Brand"]);
                    var subset    = Convert.ToString(card["Category"]);
                    var number    = Convert.ToString(card["Number"]);
                    var name      = Convert.ToString(card["Name"]);
                    var team      = Convert.ToString(card["Team"]);
                    var printRun  = Convert.ToString(card["PrintRun"]);
                    var odds      = Convert.ToString(card["Odds"]);
                    var extraData = Convert.ToString(card["ExtraData"]);
                    var inventory = Convert.ToString(card["Inventory"]);
                    
                    values.Add("id", id);
                    values.Add("year", year);
                    values.Add("sport", sport);
                    values.Add("brand", brand);
                    values.Add("subset", subset);
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
                try
                {
                    AddCardToRowDelegate addCardToRowDelegate = new AddCardToRowDelegate(AddCardToRow);
                    Invoke(addCardToRowDelegate, new object[] { cardData });
                }
                catch(Exception ex)
                {
                    //Form was disposed
                }
                return;
            }

            var id          = cardData["id"];
            var year        = cardData["year"];
            var sport       = cardData["sport"];
            var brand       = cardData["brand"];
            var subset      = cardData["subset"];
            var number      = cardData["number"];
            var name        = cardData["name"];
            var team        = cardData["team"];
            var printRun    = cardData["printRun"];
            var odds        = cardData["odds"];
            var inventory   = cardData["inventory"];
            var extraData   = cardData["extraData"];

            dataGridView_CardData.Rows.Add(year, brand, sport, subset, number, name, team, printRun, odds, inventory, extraData, id);
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

        private void textBox_SearchParameter_Enter(object sender, EventArgs e)
        {
            AcceptButton = button_AddParam;
        }

        //CART
        public void LoadCarts()
        {
            dataGridView_Carts.Rows.Clear();

            var query = "SELECT ID, CustomerName, CardAmounts FROM Carts WHERE Status = 'Active' ORDER BY ID";
            var cartData = DatabaseManager.RunQuery(query);

            foreach (DataRow cart in cartData.Rows)
            {
                var cartID       = Convert.ToString(cart[0]);
                var cartCustomer = Convert.ToString(cart[1]);
                var cardAmounts  = Convert.ToString(cart[2]);

                var amount = GetCardAmount(cardAmounts);

                try
                {
                    dataGridView_Carts.Rows.Add(cartID, cartCustomer, amount);
                }
                catch (Exception ex)
                {
                    //TODO: Log this? Most likely because form is closed
                }
            }
        }
        int GetCardAmount(string cardAmounts)
        {
            if (String.IsNullOrWhiteSpace(cardAmounts))
                return 0;

            while(cardAmounts.StartsWith("|"))
            {
                cardAmounts = cardAmounts.Substring(1);
            }

            var splitAmounts = cardAmounts.Split('|');

            var totalAmount = 0;

            foreach(var amount in splitAmounts)
            {
                totalAmount += Convert.ToInt32(amount);
            }

            return totalAmount;
        }
        private void button_AddToCart_Click(object sender, EventArgs e)
        {
            try
            {
                var cardIndex = dataGridView_CardData.CurrentCell.RowIndex;
                var cartIndex = dataGridView_Carts.CurrentCell.RowIndex;

                var cardExpansion = dataGridView_CardData.Rows[cardIndex].Cells["Year"].Value.ToString();
                var cardCategory  = dataGridView_CardData.Rows[cardIndex].Cells["Brand"].Value.ToString();
                var cardNumber    = dataGridView_CardData.Rows[cardIndex].Cells["Number"].Value.ToString();
                var cardName      = dataGridView_CardData.Rows[cardIndex].Cells["Category"].Value.ToString();

                var cartID = Convert.ToInt32(dataGridView_Carts.Rows[cartIndex].Cells["CartID"].Value);

                var name = cardName + " - " + cardNumber;
                var set = cardExpansion + ": " + cardCategory;

                var cardID = dataGridView_CardData.Rows[cardIndex].Cells["CardID"].Value.ToString();

                CartManager.AddItemToCart(cartID, cardID, name, set, 1);
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
                var cartID = Convert.ToInt32(dataGridView_Carts.Rows[cartIndex].Cells["CartID"].Value);

                CartManager.DisplayCart(cartID);
            }
            catch (Exception ex)
            {
                //TODO: Message? Issue is most likely having not selected a cart or card
            }
        }
        private void button_NewCart_Click(object sender, EventArgs e)
        {
            CartManager.CreateCart();
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
                var md5 = dataGridView_CardData.Rows[e.RowIndex].Cells["CardID"].Value.ToString();
                var amount = Convert.ToInt32(dataGridView_CardData.Rows[e.RowIndex].Cells["Inventory"].Value);

                UpdateInventory(md5, amount);
            }
        }
    }
}
