# VendingM

Visual studio 2010 - console application

A vending machine sells items for various prices and can give change. At the start of the day it is loaded with a certain number of coins of various denominations e.g. 100 x 1p, 50 x 5p, 50 x 10p etc. 
When an item is requested a certain number of coins are provided. 
Write code that models the vending machine and calculates the change to be given when an item is purchased (e.g. 2 x 20p used to purchase an item costing 25p might return 1 x 10p and 1 x 5p).

The application - allows any number of items to be purchased. Not just one.

Use of the application: run commands like: p UI_interact.txt.
The application was tested using the 3 text files provided. You can modify them or create new ones using the same structure as described below. 

Input - from text files
structure:
first line in each file is about the items. Elements of the line are separated by commas. First element shows the fields for the item e.g value or quantity. The rest of the elements are the items (the fields are separated by tabs)
second line in each file is about the money. Elements of the line are separated by commas. First element shows the fields for the money e.g value or quantity. The rest of the elements are the money (the fields are separated by tabs)

Output - change given and status of money and items in the vending machine for every interaction.