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

namespace GameBreakersDBManagement
{
    public partial class Form1 : Form
    {
        DatabaseManager dbMan;
        Logger logger;

        static string MTGSTOCKS_QUERY_ID = @"https://api.mtgstocks.com/search/autocomplete/";
        static string MTGSTOCKS_QUERY_DATA = @"https://api.mtgstocks.com/prints/";
        static string GATHERER_IMAGE_URL = @"http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=ABCDE&type=card";
        static string LOCAL_IMAGE_PATH = @"C:\GameBreakersInventory\Images\";
        static string IMAGE_TYPE = ".jpg";


        public Form1()
        {
            InitializeComponent();
            dbMan = DatabaseManager.GetInstace();
            logger = Logger.GetLogger();
            
            //TODO: Compare to number of files in directory, if lower comapre each set to the DB, if missing add the set
            if(dbMan.GetAllSets().Rows.Count == 0)
            {
                string[] files = Directory.GetFiles(@"C:\GameBreakersInventory\Set JSON\", "*.json", SearchOption.AllDirectories);
                foreach(var file in files)
                {
                    AddNewSet(file);
                }
            }
        }
        
        //BUTTONS
        private void button_Search_Click(object sender, EventArgs e)
        {
            //TODO: Call UpdateDisplays in these?
            dataGridView_CardData.Rows.Clear();
            try
            {
                SearchForCard(textBox_Name.Text);
            }
            catch(Exception ex)
            {
                logger.LogError("Error searching for card: " + textBox_Name.Text + "\r\n\r\nError message:" + ex.ToString());
            }
        }
        private void button_SearchSet_Click(object sender, EventArgs e)
        {
            //TODO: abbreviations search
            dataGridView_CardData.Rows.Clear();
            try
            {
                SearchForSet(textBox_Set.Text);
            }
            catch(Exception ex)
            {
                logger.LogError("Error searching for set: " + textBox_Set.Text + "\r\n\r\nError message:" + ex.ToString());
            }
        }
        private void button_SelectSet_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            DialogResult result = ofd.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                AddNewSet(ofd.FileName);
            }
        }
        private void button_RemoveSingle_Click(object sender, EventArgs e)  //TODO: Condense these
        {
            var index = dataGridView_CardData.CurrentCell.RowIndex;

            var name = dataGridView_CardData.Rows[index].Cells[0].Value.ToString();
            var set = dataGridView_CardData.Rows[index].Cells[1].Value.ToString();

            RemoveOneFromInventory(name, set, false);

            logger.LogActivity("Removed one of card: " + name + " of set " + set + " from inventory");

            var value = Int32.Parse(dataGridView_CardData.Rows[index].Cells[3].Value.ToString()) - 1;
            if (value < 0) value = 0;
            dataGridView_CardData.Rows[index].Cells[3].Value = value;
        }
        private void button_RemoveFoil_Click(object sender, EventArgs e)
        {
            var index = dataGridView_CardData.CurrentCell.RowIndex;

            var name = dataGridView_CardData.Rows[index].Cells[0].Value.ToString();
            var set = dataGridView_CardData.Rows[index].Cells[1].Value.ToString();

            RemoveOneFromInventory(name, set, true);

            logger.LogActivity("Removed foil one of card: " + name + " of set " + set + " from inventory");

            var value = Int32.Parse(dataGridView_CardData.Rows[index].Cells[4].Value.ToString()) - 1;
            if (value < 0) value = 0;
            dataGridView_CardData.Rows[index].Cells[4].Value = value;
        }
        private void button_AddSingle_Click(object sender, EventArgs e)
        {
            var index = dataGridView_CardData.CurrentCell.RowIndex;

            var name = dataGridView_CardData.Rows[index].Cells[0].Value.ToString();
            var set = dataGridView_CardData.Rows[index].Cells[1].Value.ToString();

            AddOneToInventory(name, set, false);

            logger.LogActivity("Added one of card: " + name + " of set " + set + " from inventory");

            var value = Int32.Parse(dataGridView_CardData.Rows[index].Cells[3].Value.ToString()) + 1;
            dataGridView_CardData.Rows[index].Cells[3].Value = value;
        }
        private void button_AddFoil_Click(object sender, EventArgs e)
        {
            var index = dataGridView_CardData.CurrentCell.RowIndex;

            var name = dataGridView_CardData.Rows[index].Cells[0].Value.ToString();
            var set = dataGridView_CardData.Rows[index].Cells[1].Value.ToString();

            AddOneToInventory(name, set, true);

            logger.LogActivity("Added foil one of card: " + name + " of set " + set + " from inventory");

            var value = Int32.Parse(dataGridView_CardData.Rows[index].Cells[4].Value.ToString()) + 1;
            dataGridView_CardData.Rows[index].Cells[4].Value = value;
        }
        private void button_EditSet_Click(object sender, EventArgs e)
        {
            Form2 setEditor = new Form2();
            setEditor.Show();
        }

