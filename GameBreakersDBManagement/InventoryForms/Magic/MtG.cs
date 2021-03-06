﻿using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Configuration;

namespace GameBreakersDBManagement
{
    public partial class MtG : Form
    {
        //TODO: Merge the MtG search into this?
        //TODO: Fix all errors caused by adding a new set
        //TODO: General refactoring
        static string MTGSTOCKS_QUERY_ID   = @"https://api.mtgstocks.com/search/autocomplete/";
        static string MTGSTOCKS_QUERY_DATA = @"https://api.mtgstocks.com/prints/";
        static string GATHERER_IMAGE_URL   = @"http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=ABCDE&type=card";
        static string MTGJSON_URL          = @"https://mtgjson.com/sets.html";
        static string MTGJSON_DATA_URL     = @"https://mtgjson.com/json/SET-x.json";

        Thread SearchThread;
        Thread ImageThread;

        delegate void AddCardToRowDelegate(Dictionary<string, object> cardData);
        delegate void SearchCompleteDelegate();
        delegate void UpdatePriceOnGUIDelegate(int row, float price, bool foil);

        public MtG()
        {
            InitializeComponent();
            LoadCarts();
            SearchThread = new Thread(BeginSearchThread);
            ImageThread = new Thread(GetImageForSelectedCard);
            new Thread(() =>
            {
                CheckForNewSets();
                //TODO: Fetch by set for MtGStocks
                //TODO: Save the MTG Stocks ID to the database
                //BeginPriceUpdate();
            }).Start();

            //WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
        }

        //BUTTONS
        private void button_Search_Click(object sender, EventArgs e)
        {
            dataGridView_CardData.Rows.Clear();
            try
            {
                if (SearchThread.IsAlive) SearchThread.Abort();
                SearchThread = new Thread(BeginSearchThread);
                SearchThread.Start();                
            }
            catch (Exception ex)
            {
                Logger.LogError("Attempting to search for MtG card", ex.Message, textBox_Name.Text);
            }
        }
        private void button_SearchSet_Click(object sender, EventArgs e)
        {
            //TODO: abbreviations search
            dataGridView_CardData.Rows.Clear();
            try
            {
                new Thread(() =>
                {
                    SearchForSet(textBox_Set.Text);
                    GetImageForFirstCard();
                    SearchComplete();
                }).Start();
            }
            catch (Exception ex)
            {
                Logger.LogError("Attempting to search for MtG set", ex.Message, textBox_Set.Text);
            }
        }
        private void button_EditSet_Click(object sender, EventArgs e)
        {
            
        }
        
