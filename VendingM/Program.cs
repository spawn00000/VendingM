using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VendingM
{
    class Program
    {
        static void Main(string[] args)
        {

            //Program p = new Program();

            //task
            //A vending machine sells items for various prices and can give change. At the start of the day it is loaded with a certain number of coins of various denominations e.g. 100 x 1p, 50 x 5p, 50 x 10p etc. 
            //When an item is requested a certain number of coins are provided. 
            //Write code that models the vending machine and calculates the change to be given when an item is purchased (e.g. 2 x 20p used to purchase an item costing 25p might return 1 x 10p and 1 x 5p).
            
            //the use of 'ref' was just for debugging the program faster and to show the flow of operations easier in the lines below

            //VM = vending machine
            vendingMachine VM = new vendingMachine();
            //init VM
            VM.items = new List<item>();
            VM.money = new List<money>();
            //other inits?

            //input
            VM.fill_VM(ref VM);

            //cicle interactions

            //interaction with VM
            userInteraction UI = new userInteraction();
            //init UI
            UI.UI_itemsRequested = new List<item>();
            UI.UI_moneyUsed = new List<money>();
            UI.UI_changeGiven = new List<money>();
            //other inits?

            UI.interact(ref UI);

            

            //calculate change
            //(update vending machine) - items and money
            //and give items and change! (update interaction)
            List<money> change= VM.giveChange(ref VM, ref UI);


            

            //ending interactions? or continue


            //keep console open.
            Console.WriteLine();
            Console.WriteLine("Press any key to exit. Thank you for using our vending machine");
            Console.ReadKey();
        }
 
    }

    public class vendingMachine : entity
    {
        public List<item> items { get; set; }
        public List<money> money { get; set; }

        //for any timeframe desired - for now we leave it as an all-time evidence
        public int VM_itemsSoldCount { get; set; }
        public int VM_moneyGainedCount { get; set; }
        public List<item> VM_itemsSold { get; set; }
        public List<money> VM_moneyGained { get; set; }

        public int getPriceOf(int itemID, vendingMachine VM)
        {
            int price = -1;
            for (int i = 0; i < VM.items.Count; i++)
            {
                if (itemID.Equals(VM.items[i].id))
                {
                    price = VM.items[i].value;
                    break;
                }
            }
            return price;
        }

        
        public List<money> giveChange(ref vendingMachine VM, ref userInteraction UI)
        {
            List<money> result = new List<money>();

            //calculate the cost pf items
            int cost = 0;
            for (int i = 0; i < UI.UI_itemsRequested.Count; i++)
            {

                int price = getPriceOf(UI.UI_itemsRequested[i].id, VM);
                if (price >= 0)
                    cost += price;
                else
                {
                    //treat error TODO
                    //error in file UI - ID not found -error in file, or the item with that ID is no longer in stock

                    //we have to remove the item form the list of items of the interaction
                    //compose message for user
                }
            }

            //calculate the change
            int moneyUsed = 0;
            for (int i = 0; i < UI.UI_moneyUsed.Count; i++)
            {
                moneyUsed += UI.UI_moneyUsed[i].value * UI.UI_moneyUsed[i].quantity;
            }
            int change = moneyUsed - cost;

            if (change < 0)
            {
                //more money for the products please
            }
            else if (change == 0)
            {
                //end transaction with a message
            }
            else
            {
                //give Change
                //give from higher to lower                

                bool changePossible = true;
                while (changePossible)
                {
                    int max = 0;
                    int indexMax=0;
                    for (int i = 0; i < VM.money.Count; i++)
                    {
                        //find max - but smaller than change
                        if ((VM.money[i].value > max) && (VM.money[i].quantity > 0) && (VM.money[i].value <= change))
                        {
                            max = VM.money[i].value; 
                            indexMax=i;
                        }
                    }
                    change -= max;

                    //update VM money
                    VM.money[indexMax].quantity -= 1;

                    //update list UI.changeGiven
                    money m = new money();
                    m.value=max;
                    m.quantity=1;

                    bool alreadyExists = false;
                    for (int i = 0; i < UI.UI_changeGiven.Count; i++)
                    {
                        if (m.value.Equals(UI.UI_changeGiven[i].value))
                        {
                            alreadyExists = true;
                            UI.UI_changeGiven[i].quantity += m.quantity;
                            break;
                        }
                    }

                    if (!alreadyExists)
                    {
                        UI.UI_changeGiven.Add(m);
                    }


                    //while loop condition
                    if ((max == 0) || (change ==0))
                    {
                        //either we completed all the change or we cannot give change so we abort
                        changePossible = false;
                    }
                }

                if (change == 0)
                {
                    //mission complete 
                    //update items in VM  -TODO
                    //update givenitems in UI -TODO

                    for (int i = 0; i < UI.UI_itemsRequested.Count; i++)
                    {
                        for (int j = 0; j < VM.items.Count; j++)
                        {
                            if (UI.UI_itemsRequested[i].id.Equals(VM.items[j].id))
                            {
                                VM.items[j].quantity -= UI.UI_itemsRequested[i].quantity;
                            }
                        }
                    }

                    //update money in VM (with money given by the user) - only if the transaction is finalized (the change could be given) - we do not simulate the money going in VM and exiting VM if the change cannot be given

                    for (int j = 0; j < UI.UI_moneyUsed.Count; j++)
                    {

                        money m = UI.UI_moneyUsed[j];

                        bool alreadyExists = false;
                        for (int i = 0; i < VM.money.Count; i++)
                        {
                            if (m.value.Equals(VM.money[i].value))
                            {
                                alreadyExists = true;
                                VM.money[i].quantity += m.quantity;
                                break;
                            }
                        }

                        if (!alreadyExists)
                        {
                            UI.UI_changeGiven.Add(m);
                        }
                    }

 
                }
                else
                {
                    //no change!!!

                    //update VM money using UI.changeGiven
                    //empty UI.changeGiven

                    for (int i = 0; i < UI.UI_changeGiven.Count; i++)
                    {
                        for (int j = 0; j < VM.money.Count; j++)
                        {
                            if (UI.UI_changeGiven[i].value.Equals(VM.money[j].value))
                            {
                                VM.money[j].quantity += UI.UI_changeGiven[i].quantity;
                            }
                        }
                    }

                    UI.UI_changeGiven.Clear();

                    //create message for user -TODO
                }



            }





            return result;
        }

        public void fill_VM(ref vendingMachine VM)
        {
            //input from a text file (an XML would be better)
            //but for now txt is fine, because the user will input interactions in the same format (no visual elements, e.g. buttons)

            

            string fileName = "VM_fill.txt";
            string[] separators = new string[1];
            separators[0] = ", ";

            using (StreamReader sr = new StreamReader(fileName))
            {
                //while (!sr.EndOfStream)
                //{

                string[] el = sr.ReadLine().Split(separators, StringSplitOptions.None); // StringSplitOptions.None will leave the indexes ok if some data is missing from the file
                //el[0] = title of item fields: id  name	category	value	quantity
                //el[1] = item1
                //el[2] = item2, etc

                for (int i = 1; i < el.Length; i++)
                {
                    string[] elements = el[i].Split('\t');

                    string id = elements[0];
                    string name = elements[1];
                    string category = elements[2];
                    string value = elements[3];
                    string quantity = elements[4];

                    item z = new item();
                    z.id = Convert.ToInt32(id);
                    z.name = name;
                    z.category = category;
                    z.value = Convert.ToInt32(value);
                    z.quantity = Convert.ToInt32(quantity);

                    VM.items.Add(z);
                }
                

                el = sr.ReadLine().Split(separators, StringSplitOptions.None); // StringSplitOptions.None will leave the indexes ok if some data is missing from the file
                //el[0] = title of money fields: value	quantity
                //el[1] = money1
                //el[2] = money2, etc                   


                for (int i = 1; i < el.Length; i++)
                {
                    string[] elements = el[i].Split('\t');

                    string value = elements[0];
                    string quantity = elements[1];

                    money z = new money();
                    z.value = Convert.ToInt32(value);
                    z.quantity = Convert.ToInt32(quantity);

                    VM.money.Add(z);
                }


                //}
                sr.Close();
            }

        }

    }

    public class userInteraction : interaction
    {
        public List<item> UI_itemsRequested { get; set; }
        public List<money> UI_moneyUsed { get; set; }

        public List<money> UI_moneyRequested { get; set; }
        public List<item> UI_itemsGiven { get; set; } //in case the machine runs out of change - gives less items if no change available (even 0 items)  (TODO - discussion)
        public List<money> UI_changeGiven { get; set; }

        public void interact(ref userInteraction UI)
        {
            //input from a text file for now

            string fileName = "UI_interact.txt";
            string[] separators = new string[1];
            separators[0] = ", ";

            using (StreamReader sr = new StreamReader(fileName))
            {
                //while (!sr.EndOfStream)
                //{

                string[] el = sr.ReadLine().Split(separators, StringSplitOptions.None); // StringSplitOptions.None will leave the indexes ok if some data is missing from the file
                //el[0] = title of item fields: id
                //el[1] = item1
                //el[2] = item2, etc

                for (int i = 1; i < el.Length; i++)
                {
                    string[] elements = el[i].Split('\t');

                    string id = elements[0];

                    item z = new item();
                    z.id = Convert.ToInt32(id);

                    //we do not fill properties from vending machine! because the user may not know all of them (we just fill property ID - simulating the press of a button)

                    UI.UI_itemsRequested.Add(z);
                }


                el = sr.ReadLine().Split(separators, StringSplitOptions.None); // StringSplitOptions.None will leave the indexes ok if some data is missing from the file
                //el[0] = title of money fields: value	quantity
                //el[1] = money1
                //el[2] = money2, etc                   


                for (int i = 1; i < el.Length; i++)
                {
                    string[] elements = el[i].Split('\t');

                    string value = elements[0];
                    string quantity = elements[1];

                    money z = new money();
                    z.value = Convert.ToInt32(value);
                    z.quantity = Convert.ToInt32(quantity);

                    UI.UI_moneyUsed.Add(z);
                }


                //}
                sr.Close();

            }
        }
    }

    public class money : entity
    {
        //public string type { get; set; } // banknote or coin - if we decide to add type --> the value for banknotes should be multiplied by 100, it is safer to have only one currency - e.g cent or penny (instead of having both cent and dollar)
    }

    public class item : entity
    {
        public int id { get; set; } //product id
        public string name { get; set; } //product name, label
        public string category { get; set; } //chips, soda, energy bars, etc
    }

    public abstract class entity
    {
        public int value { get; set; }
        public int quantity { get; set; }
    }

    public abstract class interaction
    {
        //maybe fill_VM can be an interaction (TODO)
    }
}
