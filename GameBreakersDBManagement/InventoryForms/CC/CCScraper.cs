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

        Logger logger;

        public CCScraper()
        {
            InitializeComponent();
            logger = Logger.GetLogger();
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

                logger.LogActivity("Successfuly fetched and parsed data for url: " + textBox_URL.Text);
            }
            catch (Exception ex)
            {
                logger.LogError("Something happened in Scrape that was not caught. Error: " + ex);
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

                logger.LogActivity("Successfuly saved data to file: " + fileName);
            }
            catch (Exception ex)
            {
                logger.LogError("Something happened in Save that was not caught. Error: " + ex);
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
                logger.LogError("Failed to fetch data from URL: " + textBox_URL.Text + "\r\n" + ex);
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
                var amount  = Regex.Match(card, @"(?<=#/ )(.*)").Value;
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
                if (String.IsNullOrWhiteSpace(amount))
                {

                }

                var other = card;
                other = Regex.Replace(other, @"\b" + odds + @"\b", String.Empty);
                other = Regex.Replace(other, @"\b" + number + @"\b", String.Empty);
                other = Regex.Replace(other, name, String.Empty);
                other = Regex.Replace(other, team, String.Empty);
                other = Regex.Replace(other, @"\b" + amount + @"\b", String.Empty);
                other = Regex.Replace(other, "  -  #/ ", String.Empty);
                other = Regex.Replace(other, "  - ", String.Empty);
                other = Regex.Replace(other, " : ", String.Empty);
                other = Regex.Replace(other, ",", String.Empty);

                if (!String.IsNullOrWhiteSpace(odds))
                {
                    if (!odds.Any(char.IsDigit))
                    {
                        other = odds;
                        odds = String.Empty;

                        other = Regex.Replace(other, "&nbsp;", String.Empty);
                    }
                    else
                    {
                        var temp = odds;
                        temp = Regex.Replace(temp, ":", String.Empty);
                        temp = Regex.Replace(temp, @"\d", String.Empty);

                        if (Regex.Replace(temp, @"\s", String.Empty).Length > 1)
                        {
                            temp = temp.Substring(1);
                            if (temp[0] == ' ')
                                temp = temp.Substring(1);
                            other = temp;
                            odds = Regex.Replace(odds, temp, String.Empty);
                        }
                    }
                }

                AddRow(category, number, name, team, amount, odds, other);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to parse a card\r\nCard data: " + card + "\r\nCategory: " + category + "\r\nError: " + ex);
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

                    var number  = "";
                    var name    = "";
                    var team    = "";
                    var amount  = "";
                    var odds    = "";
                    var other   = "";
                    var card    = "";

                    for(int j = 0; j < linesPerCard; j++)
                    {
                        int index = i + j;
                        if(j == 0)
                        {
                            var data = cards[index];

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
                            amount = split.Last();
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

                        if(oldTeamName == amount)
                        {
                            amount = Regex.Replace(amount, team, String.Empty);
                        }
                    }

                    card = Regex.Replace(card, "&nbsp;", String.Empty);

                    other = card;
                    other = Regex.Replace(other, @"\b" + odds + @"\b", String.Empty);
                    other = Regex.Replace(other, @"\b" + number + @"\b", String.Empty);
                    other = Regex.Replace(other, @"\b" + amount + @"\b", String.Empty);

                    var names = name.Split(new string[] { ", " }, StringSplitOptions.None);
                    foreach (string playerName in names)
                        other = Regex.Replace(other, playerName, String.Empty);
                    var teams = team.Split(new string[] { ", " }, StringSplitOptions.None);
                    foreach (string playerTeam in teams)
                        other = Regex.Replace(other, playerTeam, String.Empty);
                    
                    AddRow(category, number, name, team, amount, odds, other);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to parse an array of cards\r\nCard data: " + cards + "\r\nCategory: " + category + "\r\nError: " + ex);
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
            var sb = new StringBuilder();

            var headers = dataGridView_CardList.Columns.Cast<DataGridViewColumn>();
            sb.AppendLine(string.Join(",", headers.Select(column => "\"" + column.HeaderText + "\"").ToArray()));

            foreach (DataGridViewRow row in dataGridView_CardList.Rows)
            {
                var cells = row.Cells.Cast<DataGridViewCell>();
                sb.AppendLine(string.Join(",", cells.Select(cell => "\"" + cell.Value + "\"").ToArray()));
            }

            var file = FILE_PATH + fileName + FILE_SUFFIX;

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var stream = File.Open(file, FileMode.OpenOrCreate))
            {
                stream.Write(Encoding.ASCII.GetBytes(sb.ToString()), 0, Encoding.ASCII.GetByteCount(sb.ToString()));
            }
        }

        //GRID VIEW HANDLERS
        void PopulateGridView()
        {

        }
        void AddRow(string category, string number, string name, string team, string amount, string odds, string other)
        {
            if (number.ToLower() == "shop")
                return;
            if (number.Contains("$"))
                return;
            if (name.ToLower().Contains("e-mail author"))
                return;
            //if (Regex.Replace(name, ",", String.Empty) == other.Remove(0,1))
            //   return;
            other = Regex.Replace(other, @"[^0-9a-zA-Z*]+", String.Empty);
            dataGridView_CardList.Rows.Add(category, number, name, team, amount, odds, other, "0");
        }
    }
}
