﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameBreakersDBManagement
{
    public partial class CCScraper : Form
    {
        static string FILE_LOCATION = @"C:\GameBreakersInventory\Data.xlsx";
        Dictionary<string, string> SetData;

        public CCScraper()
        {
            InitializeComponent();
        }

        //BUTTONS
        private void button_Scrape_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView_CardList.Columns.Clear();
                //dataGridView_CardList.Rows.Clear();f
                //TODO: Exception popup
                if (!ValidateURL())
                    return;

                FetchData();

                Logger.LogActivity("Successfuly fetched and parsed data for url: " + textBox_URL.Text);
            }
            catch (Exception ex)
            {
                Logger.LogError("Something happened in Scrape that was not caught", ex.ToString(), null);
            }
        }
        private void button_Save_Click(object sender, EventArgs e)
        {
            try
            {
                SaveData();
                dataGridView_CardList.Columns.Clear();
                //dataGridView_CardList.Rows.Clear();

                Logger.LogActivity("Successfuly saved data to database");
            }
            catch (Exception ex)
            {
                Logger.LogError("Something happened in Save that was not caught", ex.ToString(), null);
            }
        }

        //DATA HANDLERS
        void FetchData()
        {
            string html = "No Response";
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(textBox_URL.Text);

            webReq.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";

            webReq.Headers.Add("Upgrade-Insecure-Requests", "1");
            webReq.Headers.Add("Cache-Control", "max-age=0");

            webReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*; q = 0.8";
            webReq.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            webReq.Headers.Add("Accept-Language", "en-US,en;q=0.9");

            webReq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            try
            {
                webReq.CookieContainer = new CookieContainer();
                webReq.Method = "GET";
                using (WebResponse response = webReq.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        html = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Fetching Cardboard Connection data from url.", ex.ToString(), textBox_URL.Text);
            }

            //TODO: Throw exception
            if (html == "No Response")
                return;

            ParseHTML(html);
        }
        void ParseHTML(string html)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode.SelectNodes("//div[@class='postTabs_divs']")[0];

            SetData = GetSetData(node);
            Text = "Cardboard Connections Data Scraper: " + SetData["Set"];

            var url = GetXLSXURL(node);
            if(DownloadXLSXFile(url))
            {
                LoadXLSXIntoGridView();
            }
            else
            {
                ParseHTMLData(node);
            }
        }
        string GetXLSXURL(HtmlNode node)
        {            
            var checklistDescriptions = node.SelectNodes("//a[text()[contains(., 'xlsx')]]");
            var test = node.SelectNodes("//a[@href]");
            foreach(var desc in test)
            {
                var attribute = desc.Attributes["href"].Value;

                if(attribute.ToLower().Contains("xlsx"))
                {
                    return attribute;
                }
            }

            return String.Empty;
        }
        bool DownloadXLSXFile(string url)
        {
            try
            {
                if (File.Exists(FILE_LOCATION))
                    File.Delete(FILE_LOCATION);

                using (var client = new WebClient())
                {
                    client.DownloadFile(url, FILE_LOCATION);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Fetching Cardboard Connection .xlsx from url.", ex.ToString(), textBox_URL.Text);
            }

            return false;
        }
        void LoadXLSXIntoGridView()
        {
            String name = "Sheet1";
            String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                            FILE_LOCATION +
                            ";Extended Properties='Excel 12.0 XML;HDR=YES;';";

            OleDbConnection con = new OleDbConnection(constr);
            OleDbCommand oconn = new OleDbCommand("Select * From [" + name + "$]", con);
            con.Open();

            OleDbDataAdapter sda = new OleDbDataAdapter(oconn);
            DataTable data = new DataTable();
            sda.Fill(data);

            DataTable dataClone = data.Clone();
            dataClone.Rows.Clear();

            var misses = 0;

            for(int i = 0; i < data.Rows.Count; i++)
            {
                DataRow row = data.Rows[i];

                if (String.IsNullOrWhiteSpace(row[0].ToString()))
                {
                    misses++;
                    var oldData = dataClone.Rows[i - misses];

                    for(int j = 0; j < row.Table.Columns.Count; j++)
                    {
                        if(!String.IsNullOrWhiteSpace(row[j].ToString()))
                        {
                            oldData[j] += ", " + row[j];
                        }
                    }
                }
                else
                {
                    dataClone.ImportRow(row);
                }
            }

            data = dataClone;

            dataGridView_CardList.DataSource = data;
        }
        void ParseHTMLData(HtmlNode node)
        {
            var htmlChunks = node.InnerHtml.Split(new string[] { "<div class=\"ezcol-divider\">" }, StringSplitOptions.None);

            dataGridView_CardList.Columns.Add("Category", "Category");
            dataGridView_CardList.Columns.Add("Number", "Number");
            dataGridView_CardList.Columns.Add("Name", "Name");
            dataGridView_CardList.Columns.Add("Team", "Team");
            dataGridView_CardList.Columns.Add("PrintRun", "Print Run");
            dataGridView_CardList.Columns.Add("ExtraData", "Extra");

            for (int i = 0; i < htmlChunks.Count(); i++)
            {
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(htmlChunks[i]);

                var listTitle        = doc.DocumentNode.SelectNodes("//h3[contains(@class, 'hot-title')]");
                var listDescriptions = doc.DocumentNode.SelectNodes("//div[contains(@class, 'checklistdesc')]");
                var cardLists        = doc.DocumentNode.SelectNodes("//div[contains(@class, 'ezcol-one-half')]");

                var titleString       = "";
                var descriptionString = "";
                var cardString        = "";

                foreach (var title in listTitle)
                {
                    titleString += title.InnerText;
                }
                foreach (var desc in listDescriptions)
                {
                    //"Shop for complete base sets on eBay:"
                    if (descriptionString != "")
                        descriptionString += "|";
                    descriptionString += desc.InnerText;
                }
                foreach (var card in cardLists)
                {
                    cardString += card.InnerText;
                }

                titleString = Regex.Replace(titleString, " Set Checklist", String.Empty);

                AddCardToGridView(titleString, cardString);
            }
        }
        Dictionary<string, string> GetSetData(HtmlNode node)
        {
            var set = Regex.Replace(node.SelectNodes("//h2").First().InnerHtml, " Checklist", String.Empty);
            var year = set.Split(' ').First();
            var sport = FindSport(set);
            var brand = set;
            brand = Regex.Replace(set, year + " ", String.Empty);
            brand = Regex.Replace(brand, " " + sport, String.Empty);

            return new Dictionary<string, string>
            {
                { "Set", set },
                { "Year", year },
                { "Sport", sport },
                { "Brand", brand }
            };
        }
        string FindSport(string set)
        {
            var query = "SELECT * FROM SportList";
            var results = DatabaseManager.RunQuery(query);

            foreach(DataRow sport in results.Rows)
            {
                if(set.Contains(sport[0].ToString()))
                {
                    return sport[0].ToString();
                }
            }

            return String.Empty;
        }
        void AddCardToGridView(string category, string cardString)
        {
            try
            {
                var cards    = cardString.Split('\n');
                var cardData = "";

                var cardNumber   = "";
                var names        = "";
                var teams        = "";
                var printRun     = "";
                var extra        = "";

                for (int j = 0; j < cards.Count(); j++)
                {
                    var line = cards[j];
                    while (line[0] == ' ' || line[0] == '|')
                    {
                        line = line.Substring(1);
                    }

                    if (Char.IsDigit(line, 0))
                    {
                        if (cardData != "")
                        {
                            AddRow(category, cardNumber, names, teams, printRun, extra);

                            cardData   = "";
                            cardNumber = "";
                            names      = "";
                            teams      = "";
                            printRun   = "";
                            extra      = "";
                        }
                        //"Don Cherry - Boston Bruins
                        cardData   += line;

                        cardNumber += line.Split(' ').First();
                        line       = Regex.Replace(line, cardNumber + " ", String.Empty);

                        if (line.Contains("#/"))
                        {
                            printRun += line.Split(new string[] { "#/ " }, StringSplitOptions.None).Last();
                            line     = Regex.Replace(line, " #/ " + printRun, String.Empty);
                        }

                        names      += Regex.Split(line, " - ").First();
                        teams      += Regex.Split(line, " - ").Last();

                        if (names.Contains("("))
                            names = Regex.Split(names, "(").First();

                        line       = Regex.Replace(line, names, String.Empty);
                        line       = Regex.Replace(line, teams, String.Empty);
                        line       = Regex.Replace(line, "-", String.Empty);

                        if (!String.IsNullOrWhiteSpace(line))
                            extra += line;
                    }
                    else
                    {
                        //"Eric Lindros - Philadelphia Flyers"
                        cardData += line;
                        var name = Regex.Split(line, " - ").First();
                        var team = Regex.Split(line, " - ").Last();

                        if (name.Contains("("))
                            name = Regex.Split(names, "(").First();

                        names    += ", " + name;
                        teams    += ", " + team;

                        line     = Regex.Replace(line, name, String.Empty);
                        line     = Regex.Replace(line, team, String.Empty);
                        line     = Regex.Replace(line, "-", String.Empty);
                        
                        if(!String.IsNullOrWhiteSpace(line))
                            extra += line;
                    }
                }
                AddRow(category, cardNumber, names, teams, printRun, extra);
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to parse Carboard Connection card", ex.ToString(), "Card: " + cardString + "\r\nCategory: " + category);
            }
        }
        void AddCardMultiLineToGridView(string category, List<string> cards, int linesPerCard)
        {
            try
            {
                var allNames = "";
                cards = FixCardArray(cards);

                for(int i = 0; i < cards.Count; i += linesPerCard)
                {

                    var number      = "";
                    var name        = "";
                    var team        = "";
                    var printRun    = "";
                    var odds        = "";
                    var card        = "";

                    for(int j = 0; j < linesPerCard; j++)
                    {
                        int index = i + j;
                        if(j == 0)
                        {
                            var data = cards[index];

                            if(data[0] == ' ')
                            {
                                data = data.Substring(1);
                            }
                            number = data.Split(' ').First();

                            if(allNames.Contains(number))
                            {
                                i += 1;
                                j -= 1;
                                continue;
                            }

                            data = Regex.Replace(data, @"\b" + number + @" \b", String.Empty);

                            var split = data.Split(new string[] { " - " }, StringSplitOptions.None);

                            name += split.First();
                            data = Regex.Replace(data, @"\b" + split.First() + @"\b", String.Empty);

                            split = split.Last().Split(new string[] { " #/ " }, StringSplitOptions.None);

                            team = split.First();
                            if(split.Length > 1)
                                printRun = split.Last();

                            var temp = -1;
                        }
                        else
                        {
                            var playerData = cards[index].Split(new string[] { " - " }, StringSplitOptions.None);
                            name += playerData.First();

                            var playerTeam = playerData.Last();
                            if (!team.Contains(playerTeam))
                                team += ", " + playerTeam;
                        }

                        if (j != linesPerCard - 1)
                            name += ", ";

                        card += cards[index] + " ";
                    }
                    
                    allNames += name;
                    if(team.Contains("1/1*"))
                    {
                        var oldTeamName = team;

                        team = Regex.Replace(team, " 1/1*", String.Empty);

                        if(oldTeamName == printRun)
                        {
                            printRun = Regex.Replace(printRun, team, String.Empty);
                            var temp = -1;
                        }
                    }

                    card = Regex.Replace(card, "&nbsp;", String.Empty);
                    
                    AddRow(category, number, name, team, printRun, odds);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to parse an array of Cardboard Connection cards", ex.ToString(), "Cards: " + cards.ToString() + "\r\nCategory: " + category);
            }
        }

        bool ValidateURL()
        {
            Uri uriResult;
            return Uri.TryCreate(textBox_URL.Text, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
        List<string> FixCardArray(List<string> cards)
        {
            var newCardList = new List<string>();
            for(int i =0; i < cards.Count; i++)
            {
                if (cards[i] != null) newCardList.Add(cards[i]);
            }
            return newCardList;
        }

        void SaveData()
        {
            //TODO: Parse file name correctly
            if (!DatabaseManager.CheckIfSetExists(SetData["Set"]))
            {
                DatabaseManager.AddNewSet(SetData["Set"], null, null, "non_mtg");
                Logger.LogActivity("Adding new set: " + SetData["Set"] + " to databse");
            }

            for (int i = 0; i < dataGridView_CardList.Rows.Count - 1; i++)
            {
                var row = dataGridView_CardList.Rows[i];

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("expansion", SetData["Set"]);
                values.Add("year", SetData["Year"]);
                values.Add("sport", SetData["Sport"]);
                values.Add("brand", SetData["Brand"]);
                
                for (int j = 0; j < dataGridView_CardList.Columns.Count; j++)
                {
                    var columnName = Regex.Replace(dataGridView_CardList.Columns[j].Name, " ", String.Empty);
                    columnName     = Regex.Replace(columnName, "'", String.Empty);
                    var columnData = row.Cells[j].Value.ToString();

                    if (columnName == "Expansion")
                    {
                        //TODO: Something here
                        continue;
                    }
                    if (columnName == "SetName") columnName = "Category";
                    if (columnName == "Card") columnName = "Number";
                    if (columnName == "Description") columnName = "Name";
                    if (columnName == "TeamCity") columnName = "Team";
                    if (columnName == "Mem") columnName = "ExtraData";
                    if (columnName == "#d") columnName = "PrintRun";
                    if (columnName == "TeamName")
                    {
                        values["Team"] += " " + columnData;
                        continue;
                    }

                    values.Add(columnName, columnData);
                }

                try
                {
                    if (CheckForDuplicates(values))
                        continue;
                }
                catch(Exception ex)
                {
                    //TODO: Why wont this check work?
                    //if (ex.Message.ToLower().Contains("invalid column name)"))

                    try
                    {
                        CreateColumnIfNotExist(values);
                        if (CheckForDuplicates(values))
                            continue;
                    }
                    catch (Exception nestedEx)
                    {
                        Logger.LogError("Checking database for duplicate CC entries", nestedEx.Message, "");
                    }
                }

                try
                {
                    DatabaseManager.AddNewCard("non_mtg", values);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Attempting to add CC card to database", ex.ToString(), dataGridView_CardList.Rows[i].ToString());
                }
            }
        }
        bool CheckForDuplicates(Dictionary<string, object> values)
        {
            var expansion = values["expansion"];

            var query = "SELECT * FROM Non_Mtg WHERE (";

            for(int i = 0; i < values.Count; i++)
            {
                var key   = "[" + Regex.Replace(values.ElementAt(i).Key.ToString(), "'", "") + "]";
                var value = Regex.Replace(values.ElementAt(i).Value.ToString(), "'", "''");
                query += key + " = '" + value + "'";

                if(i == values.Count - 1)
                {
                    query += ")";
                }
                else
                {
                    query += " AND ";
                }
            }

            var duplicates = DatabaseManager.RunQuery(query);

            if (duplicates.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        void CreateColumnIfNotExist(Dictionary<string, object> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                var key = Regex.Replace(values.ElementAt(i).Key.ToString(), "'", "");
                var query = "IF NOT EXISTS(SELECT * FROM sys.columns WHERE[name] = N'" +
                    key + "' AND[object_id] = OBJECT_ID(N'non_mtg')) BEGIN ALTER TABLE non_mtg ADD " + 
                    key + " varchar(MAX) END";

                DatabaseManager.RunQuery(query);
            }
        }

        //GRID VIEW HANDLERS
        void PopulateGridView()
        {

        }
        void AddRow(string category, string number, string name, string team, string amount, string odds)
        {
            if (number.ToLower() == "shop")
                return;
            if (number.Contains("$"))
                return;
            if (name.ToLower().Contains("e-mail author"))
                return;
            //if (Regex.Replace(name, ",", String.Empty) == other.Remove(0,1))
            //   return;
            dataGridView_CardList.Rows.Add(category, number, name, team, amount, odds, "0");
        }
    }
}