        //SEARCH
        void BeginSearchThread()
        {
            var searchParam = Regex.Replace(textBox_Name.Text, "[^0-9a-zA-Z ]+", "");
            var cardFound = SearchForCard(searchParam);

            if (!cardFound)
                return;

            GetImageForFirstCard();
            SearchComplete();
        }
        bool SearchForCard(string name)
        {
            if (!SearchDatabaseForCard(name))
                return SearchOnlineForCard(name);

            return true;
        }
        bool SearchDatabaseForCard(string name)
        {
            var nameArray = name.Contains(" ") ? name.Split(' ') : new string[] { name };
            var cards = DatabaseManager.GetMagicCard(nameArray);

            if (cards.Rows.Count > 0)
            {
                foreach (DataRow card in cards.Rows)
                {
                    name = card["name"].ToString();

                    var set = card["expansion"].ToString();
                    var rarity = card["rarity"].ToString();
                    var colour = Convert.ToString(card["colorIdentity"]) == "" ? "C" : card["colorIdentity"];
                    var inventory = float.Parse(card["inventory"].ToString());
                    var foilInventory = float.Parse(card["foilInventory"].ToString());
                    var price = float.Parse(card["price"].ToString());
                    var foilPrice = float.Parse(card["foilPrice"].ToString());
                    var lastUpdated = card["priceLastUpdated"];

                    AddCardToRow(new Dictionary<string, object>
                    {
                        { "name", name },
                        { "set", set },
                        { "rarity", rarity },
                        { "colour", colour },
                        { "inventory", inventory },
                        { "foilInventory", foilInventory },
                        { "price", price },
                        { "foilPrice", foilPrice },
                        { "lastUpdated", lastUpdated }
                    });
                }
                return true;
            }

            return false;
        }
        bool SearchOnlineForCard(string name)
        {
            //TODO: Add cards found here to DB
            var id = GetMTGStocksID(name);
            if (id == -1)
            {
                Logger.LogError("Searching MTG Stocks for MtG card by name", "No card found for name: " + name, null);
                return false;
            }

            var cardObject = GetMTGStocksData(id, name, "NA");
            if (cardObject == null)
            {
                Logger.LogError("Searching MTG Stocks for MtG card by ID", "No card named " + name + " found for ID: " + id, null);
                return false;
            }

            var card = ParseCardData(cardObject);
            if (card == null)
            {
                Logger.LogError("Attempting to parse MTG Stocks data", "Failed to parse data", cardObject.ToString());
                return false;
            }

            if (card == cardObject)
            {
                JArray sets = JArray.Parse(card["sets"].ToString());

                foreach (var cardSet in sets)
                {
                    JObject jObject = GetMTGStocksData(Int32.Parse(cardSet["id"].ToString()), name, "NA");
                    var tokenName = jObject["name"].ToString();
                    var tokenSet = jObject["card_set"]["name"].ToString();
                    var cardRarity = jObject["rarity"].ToString();
                    var cardPrice = float.Parse(jObject["latest_price"]["avg"].ToString());
                    var colour = "";    //TODO: Maybe parse the colour out of cost?
                    var cardFoilPrice = 0f;
                    var cardStrprice = jObject["latest_price"]["foil"].ToString(); //TODO: Market price? [market]

                    if (cardStrprice == "") cardFoilPrice = 0;
                    else cardFoilPrice = float.Parse(cardStrprice);

                    AddCardToRow(new Dictionary<string, object>
                    {
                            { "name", tokenName },
                            { "set", tokenSet },
                            { "rarity", cardRarity },
                            { "colour", colour },
                            { "inventory", 0 },
                            { "foilInventory", 0 },
                            { "price", cardPrice },
                            { "foilPrice", cardFoilPrice },
                            { "lsatUpdated", DateTimeOffset.Now.ToUnixTimeMilliseconds() }
                    });
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

            AddCardToRow(new Dictionary<string, object>
            {
                    { "name", cardName },
                    { "set", set },
                    { "rarity", rarity },
                    { "colour", "" },   //TODO: See above
                    { "inventory", 0 },
                    { "foilInventory", 0 },
                    { "price", price },
                    { "foilPrice", foilPrice },
                    { "lastUpdated", DateTimeOffset.Now.ToUnixTimeMilliseconds() }
            });

            return true;
        }
        void SearchForSet(string set)
        {
            var cards = DatabaseManager.GetAllCardsForSet(set);
            if (cards.Rows.Count > 0)
            {
                foreach (DataRow card in cards.Rows)
                {
                    var name = card["name"].ToString();
                    var expansion = card["expansion"];
                    var rarity = card["rarity"];
                    var colour = Convert.ToString(card["colorIdentity"]) == "" ? "C" : card["colorIdentity"];
                    var inventory = card["inventory"];
                    var foilInventory = card["foilInventory"];
                    var price = float.Parse(card["price"].ToString());
                    var foilPrice = float.Parse(card["foilPrice"].ToString());
                    var lastUpdated = card["priceLastUpdated"];

                    AddCardToRow(new Dictionary<string, object>
                    {
                        { "name", name },
                        { "set", expansion },
                        { "colour", colour },
                        { "rarity", rarity },
                        { "inventory", inventory },
                        { "foilInventory", foilInventory },
                        { "price", price },
                        { "foilPrice", foilPrice },
                        { "lastUpdated", lastUpdated }
                    });
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

                    if(html == "null")
                    {
                        Logger.LogError("Attempting to get IDs for card: " + name, "HTML returned was null", url);
                        return -1;
                    }

                    html = Regex.Replace(html, @"[\[\]']+", "");

                    try
                    {
                        json = JObject.Parse(html);
                    }
                    catch (Exception ex)
                    {
                        var htmlArray = html.Split('{');
                        var targetString = "\"name\":\"" + name.Replace("'", String.Empty);// + "\""; TODO: Add this back in, figure something else out for split cards
                        for(int i = 0; i < htmlArray.Count(); i++)
                        {
                            var htmlChunk = RemoveDiacritics(htmlArray[i]);

                            if (htmlChunk.Contains("\\\""))
                                htmlChunk = htmlChunk.Replace("\\\"", "\"");
                            if (htmlChunk.Contains("..."))
                                htmlChunk = htmlChunk.Replace("...", String.Empty);
                            if (htmlChunk.ToLower().Contains(targetString.ToLower()))
                            {
                                //var idString = htmlChunk.Split(new string[] { "\"name\":\"" }, StringSplitOptions.None).First();
                                var idString = htmlChunk.ToLower().Replace(targetString.ToLower(), "");
                                idString = idString.Split(',').First();
                                idString = idString.Replace("\"", "");
                                idString = idString.Replace("}", "");
                                return Int32.Parse(idString.Split(':').Last());
                            }
                        }

                        Logger.LogError("Attempting to get IDs for card: " + name, "No match was found", url);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Attempting to get IDs for card: " + name, ex.Message, url);
            }

            if (json != null)
            {
                return json["id"].ToObject<int>();
            }

            return -1;
        }
        JObject GetMTGStocksData(int id, string name, string set)
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
                Logger.LogError("Attempting to get data for card with MTG Stocks ID", ex.Message, "ID: " + id + " | Card Name: " + name + " | Expansion: " + set + " | URL: " + url);
                return null;
            }
        }
        JToken ParseCardData(JObject cardData, string set = "")
        {
            var cardSetName = cardData["card_set"]["name"].ToString();
            if (cardSetName.Contains("Magic 2"))
                cardSetName = cardSetName.Split(new string[] { " (" }, StringSplitOptions.None).First();
            if (set == "" || cardSetName == set)
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
                    catch (Exception exNested)
                    {
                        return cardData;
                    }
                }
            }

            Logger.LogError("Attempting to find a match for card with MTG Stocks data", "No match found", "Set: " + set + " | Name: " + cardData["name"]);// + " | Card Data: " + cardData.ToString());

            return null;
        }
        void SearchComplete()
        {
            if (InvokeRequired)
            { 
                try
                {
                    SearchCompleteDelegate searchCompleteDelegate = new SearchCompleteDelegate(SearchComplete);
                    Invoke(searchCompleteDelegate);
                }
                catch(Exception ex)
                {

                }
                return;
            }

            UpdatePriceOnList();
        }
        void UpdatePriceOnList()
        {
            for (int i = 0; i < dataGridView_CardData.Rows.Count - 1; i++)
            {
                if (!SearchThread.IsAlive)
                    return;

                var name        = dataGridView_CardData.Rows[i].Cells["CardName"].Value.ToString();
                var set         = dataGridView_CardData.Rows[i].Cells["Expansion"].Value.ToString();
                var price       = float.Parse(dataGridView_CardData.Rows[i].Cells["Price"].Value.ToString());
                var foilPrice   = float.Parse(dataGridView_CardData.Rows[i].Cells["FoilPrice"].Value.ToString());
                var lastUpdated = dataGridView_CardData.Rows[i].Cells["TimeLastUpdated"].Value;
                
                ThreadPool.QueueUserWorkItem(UpdatePrice, new object[] { i, name, set, price, foilPrice, lastUpdated });
            }
        }

