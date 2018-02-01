using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace GameBreakersDBManagement
{
    public partial class SetEditorForm : Form
    {
        DatabaseManager dbMan;
        Logger logger;
        string CurrentSet;
        public SetEditorForm()
        {
            InitializeComponent();
            dbMan = DatabaseManager.GetInstace();
            logger = Logger.GetLogger();
            CurrentSet = "";
            LoadSets();
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < dataGridView_CardData.Rows.Count - 2; i++)   //-2 Because of the usual -1, then another because of the empty row at the end
            {
                string name = dataGridView_CardData.Rows[i].Cells[0].Value.ToString();
                string set = dataGridView_CardData.Rows[i].Cells[1].Value.ToString();
                int inventory = Int32.Parse(dataGridView_CardData.Rows[i].Cells[3].Value.ToString());
                int foilInventory = Int32.Parse(dataGridView_CardData.Rows[i].Cells[4].Value.ToString());

                try
                {
                    dbMan.UpdateInventory(name, set, inventory, foilInventory);
                }
                catch(Exception ex)
                {
                    logger.LogError("Error saving card: " + name + " from set: " + set + "\r\n\r\nError message:" + ex.ToString());
                }
            }

            logger.LogActivity("Updated inventory of set: " + dataGridView_CardData.Rows[0].Cells[1].Value.ToString()); //TODO: Use CurrentSet?
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            if(CurrentSet != "") dbMan.UnlockSet(CurrentSet);
            logger.LogActivity("Closing set editor");
            Close();
        }

        void LoadSets()
        {
            var setList = dbMan.GetAllSets();
            
            foreach(DataRow set in setList.Rows)
            {
                comboBox_Sets.Items.Add(set[1]);
            }
        }

        void GetSetSymbol()
        {
            //TODO: This?
            //https://andrewgioia.github.io/Keyrune/icons.html?icon=ody
        }

        private void comboBox_Sets_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void GetImageForCard(string name, string set)
        {
            var mid = dbMan.GetMultiverseID(name, set);
            var gathererURL = Regex.Replace(@"http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=ABCDE&type=card", "ABCDE", mid.ToString());
            LoadImageFromURL(gathererURL);
        }
        bool LoadImageFromURL(string url)
        {
            pictureBox_Card.Image = Properties.Resources.Magic_card_back;

            try
            {
                pictureBox_Card.Load(url);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void dataGridView_CardData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_CardData.Rows.Count == 0 || dataGridView_CardData.CurrentCell == null)
            {
                return;
            }
            var dgvIndex = dataGridView_CardData.CurrentCell.RowIndex;
            var card = dataGridView_CardData.Rows[dgvIndex].Cells[0].Value.ToString();
            var set = dataGridView_CardData.Rows[dgvIndex].Cells[1].Value.ToString();

            GetImageForCard(card, set);
        }
        
        private void button_LoadSet_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBox_Sets.Text))
                return;

            if (dbMan.IsSetLocked(comboBox_Sets.Text))
                return; //TODO: Alert user?


            dataGridView_CardData.Rows.Clear();
            var cards = dbMan.GetAllCardsForSet(comboBox_Sets.Text);

            dbMan.LockSet(comboBox_Sets.Text);
            logger.LogActivity("Locked set: " + comboBox_Sets.Text);
            if (CurrentSet != "") dbMan.UnlockSet(CurrentSet);
            logger.LogActivity("Unlocked set: " + CurrentSet);
            CurrentSet = comboBox_Sets.Text;

            foreach (DataRow card in cards.Rows)
            {
                var name = card[3];
                var set = card[18];
                var rarity = card[7];
                var inventory = card[20];
                var foilInventory = card[22];

                dataGridView_CardData.Rows.Add(name, set, rarity, inventory, foilInventory);
            }
        }
    }
}
