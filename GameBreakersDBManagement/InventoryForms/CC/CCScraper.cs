using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        static string FILE_PATH = @"C:\GameBreakersInventory\Card Data\";
        static string FILE_SUFFIX = ".csv";

        string fileName = "";

        public CCScraper()
        {
            InitializeComponent();
        }

        //BUTTONS
        private void button_Scrape_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView_CardList.Rows.Clear();
                //TODO: Exception popup
                if (!ValidateURL())
                    return;

                fileName = Regex.Match(textBox_URL.Text, @"(?<=com/)(.*)").Value;
                FetchData();

                Logger.LogActivity("Successfuly fetched and parsed data for url: " + textBox_URL.Text);
            }
            catch (Exception ex)
            {
                Logger.LogError("Something happened in Scrape that was not caught. Error: " + ex);
            }
        }
        private void button_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(fileName))
                    return;

                SaveData();
                dataGridView_CardList.Rows.Clear();

                Logger.LogActivity("Successfuly saved data to database");
            }
            catch (Exception ex)
            {
                Logger.LogError("Something happened in Save that was not caught. Error: " + ex);
            }
        }

        //DATA HANDLERS
        void FetchData()
        {
            //https://www.cardboardconnection.com/2017-18-sp-game-used-hockey-cards
            //https://www.cardboardconnection.com/2017-18-sp-game-used-hockey-cards
            //var testing = new WebClient().DownloadString("https://www.cardboardconnection.com/2017-18-sp-game-used-hockey-cards");

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
                Logger.LogError("Failed to fetch data from URL: " + textBox_URL.Text + "\r\n" + ex);
            }

            //TODO: Throw exception
            if (html == "No Response")
                return;

            ParseHTML(html);
        }
        void ParseHTML(string html)
        {
            var splitHtml = html.Split(new string[] { "<h3 class=\"hot-title\">" }, StringSplitOptions.None).Skip(1);
            foreach (string checklist in splitHtml)
            {
                ParseChecklist(checklist);
            }
        }
        void ParseChecklist(string checklist)
        {
            var category = checklist.Split(new string[] { "</h3>" }, StringSplitOptions.None).First();
            category = Regex.Replace(category, " Set Checklist", String.Empty);
            var matches = Regex.Matches(checklist, "<div class=\"ezcol(.*)</div>", RegexOptions.Singleline)[0].ToString().Split(new string[] { "<br /></div>" }, StringSplitOptions.None);

            List<string> cards = new string[matches.Length].ToList();

            foreach(string match in matches)
            {
                if (match.Contains("auth-bio clearfix"))
                    break;

                cards = cards.Concat(Regex.Replace(match, "<.*?>", string.Empty).Split('\n')).ToList();
            }            

            if (category.ToLower().Contains("dual") || category.ToLower().Contains("combos"))
                AddCardMultiLineToGridView(category, cards, 2);
            else if (category.ToLower().Contains("trio"))
                AddCardMultiLineToGridView(category, cards, 3);
            else if (category.ToLower().Contains("quad"))
                AddCardMultiLineToGridView(category, cards, 4);
            else
            {
                foreach (string card in cards)
                {
                    if(!String.IsNullOrWhiteSpace(card))
                        AddCardToGridView(category, card);
                }
            }
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
                
                //if (!String.IsNullOrWhiteSpace(odds))
                //{
                //    if (!odds.Any(char.IsDigit))
                //    {
                //        odds = String.Empty;
                //    }
                //    else
                //    {
                //        var temp = odds;
                //        temp = Regex.Replace(temp, ":", String.Empty);
                //        temp = Regex.Replace(temp, @"\d", String.Empty);

                //        if (Regex.Replace(temp, @"\s", String.Empty).Length > 1)
                //        {
                //            temp = temp.Substring(1);
                //            if (temp[0] == ' ')
                //                temp = temp.Substring(1);
                //            odds = Regex.Replace(odds, temp, String.Empty);
                //        }
                //    }
                //}

                AddRow(category, number, name, team, printRun, odds);
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to parse a card\r\nCard data: " + card + "\r\nCategory: " + category + "\r\nError: " + ex);
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
                Logger.LogError("Failed to parse an array of cards\r\nCard data: " + cards + "\r\nCategory: " + category + "\r\nError: " + ex);
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
            for (int i = 0; i < dataGridView_CardList.Rows.Count - 1; i++)
            {
                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("category", dataGridView_CardList.Rows[i].Cells[0].Value.ToString());
                values.Add("number", dataGridView_CardList.Rows[i].Cells[1].Value.ToString());
                values.Add("name", dataGridView_CardList.Rows[i].Cells[2].Value.ToString());
                values.Add("team", dataGridView_CardList.Rows[i].Cells[3].Value.ToString());
                values.Add("printRun", dataGridView_CardList.Rows[i].Cells[4].Value.ToString());
                values.Add("odds", dataGridView_CardList.Rows[i].Cells[5].Value.ToString());
                values.Add("inventory", dataGridView_CardList.Rows[i].Cells[6].Value.ToString());

                if (!DatabaseManager.CheckIfSetExists(values["category"].ToString()))
                {
                    DatabaseManager.AddNewSet(values["category"].ToString(), null, null, "non_mtg");
                    Logger.LogActivity("Adding new set: " + values["category"].ToString() + " to databse");
                }

                if (CheckForDuplicates(values)) continue;

                try
                {
                    DatabaseManager.AddNewCard("non_mtg", values);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error adding card: " + values["name"] + " in set: " + values["category"].ToString() + "\r\n\r\nError message:" + ex.ToString());
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
