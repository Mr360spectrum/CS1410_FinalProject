namespace lib;
using System.Collections.Generic;

public class Player
{
    private string name;
    public string Name
    {
        get
        {
            if (this.name == "" || this.name is null)
            {
                throw new EmptyNameException("'name' returned null or an empty string.");
            }
            return this.name;
        }
        set 
        {
            if (value == "" || value is null)
            {
                throw new EmptyNameException("Player name cannot be set as null or an empty string.");
            }
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
}