using HtmlAgilityPack;
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
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameBreakersDBManagement
{
    public partial class CCScraper : Form
    {
        //TODO: Something about this?
        //https://www.cardboardconnection.com/2017-18-leaf-ultimate-hockey-cards
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
                AddRowIndices();

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

            //This URL needs better parsing: https://www.cardboardconnection.com/2017-bowman-draft-baseball-cards
            var mainNode = doc.DocumentNode.SelectNodes("//div[@class='postTabs_divs']")[0];
            
            SetData = GetSetData(mainNode);
            Text = "Cardboard Connections Data Scraper: " + SetData["Set"];

            var url = GetXLSXURL(mainNode);
            if(DownloadXLSXFile(url))
            {
                try
                {
                    LoadXLSXIntoGridViewMethodOne();
                }
                catch(Exception ex)
                {
                    try
                    {
                        LoadXLSXIntoGridViewMethodTwo();
                    }
                    catch (Exception nestEX)
                    {
                        ParseHTMLData(mainNode);
                    }
                }
            }
            else
            {
                ParseHTMLData(mainNode);
            }

            dataGridView_CardList.Columns.Add("Inventory", "Inventory");
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
        void LoadXLSXIntoGridViewMethodOne()
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

            DataTable dataClone = data.Clone();//new DataTable();
            dataClone.Rows.Clear();
            //dataClone.Columns.Add("Category");
            //dataClone.Columns.Add("Number");
            //dataClone.Columns.Add("Name");
            //dataClone.Columns.Add("Team");
            //dataClone.Columns.Add("PrintRun");
            //dataClone.Columns.Add("ExtraData");

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
                    //for (int j = 0; j < row.Table.Columns.Count; j++)
                    //{
                    //    var columnName = row.Table.Columns[j].ColumnName.Replace(" ", String.Empty);

                    //    if (columnName == "SetName" || columnName == "CardSet") columnName = "Category";
                    //    if (columnName == "Card") columnName = "Number";
                    //    if (columnName == "Description" || columnName == "Player") columnName = "Name";
                    //    if (columnName == "TeamCity") columnName = "Team";
                    //    if (columnName.Contains("#")) columnName = "PrintRun";

                    //}
                    dataClone.ImportRow(row);
                }
            }

            data = dataClone;

            dataGridView_CardList.DataSource = data;
        }
        void LoadXLSXIntoGridViewMethodTwo()
        {
            String name = "FINAL";
            String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                            FILE_LOCATION +
                            ";Extended Properties='Excel 12.0 XML;HDR=YES;';";

            OleDbConnection con = new OleDbConnection(constr);
            OleDbCommand oconn = new OleDbCommand("Select * From [" + name + "$]", con);
            con.Open();

            OleDbDataAdapter sda = new OleDbDataAdapter(oconn);
            DataTable data = new DataTable();
            sda.Fill(data);

            DataTable dataClone = data.Clone();//new DataTable();
            dataClone.Rows.Clear();
            //dataClone.Columns.Add("Category");
            //dataClone.Columns.Add("Number");
            //dataClone.Columns.Add("Name");
            //dataClone.Columns.Add("Team");
            //dataClone.Columns.Add("PrintRun");
            //dataClone.Columns.Add("ExtraData");

            var misses = 0;

            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow row = data.Rows[i];
                if (String.IsNullOrWhiteSpace(row[0].ToString()))
                {
                    misses++;
                    var oldData = dataClone.Rows[i - misses];

                    for (int j = 0; j < row.Table.Columns.Count; j++)
                    {
                        if (!String.IsNullOrWhiteSpace(row[j].ToString()))
                        {
                            oldData[j] += ", " + row[j];
                        }
                    }
                }
                else
                {
                    //for (int j = 0; j < row.Table.Columns.Count; j++)
                    //{
                    //    var columnName = row.Table.Columns[j].ColumnName.Replace(" ", String.Empty);

                    //    if (columnName == "SetName" || columnName == "CardSet") columnName = "Category";
                    //    if (columnName == "Card") columnName = "Number";
                    //    if (columnName == "Description" || columnName == "Player") columnName = "Name";
                    //    if (columnName == "TeamCity") columnName = "Team";
                    //    if (columnName.Contains("#")) columnName = "PrintRun";

                    //}
                    dataClone.ImportRow(row);
                }
            }

            data = dataClone;

            dataGridView_CardList.DataSource = data;
        }
        void ParseHTMLData(HtmlNode node)
        {
            //TODO: Deal wih foils and shit
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

                if (listTitle != null)
                {
                    foreach (var title in listTitle)
                    {
                        titleString += title.InnerText;
                    }
                }
                if (listDescriptions != null)
                {
                    foreach (var desc in listDescriptions)
                    {
                        //"Shop for complete base sets on eBay:"
                        if (descriptionString != "")
                            descriptionString += "|";
                        descriptionString += desc.InnerText;
                    }
                }
                if (cardLists != null)
                {
                    foreach (var card in cardLists)
                    {
                        cardString += card.InnerText;
                    }
                }
                else
                {
                    cardString = descriptionString;
                    descriptionString = "";
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
            if(!String.IsNullOrWhiteSpace(sport))
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
                var cards = cardString.Split('\n');                

                var noNumbers = true;

                for (int j = 0; j < cards.Count(); j++)
                {
                    var line = cards[j];
                    while (line[0] == ' ' || line[0] == '|')
                    {
                        line = line.Substring(1);
                    }
                    if (Char.IsDigit(line, 0))
                    {
                        noNumbers = false;
                        break;
                    }
                }

                if (noNumbers) AddCardsWithNoNumbers(category, cards);
                else AddCardsWithNumbers(category, cards);
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to parse Carboard Connection card", ex.ToString(), "Card: " + cardString + "\r\nCategory: " + category);
            }
        }
        void AddCardsWithNumbers(string category, string[] cards)
        {
            var cardData = "";

            var cardNumber = "";
            var names = "";
            var teams = "";
            var printRun = "";
            var extra = "";

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

                        cardData = "";
                        cardNumber = "";
                        names = "";
                        teams = "";
                        printRun = "";
                        extra = "";
                    }
                    //"Don Cherry - Boston Bruins
                    cardData += line;

                    cardNumber += line.Split(' ').First();
                    line = Regex.Replace(line, cardNumber + " ", String.Empty);

                    if (line.Contains("#/"))
                    {
                        printRun += line.Split(new string[] { "#/" }, StringSplitOptions.None).Last();
                        line = Regex.Replace(line, "#/" + printRun, String.Empty);

                        printRun = printRun.Trim();
                        line = line.Trim();
                    }

                    names += Regex.Split(line, " - ").First();

                    if(line.Contains("-"))
                        teams += Regex.Split(line, " - ").Last();

                    //if (names.Contains("("))
                    //    names = Regex.Split(names, "(").First();

                    line = Regex.Replace(line, names, String.Empty);
                    line = Regex.Replace(line, teams, String.Empty);
                    line = Regex.Replace(line, "-", String.Empty);

                    if (!String.IsNullOrWhiteSpace(line))
                        extra += line;
                }
                else
                {
                    //"Eric Lindros - Philadelphia Flyers"
                    cardData += line;
                    var name = Regex.Split(line, " - ").First();
                    var team = Regex.Split(line, " - ").Last();

                    //if (name.Contains("("))
                    //    name = Regex.Split(names, "(").First();

                    names += ", " + name;
                    teams += ", " + team;

                    line = Regex.Replace(line, name, String.Empty);
                    line = Regex.Replace(line, team, String.Empty);
                    line = Regex.Replace(line, "-", String.Empty);

                    if (!String.IsNullOrWhiteSpace(line))
                        extra += line;
                }
            }
            AddRow(category, cardNumber, names, teams, printRun, extra);
        }
        void AddCardsWithNoNumbers(string category, string[] cards)
        {
            for (int j = 0; j < cards.Count(); j++)
            {
                var line = cards[j];
                while (line[0] == ' ' || line[0] == '|')
                {
                    line = line.Substring(1);
                }
                
                var cardNumber = "";
                var names = "";
                var teams = "";
                var printRun = "";
                var extra = "";

                if(category.ToLower().Contains("artists"))
                {
                    names = line;
                    AddRow(category, cardNumber, names, teams, printRun, extra);
                    continue;
                }

                cardNumber += line.Split(' ').First();
                line = Regex.Replace(line, cardNumber + " ", String.Empty);

                if (line.Contains("#/"))
                {
                    printRun += line.Split(new string[] { "#/ " }, StringSplitOptions.None).Last();
                    line = Regex.Replace(line, " #/ " + printRun, String.Empty);
                }

                names += Regex.Split(line, " - ").First();
                teams += Regex.Split(line, " - ").Last();

                if (names.Contains("("))
                    names = Regex.Split(names, "(").First();

                line = Regex.Replace(line, names, String.Empty);
                line = Regex.Replace(line, teams, String.Empty);
                line = Regex.Replace(line, "-", String.Empty);

                if (!String.IsNullOrWhiteSpace(line))
                    extra += line;

                AddRow(category, cardNumber, names, teams, printRun, extra);
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

        void AddRowIndices()
        {
            foreach (DataGridViewRow row in dataGridView_CardList.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
            }

            //dataGridView_CardList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            //dataGridView_CardList.AutoResizeColumn(0);
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
            var setExists = true;
            if (!DatabaseManager.CheckIfSetExists(SetData["Set"]))
            {
                DatabaseManager.AddNewSet(SetData["Set"], null, null, "non_mtg");
                Logger.LogActivity("Adding new set: " + SetData["Set"] + " to databse");

                setExists = false;
            }

            for (int i = 0; i < dataGridView_CardList.Rows.Count - 1; i++)
            {
                var row = dataGridView_CardList.Rows[i];

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Expansion", SetData["Set"]);
                values.Add("Year", SetData["Year"]);
                values.Add("Sport", SetData["Sport"]);
                values.Add("Brand", SetData["Brand"]);
                
                for (int j = 0; j < dataGridView_CardList.Columns.Count; j++)
                {
                    var columnName = Regex.Replace(dataGridView_CardList.Columns[j].Name, " ", String.Empty);
                    columnName     = Regex.Replace(columnName, "'", String.Empty);

                    if (columnName == "Inventory")
                        continue;

                    var columnData = Convert.ToString(row.Cells[j].Value);

                    if (columnName == "Expansion" || columnName == "Set")
                    {
                        //TODO: Something here?
                        continue;
                    }
                    if (columnName == "SetName" || columnName == "CardSet" || columnName == "Subset") columnName = "Category";
                    if (columnName == "Card" || columnName == "Checklist") columnName = "Number";
                    if (columnName == "Description" || columnName == "Player") columnName = "Name";
                    if (columnName == "TeamCity") columnName = "Team";
                    if (columnName.Contains("#")) columnName = "PrintRun";
                    if (columnName == "TeamName")
                    {
                        values["Team"] += " " + columnData;
                        continue;
                    }
                    
                    var newColumnName = PrepColumnName(columnName);
                    if(newColumnName == "ExtraData")
                    {
                        object entry;
                        values.TryGetValue("ExtraData", out entry);

                        if (entry == null)
                        {
                            values.Add("ExtraData", "");
                            values.TryGetValue("ExtraData", out entry);
                        }
                        if (!String.IsNullOrWhiteSpace(columnData))
                        {
                            if (columnName.Contains("("))
                            {
                                var firstPart = columnName.Split('(').First();
                                var lastPart = columnName.Split(')').Last();

                                columnName = firstPart + lastPart;
                            }
                            if (!String.IsNullOrWhiteSpace(entry.ToString()))
                            {
                                values["ExtraData"] += ", " + columnName + ": " + columnData;
                            }
                            else
                            {
                                values["ExtraData"] = columnName + ": " + columnData;
                            }
                        }
                        continue;
                    }

                    if (columnData == null)
                        columnData = String.Empty;

                    values.Add(columnName, columnData);
                }

                var md5 = String.Empty;
                if(setExists)
                    md5 = CheckForDuplicates(values);
                if (md5 == String.Empty)
                    continue;
                values.Add("MD5Hash", md5);

                var amount = String.IsNullOrWhiteSpace(Convert.ToString(row.Cells["Inventory"].Value)) ? "0" : row.Cells["Inventory"].Value.ToString();
                values.Add("Inventory", amount);

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
        string CheckForDuplicates(Dictionary<string, object> values)
        {
            var allString = "";
            var md5String = "";

            for(int i = 0; i < values.Count; i++)
            {
                allString += values.ElementAt(i).ToString();
            }
            
            byte[] encodedPassword = new UTF8Encoding().GetBytes(allString);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            md5String = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

            var query      = "SELECT * FROM Non_Mtg WHERE MD5Hash = '" + md5String + "'";
            var duplicates = DatabaseManager.RunQuery(query);

            if (duplicates.Rows.Count > 0)
            {
                return String.Empty;
            }
            return md5String;
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

        string PrepColumnName(string columnName)
        {
            if (columnName == "Category" || columnName == "Number" || columnName == "Name" ||
               columnName == "Team" || columnName == "PrintRun" || columnName == "Odds")
            {
                return columnName;
            }

            else return "ExtraData";
        }

        //GRID VIEW HANDLERS
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
