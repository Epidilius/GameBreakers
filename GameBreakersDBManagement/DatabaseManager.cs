using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GameBreakersDBManagement
{
    public static class DatabaseManager
    {
        //TODO: Refactor this class
        static string ConnectionString = ConfigurationManager.ConnectionStrings["GameBreakers"].ConnectionString;
        private static readonly object _syncObject = new object();

        //Utility Functions
        static SqlCommand CreateCommand(string query)
        {
            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand command = new SqlCommand();
            command.Connection = con;
            command.CommandType = CommandType.Text;
            command.CommandText = query;

            return command;
        }
        static SqlCommand CreateCommandWithArgs(string query, Dictionary<string, object> values)
        {
            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand command = new SqlCommand();
            command.Connection = con;
            command.CommandType = CommandType.Text;

            var firstHalf = "";
            var secondHalf = "";

            int i = 0;
            var firstLoop = true;

            foreach(KeyValuePair<string, object> value in values)
            {
                if(firstLoop)
                {
                    firstHalf += "(";
                    secondHalf += "(";
                    firstLoop = false;
                }
                else
                {
                    firstHalf += ", ";
                    secondHalf += ", ";
                }

                firstHalf += value.Key;
                secondHalf += "@val" + i.ToString();
                i++;
            }

            i = 0;
            firstHalf += ")";
            secondHalf += ")";

            query += firstHalf + " VALUES " + secondHalf;

            command.CommandText = query;

            foreach (KeyValuePair<string, object> value in values)
            {
                try
                {
                    if(!String.IsNullOrWhiteSpace(value.Value.ToString()))
                        command.Parameters.AddWithValue("@val" + i.ToString(), value.Value);
                    else
                        command.Parameters.AddWithValue("@val" + i.ToString(), "");
                }
                catch(Exception ex)
                {
                    command.Parameters.AddWithValue("@val" + i.ToString(), "");
                }
                i++;
            }

            return command;
        }
        static SqlDataAdapter CreateDataAdapter(SqlCommand command)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            return adapter;
        }
        static DataTable CreateDataTable(SqlDataAdapter dataAdapter)
        {
            DataTable table = new DataTable();
            dataAdapter.Fill(table);

            return table;
        }
        public static DataTable RunQuery(string query)   //TODO: Rename this
        {
            lock (_syncObject)
            {
                SqlCommand command = CreateCommand(query);
                SqlDataAdapter adapter = CreateDataAdapter(command);
                DataTable table = CreateDataTable(adapter);

                return table;
            }
        }
        public static DataTable RunQueryWithArgs(string query, Dictionary<string, object> values)  
        {
            lock (_syncObject)
            {
                SqlCommand command = CreateCommandWithArgs(query, values);
                SqlDataAdapter adapter = CreateDataAdapter(command);
                DataTable table = CreateDataTable(adapter);

                return table;
            }
        }

        //Get Functions
        public static int GetInventory(string card, string set)
        {
            card = card.Replace(@"'", @"''");
            set = set.Replace(@"'", @"''");
            var dataTable = RunQuery("SELECT INVENTORY FROM MtG WHERE NAME = \'" + card + "\' AND EXPANSION = \'" + set + "\'");
            return (int)dataTable.Rows[0]["inventory"];
        }
        public static int GetFoilInventory(string card, string set)
        {
            card = card.Replace(@"'", @"''");
            set = set.Replace(@"'", @"''");
            var dataTable = RunQuery("SELECT FOILINVENTORY FROM MtG WHERE NAME = \'" + card + "\' AND EXPANSION = \'" + set + "\'");            
            return (int)dataTable.Rows[0]["foilInventory"];
        }
        public static int GetMultiverseID(string card, string set)
        {
            card = card.Replace(@"'", @"''");
            set = set.Replace(@"'", @"''");
            var dataTable = RunQuery("SELECT MULTIVERSEID FROM MtG WHERE NAME = \'" + card + "\' AND EXPANSION = \'" + set + "\'");
            var id = dataTable.Rows[0]["multiverseID"].ToString();
            return Int32.Parse(id);
        }
        public static byte[] GetImageForCard(string card, string set)
        {
            card = card.Replace(@"'", @"''");
            set = set.Replace(@"'", @"''");
            byte[] data = new byte[0];

            var dataTable = RunQuery("SELECT IMAGE FROM MtG WHERE NAME = \'" + card + "\' AND EXPANSION = \'" + set + "\'");
            var id = dataTable.Rows[0]["image"].ToString();

            if (dataTable.Rows.Count == 1)
            {
                data = (byte[])(dataTable.Rows[0]["image"]);
            }

            return data;
        }
        public static DataTable GetAllSets()
        {
            var dataTable = RunQuery("SELECT * FROM Sets");
            return dataTable;
        }
        public static DataTable GetAllCardsForSet(string set)
        {
            set = set.Replace(@"'", @"''");
            var dataTable = RunQuery("SELECT * FROM MtG WHERE EXPANSION = \'" + set + "\'");
            return dataTable;
        }
        public static DataTable GetMagicCard(string name)
        {
            name = name.Replace(@"'", @"''");
            var dataTable = RunQuery("SELECT * FROM MtG WHERE NAME LIKE \'%" + name + "%\' and onlineOnlyVersion = 0");
            return dataTable;
        }
        
        //Modify Functions
        public static void AddOneToInventory(string name, string set, bool foil)
        {
            name = name.Replace(@"'", @"''");
            set = set.Replace(@"'", @"''");
            var query = "UPDATE MtG ";
            if (foil)
            {
                query += "SET foilInventory = foilInventory + 1 ";
            }
            else
            {
                query += "SET Inventory = Inventory + 1 ";
            }
            query += "WHERE NAME = \'" + name + "\' AND EXPANSION = \'" + set + "\'";

            RunQuery(query);
        }
        public static void RemoveOneToInventory(string name, string set, bool foil)
        {
            name = name.Replace(@"'", @"''");
            set = set.Replace(@"'", @"''");
            var query = "UPDATE MtG ";
            if (foil)
            {
                query += "SET foilInventory = foilInventory - 1 ";
            }
            else
            {
                query += "SET Inventory = Inventory - 1 ";
            }
            query += "WHERE NAME = \'" + name + "\' AND EXPANSION = \'" + set + "\'";

            RunQuery(query);
        }
        public static void UpdateInventory(string name, string set, int newAmount, int newFoilAmount)
        {
            name = name.Replace(@"'", @"''");
            set = set.Replace(@"'", @"''");
            var query = "UPDATE MtG SET foilInventory = " + newFoilAmount + ", inventory = " + newAmount;

            query += " WHERE NAME = \'" + name + "\' AND EXPANSION = \'" + set + "\'";

            RunQuery(query);
        }
        public static void UpdatePrice(string name, string set, float price, bool foil)
        {
            name = name.Replace(@"'", @"''");
            set = set.Replace(@"'", @"''");

            var query = "";
            if (foil)
                query = "UPDATE MtG SET foilPrice = @val0, priceLastUpdated = @val1 WHERE name = \'" + name + "\' AND EXPANSION = \'" + set + "\'";
            else
                query = "UPDATE MtG SET price = @val0, priceLastUpdated = @val1 WHERE name = \'" + name + "\' AND EXPANSION = \'" + set + "\'";

            query = query.Replace("@val0", "\'" + price.ToString() + "\'");
            query = query.Replace("@val1", "\'" + DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() + "\'");
            RunQuery(query);
        }
        public static void UpdateSetID(string set, int id)
        {
            var query = "UPDATE Sets SET SetID = \'" + id.ToString() + "\' WHERE name = \'" + set + "\'";
            RunQuery(query);
        }


        //Check/Toggle Functions
        public static bool CheckIfCardExists(string multiverseID)
        {
            var dataTable = RunQuery("SELECT * FROM MtG WHERE MULTIVERSEID = \'" + multiverseID + "\'");
            
            if(dataTable.Rows.Count == 0) return false;
            return true;
        }
        public static bool CheckIfSetExists(string set)
        {
            var dataTable = RunQuery("SELECT * FROM Sets WHERE Name = \'" + set + "\'");

            if (dataTable.Rows.Count == 0) return false;
            return true;
        }
        public static bool CheckIfSetExistsByAbbreviation(string setAbbr)
        {
            var dataTable = RunQuery("SELECT * FROM Sets WHERE Abbreviation = \'" + setAbbr + "\'");

            if (dataTable.Rows.Count == 0) return false;
            return true;
        }
        public static bool IsSetLocked(string set)
        {
            var dataTable = RunQuery("SELECT Locked FROM Sets WHERE Name = \'" + set + "\'");

            bool data = (bool)dataTable.Rows[0].ItemArray[0];

            return data;
        }
        public static void LockSet(string set)
        {
            var query = "UPDATE Sets SET Locked = 1 WHERE Name = \'" + set + "\'";
            RunQuery(query);
        }
        public static void UnlockSet(string set)
        {
            var query = "UPDATE Sets SET Locked = 0 WHERE Name = \'" + set + "\'";
            RunQuery(query);
        }
        public static void UnlockAllSets()
        {
            var query = "UPDATE Sets SET Locked = 0";
            RunQuery(query);
        }

        //Create Functions
        public static void AddNewCard(string type, Dictionary<string, object> values)
        {
            var query = "INSERT INTO ";

            if (type.ToLower() == "mtg")
                query += "MtG ";
            else if (type.ToLower() == "non_mtg")
                query += "Non_Mtg ";

            RunQueryWithArgs(query, values);
        }
        public static void AddNewSet(string name, string abbreviation, Image symbol, string type)
        {
            var query = "INSERT INTO Sets ";

            //TODO: Use the CSS thing instead?
            byte[] symbolData = new byte[0];
            if (symbol != null)
            {
                MemoryStream ms = new MemoryStream();
                symbol.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                symbolData = ms.ToArray();
            }

            Dictionary<string, object> values = new Dictionary<string, object>()
            {
                { "name", name },
                { "abbreviation", abbreviation },
                { "symbol", symbolData },
                { "locked", false },
                { "type", type }
            };

            RunQueryWithArgs(query, values);
        }
    }
}
