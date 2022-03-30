namespace lib;
using System.Collections.Generic;

public class Player
{
    private string name;
    public string Name
    {
        get => this.name;
        set 
        {
            if (value == "" || value is null)
            {
                throw new EmptyNameException("Player name cannot be set as null or an empty string.");
            }
            this.name = value;
        }
    }

    private List<Item> inventory;

    public Player(string inName)
    {
        this.name = inName;
        this.inventory = new List<Item>();
    }
    public Player(string inName, List<Item> inInventory)
    {
        this.name = inName;
        inventory = inInventory;
    }

    public void ShowInventory()
    {
        //!REMOVE
        if (this.inventory.Count == 0)
        {
            Console.WriteLine("The inventory is empty.");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            System.Environment.Exit(0);
        }

        int i = 0;
        Console.WriteLine("");
        foreach (var item in this.inventory)
        {
            Console.WriteLine((i+1) + ". " + item.Name);
            i++;
        }

        Console.WriteLine("Enter an empty string to exit.");
        Console.WriteLine("Select which item you'd like to rename: ");
        string input = Console.ReadLine();

        if (input == "")
        {
            System.Environment.Exit(0);
        }
        int selection = int.Parse(input) - 1;

        try
        {
            var renameableItem = inventory[selection] as IRenameable;
            Console.WriteLine("What would you like to rename the item to?");
            string rename = Console.ReadLine();
            renameableItem.Rename(rename);
        }
        catch
        {
            Console.ForegroundColor = GameHelper.highlightColor;
            Console.WriteLine("That item cannot be renamed!");
            Console.ForegroundColor = GameHelper.defaultColor;
        }
    }
}