        //PRICE
        void BeginPriceUpdate()
        {
            //TODO: Deal with the transformed version of cards better
            //TODO: Currently, B.F.M. (Big Furry Monster) is getting parsed to B.F.M. I have to fix this
            double days = Convert.ToDouble(Properties.Settings.Default["DaysBetweenPriceCheck"]);
            var time = DateTimeOffset.Now.AddDays(-days).ToUnixTimeMilliseconds().ToString();
            var query = "select name, expansion, types, layout from MtG where ((price < 0 or foilPrice < 0 or priceLastUpdated < " + time + " ) and onlineOnlyVersion = 0 and layout != 'double-faced' and layout != 'meld')";
            var cardsToUpdate = DatabaseManager.RunQuery(query);

            for(int i = 0; i < cardsToUpdate.Rows.Count; i++)
            {
                var card = cardsToUpdate.Rows[i];
                                
                var prices = GetPrice(card);

                DatabaseManager.UpdatePrice(card[0].ToString(), card[1].ToString(), prices["price"], false);
                DatabaseManager.UpdatePrice(card[0].ToString(), card[1].ToString(), prices["foilPrice"], true);
            }
        }
        Dictionary<string, float> GetPrice(DataRow cardData)
        {
            var name = cardData[0].ToString();
            var set = cardData[1].ToString();
            var types = cardData[2].ToString();
            var layout = cardData[3].ToString();

            name = PrepareNameString(name, set, types, layout);
            set = PrepareSetString(set);
            Dictionary<string, float> prices = new Dictionary<string, float> { { "price", -1 }, { "foilPrice", -1 } };

            var id = GetMTGStocksID(name);
            if (id == -1)
            {
                //FAIL
                return prices;
            }

            var cardObject = GetMTGStocksData(id, name, set);
            if (cardObject == null)
            {
                //FAIL
                return prices;
            }

            var card = ParseCardData(cardObject, set);
            if (card == null)
            {
                //FAIL
                return prices;
            }

            try
            {
                prices["price"] = float.Parse(card["latest_price"]["avg"].ToString());    //TODO: Market price?
            }
            catch (Exception ex)
            {
                prices["price"] = float.Parse(card["latest_price"].ToString());
                if(prices["price"] == 0) prices["price"] = float.Parse(card["latest_price_mkm"].ToString());
            }
            
            try
            {
                var strprice = card["latest_price"]["foil"].ToString(); //TODO: Market price? [market]
                if (strprice == "") prices["foilPrice"] = 0;
                else prices["foilPrice"] = float.Parse(strprice);
            }
            catch (Exception ex)
            {
                try
                {
                    var nestedCardObject = GetMTGStocksData(Int32.Parse(card["id"].ToString()), name, set);
                    var nestedCardData = ParseCardData(nestedCardObject);

                    var strprice = nestedCardData["latest_price"]["foil"].ToString(); //TODO: Market price? [market]
                    if (strprice == "") prices["foilPrice"] = 0;
                    else prices["foilPrice"] = float.Parse(strprice);
                }
                catch (Exception exNested)
                {
                    //No foil
                    prices["foilPrice"] = 0;
                }
            }

            if (types.ToLower().Contains("land") && prices["price"] == -1f)
            {
                name = name.Split(new string[] { " - " }, StringSplitOptions.None).First();
                prices = GetPrice(cardData);
            }

            if (types.ToLower().Contains("plane") && prices["price"] == -1f)
            {
                set = "Oversize Cards";
                prices = GetPrice(cardData);
            }

            Logger.LogActivity("Updating price of card:" + " | Card Name: " + name + " | Set: " + set + " | Price: " + prices["price"] + " | Foil Price: " + prices["foilPrice"]);
            return prices;
        }
        void UpdatePrice(object state)
        {
            if (!SearchThread.IsAlive)
                return;

            object[] args   = state as object[];
            var i           = Int32.Parse(args[0].ToString());
            var name        = args[1].ToString().Replace("'", "''");
            var set         = args[2].ToString().Replace("'", "''");
            var price       = float.Parse(args[3].ToString());
            var foilPrice   = float.Parse(args[4].ToString());
            var lastupdated = long.Parse(args[5].ToString());

            double days = Convert.ToDouble(Properties.Settings.Default["DaysBetweenPriceCheck"]);
            var oldTIme = DateTimeOffset.FromUnixTimeMilliseconds(lastupdated).AddDays(days).ToUnixTimeMilliseconds();
            
            if (price < 0 || foilPrice < 0 || oldTIme < DateTimeOffset.Now.ToUnixTimeMilliseconds())
            {
                var query = "select name, expansion, types, layout from MtG where (name = '" + name + "' and expansion = '" + set + "')";
                var card = DatabaseManager.RunQuery(query).Rows[0];
                var newPrices = GetPrice(card);
                if (newPrices["price"] != -1)
                {
                    price = newPrices["price"];
                    DatabaseManager.UpdatePrice(name, set, price, false);
                    UpdatePriceOnGUI(i, price, false);
                }
                if (newPrices["foilPrice"] != -1)
                {
                    foilPrice = newPrices["foilPrice"];
                    DatabaseManager.UpdatePrice(name, set, foilPrice, true);
                    UpdatePriceOnGUI(i, price, true);
                }
            }
        }
        void UpdatePriceOnGUI(int row, float price, bool foil)
        {
            if (dataGridView_CardData.InvokeRequired)
            {
                try
                {
                    UpdatePriceOnGUIDelegate updateDel = new UpdatePriceOnGUIDelegate(UpdatePriceOnGUI);
                    Invoke(updateDel, new object[] { row, price, foil });
                }
                catch(Exception ex)
                {

                }
                return;
            }

            if (!SearchThread.IsAlive || dataGridView_CardData.Rows.Count <= 1)
                return;

            if (!foil)   dataGridView_CardData.Rows[row].Cells["Price"].Value = price;
            else         dataGridView_CardData.Rows[row].Cells["FoilPrice"].Value = price;
        }
        string CurrencyConversion(decimal amount, string fromCurrency, string toCurrency)
        {
            amount = amount * Convert.ToDecimal(Properties.Settings.Default["ExchangeRate"]);
            amount = Decimal.Round(amount, 2);
            return amount.ToString();

            string urlPattern = "http://www.xe.com/ucc/convert.cgi?template=mobile&Amount={0}&From={1}&To={2}";
            string url = string.Format(urlPattern, amount, fromCurrency, toCurrency);

            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                webReq.CookieContainer = new CookieContainer();
                webReq.Method = "GET";
                using (WebResponse response = webReq.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        var html = reader.ReadToEnd();

                        var doc = new HtmlAgilityPack.HtmlDocument();
                        doc.LoadHtml(html);

                        var converted = doc.DocumentNode.SelectNodes("//td[@class='resultColRght']");

                        foreach(var item in converted)
                        {
                            if(item.InnerText.ToLower().Contains(toCurrency.ToLower()))
                            {
                                var price = Convert.ToDecimal(item.InnerText.Split(' ').First());
                                price = Decimal.Round(price, 2);
                                return price.ToString();
                            }
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Fetching price conversion data from url.", ex.ToString(), url);
            }

            return "";
        }

        //UTIL
        string PrepareSetString(string dataStr)
        {
            var loweredSet = dataStr.ToLower();
            var finalName = dataStr;

            if(loweredSet.Contains("magic: the gathering"))
            {
                if(loweredSet.Contains('—'))
                    finalName = dataStr.Split(new string[] { "Magic: The Gathering—" }, StringSplitOptions.None).Last();
                else 
                    finalName = dataStr.Split(new string[] { "Magic: The Gathering-" }, StringSplitOptions.None).Last();
            }
            else if(loweredSet.Contains("magic 2"))
            {
                var splitName = finalName.Split(' ');
                finalName = splitName[0] + " " + splitName[1];
            }
            else if (loweredSet.Contains("fourth"))
            {
                finalName = dataStr;//.Replace("Fourth", "4th");
            }
            else if (loweredSet.Contains("fifth"))
            {
                finalName = dataStr;//.Replace("Fifth", "5th");
            }
            else if (loweredSet.Contains("sixth"))
            {
                if(loweredSet != "classic sixth edition")
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
            else if (loweredSet.Contains("commander") && loweredSet.Contains("edition"))
            {
                finalName = dataStr.Replace(" Edition", "");
            }
            else if(loweredSet.Contains("time spiral \"timeshifted\""))
            {
                finalName = "Timeshifted";
            }
            else if(loweredSet.Contains("ravnica: city of guilds"))
            {
                finalName = "Ravnica";
            }
            else if(loweredSet.Contains("modern event deck 2014"))
            {
                finalName = "Modern Event Deck";
            }
            else if (loweredSet.Contains("media inserts"))
            {
                finalName = "Media Promos";
            }
            else if(loweredSet.Contains("''"))
            {
                finalName = dataStr.Replace("''", "'");
            }
            else if(loweredSet.Contains("("))
            {
                finalName = dataStr.Split(new string[] { " (" }, StringSplitOptions.None).First();
            }
            else if(loweredSet.Contains("planechase"))
            {
                if (loweredSet.Contains("anthology"))
                {
                    finalName = "Oversize Cards";
                }
                else
                {
                    finalName = dataStr.Replace(" Edition", String.Empty);
                }
            }
            else if (loweredSet.Contains("duel decks"))
            {
                if(loweredSet.Contains("anthology"))
                {
                    finalName = "Duel Decks: Anthology";
                }
                else
                {
                    //god damn period
                }
            }
            else if(loweredSet.Contains("expeditions"))
            {
                finalName = dataStr.Replace("Expeditions", "Expedition");
            }

            //TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            //return textInfo.ToTitleCase(finalName);

            return finalName;
        }
        string PrepareNameString(string name, string set, string types, string layout)
        {
            if (name.Contains("("))
            {
                name = name.Split(new string[] { " (" }, StringSplitOptions.None).First();
            }
            if (types.ToLower().Contains("land"))
            {
                name += " - " + set;
            }

            if (name == "Our Market Research Shows That Players Like Really Long Card Names So We Made this Card to Have the Absolute Longest Card Name Ever Elemental")
            {
                name = "Our Market Research";
            }
            if (name.ToLower().Contains("token card"))
            {
                name = name.Split(new string[] { " token" }, StringSplitOptions.None).First();
                name += " Token";
            }
            if (layout == "token")
            {
                name += " Token";
            }

            return name;
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
            if (dataGridView_CardData.InvokeRequired)
            {
                try
                {
                    AddCardToRowDelegate addCardToRowDelegate = new AddCardToRowDelegate(AddCardToRow);
                    Invoke(addCardToRowDelegate, new object[] { cardData });
                }
                catch(Exception ex)
                {

                }
                return;
            }

            var name          = cardData["name"].ToString().Replace("''", "'");
            var set           = cardData["set"].ToString().Replace("''", "'");
            var rarity        = cardData["rarity"];
            var colour        = cardData["colour"];
            var inventory     = cardData["inventory"].ToString();
            var foilInventory = cardData["foilInventory"].ToString();
            var price         = cardData["price"].ToString();
            var foilPrice     = cardData["foilPrice"].ToString();
            var lastUpdated   = cardData["lastUpdated"];

            var currency = Properties.Settings.Default["Currency"].ToString();

            if(currency != "USD")
            {
                price = (CurrencyConversion(Convert.ToDecimal(price), "USD", currency));
                foilPrice = (CurrencyConversion(Convert.ToDecimal(foilPrice), "USD", currency));
            }

            dataGridView_CardData.Rows.Add(name, set, rarity, colour, float.Parse(inventory), float.Parse(foilInventory), float.Parse(price), float.Parse(foilPrice), lastUpdated);
        }
        void CheckForNewSets()
        {
            try
            {
                string html = FetchDataFromURL(MTGJSON_URL);

                if (html == "No Response")
                    return;

                ParseHTML(html);
            }
            catch (Exception ex)
            {
                Logger.LogError("Checking for new sets", ex.Message, "");
            }
        }
        void ParseHTML(string html)
        {
            html = html.Split(new string[] { "sets-contents" }, StringSplitOptions.None).Last();
            html = html.Split(new string[] { "windowswarning" }, StringSplitOptions.None).First();

            var splitHTML = html.Split(new string[] { "<div>" }, StringSplitOptions.None).Skip(1).ToArray();
            
            foreach (string div in splitHTML)
            {
                var set = Regex.Match(div, "<p>(.*)</p>").Groups[0].ToString();
                set = Regex.Replace(set, "<p>", String.Empty);
                set = Regex.Replace(set, "</p>", String.Empty);

                if(!DatabaseManager.CheckIfSetExistsByAbbreviation(set))
                {
                    var dataUrl = Regex.Replace(MTGJSON_DATA_URL, "SET", set);
                    //var jsonData = JObject.Parse(FetchDataFromURL(dataUrl));

                    AddNewSet(FetchDataFromURL(dataUrl));
                }
            }
        }
        string FetchDataFromURL(string url)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream data = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(data))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Attempting to fetch MtG data from URL", ex.Message, MTGJSON_URL);
            }

