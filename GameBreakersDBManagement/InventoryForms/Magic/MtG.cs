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
    public partial class MtG : Form
    {
        DatabaseManager dbMan;
        Logger logger;
        Thread bgThread;

        static string MTGSTOCKS_QUERY_ID = @"https://api.mtgstocks.com/search/autocomplete/";
        static string MTGSTOCKS_QUERY_DATA = @"https://api.mtgstocks.com/prints/";
        static string GATHERER_IMAGE_URL = @"http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=ABCDE&type=card";
        
        delegate void AddCardToRowDelegate(Dictionary<string, object> cardData);
        //TODO: Online card and price search in background thread? Then it just adds rows as it completes

        public MtG()
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

            BackgroundDataManager bgman = new BackgroundDataManager();
            //bgman.Run();
            bgThread = new Thread(new ThreadStart(bgman.Run));
            bgThread.Start();

            var applicationPath = @"C:\GameBreakersInventory\Application\";
            var imageAppPath = applicationPath + @"mtg-image-fetcher\application.py";
            var priceAppPath = applicationPath + @"mtg-price-fetcher\application.py";
            run_cmd(@"C:\GameBreakersInventory\Application\startFetchers.bat");
        }

        private void run_cmd(string command)
        {
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.WorkingDirectory = @"C:\GameBreakersInventory";
            processInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processInfo.UseShellExecute = false;

            process = Process.Start(processInfo);
            process.WaitForExit();

            process.Close();
        }

        //BUTTONS
        private void button_Search_Click(object sender, EventArgs e)
        {
            //TODO: Call UpdateDisplays in these?
            dataGridView_CardData.Rows.Clear();
            try
            {
                new Thread(() =>
                {
                    SearchForCard(textBox_Name.Text);
                }).Start();
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
            SetEditorForm setEditor = new SetEditorForm();
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
                    var set = card[18].ToString().ToString();
                    var rarity = card[7].ToString();
                    var inventory = float.Parse(card[20].ToString());
                    var foilInventory = float.Parse(card[22].ToString());
                    var price = float.Parse(card[19].ToString());
                    var foilPrice = float.Parse(card[21].ToString());

                    if (price < 0)
                    {
                        var newPrice = GetPrice(name, card[18].ToString(), false);
                        if (newPrice != -1)
                        {
                            price = newPrice;
                            dbMan.UpdatePrice(name, set, float.Parse(price.ToString()), false);
                        }
                    }
                    if (foilPrice < 0)
                    {
                        var newFoilPrice = GetPrice(name, card[18].ToString(), true);
                        if (newFoilPrice != -1)
                        {
                            foilPrice = newFoilPrice;
                            dbMan.UpdatePrice(name, set, float.Parse(foilPrice.ToString()), true);
                        }
                    }

                    if (card == cards.Rows[cards.Rows.Count-1])
                    {
                        GetImageForCard(Int32.Parse(card[17].ToString()));
                    }

                    name = card[3].ToString();
                    AddCardToRow(new Dictionary<string, object> {
                        { "name", name },
                        { "set", set },
                        { "rarity", rarity },
                        { "inventory", inventory },
                        { "foilInventory", foilInventory },
                        { "price", price },
                        { "foilPrice", foilPrice } });
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
                        var tokenName = jObject["name"].ToString();
                        var tokenSet = jObject["card_set"]["name"].ToString();
                        var cardRarity = jObject["rarity"].ToString();
                        var cardPrice = float.Parse(jObject["latest_price"]["avg"].ToString());
                        var cardFoilPrice = 0f;
                        var cardStrprice = jObject["latest_price"]["foil"].ToString(); //TODO: Market price? [market]
                        if (cardStrprice == "") cardFoilPrice = 0;
                        else cardFoilPrice = float.Parse(cardStrprice);
                        AddCardToRow(new Dictionary<string, object> {
                                { "name", tokenName },
                                { "set", tokenSet },
                                { "rarity", cardRarity },
                                { "inventory", 0 },
                                { "foilInventory", 0 },
                                { "price", cardPrice },
                                { "foilPrice", cardFoilPrice } });
                    }
                }

                var cardName = card["name"].ToString();
                var set = card["card_set"]["name"].ToString();
                var rarity = card["rarity"].ToString();
                var price = float.Parse(card["latest_price"]["avg"].ToString());
                var foilPrice = 0f;
                var strprice = card["latest_price"]["foil"].ToString(); //TODO: Market price? [market]
                if (strprice == "") foilPrice = 0;
                else foilPrice = float.Parse(strprice);
                AddCardToRow(new Dictionary<string, object> {
                        { "name", cardName },
                        { "set", set },
                        { "rarity", rarity },
                        { "inventory", 0 },
                        { "foilInventory", 0 },
                        { "price", price },
                        { "foilPrice", foilPrice } });

                GetImageForCard(dbMan.GetMultiverseID(cardName, set));
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
                    var price = float.Parse(card[19].ToString());
                    var foilPrice = float.Parse(card[21].ToString());

                    if (price < 0)
                    {
                        var newPrice = GetPrice(name, card[18].ToString(), false);
                        if (newPrice != -1)
                        {
                            price = newPrice;
                            dbMan.UpdatePrice(name, set, float.Parse(price.ToString()), false);
                        }
                    }
                    if (foilPrice < 0)
                    {
                        var newFoilPrice = GetPrice(name, card[18].ToString(), true);
                        if (newFoilPrice != -1)
                        {
                            foilPrice = newFoilPrice;
                            dbMan.UpdatePrice(name, set, float.Parse(foilPrice.ToString()), true);
                        }
                    }

                    if (card == cards.Rows[cards.Rows.Count - 1])
                    {
                        GetImageForCard(Int32.Parse(card[17].ToString()));
                    }
                    
                    AddCardToRow(new Dictionary<string, object> {
                        { "name", name },
                        { "set", set },
                        { "rarity", rarity },
                        { "inventory", inventory },
                        { "foilInventory", foilInventory },
                        { "price", price },
                        { "foilPrice", foilPrice } });
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
                            
                            if (htmlChunk.ToLower().Contains(targetString.ToLower()))
                            {
                                var idString = htmlChunk.ToLower().Replace(targetString, "");
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
            if (set == "" || cardData["card_set"]["name"].ToString() == set)
            {
                return cardData;
            }

            JArray sets = JArray.Parse(cardData["sets"].ToString());
            if (sets.Count == 0)
            {
               return cardData;
            }

            set = PrepString(set);

            foreach (var card in sets)
            {
                try
                {
                    var cardSet = card["card_set"]["name"].ToString();
                    if (cardSet.Contains("Magic 2"))
                    {
                        cardSet = cardSet.Split(new string[] { " (" }, StringSplitOptions.None).First();
                    }
                    if (cardSet == set)
                        return card;
                    
                }
                catch (Exception ex)
                {
                    try
                    {
                        var cardSet = card["set_name"].ToString();
                        if (cardSet.Contains("Magic 2"))
                        {
                            cardSet = cardSet.Split(new string[] { " (" }, StringSplitOptions.None).First();
                        }
                        if (cardSet == set)
                            return card;
                    }
                    catch(Exception exNested)
                    {
                        return cardData;
                    }
                }
            }
            return null;
        }
        float GetPrice(string name, string set, bool foil)
        {
            float price = -1;

            var id = GetMTGStocksID(name);
            if (id == -1)
            {
                //FAIL
                return -1;
            }

            var cardObject = GetMTGStocksData(id);
            if (cardObject == null)
            {
                //FAIL
                return -1;
            }

            var card = ParseCardData(cardObject, set);
            if (card == null)
            {
                //FAIL
                return -1;
            }


            if (!foil)
                try {
                    price = float.Parse(card["latest_price"]["avg"].ToString());    //TODO: Market price?
                }
                catch(Exception ex)
                {
                    price = float.Parse(card["latest_price"].ToString());
                }
            else
            {
                try
                {
                    var strprice = card["latest_price"]["foil"].ToString(); //TODO: Market price? [market]
                    if (strprice == "") price = 0;
                    else price = float.Parse(strprice);
                }
                catch(Exception ex)
                {
                    try
                    {
                        var nestedCardObject = GetMTGStocksData(Int32.Parse(card["id"].ToString()));
                        var nestedCardData = ParseCardData(nestedCardObject);

                        var strprice = nestedCardData["latest_price"]["foil"].ToString(); //TODO: Market price? [market]
                        if (strprice == "") price = 0;
                        else price = float.Parse(strprice);
                    }
                    catch(Exception exNested)
                    {
                        //No foil
                        price = 0;
                    }
                }
            }

            dbMan.UpdatePrice(name, set, price, foil);
            logger.LogActivity("Updating price of card:\r\nFoil: " + foil + "\r\nCard Name: " + name + "\r\nSet: " + set + "\r\nPrice: " + price);
            return price;
        }

        //UTIL
        string PrepString(string dataStr)
        {
            var loweredSet = dataStr.ToLower();
            var finalName = dataStr;

            if (loweredSet.Contains("fourth"))
            {
                finalName = dataStr.Replace("Fourth", "4th");
            }
            else if (loweredSet.Contains("fifth"))
            {
                finalName = dataStr.Replace("Fifth", "5th");
            }
            else if (loweredSet.Contains("sixth"))
            {
                finalName = dataStr.Replace("Sixth", "6th");
            }
            else if (loweredSet.Contains("seventh"))
            {
                finalName = dataStr.Replace("Seventh", "7th");
            }
            else if (loweredSet.Contains("eighth"))
            {
                finalName = dataStr.Replace("Eighth", "8th");
            }
            else if (loweredSet.Contains("ninth"))
            {
                finalName = dataStr.Replace("Ninth", "9th");
            }
            else if (loweredSet.Contains("tenth"))
            {
                finalName = dataStr.Replace("Tenth", "10th");
            }
            else if (loweredSet.Contains("alpha"))
            {
                finalName = dataStr.Replace("Limited Edition Alpha", "Alpha Edition");
            }
            else if (loweredSet.Contains("beta"))
            {
                finalName = dataStr.Replace("Limited Edition Beta", "Beta Edition");
            }
            else if (loweredSet.Contains("modern masters"))
            {
                finalName = dataStr.Replace(" Edition", "");
            }

            return finalName;
        }
        string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        private void textBox_Name_GotFocus(object sender, EventArgs e)
        {
            AcceptButton = button_Search;
        }
        private void textBox_Set_GotFocus(object sender, EventArgs e)
        {
            AcceptButton = button_SearchSet;
        }
        void AddCardToRow(Dictionary<string, object> cardData)
        {
            if(dataGridView_CardData.InvokeRequired)
            {
                AddCardToRowDelegate addCardToRowDelegate = new AddCardToRowDelegate(AddCardToRow);
                Invoke(addCardToRowDelegate, new object[] { cardData });
                return;
            }

            var name            = cardData["name"];
            var set             = cardData["set"];
            var rarity          = cardData["rarity"];
            var inventory       = cardData["inventory"];
            var foilInventory   = cardData["foilInventory"];
            var price           = cardData["price"];
            var foilPrice       = cardData["foilPrice"];

            dataGridView_CardData.Rows.Add(name, set, rarity, inventory, foilInventory, price, foilPrice);
        }

        //IMAGE
        void GetImageForCard(int multiverseID)
        {
            try
            {
                LoadImageFromURL(multiverseID);
            }
            catch(Exception ex)
            {
                logger.LogError("Failed to load image from file, will attempt to from URL. Multiverse ID: " + multiverseID);
            }
        }
        //TODOL Get rid of this?
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
                //TODO: ADD SET ID
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

                //TODO:: Get ID from name and set
                GetImageForCard(dbMan.GetMultiverseID(card, set));
            }
            catch(Exception ex)
            {

            }
        }
    }
}
