using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Globalization;

namespace GameBreakersDBManagement
{    
    class BackgroundDataManager : BackgroundWorker
    {
        DatabaseManager dbMan;
        static string PYTHON_PRICE_URL = "http://127.0.0.1:5001/cards?name=ABCDE&set=FGHIJ";
        static string PYTHON_IMAGE_URL = "http://127.0.0.1:5000/cards?multiverse_id=ABCDE";
        static string LOCAL_IMAGE_PATH = @"C:\GameBreakersInventory\Images\";
        static string IMAGE_TYPE = ".jpg";


        static string MTGSTOCKS_BASE_URL = @"https://api.mtgstocks.com/";
        static string MTGSTOCKS_QUERY_ID = @"search/autocomplete/";
        static string MTGSTOCKS_QUERY_DATA = @"prints/";

        Logger logger;

        public BackgroundDataManager()
        {
            dbMan = DatabaseManager.GetInstace();
            logger = Logger.GetLogger();
        }

        public void Run()
        {
            IterateThroughSets();
        }

        void IterateThroughSets()
        {
            var sets = dbMan.GetAllSets().Rows;

            foreach (DataRow set in sets)
            {
                IterateThroughSet(set[1].ToString());
            }
        }
        void IterateThroughSet(string setName)
        {
            var cards = dbMan.GetAllCardsForSet(setName);

            foreach (DataRow card in cards.Rows)
            {
                var name = card[3].ToString();
                var stringID = card[17].ToString();
                var multiverseID = -1;
                if(!String.IsNullOrWhiteSpace(stringID))
                    multiverseID = Int32.Parse(stringID);

                double timeLastUpdated = double.Parse(card[23].ToString());              

                GetImageForCard(0);
                if (PriceOldEnoughToUpdate(timeLastUpdated) || float.Parse(card[19].ToString()) < 0 || float.Parse(card[21].ToString()) < 0)
                {
                    UpdatePriceOfCard(name, setName);
                }
                if (multiverseID != -1)
                {
                    GetImageForCard(multiverseID);
                }
            }
        }

        bool PriceOldEnoughToUpdate(double timeLastUpdated)
        {
            var timeBetweenUpdate = ConfigurationManager.AppSettings["daysBetweenPriceCheck"];
            double timeBetweenInMilliseconds = TimeSpan.FromDays(double.Parse(timeBetweenUpdate)).TotalMilliseconds;
            if ((DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeBetweenInMilliseconds) > timeLastUpdated)
                return true;
            return false;
        }

        void GetSetID(string name, string set)
        {
            var url = MTGSTOCKS_BASE_URL + MTGSTOCKS_QUERY_ID + name;
            var cardID = "";
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            using (WebClient wc = new WebClient())
            {
                var html = wc.DownloadString(url);
                html = html.Replace("[", "");
                html = html.Replace("]", "");

                try
                {
                    var json = JObject.Parse(html);
                    cardID = json["id"].ToString();
                }
                catch (Exception ex)
                {
                    logger.LogError("Failed to parse card: " + name + " for set: " + set);
                    throw new Exception();
                }
            }

            var idURL = MTGSTOCKS_BASE_URL + MTGSTOCKS_QUERY_DATA + cardID;
            using (WebClient wc = new WebClient())
            {
                var html = wc.DownloadString(idURL);

                var json = JObject.Parse(html);

                dbMan.UpdateSetID(set, json["card_set"]["id"].ToObject<int>());

                foreach(var card in json["sets"])
                {
                    var setID = card["set_id"].ToObject<int>();
                    var setName = card["set_name"].ToString();

                    try
                    {
                        dbMan.UpdateSetID(setName, setID);
                        logger.LogActivity("Added ID: " + setID + " to set: " + setName);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("Failed to add ID.\r\nSet Name: " + setName + "\r\nSet ID: " + setID);
                    }
                }
            }
        }

        //DATA HANDLERS
        void UpdatePriceOfCard(string name, string set)
        {
            var price = -1f;
            var foilPrice = -1f;

            var preppedSetName = PrepareSetName(set);

            var url = GeneratePriceURL(name, preppedSetName);

            using (WebClient wc = new WebClient())
            {
                string html = "";
                JObject json = null;
                try
                {
                    html = wc.DownloadString(url);
                    json = JObject.Parse(html);
                }
                catch(Exception ex)
                {
                    logger.LogError("Failed to get parse HTML into JSON. \r\nHTML: " + html + "\r\nURL: " + url + "\r\nName" + name + "\r\nSet: " + set + "\r\nPrepped set name: " + preppedSetName);
                    return;
                }

                try
                {
                    price = json["price"].ToObject<float>();
                }
                catch(Exception ex)
                {
                    logger.LogError("Failed to get price for card: " + name + " from set: " + set + ", using set name: " + preppedSetName);
                }
                try
                {
                    foilPrice = json["price_foil"].ToObject<float>();
                }
                catch(Exception ex)
                {
                    logger.LogError("Failed to get foil price for card: " + name + " from set: " + set + ", using set name: " + preppedSetName);
                }

                if(price != -1)
                {
                    dbMan.UpdatePrice(name, set, price, false);
                }
                if(foilPrice != -1)
                {
                    dbMan.UpdatePrice(name, set, foilPrice, true);
                }
            }
        }
        void GetImageForCard(int multiverseID)
        {
            if (File.Exists(LOCAL_IMAGE_PATH + multiverseID.ToString() + IMAGE_TYPE))
                return;

            var url = GenerateImageURL(multiverseID);
            using (WebClient wc = new WebClient())
            {
                try
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead(url);
                    Bitmap bitmap; bitmap = new Bitmap(stream);

                    if (bitmap != null)
                        bitmap.Save(LOCAL_IMAGE_PATH + multiverseID.ToString(), ImageFormat.Jpeg);

                    stream.Flush();
                    stream.Close();
                    client.Dispose();
                }
                catch (Exception ex)
                {
                    logger.LogError("Failed to get image for ID: " + multiverseID);
                }
            }
        }
        string PrepareSetName(string set)
        {
            var loweredSet = set.ToLower();
            var finalName = "";

            if (loweredSet.Contains("fourth"))
            {
                finalName = set.Replace("Fourth", "4th");
            }
            else if (loweredSet.Contains("fifth"))
            {
                finalName = set.Replace("Fifth", "5th");
            }
            else if (loweredSet.Contains("sixth"))
            {
                finalName = set.Replace("Sixth", "6th");
            }
            else if (loweredSet.Contains("seventh"))
            {
                finalName = set.Replace("Seventh", "7th");
            }
            else if (loweredSet.Contains("eighth"))
            {
                finalName = set.Replace("Eighth", "8th");
            }
            else if(loweredSet.Contains("ninth"))
            {
                finalName = set.Replace("Ninth", "9th");
            }
            else if (loweredSet.Contains("tenth"))
            {
                finalName = set.Replace("Tenth", "10th");
            }

            return finalName;
        }
                
        //URLS
        string GeneratePriceURL(string name, string setName)
        {
            var url = PYTHON_PRICE_URL.Replace("ABCDE", RemoveDiacritics(name));
            url = url.Replace("FGHIJ", RemoveDiacritics(setName));

            return url;
        }
        string GenerateImageURL(int multiverseID)
        {
            var url = PYTHON_IMAGE_URL.Replace("ABCDE", multiverseID.ToString());

            return url;
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
    }
}
