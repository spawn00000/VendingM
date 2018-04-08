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
            
            //VM = vending machine
            vendingMachine VM = new vendingMachine();

            //input
            VM.fill_VM();

            //cicle interactions

            //interaction with VM
            userInteraction UI = new userInteraction();
            UI.interact();

            //calculate change
            List<money> change= VM.giveChange();

            //update vending machine 
            //and give items and change! (update interaction)

            //output results


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
        public List<money> currency { get; set; }

        //for any timeframe desired - for now we leave it as an all-time evidence
        public int VM_itemsSoldCount { get; set; }
        public int VM_moneyGainedCount { get; set; }
        public List<item> VM_itemsSold { get; set; }
        public List<money> VM_moneyGained { get; set; }

        public List<money> giveChange()
        {
            List<money> result = new List<money>();
            return result;
        }

        public void fill_VM()
        {
            //input from a text file (an XML would be better)
            //but for now txt is fine, because the user will input interactions in the same format (no visual elements, e.g. buttons)

            string fileName = "VM_init.txt";
            string[] separators = new string[1];
            separators[0] = ", ";

            using (StreamReader sr = new StreamReader(fileName))
            {
                //while (!sr.EndOfStream)
                //{

                string[] el = sr.ReadLine().Split(separators, StringSplitOptions.None); // StringSplitOptions.None will leave the indexes ok if some data is missing from the file
                //el[1] = title of item fields
                //el[2] = item1
                //el[3] = item2, etc

                string[] name = el[0].Split('\t');

                el = sr.ReadLine().Split(separators, StringSplitOptions.None); // StringSplitOptions.None will leave the indexes ok if some data is missing from the file
                //el[1] = title of money fields
                //el[2] = money1
                //el[3] = money2, etc                   



                //}
                sr.Close();
            }

        }
    }

    public class userInteraction : interaction
    {
        public List<item> UI_itemsRequested { get; set; }
        public List<item> UI_itemsGiven { get; set; } //in case the machine runs out of change - gives less items if no change available (even 0 items)  (TODO - discussion)

        public List<money> UI_moneyRequested { get; set; }
        public List<money> UI_moneyUsed { get; set; }
        public List<money> UI_changeGiven { get; set; }

        public void interact()
        {
        }
    }

    public class money : entity
    {
        //public string type { get; set; } // banknote or coin - if we decide to add type --> the value for banknotes should be multiplied by 100, it is safer to have only one currency - e.g cent or penny (instead of having both cent and dollar)
    }

    public class item : entity
    {
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
