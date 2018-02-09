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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameBreakersDBManagement
{
    public partial class CCScraper : Form
    {
        static string FILE_LOCATION = @"C:\GameBreakersInventory\Data.xlsx";

        public CCScraper()
        {
            InitializeComponent();
        }

        //BUTTONS
        private void button_Scrape_Click(object sender, EventArgs e)
        {
            try
            {
                //dataGridView_CardList.Rows.Clear();
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
                dataGridView_CardList.Rows.Clear();

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

            var setData = GetSetData(node);

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
            dataGridView_CardList.DataSource = data;
        }
        void ParseHTMLData(HtmlNode node)
        {
            var htmlChunks = node.InnerHtml.Split(new string[] { "<div class=\"ezcol-divider\">" }, StringSplitOptions.None);
            
            for(int i = 0; i < htmlChunks.Count(); i++)
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
                    if (descriptionString != "")
                        descriptionString += "|";
                    descriptionString += desc.InnerText;
                }
                foreach (var card in cardLists)
                {
                    cardString += card.InnerText;
                }
                //"Shop for complete base sets on eBay:"

                //Multis come in different flavours
                //AS2-CD Jeff Carter/Drew Doughty - Kings
                //1 Nail Yakupov - Edmonton Oilers
                //Nathan MacKinnon -Colorado Avalanche

                var cards = cardString.Split('\n');
                

                for(int j = 0; j < cards.Count(); j++)
                {
                    var card = cards[j];
                    while (card[0] == ' ' || card[0] == '|')
                    {
                        card = card.Substring(1);
                    }

                    if (Char.IsDigit(card, 0))
                    {
                        //"1 Don Cherry - Boston Bruins #/ 49"
                    }
                    else
                    {
                        //"Eric Lindros - Philadelphia Flyers"
                        var temp = -1;
                    }
                }

                var test = -1;
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
        void AddCardToGridView(string category, string card)
        {
            try
            {
                card = Regex.Replace(card, "&nbsp;", String.Empty);

                var number  = Regex.Match(card, @"([^\s]+)").Value;
                var name    = Regex.Match(card, @"(?<=" + number + @" )(.*?)(?= -)").Value;
                var team    = Regex.Match(card, @"(?<=- )(.*?)(?= #)").Value;
                var printRun  = Regex.Match(card, @"(?<=#/ )(.*)").Value;
                var odds    = Regex.Match(card, @"(?<=: )(.*)").Value;
                
                if (String.IsNullOrWhiteSpace(name))
                {
                    name = Regex.Replace(card, @"\b" + number + @" \b", String.Empty);
                }
                
                if (String.IsNullOrWhiteSpace(team))
                {
                    team = Regex.Match(card, @"(?<=- )(.*?)(?= :)").Value;

                    if (String.IsNullOrWhiteSpace(team))
                    {
                        team = Regex.Match(card, @"(?<=- )(.*)").Value;
                    }
                }
                if (String.IsNullOrWhiteSpace(printRun))
                {

                }

                AddRow(category, number, name, team, printRun, odds);
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to parse Carboard Connection card", ex.ToString(), "Card: " + card + "\r\nCategory: " + category);
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
            var fileName = "";
            fileName = Regex.Match(textBox_URL.Text, @"(?<=com/)(.*)").Value;
            //TODO: Parse file name correctly
            if (!DatabaseManager.CheckIfSetExists(fileName))
            {
                DatabaseManager.AddNewSet(fileName, null, null, "non_mtg");
                Logger.LogActivity("Adding new set: " + fileName + " to databse");
            }

            for (int i = 0; i < dataGridView_CardList.Rows.Count - 1; i++)
            {
                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("category", fileName);
                values.Add("subCategory", dataGridView_CardList.Rows[i].Cells[0].Value.ToString());
                values.Add("number", dataGridView_CardList.Rows[i].Cells[1].Value.ToString());
                values.Add("name", dataGridView_CardList.Rows[i].Cells[2].Value.ToString());
                values.Add("team", dataGridView_CardList.Rows[i].Cells[3].Value.ToString());
                values.Add("printRun", dataGridView_CardList.Rows[i].Cells[4].Value.ToString());
                values.Add("odds", dataGridView_CardList.Rows[i].Cells[5].Value.ToString());
                values.Add("inventory", dataGridView_CardList.Rows[i].Cells[6].Value.ToString());

                if (CheckForDuplicates(values)) continue;

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
            var name = values["name"].ToString().Replace("'", "''");

            var potentialDuplicates = DatabaseManager.RunQuery("SELECT * FROM Non_Mtg WHERE NAME LIKE \'%" + name + "%\'");

            if (potentialDuplicates.Rows.Count > 0)
            {
                foreach (DataRow match in potentialDuplicates.Rows)
                {
                    var matchCount = 1;

                    if (match[1].ToString() == values["category"].ToString()) matchCount++;
                    if (match[2].ToString() == values["number"].ToString()) matchCount++;
                    if (match[3].ToString() == values["name"].ToString()) matchCount++;
                    if (match[4].ToString() == values["team"].ToString()) matchCount++;
                    if (match[5].ToString() == values["printRun"].ToString()) matchCount++;
                    if (match[6].ToString() == values["odds"].ToString()) matchCount++;

                    if (matchCount == 7)
                        return true;
                }
            }
            return false;
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