        //SEARCH
        void SearchForCard(string name)
        {
            var cards = dbMan.GetCard(name);

            if (cards.Rows.Count > 0)
            {
                foreach (DataRow card in cards.Rows)
                {
                    var set = card[18].ToString();
                    var rarity = card[7];
                    var inventory = card[20];
                    var foilInventory = card[22];
                    var price = card[19];
                    var foilPrice = card[21];

                    if(card == cards.Rows[cards.Rows.Count-1])
                    {
                        pictureBox_Card.Image = GetImageForCard(name, set, Int32.Parse(card[17].ToString()));
                    }
                    
                    dataGridView_CardData.Rows.Add(name, set, rarity, inventory, foilInventory, price, foilPrice);
                }
            }
            else
            {
                //TODO: Add cards found here to DB
                var id = GetMTGStocksID(name);
                if (id == -1)
                {
                    logger.LogError("Could not find card with name: " + name);
                    return;
                }

                var cardObject = GetMTGStocksData(id);
                if (cardObject == null)
                {
                    logger.LogError("Could not find card with name: " + name + " and MTG Stocks ID: " + id);
                    return;
                }

                var card = ParseCardData(cardObject);
                if (card == null)
                {
                    logger.LogError("Could not find parse data: " + card.ToString());
                    return;
                }

                if(card == cardObject)
                {
                    JArray sets = JArray.Parse(card["sets"].ToString());

                    foreach(var cardData in sets)
                    {
                        JObject jObject = GetMTGStocksData(Int32.Parse(cardData["id"].ToString()));
                        var tokenSet = jObject["card_set"]["name"].ToString();
                        var cardRarity = jObject["rarity"].ToString();
                        var cardPrice = float.Parse(jObject["latest_price"]["avg"].ToString());
                        var cardFoilPrice = 0f;
                        var cardStrprice = jObject["latest_price"]["foil"].ToString(); //TODO: Market price? [market]
                        if (cardStrprice == "") cardFoilPrice = 0;
                        else cardFoilPrice = float.Parse(cardStrprice);
                        dataGridView_CardData.Rows.Add(name, tokenSet, cardRarity, 0, 0, cardPrice, cardFoilPrice);
                    }
                }

                var set = card["card_set"]["name"].ToString();
                var rarity = card["rarity"].ToString();
                var price = float.Parse(card["latest_price"]["avg"].ToString());
                var foilPrice = 0f;
                var strprice = card["latest_price"]["foil"].ToString(); //TODO: Market price? [market]
                if (strprice == "") foilPrice = 0;
                else foilPrice = float.Parse(strprice);
                dataGridView_CardData.Rows.Add(name, set, rarity, 0, 0, price, foilPrice);
            }
        }
        void SearchForSet(string set)
        {
            var cards = dbMan.GetAllCardsForSet(set);

            if (cards.Rows.Count > 0)
            {
                foreach (DataRow card in cards.Rows)
                {
                    var name = card[3].ToString();
                    var rarity = card[7];
                    var inventory = card[20];
                    var foilInventory = card[22];
                    var price = card[19];
                    var foilPrice = card[21];

                    if (card == cards.Rows[cards.Rows.Count - 1])
                    {
                        pictureBox_Card.Image = GetImageForCard(name, set, Int32.Parse(card[17].ToString()));
                    }

                    dataGridView_CardData.Rows.Add(name, set, rarity, inventory, foilInventory, price, foilPrice);
                }
            }
        }
        int GetMTGStocksID(string name)
        {
            JObject json = null;

            var nameWithoutWhitespace = Regex.Replace(name, @"\s+", "%20");
            var url = MTGSTOCKS_QUERY_ID + nameWithoutWhitespace;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.Proxy = null;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var html = reader.ReadToEnd();
                    html = Regex.Replace(html, @"[\[\]']+", "");

                    try
                    {
                        json = JObject.Parse(html);
                    }
                    catch(Exception ex)
                    {
                        var htmlArray = html.Split('{');
                        var targetString = "\"name\":\"" + name + "\"";
                        foreach(var htmlChunk in htmlArray)
                        {
                            if (htmlChunk.Contains(targetString))
                            {
                                var idString = htmlChunk.Replace(targetString, "");
                                idString = idString.Replace("\"", "");
                                idString = idString.Replace(",", "");
                                idString = idString.Replace("}", "");
                                return Int32.Parse(idString.Split(':').Last());
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                //TODO: LOGGING
                logger.LogError("Failed to get IDs for card: " + name);
            }

            if(json != null)
            {
                return json["id"].ToObject<int>();
            }

            return -1;
        }
        JObject GetMTGStocksData(int id)
        {
            var url = MTGSTOCKS_QUERY_DATA + id;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var html = reader.ReadToEnd();
                    return JObject.Parse(html);
                }
            }
            catch (Exception ex)
            {
                //TODO: LOGGING
                logger.LogError("Failed to get data for card with MTG Stocks ID: " + id + "\r\nURL: " + url);
                return null;
            }
        }
        JToken ParseCardData(JObject cardData, string set = "")
        {
            if (set == "")
            {
                return cardData;
            }

            JArray sets = JArray.Parse(cardData["sets"].ToString());
            if (sets.Count == 0)
            {
               return cardData;
            }

            foreach (var card in sets)
            {
                try
                {
                    if (card["card_set"]["name"].ToString() == set)
                        return card;
                    
                }
                catch (Exception ex)
                {
                    try
                    {
                        if(card["set_name"].ToString() == set)
                            return card;
                    }
                    catch(Exception exNested)
                    {
                        return cardData;
                    }
                    return cardData;
                }
            }

            return null;
            //JArray jArray = new JArray();

            //JArray sets = JArray.Parse(cardData["sets"].ToString());
            //if (sets.Count == 0)
            //{
            //    jArray.Add(cardData);
            //}
            //else //TODO: Something different than this/else
            //{
            //    foreach (var card in sets)
            //    {
            //        jArray.Add(GetMTGStocksData(card["id"].ToObject<int>()));
            //    }

            //    jArray.Add(GetMTGStocksData(cardData["id"].ToObject<int>()));
            //}
            
            //return jArray;
        }

