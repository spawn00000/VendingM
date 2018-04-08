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

            //            from README
            //            Visual studio 2010 - console application

            //A vending machine sells items for various prices and can give change. At the start of the day it is loaded with a certain number of coins of various denominations e.g. 100 x 1p, 50 x 5p, 50 x 10p etc. 
            //When an item is requested a certain number of coins are provided. 
            //Write code that models the vending machine and calculates the change to be given when an item is purchased (e.g. 2 x 20p used to purchase an item costing 25p might return 1 x 10p and 1 x 5p).

            //The application - allows any number of items to be purchased. Not just one.

            //Use of the application: run commands like: p UI_interact.txt.
            //The application was tested using the 3 text files provided. You can modify them or create new ones using the same structure as described below. 

            //Input - from text files
            //structure:
            //first line in each file is about the items. Elements of the line are separated by commas. First element shows the fields for the item e.g value or quantity. The rest of the elements are the items (the fields are separated by tabs)
            //second line in each file is about the money. Elements of the line are separated by commas. First element shows the fields for the money e.g value or quantity. The rest of the elements are the money (the fields are separated by tabs)

            //Output - change given and status of money and items in the vending machine for every interaction.


            //about the coding
            //all done in one file (for easy archiving and explanations)
            //input is done through txt files (in the same folder as the executable) chosen by the user from the console.
            //two important classes for modelling the vending machine and the users actions - vendingMachine and userInteraction
            //two classes for objects - items from the vending machine and money
            //the interactions will succeed if items or money are available. if not, user is notified by messages
            //the use of 'ref' was just for debugging the program faster and to show the flow of operations easier in the lines below
            //hope all cases where covered. The 3 text files worked OK, in any succession in the same session. you can enter any number of commands like "p fileName.txt" in the same succession


            Console.WindowHeight = 60; //increase height of the console 
            Console.WindowWidth = 140; //increase height of the console 
            Program p = new Program();
            Console.WriteLine("Welcome to Vending Machine 1.0!");
            Console.WriteLine();

            //VM = vending machine
            vendingMachine VM = new vendingMachine();
            //init VM
            VM.items = new List<item>();
            VM.money = new List<money>();
            VM.VM_moneyGained = new List<money>();
            //other inits?

            //input
            VM.fill_VM(ref VM);
            //display VM initial items
            Console.Write("VM items filled today: ");
            p.display(VM.items);
            //display VM initial money
            Console.Write("VM money filled today: ");
            p.display(VM.money);
            Console.WriteLine();

            Console.WriteLine("Write 'p [fileName]' to purchase items. e.g.   p UI_interact.txt   or   p UI_interact2.txt");
            Console.WriteLine("Write 'q' to leave VM");
            Console.WriteLine();


            //cicle interactions
            int counter = 0;

            while (true)
            {
                string userConsole = Console.ReadLine();
                if (userConsole.Equals("q"))
                {
                    Console.Write("Money that came in VM today: ");
                    p.display(VM.VM_moneyGained);
                    break;
                }
                else if (userConsole.Contains('p'))
                {
                    string fileName = "UI_interact.txt";
                    try
                    {
                        fileName = userConsole.Split(' ')[1];
                        //try the path
                        StreamReader sr = new StreamReader(fileName);
                        sr.Dispose();
                    }
                    catch
                    {
                        Console.WriteLine("We do not have that product. Input error");
                        continue;
                    }

                    counter += 1;
                    Console.WriteLine("Interaction" + counter);

                    //interaction with VM
                    userInteraction UI = new userInteraction();
                    //init UI
                    UI.UI_itemsRequested = new List<item>();
                    UI.UI_moneyUsed = new List<money>();
                    UI.UI_changeGiven = new List<money>();
                    //other inits?

                    UI.interact(ref UI, fileName);
                    //display money used for interaction
                    Console.Write("Money used: ");
                    p.display(UI.UI_moneyUsed);

                    //calculate change
                    //(update vending machine) - items and money
                    //and give items and change! (update interaction)
                    VM.giveChange(ref VM, ref UI);

                    //display change
                    Console.Write("Change received: ");
                    p.display(UI.UI_changeGiven);
                    if (UI.UI_changeGiven.Count == 0)
                    {
                        //cannot give change
                        Console.WriteLine(VM.message);
                    }

                    //display VM money
                    Console.Write("VM money after interaction: ");
                    p.display(VM.money);

                    //display VM items
                    Console.Write("VM items after interaction: ");
                    p.display(VM.items);

                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Please don't hit the vending machine");
                }
                //ending interactions? or continue
            }

            //keep console open.
            Console.WriteLine();
            Console.WriteLine("Press any key to exit. Thank you for using our vending machine");
            Console.ReadKey();
        }


        public void display(List<money> e)
        {
            for (int i = 0; i < e.Count; i++)
            {
                Console.Write(e[i].quantity + "x" + e[i].value + "p");
                if (i == e.Count - 1)
                {
                    Console.Write("\n");
                }
                else
                {
                    Console.Write(", ");
                }
            }
        }

        public void display(List<item> e)
        {
            for (int i = 0; i < e.Count; i++)
            {
                Console.Write(e[i].name + " " + e[i].quantity + "x" + e[i].value + "p");
                if (i == e.Count - 1)
                {
                    Console.Write("\n");
                }
                else
                {
                    Console.Write(", ");
                }
            }
        }

    }

    public class vendingMachine : entity
    {
        public List<item> items { get; set; }
        public List<money> money { get; set; }

        public string message { get; set; }

        //for any timeframe desired - for now we leave it as an all-time evidence -TODO
        public int VM_itemsSoldCount { get; set; } //-TODO
        public int VM_moneyGainedCount { get; set; }//-TODO
        public List<item> VM_itemsSold { get; set; }//-TODO
        public List<money> VM_moneyGained { get; set; } //done    -- this is the money that came in (it does not include what came out)
        public List<money> VM_moneyLostAsChange { get; set; } //-TODO

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


        public void giveChange(ref vendingMachine VM, ref userInteraction UI)
        {
            //see if items are available in stock
            //this should not happen! (you should not ask for an item that is not in VM)
            bool stock = true;
            for (int i = 0; i < UI.UI_itemsRequested.Count; i++)
            {

                for (int j = 0; j < VM.items.Count; j++)
                {
                    if (UI.UI_itemsRequested[i].id.Equals(VM.items[j].id))
                    {
                        if (VM.items[j].quantity <= 0)
                        {
                            stock = false;
                            break;
                        }
                    }
                }
            }
            if (!stock)
            {
                //message for user - what products are not avaiable? maybe at VM 2.0
                VM.message = "One of the products is not on stock! Interaction cancelled. Please take your money back";
                return;
            }

            //calculate the cost pf items
            int cost = 0;
            for (int i = 0; i < UI.UI_itemsRequested.Count; i++)
            {

                int price = getPriceOf(UI.UI_itemsRequested[i].id, VM);
                if (price >= 0)
                    cost += price;
                else
                {
                    //price =-1;
                    //error in file UI - ID not found -error in file, 
                    //message for user
                    VM.message = "One of the products is not in the menu! Interaction cancelled. Please take your money back";
                    return;
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
                //insert more money for the products please - this should be the proper response but since we get data from a txt file, we have to do another interation to read a new file
                VM.message = "You did not insert enough money. Interaction cancelled. Please take your money back";
                return;
            }
            else if (change == 0)
            {
                //end transaction with a message
                VM.message = "Exact sum provided. No change needed.";
                //return;
            }
            else
            {
                //give Change
                //give from higher to lower                

                bool changePossible = true;
                while (changePossible)
                {
                    int max = 0;
                    int indexMax = 0;
                    for (int i = 0; i < VM.money.Count; i++)
                    {
                        //find max - but smaller than change
                        if ((VM.money[i].value > max) && (VM.money[i].quantity > 0) && (VM.money[i].value <= change))
                        {
                            max = VM.money[i].value;
                            indexMax = i;
                        }
                    }
                    change -= max;

                    if (max > 0)
                    { //if we can add more to the change

                        //update VM money
                        VM.money[indexMax].quantity -= 1;

                        //update list UI.changeGiven
                        money m = new money();
                        m.value = max;
                        m.quantity = 1;

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
                    }

                    //while loop condition
                    if ((max == 0) || (change == 0))
                    {
                        //either we completed all the change or we cannot give change so we abort
                        changePossible = false;
                    }
                }

            }
            if (change == 0)
            {
                //mission complete 
                //update items in VM  
                //update givenitems in UI 

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
                        VM.money.Add(m);
                    }
                }


                //bonus - statistics for VM
                //do the same for the total gains stored in VM_moneyGained
                for (int j = 0; j < UI.UI_moneyUsed.Count; j++)
                {

                    money m = UI.UI_moneyUsed[j];

                    bool alreadyExists = false;
                    for (int i = 0; i < VM.VM_moneyGained.Count; i++)
                    {
                        if (m.value.Equals(VM.VM_moneyGained[i].value))
                        {
                            alreadyExists = true;
                            VM.VM_moneyGained[i].quantity += m.quantity;
                            break;
                        }
                    }

                    if (!alreadyExists)
                    {
                        VM.VM_moneyGained.Add(m);
                    }
                }


            }
            else
            {
                //no change to give!!!

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

                //create message for user
                VM.message = "No change to give! Interaction cancelled. Place take your money back.";
            }



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
                //read items from file
                string[] el = sr.ReadLine().Split(separators, StringSplitOptions.None); // StringSplitOptions.None will leave the indexes ok if some data is missing from the file
                //el[0] = title of item fields: id  name	category	value	quantity
                //el[1] = item1
                //el[2] = item2, etc

                //add items to VM
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

                //read money from file
                el = sr.ReadLine().Split(separators, StringSplitOptions.None); // StringSplitOptions.None will leave the indexes ok if some data is missing from the file
                //el[0] = title of money fields: value	quantity
                //el[1] = money1
                //el[2] = money2, etc                   

                //add money to VM
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

                sr.Close();
            }

        }

    }

    public class userInteraction : interaction
    {
        public List<item> UI_itemsRequested { get; set; }
        public List<money> UI_moneyUsed { get; set; }

        public List<money> UI_changeGiven { get; set; }

        public void interact(ref userInteraction UI, string fileName)
        {
            //input from a text file

            //string fileName = "UI_interact.txt";
            string[] separators = new string[1];
            separators[0] = ", ";

            using (StreamReader sr = new StreamReader(fileName))
            {
                //read items from file
                string[] el = sr.ReadLine().Split(separators, StringSplitOptions.None); // StringSplitOptions.None will leave the indexes ok if some data is missing from the file
                //el[0] = title of item fields: id
                //el[1] = item1
                //el[2] = item2, etc

                //add items to requested list
                for (int i = 1; i < el.Length; i++)
                {
                    string[] elements = el[i].Split('\t');

                    string id = elements[0];

                    item z = new item();
                    z.id = Convert.ToInt32(id);
                    z.quantity = 1; // he can take only one fron an id (if he wants two of the same type, the user makes another interaction)

                    //we do not fill properties from vending machine! because the user may not know all of them (we just fill property ID - simulating the press of a button)

                    UI.UI_itemsRequested.Add(z);
                }

                //read money from file
                el = sr.ReadLine().Split(separators, StringSplitOptions.None); // StringSplitOptions.None will leave the indexes ok if some data is missing from the file
                //el[0] = title of money fields: value	quantity
                //el[1] = money1
                //el[2] = money2, etc                   

                //add money to list
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
