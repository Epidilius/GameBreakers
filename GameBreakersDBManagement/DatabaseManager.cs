﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GameBreakersDBManagement
{
    class DatabaseManager
    {
        string ConnectionString;
        Logger logger;
        static DatabaseManager dbMan = null;
        
        public DatabaseManager()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["GameBreakers"].ConnectionString;
            logger = Logger.GetLogger();
        }
        public static DatabaseManager GetInstace()
        {
            if(dbMan == null)
            {
                dbMan = new DatabaseManager();
            }

            return dbMan;
        }

        //Utility Functions
        SqlCommand CreateCommand(string query)
        {
            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand command = new SqlCommand();
            command.Connection = con;
            command.CommandType = CommandType.Text;
            command.CommandText = query;

            return command;
        }
        SqlDataAdapter CreateDataAdapter(SqlCommand command)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            return adapter;
        }
        DataTable CreateDataTable(SqlDataAdapter dataAdapter)
        {
            DataTable table = new DataTable();
            dataAdapter.Fill(table);

            return table;
        }
        DataTable RunQuery(string query)   //TODO: Rename this
        {
            SqlCommand command = CreateCommand(query);
            SqlDataAdapter adapter = CreateDataAdapter(command);
            DataTable table = CreateDataTable(adapter);

            return table;
        }

        //Get Functions
        public int GetInventory(string card, string set)
        {
            card = card.Replace(@"'", @"''");
            set = set.Replace(@"'", @"''");
            var dataTable = RunQuery("SELECT INVENTORY FROM MtG WHERE NAME = \'" + card + "\' AND EXPANSION = \'" + set + "\'");
            return (int)dataTable.Rows[0]["inventory"];
        }
        public int GetFoilInventory(string card, string set)
        {
            card = card.Replace(@"'", @"''");
            set = set.Replace(@"'", @"''");
            var dataTable = RunQuery("SELECT FOILINVENTORY FROM MtG WHERE NAME = \'" + card + "\' AND EXPANSION = \'" + set + "\'");            
            return (int)dataTable.Rows[0]["foilInventory"];
        }
        public int GetMultiverseID(string card, string set)
        {
            card = card.Replace(@"'", @"''");
            set = set.Replace(@"'", @"''");
            var dataTable = RunQuery("SELECT MULTIVERSEID FROM MtG WHERE NAME = \'" + card + "\' AND EXPANSION = \'" + set + "\'");
            var id = dataTable.Rows[0]["multiverseID"].ToString();
            return Int32.Parse(id);
        }
        public byte[] GetImageForCard(string card, string set)
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
        public DataTable GetAllSets()
        {
            var dataTable = RunQuery("SELECT * FROM Sets");
            return dataTable;
        }
        public DataTable GetAllCardsForSet(string set)
        {
            set = set.Replace(@"'", @"''");
            var dataTable = RunQuery("SELECT * FROM MtG WHERE EXPANSION = \'" + set + "\'");
            return dataTable;
        }
        public DataTable GetCard(string name)
        {
            name = name.Replace(@"'", @"''");
            var dataTable = RunQuery("SELECT * FROM MtG WHERE NAME = \'" + name + "\'");
            return dataTable;
        }

        //TODO: Change these to use my utility funcs
        //Modify Functions
        public void AddOneToInventory(string name, string set, bool foil)
        {
            name = name.Replace(@"'", @"''");
            set = set.Replace(@"'", @"''");
            var cmdString = "UPDATE MtG ";
            if (foil)
            {
                cmdString += "SET foilInventory = foilInventory + 1 ";
            }
            else
            {
                cmdString += "SET Inventory = Inventory + 1 ";
            }
            cmdString += "WHERE NAME = \'" + name + "\' AND EXPANSION = \'" + set + "\'";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = cmdString;
                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        logger.LogError("Failed to increase card: " + name + " from set: " + set + " by 1" + "\r\n" + e);
                    }
                }
            }
        }
        public void RemoveOneToInventory(string name, string set, bool foil)
        {
            name = name.Replace(@"'", @"''");
            set = set.Replace(@"'", @"''");
            var cmdString = "UPDATE MtG ";
            if (foil)
            {
                cmdString += "SET foilInventory = foilInventory - 1 ";
            }
            else
            {
                cmdString += "SET Inventory = Inventory - 1 ";
            }
            cmdString += "WHERE NAME = \'" + name + "\' AND EXPANSION = \'" + set + "\'";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = cmdString;
                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        logger.LogError("Failed to decrease card: " + name + " from set: " + set + " by 1" + "\r\n" + e);
                    }
                }
            }
        }
        public void UpdateInventory(string name, string set, int newAmount, int newFoilAmount)
        {
            name = name.Replace(@"'", @"''");
            set = set.Replace(@"'", @"''");
            var cmdString = "UPDATE MtG SET foilInventory = " + newFoilAmount + ", inventory = " + newAmount;

            cmdString += " WHERE NAME = \'" + name + "\' AND EXPANSION = \'" + set + "\'";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = cmdString;
                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        logger.LogError("Failed to update inventory of card: " + name + " from set: " + set + " to: " + newAmount + " and foils to: " + newFoilAmount + "\r\n" + e);
                    }
                }
            }
        }
        public void UpdatePrice(string name, string set, float price, bool foil)
        {
            name = name.Replace(@"'", @"''");
            set = set.Replace(@"'", @"''");

            var cmdString = "";
            if (foil)
                cmdString = "UPDATE MtG SET foilPrice = @price WHERE name = \'" + name + "\' AND EXPANSION = \'" + set + "\'";
            else
                cmdString = "UPDATE MtG SET price = @price, priceLastUpdated = @time WHERE name = \'" + name + "\' AND EXPANSION = \'" + set + "\'";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = cmdString;

                    comm.Parameters.AddWithValue("@price", price);
                    comm.Parameters.AddWithValue("@time", DateTimeOffset.Now.ToUnixTimeMilliseconds());

                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        logger.LogError("Failed to update price of (foil: " + foil + ") card: " + name + " from set: " + set + " to: "+ price + "\r\n" + e);
                    }
                }
            }
        }


        //Check/Toggle Functions
        public bool CheckIfCardExists(string multiverseID)
        {
            var dataTable = RunQuery("SELECT * FROM MtG WHERE MULTIVERSEID = \'" + multiverseID + "\'");
            
            if(dataTable.Rows.Count == 0) return false;
            return true;
        }
        public bool CheckIfSetExists(string set)
        {
            var dataTable = RunQuery("SELECT * FROM Sets WHERE Name = \'" + set + "\'");

            if (dataTable.Rows.Count == 0) return false;
            return true;
        }
        public bool IsSetLocked(string set)
        {
            var dataTable = RunQuery("SELECT Locked FROM Sets WHERE Name = \'" + set + "\'");

            bool data = (bool)dataTable.Rows[0].ItemArray[0];

            return data;
        }
        public void LockSet(string set)
        {
            var cmdString = "UPDATE Sets SET Locked = @lock WHERE Name = \'" + set + "\'";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = cmdString;

                    comm.Parameters.AddWithValue("@lock", true);

                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        logger.LogError("Failed to lock set: " + set + "\r\n" + e);
                    }
                }
            }
        }
        public void UnlockSet(string set)
        {
            var cmdString = "UPDATE Sets SET Locked = @lock WHERE Name = \'" + set + "\'";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = cmdString;

                    comm.Parameters.AddWithValue("@lock", false);

                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        logger.LogError("Failed to unlock set: " + set + "\r\n" + e);
                    }
                }
            }
        }

        //Create Functions
        public void AddNewCard(string layout, string cardID, string name, string manaCost, int cmc, string colours, string rarity, string type, string types, string subtypes, string text, string flavourText, string power, string toughness, string imageName, string colourIdentity, string multiverseID, string set, float price, int inventory, float foilPrice, int foilInventory, Image image)
        {
            var cmdString = "INSERT INTO MtG (layout, cardID, name, manaCost, cmc, colours, rarity, type, types, subtypes, text, flavourText, power, toughness, imageName, colourIdentity, multiverseID, expansion, price, inventory, foilPrice, foilInventory, priceLastUpdated, image) VALUES (@val0, @val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8, @val9, @val10, @val11, @val12, @val13, @val14, @val15, @val16, @val17, @val18, @val19, @val20, @val21, @val22, @val23)";
            var imageData = new byte[0];
            if (image != null)
            {
                MemoryStream ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                imageData = ms.ToArray();
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = cmdString;

                    comm.Parameters.AddWithValue("@val0", layout);
                    comm.Parameters.AddWithValue("@val1", cardID);
                    comm.Parameters.AddWithValue("@val2", name);
                    comm.Parameters.AddWithValue("@val3", manaCost);
                    comm.Parameters.AddWithValue("@val4", cmc);
                    comm.Parameters.AddWithValue("@val5", colours);
                    comm.Parameters.AddWithValue("@val6", rarity);
                    comm.Parameters.AddWithValue("@val7", type);
                    comm.Parameters.AddWithValue("@val8", types);
                    comm.Parameters.AddWithValue("@val9", subtypes);
                    comm.Parameters.AddWithValue("@val10", text);
                    comm.Parameters.AddWithValue("@val11", flavourText);
                    comm.Parameters.AddWithValue("@val12", power);
                    comm.Parameters.AddWithValue("@val13", toughness);
                    comm.Parameters.AddWithValue("@val14", imageName);
                    comm.Parameters.AddWithValue("@val15", colourIdentity);
                    comm.Parameters.AddWithValue("@val16", multiverseID);
                    comm.Parameters.AddWithValue("@val17", set);
                    comm.Parameters.AddWithValue("@val18", price);
                    comm.Parameters.AddWithValue("@val19", inventory);
                    comm.Parameters.AddWithValue("@val20", foilPrice);
                    comm.Parameters.AddWithValue("@val21", foilInventory);
                    comm.Parameters.AddWithValue("@val22", DateTimeOffset.Now.ToUnixTimeMilliseconds());
                    comm.Parameters.AddWithValue("@val23", imageData);

                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        logger.LogError("Failed to add new card: " + name + " to set: " + set + "\r\n" + e);
                    }
                }
            }
        }
        public void AddNewSet(string set, string abbreviation, Image symbol)
        {
            var cmdString = "INSERT INTO Sets (name, abbreviation, symbol, locked) VALUES (@val1, @val2, @val3, @val4)";

            //TODO: Use the CSS thing instead?
            byte[] symbolData = new byte[0];
            if (symbol != null)
            {
                MemoryStream ms = new MemoryStream();
                symbol.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                symbolData = ms.ToArray();
            }
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = cmdString;
                    
                    comm.Parameters.AddWithValue("@val1", set);
                    comm.Parameters.AddWithValue("@val2", abbreviation);
                    comm.Parameters.AddWithValue("@val3", symbolData);
                    comm.Parameters.AddWithValue("@val4", false);

                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        logger.LogError("Failed to add new set: " + set + "\r\n" + e);
                    }
                }
            }
        }
    }
}
