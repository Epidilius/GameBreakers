using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBreakersDBManagement
{
    public static class CartManager
    {
        static List<Cart> OpenCarts = new List<Cart>();

        public static void CreateCart()
        {
            var query = "INSERT INTO ActiveCarts ";

            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "CustomerName", "GameBreakers" }
            };

            DatabaseManager.RunQueryWithArgs(query, values);
        }

        public static void DeleteCart(int cartID)
        {
            try
            {
                var query = "DELETE FROM ActiveCarts WHERE ID = " + cartID;
                DatabaseManager.RunQuery(query);
            }
            catch(Exception ex)
            {
                Logger.LogError("Attempted to delete card", ex.Message, cartID.ToString());
            }          
        }

        public static void DisplayCart(int cartID)
        {
            for(int i = 0; i < OpenCarts.Count; i++)
            {
                if(OpenCarts[i].GetID() == cartID)
                {
                    OpenCarts[i].Show();
                    return;
                }
            }

            Cart cart = new Cart(cartID);
            OpenCarts.Add(cart);
            cart.Show();
        }

        public static void AddItemToCart(int cartID, string itemName, string itemExpansion, int itemAmount)
        {
            //TODO: How do I want to do this? Take in a dictionary of the items I list? Maybe just the ID? I'll have to condense all cards into a single card table

            var fetchQuery = "Select CardNames, CardExpansions, CardAmounts FROM ActiveCarts WHERE ID = '" + cartID + "'";
            var fetchResults = DatabaseManager.RunQuery(fetchQuery);
            var row = fetchResults.Rows[0];

            var nameString = row[0].ToString();
            var expansionString = row[1].ToString();
            var amountString = row[2].ToString();

            var nameSplit      = nameString.Split('|');
            var expansionSplit = expansionString.Split('|');
            var splitAmount    = amountString.Split('|');

            var skipCheck  = false;
            var foundMatch = false;
            if(nameSplit.Count() != expansionSplit.Count())
            {
                Logger.LogError("Searching for duplicates in cart: " + cartID, "The name field and expansion field have different amounts of entries.", "");
                skipCheck = true;
            }

            if (!skipCheck)
            {
                for (int i = 0; i < nameSplit.Count(); i++)
                {
                    if (nameSplit[i] == itemName)
                    {
                        if (expansionSplit[i] == itemExpansion)
                        {
                            var currentAmount = Convert.ToInt32(splitAmount[i]);
                            currentAmount += itemAmount;
                            splitAmount[i] = currentAmount.ToString();

                            foundMatch = true;
                            break;
                        }
                    }
                }
            }
            if(foundMatch)
            {
                amountString = "";

                for (int j = 0; j < splitAmount.Count(); j++)
                {
                    if (j != 0)
                        amountString += "|";
                    amountString += splitAmount[j];
                }
            }
            else
            {
                nameString += "|" + itemName;
                expansionString += "|" + itemExpansion;
                amountString += "|" + itemAmount;
            }

            var query = "UPDATE ActiveCarts SET " +
                "CardNames = '" + nameString.Replace("'", "''") +
                "', CardExpansions = '" + expansionString.Replace("'", "''") +
                "', CardAmounts = '" + amountString +
                "' WHERE ID = '" + cartID + "'";

            DatabaseManager.RunQuery(query);

            for (int i = 0; i < OpenCarts.Count; i++)
            {
                if(OpenCarts[i].GetID() == cartID)
                {
                    OpenCarts[i].CartUpdated();
                    return;
                }
            }
        }

        public static void CartClosed(int cartID)
        {
            for(int i = 0; i < OpenCarts.Count; i++)
            {
                if(OpenCarts[i].GetID() == cartID)
                {
                    OpenCarts.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
