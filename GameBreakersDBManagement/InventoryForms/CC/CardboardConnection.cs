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
        //TODO: Use background threads in MtG form
        //TODO: Save to database in ccscraper
        //TODO: Clean up code. Start over? 
        //TODO: Clean up designer
        //TODO: CLean up Database manager
        //TODO: Get rid of network calls
        //TODO: Only query the database for stuff, not internet
        //TODO: Remove all the static vars, I don't need them
        //TODO: Change rows in grid view

        static string MTGSTOCKS_QUERY_ID = @"https://api.mtgstocks.com/search/autocomplete/";
        static string MTGSTOCKS_QUERY_DATA = @"https://api.mtgstocks.com/prints/";
        static string GATHERER_IMAGE_URL = @"http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=ABCDE&type=card";
        static string LOCAL_IMAGE_PATH = @"C:\GameBreakersInventory\Images\";
        static string IMAGE_TYPE = ".jpg";

        delegate void AddCardToRowDelegate(Dictionary<string, object> cardData);
        //string category, string number, string name, string team, string amount, string odds, string other, string inventory
        public CardboardConnection()
        {
            InitializeComponent();
        }
        
        //BUTTONS
        private void button_Name_Click(object sender, EventArgs e)
        {
            //TODO: Search Database for cards
        }
        private void button_Number_Click(object sender, EventArgs e)
        {
            //TODO: Same as name search
        }
        private void button_Set_Click(object sender, EventArgs e)
        {

        }
        private void button_Team_Click(object sender, EventArgs e)
        {

        }
        private void button_Search_Click(object sender, EventArgs e)
        {

        }
        private void button_SelectSet_Click(object sender, EventArgs e)
        {
            CCScraper ccScraper = new CCScraper();
            ccScraper.Show();
        }
        private void button_RemoveSingle_Click(object sender, EventArgs e)  //TODO: Condense these
        {
            var index = dataGridView_CardData.CurrentCell.RowIndex;

            var name = dataGridView_CardData.Rows[index].Cells[0].Value.ToString();
            var set = dataGridView_CardData.Rows[index].Cells[1].Value.ToString();

            RemoveOneFromInventory(name, set, false);

            Logger.LogActivity("Removed one of card: " + name + " of set " + set + " from inventory");

            var value = Int32.Parse(dataGridView_CardData.Rows[index].Cells[3].Value.ToString()) - 1;
            if (value < 0) value = 0;
            dataGridView_CardData.Rows[index].Cells[3].Value = value;
        }
        private void button_AddSingle_Click(object sender, EventArgs e)
        {
            var index = dataGridView_CardData.CurrentCell.RowIndex;

            var name = dataGridView_CardData.Rows[index].Cells[0].Value.ToString();
            var set = dataGridView_CardData.Rows[index].Cells[1].Value.ToString();

            AddOneToInventory(name, set, false);

            Logger.LogActivity("Added one of card: " + name + " of set " + set + " from inventory");

            var value = Int32.Parse(dataGridView_CardData.Rows[index].Cells[3].Value.ToString()) + 1;
            dataGridView_CardData.Rows[index].Cells[3].Value = value;
        }
        private void button_EditSet_Click(object sender, EventArgs e)
        {
            //TODO: Setup a CC Set Editor?
            SetEditorForm setEditor = new SetEditorForm();
            setEditor.Show();
        }

        //SEARCH
        void SearchForCard(string name)
        {
            var cards = DatabaseManager.GetCard(name);

            if (cards.Rows.Count < 1)
                return;

            foreach (DataRow card in cards.Rows)
            {
                var category = card[18].ToString().ToString();
                var number = card[7].ToString();
                var team = float.Parse(card[22].ToString());
                var printRun = float.Parse(card[19].ToString());
                var odds = float.Parse(card[21].ToString());
                var other = "";
                var inventory = "";
                
                name = card[3].ToString();
                AddCardToRow(new Dictionary<string, object> {
                    { "category", category },
                    { "number", number},
                    { "name", name},
                    { "team", team},
                    { "printRun", printRun},
                    { "odds", odds },
                    { "other", other},
                    { "inventory", inventory} });
            }            
        }

        //UTIL
        private void textBox_Name_GotFocus(object sender, EventArgs e)
        {
            AcceptButton = button_Name;
        }
        private void textBox_Set_GotFocus(object sender, EventArgs e)
        {
            AcceptButton = button_Number;
        }
        void AddCardToRow(Dictionary<string, object> cardData)
        {
            if(dataGridView_CardData.InvokeRequired)
            {
                AddCardToRowDelegate addCardToRowDelegate = new AddCardToRowDelegate(AddCardToRow);
                Invoke(addCardToRowDelegate, new object[] { cardData });
                return;
            }

            var category    = cardData["category"];
            var number      = cardData["number"];
            var name        = cardData["name"];
            var team        = cardData["team"];
            var printRun    = cardData["printRun"];
            var odds        = cardData["odds"];
            var other       = cardData["other"];
            var inventory   = cardData["inventory"];

            dataGridView_CardData.Rows.Add(category, number, name, team, printRun, odds, other, inventory);
        }
        
        //INVENTORY
        void AddOneToInventory(string card, string set, bool foil)
        {
            //TODO: Do I like this single line function?
            DatabaseManager.AddOneToInventory(card, set, foil);
        }
        void RemoveOneFromInventory(string card, string set, bool foil)
        {
            DatabaseManager.RemoveOneToInventory(card, set, foil);
        }
        int GetInventory(string card, string set)
        {
            return DatabaseManager.GetInventory(card, set);
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
                Logger.LogError("Error adding set to database, file: " + file);
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
                        Logger.LogError("Error adding card: " + values["name"] + " in set: " + expansion + "\r\n\r\nError message:" + ex.ToString());
                    }
                }
                catch(Exception ex)
                {
                    //TODO: THROW MY OWN EXCEPTIONS
                    Logger.LogError("Error parsing card: " + PrepareString(card, "name") + " in set: " + expansion  + "\r\n\r\nError message:" + ex.ToString());
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

            try
            {
                if (index == "colors" || index == "types" || index == "subtypes" || index == "colorIdentity" || index == "printings")
                    data = Regex.Replace(data, @"[^0-9a-zA-Z,]+", "");
                if (index == "printings")
                {
                    data = data.Split(',').Last();
                }
            }
            catch (Exception ex)
            {
                
            }

            return data;
        }
    }
}