            return "No Response";
        }

        //IMAGE
        void GetImageForCard(int multiverseID)
        {
            //TODO: Locally store and check for images
            LoadImageFromURL(multiverseID);
        }
        void GetImageForFirstCard()
        {
            if (dataGridView_CardData.Rows.Count < 2)
                return;

            var name = dataGridView_CardData.Rows[0].Cells["CardName"].Value.ToString();
            var set  = dataGridView_CardData.Rows[0].Cells["Expansion"].Value.ToString();

            var mID = DatabaseManager.GetMultiverseID(name, set);
            GetImageForCard(mID);
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
                Logger.LogError("Attempting to load image from URL", ex.Message, multiverseID.ToString());
                pictureBox_Card.Image = Properties.Resources.Magic_card_back;
            }
        }

        //INVENTORY
        int GetInventory(string card, string set)
        {
            return DatabaseManager.GetInventory(card, set);
        }
        int GetFoilInventory(string card, string set)
        {
            return DatabaseManager.GetFoilInventory(card, set);
        }
        void AddNewSet(string setData)
        {
            try
            {
                JObject setJSON = JObject.Parse(setData);
                //TODO: ADD SET ID
                AddExpansionToDatabase(setJSON["cards"], setJSON["name"].ToString(), setJSON["code"].ToString());
            }
            catch (Exception ex)
            {
                Logger.LogError("Attempting to add set to database", ex.Message, setData);
            }
        }
        void AddExpansionToDatabase(JToken cardList, string expansion, string abbreviation)
        {
            expansion = Regex.Replace(expansion, "'", "''");
            if (!DatabaseManager.CheckIfSetExists(expansion))
            {
                DatabaseManager.AddNewSet(expansion, abbreviation, null, "mtg");
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

                    values.Add("layout", PrepareString(card, "layout"));
                    values.Add("cardID", PrepareString(card, "id"));
                    values.Add("name", PrepareString(card, "name"));
                    values.Add("manaCost", PrepareString(card, "manaCost"));
                    values.Add("cmc", card["cmc"].ToObject<int>());
                    values.Add("colors", PrepareString(card, "colors"));
                    values.Add("rarity", PrepareString(card, "rarity"));
                    values.Add("type", PrepareString(card, "type"));
                    values.Add("types", PrepareString(card, "types"));
                    values.Add("subtypes", PrepareString(card, "subtypes"));
                    values.Add("text", PrepareString(card, "text"));
                    values.Add("flavorText", PrepareString(card, "flavor"));
                    values.Add("power", PrepareString(card, "power"));
                    values.Add("toughness", PrepareString(card, "toughness"));
                    values.Add("colorIdentity", PrepareString(card, "colorIdentity"));
                    values.Add("multiverseID", multiverseID);
                    values.Add("expansion", expansion);
                    values.Add("price", -1);
                    values.Add("inventory", 0);
                    values.Add("foilPrice", -1);
                    values.Add("foilInventory", 0);
                    values.Add("priceLastUpdated", 0);

                    try
                    {
                        DatabaseManager.AddNewCard("mtg", values);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Attempting to add MtG card to database", ex.Message, values.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError("Attempting to parse card", ex.Message, card.ToString());
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

        //CART
        public void LoadCarts()
        {
            dataGridView_Carts.Rows.Clear();

            var query = "SELECT ID, CustomerName, CardAmounts FROM Carts WHERE Status = 'Active' ORDER BY ID";
            var cartData = DatabaseManager.RunQuery(query);

            foreach (DataRow cart in cartData.Rows)
            {
                var cartID       = cart[0].ToString();
                var cartCustomer = cart[1].ToString();
                var cardAmounts = Convert.ToString(cart[2]);

                var amount = GetCardAmount(cardAmounts);

                try
                {
                    dataGridView_Carts.Rows.Add(cartID, cartCustomer, amount);
                }
                catch(Exception ex)
                {
                    //TODO: Log this? Most likely because form is closed
                }
            }
        }
        int GetCardAmount(string cardAmounts)
        {
            if (String.IsNullOrWhiteSpace(cardAmounts))
                return 0;

            while (cardAmounts.StartsWith("|"))
            {
                cardAmounts = cardAmounts.Substring(1);
            }

            var splitAmounts = cardAmounts.Split('|');

            var totalAmount = 0;

            foreach (var amount in splitAmounts)
            {
                totalAmount += Convert.ToInt32(amount);
            }

            return totalAmount;
        }
        void AddSelectedCardToCart()
        {
            try
            {
                var mtgIndex = dataGridView_CardData.CurrentCell.RowIndex;
                var cartIndex = dataGridView_Carts.CurrentCell.RowIndex;

                var cardName = dataGridView_CardData.Rows[mtgIndex].Cells["CardName"].Value.ToString();
                var cardSet = dataGridView_CardData.Rows[mtgIndex].Cells["Expansion"].Value.ToString();
                var cartID = Convert.ToInt32(dataGridView_Carts.Rows[cartIndex].Cells["CartID"].Value);

                var cardID = DatabaseManager.GetMultiverseID(cardName, cardSet).ToString();

                CartManager.AddItemToCart(cartID, cardID, cardName, cardSet, 1);
            }
            catch (Exception ex)
            {
                //TODO: Message? Issue is most likely having not selected a cart or card
            }
        }
        private void button_AddToCart_Click(object sender, EventArgs e)
        {
            AddSelectedCardToCart();
        }
        private void button_AddFoilToCart_Click(object sender, EventArgs e)
        {
            try
            {
                var mtgIndex = dataGridView_CardData.CurrentCell.RowIndex;
                var cartIndex = dataGridView_Carts.CurrentCell.RowIndex;

                var cardName = dataGridView_CardData.Rows[mtgIndex].Cells["CardName"].Value.ToString();
                var cardSet  = dataGridView_CardData.Rows[mtgIndex].Cells["Expansion"].Value.ToString();
                var cartID   = Convert.ToInt32(dataGridView_Carts.Rows[cartIndex].Cells["CartID"].Value);

                var cardID = DatabaseManager.GetMultiverseID(cardName, cardSet).ToString();

                CartManager.AddItemToCart(cartID, cardID, cardName + " *F*", cardSet, 1);
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
                var cartID    = Convert.ToInt32(dataGridView_Carts.Rows[cartIndex].Cells["CartID"].Value);

                CartManager.DisplayCart(cartID);
            }
            catch (Exception ex)
            {
                //TODO: Message? Issue is most likely having not selected a cart or card
            }
        }
        private void button_DeleteCart_Click(object sender, EventArgs e)
        {
            
        }
        private void button_NewCart_Click(object sender, EventArgs e)
        {
            CartManager.CreateCart();
        }
        
        void UpdateInventory(int multiverseID, int amount, bool foil)
        {
            var query = "UPDATE Mtg SET ";

            if(foil) query += "FoilInventory = '" + amount + "' " + "WHERE multiverseID = '" + multiverseID + "'";
            else query += "Inventory = '" + amount + "' " + "WHERE multiverseID = '" + multiverseID + "'";

            DatabaseManager.RunQuery(query);
        }
        private void InventoryChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_CardData.Columns[e.ColumnIndex].Name == "Inventory")
            {
                var amount = Convert.ToInt32(dataGridView_CardData.Rows[e.RowIndex].Cells["Inventory"].Value);
                var name = dataGridView_CardData.Rows[e.RowIndex].Cells["CardName"].Value.ToString();
                var set = dataGridView_CardData.Rows[e.RowIndex].Cells["Expansion"].Value.ToString();

                var multiverseID = DatabaseManager.GetMultiverseID(name, set);

                UpdateInventory(multiverseID, amount, false);
            }
            else if (dataGridView_CardData.Columns[e.ColumnIndex].Name == "FoilInventory")
            {
                var amount = Convert.ToInt32(dataGridView_CardData.Rows[e.RowIndex].Cells["FoilInventory"].Value);
                var name = dataGridView_CardData.Rows[e.RowIndex].Cells["CardName"].Value.ToString();
                var set = dataGridView_CardData.Rows[e.RowIndex].Cells["Expansion"].Value.ToString();

                var multiverseID = DatabaseManager.GetMultiverseID(name, set);

                UpdateInventory(multiverseID, amount, true);
            }
        }

        private void OnDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            AddSelectedCardToCart();
        }

        void GetImageForSelectedCard()
        {
            int multiverseID = -1;
            if (dataGridView_CardData.Rows.Count == 0 || dataGridView_CardData.CurrentCell == null)
            {
                return;
            }
            var dgvIndex = dataGridView_CardData.CurrentCell.RowIndex;
            try
            {
                var card = dataGridView_CardData.Rows[dgvIndex].Cells["CardName"].Value.ToString();
                var set = dataGridView_CardData.Rows[dgvIndex].Cells["Expansion"].Value.ToString();

                multiverseID = DatabaseManager.GetMultiverseID(card, set);

                GetImageForCard(multiverseID);
            }
            catch (Exception ex)
            {
                Logger.LogError("Attempting tp fetch image for card", ex.Message, multiverseID.ToString());
            }
        }
        private void dataGridView_CardData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ImageThread.IsAlive) ImageThread.Abort();
            ImageThread = new Thread(GetImageForSelectedCard);
            ImageThread.Start();
        }
        private void dataGridView_CardData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ImageThread.IsAlive) ImageThread.Abort();
            ImageThread = new Thread(GetImageForSelectedCard);
            ImageThread.Start();
        }
    }
}