        //IMAGE
        void GetImageForCard(int multiverseID)
        {
            try
            {
                pictureBox_Card.Image = Image.FromFile(LOCAL_IMAGE_PATH + multiverseID.ToString() + IMAGE_TYPE);
            }
            catch(Exception ex)
            {
                logger.LogError("Failed to load image from file, will attempt to from URL. Multiverse ID: " + multiverseID);
                LoadImageFromURL(multiverseID);
            }
        }
        void LoadImageFromURL(int multiverseID)
        {
            var url = GATHERER_IMAGE_URL.Replace("ABCDE", multiverseID.ToString());
            try
            {
                var webClient = new WebClient();
                byte[] imageBytes = webClient.DownloadData(url);

                using (var ms = new MemoryStream(imageBytes))
                {
                    pictureBox_Card.Image = Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to load image from URL, using back of MtG card. Multiverse ID: " + multiverseID.ToString());
                pictureBox_Card.Image = Properties.Resources.Magic_card_back;
            }
        }

        //INVENTORY
        void AddOneToInventory(string card, string set, bool foil)
        {
            //TODO: Do I like this single line function?
            dbMan.AddOneToInventory(card, set, foil);
        }
        void RemoveOneFromInventory(string card, string set, bool foil)
        {
            dbMan.RemoveOneToInventory(card, set, foil);
        }
        int GetInventory(string card, string set)
        {
            return dbMan.GetInventory(card, set);
        }
        int GetFoilInventory(string card, string set)
        {
            return dbMan.GetFoilInventory(card, set);
        }
        //TODO: This in Python?
        void AddNewSet(string file)
        {
            try
            {
                string setData = File.ReadAllText(file);
                JObject setJSON = JObject.Parse(setData);
                AddExpansionToDatabase(setJSON["cards"], setJSON["name"].ToString(), setJSON["code"].ToString());
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding set to database, file: " + file);
            }
        }
        void AddExpansionToDatabase(JToken cardList, string expansion, string abbreviation)
        {
            if (!dbMan.CheckIfSetExists(expansion))
            {
                dbMan.AddNewSet(expansion, abbreviation, null);
                logger.LogActivity("Adding new set: " + expansion + " to databse");
            }
            foreach (var card in cardList)
            {
                try
                {
                    string multiverseID = PrepareString(card, "multiverseid");
                    if (dbMan.CheckIfCardExists(multiverseID))
                        continue;

                    string layout = PrepareString(card, "layout");
                    string cardID = PrepareString(card, "id");
                    string name = PrepareString(card, "name");
                    string manaCost = PrepareString(card, "manaCost");
                    int cmc = card["cmc"].ToObject<int>();
                    string colours = PrepareString(card, "colors");
                    string rarity = PrepareString(card, "rarity");
                    string type = PrepareString(card, "type");
                    string types = PrepareString(card, "types");
                    string subtypes = PrepareString(card, "subtypes");
                    string text = PrepareString(card, "text");
                    string flavourText = PrepareString(card, "flavor");
                    string power = PrepareString(card, "power");
                    string toughness = PrepareString(card, "toughness");
                    string imageName = PrepareString(card, "imageName");
                    string colourIdentity = PrepareString(card, "colorIdentity");
                    float price = -1;
                    int inventory = 0;
                    float foilPrice = -1;
                    int foilInventory = 0;

                    try
                    {
                        dbMan.AddNewCard(layout, cardID, name, manaCost, cmc, colours, rarity, type, types, subtypes, text, flavourText, power, toughness, imageName, colourIdentity, multiverseID, expansion, price, inventory, foilPrice, foilInventory);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("Error adding card: " + name + " in set: " + expansion + "\r\n\r\nError message:" + ex.ToString());
                    }
                }
                catch(Exception ex)
                {
                    //TODO: THROW MY OWN EXCEPTIONS
                    logger.LogError("Error parsing card: " + PrepareString(card, "name") + " in set: " + expansion  + "\r\n\r\nError message:" + ex.ToString());
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
                //logger.LogError("Failed to parse data: " + index + " for card: " + card["name"].ToString());
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

        private void dataGridView_CardData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dataGridView_CardData.Rows.Count == 0 || dataGridView_CardData.CurrentCell == null)
            {
                return;
            }
            var dgvIndex = dataGridView_CardData.CurrentCell.RowIndex;
            try
            {
                var card = dataGridView_CardData.Rows[dgvIndex].Cells[0].Value.ToString();
                var set = dataGridView_CardData.Rows[dgvIndex].Cells[1].Value.ToString();

                pictureBox_Card.Image = GetImageForCard(card, set);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
