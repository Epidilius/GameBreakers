using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameBreakersDBManagement
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            LoadSettings();
        }

        void LoadSettings()
        {
            textBox_DaysBetweenUpdate.Text = Properties.Settings.Default["DaysBetweenPriceCheck"].ToString();
            var currency                   = Properties.Settings.Default["Currency"].ToString();

            for(int i = 0; i < comboBox_Currency.Items.Count; i++)
            {
                if(comboBox_Currency.Items[i].ToString() == currency)
                {
                    comboBox_Currency.SelectedIndex = i;
                    break;
                }
            }
        }
        void SaveSettings()
        {
            Properties.Settings.Default["DaysBetweenPriceCheck"] = textBox_DaysBetweenUpdate.Text;
            Properties.Settings.Default["Currency"] = comboBox_Currency.Text;

            Properties.Settings.Default.Save();
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button_UpdatePrice_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Updating the price for cards can take a long time, and slow down the computer. \r\n\r\nContinue?",
                                         "Update Price Confirmation",
                                         MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                new Thread(() =>
                {
                    BeginPriceUpdate();
                }).Start();
            }
        }

        private void button_ImportExcelData_Click(object sender, EventArgs e)
        {
            //TODO: Determine type, import, display data on screen, save
        }

        void BeginPriceUpdate()
        {
            //TODO: Push ID to database, then just use that and skip some steps
            var base_url = @"https://api.mtgstocks.com/card_sets/";
            var url_counter = 1;
            var noMoreSets = false;
            do
            {
                var url = base_url + url_counter;
                JObject json = GetJSONFromURL(url);

                if (json == null)
                {
                    noMoreSets = true;
                    continue;
                }

                var setName = json["name"].ToString();
                
                UpdateCardInSet(setName, json);

                url_counter++;
            } while (noMoreSets == false);
        }
        JObject GetJSONFromURL(string url)
        {
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
                Logger.LogError("Attempting to get data for set", ex.Message, "URL: " + url);
                return null;
            }
        }
        void UpdateCardInSet(string set, JObject setData)
        {
            DatabaseManager.LockSet(set.Replace("'", "''"));
            var cardList = DatabaseManager.GetAllCardsForSet(set.Replace("'", "''"));

            var json = setData["prints"];

            foreach(var card in json)
            {
                var id   = card["id"].ToString();
                var name = card["name"].ToString();

                var cardObject = GetMTGStocksData(id);
                var cardData   = ParseCardData(cardObject, set);
                var prices     = GetPriceFromData(cardData);

                DatabaseManager.UpdatePrice(name, set, prices["price"], false);
                DatabaseManager.UpdatePrice(name, set, prices["foilPrice"], true);
            }

            Logger.LogActivity("Updated price of cards in set:" + set);
            DatabaseManager.UnlockSet(set.Replace("'", "''"));
        }
        JObject GetMTGStocksData(string id)
        {
            var url = @"https://api.mtgstocks.com/prints/" + id;

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
                Logger.LogError("Attempting to get data for card with MTG Stocks ID", ex.Message, "URL: " + url);
                return null;
            }
        }
        JToken ParseCardData(JObject cardData, string set = "")
        {
            var cardSetName = cardData["card_set"]["name"].ToString();
            if (set == "" || cardSetName == set)
            {
                return cardData;
            }
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
        Dictionary<string, float> GetPriceFromData(JToken card)
        {
            var prices = new Dictionary<string, float>();
            try
            {
                prices["price"] = float.Parse(card["latest_price"]["avg"].ToString());    //TODO: Market price?
            }
            catch (Exception ex)
            {
                try
                {
                    prices["price"] = float.Parse(card["latest_price"].ToString());
                }
                catch(Exception exNested)
                {
                    prices["price"] = 0;
                }
                if (prices["price"] == 0)
                {
                    try
                    {
                        prices["price"] = float.Parse(card["latest_price_mkm"].ToString());
                    }
                    catch(Exception exNested)
                    {
                        prices["price"] = float.Parse(card["latest_price_mkm"]["avg"].ToString());
                    }
                }
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
                    var nestedCardObject = GetMTGStocksData(card["id"].ToString());
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

            return prices;
        }
    }
}
