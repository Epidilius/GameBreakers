using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace GameBreakersDBManagement
{
    public partial class ManualEntry : Form
    {
        public ManualEntry()
        {
            InitializeComponent();
        }

        private void button_AddCard_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
                return;

            var expansion = textBox_Expansion.Text;
            var category  = textBox_Category.Text;
            var year      = textBox_Year.Text;
            var sport     = textBox_Sport.Text;
            var number    = textBox_Number.Text;
            var name      = textBox_Name.Text;
            var team      = textBox_Team.Text;
            var printRun  = textBox_PrintRun.Text;
            var odds      = textBox_Odds.Text;
            var extraData = textBox_ExtraData.Text;
            var brand     = textBox_Brand.Text;
            var inventory = textBox_Inventory.Text;

            Dictionary<string, object> values = new Dictionary<string, object>()
            {
                { "Expansion", expansion },
                { "Year", year },
                { "Sport", sport },
                { "Brand", brand },
                { "Category", category },
                { "Number", number },
                { "Name", name },
                { "Team", team },
                { "ExtraData", extraData },
                { "PrintRun", printRun },
                { "Odds", odds }
            };

            var md5 = CheckForDuplicates(values);

            if(md5 == String.Empty)
            {
                return;
            }

            values.Add("MD5Hash", md5);
            values.Add("Inventory", inventory);

            try
            {
                DatabaseManager.AddNewCard("non_mtg", values);
                ClearFields();
            }
            catch (Exception ex)
            {
                Logger.LogError("Attempting to manually add card to database", ex.ToString(), string.Join("|", values));
            }
        }

        string CheckForDuplicates(Dictionary<string, object> values)
        {
            var allString = "";
            var md5String = "";

            for (int i = 0; i < values.Count; i++)
            {
                allString += values.ElementAt(i).ToString();
            }

            byte[] encodedPassword = new UTF8Encoding().GetBytes(allString);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            md5String = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

            var query = "SELECT * FROM Non_Mtg WHERE MD5Hash = '" + md5String + "'";
            var duplicates = DatabaseManager.RunQuery(query);

            if (duplicates.Rows.Count > 0)
            {
                return String.Empty;
            }
            return md5String;
        }

        bool ValidateFields()
        {
            var controls = GetAll(this, typeof(TextBox));

            foreach (var control in controls)
            {
                if (!String.IsNullOrWhiteSpace(control.Text))
                {
                    return true;
                }
            }

            return false;
        }
        void ClearFields()
        {
            var controls = GetAll(this, typeof(TextBox));

            foreach(var control in controls)
            {
                control.Text = String.Empty;
            }
        }
        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => GetAll(ctrl, type)).Concat(controls).Where(c => c.GetType() == type);
        }
    }
}
