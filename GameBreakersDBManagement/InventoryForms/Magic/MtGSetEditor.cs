﻿using System;
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
        string CurrentSet;
        public SetEditorForm()
        {
            InitializeComponent();
            CurrentSet = "";
            LoadSets();
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < dataGridView_CardData.Rows.Count - 2; i++)   //-2 Because of the usual -1, then another because of the empty row at the end
            {
                string name = dataGridView_CardData.Rows[i].Cells["CardName"].Value.ToString();
                string set = dataGridView_CardData.Rows[i].Cells["Set"].Value.ToString();
                int inventory = Int32.Parse(dataGridView_CardData.Rows[i].Cells["Inventory"].Value.ToString());
                int foilInventory = Int32.Parse(dataGridView_CardData.Rows[i].Cells["FoilInventory"].Value.ToString());

                try
                {
                    DatabaseManager.UpdateInventory(name, set, inventory, foilInventory);
                }
                catch(Exception ex)
                {
                    Logger.LogError("Attempting to save card from set editor", ex.ToString(), "Name: " + name + "\r\nSet: " + set);
                }
            }

            Logger.LogActivity("Updated inventory of set: " + dataGridView_CardData.Rows[0].Cells["Set"].Value.ToString()); //TODO: Use CurrentSet?
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            if(CurrentSet != "") DatabaseManager.UnlockSet(CurrentSet);
            Logger.LogActivity("Closing set editor");
            Close();
        }

        void LoadSets()
        {
            var setList = DatabaseManager.GetAllMTGSets();
            
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
            var mid = DatabaseManager.GetMultiverseID(name, set);
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
            var card = dataGridView_CardData.Rows[dgvIndex].Cells["CardName"].Value.ToString();
            var set = dataGridView_CardData.Rows[dgvIndex].Cells["Set"].Value.ToString();

            GetImageForCard(card, set);
        }
        
        private void button_LoadSet_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBox_Sets.Text))
                return;

            if (DatabaseManager.IsSetLocked(comboBox_Sets.Text))
                return; //TODO: Alert user?


            dataGridView_CardData.Rows.Clear();
            var cards = DatabaseManager.GetAllCardsForSet(comboBox_Sets.Text);

            if (cards.Rows.Count < 1)
                return;

            DatabaseManager.LockSet(comboBox_Sets.Text);
            Logger.LogActivity("Locked set: " + comboBox_Sets.Text);
            if (CurrentSet != "") DatabaseManager.UnlockSet(CurrentSet);
            Logger.LogActivity("Unlocked set: " + CurrentSet);
            CurrentSet = comboBox_Sets.Text;

            foreach (DataRow card in cards.Rows)
            {
                var name = card["name"];
                var set = card["expansion"];
                var colour = Convert.ToString(card["colorIdentity"]) == "" ? "C" : card["colorIdentity"];
                var rarity = card["rarity"];
                var inventory = card["inventory"];
                var foilInventory = card["foilInventory"];

                

                dataGridView_CardData.Rows.Add(name, set, rarity, colour, inventory, foilInventory);
            }
        }

        private void SetEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (CurrentSet != "") DatabaseManager.UnlockSet(CurrentSet);
            Logger.LogActivity("Closing set editor");
        }
    }
}
