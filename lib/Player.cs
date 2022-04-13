namespace lib;
using System.Collections.Generic;

public class Player : Entity
{
    private string name;
    public string Name
    {
        get => name;
        set
        {
            if (value == "" || value is null)
            {
                throw new EmptyNameException("Player name cannot be set as null or an empty string.");
            }
            this.name = value;
        }
    }

    private List<Item> inventory = new List<Item>();
    [System.Text.Json.Serialization.JsonIgnore] //Do not serialize Inventory directly from player
    public List<Item> Inventory
    {
        get => inventory;
        set => inventory = value;
    }

    public Weapon? EquippedWeapon { get; protected set; }
    public Armor? EquippedArmor { get; protected set; }

    public Player(string inName)
    {
        this.name = inName;
        this.inventory = new List<Item>();
        Health = 100;
        Defense = 0;
    }
    public Player(string inName, List<Item> inInventory)
    {
        this.name = inName;
        inventory = inInventory;
        Health = 100;
        Defense = 0;
    }
    //* Only for JSON deserialization
    public Player()
    {
        //Reset health to 100 if 0 or not set
        if (Health == 0)
        {
            Health = 100;
        }
    }

    public void Attack(Enemy inEnemy)
    {
        if (EquippedWeapon != null)
        {
            inEnemy.TakeDamage(EquippedWeapon.DamageModifier);
        }
        else
        {
            Console.WriteLine("There is no weapon equipped!");
            Console.WriteLine("Hand-to-hand combat is fine, I guess.");
            inEnemy.TakeDamage(1);
        }
    }

    public override void TakeDamage(double amount)
    {
        if (EquippedWeapon != null)
        {
            var rand = new Random();
            var randNum = rand.Next(101);
            if (randNum < this.EquippedArmor.DodgeChance)
            {
                return;
            }
            else
            {
                base.TakeDamage(amount);
            }
        }
        else
        {
            base.TakeDamage(amount);
        }
    }

    public void EquipWeapon(Weapon inWeapon)
    {
        this.EquippedWeapon = inWeapon;
    }

    public void EquipArmor(Armor inArmor)
    {
        this.EquippedArmor = inArmor;
        this.Defense = EquippedArmor.DefenseModifier;

    }

    public void ShowInventory()
    {
        while (true)
        {
            //!REMOVE
            Console.Clear();
            if (this.inventory.Count == 0)
            {
                Console.WriteLine("The inventory is empty.");
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                return;
            }

            int i = 0;
            Console.WriteLine("");
            foreach (var item in this.inventory)
            {
                Console.WriteLine((i + 1) + ". " + item.Name);
                i++;
            }

            Console.WriteLine("Enter an empty string to exit.");
            Console.WriteLine("Select which item you'd like to view options for: ");
            string input = Console.ReadLine();

            if (input == "")
            {
                //!REMOVE
                return;
            }
            int selection = int.Parse(input) - 1;

            var selectedItem = inventory[selection];
            Console.Clear();
            if (selectedItem.Type == Item.ItemType.Crafting)
            {
                Console.WriteLine("There are no options available.");
            }
            else if (selectedItem.Type == Item.ItemType.Armor)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Rename");
                var option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        Console.WriteLine("What would you like to rename the item to?");
                        (selectedItem as Armor).Rename(Console.ReadLine());
                        break;
                }
            }
            else if (selectedItem.Type == Item.ItemType.Weapon)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Rename");
                Console.WriteLine("2. Equip");
                var option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        Console.WriteLine("What would you like to rename the item to?");
                        (selectedItem as Weapon).Rename(Console.ReadLine());
                        break;
                    case "2":
                        EquipWeapon(selectedItem as Weapon);
                        Console.WriteLine($"{selectedItem.Name} equipped.");
                        break;
                }
            }

            Console.WriteLine("Press a key to continue.");
            Console.ReadKey();
        }
    }

    public void GainItem(Item item)
    {
        this.inventory.Add(item);
    }
}
